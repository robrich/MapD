namespace MapDLib {

	#region using
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	#endregion

	internal class MapEntryManager {

		private static List<MapEntry> mapList = new List<MapEntry>();

		public static void AddMapEntry( Type From, Type To ) {
			if ( mapList.Any( mm => mm.From == From && mm.To == To ) ) {
				return; // Already got one
			}
			MapEntry m = new MapEntry {
				From = From,
				To = To
			};
			mapList.Add( m );

			if ( m.From.IsListOfT() && m.To.IsListOfT() ) {
				// List<T> to List<U> auto-creates T to U
				Type fromBase = m.From.GetGenericBaseType();
				Type toBase = m.To.GetGenericBaseType(); // If it isn't a Generic, the above would've caught it
				if ( fromBase.IsClassType() && toBase.IsClassType() ) {
					AddMapEntry( fromBase, toBase );
				}
			}
		}

		public static void CreateMaps( Assembly Assembly ) {

			Type[] types = Assembly.GetExportedTypes();
			foreach ( Type toType in types ) {

				if ( !toType.IsClass || toType.IsNotPublic ) {
					continue;
				}

				Type fromType = toType.GetMapFromType();
				if ( fromType != null ) {
					AddMapEntry( fromType, toType );
				}

				fromType = toType.GetMapListFromType();
				if ( fromType != null ) {
					AddMapEntry( fromType.GetListOfType(), toType.GetListOfType() );
				}

			}

		}

		public static void ResetMap() {
			mapList.Clear();
		}

		#region GetMapEntry
		private static MapEntry GetMapEntry( Type FromType, Type ToType ) {

			MapEntry m = (
				from mm in mapList
				where mm.From == FromType
					&& mm.To == ToType
				select mm
				).FirstOrDefault();

			if ( m == null ) {
				throw new MissingMapException( FromType, ToType );
			}

			if ( m.Properties == null ) {
				MapBuilder.BuildMap( m );
			}

			if ( m.From.IsListOfT() ) {
				Type fromBase = m.From.GetGenericBaseType();
				Type toBase = m.To.GetGenericBaseType();
				if ( fromBase.IsClassType() && toBase.IsClassType() ) {
					GetMapEntry( fromBase, toBase ); // Recurse
				}
			}

			// You should load your map before using any of your map,
			// so I can now check properties and base classes
			// to insure they're mapped too

			if ( m.Properties != null && m.Properties.Count > 0 ) {
				foreach ( var prop in m.Properties ) {
					if ( prop.Source.IsListOfT() && prop.Destination.IsListOfT() ) {
						// No need for a map entry of List<> to List<> -- we know how to map lists
						Type sourceBase = prop.Source.PropertyType.GetGenericBaseType();
						Type destBase = prop.Destination.PropertyType.GetGenericBaseType();
						if ( sourceBase.IsClassType() && destBase.IsClassType() ) {
							GetMapEntry( sourceBase, destBase ); // Recurse
						}
					} else if ( prop.Source.PropertyType.IsClassType() && prop.Destination.PropertyType.IsClassType() ) {
						GetMapEntry( prop.Source.PropertyType, prop.Destination.PropertyType ); // Recurse
					}
				}
			}

			return m;
		}
		public static MapEntry GetMapEntry( Type FromType, Type ToType, MapDirection MapDirection ) {
			MapEntry map = null;
			switch ( MapDirection ) {
				case MapDirection.SourceToDestination:
					map = MapEntryManager.GetMapEntry( FromType, ToType );
					break;
				case MapDirection.DestinationToSource:
					map = MapEntryManager.GetMapEntry( ToType, FromType );
					break;
				default:
					throw new ArgumentOutOfRangeException( "MapDirection" );
			}
			return map;
		}
		#endregion

		// Doesn't check recursively, that happens on property load
		#region AssertTypesCanMap
		public static void AssertTypesCanMap<From, To>() {
			Type from = typeof(From);
			Type to = typeof(To);
			AssertTypesCanMap(from, to);
		}
		public static void AssertTypesCanMap(Type From, Type To) {
			if ( From.IsListOfT() != To.IsListOfT() ) {
				if ( From.IsListOfT() ) {
					throw new InvalidTypeConversionException( From, To, InvalidPropertyReason.ListTypeToNonListType );
				}
				if ( To.IsListOfT() ) {
					throw new InvalidTypeConversionException( From, To, InvalidPropertyReason.NonListTypeToListType );
				}
			}
			AssertTypeClassesCanMap( From, To );
			if ( From.IsListOfT() ) {
				Type fromBase = From.GetGenericBaseType();
				Type toBase = To.GetGenericBaseType(); // If it isn't a Generic, the above would've caught it
				AssertTypeClassesCanMap( fromBase, toBase );
			}
		}
		public static void AssertTypeClassesCanMap( Type from, Type to ) {
			if ( from.IsClassType() != to.IsClassType() ) {
				if ( from.IsClassType() ) {
					throw new InvalidTypeConversionException( from, to, InvalidPropertyReason.ClassTypeToNonClassType );
				}
				if ( to.IsClassType() ) {
					throw new InvalidTypeConversionException( from, to, InvalidPropertyReason.NonClassTypeToClassType );
				}
			}
		}
		#endregion

		#region AssertConfigurationIsValid
		public static void AssertConfigurationIsValid() {
			if ( mapList == null || mapList.Count == 0 ) {
				throw new ArgumentNullException( "", "You haven't created any maps" );
			}
			foreach ( MapEntry m in mapList ) {
				// This flexes the "null to null" case, which is pretty much not helpful
				// But it does flex MapBuilder.BuildMap(m), which is quite helpful

				object source = Activator.CreateInstance( m.From );
				object destination = Activator.CreateInstance( m.To );

				// Populate class and list properties so copying will recurse as needed
				FillObjectWithDefaults( m.From, m.To, source, destination );

				// Can't use MapD.Copy<>() because source and destination aren't strongly typed
				MapD.ExecuteMap( m.From, m.To, source, ref destination, MapDirection.SourceToDestination, ExecutionType.Copy ); // If this errors, it should tell you why
				MapD.ExecuteMap( m.From, m.To, source, ref destination, MapDirection.DestinationToSource, ExecutionType.Copy ); // If this errors, it should tell you why
			}
		}

		// Recursive Helper method for AssertConfigurationIsValid
		private static void FillObjectWithDefaults( Type FromType, Type ToType, object Source, object Destination ) {

			MapEntryManager.AssertTypesCanMap(FromType, ToType);
			MapEntry map = MapEntryManager.GetMapEntry( FromType, ToType, MapDirection.SourceToDestination );
			if ( map == null ) {
				throw new MissingMapException( FromType, ToType );
			}
			
			foreach ( MapEntryProperty mapEntryProperty in map.Properties ) {

				PropertyInfo sourceInfo = mapEntryProperty.Source;
				Type sourceType = sourceInfo.PropertyType;
				object sourceValue = null;

				PropertyInfo destinationInfo = mapEntryProperty.Destination;
				Type destinationType = destinationInfo.PropertyType;
				object destinationValue = null;


				if ( sourceType.IsListOfT() || destinationType.IsListOfT() ) {
					MapEntryManager.AssertTypesCanMap( sourceType, destinationType );
					IList sourceList = (IList)Activator.CreateInstance( sourceType );
					IList destinationList = (IList)Activator.CreateInstance( destinationType );
					Type sourceBaseType = sourceType.GetGenericBaseType();
					Type destinationBaseType = destinationType.GetGenericBaseType();
					// Add an object to the list
					object sourceChild = Activator.CreateInstance( sourceBaseType );
					sourceList.Add( sourceChild );
					object destinationChild = Activator.CreateInstance( destinationBaseType );
					destinationList.Add( destinationChild );
					if ( sourceBaseType.IsClassType() || destinationBaseType.IsClassType() ) {
						// Fill the child with default values
						FillObjectWithDefaults( sourceBaseType, destinationBaseType, sourceChild, destinationChild ); // Recurse to populate this object
					}
					sourceValue = sourceList;
					destinationValue = destinationList;
				} else if ( sourceType.IsClassType() ) {
					MapEntryManager.AssertTypesCanMap( sourceType, destinationType );
					sourceValue = Activator.CreateInstance( sourceType );
					destinationValue = Activator.CreateInstance( destinationType );
					FillObjectWithDefaults( sourceType, destinationType, sourceValue, destinationValue ); // Recurse to populate this object
				} else {
					// Creating the object set it to the appropriate default value
					// default( propertyType ) only works with generic<T> type code
					sourceValue = null;
					destinationValue = null;
				}

				if ( sourceValue != null ) {
					sourceInfo.SetValue( Source, sourceValue, null );
				}
				if ( destinationValue != null ) {
					destinationInfo.SetValue( Destination, destinationValue, null );
				}

			}
		}
		#endregion

		#region MapCount - for Assert.MapCount
		public static int MapCount {
			get { return mapList == null ? 0 : mapList.Count; }
		}
		#endregion

	}

}
