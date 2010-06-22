namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using NUnit.Framework;

	#endregion

	public class IgnoreAttributeTests : BaseTest {

		#region No_Ignore_Class

		[Test]
		public void No_Ignore_Class() {

			AutoMapper2.CreateMap<No_Ignore_Class_Type, No_Ignore_Class_Type>();

			No_Ignore_Class_Type source = new No_Ignore_Class_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};

			No_Ignore_Class_Type destination = AutoMapper2.Map<No_Ignore_Class_Type, No_Ignore_Class_Type>( source );

			source.AssertEqual( destination );

		}

		public class No_Ignore_Class_Type {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }

			public void AssertEqual( No_Ignore_Class_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, Actual.Integer );
				Assert.AreEqual( this.Char, Actual.Char );
				Assert.AreEqual( this.Double, Actual.Double );
				Assert.AreEqual( this.String, Actual.String );
				Assert.AreEqual( this.Guid, Actual.Guid );
			}
		}

		#endregion

		#region Ignore_Class

		[Test]
		public void Ignore_Class() {

			AutoMapper2.CreateMap<Ignore_Class_Type, Ignore_Class_Type>();

			Ignore_Class_Type source = new Ignore_Class_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};

			Ignore_Class_Type destination = AutoMapper2.Map<Ignore_Class_Type, Ignore_Class_Type>( source );

			source.AssertEqual( destination );

		}

		public class Ignore_Class_Type {
			[IgnoreMap]
			public int Integer { get; set; }
			[IgnoreMap]
			public string String { get; set; }
			[IgnoreMap]
			public double Double { get; set; }
			[IgnoreMap]
			public Guid Guid { get; set; }
			[IgnoreMap]
			public char Char { get; set; }

			public void AssertEqual( Ignore_Class_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( 0, Actual.Integer );
				Assert.AreEqual( '\0', Actual.Char );
				Assert.AreEqual( 0.0, Actual.Double );
				Assert.AreEqual( null, Actual.String );
				Assert.AreEqual( Guid.Empty, Actual.Guid );
			}
		}

		#endregion

		#region Ignore_Type

		[Test]
		public void Ignore_Type() {

			AutoMapper2.CreateMap<Ignore_Type_Type, Ignore_Type_Type>();

			Ignore_Type_Type source = new Ignore_Type_Type {
				Property = new Ignore_Type_InnerType {
					Integer = 1
				}
			};

			Ignore_Type_Type destination = AutoMapper2.Map<Ignore_Type_Type, Ignore_Type_Type>( source );

			Assert.IsNotNull( destination );
			Assert.IsNull( destination.Property );

		}

		public class Ignore_Type_Type {
			public Ignore_Type_InnerType Property { get; set;}
		}

		[IgnoreMap]
		public class Ignore_Type_InnerType {
			public int Integer { get; set; }
			
		}

		#endregion

	}

}
