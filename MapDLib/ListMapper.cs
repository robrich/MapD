namespace MapDLib {

	#region using
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	#endregion

	internal class ListMapper {

		// Pass in FromType and ToType in case Source or Destination are null or a derived type
		// No need for direction since the logic would be the same either way
		// If List<Non-class>
		public static List<PropertyChangedResults> CopyListOfNonClass( Type FromType, Type ToType, IList Source, ref IList Destination, MapDirection MapDirection, ExecutionType ExecutionType ) {

			List<PropertyChangedResults> changes = new List<PropertyChangedResults>();

			Type toInnerType = ToType.GetGenericBaseType();
			Type fromInnerType = FromType.GetGenericBaseType();

			if ( toInnerType.IsClassType() ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.ListNonClassTypeToListClassType );
			}
			if ( fromInnerType.IsClassType() ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.ListClassTypeToListNonClassType );
			}

			switch ( ExecutionType ) {
				case ExecutionType.Copy:
					if ( Source == null ) {
						changes.Add(
							new PropertyChangedResults {
								Source = new PropertyChangedResult {
									Object = Source,
									ObjectType = FromType,
									PropertyName = "this",
									PropertyType = FromType,
									PropertyValue = Source
								},
								Destination = new PropertyChangedResult {
									Object = Destination,
									ObjectType = ToType,
									PropertyName = "this",
									PropertyType = ToType,
									PropertyValue = Destination
								}
							} );
						if ( MapDirection != MapDirection.DestinationToSource ) {
							Destination = null;
						} else {
							// Leave it be
						}
						return changes;
					}
					if ( Destination == null ) {
						changes.Add(
							new PropertyChangedResults {
								Source = new PropertyChangedResult {
									Object = Source,
									ObjectType = FromType,
									PropertyName = "this",
									PropertyType = FromType,
									PropertyValue = Source
								},
								Destination = new PropertyChangedResult {
									Object = Destination,
									ObjectType = ToType,
									PropertyName = "this",
									PropertyType = ToType,
									PropertyValue = Destination
								}
							} );
						Destination = (IList)Instantiator.CreateInstance( ToType );
					}
					break;

				case ExecutionType.Compare:
					if ( Source == null || Destination == null ) {
						changes.Add(
							new PropertyChangedResults {
								Source = new PropertyChangedResult {
									Object = Source,
									ObjectType = FromType,
									PropertyName = "this",
									PropertyType = FromType,
									PropertyValue = Source
								},
								Destination = new PropertyChangedResult {
									Object = Destination,
									ObjectType = ToType,
									PropertyName = "this",
									PropertyType = ToType,
									PropertyValue = Destination
								}
							} );
						return changes;
					}
					break;

				default:
					throw new ArgumentOutOfRangeException( "ExecutionType" );
			}


			// Copy source to validate there are no duplicates
			IList sourceCopy = new List<object>();
			foreach ( object s in Source ) {
				if ( sourceCopy.Contains( s ) ) {
					throw new MapFailureException( null, Source, s, MapFailureReason.DuplicateFromPrimaryKey, null );
				}
				sourceCopy.Add( s );
			}

			// Copy destination so I can keep track of which items were extra
			IList destinationCopy = new List<object>();
			foreach ( object d in Destination ) {
				if ( destinationCopy.Contains( d ) ) {
					throw new MapFailureException( null, Destination, d, MapFailureReason.DuplicateToPrimaryKey, null );
				}
				destinationCopy.Add( d );
			}


			foreach ( object from in sourceCopy ) {

				object to = null;
				try {
					to = TypeConvert.Convert( from, toInnerType );
				} catch ( Exception ex ) {
					throw new MapFailureException( null, Destination, from, MapFailureReason.ConvertTypeFailure, ex );
				}
				if ( !Destination.Contains( to ) ) {
					changes.Add(
						new PropertyChangedResults {
							Source = new PropertyChangedResult {
								Object = Source,
								ObjectType = FromType,
								PropertyName = "index",
								PropertyType = fromInnerType,
								PropertyValue = from
							},
							Destination = new PropertyChangedResult {
								Object = Destination,
								ObjectType = ToType,
								PropertyName = "index",
								PropertyType = toInnerType,
								PropertyValue = null // we created it
							}
						} );

					switch ( ExecutionType ) {
						case ExecutionType.Copy:
							Destination.Add( to );
							break;
						case ExecutionType.Compare:
							// We've already done so
							break;
						default:
							throw new ArgumentOutOfRangeException( "ExecutionType" );
					}
				} else {
					// It already exists, we're good
					destinationCopy.Remove( to );
				}

			}

			if ( MapDirection == MapDirection.SourceToDestination && destinationCopy.Count > 0 ) {
				// These existed in dest but don't exist in source
				// Remove them from destination
				foreach ( object destEntry in destinationCopy ) {
					changes.Add(
						new PropertyChangedResults {
							Source = new PropertyChangedResult {
								Object = Source,
								ObjectType = FromType,
								PropertyName = "index",
								PropertyType = fromInnerType,
								PropertyValue = null
							},
							Destination = new PropertyChangedResult {
								Object = Destination,
								ObjectType = ToType,
								PropertyName = "index",
								PropertyType = toInnerType,
								PropertyValue = destEntry
							}
						} );

					switch ( ExecutionType ) {
						case ExecutionType.Copy:
							Destination.Remove( destEntry );
							break;
						case ExecutionType.Compare:
							// We've already done so
							break;
						default:
							throw new ArgumentOutOfRangeException( "ExecutionType" );
					}
				}
			}


			return changes;
		}

		// Pass in FromType and ToType in case Source or Destination are null or a derived type
		// If List<class>
		public static List<PropertyChangedResults> CopyListOfClass( Type FromType, Type ToType, IList Source, ref IList Destination, MapDirection MapDirection, ExecutionType ExecutionType ) {

			List<PropertyChangedResults> changes = new List<PropertyChangedResults>();

			Type toInnerType = ToType.GetGenericBaseType();
			Type fromInnerType = FromType.GetGenericBaseType();

			if ( !toInnerType.IsClassType() ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.ListClassTypeToListNonClassType );
			}
			if ( !fromInnerType.IsClassType() ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.ListNonClassTypeToListClassType );
			}

			MapEntry map = MapEntryManager.GetMapEntry( FromType, ToType, MapDirection );
			if ( map == null ) {
				throw new MissingMapException( FromType, ToType );
			}

			List<PropertyInfo> sourcePrimaryKeys = null;
			List<PropertyInfo> destinationPrimaryKeys = null;
			switch ( MapDirection ) {
				case MapDirection.SourceToDestination:
					sourcePrimaryKeys = (
						from p in map.Properties
						select p.Source
						).ToList();
					destinationPrimaryKeys = (
						from p in map.Properties
						select p.Destination
						).ToList();
					break;
				case MapDirection.DestinationToSource:
					destinationPrimaryKeys = (
						from p in map.Properties
						select p.Source
						).ToList();
					sourcePrimaryKeys = (
						from p in map.Properties
						select p.Destination
						).ToList();
					break;
				default:
					throw new ArgumentOutOfRangeException( "MapDirection" );
			}
			if ( sourcePrimaryKeys == null || sourcePrimaryKeys.Count == 0 ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.MissingFromPrimaryKey );
			}
			if ( destinationPrimaryKeys == null || destinationPrimaryKeys.Count == 0 ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.MissingToPrimaryKey );
			}

			switch ( ExecutionType ) {
				case ExecutionType.Copy:
					if ( Source == null ) {
						changes.Add(
							new PropertyChangedResults {
								Source = new PropertyChangedResult {
									Object = Source,
									ObjectType = FromType,
									PropertyName = "this",
									PropertyType = FromType,
									PropertyValue = Source
								},
								Destination = new PropertyChangedResult {
									Object = Destination,
									ObjectType = ToType,
									PropertyName = "this",
									PropertyType = ToType,
									PropertyValue = Destination
								}
							} );
						if ( MapDirection != MapDirection.DestinationToSource ) {
							Destination = null;
						} else {
							// Leave it be
						}
						return changes;
					}
					if ( Destination == null ) {
						changes.Add(
							new PropertyChangedResults {
								Source = new PropertyChangedResult {
									Object = Source,
									ObjectType = FromType,
									PropertyName = "this",
									PropertyType = FromType,
									PropertyValue = Source
								},
								Destination = new PropertyChangedResult {
									Object = Destination,
									ObjectType = ToType,
									PropertyName = "this",
									PropertyType = ToType,
									PropertyValue = Destination
								}
							} );
						Destination = (IList)Instantiator.CreateInstance( ToType );
					}
					/* Easier, but defeats accurate tracking of changes
					if ( Source.Count == 0 ) {
						if ( MapDirection != MapDirection.DestinationToSource ) {
							Destination = (IList)Instantiator.CreateInstance( ToType ); // Easier than emptying the list
						} else {
							// Leave it be
						}
						return changes;
					}
					*/
					break;

				case ExecutionType.Compare:
					if ( Source == null || Destination == null ) {
						changes.Add(
							new PropertyChangedResults {
								Source = new PropertyChangedResult {
									Object = Source,
									ObjectType = FromType,
									PropertyName = "this",
									PropertyType = FromType,
									PropertyValue = Source
								},
								Destination = new PropertyChangedResult {
									Object = Destination,
									ObjectType = ToType,
									PropertyName = "this",
									PropertyType = ToType,
									PropertyValue = Destination
								}
							} );
						return changes;
					}
					break;

				default:
					throw new ArgumentOutOfRangeException( "ExecutionType" );
			}

			// Map source and destination objects to avoid reflecting against them too many times

			Dictionary<List<object>, object> sourceMap = MapListOfObject(
				Source, sourcePrimaryKeys, InvalidPropertyReason.FromPrimaryKeyBlank, MapFailureReason.DuplicateFromPrimaryKey, 
				( fromKey, index, sourcePrimaryKey ) => {
					object convertedKey = TypeConvert.Convert( fromKey, destinationPrimaryKeys[index].PropertyType );
					if ( convertedKey == null ) {
						throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.FromPrimaryKeyConversionFailure, sourcePrimaryKey );
					}
					return convertedKey;
				},
				FromType, ToType );

			Dictionary<List<object>, object> destinationMap = MapListOfObject( Destination, destinationPrimaryKeys, InvalidPropertyReason.ToPrimaryKeyBlank, MapFailureReason.DuplicateToPrimaryKey, ( keyObj, i, key ) => keyObj, FromType, ToType );


			foreach ( KeyValuePair<List<object>, object> fromEntry in sourceMap ) {

				// Get source object and key
				List<object> fromKey = fromEntry.Key;
				object from = fromEntry.Value;

				// Locate destination object
				var toEntry = destinationMap.Where(
					d => {
						for ( int i = 0; i < d.Key.Count; i++ ) {
							if ( !fromKey[i].Equals( d.Key[i] ) ) {
								return false;
							}
						}
						return true;
					} ).FirstOrDefault();
				object to = null;
				if ( toEntry.Key != null ) {
					// Found
					to = toEntry.Value;
				}

				// If destination object isn't found, create one
				if ( to == null ) {
					changes.Add(
						new PropertyChangedResults {
							Source = new PropertyChangedResult {
								Object = Source,
								ObjectType = FromType,
								PropertyName = "index",
								PropertyType = fromInnerType,
								PropertyValue = from
							},
							Destination = new PropertyChangedResult {
								Object = Destination,
								ObjectType = ToType,
								PropertyName = "index",
								PropertyType = toInnerType,
								PropertyValue = null // We created it, it didn't exist yet
							}
						} );

					switch ( ExecutionType ) {
						case ExecutionType.Copy:
							to = Instantiator.CreateInstance( toInnerType );
							Destination.Add( to );
							break;
						case ExecutionType.Compare:
							// We've already done so
							break;
						default:
							throw new ArgumentOutOfRangeException( "ExecutionType" );
					}
				}

				// If we're in compare mode and we just noticed dest is missing but didn't create one, we're good
				List<PropertyChangedResults> changeListStep = null;
				if ( to != null ) {
					// Map the source object to the destination object
					changeListStep = PropertyMapper.CopyProperties( fromInnerType, toInnerType, from, ref to, MapDirection, ExecutionType );
				}

				if ( toEntry.Key != null ) {
					// If we created it, "created it" is all that we need to document for changes
					// That we set every property (recursively) to match the original is irrelevant
					if ( changeListStep != null && changeListStep.Count > 0 ) {
						changes.AddRange( changeListStep );
					}
					// We've successfully mapped this one
					// Remove it from the "to map" list
					// so it'll be faster to map subsequent entries
					// and so we know which are in Destination but not in Source
					destinationMap.Remove( toEntry.Key );
				}

			}

			if ( MapDirection == MapDirection.SourceToDestination && destinationMap.Count > 0 ) {
				// These existed in dest but don't exist in source
				// Remove them from destination
				foreach ( KeyValuePair<List<object>, object> destEntry in destinationMap ) {
					changes.Add(
						new PropertyChangedResults {
							Source = new PropertyChangedResult {
								Object = Source,
								ObjectType = FromType,
								PropertyName = "index",
								PropertyType = fromInnerType,
								PropertyValue = null
							},
							Destination = new PropertyChangedResult {
								Object = Destination,
								ObjectType = ToType,
								PropertyName = "index",
								PropertyType = toInnerType,
								PropertyValue = destEntry.Value
							}
						} );

					switch ( ExecutionType ) {
						case ExecutionType.Copy:
							Destination.Remove( destEntry.Value );
							break;
						case ExecutionType.Compare:
							// We've already done so
							break;
						default:
							throw new ArgumentOutOfRangeException( "ExecutionType" );
					}
				}
			}

			return changes;
		}

		private static Dictionary<List<object>, object> MapListOfObject( IList Source, List<PropertyInfo> PrimaryKeys, InvalidPropertyReason KeyBlankReason, MapFailureReason DuplicateReason, 
			Func<object, int, PropertyInfo, object> ConvertKeyFunc, Type FromType, Type ToType ) {

			Dictionary<List<object>, object> results = new Dictionary<List<object>, object>();

			if ( Source != null && Source.Count > 0 ) {
				foreach ( object from in Source ) {

					List<object> fromKeys = new List<object>();

					for ( int i = 0; i < PrimaryKeys.Count; i++ ) {
						PropertyInfo primaryKey = PrimaryKeys[i];
						object keyObj = null;
						try {
							keyObj = primaryKey.GetValue( from, null );
						} catch ( Exception ex ) {
							throw new MapFailureException( primaryKey, Source, from, MapFailureReason.GetSourceFailure, ex );
						}
						if ( keyObj == null ) {
							throw new InvalidTypeConversionException( FromType, ToType, KeyBlankReason, primaryKey );
						}
						// Convert to destination primary key type
						object convertedKey = ConvertKeyFunc( keyObj, i, primaryKey );
						fromKeys.Add( convertedKey );
					}

					// Does it already exist?
					if ( results.Where(
						d => {
							for ( int i = 0; i < d.Key.Count; i++ ) {
								if ( !fromKeys[i].Equals( d.Key[i] ) ) {
									return false;
								}
							}
							return true;
						} ).Any() ) {
						throw new MapFailureException( PrimaryKeys[0], Source, from, DuplicateReason, null );
					}

					// It's unique, add it
					results.Add( fromKeys, from );

				}
			}

			return results;
		}

	}

}
