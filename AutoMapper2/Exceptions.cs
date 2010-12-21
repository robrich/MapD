namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Reflection;

	#endregion

	public class MissingMapException : Exception {

		private Type fromType;
		private Type toType;

		// FRAGILE: ASSUME: both From and To aren't null
		public MissingMapException( Type FromType, Type ToType )
			: base( string.Format( "Can't convert from {0} to {1} because there is no map to do so",
			FromType.FullName, ToType.FullName ) ) {
			this.fromType = FromType;
			this.toType = ToType;
		}

		public Type FromType {
			get { return fromType; }
		}
		public Type ToType {
			get { return toType; }
		}

	}

	public class InvalidTypeConversionException : Exception {

		private Type from;
		private Type to;
		private readonly InvalidPropertyReason invalidPropertyReason;
		private PropertyInfo propertyInfo;

		public InvalidTypeConversionException( Type From, Type To, InvalidPropertyReason InvalidPropertyReason )
			: this( From, To, InvalidPropertyReason, null ) {
		}

		// FRAGILE: ASSUME: both From and To aren't null
		public InvalidTypeConversionException( Type From, Type To, InvalidPropertyReason InvalidPropertyReason, PropertyInfo PropertyInfo )
			: base( string.Format( "Can't convert{0} from {1} to {2} because {3}",
			( PropertyInfo != null ? ( " " + PropertyInfo.Name ) : "" ), From.FullName, To.FullName, InvalidPropertyReason ) ) {
			this.from = From;
			this.to = To;
			this.invalidPropertyReason = InvalidPropertyReason;
			this.propertyInfo = PropertyInfo;
		}

		public Type From {
			get { return this.from; }
		}
		public Type To {
			get { return this.to; }
		}

		public InvalidPropertyReason InvalidPropertyReason {
			get { return this.invalidPropertyReason; }
		}

		public PropertyInfo PropertyInfo {
			get { return this.propertyInfo; }
		}

	}

	public class InvalidPropertyException : Exception {

		private readonly PropertyInfo propertyInfo;
		private readonly InvalidPropertyReason invalidPropertyReason;

		public InvalidPropertyException( PropertyInfo PropertyInfo, InvalidPropertyReason InvalidPropertyReason )
			// FRAGILE: Assumes PropertyInfo isn't null
			: base( PropertyInfo.Name + " can't be used for type " + PropertyInfo.PropertyType.Name + " because " + InvalidPropertyReason ) {
			this.propertyInfo = PropertyInfo;
			this.invalidPropertyReason = InvalidPropertyReason;
		}

		public PropertyInfo PropertyInfo {
			get { return this.propertyInfo; }
		}
		public InvalidPropertyReason InvalidPropertyReason {
			get { return this.invalidPropertyReason; }
		}

	}

	public enum InvalidPropertyReason {
		CantWrite,
		CantRead,
		NonClassTypeToClassType,
		ClassTypeToNonClassType,
		NonListTypeToListType,
		ListTypeToNonListType,
		ListNonClassTypeToListClassType,
		ListClassTypeToListNonClassType,
		MissingPrimaryKey,
		MissingFromPrimaryKey,
		MissingToPrimaryKey,
		FromPrimaryKeyBlank,
		FromPrimaryKeyConversionFailure,
		ToPrimaryKeyBlank,
		MissingProperty
	}

	// Using the map failed, building the map succeeded previously
	public class MapFailureException : Exception {

		private readonly PropertyInfo propertyInfo;
		private readonly object target;
		private readonly object value;
		private readonly MapFailureReason mapFailureReason;

		public MapFailureException( PropertyInfo PropertyInfo, object Target, object Value, MapFailureReason MapFailureReason, Exception innerException )
			// FRAGILE: Assumes PropertyInfo isn't null
			: base( string.Format( "Failed to map {0} on {1}{2} because {3}: {4}",
				(PropertyInfo != null ? PropertyInfo.Name : ""),
				Target.ObjectToString(),
				( Value != null ? ( "to " + Value.ObjectToString() ) : null ),
				MapFailureReason,
				( innerException != null ? innerException.Message : "" )
			) ) {
			this.propertyInfo = PropertyInfo;
			this.target = Target;
			this.value = Value;
			this.mapFailureReason = MapFailureReason;
		}

		public PropertyInfo PropertyInfo {
			get { return this.propertyInfo; }
		}
		public object Target {
			get { return this.target; }
		}
		public object Value {
			get { return this.value; }
		}
		public MapFailureReason MapFailureReason {
			get { return this.mapFailureReason; }
		}

	}

	public enum MapFailureReason {
		GetSourceFailure,
		GetDestinationFailure,
		SetDestinationFailure,
		DuplicateFromPrimaryKey,
		DuplicateToPrimaryKey,
		ConvertTypeFailure
	}

}
