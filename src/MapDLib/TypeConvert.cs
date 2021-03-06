namespace MapDLib {
	using System;
	using System.ComponentModel;

	/// <summary>
	/// Given a non-object, non-list, convert between types
	/// </summary>
	internal static class TypeConvert {

		public static object Convert( object Value, Type DestinationType ) {

			if ( DestinationType == null ) {
				throw new ArgumentNullException( "DestinationType" );
			}

			if ( Value == null ) {
				if ( DestinationType.IsValueType ) {
					return Instantiator.CreateInstance( DestinationType );
				} else {
					return null;
				}
			}

			Type sourceType = Value.GetType();

			if ( DestinationType.FullName == sourceType.FullName ) {
				return Value; // Type didn't change
			}

			if ( DestinationType.IsAssignableFrom( sourceType ) && Value is IConvertible && typeof(IConvertible).IsAssignableFrom( DestinationType ) ) {
				return System.Convert.ChangeType( Value, DestinationType );
			}

			if ( sourceType.IsEnum || DestinationType.IsEnum ) {
				if ( sourceType.IsNumeric() && DestinationType.IsNumeric() ) {
					// Convert to numeric then to new type
					Type sourceBaseType = sourceType.IsEnum ? Enum.GetUnderlyingType( sourceType ) : sourceType;
					Type destBaseType = DestinationType.IsEnum ? Enum.GetUnderlyingType( DestinationType ) : DestinationType;

					object sourceBase = Value;
					if ( sourceType.IsEnum ) {
						sourceBase = System.Convert.ChangeType( Value, Enum.GetUnderlyingType( sourceType ) );
					}
					object destValue = sourceBase;
					if ( destBaseType.FullName != sourceBaseType.FullName ) {
						destValue = System.Convert.ChangeType( destValue, destBaseType );
					}
					if ( DestinationType.IsEnum ) {
						destValue = Enum.Parse( DestinationType, sourceBase.ToString() );
					}
					return destValue;
				} else {
					// Fall into string conversion
				}
			}

			// We're still here, convert to a string then to new type
			string val = TypeConvert.ConvertToString( Value, sourceType );
			object result = TypeConvert.ConvertFromString( val, DestinationType );
			return result;
		}

		#region ConvertFromString
		public static object ConvertFromString( string Value, string Type ) {

			if ( string.IsNullOrEmpty( Type ) ) {
				throw new ArgumentNullException( "Type" );
			}
			Type derivedType = GetDataType( Type );

			return ConvertFromString( Value, derivedType );
		}

		public static object ConvertFromString( string Value, Type Type ) {

			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			if ( string.IsNullOrEmpty( Value ) ) {
				// You asked for nothing, you got it
				if ( Type == typeof(string) ) {
					return Value; // It already is
				}
				if ( Type.IsValueType ) {
					return Instantiator.CreateInstance( Type );
				}
				return null;
			}
			if ( Type.IsNullable() ) {
				// because (for example) 123.456 is both a double and a Nullable<double> this is probably safe
				Type = Type.GetNullableBaseType();
			}

			object results = null;
			ConversionMethod method = GetConversionMethod( Type );

			try {
				switch ( method ) {
					case ConversionMethod.ChangeType:
						try {
							results = System.Convert.ChangeType( Value, Type );
						} catch {
							TypeConverter converter = TypeDescriptor.GetConverter( Type );
							if ( converter != null && converter.CanConvertFrom( typeof(string) ) ) {
								results = converter.ConvertFrom( Value );
							} else {
								throw;
							}
						}
						break;
					case ConversionMethod.ByteArray:
						results = System.Convert.FromBase64String( Value );
						break;
					case ConversionMethod.Guid:
						results = new Guid( Value );
						break;
					case ConversionMethod.TimeSpan:
						results = TimeSpan.Parse( Value );
						break;
					case ConversionMethod.Char:
						results = System.Convert.ChangeType( Value, Type );
						break;
					case ConversionMethod.Enum:
						if ( !Type.IsEnum ) {
							throw new ArgumentException( Type.FullName + " is not an enum" );
						}
						if ( string.IsNullOrEmpty( Value ) ) {
							results = 0; // FRAGILE: ASSUME: 0 is a default for your type
						} else {
							results = Enum.Parse( Type, Value );
						}
						break;
					default:
						throw new ArgumentOutOfRangeException( "Can't determine conversion method" );
				}
			} catch ( Exception ex ) {
				throw new FormatException( string.Format( "Error deserializing {0} from {1} via {2}: {3}", Value, Type.FullName, method, ex.Message ), ex );
			}

			return results;
		}
		#endregion

		#region ConvertToString
		// pass in Type in case Value is null
		public static string ConvertToString( object Value, string Type ) {
			return ConvertToString( Value, Type, false );
		}

		public static string ConvertToString( object Value, string Type, bool ToStringIfSerializeFails ) {

			if ( string.IsNullOrEmpty( Type ) ) {
				throw new ArgumentNullException( "Type" );
			}
			Type derivedType = GetDataType( Type );

			return ConvertToString( Value, derivedType, ToStringIfSerializeFails );
		}

		public static string ConvertToString( object Value, Type Type ) {
			return ConvertToString( Value, Type, false );
		}

		public static string ConvertToString( object Value, Type Type, bool ToStringIfSerializeFails ) {

			if ( Type == null && Value != null ) {
				Type = Value.GetType();
			}
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			if ( Value == null ) {
				return null; // You asked for nothing, you got it
			}

			string results = null;
			ConversionMethod method = GetConversionMethod( Type );

			try {
				switch ( method ) {
					case ConversionMethod.ChangeType:
						Type stringType = typeof(string);
						if ( Value is IConvertible ) {
							results = (string)System.Convert.ChangeType( Value, stringType );
						} else {
							TypeConverter converter = TypeDescriptor.GetConverter( Type );
							if ( converter != null && converter.CanConvertTo( stringType ) ) {
								results = (string)converter.ConvertTo( Value, stringType );
							} else {
								results = Value.ToString();
							}
						}
						break;
					case ConversionMethod.ByteArray:
						results = System.Convert.ToBase64String( (byte[])Value );
						break;
					case ConversionMethod.Guid:
					case ConversionMethod.TimeSpan:
						results = Value.ToString();
						break;
					case ConversionMethod.Char:
						char c = (char)Value;
						if ( c == '\0' ) {
							results = null;
						} else {
							results = c.ToString();
						}
						break;
					case ConversionMethod.Enum:
						results = Value.ToString();
						break;
					default:
						throw new ArgumentOutOfRangeException( "Can't determine conversion method" );
				}
			} catch ( Exception ex ) {
				throw new Exception( string.Format( "Error serializing {0} to {1} as {2}: {3}", Value, Type, method, ex.Message ), ex );
			}

			return results;
		}
		#endregion

		#region GetDataType
		private static Type GetDataType( string TypeName ) {
			return Type.GetType( TypeName, true, true );
		}

		#endregion

		#region GetConversionMethod
		private enum ConversionMethod {
			ChangeType,
			ByteArray,
			TimeSpan,
			Guid,
			Char,
			Enum
		}

		private static ConversionMethod GetConversionMethod( Type Type ) {
			ConversionMethod method = ConversionMethod.ChangeType;

			if ( Type.IsEnum ) {
				return ConversionMethod.Enum;
			}
			if ( Type.IsNullable() ) {
				return GetConversionMethod( Type.GetNullableBaseType() );
			}

			// Since I'm lazy, this is hardly an exhaustive list
			switch ( Type.FullName ) {
				case "System.String":
				case "String":
				case "string":
				case "System.Int32":
				case "Int32":
				case "int":
				case "System.Boolean":
				case "Boolean":
				case "bool":
				case "System.Long":
				case "Long":
				case "long":
				case "System.Double":
				case "Double":
				case "double":
				case "System.DateTime":
				case "DateTime":
				case "System.UInt32":
				case "UInt32":
				case "uint":
				case "System.UInt64":
				case "UInt64":
				case "ulong":
				case "System.Single":
				case "Single":
				case "single":
					method = ConversionMethod.ChangeType;
					break;

				case "System.Byte[]":
				case "byte[]":
					method = ConversionMethod.ByteArray;
					break;

				case "System.Guid":
				case "Guid":
					method = ConversionMethod.Guid;
					break;

				case "System.TimeSpan":
				case "TimeSpan":
					method = ConversionMethod.TimeSpan;
					break;

				case "System.Char":
				case "Char":
				case "char":
					method = ConversionMethod.Char;
					break;

				case "System.Enum":
				case "Enum":
					method = ConversionMethod.Enum;
					break;

				default:
					method = ConversionMethod.ChangeType;
					break;
			}

			return method;
		}
		#endregion

	}
}