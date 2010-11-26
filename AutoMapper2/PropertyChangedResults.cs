namespace AutoMapper2Lib {

	#region using
	using System;

	#endregion

	public class PropertyChangedResults {

		public PropertyChangedResults() {
			this.Source = new PropertyChangedResult();
			this.Destination = new PropertyChangedResult();
		}

		/// <summary>
		/// The object you were mapping / comparing from
		/// </summary>
		public PropertyChangedResult Source { get; set; }
		/// <summary>
		/// The object you were mapping / comparing to
		/// </summary>
		public PropertyChangedResult Destination { get; set; }

	}

	public class PropertyChangedResult {
		public object Object { get; set; }
		public Type ObjectType { get; set; }
		public object PropertyValue { get; set; }
		public Type PropertyType { get; set; }
		public string PropertyName { get; set; }
	}

}
