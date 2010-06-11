namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Data.Linq.Mapping;
	using System.Data.Objects;
	using System.Data.Objects.DataClasses;
	using System.Data.EntityClient;
	using System.Linq;
	#endregion

	internal static class PropertyMapper {

		public static U CopyProperties<T, U>( T Source, SmallerObject SmallerObject, bool ExcludeLinqAssociationProperties )
			where T : class
			where U : class, new() {

			if ( Source == null ) {
				return null;
			}

			U destination = null;
			CopyProperties<T, U>( Source, ref destination, SmallerObject, ExcludeLinqAssociationProperties );
			return destination;
		}

		public static List<PropertyChanged> CopyProperties<T, U>( T Source, ref U Destination, SmallerObject SmallerObject, bool ExcludeLinqAssociationProperties )
			where T : class
			where U : class, new() {

			List<PropertyChanged> changes = new List<PropertyChanged>();

			if ( Destination == null ) {
				// Create one
				Destination = (U)Activator.CreateInstance( typeof( U ) );
			}

			if ( Source == null ) {
				return changes; // You asked for nothing and I agree
			}

			// typeof(T) means we only get T's properties, and not properties from objects derived from T
			// Source.GetType() means we get derived properties too

			List<PropertyInfo> sourceProperties = ReflectionHelper.GetProperties( typeof( T ) );
			List<PropertyInfo> destinationProperties = ReflectionHelper.GetProperties( typeof( U ) );

			List<PropertyInfo> loopProperties = null;
			switch ( SmallerObject ) {
				case SmallerObject.Source:
					loopProperties = sourceProperties;
					break;
				case SmallerObject.Destination:
					loopProperties = destinationProperties;
					break;
				default:
					throw new ArgumentOutOfRangeException( "SmallerObject" );
			}

			if ( loopProperties == null || loopProperties.Count == 0 ) {
				// That's fine, there's just nothing to do
			}

			foreach ( PropertyInfo smallerProperty in loopProperties ) {

				if ( smallerProperty.IsMapIgnored() ) {
					continue; // Ignore it
				}

				PropertyInfo sourceProperty = sourceProperties.GetPropertyByName( smallerProperty.Name );
				PropertyInfo destinationProperty = destinationProperties.GetPropertyByName( smallerProperty.Name );

				PropertyInfo largerProperty = null;
				#region is either property not found?
				switch ( SmallerObject ) {
					case SmallerObject.Source:
						if ( sourceProperty == null ) {
							continue;
						}
						if ( destinationProperty == null ) {
							// TODO: Just fail silently? continue;
							throw new MissingPropertyException<T>( smallerProperty );
						}
						largerProperty = destinationProperty;
						break;
					case SmallerObject.Destination:
						if ( destinationProperty == null ) {
							continue;
						}
						if ( sourceProperty == null ) {
							// TODO: Just fail silently? continue;
							throw new MissingPropertyException<U>( smallerProperty );
						}
						largerProperty = sourceProperty;
						break;
					default:
						throw new ArgumentOutOfRangeException( "SmallerObject" );
				}
				#endregion

				if ( largerProperty.IsMapIgnored() ) {
					continue; // Ignore it, TODO: throw? do it anyway?
				}

				// TODO: If property can't read/write, does this property "not exist"?
				if ( !sourceProperty.CanRead ) {
					throw new InvalidPropertyException<T>( sourceProperty, InvalidPropertyReason.CantRead );
				}
				if ( !destinationProperty.CanWrite ) {
					throw new InvalidPropertyException<U>( destinationProperty, InvalidPropertyReason.CantWrite );
				}
				if ( !destinationProperty.CanRead ) {
					throw new InvalidPropertyException<U>( destinationProperty, InvalidPropertyReason.CantWrite );
				}

				if ( ExcludeLinqAssociationProperties ) {
					if ( sourceProperty.IsLinqProperty()
						|| destinationProperty.IsLinqProperty() ) {
						continue;
					}
				}


				object sourcePropertyValue = sourceProperty.GetValue( Source, null );
				object destinationPropertyValueOriginal = destinationProperty.GetValue( Destination, null );
				object destinationPropertyValue = null;

				if ( sourceProperty.IsListOfT() || destinationProperty.IsListOfT() ) {
					#region Copy property as List<>
					if ( !sourceProperty.IsListOfT() ) {
						throw new InvalidTypeConversionException( sourceProperty.PropertyType, destinationProperty.PropertyType, InvalidPropertyReason.NonListTypeToListType, destinationProperty );
					}
					if ( !destinationProperty.IsListOfT() ) {
						throw new InvalidTypeConversionException( sourceProperty.PropertyType, destinationProperty.PropertyType, InvalidPropertyReason.ListTypeToNonListType, destinationProperty );
					}
					destinationPropertyValue = destinationPropertyValueOriginal;

					Type destListType = destinationProperty.PropertyType.GetGenericTypeDefinition();
					Type sourceListType = sourceProperty.PropertyType.GetGenericTypeDefinition();
					if ( destListType.IsClassType() != sourceListType.IsClassType() ) {
						if ( !sourceProperty.PropertyType.IsClassType() ) {
							throw new InvalidTypeConversionException( sourceProperty.PropertyType, destinationProperty.PropertyType, InvalidPropertyReason.ListNonClassTypeToListClassType, destinationProperty );
						}
						if ( !destinationProperty.PropertyType.IsClassType() ) {
							throw new InvalidTypeConversionException( sourceProperty.PropertyType, destinationProperty.PropertyType, InvalidPropertyReason.ListClassTypeToListNonClassType, destinationProperty );
						}
					}

					IList sourcePropertyValueList = sourcePropertyValue as IList;
					IList destinationPropertyValueList = destinationPropertyValue as IList;
					List<PropertyChanged> changesStep = null;
					// the below won't change what destination points to out from under Destination because Destination isn't null
					// FRAGILE: Only check source property, assuming dest property is also the same .IsClassType()
					if ( sourceProperty.PropertyType.GetGenericBaseType().IsClassType() ) {
						changesStep = ListMapper.CopyListOfClass( sourceProperty.PropertyType, destinationProperty.PropertyType, sourcePropertyValueList, ref destinationPropertyValueList, SmallerObject, ExcludeLinqAssociationProperties );
					} else {
						changesStep = ListMapper.CopyListOfNonClass( sourceProperty.PropertyType, destinationProperty.PropertyType, sourcePropertyValueList, ref destinationPropertyValueList );
					}
					
					// if references aren't to the same object, note that
					if ( destinationPropertyValueOriginal != destinationPropertyValueList ) {
						changes.Add(
							new PropertyChanged {
								ObjectType = typeof( U ),
								Object = Destination,
								PropertyType = destinationProperty.PropertyType,
								PropertyName = destinationProperty.Name,
								OldValue = TypeConvert.ConvertToString( destinationPropertyValueOriginal, destinationProperty.PropertyType ),
								NewValue = TypeConvert.ConvertToString( destinationPropertyValueList, destinationProperty.PropertyType )
							} );
						destinationProperty.SetValue( Destination, destinationPropertyValueList, null );
					}
					if ( changesStep != null && changesStep.Count > 0 ) {
						changes.AddRange( changesStep );
					}
					#endregion
					continue; // All done
				}

				if ( sourceProperty.PropertyType.IsClassType() || destinationProperty.PropertyType.IsClassType() ) {
					#region Copy property as class type (recurse)
					if ( !sourceProperty.PropertyType.IsClassType() ) {
						throw new InvalidTypeConversionException( sourceProperty.PropertyType, destinationProperty.PropertyType, InvalidPropertyReason.NonClassTypeToClassType, ToPropertyInfo: destinationProperty );
					}
					if ( !destinationProperty.PropertyType.IsClassType() ) {
						throw new InvalidTypeConversionException( sourceProperty.PropertyType, destinationProperty.PropertyType, InvalidPropertyReason.ClassTypeToNonClassType, ToPropertyInfo: destinationProperty );
					}
					destinationPropertyValue = destinationPropertyValueOriginal;
					List<PropertyChanged> changesStep = PropertyMapper.CopyProperties( sourcePropertyValue, ref destinationPropertyValue, SmallerObject, ExcludeLinqAssociationProperties ); // Recurse
					// if references aren't to the same object, note that
					if ( destinationPropertyValueOriginal != destinationPropertyValue ) {
						changes.Add(
							new PropertyChanged {
								ObjectType = typeof( U ),
								Object = Destination,
								PropertyType = destinationProperty.PropertyType,
								PropertyName = destinationProperty.Name,
								OldValue = TypeConvert.ConvertToString( destinationPropertyValueOriginal, destinationProperty.PropertyType ),
								NewValue = TypeConvert.ConvertToString( destinationPropertyValue, destinationProperty.PropertyType )
							} );
						destinationProperty.SetValue( Destination, destinationPropertyValue, null );
					}
					if ( changesStep != null && changesStep.Count > 0 ) {
						changes.AddRange( changesStep );
					}
					#endregion
					continue; // All done
				}

				if ( sourceProperty.PropertyType.IsGenericType ) {
					if ( !sourceProperty.PropertyType.IsNullable() ) {
						// T<U>
						// just do the normal (it'll serialize, and if property type isn't identical, probably fail
					}
				}


				#region Copy property as non-class type
				if ( destinationProperty.PropertyType.IsAssignableFrom( sourceProperty.PropertyType ) ) {
					// They're compatable types
					destinationPropertyValue = sourcePropertyValue;
				} else if ( sourcePropertyValue == null ) {
					// It's null
					if ( destinationProperty.PropertyType.IsValueType ) {
						destinationPropertyValue = Activator.CreateInstance( destinationProperty.PropertyType );
					} else {
						destinationPropertyValue = null;
					}
				} else {
					// They're incompatible types
					destinationPropertyValue = TypeConvert.Convert( sourcePropertyValue, destinationProperty.PropertyType );
				}

				if ( destinationPropertyValue != destinationPropertyValueOriginal ) {
					// It changed
					changes.Add(
						new PropertyChanged {
							ObjectType = typeof(U),
							Object = Destination,
							PropertyType = destinationProperty.PropertyType,
							PropertyName = destinationProperty.Name,
							OldValue = TypeConvert.ConvertToString( destinationPropertyValueOriginal, destinationProperty.PropertyType ),
							NewValue = TypeConvert.ConvertToString( destinationPropertyValue, destinationProperty.PropertyType )
						} );
					destinationProperty.SetValue( Destination, destinationPropertyValue, null );
				}
				#endregion

			}

			return changes;
		}


	}

}
