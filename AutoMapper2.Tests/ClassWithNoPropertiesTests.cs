namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class ClassWithNoPropertiesTests : BaseTest {

		[Test]
		public void ClassWithNoProperties_NotNull() {

			AutoMapper2.CreateMap<NoPropertiesClass,NoPropertiesClass>();

			NoPropertiesClass source = new NoPropertiesClass();

			NoPropertiesClass destination = AutoMapper2.Map<NoPropertiesClass, NoPropertiesClass>( source );

			Assert.IsNotNull( destination );

		}

		[Test]
		public void ClassWithNoProperties_NotNullDirectly() {

			AutoMapper2.CreateMap<NoPropertiesClass, NoPropertiesClass>();

			NoPropertiesClass source = new NoPropertiesClass();

			NoPropertiesClass destination = null;

			var changes = AutoMapper2.Map<NoPropertiesClass, NoPropertiesClass>( source, ref destination );

			Assert.IsNotNull( destination );

		}

		[Test]
		public void ClassWithNoProperties_NotNullBack() {

			AutoMapper2.CreateMap<NoPropertiesClass, NoPropertiesClass>();

			NoPropertiesClass source = new NoPropertiesClass();

			NoPropertiesClass destination = null;

			var changes = AutoMapper2.MapBack<NoPropertiesClass, NoPropertiesClass>( source, ref destination );

			Assert.IsNotNull( destination );

		}

		[Test]
		public void ClassWithNoProperties_Null() {

			AutoMapper2.CreateMap<NoPropertiesClass, NoPropertiesClass>();

			NoPropertiesClass source = null;

			NoPropertiesClass destination = AutoMapper2.Map<NoPropertiesClass, NoPropertiesClass>( source );

			Assert.IsNull( destination );

		}

		[Test]
		public void ClassWithNoProperties_NullDirectly() {

			AutoMapper2.CreateMap<NoPropertiesClass, NoPropertiesClass>();

			NoPropertiesClass source = null;

			NoPropertiesClass destination = new NoPropertiesClass();

			var changes = AutoMapper2.Map<NoPropertiesClass, NoPropertiesClass>( source, ref destination );

			Assert.IsNull( destination );

		}

		[Test]
		public void ClassWithNoProperties_NullBack() {

			AutoMapper2.CreateMap<NoPropertiesClass, NoPropertiesClass>();

			NoPropertiesClass source = null;

			NoPropertiesClass destination = new NoPropertiesClass();

			var changes = AutoMapper2.MapBack<NoPropertiesClass, NoPropertiesClass>( source, ref destination );

			Assert.IsNotNull( destination );

		}

		public class NoPropertiesClass {
		}

	}

}
