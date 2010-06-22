namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Text;
	#endregion

	public static class AutoMapper2 {

		// Properties are enumerated when you call Map<> the first time, therefore you can't change this property after that
		private static bool excludeLinqPropertiesLocked;
		private static bool excludeLinqProperties;
		public static bool ExcludeLinqProperties {
			get { return excludeLinqProperties; }
			set {
				if ( excludeLinqPropertiesLocked ) {
					throw new NotSupportedException( "Once you call CreateMap(), you can't change ExcludeLinqProperties because the properties have already been mapped" );
				}
				excludeLinqProperties = value;
			}
		}

		static AutoMapper2() {
			ResetMap();
		}

		/// <summary>
		/// Reset the AutoMapper -- for Unit Tests and initialization
		/// </summary>
		public static void ResetMap() {
			excludeLinqPropertiesLocked = false;
			ExcludeLinqProperties = true;
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
			excludeLinqPropertiesLocked = true;
		}

		public static void AssertConfigurationIsValid() {
			MapEntryManager.AssertConfigurationIsValid();
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
			List<PropertyChanged> changes = Map<From, To>( Source, ref destination );
			return destination;
		}

		public static List<PropertyChanged> Map<From, To>( From Source, ref To Destination )
			where From : class, new()
			where To : class, new() {


			Type fromType = typeof( From );
			Type toType = typeof( To );
			MapEntryManager.AssertTypesCanMap<From, To>();

			if ( Source == null ) {
				Destination = null;
				return null;
			}
			if ( Destination == null ) {
				Destination = (To)Activator.CreateInstance( typeof( To ) );
			}

			object destinationObject = Destination;
			return ExecuteMap( fromType, toType, Source, ref destinationObject, MapDirection.SourceToDestination );
		}

		public static List<PropertyChanged> MapBack<From, To>( To Source, ref From Destination )
			where From : class, new()
			where To : class, new() {

			// In MapBack, "fromType" is type of From, which is the Destination
			// Therefore calling *Mapper.Copy*() passes in toType first

			Type fromType = typeof( From );
			Type toType = typeof( To );
			MapEntryManager.AssertTypesCanMap<From, To>();

			if ( Source == null ) {
				Destination = null;
				return null;
			}
			if ( Destination == null ) {
				Destination = (From)Activator.CreateInstance( typeof(From) );
			}

			object destinationObject = Destination;
			return ExecuteMap( toType, fromType, Source, ref destinationObject, MapDirection.DestinationToSource );
		}

		private static List<PropertyChanged> ExecuteMap( Type FromType, Type ToType, object Source, ref object Destination, MapDirection MapDirection ) {
			List<PropertyChanged> changes = null;
			if ( FromType.IsListOfT() ) {
				IList sourceList = Source as IList;
				IList destinationList = Destination as IList;
				// the below won't change what destination points to out from under Destination because Destination isn't null
				// FRAGILE: Only check source property, assuming dest property is also the same .IsClassType()
				if ( FromType.GetGenericBaseType().IsClassType() ) {
					changes = ListMapper.CopyListOfClass( FromType, ToType, sourceList, ref destinationList, MapDirection );
				} else {
					changes = ListMapper.CopyListOfNonClass( FromType, ToType, sourceList, ref destinationList );
				}
			} else if ( FromType.IsClassType() ) {
				changes = PropertyMapper.CopyProperties( FromType, ToType, Source, ref Destination, MapDirection );
			} else {
				throw new NotSupportedException( "Can't map value types here" ); // This should be a compile-time error too
			}

			return changes;
		}

		public static To MapType<From, To>( From Source ) {
			Type fromType = typeof(From);
			Type toType = typeof(To);
			MapEntryManager.AssertTypesCanMap<From, To>();
			if ( fromType.IsClassType() ) {
				throw new NotSupportedException( "Can't call this with a class type, call Map() instead" );
			}
			return (To)TypeConvert.Convert( Source, toType );
		}

	}

}