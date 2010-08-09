namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	#endregion

	internal class MapEntry {
		public Type From { get; set; }
		public Type To { get; set; }

		public MapEntryType MapEntryType { get; set; }
		// Null means we haven't mapped it yet
		// MapEntryType.Class: these are the properties
		// MapEntryType.ListOfClass: these are the primary keys
		// MapEntryType.ListOfNonClass: it's just not blank
		public List<MapEntryProperty> Properties { get; set; }

	}

	internal class MapEntryProperty {
		public PropertyInfo Source { get; set; }
		public PropertyInfo Destination { get; set; }
		public IgnoreDirection IgnoreDirection { get; set; }
	}

	internal enum MapEntryType {
		NotSet = 0,
		Class,
		NonClass,
		ListOfClass,
		ListOfNonClass
	}

}
