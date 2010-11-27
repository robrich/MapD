namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;

	#endregion

	public static class AutoMapper2 {

		public static class Config {

			// Properties are enumerated when you call Map<> the first time, therefore you can't change this property after that
			private static bool mappingPropertiesLocked;

			private static bool excludeLinqProperties;
			public static bool ExcludeLinqProperties {
				get { return excludeLinqProperties; }
				set {
					if ( mappingPropertiesLocked ) {
						throw new NotSupportedException( "Once you call CreateMap(), you can't change ExcludeLinqProperties because the properties have already been mapped" );
					}
					excludeLinqProperties = value;
				}
			}

			private static PropertyIs ignorePropertiesIf;
			public static PropertyIs IgnorePropertiesIf {
				get { return ignorePropertiesIf; }
				set {
					if ( mappingPropertiesLocked ) {
						throw new NotSupportedException( "Once you call CreateMap(), you can't change IgnorePropertiesIf because the properties have already been mapped" );
					}
					ignorePropertiesIf = value;
				}
			}

			static Config() {
				ResetMap();
			}

			/// <summary>
			/// Reset the AutoMapper -- for Unit Tests and initialization
			/// </summary>
			public static void ResetMap() {
				mappingPropertiesLocked = false;
				ExcludeLinqProperties = true;
				IgnorePropertiesIf = PropertyIs.NotSet;
				MapEntryManager.ResetMap();
			}

			public static void CreateMap<From, To>()
				where From : class, new()
				where To : class, new() {
				Type fromType = typeof( From );
				Type toType = typeof( To );
				MapEntryManager.AssertTypesCanMap<From, To>();
				MapEntryManager.AddMapEntry( fromType, toType );

				/*
				 * Though technically not "locked" until you .Map<>() or .MapBack<>(),
				 * we lock it here to prevent stuff like:
				 * .CreateMap<T,U>();
				 * .ExcludeLinqProperties = true;
				 * .CreateMap<T,U>();
				 * .ExcludeLinqProperties = false;
				 * which would produce unexpected results
				 */
				mappingPropertiesLocked = true;
			}

			/// <summary>
			/// Reflect on the current assembly to get the maps from all public <see cref="MapFromAttribute"/>-annotated classes and all public <see cref="MapListFromListOfAttribute"/>-annotated classes
			/// </summary>
			public static void CreateMaps() {
				CreateMaps( Assembly.GetCallingAssembly() );
			}

			/// <summary>
			/// Reflect on all loaded assemblies to get the maps from all public <see cref="MapFromAttribute"/>-annotated classes and all public <see cref="MapListFromListOfAttribute"/>-annotated classes
			/// </summary>
			public static void CreateAllMaps() {
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				if ( assemblies != null && assemblies.Length > 0 ) {
					foreach ( Assembly assembly in assemblies ) {
#if NET_4
						if ( assembly.IsDynamic ) {
							continue; // Can't reflect on dynamic-only assemblies
						}
#endif
						CreateMaps( assembly );
					}
				}
			}

			/// <summary>
			/// Reflect on the specified assembly to get the maps from all public <see cref="MapFromAttribute"/>-annotated classes and all public <see cref="MapListFromListOfAttribute"/>-annotated classes
			/// </summary>
			/// <param name="Assembly"></param>
			public static void CreateMaps( Assembly Assembly ) {
				if ( Assembly == null ) {
					Assembly = Assembly.GetCallingAssembly();
					if ( Assembly == null ) {
						throw new ArgumentNullException( "Assembly" );
					}
				}

				MapEntryManager.CreateMaps( Assembly );
			}

		}

		public static class Assert {

			public static void AssertConfigurationIsValid() {
				MapEntryManager.AssertConfigurationIsValid();
			}
			public static int MapCount {
				get { return MapEntryManager.MapCount; }
			}

		}

		public static To Map<From, To>( From Source )
			where From : class, new()
			where To : class, new() {

			Type fromType = typeof( From );
			Type toType = typeof( To );
			MapEntryManager.AssertTypesCanMap<From, To>();

			if ( Source == null ) {
				return null;
			}
			To destination = (To)Activator.CreateInstance( typeof( To ) );
			List<PropertyChangedResults> changes = Map<From, To>( Source, ref destination );
			return destination;
		}

		public static List<PropertyChangedResults> Map<From, To>( From Source, ref To Destination )
			where From : class, new()
			where To : class, new() {

			List<PropertyChangedResults> changes = new List<PropertyChangedResults>();

			Type fromType = typeof( From );
			Type toType = typeof( To );
			MapEntryManager.AssertTypesCanMap<From, To>();

			if ( Source == null ) {
				if ( Destination != null ) {
					changes.Add(
						new PropertyChangedResults {
							Source = new PropertyChangedResult {
								Object = Source,
								ObjectType = fromType,
								PropertyName = "this",
								PropertyType = fromType,
								PropertyValue = Source
							},
							Destination = new PropertyChangedResult {
								Object = Destination,
								ObjectType = toType,
								PropertyName = "this",
								PropertyType = toType,
								PropertyValue = Destination
							}
						} );
					Destination = null;
				}
				return changes;
			}
			if ( Destination == null ) {
				changes.Add(
					new PropertyChangedResults {
						Source = new PropertyChangedResult {
							Object = Source,
							ObjectType = fromType,
							PropertyName = "this",
							PropertyType = fromType,
							PropertyValue = Source
						},
						Destination = new PropertyChangedResult {
							Object = Destination,
							ObjectType = toType,
							PropertyName = "this",
							PropertyType = toType,
							PropertyValue = Destination
						}
					} );
				Destination = (To)Activator.CreateInstance( typeof( To ) );
			}

			object destinationObject = Destination;
			List<PropertyChangedResults> changesStep = ExecuteMap( fromType, toType, Source, ref destinationObject, MapDirection.SourceToDestination, ExecutionType.Copy );
			changes.AddRange( changesStep );
			return changes;
		}

		public static List<PropertyChangedResults> MapBack<From, To>( To Source, ref From Destination )
			where From : class, new()
			where To : class, new() {

			// In MapBack, "fromType" is type of From, which is the Destination
			// Therefore calling *Mapper.Copy*() passes in toType first

			List<PropertyChangedResults> changes = new List<PropertyChangedResults>();

			Type fromType = typeof( From );
			Type toType = typeof( To );
			MapEntryManager.AssertTypesCanMap<From, To>();

			if ( Source == null ) {
				if ( Destination != null ) {
					changes.Add(
						new PropertyChangedResults {
							Source = new PropertyChangedResult {
								Object = Source,
								ObjectType = toType,
								PropertyName = "this",
								PropertyType = toType,
								PropertyValue = Source
							},
							Destination = new PropertyChangedResult {
								Object = Destination,
								ObjectType = fromType,
								PropertyName = "this",
								PropertyType = fromType,
								PropertyValue = Destination
							}
						} );
				}
				// Leave Destination alone
				return changes;
			}
			if ( Destination == null ) {
				changes.Add(
					new PropertyChangedResults {
						Source = new PropertyChangedResult {
							Object = Source,
							ObjectType = toType,
							PropertyName = "this",
							PropertyType = toType,
							PropertyValue = Source
						},
						Destination = new PropertyChangedResult {
							Object = Destination,
							ObjectType = fromType,
							PropertyName = "this",
							PropertyType = fromType,
							PropertyValue = Destination
						}
					} );
				Destination = (From)Activator.CreateInstance( typeof( From ) );
			}

			object destinationObject = Destination;
			List<PropertyChangedResults> changesStep = ExecuteMap( toType, fromType, Source, ref destinationObject, MapDirection.DestinationToSource, ExecutionType.Copy );
			changes.AddRange( changesStep );
			return changes;
		}

		public static List<PropertyChangedResults> Compare<From, To>( From Source, To Destination )
			where From : class, new()
			where To : class, new() {


			Type fromType = typeof(From);
			Type toType = typeof(To);
			MapEntryManager.AssertTypesCanMap<From, To>();

			if ( Source == null && Destination == null ) {
				return new List<PropertyChangedResults>(); // Both are null, no change
			} else if ( Source == null || Destination == null ) {
				// One is null, the other isn't
				return new List<PropertyChangedResults> {
					new PropertyChangedResults {
						Source = new PropertyChangedResult {
							Object = Source,
							ObjectType = fromType,
							PropertyName = "this",
							PropertyType = fromType,
							PropertyValue = Source
						},
						Destination = new PropertyChangedResult {
							Object = Destination,
							ObjectType = toType,
							PropertyName = "this",
							PropertyType = toType,
							PropertyValue = Destination
						}
					}
				};
			}
			// Both aren't null

			object destinationObject = Destination;
			return ExecuteMap( fromType, toType, Source, ref destinationObject, MapDirection.SourceToDestination, ExecutionType.Compare );
		}

		internal static List<PropertyChangedResults> ExecuteMap( Type FromType, Type ToType, object Source, ref object Destination, MapDirection MapDirection, ExecutionType ExecutionType ) {
			List<PropertyChangedResults> changes = null;
			if ( FromType.IsListOfT() ) {
				IList sourceList = Source as IList;
				IList destinationList = Destination as IList;
				// the below won't change what destination points to out from under Destination because Destination isn't null
				// FRAGILE: Only check source property, assuming dest property is also the same .IsClassType()
				if ( FromType.GetGenericBaseType().IsClassType() ) {
					changes = ListMapper.CopyListOfClass( FromType, ToType, sourceList, ref destinationList, MapDirection, ExecutionType );
				} else {
					changes = ListMapper.CopyListOfNonClass( FromType, ToType, sourceList, ref destinationList, MapDirection, ExecutionType );
				}
			} else if ( FromType.IsClassType() ) {
				changes = PropertyMapper.CopyProperties( FromType, ToType, Source, ref Destination, MapDirection, ExecutionType );
			} else {
				throw new NotSupportedException( "Can't map value types here" ); // This should be a compile-time error too
			}

			return changes;
		}

		/// <summary>
		/// For non-class types, convert type from/to
		/// </summary>
		public static To MapType<From, To>( From Source ) {
			Type fromType = typeof( From );
			Type toType = typeof( To );
			MapEntryManager.AssertTypesCanMap<From, To>();
			if ( fromType.IsClassType() ) {
				throw new NotSupportedException( "Can't call this with a class type, call Map() instead" );
			}
			return (To)TypeConvert.Convert( Source, toType );
		}

	}

}