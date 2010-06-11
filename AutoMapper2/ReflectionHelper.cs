namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
	using System.Data.Objects.DataClasses;
	using System.Reflection;
	using System.Text;
	#endregion

	internal static class ReflectionHelper {

		internal class ReflectionCacheEntry {
			public Type Type { get; set; }
			public List<PropertyInfo> PropertyInfo { get; set; }
			public List<MethodInfo> MethodInfo { get; set; }
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
			return e.PropertyInfo;
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
			return e.MethodInfo;
		}

		public static string ObjectToString( object obj, bool ExcludeLinqAssociationProperties ) {

			if ( obj == null ) {
				return null;
			}

			Type type = obj.GetType();
			List<PropertyInfo> properties = GetProperties( type );

			StringBuilder sb = new StringBuilder();

			sb.Append( type.Name );
			sb.Append( ": " );
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
						if ( property.PropertyType.Name.StartsWith( "EntityRef" )
						|| property.PropertyType.Name.StartsWith( "EntitySet" )
						|| property.PropertyType.Name.StartsWith( "EntityObject" ) ) {
							continue;
						}
						AssociationAttribute association = (AssociationAttribute)Attribute.GetCustomAttribute( property, typeof(AssociationAttribute) );
						if ( association != null && !string.IsNullOrEmpty( association.ThisKey ) ) {
							continue;
						}
						EdmEntityTypeAttribute entity = (EdmEntityTypeAttribute)Attribute.GetCustomAttribute( property, typeof( EdmEntityTypeAttribute ) );
						if ( entity != null ) {
							continue;
						}
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
