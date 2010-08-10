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
	[AttributeUsage( AttributeTargets.Property )]
	public class PrimaryKeyAttribute : Attribute {
	}

	/// <summary>
	/// This property should be ignored while mapping -- placed on either source or destination to take effect
	/// </summary>
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public class IgnoreMapAttribute : Attribute {

		private IgnoreDirection ignoreDirection;

		public IgnoreMapAttribute() : this( IgnoreDirection.Map | IgnoreDirection.MapBack ) {
		}

		public IgnoreMapAttribute( IgnoreDirection IgnoreDirection ) {
			if ( IgnoreDirection == IgnoreDirection.None ) {
				throw new ArgumentOutOfRangeException( "IgnoreDirection" );
			}
			this.ignoreDirection = IgnoreDirection;
		}

		public IgnoreDirection IgnoreDirection {
			get { return this.ignoreDirection; }
		}
		
	}

	[Flags]
	public enum IgnoreDirection {
		None = 0,
		Map = 1,
		MapBack = 2
	}

	/// <summary>
	/// Map a property's source to a new name -- sadly, with a magic string.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property )]
	public class RemapPropertyAttribute : Attribute {

		private string mapPropertyName;

		public RemapPropertyAttribute( string MapPropertyName ) {
			if ( string.IsNullOrEmpty( MapPropertyName ) ) {
				throw new ArgumentNullException( "MapPropertyName" );
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
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
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

	internal enum MapDirection {
		SourceToDestination,
		DestinationToSource
	}

	/// <summary>
	/// Map from the passed type to the current type when you call AutoMapper2.CreateMaps()
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
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
	[AttributeUsage( AttributeTargets.Class )]
	public class MapFromSelfAttribute : Attribute {
	}

	/// <summary>
	/// Map a List&lt;&gt; from the passed type to a List&lt;&gt; of the current type when you call AutoMapper2.CreateMaps()
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
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
	[AttributeUsage( AttributeTargets.Class )]
	public class MapListFromListOfSelfAttribute : Attribute {
	}

}
