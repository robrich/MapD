namespace AutoMapper2Lib {

	#region using
	using System;
	#endregion

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
					return Activator.CreateInstance( DestinationType );
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
					return Activator.CreateInstance( Type );
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
					case ConversionMethod.Serialize:
						results = SerializationHelper.Deserialize( Value, Type );
						break;
					case ConversionMethod.ChangeType:
						results = System.Convert.ChangeType( Value, Type );
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
					case ConversionMethod.Serialize:
						try {
							results = SerializationHelper.Serialize( Value, true );
						} catch ( InvalidOperationException ) {
							// Serialization failed
							if ( ToStringIfSerializeFails ) {
								results = Value.ToString();
							} else {
								throw;
							}
						}
						break;
					case ConversionMethod.ChangeType:
						if ( Value is IConvertible ) {
							results = (string)System.Convert.ChangeType( Value, typeof(string) );
						} else {
							results = SerializationHelper.Serialize( Value, true );
						}
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
			Type results = null;

			if ( string.IsNullOrEmpty( TypeName ) ) {
				return results; // You asked for nothing, you got it
			}

			switch ( TypeName ) {
				case "bool":
				case "System.Boolean":
					results = typeof( bool );
					break;
				case "string":
				case "System.String":
					results = typeof( string );
					break;
				case "int":
				case "System.Int32":
					results = typeof( int );
					break;
				case "long":
				case "System.Int64":
					results = typeof( long );
					break;
				case "double":
				case "System.Double":
					results = typeof( double );
					break;
				case "DateTime":
				case "System.DateTime":
					results = typeof( DateTime );
					break;
				case "System.DBNull":
				case "null":
					results = null;
					break;
				case "System.UInt32":
				case "uint":
					results = typeof( uint );
					break;
				case "System.UInt64":
				case "ulong":
					results = typeof( ulong );
					break;
				case "System.Single":
				case "Single":
				case "single":
					results = typeof( Single );
					break;
				case "System.TimeSpan":
				case "TimeSpan":
					results = typeof( TimeSpan );
					break;
				case "System.Char":
				case "char":
					results = typeof( char );
					break;
				case "System.Guid":
				case "Guid":
					results = typeof( Guid );
					break;

				default:
					results = Type.GetType( TypeName, true, true );
					break;

			}

			return results;
		}

		#endregion

		#region GetConversionMethod
		private enum ConversionMethod {
			Serialize,
			ChangeType,
			TimeSpan,
			Guid,
			Char,
			Enum
		}

		private static ConversionMethod GetConversionMethod( Type Type ) {
			ConversionMethod method = ConversionMethod.Serialize;

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
					method = ConversionMethod.Serialize;
					break;
			}

			return method;
		}
		#endregion

	}

}
