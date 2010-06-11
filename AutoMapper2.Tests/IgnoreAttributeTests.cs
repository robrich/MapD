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
			[MapIgnore]
			public int Integer { get; set; }
			[MapIgnore]
			public string String { get; set; }
			[MapIgnore]
			public double Double { get; set; }
			[MapIgnore]
			public Guid Guid { get; set; }
			[MapIgnore]
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

	}

}
