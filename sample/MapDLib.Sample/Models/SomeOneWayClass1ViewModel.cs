namespace MapDLib.Sample.Models {
	using MapDLib.Sample.Some_Other_Project;

	// This maps from the class but doesn't map back to the target class
	// Useful if the ViewModel is a sumation class or a sumation of the source
	[MapFrom( typeof( SomeOneWayClass1 ) )]
	[IgnoreMap( IgnoreDirection.MapBack )]
	public class SomeOneWayClass1ViewModel {
		public string Property1 { get; set; }
		public string Property2 { get; set; }
	}
}