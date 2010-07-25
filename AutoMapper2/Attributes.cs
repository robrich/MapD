namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	#endregion

	/// <summary>
	/// This property/ies are the primary key when mapping lists
	/// </summary>
	public class PrimaryKeyAttribute : Attribute {
	}

	/// <summary>
	/// This property should be ignored while mapping -- placed on either source or destination to take effect
	/// </summary>
	public class IgnoreMapAttribute : Attribute {
		
	}

	/// <summary>
	/// Map a property's source to a new name -- sadly, with a magic string.
	/// </summary>
	public class RemapPropertyAttribute : Attribute {

		private string mapPropertyName;

		public RemapPropertyAttribute( string MapPropertyName ) {
			if ( string.IsNullOrEmpty( MapPropertyName ) ) {
				throw new ArgumentNullException( "MapProperty" );
			}
			this.mapPropertyName = MapPropertyName;
		}

		public string MapPropertyName {
			get { return this.mapPropertyName; }
		}

	}

	/// <summary>
	/// If the property is <see cref="PropertyIs"/>.ReadOnly or <see cref="PropertyIs"/>.WriteOnly, we can flag the class to ignore them while mapping
	/// </summary>
	public class IgnorePropertiesIfAttribute : Attribute {

		private PropertyIs propertyIs;

		public IgnorePropertiesIfAttribute( PropertyIs PropertyIs ) {
			if ( !Enum.IsDefined(typeof(PropertyIs), PropertyIs) ) {
				throw new ArgumentOutOfRangeException( "PropertyIs" );
			}
			this.propertyIs = PropertyIs;
		}

		public PropertyIs PropertyIs {
			get { return this.propertyIs; }
		}
		
	}

	[Flags]
	public enum PropertyIs {
		NotSet = 0,
		ReadOnly = 1,
		WriteOnly = 2
	}

	/// <summary>
	/// Map from the passed type to the current type when you call AutoMapper2.CreateMaps()
	/// </summary>
	public class MapFromAttribute : Attribute {

		private Type type;

		public MapFromAttribute( Type Type ) {
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			this.type = Type;
		}

		public Type Type {
			get { return this.type; }
		}
		
	}

	/// <summary>
	/// Map from the current type to the current type when you call AutoMapper2.CreateMaps()
	/// </summary>
	public class MapFromSelfAttribute : Attribute {
	}

	/// <summary>
	/// Map a List&lt;&gt; from the passed type to a List&lt;&gt; of the current type when you call AutoMapper2.CreateMaps()
	/// </summary>
	public class MapListFromListOfAttribute : Attribute {

		private Type type;

		public MapListFromListOfAttribute( Type Type ) {
			if ( Type == null ) {
				throw new ArgumentNullException( "Type" );
			}
			this.type = Type;
		}

		public Type Type {
			get { return this.type; }
		}

	}

	/// <summary>
	/// Map a List&lt;&gt; from the current type to a List&lt;&gt; of the current type when you call AutoMapper2.CreateMaps()
	/// </summary>
	public class MapListFromListOfSelfAttribute : Attribute {
	}

}
