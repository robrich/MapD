namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	#endregion

	public class PrimaryKeyAttribute : Attribute {
	}

	public class IgnoreMapAttribute : Attribute {
		
	}

	public class RemapPropertyAttribute : Attribute {

		private string mapPropertyName;

		public RemapPropertyAttribute( string MapPropertyName ) {
			this.mapPropertyName = MapPropertyName;
			if ( string.IsNullOrEmpty( this.mapPropertyName ) ) {
				throw new ArgumentNullException( "MapProperty" );
			}
		}

		public string MapPropertyName {
			get { return this.mapPropertyName; }
		}

	}

}
