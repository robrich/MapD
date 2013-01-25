namespace MapDLib {
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	internal static class MapBuilder {

		public static void BuildMap( MapEntry MapEntry ) {
			Type fromType = MapEntry.From;

			// AssertTypesCanMap<From,To>() would catch if fromType.IsListOfT() doesn't match toType.IsListOfT()
			if ( fromType.IsListOfT() ) {
				// List

				if ( fromType.GetGenericBaseType().IsClassType() ) {
					// ListOfClass
					MapEntry.MapEntryType = MapEntryType.ListOfClass;
					BuildListClassMap( MapEntry );
				} else {
					// ListOfNonClass
					MapEntry.MapEntryType = MapEntryType.ListOfNonClass;
					BuildListNonClassMap( MapEntry );
				}

			} else {
				// Non-list

				if ( fromType.IsClassType() ) {
					// Class
					MapEntry.MapEntryType = MapEntryType.Class;
					BuildClassMap( MapEntry );
				} else {
					// NonClass
					MapEntry.MapEntryType = MapEntryType.NonClass;
					BuildNonClassMap( MapEntry );
				}

			}
		}

		public static void BuildClassMap( MapEntry MapEntry ) {

			List<MapEntryProperty> properties = new List<MapEntryProperty>();

			Type fromType = MapEntry.From;
			Type toType = MapEntry.To;

			if ( fromType.IsGenericType && !fromType.IsNullable() ) {
				// Because we previously filtered for List<>, we'll assume this isn't a collection
				//throw new NotSupportedException(
				//    string.Format(
				//        "How to convert from T<U> to V<W>?: {0}<{1}> to {2}<{3}>",
				//        fromType.Name, fromType.GetGenericBaseType().Name, toType.Name, toType.GetGenericBaseType().Name
				//        ) );
				//fromType = fromType.GetGenericBaseType();
				//toType = toType.GetGenericBaseType();
			}

			IgnoreDirection ignoreDirectionBase = IgnoreDirection.None
				| toType.GetIgnoreStatus()
				| fromType.GetIgnoreStatus();

			List<PropertyInfo> fromProperties = ReflectionHelper.GetProperties( fromType );
			List<PropertyInfo> toProperties = ReflectionHelper.GetProperties( toType );

			PropertyIs ignorePropertiesIfObject = toType.GetIgnorePropertiesIf() 
				| fromType.GetIgnorePropertiesIf() 
				| MapD.Config.IgnorePropertiesIf;

			if ( toProperties != null && toProperties.Count != 0 ) {
				foreach ( PropertyInfo toProperty in toProperties ) {

					IgnoreDirection ignoreDirection = ignoreDirectionBase;

					ignoreDirection |= toProperty.GetIgnoreStatus();
					if ( ignoreDirection == ( IgnoreDirection.Map | IgnoreDirection.MapBack ) ) {
						continue; // Ignore it fully
					}
					if ( MapD.Config.ExcludeLinqProperties && toProperty.IsLinqProperty() ) {
						continue;
					}

					string fromPropertyName = toProperty.Name;
					// ColumnAttribute primary key
					RemapPropertyAttribute remapProperty = (RemapPropertyAttribute)toProperty.GetFirstAttribute( typeof( RemapPropertyAttribute ) );
					if ( remapProperty != null ) {
						fromPropertyName = remapProperty.MapPropertyName;
					}

					PropertyInfo fromProperty = fromProperties.GetPropertyByName( fromPropertyName );
					if ( fromProperty == null ) {
						throw new InvalidPropertyException( toProperty, InvalidPropertyReason.MissingProperty );
					}

					ignoreDirection |= fromProperty.GetIgnoreStatus();
					if ( ignoreDirection == ( IgnoreDirection.Map | IgnoreDirection.MapBack ) ) {
						continue; // Ignore it fully
					}
					if ( MapD.Config.ExcludeLinqProperties && fromProperty.IsLinqProperty() ) {
						continue;
					}

					PropertyIs ignorePropertiesIf = toProperty.GetIgnorePropertiesIf() 
						| fromProperty.GetIgnorePropertiesIf()
						| ignorePropertiesIfObject;

					if ( !fromProperty.CanRead ) {
						if ( ( ignorePropertiesIf & PropertyIs.ReadOnly ) == PropertyIs.ReadOnly ) {
							continue;
						}
						if ( ( ignoreDirection & IgnoreDirection.Map ) == IgnoreDirection.Map ) {
							// If we can't read the source, we'll just write to dest and say "it changed from unknown can't read value"
						} else {
							throw new InvalidPropertyException( fromProperty, InvalidPropertyReason.CantRead );
						}
					}
					if ( !toProperty.CanRead ) {
						if ( ( ignorePropertiesIf & PropertyIs.ReadOnly ) == PropertyIs.ReadOnly ) {
							continue;
						}
						if ( ( ignoreDirection & IgnoreDirection.MapBack ) == IgnoreDirection.MapBack ) {
							// If we can't read the source, we'll just write to dest and say "it changed from unknown can't read value"
						} else {
							throw new InvalidPropertyException( toProperty, InvalidPropertyReason.CantRead );
						}
					}
					if ( !fromProperty.CanWrite ) {
						if ( ( ignorePropertiesIf & PropertyIs.WriteOnly ) == PropertyIs.WriteOnly ) {
							continue;
						}
						if ( ( ignoreDirection & IgnoreDirection.MapBack ) != IgnoreDirection.MapBack ) {
							// If we're ever called to map back, fromProperty.CanWrite is also important
							throw new InvalidPropertyException( fromProperty, InvalidPropertyReason.CantWrite );
						}
					}
					if ( !toProperty.CanWrite ) {
						if ( ( ignorePropertiesIf & PropertyIs.WriteOnly ) == PropertyIs.WriteOnly ) {
							continue;
						}
						if ( ( ignoreDirection & IgnoreDirection.Map ) != IgnoreDirection.Map ) {
							throw new InvalidPropertyException( toProperty, InvalidPropertyReason.CantWrite );
						}
					}
					// If it's a list and it's initialized, we can get by without write, but that's a fragile assumption, so don't

					if ( fromProperty.PropertyType.IsListOfT() != toProperty.PropertyType.IsListOfT() ) {
						if ( fromProperty.PropertyType.IsListOfT() ) {
							throw new InvalidTypeConversionException( fromProperty.PropertyType, toProperty.PropertyType, InvalidPropertyReason.ListTypeToNonListType, toProperty );
						}
						if ( toProperty.PropertyType.IsListOfT() ) {
							throw new InvalidTypeConversionException( fromProperty.PropertyType, toProperty.PropertyType, InvalidPropertyReason.NonListTypeToListType, toProperty );
						}
					}

					if ( fromProperty.PropertyType.IsClassType() != toProperty.PropertyType.IsClassType() ) {
						if ( fromProperty.PropertyType.IsClassType() ) {
							throw new InvalidTypeConversionException( fromProperty.PropertyType, toProperty.PropertyType, InvalidPropertyReason.ClassTypeToNonClassType, toProperty );
						}
						if ( toProperty.PropertyType.IsClassType() ) {
							throw new InvalidTypeConversionException( fromProperty.PropertyType, toProperty.PropertyType, InvalidPropertyReason.NonClassTypeToClassType, toProperty );
						}
					}

					if ( fromProperty.PropertyType != toProperty.PropertyType
						&& ( ( !fromProperty.PropertyType.IsListOfT() && fromProperty.PropertyType.IsClassType() )
							|| ( fromProperty.PropertyType.IsListOfT() && fromProperty.PropertyType.GetGenericBaseType().IsClassType() )
							)
						) {
						// Insure a map exists because you should've mapped them all before you use any of them
						// This is redundant, but nice to know
						var map = MapEntryManager.GetMapEntry( fromProperty.PropertyType, toProperty.PropertyType );
						if ( map == null ) {
							throw new MissingMapException( fromProperty.PropertyType, toProperty.PropertyType );
						}
					}

					properties.Add(
						new MapEntryProperty {
							Source = fromProperty,
							Destination = toProperty,
							IgnoreDirection = ignoreDirection
						} );

				}
			}

			MapEntry.Properties = properties;
		}

		public static void BuildNonClassMap( MapEntry MapEntry ) {
			// There's nothing to do but set the "we did it"
			MapEntry.Properties = new List<MapEntryProperty>();
		}

		public static void BuildListClassMap( MapEntry MapEntry ) {

			Type fromType = MapEntry.From;
			Type toType = MapEntry.To;

			Type fromInnerType = fromType.GetGenericBaseType();
			Type toInnerType = toType.GetGenericBaseType();

			List<PropertyInfo> fromProperties = ReflectionHelper.GetProperties( fromInnerType );
			List<PropertyInfo> toProperties = ReflectionHelper.GetProperties( toInnerType );

			List<PropertyInfo> fromPrimaryKeys = null;
			List<PropertyInfo> toPrimaryKeys = null;

			MapEntry.Properties = new List<MapEntryProperty>();

			// Look to destination for primary keys

			toPrimaryKeys = toProperties.GetPrimaryKeys();
			if ( toPrimaryKeys != null && toPrimaryKeys.Count > 0 ) {
				// Look to source for matching properties

				foreach ( PropertyInfo toPrimaryKey in toPrimaryKeys ) {
					PropertyInfo fromProperty = fromProperties.GetPropertyByName( toPrimaryKey.Name );
					if ( fromProperty == null ) {
						throw new InvalidTypeConversionException( fromType, toType, InvalidPropertyReason.MissingFromPrimaryKey, toPrimaryKey );
					}
					MapEntry.Properties.Add(
						new MapEntryProperty {
							Source = fromProperty,
							Destination = toPrimaryKey,
							IgnoreDirection = IgnoreDirection.None
						} );
				}

				return;
			}

			// Look to source for primary keys

			fromPrimaryKeys = fromProperties.GetPrimaryKeys();
			if ( fromPrimaryKeys != null && fromPrimaryKeys.Count > 0 ) {
				// Look to destination for matching properties

				foreach ( PropertyInfo fromPrimaryKey in fromPrimaryKeys ) {
					PropertyInfo toProperty = toProperties.GetPropertyByName( fromPrimaryKey.Name );
					if ( toProperty == null ) {
						throw new InvalidTypeConversionException( fromType, toType, InvalidPropertyReason.MissingToPrimaryKey, fromPrimaryKey );
					}
					MapEntry.Properties.Add( new MapEntryProperty {
						Source = fromPrimaryKey,
						Destination = toProperty
					} );
				}

				return;
			}

			// No primary key found on either object
			throw new InvalidTypeConversionException( fromType, toType, InvalidPropertyReason.MissingPrimaryKey );

		}

		public static void BuildListNonClassMap( MapEntry MapEntry ) {
			// There's nothing to do but set the "we did it"
			MapEntry.Properties = new List<MapEntryProperty>();
		}

	}
}