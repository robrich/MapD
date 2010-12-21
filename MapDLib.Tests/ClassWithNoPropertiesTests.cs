namespace MapDLib.Tests {

	#region using
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class ClassWithNoPropertiesTests : BaseTest {

		[Test]
		public void ClassWithNoProperties_NotNull() {

			MapD.Config.CreateMap<NoPropertiesClass,NoPropertiesClass>();

			NoPropertiesClass source = new NoPropertiesClass();

			NoPropertiesClass destination = MapD.Copy<NoPropertiesClass, NoPropertiesClass>( source );

			Assert.IsNotNull( destination );

		}

		[Test]
		public void ClassWithNoProperties_NotNullDirectly() {

			MapD.Config.CreateMap<NoPropertiesClass, NoPropertiesClass>();

			NoPropertiesClass source = new NoPropertiesClass();

			NoPropertiesClass destination = null;

			var changes = MapD.Copy<NoPropertiesClass, NoPropertiesClass>( source, ref destination );

			Assert.IsNotNull( destination );

		}

		[Test]
		public void ClassWithNoProperties_NotNullBack() {

			MapD.Config.CreateMap<NoPropertiesClass, NoPropertiesClass>();

			NoPropertiesClass source = new NoPropertiesClass();

			NoPropertiesClass destination = null;

			var changes = MapD.CopyBack<NoPropertiesClass, NoPropertiesClass>( source, ref destination );

			Assert.IsNotNull( destination );

		}

		[Test]
		public void ClassWithNoProperties_Null() {

			MapD.Config.CreateMap<NoPropertiesClass, NoPropertiesClass>();

			NoPropertiesClass source = null;

			NoPropertiesClass destination = MapD.Copy<NoPropertiesClass, NoPropertiesClass>( source );

			Assert.IsNull( destination );

		}

		[Test]
		public void ClassWithNoProperties_NullDirectly() {

			MapD.Config.CreateMap<NoPropertiesClass, NoPropertiesClass>();

			NoPropertiesClass source = null;

			NoPropertiesClass destination = new NoPropertiesClass();

			var changes = MapD.Copy<NoPropertiesClass, NoPropertiesClass>( source, ref destination );

			Assert.IsNull( destination );

		}

		[Test]
		public void ClassWithNoProperties_NullBack() {

			MapD.Config.CreateMap<NoPropertiesClass, NoPropertiesClass>();

			NoPropertiesClass source = null;

			NoPropertiesClass destination = new NoPropertiesClass();

			var changes = MapD.CopyBack<NoPropertiesClass, NoPropertiesClass>( source, ref destination );

			Assert.IsNotNull( destination );

		}

		private class NoPropertiesClass {
		}

	}

}
