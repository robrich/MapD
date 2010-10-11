namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	#endregion

	internal class ListMapper {

		// Pass in FromType and ToType in case Source or Destination are null or a derived type
		// No need for direction since the logic would be the same either way
		// If List<Non-class>
		public static List<PropertyChangedResults> CopyListOfNonClass( Type FromType, Type ToType, IList Source, ref IList Destination, MapDirection MapDirection ) {

			List<PropertyChangedResults> changes = new List<PropertyChangedResults>();

			Type toInnerType = ToType.GetGenericBaseType();
			Type fromInnerType = FromType.GetGenericBaseType();

			if ( toInnerType.IsClassType() ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.ListNonClassTypeToListClassType );
			}
			if ( fromInnerType.IsClassType() ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.ListClassTypeToListNonClassType );
			}

			if ( Source == null ) {
				if ( MapDirection != MapDirection.DestinationToSource ) {
					Destination = null;
				} else {
					// Leave it be
				}
				return changes;
			}
			if ( Destination == null ) {
				Destination = (IList)Activator.CreateInstance( ToType );
			}
			if ( Source.Count == 0 ) {
				return changes;
			}

			foreach ( object from in Source ) {

				object to = null;
				try {
					to = TypeConvert.Convert( from, toInnerType );
				} catch ( Exception ex ) {
					throw new MapFailureException( null, Destination, from, MapFailureReason.ConvertTypeFailure, ex );
				}
				if ( !Destination.Contains( to ) ) {
					changes.Add(
						new PropertyChangedResults {
							NewValue = TypeConvert.ConvertToString( to, toInnerType ),
							Object = Destination,
							OldValue = null,
							ObjectType = toInnerType,
							PropertyName = "this",
							PropertyType = fromInnerType
						} );
					Destination.Add( to );
				}

			}

			// TODO: Check for stuff in destinationPropertyValue not in sourcePropertyValue and remove them?

			return changes;
		}

		// Pass in FromType and ToType in case Source or Destination are null or a derived type
		// If List<class>
		public static List<PropertyChangedResults> CopyListOfClass( Type FromType, Type ToType, IList Source, ref IList Destination, MapDirection MapDirection ) {

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

			if ( Source == null ) {
				if ( MapDirection != MapDirection.DestinationToSource ) {
					Destination = null;
				} else {
					// Leave it be
				}
				return changes;
			}
			if ( Destination == null ) {
				Destination = (IList)Activator.CreateInstance( ToType );
			}
			if ( Source.Count == 0 ) {
				if ( MapDirection != MapDirection.DestinationToSource ) {
					Destination = (IList)Activator.CreateInstance( ToType ); // Easier than emptying the list
				} else {
					// Leave it be
				}
				return changes;
			}


			// Map source and destination objects to avoid reflecting against them too many times
			Dictionary<List<object>, object> sourceMap = new Dictionary<List<object>, object>();
			Dictionary<List<object>, object> destinationMap = new Dictionary<List<object>, object>();

			// Map source list
			foreach ( object from in Source ) {

				List<object> fromKeys = new List<object>();

				for ( int i = 0; i < sourcePrimaryKeys.Count; i++ ) {
					PropertyInfo sourcePrimaryKey = sourcePrimaryKeys[i];
					object fromKey = null;
					try {
						fromKey = sourcePrimaryKey.GetValue( from, null );
					} catch ( Exception ex ) {
						throw new MapFailureException( sourcePrimaryKey, from, null, MapFailureReason.GetSourceFailure, ex );
					}
					if ( fromKey == null ) {
						throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.FromPrimaryKeyBlank, sourcePrimaryKey );
					}
					// Convert to destination primary key type
					object toKey = TypeConvert.Convert( fromKey, destinationPrimaryKeys[i].PropertyType );
					if ( toKey == null ) {
						throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.FromPrimaryKeyConversionFailure, sourcePrimaryKey );
					}
					fromKeys.Add( toKey );
				}

				// Does it already exist?
				if ( sourceMap.Where(
					d => {
						for ( int i = 0; i < d.Key.Count; i++ ) {
							if ( !fromKeys[i].Equals( d.Key[i] ) ) {
								return false;
							}
						}
						return true;
					} ).Any() ) {
					throw new MapFailureException( sourcePrimaryKeys[0], from, null, MapFailureReason.DuplicateFromPrimaryKey, null );
				}

				// It's unique, add it
				sourceMap.Add( fromKeys, from );

			}

			if ( Destination != null && Destination.Count > 0 ) {
				// Map destination list
				foreach ( object to in Destination ) {

					List<object> toKeys = new List<object>();

					foreach ( PropertyInfo destinationPrimaryKey in destinationPrimaryKeys ) {
						object toKey = null;
						try {
							toKey = destinationPrimaryKey.GetValue( to, null );
						} catch ( Exception ex ) {
							throw new MapFailureException( destinationPrimaryKey, to, null, MapFailureReason.GetDestinationFailure, ex );
						}
						if ( toKey == null ) {
							throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.ToPrimaryKeyBlank, destinationPrimaryKey );
						}
						toKeys.Add( toKey );
					}

					// Does it already exist?
					if ( destinationMap.Where(
						d => {
							for ( int i = 0; i < d.Key.Count; i++ ) {
								if ( !toKeys[i].Equals( d.Key[i] ) ) {
									return false;
								}
							}
							return true;
						} ).Any() ) {
						throw new MapFailureException( destinationPrimaryKeys[0], to, null, MapFailureReason.DuplicateToPrimaryKey, null );
					}

					// It's unique, add it
					destinationMap.Add( toKeys, to );

				}
			}


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
					to = Activator.CreateInstance( toInnerType );
					changes.Add(
						new PropertyChangedResults {
							NewValue = TypeConvert.ConvertToString( to, toInnerType ),
							Object = Destination,
							OldValue = null,
							ObjectType = toInnerType,
							PropertyName = "this",
							PropertyType = fromInnerType
						} );
					Destination.Add( to );
				}

				// Map the source object to the destination object
				List<PropertyChangedResults> changeListStep = PropertyMapper.CopyProperties( fromInnerType, toInnerType, from, ref to, MapDirection );
				if ( changeListStep != null && changeListStep.Count > 0 ) {
					changes.AddRange( changeListStep );
				}

				if ( toEntry.Key != null ) {
					// We've successfully mapped this one
					// Remove it from the list
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
							NewValue = null,
							Object = Destination,
							OldValue = TypeConvert.ConvertToString( destEntry.Value, toInnerType ),
							ObjectType = toInnerType,
							PropertyName = "this",
							PropertyType = fromInnerType
						} );
					Destination.Remove( destEntry.Value );
				}
			}

			return changes;
		}

	}

}
