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
		
		// Pass in FromType and ToType in case Source or Destination are null or a derived type
		public static List<PropertyChanged> CopyProperties( Type FromType, Type ToType, object Source, ref object Destination, MapDirection MapDirection ) {

			List<PropertyChanged> changes = new List<PropertyChanged>();

			MapEntry map = MapEntryManager.GetMapEntry( FromType, ToType, MapDirection );
			if ( map == null ) {
				throw new MissingMapException( FromType, ToType );
			}


			if ( Source == null ) {
				Destination = null;
				return changes; // You asked for nothing and I agree
			}
			
			if ( Destination == null ) {
				// Create one
				switch ( MapDirection ) {
					case MapDirection.SourceToDestination:
						Destination = Activator.CreateInstance( ToType );
						break;
					case MapDirection.DestinationToSource:
						Destination = Activator.CreateInstance( FromType );
						break;
					default:
						throw new ArgumentOutOfRangeException( "MapDirection" );
				}
			}
			

			if ( FromType.IsGenericType && !FromType.IsNullable() ) {
				// Because we previously filtered for List<>, we'll assume this isn't a collection
				//throw new NotSupportedException(
				//    string.Format(
				//        "How to convert from T<U> to V<W>?: {0}<{1}> to {2}<{3}>",
				//        FromType.Name, FromType.GetGenericBaseType().Name, ToType.Name, ToType.GetGenericBaseType().Name
				//        ) );
			}
			
			foreach ( var mapEntry in map.Properties ) {

				PropertyInfo sourceProperty = mapEntry.Source;
				PropertyInfo destinationProperty = mapEntry.Destination;
				if ( MapDirection == MapDirection.DestinationToSource ) {
					sourceProperty = mapEntry.Destination;
					destinationProperty = mapEntry.Source;
				}


				object sourcePropertyValue = null;
				object destinationPropertyValueOriginal = null;
				object destinationPropertyValue = null;

				try {
					sourcePropertyValue = sourceProperty.GetValue( Source, null );
				} catch (Exception ex) {
					throw new MapFailureException( sourceProperty, Source, null, MapFailureReason.GetSourceFailure, ex );
				}
				try {
					destinationPropertyValueOriginal = destinationProperty.GetValue( Destination, null );
				} catch (Exception ex) {
					throw new MapFailureException( destinationProperty, Destination, null, MapFailureReason.GetDestinationFailure, ex );
				}

				if ( sourceProperty.IsListOfT() || destinationProperty.IsListOfT() ) {
					#region Copy property as List<>
					if ( !sourceProperty.IsListOfT() ) {
						throw new InvalidTypeConversionException( sourceProperty.PropertyType, destinationProperty.PropertyType, InvalidPropertyReason.NonListTypeToListType, destinationProperty );
					}
					if ( !destinationProperty.IsListOfT() ) {
						throw new InvalidTypeConversionException( sourceProperty.PropertyType, destinationProperty.PropertyType, InvalidPropertyReason.ListTypeToNonListType, destinationProperty );
					}
					destinationPropertyValue = destinationPropertyValueOriginal;

					Type destListType = destinationProperty.PropertyType.GetGenericBaseType();
					Type sourceListType = sourceProperty.PropertyType.GetGenericBaseType();
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
						changesStep = ListMapper.CopyListOfClass( sourceProperty.PropertyType, destinationProperty.PropertyType, sourcePropertyValueList, ref destinationPropertyValueList, MapDirection );
					} else {
						changesStep = ListMapper.CopyListOfNonClass( sourceProperty.PropertyType, destinationProperty.PropertyType, sourcePropertyValueList, ref destinationPropertyValueList );
					}
					
					// if references aren't to the same object, note that
					if ( destinationPropertyValueOriginal != destinationPropertyValueList ) {
						changes.Add(
							new PropertyChanged {
								ObjectType = ToType,
								Object = Destination,
								PropertyType = destinationProperty.PropertyType,
								PropertyName = destinationProperty.Name,
								OldValue = TypeConvert.ConvertToString( destinationPropertyValueOriginal, destinationProperty.PropertyType ),
								NewValue = TypeConvert.ConvertToString( destinationPropertyValueList, destinationProperty.PropertyType )
							} );
						try {
							destinationProperty.SetValue( Destination, destinationPropertyValueList, null );
						} catch ( Exception ex ) {
							throw new MapFailureException( destinationProperty, Destination, destinationPropertyValue, MapFailureReason.SetDestinationFailure, ex );
						}
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
					List<PropertyChanged> changesStep = PropertyMapper.CopyProperties( sourceProperty.PropertyType, destinationProperty.PropertyType, sourcePropertyValue, ref destinationPropertyValue, MapDirection ); // Recurse
					// if references aren't to the same object, note that
					if ( destinationPropertyValue != destinationPropertyValueOriginal ) {
						changes.Add(
							new PropertyChanged {
								ObjectType = ToType,
								Object = Destination,
								PropertyType = destinationProperty.PropertyType,
								PropertyName = destinationProperty.Name,
								OldValue = TypeConvert.ConvertToString( destinationPropertyValueOriginal, destinationProperty.PropertyType, true ),
								NewValue = TypeConvert.ConvertToString( destinationPropertyValue, destinationProperty.PropertyType, true )
							} );
						try {
							destinationProperty.SetValue( Destination, destinationPropertyValue, null );
						} catch ( Exception ex ) {
							throw new MapFailureException( destinationProperty, Destination, destinationPropertyValue, MapFailureReason.SetDestinationFailure, ex );
						}
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
							ObjectType = ToType,
							Object = Destination,
							PropertyType = destinationProperty.PropertyType,
							PropertyName = destinationProperty.Name,
							OldValue = TypeConvert.ConvertToString( destinationPropertyValueOriginal, destinationProperty.PropertyType ),
							NewValue = TypeConvert.ConvertToString( destinationPropertyValue, destinationProperty.PropertyType )
						} );
					try {
						destinationProperty.SetValue( Destination, destinationPropertyValue, null );
					} catch ( Exception ex ) {
						throw new MapFailureException( destinationProperty, Destination, destinationPropertyValue, MapFailureReason.SetDestinationFailure, ex );
					}
				}
				#endregion

			}

			return changes;
		}


	}

}