namespace MapDLib.Tests {

	#region using
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class ClassToDerivedClassTests : BaseTest {

		[Test]
		public void ClassToDerivedClass1_NonNull() {

			MapD.Config.CreateMap<DerivedClass1, BaseClass1>();

			DerivedClass1 source = new DerivedClass1 {
				Property1 = 1,
				Property2 = 2,
				Property3 = 3
			};

			BaseClass1 destination = MapD.Copy<DerivedClass1, BaseClass1>( source );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Property1, destination.Property1 );
			Assert.AreEqual( source.Property2, destination.Property2 );
		}

		[Test]
		public void ClassToDerivedClass1_Null() {

			MapD.Config.CreateMap<DerivedClass1, BaseClass1>();

			DerivedClass1 source = null;

			BaseClass1 destination = MapD.Copy<DerivedClass1, BaseClass1>( source );

			Assert.IsNull( destination );
		}

		private class BaseClass1 {
			public int Property1 { get; set; }
			public int Property2 { get; set; }
		}
		private class DerivedClass1 : BaseClass1 {
			public int Property3 { get; set; }
		}

		[Test]
		public void ClassToDerivedClass2_NonNull() {

			MapD.Config.CreateMap<BaseClass2, DerivedClass2>();

			BaseClass2 source = new BaseClass2 {
				Property1 = 1,
				Property2 = 2
			};

			DerivedClass2 destination = MapD.Copy<BaseClass2, DerivedClass2>( source );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Property1, destination.Property1 );
			Assert.AreEqual( source.Property2, destination.Property2 );
			Assert.AreEqual( 0, destination.Property3 );
			Assert.AreEqual( 4, destination.Property4 );
		}

		[Test]
		public void ClassToDerivedClass2_Null() {

			MapD.Config.CreateMap<BaseClass2, DerivedClass2>();

			DerivedClass2 source = null;

			DerivedClass2 destination = MapD.Copy<BaseClass2, DerivedClass2>( source );

			Assert.IsNull( destination );
		}

		private class BaseClass2 {
			public int Property1 { get; set; }
			public int Property2 { get; set; }
		}
		private class DerivedClass2 : BaseClass2 {
			public DerivedClass2() {
				this.Property4 = 4;
			}
			[IgnoreMap]
			public int Property3 { get; set; }
			[IgnoreMap]
			public int Property4 { get; set; }
		}

	}

}
