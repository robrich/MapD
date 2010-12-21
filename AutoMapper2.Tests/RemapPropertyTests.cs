namespace MapDLib.Tests {

	#region using
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class RemapPropertyTests : BaseTest {

		[Test]
		public void ClassWithClassProperties_NotNull() {

			MapD.Config.CreateMap<RemapClass1, RemapClass2>();

			RemapClass1 source = new RemapClass1 {
				Property1 = "one",
				Property2 = "two"
			};

			RemapClass2 destination = MapD.Copy<RemapClass1, RemapClass2>( source );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_Null() {

			MapD.Config.CreateMap<RemapClass1, RemapClass2>();

			RemapClass1 source = new RemapClass1 {
				Property1 = null
			};

			RemapClass2 destination = MapD.Copy<RemapClass1, RemapClass2>( source );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_NullDirectly() {

			MapD.Config.CreateMap<RemapClass1, RemapClass2>();

			RemapClass1 source = new RemapClass1 {
				Property1 = null,
				Property2 = null
			};
			RemapClass2 destination = null;

			var changes = MapD.Copy<RemapClass1, RemapClass2>( source, ref destination );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_NullToNonNull() {

			MapD.Config.CreateMap<RemapClass1, RemapClass2>();

			RemapClass1 source = new RemapClass1 {
				Property1 = null,
				Property2 = null
			};

			RemapClass2 destination = new RemapClass2 {
				Property1a = "one",
				Property2a = "two"
			};

			var changed = MapD.Copy<RemapClass1, RemapClass2>( source, ref destination );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_NullBack() {

			MapD.Config.CreateMap<RemapClass1, RemapClass2>();

			RemapClass2 source = null;

			RemapClass1 destination = new RemapClass1 {
				Property1 = "one",
				Property2 = "two"
			};

			var changed = MapD.CopyBack<RemapClass1, RemapClass2>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( "one", destination.Property1 );
			Assert.AreEqual( "two", destination.Property2 );
		}

		[Test]
		public void ClassWithClassProperties_NullToNonNullBack() {

			MapD.Config.CreateMap<RemapClass1, RemapClass2>();

			RemapClass2 source = new RemapClass2 {
				Property1a = null,
				Property2a = null
			};

			RemapClass1 destination = new RemapClass1 {
				Property1 = "one",
				Property2 = "two"
			};

			var changed = MapD.CopyBack<RemapClass1, RemapClass2>( source, ref destination );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_NotNullBack() {

			MapD.Config.CreateMap<RemapClass1, RemapClass2>();

			RemapClass2 source = new RemapClass2 {
				Property1a = "one",
				Property2a = "two"
			};
			RemapClass1 destination = null;

			var changes = MapD.CopyBack<RemapClass1, RemapClass2>( source, ref destination );

			source.AssertEqual( destination );
		}

		private class RemapClass1 {
			public string Property1 { get; set; }
			public string Property2 { get; set; }

			public void AssertEqual( RemapClass2 Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Property1, Actual.Property1a );
				Assert.AreEqual( this.Property2, Actual.Property2a );
			}
		}

		private class RemapClass2 {
			[RemapProperty( "Property1" )]
			public string Property1a { get; set; }
			[RemapProperty( "Property2" )]
			public string Property2a { get; set; }

			public void AssertEqual( RemapClass1 Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Property1a, Actual.Property1 );
				Assert.AreEqual( this.Property2a, Actual.Property2 );
			}
		}

	}

}
