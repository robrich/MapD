namespace MapDLib.TestsResource {
	using MapDLib;
	using NUnit.Framework;

	[MapFrom( typeof( MapFromAttributeResourceType ) )]
	[MapListFromListOf( typeof( MapFromAttributeResourceType ) )]
	public class MapFromAttributeResourceType {
		[PrimaryKey]
		public int Property1 { get; set; }
		public int Property2 { get; set; }
		public int Property3 { get; set; }

		public void AssertEqual( MapFromAttributeResourceType Actual ) {
			Assert.IsNotNull( Actual );
			Assert.AreEqual( this.Property1, Actual.Property1 );
			Assert.AreEqual( this.Property2, Actual.Property2 );
			Assert.AreEqual( this.Property3, Actual.Property3 );
		}

	}
}