namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class ClassPropertiesTests : BaseTest {

		[Test]
		public void ClassWithClassProperties_NotNull() {

			AutoMapper2.Config.CreateMap<ClassWithProperties, ClassWithProperties>();
			AutoMapper2.Config.CreateMap<ClassWithPropertiesInner, ClassWithPropertiesInner>();

			ClassWithProperties source = new ClassWithProperties {
				Property1 = new ClassWithPropertiesInner {
					Property2 = 1,
					Property3 = "2",
					Property4 = Guid.NewGuid()
				}
			};

			ClassWithProperties destination = AutoMapper2.Map<ClassWithProperties, ClassWithProperties>( source );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_Null() {

			AutoMapper2.Config.CreateMap<ClassWithProperties, ClassWithProperties>();
			AutoMapper2.Config.CreateMap<ClassWithPropertiesInner, ClassWithPropertiesInner>();

			ClassWithProperties source = new ClassWithProperties {
				Property1 = null
			};

			ClassWithProperties destination = AutoMapper2.Map<ClassWithProperties, ClassWithProperties>( source );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_NullDirectly() {

			AutoMapper2.Config.CreateMap<ClassWithProperties, ClassWithProperties>();
			AutoMapper2.Config.CreateMap<ClassWithPropertiesInner, ClassWithPropertiesInner>();

			ClassWithProperties source = new ClassWithProperties {
				Property1 = null
			};
			ClassWithProperties destination = null;

			var changes = AutoMapper2.Map<ClassWithProperties, ClassWithProperties>( source, ref destination );

			source.AssertEqual( destination );
		}

		[Test]
		public void ClassWithClassProperties_NullToNonNull() {

			AutoMapper2.Config.CreateMap<ClassWithProperties, ClassWithProperties>();
			AutoMapper2.Config.CreateMap<ClassWithPropertiesInner, ClassWithPropertiesInner>();

			ClassWithProperties source = new ClassWithProperties {
				Property1 = null
			};

			ClassWithProperties destination = new ClassWithProperties {
				Property1 = new ClassWithPropertiesInner {
					Property2 = 1000,
					Property3 = "1000",
					Property4 = Guid.NewGuid()
				}
			};

			var changed = AutoMapper2.Map<ClassWithProperties, ClassWithProperties>( source, ref destination );

			source.AssertEqual( destination );
		}

		private class ClassWithProperties {
			public ClassWithPropertiesInner Property1 { get; set; }

			public void AssertEqual( ClassWithProperties Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Property1 == null, Actual.Property1 == null );
				if ( this.Property1 != null ) {
					this.Property1.AssertEqual( Actual.Property1 );
				}
			}

		}
		private class ClassWithPropertiesInner {
			public int Property2 { get; set; }
			public string Property3 { get; set; }
			public Guid Property4 { get; set; }

			public void AssertEqual( ClassWithPropertiesInner Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( Property2, Actual.Property2 );
				Assert.AreEqual( Property3, Actual.Property3 );
				Assert.AreEqual( Property4, Actual.Property4 );
			}
		}


	}

}
