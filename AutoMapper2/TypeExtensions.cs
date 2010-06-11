namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	#endregion

	internal static class TypeExtensions {

		public static bool IsClassType( this Type Type ) {
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
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

		public static Type GetGenericBaseType( this Type Type ) {
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			if ( !Type.IsGenericType ) {
				throw new ArgumentOutOfRangeException( "Type", Type.FullName + " isn't Generic" );
			}
			Type genericType = Type.GetGenericTypeDefinition();
			Type[] args = Type.GetGenericArguments();
			if ( genericType != typeof( List<> ) || args.Length != 1 ) {
				throw new ArgumentOutOfRangeException( "Type", Type.FullName + " isn't List<T>" );
			}
			return args[0];
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
