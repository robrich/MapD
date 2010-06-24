namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	#endregion

	public class PropertyChangedResults {

		public Type ObjectType { get; set; }
		public object Object { get; set; }
		public Type PropertyType { get; set; }
		public string PropertyName { get; set; }
		public string OldValue { get; set; }
		public string NewValue { get; set; }

	}

}
