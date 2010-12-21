namespace MapDLib {

	#region using
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;

	#endregion

	internal static class PropertyMapper {
		
		// Pass in FromType and ToType in case Source or Destination are null or a derived type
		public static List<PropertyChangedResults> CopyProperties( Type FromType, Type ToType, object Source, ref object Destination, MapDirection MapDirection, ExecutionType ExecutionType ) {

			List<PropertyChangedResults> changes = new List<PropertyChangedResults>();

			MapEntry map = MapEntryManager.GetMapEntry( FromType, ToType, MapDirection );
			if ( map == null ) {
				throw new MissingMapException( FromType, ToType );
			}


			if ( Source == null ) {
				if ( MapDirection != MapDirection.DestinationToSource ) {
					Destination = null;
				} else {
					// Leave it be
				}
				return changes; // You asked for nothing and I agree
			}
			
			if ( Destination == null ) {
				// Create one
				switch ( MapDirection ) {
					case MapDirection.SourceToDestination:
						Destination = Instantiator.CreateInstance( ToType );
						break;
					case MapDirection.DestinationToSource:
						Destination = Instantiator.CreateInstance( FromType );
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

				switch ( MapDirection ) {
					case MapDirection.SourceToDestination:
						if ( ( mapEntry.IgnoreDirection & IgnoreDirection.Map ) == IgnoreDirection.Map ) {
							continue; // This property is ignored going this direction
						}
						break;
					case MapDirection.DestinationToSource:
						if ( ( mapEntry.IgnoreDirection & IgnoreDirection.MapBack ) == IgnoreDirection.MapBack ) {
							continue; // This property is ignored going this direction
						}
						break;
					default:
						throw new ArgumentOutOfRangeException( "MapDirection" );
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
					List<PropertyChangedResults> changesStep = null;
					// the below won't change what destination points to out from under Destination because Destination isn't null
					// FRAGILE: Only check source property, assuming dest property is also the same .IsClassType()
					if ( sourceProperty.PropertyType.GetGenericBaseType().IsClassType() ) {
						changesStep = ListMapper.CopyListOfClass( sourceProperty.PropertyType, destinationProperty.PropertyType, sourcePropertyValueList, ref destinationPropertyValueList, MapDirection, ExecutionType );
					} else {
						changesStep = ListMapper.CopyListOfNonClass( sourceProperty.PropertyType, destinationProperty.PropertyType, sourcePropertyValueList, ref destinationPropertyValueList, MapDirection, ExecutionType );
					}
					
					// if references aren't to the same object, note that
					if ( destinationPropertyValueOriginal != destinationPropertyValueList ) {
						changes.Add(
							new PropertyChangedResults {
								Source = new PropertyChangedResult {
									Object = Source,
									ObjectType = FromType,
									PropertyName = sourceProperty.Name,
									PropertyType =  sourceProperty.PropertyType,
									PropertyValue = sourcePropertyValue
								},
								Destination = new PropertyChangedResult {
									Object = Destination,
									ObjectType = ToType,
									PropertyName = destinationProperty.Name,
									PropertyType = destinationProperty.PropertyType,
									PropertyValue = destinationPropertyValueOriginal
								}
							} );

						switch ( ExecutionType ) {
							case ExecutionType.Copy:
								try {
									destinationProperty.SetValue( Destination, destinationPropertyValueList, null );
								} catch ( Exception ex ) {
									throw new MapFailureException( destinationProperty, Destination, destinationPropertyValue, MapFailureReason.SetDestinationFailure, ex );
								}
								break;
							case ExecutionType.Compare:
								// We've already done it
								break;
							default:
								throw new ArgumentOutOfRangeException( "ExecutionType" );
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
						throw new InvalidTypeConversionException( sourceProperty.PropertyType, destinationProperty.PropertyType, InvalidPropertyReason.NonClassTypeToClassType, destinationProperty );
					}
					if ( !destinationProperty.PropertyType.IsClassType() ) {
						throw new InvalidTypeConversionException( sourceProperty.PropertyType, destinationProperty.PropertyType, InvalidPropertyReason.ClassTypeToNonClassType, destinationProperty );
					}
					destinationPropertyValue = destinationPropertyValueOriginal;
					List<PropertyChangedResults> changesStep = PropertyMapper.CopyProperties( sourceProperty.PropertyType, destinationProperty.PropertyType, sourcePropertyValue, ref destinationPropertyValue, MapDirection, ExecutionType ); // Recurse
					// if references aren't to the same object, note that
					if ( destinationPropertyValue != destinationPropertyValueOriginal ) {
						changes.Add(
							new PropertyChangedResults {
								Source = new PropertyChangedResult {
									Object = Source,
									ObjectType = FromType,
									PropertyName = sourceProperty.Name,
									PropertyType =  sourceProperty.PropertyType,
									PropertyValue = sourcePropertyValue
								},
								Destination = new PropertyChangedResult {
									Object = Destination,
									ObjectType = ToType,
									PropertyName = destinationProperty.Name,
									PropertyType = destinationProperty.PropertyType,
									PropertyValue = destinationPropertyValueOriginal
								}
							} );

						switch ( ExecutionType ) {
							case ExecutionType.Copy:
								try {
									destinationProperty.SetValue( Destination, destinationPropertyValue, null );
								} catch ( Exception ex ) {
									throw new MapFailureException( destinationProperty, Destination, destinationPropertyValue, MapFailureReason.SetDestinationFailure, ex );
								}
								break;
							case ExecutionType.Compare:
								// We've already done it
								break;
							default:
								throw new ArgumentOutOfRangeException( "ExecutionType" );
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
					try {
						destinationPropertyValue = sourcePropertyValue;
					} catch ( Exception ex ) {
						throw new MapFailureException( destinationProperty, Destination, destinationPropertyValue, MapFailureReason.ConvertTypeFailure, ex );
					}
				} else if ( sourcePropertyValue == null ) {
					// It's null
					if ( destinationProperty.PropertyType.IsValueType ) {
						destinationPropertyValue = Instantiator.CreateInstance( destinationProperty.PropertyType );
					} else {
						destinationPropertyValue = null;
					}
				} else {
					// They're incompatible types
					try {
						destinationPropertyValue = TypeConvert.Convert( sourcePropertyValue, destinationProperty.PropertyType );
					} catch ( Exception ex ) {
						throw new MapFailureException( destinationProperty, Destination, sourcePropertyValue, MapFailureReason.ConvertTypeFailure, ex );
					}
				}

				if ( !object.Equals( destinationPropertyValue, destinationPropertyValueOriginal ) ) {
					// It changed
					changes.Add(
						new PropertyChangedResults {
							Source = new PropertyChangedResult {
								Object = Source,
								ObjectType = FromType,
								PropertyName = sourceProperty.Name,
								PropertyType =  sourceProperty.PropertyType,
								PropertyValue = sourcePropertyValue
							},
							Destination = new PropertyChangedResult {
								Object = Destination,
								ObjectType = ToType,
								PropertyName = destinationProperty.Name,
								PropertyType = destinationProperty.PropertyType,
								PropertyValue = destinationPropertyValueOriginal
							}
						} );

					switch ( ExecutionType ) {
						case ExecutionType.Copy:
							try {
								destinationProperty.SetValue( Destination, destinationPropertyValue, null );
							} catch ( Exception ex ) {
								throw new MapFailureException( destinationProperty, Destination, destinationPropertyValue, MapFailureReason.SetDestinationFailure, ex );
							}
							break;
						case ExecutionType.Compare:
							// We've already done it
							break;
						default:
							throw new ArgumentOutOfRangeException( "ExecutionType" );
					}
				}
				#endregion

			}

			return changes;
		}


	}

}
