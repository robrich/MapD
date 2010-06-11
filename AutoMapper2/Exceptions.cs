namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	#endregion

	public class MissingMapException<From, To> : Exception {

		public MissingMapException()
			: base( string.Format( "Can't convert from {0} to {1} because there is no map to do so",
			typeof( From ).FullName, typeof( To ).FullName ) ) {
		}

	}

	public class InvalidTypeConversionException : Exception {

		private Type from;
		private Type to;
		private readonly InvalidPropertyReason invalidPropertyReason;
		private PropertyInfo toPropertyInfo;

		// FRAGILE: ASSUME: both From and To aren't null
		public InvalidTypeConversionException( Type From, Type To, InvalidPropertyReason InvalidPropertyReason, PropertyInfo ToPropertyInfo = null )
			: base( string.Format( "Can't convert{0} from {1} to {2} because {3}",
			( ToPropertyInfo != null ? ( " " + ToPropertyInfo.Name ) : "" ), From.FullName, To.FullName, InvalidPropertyReason ) ) {
			this.from = From;
			this.to = To;
			this.invalidPropertyReason = InvalidPropertyReason;
			this.toPropertyInfo = ToPropertyInfo;
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

		public PropertyInfo ToPropertyInfo {
			get { return this.toPropertyInfo; }
		}

	}

	public class MissingPropertyException<T> : Exception {

		private readonly PropertyInfo propertyInfo;

		public MissingPropertyException( PropertyInfo PropertyInfo )
			// FRAGILE: Assumes PropertyInfo isn't null
			: base( PropertyInfo.Name + " isn't found in type " + typeof( T ).Name ) {
			this.propertyInfo = PropertyInfo;
		}

		public PropertyInfo PropertyInfo {
			get { return this.propertyInfo; }
		}

	}

	public class InvalidPropertyException<T> : Exception {

		private readonly PropertyInfo propertyInfo;
		private readonly InvalidPropertyReason invalidPropertyReason;

		public InvalidPropertyException( PropertyInfo PropertyInfo, InvalidPropertyReason InvalidPropertyReason )
			// FRAGILE: Assumes PropertyInfo isn't null
			: base( PropertyInfo.Name + " can't be used for type " + typeof( T ).Name + " because " + InvalidPropertyReason ) {
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
		MissingFromPrimaryKey,
		MissingToPrimaryKey,
		FromPrimaryKeyBlank
	}

}
