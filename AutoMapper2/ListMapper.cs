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
		// If List<Non-class>
		public static List<PropertyChanged> CopyListOfNonClass( Type FromType, Type ToType, IList Source, ref IList Destination ) {

			List<PropertyChanged> changes = new List<PropertyChanged>();

			Type toInnerType = ToType.GetGenericBaseType();
			Type fromInnerType = FromType.GetGenericBaseType();

			if ( toInnerType.IsClassType() ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.ListNonClassTypeToListClassType );
			}
			if ( fromInnerType.IsClassType() ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.ListClassTypeToListNonClassType );
			}

			if ( Source == null ) {
				// TODO: null Destination?
				return changes;
			}
			if ( Destination == null ) {
				Destination = (IList)Activator.CreateInstance( ToType );
			}
			if ( Source.Count == 0 ) {
				return changes;
			}

			foreach ( object from in Source ) {

				object to = TypeConvert.Convert( from, toInnerType );
				if ( !Destination.Contains( to ) ) {
					changes.Add(
						new PropertyChanged {
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
		public static List<PropertyChanged> CopyListOfClass( Type FromType, Type ToType, IList Source, ref IList Destination, SmallerObject SmallerObject, bool ExcludeLinqProperties ) {

			List<PropertyChanged> changes = new List<PropertyChanged>();

			Type toInnerType = ToType.GetGenericBaseType();
			Type fromInnerType = FromType.GetGenericBaseType();

			if ( !toInnerType.IsClassType() ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.ListClassTypeToListNonClassType );
			}
			if ( !fromInnerType.IsClassType() ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.ListNonClassTypeToListClassType );
			}

			if ( Source == null ) {
				// TODO: null Destination?
				return changes;
			}
			if ( Destination == null ) {
				Destination = (IList)Activator.CreateInstance( ToType );
			}
			if ( Source.Count == 0 ) {
				return changes;
			}

			PropertyInfo sourcePrimaryKey = ReflectionHelper.GetProperties( fromInnerType ).GetPrimaryKey();
			PropertyInfo destinationPrimaryKey = ReflectionHelper.GetProperties( toInnerType ).GetPrimaryKey();
			if ( sourcePrimaryKey == null ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.MissingFromPrimaryKey );
			}
			if ( destinationPrimaryKey == null ) {
				throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.MissingToPrimaryKey );
			}

			foreach ( object from in Source ) {

				// Get source primary key value
				object fromKey = sourcePrimaryKey.GetValue( from, null );
				if ( fromKey == null ) {
					throw new InvalidTypeConversionException( FromType, ToType, InvalidPropertyReason.FromPrimaryKeyBlank, sourcePrimaryKey );
				}

				// Convert to destination primary key type
				object toKey = TypeConvert.Convert( fromKey, destinationPrimaryKey.PropertyType );

				// Locate destination object
				object to = null;
				foreach ( object toStep in Destination ) {
					// TODO: Loop through destination fully to map?
					// TODO: or cache value / object to avoid repeating this on next lap?
					object toKeyStep = destinationPrimaryKey.GetValue( toStep, null );
					if ( toKeyStep == toKey ) {
						to = toStep;
						break;
					}
				}
				// If destination object isn't found, create one
				if ( to == null ) {
					to = Activator.CreateInstance( toInnerType );
					changes.Add(
						new PropertyChanged {
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
				List<PropertyChanged> changeListStep = PropertyMapper.CopyProperties( from, ref to, SmallerObject, ExcludeLinqProperties );
				if ( changeListStep != null && changeListStep.Count > 0 ) {
					changes.AddRange( changeListStep );
				}

			}

			// TODO: Check for stuff in destinationPropertyValue not in sourcePropertyValue and remove them?

			return changes;
		}

	}

}
