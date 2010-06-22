namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class RemapPropertyTests {

		[Test]
		public void ClassWithClassProperties_NotNull() {

			AutoMapper2.CreateMap<RemapClass1, RemapClass2>();

			RemapClass1 source = new RemapClass1 {
				Property1 = "one",
				Property2 = "two"
			};

			RemapClass2 destination = AutoMapper2.Map<RemapClass1, RemapClass2>( source );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_Null() {

			AutoMapper2.CreateMap<RemapClass1, RemapClass2>();

			RemapClass1 source = new RemapClass1 {
				Property1 = null
			};

			RemapClass2 destination = AutoMapper2.Map<RemapClass1, RemapClass2>( source );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_NullDirectly() {
			
			AutoMapper2.CreateMap<RemapClass1, RemapClass2>();

			RemapClass1 source = new RemapClass1 {
				Property1 = null,
				Property2 = null
			};
			RemapClass2 destination = null;

			var changes = AutoMapper2.Map<RemapClass1, RemapClass2>( source, ref destination );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_NullToNonNull() {
			
			AutoMapper2.CreateMap<RemapClass1, RemapClass2>();

			RemapClass1 source = new RemapClass1 {
				Property1 = null,
				Property2 = null
			};

			RemapClass2 destination = new RemapClass2 {
				Property1a = "one",
				Property2a = "two"
			};

			var changed = AutoMapper2.Map<RemapClass1, RemapClass2>( source, ref destination );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_NullBack() {
			
			AutoMapper2.CreateMap<RemapClass1, RemapClass2>();

			RemapClass2 source = null;

			RemapClass1 destination = new RemapClass1 {
				Property1 = "one",
				Property2 = "two"
			};

			var changed = AutoMapper2.MapBack<RemapClass1, RemapClass2>( source, ref destination );

			Assert.IsNull( destination );
		}

		[Test]
		public void ClassWithClassProperties_NullToNonNullBack() {
			
			AutoMapper2.CreateMap<RemapClass1, RemapClass2>();

			RemapClass2 source = new RemapClass2 {
				Property1a = null,
				Property2a = null
			};

			RemapClass1 destination = new RemapClass1 {
				Property1 = "one",
				Property2 = "two"
			};

			var changed = AutoMapper2.MapBack<RemapClass1, RemapClass2>( source, ref destination );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_NotNullBack() {

			AutoMapper2.CreateMap<RemapClass1, RemapClass2>();

			RemapClass2 source = new RemapClass2 {
				Property1a = "one",
				Property2a = "two"
			};
			RemapClass1 destination = null;

			var changes = AutoMapper2.MapBack<RemapClass1, RemapClass2>( source, ref destination );

			source.AssertEqual( destination );
		}

		public class RemapClass1 {
			public string Property1 { get; set; }
			public string Property2 { get; set; }

			public void AssertEqual(RemapClass2 Actual) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Property1, Actual.Property1a );
				Assert.AreEqual( this.Property2, Actual.Property2a );
			}
		}

		public class RemapClass2 {
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
