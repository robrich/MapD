namespace MapDLib {
	using System;

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

		public override string ToString() {
			return this.ObjectToString();
		}
		public override int GetHashCode() {
			return base.GetHashCode();
		}

	}

	public class PropertyChangedResult {

		public object Object { get; set; }
		public Type ObjectType { get; set; }
		public object PropertyValue { get; set; }
		public Type PropertyType { get; set; }
		public string PropertyName { get; set; }

		public override string ToString() {
			return this.ObjectToString();
		}
		public override int GetHashCode() {
			return base.GetHashCode();
		}

	}

}