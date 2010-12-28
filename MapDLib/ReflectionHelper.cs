namespace MapDLib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
#if ENTITY_FRAMEWORK
	using System.Data.Objects.DataClasses;
#endif
	using System.Reflection;
	using System.Text;
	#endregion

	internal static class ReflectionHelper {

		internal class ReflectionCacheEntry {
			public Type Type { get; set; }
			public List<PropertyInfo> PropertyInfo { get; set; }
			public List<MethodInfo> MethodInfo { get; set; }
			public List<Attribute> Attributes { get; set; }
			public Dictionary<PropertyInfo, List<Attribute>> PropertyAttributes { get; set; }
		}
		private static Dictionary<Type, ReflectionCacheEntry> Entries { get; set; }

		static ReflectionHelper() {
			Entries = new Dictionary<Type, ReflectionCacheEntry>();
		}

		private static ReflectionCacheEntry FindEntry( Type type ) {
			if ( type == null ) {
				throw new ArgumentNullException( "Can't reflect against type null" );
			}

			if ( !Entries.ContainsKey( type ) ) {
				lock ( Entries ) {
					if ( !Entries.ContainsKey( type ) ) {
						Entries.Add( type, new ReflectionCacheEntry {
							Type = type,
							PropertyInfo = null,
							MethodInfo = null
						} );
					}
				}
			}
			return Entries[type];
		}

		public static List<PropertyInfo> GetProperties( Type type ) {
			ReflectionCacheEntry e = FindEntry( type );
			if ( e.PropertyInfo == null ) {
				lock ( Entries ) {
					if ( e.PropertyInfo == null ) {
						e.PropertyInfo = new List<PropertyInfo>( type.GetProperties() );
					}
				}
			}
			return new List<PropertyInfo>( e.PropertyInfo );
		}

		public static List<MethodInfo> GetMethods( Type type ) {
			ReflectionCacheEntry e = FindEntry( type );
			if ( e.MethodInfo == null ) {
				lock ( Entries ) {
					if ( e.MethodInfo == null ) {
						e.MethodInfo = new List<MethodInfo>( type.GetMethods() );
					}
				}
			}
			return new List<MethodInfo>( e.MethodInfo );
		}

		public static List<Attribute> GetAttributes( Type type ) {
			ReflectionCacheEntry e = FindEntry( type );
			if ( e.Attributes == null ) {
				lock ( Entries ) {
					if ( e.Attributes == null ) {
						object[] attributes = type.GetCustomAttributes(true);
						e.Attributes = HarvestAttributes( attributes );
					}
				}
			}
			return new List<Attribute>( e.Attributes );
		}

		public static Dictionary<PropertyInfo, List<Attribute>> GetPropertyAttributes( Type type ) {
			ReflectionCacheEntry e = FindEntry( type );
			if ( e.PropertyAttributes == null ) {
				lock ( Entries ) {
					if ( e.PropertyAttributes == null ) {
						e.PropertyAttributes = new Dictionary<PropertyInfo, List<Attribute>>();
						List<PropertyInfo> properties = GetProperties( type );
						if ( !properties.IsNullOrEmpty() ) {
							foreach ( PropertyInfo property in properties ) {
								object[] attributes = property.GetCustomAttributes( true );
								List<Attribute> attrList = HarvestAttributes( attributes );
								if ( !attrList.IsNullOrEmpty() ) {
									e.PropertyAttributes.Add( property, attrList );
								}
							}
						}
					}
				}
			}
			return e.PropertyAttributes;
		}

		private static List<Attribute> HarvestAttributes( object[] Source ) {
			List<Attribute> results = new List<Attribute>();
			if ( !Source.IsNullOrEmpty() ) {
				foreach ( object aObj in Source ) {
					Attribute a = aObj as Attribute;
					if ( a != null ) {
						results.Add( a );
					}
				}
			}
			return results;
		}

		public static string ObjectToString( object obj, bool ExcludeLinqAssociationProperties ) {

			if ( obj == null ) {
				return null;
			}

			Type type = obj.GetType();

			// TODO: If it's a value type or has no properties (outside object), use TypeConvert.ConvertToString(  )

			List<PropertyInfo> properties = GetProperties( type );

			StringBuilder sb = new StringBuilder();

			sb.Append( type.Name );
			sb.Append( ": {" ); // Cheer up, reflection isn't that bad.  :P
			if ( properties == null || properties.Count == 0 ) {
				sb.Append( obj );
			} else {

				bool first = true;
				foreach ( PropertyInfo property in properties ) {
					if ( !property.CanRead ) {
						continue;
					}
					if ( property.PropertyType.IsGenericType ) {
						Type genericType = property.PropertyType.GetGenericTypeDefinition();
						Type[] args = property.PropertyType.GetGenericArguments();
						if ( genericType == typeof(Nullable<>) && args.Length == 1 ) {
							// It's Nullable<T> or written better T? so it's fine
						} else {
							// This is a V<T> which may be a List<T> or an EntitySet<T>
							continue;
						}
					}
					if ( ExcludeLinqAssociationProperties ) {
						if ( property.PropertyType.IsLinqProperty() ) {
							continue;
						}
						// LINQ to SQL
						AssociationAttribute association = (AssociationAttribute)property.GetFirstAttribute( typeof(AssociationAttribute) );
						if ( association != null && !string.IsNullOrEmpty( association.ThisKey ) ) {
							continue;
						}
#if ENTITY_FRAMEWORK
						// Entity Framework
						EdmEntityTypeAttribute entity = (EdmEntityTypeAttribute)property.GetFirstAttribute( typeof( EdmEntityTypeAttribute ) );
						if ( entity != null ) {
							continue;
						}
#endif
						// TODO: Determine if this is a value type that's linked to an association and exclude that too
					}

					string valString = null;
					try {
						object val = property.GetValue( obj, null );
						if ( val == null ) {
							valString = "NULL";
						} else {
							valString = val.ToString();
						}
					} catch ( Exception ex ) {
						// Don't error trying to show the object
						valString = "Exception: " + ex.Message;
					}

					if ( !first ) {
						sb.Append( ", " );
					}
					first = false;
					sb.Append( property.Name );
					sb.Append( ": " );
					sb.Append( valString );

				}
			}

			sb.Append( "}" );
			return sb.ToString();
		}

	}

	internal static class ReflectionExtensions {

		public static string ObjectToString( this object obj ) {
			return ReflectionHelper.ObjectToString( obj, true );
		}
		public static string ObjectToString( this object obj, bool ExcludeLinqAssociationProperties ) {
			return ReflectionHelper.ObjectToString( obj, ExcludeLinqAssociationProperties );
		}

	}

}
