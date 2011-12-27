namespace MapDLib.Sample.Models {
	using MapDLib.Sample.Some_Other_Project;

	// This attribute tells you where to map from
	[MapFrom( typeof(SomeClass1) )]
	public class SomeClass1ViewModel {

		// Presuming the property names exactly match, nothing else is needed

		public string Property1 { get; set; }
		public string Property2 { get; set; }
	}
}