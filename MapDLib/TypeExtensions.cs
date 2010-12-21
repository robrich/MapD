namespace MapDLib {

	#region using
	using System;
	using System.Collections.Generic;

	#endregion

	internal static class TypeExtensions {

		public static bool IsClassType( this Type Type ) {
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			if ( Type.IsNullable() ) {
				// Really what we're after is the base type, not the generic type
				return Type.GetGenericBaseType().IsClassType();
			}
			if ( Type == typeof(string) ) {
				return false; // This says it's Type.IsClass == true, but we don't treat it that way
			}
			return Type.IsClass || Type.IsInterface;
			// TODO: Anything else?
		}

		public static bool IsNullable( this Type Type ) {
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			bool results = false;
			if ( Type.IsGenericType ) {
				Type genericType = Type.GetGenericTypeDefinition();
				Type[] args = Type.GetGenericArguments();
				if ( genericType == typeof( Nullable<> ) && args.Length == 1 ) {
					// It's Nullable<T> or written better T?
					results = true;
				} else {
					// This is a V<T> or V<T,U,...>
					results = false;
				}
			}
			return results;
		}

		public static bool IsListOfT( this Type Type ) {
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			bool results = false;
			if ( Type.IsGenericType
				&& typeof( List<> ).IsAssignableFrom( Type.GetGenericTypeDefinition() ) ) {
				results = true;
			}
			return results;
		}

		public static IgnoreDirection GetIgnoreStatus( this Type Type ) {
			IgnoreDirection results = IgnoreDirection.None;
			IgnoreMapAttribute ignore = (IgnoreMapAttribute)Attribute.GetCustomAttribute( Type, typeof( IgnoreMapAttribute ) );
			if ( ignore != null ) {
				results |= ignore.IgnoreDirection;
			}
			return results;
		}

		public static PropertyIs GetIgnorePropertiesIf( this Type Type ) {
			PropertyIs propertyIs = PropertyIs.NotSet;
			IgnorePropertiesIfAttribute ignoreIf = (IgnorePropertiesIfAttribute)Attribute.GetCustomAttribute( Type, typeof( IgnorePropertiesIfAttribute ) );
			if ( ignoreIf != null ) {
				propertyIs |= ignoreIf.PropertyIs;
			}
			return propertyIs;
		}

		public static Type GetMapFromType( this Type Type ) {
			Type results = null;
			MapFromAttribute attr = (MapFromAttribute)Attribute.GetCustomAttribute( Type, typeof( MapFromAttribute ) );
			if ( attr != null ) {
				results = attr.Type;
			}
			MapFromSelfAttribute selfAttr = (MapFromSelfAttribute)Attribute.GetCustomAttribute( Type, typeof( MapFromSelfAttribute ) );
			if ( selfAttr != null ) {
				results = Type;
			}
			return results;
		}

		public static Type GetMapListFromType( this Type Type ) {
			Type results = null;
			MapListFromListOfAttribute attr = (MapListFromListOfAttribute)Attribute.GetCustomAttribute( Type, typeof(MapListFromListOfAttribute) );
			if ( attr != null ) {
				results = attr.Type;
			}
			MapListFromListOfSelfAttribute listOfSelfAttr = (MapListFromListOfSelfAttribute)Attribute.GetCustomAttribute( Type, typeof( MapListFromListOfSelfAttribute ) );
			if ( listOfSelfAttr != null ) {
				results = Type;
			}
			return results;
		}

		public static Type GetGenericBaseType( this Type Type ) {
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			if ( !Type.IsGenericType ) {
				throw new ArgumentOutOfRangeException( "Type", Type.FullName + " isn't Generic" );
			}
			Type genericType = Type.GetGenericTypeDefinition();
			Type[] args = Type.GetGenericArguments();
			if ( args.Length != 1 ) {
				throw new ArgumentOutOfRangeException( "Type", Type.FullName + " isn't List<T>" );
			}
			return args[0];
		}

		public static Type GetListOfType( this Type Type ) {
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			return typeof(List<>).MakeGenericType(
				new Type[] { Type }
			);
		}

		public static Type GetNullableBaseType( this Type Type ) {
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			if ( !Type.IsGenericType ) {
				throw new ArgumentOutOfRangeException( "Type", Type.FullName + " isn't Generic" );
			}
			Type genericType = Type.GetGenericTypeDefinition();
			Type[] args = Type.GetGenericArguments();
			if ( genericType != typeof( Nullable<> ) || args.Length != 1 ) {
				throw new ArgumentOutOfRangeException( "Type", Type.FullName + " isn't Nullable<T>" );
			}
			return args[0];
		}

		public static bool IsLinqProperty( this Type Type ) {
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			string name = Type.Name ?? "";
			return name.StartsWith( "EntityRef" )
			|| name.StartsWith( "EntitySet" )
			|| name.StartsWith( "EntityObject" )
			|| name.StartsWith( "EntityCollection" )
			|| name.StartsWith( "EntityReference" );
		}

		public static bool IsNumeric(this Type Type) {
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			if ( !Type.IsValueType ) {
				return false;
			}
			if ( Type.IsEnum ) {
				return Enum.GetUnderlyingType( Type ).IsNumeric();
			}
			if ( Type.IsNullable() ) {
				return Type.GetGenericBaseType().IsNumeric();
			}
			bool results = false;
			switch ( Type.FullName ) {
				case "System.Byte":
				case "Byte":
				case "byte":
				case "System.SByte":
				case "SByte":
				case "sbyte":
				case "System.Int16":
				case "Int16":
				case "short":
				case "System.UInt16":
				case "UInt16":
				case "ushort":
				case "System.Int32":
				case "Int32":
				case "int":
				case "System.UInt32":
				case "UInt32":
				case "uint":
				case "System.Long":
				case "Long":
				case "long":
				case "System.UInt64":
				case "UInt64":
				case "ulong":
					results = true;
					break;
				default:
					results = false;
					break;
			}
			return results;
		}
	}

}
