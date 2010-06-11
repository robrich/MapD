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

		#region List<MapEntry> -- insure you say we can before we map

		private class MapEntry {
			public Type From { get; set; }
			public Type To { get; set; }

			public override bool Equals( object obj ) {
				MapEntry m = obj as MapEntry;
				if ( m != null ) {
					return ( m.From == this.From && m.To == this.To );
				}
				return base.Equals( obj );
			}

			public override int GetHashCode() {
				return base.GetHashCode();
			}
		}

		private static List<MapEntry> mapList = new List<MapEntry>();

		private static void AddMapEntry( Type From, Type To ) {
			MapEntry m = new MapEntry {
				From = From,
				To = To
			};
			if ( !mapList.Contains( m ) ) {
				mapList.Add( m );
			}
		}

		#endregion

		#region AssertMapEntry
		private static void AssertMapEntry<From, To>() {
			MapEntry m = new MapEntry {
				From = typeof( From ),
				To = typeof( To )
			};
			if ( !mapList.Contains( m ) ) {
				throw new MissingMapException<From, To>();
			}
		}
		#endregion

		#region AssertTypesCanMap
		private static void AssertTypesCanMap<From, To>() {
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
			if ( from.IsListOfT() ) {
				Type fromBase = from.GetGenericTypeDefinition();
				Type toBase = to.GetGenericTypeDefinition(); // If it isn't a Generic, the above would've caught it
				AssertTypeClassesCanMap( fromBase, toBase );
			}
			AssertTypeClassesCanMap( from, to );
		}
		private static void AssertTypeClassesCanMap( Type from, Type to ) {
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

		public static bool ExcludeLinqProperties { get; set; }

		static AutoMapper2() {
			ExcludeLinqProperties = true;
		}

		public static void CreateMap<From, To>()
			where From : class, new()
			where To : class, new() {
			Type fromType = typeof( From );
			Type toType = typeof( To );
			AssertTypesCanMap<From, To>();
			AddMapEntry( fromType, toType );
		}

		public static void ResetMap() {
			mapList.Clear();
		}

		public static void AssertConfigurationIsValid() {
			if ( mapList == null || mapList.Count == 0 ) {
				throw new ArgumentNullException( "", "You've not created any maps" );
			}
			foreach ( MapEntry mapEntry in mapList ) {
				// TODO: This flexes the "null to null" case, which is pretty much not helpful
				object source = Activator.CreateInstance( mapEntry.From );
				object destination = Activator.CreateInstance( mapEntry.To );
				AutoMapper2.Map( source, destination ); // If this errors, it should tell you why
			}
		}

		public static To Map<From, To>( From Source )
			where From : class
			where To : class, new() {
			AssertMapEntry<From, To>();
			AssertTypesCanMap<From, To>();
			if ( Source == null ) {
				return null;
			}
			Type fromType = typeof( From );
			Type toType = typeof( To );
			To destination = (To)Activator.CreateInstance( toType );
			List<PropertyChanged> changes = null; // Unused here, but maintained for similarity between Map(Source,Destination)
			if ( fromType.IsListOfT() ) {
				IList sourceList = Source as IList;
				IList destinationList = (IList)destination;
				// the below won't change what destination points to out from under Destination because Destination isn't null
				// FRAGILE: Only check source property, assuming dest property is also the same .IsClassType()
				if ( fromType.GetGenericBaseType().IsClassType() ) {
					changes = ListMapper.CopyListOfClass( fromType, toType, sourceList, ref destinationList, SmallerObject.Destination, ExcludeLinqProperties );
				} else {
					changes = ListMapper.CopyListOfNonClass( fromType, toType, sourceList, ref destinationList );
				}
			} else {
				changes = PropertyMapper.CopyProperties<From, To>( Source, ref destination, SmallerObject.Destination, ExcludeLinqProperties );
			}
			return destination;
		}

		public static List<PropertyChanged> Map<From, To>( From Source, To Destination )
			where From : class, new()
			where To : class, new() {
			AssertMapEntry<From, To>();
			AssertTypesCanMap<From, To>();
			if ( Destination == null ) {
				// Otherwise we'll return null right back at you
				throw new ArgumentNullException( "Destination" );
			}
			if ( Source == null ) {
				// TODO: destination is still set
				return null;
			}
			Type fromType = typeof( From );
			Type toType = typeof( To );
			List<PropertyChanged> changes = null;
			if ( fromType.IsListOfT() ) {
				IList sourceList = Source as IList;
				IList destinationList = Destination as IList;
				// the below won't change what destination points to out from under Destination because Destination isn't null
				// FRAGILE: Only check source property, assuming dest property is also the same .IsClassType()
				if ( fromType.GetGenericBaseType().IsClassType() ) {
					changes = ListMapper.CopyListOfClass( fromType, toType, sourceList, ref destinationList, SmallerObject.Destination, ExcludeLinqProperties );
				} else {
					changes = ListMapper.CopyListOfNonClass( fromType, toType, sourceList, ref destinationList );
				}
			} else {
				changes = PropertyMapper.CopyProperties<From, To>( Source, ref Destination, SmallerObject.Destination, ExcludeLinqProperties );
			}
			return changes;
		}

		public static List<PropertyChanged> MapBack<From, To>( To Source, From Destination )
			where From : class, new()
			where To : class, new() {
			AssertMapEntry<From, To>();
			AssertTypesCanMap<From, To>();
			if ( Destination == null ) {
				// Otherwise we'll return null right back at you
				throw new ArgumentNullException( "Destination" );
			}
			if ( Source == null ) {
				// TODO: destination is still set
				return null;
			}
			Type fromType = typeof( From );
			Type toType = typeof( To );
			List<PropertyChanged> changes = null;
			if ( fromType.IsListOfT() ) {
				IList sourceList = Source as IList;
				IList destinationList = Destination as IList;
				// the below won't change what destination points to out from under Destination because Destination isn't null
				// FRAGILE: Only check source property, assuming dest property is also the same .IsClassType()
				if ( fromType.GetGenericBaseType().IsClassType() ) {
					changes = ListMapper.CopyListOfClass( fromType, toType, sourceList, ref destinationList, SmallerObject.Destination, ExcludeLinqProperties );
				} else {
					changes = ListMapper.CopyListOfNonClass( fromType, toType, sourceList, ref destinationList );
				}
			} else {
				changes = PropertyMapper.CopyProperties<To, From>( Source, ref Destination, SmallerObject.Source, ExcludeLinqProperties );
			}
			return changes;
		}

	}

}
