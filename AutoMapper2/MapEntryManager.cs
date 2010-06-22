namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
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
		public static MapEntry GetMapEntry(Type FromType, Type ToType, MapDirection MapDirection) {
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
			Type from = typeof( From );
			Type to = typeof( To );
			if ( from.IsListOfT() != to.IsListOfT() ) {
				if ( from.IsListOfT() ) {
					throw new InvalidTypeConversionException( from, to, InvalidPropertyReason.ListTypeToNonListType );
				}
				if ( to.IsListOfT() ) {
					throw new InvalidTypeConversionException( from, to, InvalidPropertyReason.NonListTypeToListType );
				}
			}
			AssertTypeClassesCanMap( from, to );
			if ( from.IsListOfT() ) {
				Type fromBase = from.GetGenericBaseType();
				Type toBase = to.GetGenericBaseType(); // If it isn't a Generic, the above would've caught it
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

		public static void AssertConfigurationIsValid() {
			if ( mapList == null || mapList.Count == 0 ) {
				throw new ArgumentNullException( "", "You've not created any maps" );
			}
			foreach ( MapEntry m in mapList ) {
				// This flexes the "null to null" case, which is pretty much not helpful
				// But it does flex MapBuilder.BuildMap(m), which is quite helpful
				object source = Activator.CreateInstance( m.From );
				object destination = Activator.CreateInstance( m.To );
				AutoMapper2.Map( source, ref destination ); // If this errors, it should tell you why
			}
		}

	}

}
