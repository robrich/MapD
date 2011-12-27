namespace MapDLib.Sample.Some_Other_Project {
	using System;
	using System.ComponentModel.DataAnnotations;

	public class SomeListClass1 {
		[Key]
		public int SomeId { get; set; }
		public string Name { get; set; }
		public DateTime OriginDate { get; set; }
		public bool Interested { get; set; }
	}
}