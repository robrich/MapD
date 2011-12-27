namespace MapDLib.Sample.Models {
	using System.ComponentModel.DataAnnotations;
	using MapDLib.Sample.Some_Other_Project;

	// This attribute tells you that you're mapping a list of these from a list of those
	// Both this class and that class must have a primary key property noted by [Key]
	[MapListFromListOf( typeof(SomeListClass1) )]
	public class SomeListClass1ViewModel {

		[Key]
		public int SomeId { get; set; }

		// If your ViewModel's property name is different, this attribute says what the Entity's property is
		[RemapProperty( "Name" )]
		public string TheName { get; set; }

		// Note that the domain object's property wasn't a string
		public string OriginDate { get; set; }

		// Note that there's no Interested property here, and that nothing needs to signify that

		// This property isn't mapped automatically
		// Pass in an argument saying which direction ignores this property
		// Default is both directions
		[IgnoreMap]
		public double SomeIrrelevantValue { get; set; }

	}
}