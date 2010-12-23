namespace MapDLib {

	#region using
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using System.Text.RegularExpressions;

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

		public static void CreateMapsFromAssembly( Assembly Assembly ) {

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

		public static void CreateMapsFromAllLoadedAssemblies() {
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			if ( assemblies != null && assemblies.Length > 0 ) {
				foreach ( Assembly assembly in assemblies ) {
#if NET_4
					if ( assembly.IsDynamic ) {
						continue; // Can't reflect on dynamic-only assemblies
					}
#endif
					CreateMapsFromAssembly( assembly );
				}
			}
		}

		#region CreateMapsFromAllAssembliesInPath
		public static void CreateMapsFromAllAssembliesInPath( string PathString, string FileFilterRegex = null ) {

			if ( string.IsNullOrEmpty( PathString ) ) {
				// Assume current directory
				string exeName = Assembly.GetExecutingAssembly().GetName().CodeBase.Substring( 8 ); // Get past file://
				PathString = Path.GetDirectoryName( exeName );
			}

			Regex filter = null;
			if ( !string.IsNullOrEmpty( FileFilterRegex ) ) {
				filter = new Regex( FileFilterRegex, RegexOptions.IgnoreCase );
			}

			DirectoryInfo dir = new DirectoryInfo( PathString ); // If you have invalid chars in your path, it'll blow here
			if ( !dir.Exists ) {
				throw new FileNotFoundException( "Can't load maps because the source doesn't exist: " + PathString );
			}

			FileInfo[] files = dir.GetFiles();
			List<FileInfo> filesToLoad = new List<FileInfo>();
			foreach ( FileInfo file in files ) {

				if ( !file.Exists ) {
					continue;
				}

				if ( !string.Equals( file.Extension, ".dll", StringComparison.CurrentCultureIgnoreCase ) 
					&& !string.Equals( file.Extension, ".exe", StringComparison.CurrentCultureIgnoreCase ) ) {
					continue; // It isn't an assembly
				}

				if ( filter != null && !filter.IsMatch( file.Name ) ) {
					continue; // Not a match
				}
				
				filesToLoad.Add( file );
			}

			if ( filesToLoad.Count == 0 ) {
				return; // Nothing to do
			}


			AppDomain tempDomain = null;
			try {

				tempDomain = AppDomain.CreateDomain(
					"MapDTempDomain",
					AppDomain.CurrentDomain.Evidence,
					AppDomain.CurrentDomain.SetupInformation
				);

				List<Assembly> assemblies = new List<Assembly>();

				foreach ( FileInfo file in filesToLoad ) {
					Assembly assembly = tempDomain.Load( new AssemblyName { CodeBase = file.FullName } );
#if NET_4
					if ( assembly.IsDynamic ) {
						continue; // Can't reflect on dynamic-only assemblies
					}
#endif
					assemblies.Add( assembly );
				}

				foreach ( Assembly assembly in assemblies ) {
					CreateMapsFromAssembly( assembly );
				}

			} finally {
				if ( tempDomain != null ) {
					try {
						AppDomain.Unload( tempDomain );
					} catch {
					}
				}
			}

		}
		#endregion

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
		public static void AssertTypesCanMap( Type From, Type To ) {
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

				object source = Instantiator.CreateInstance( m.From );
				object destination = Instantiator.CreateInstance( m.To );

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
					IList sourceList = (IList)Instantiator.CreateInstance( sourceType );
					IList destinationList = (IList)Instantiator.CreateInstance( destinationType );
					Type sourceBaseType = sourceType.GetGenericBaseType();
					Type destinationBaseType = destinationType.GetGenericBaseType();
					// Add an object to the list
					object sourceChild = Instantiator.CreateInstance( sourceBaseType );
					sourceList.Add( sourceChild );
					object destinationChild = Instantiator.CreateInstance( destinationBaseType );
					destinationList.Add( destinationChild );
					if ( sourceBaseType.IsClassType() || destinationBaseType.IsClassType() ) {
						// Fill the child with default values
						FillObjectWithDefaults( sourceBaseType, destinationBaseType, sourceChild, destinationChild ); // Recurse to populate this object
					}
					sourceValue = sourceList;
					destinationValue = destinationList;
				} else if ( sourceType.IsClassType() ) {
					MapEntryManager.AssertTypesCanMap( sourceType, destinationType );
					sourceValue = Instantiator.CreateInstance( sourceType );
					destinationValue = Instantiator.CreateInstance( destinationType );
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
