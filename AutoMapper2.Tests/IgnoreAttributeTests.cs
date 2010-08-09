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

		#region Ignore_Property

		[Test]
		public void Ignore_Property() {

			AutoMapper2.CreateMap<Ignore_Property_Type, Ignore_Property_Type>();

			Ignore_Property_Type source = new Ignore_Property_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};

			Ignore_Property_Type destination = AutoMapper2.Map<Ignore_Property_Type, Ignore_Property_Type>( source );

			source.AssertEqual( destination );

		}

		public class Ignore_Property_Type {
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

			public void AssertEqual( Ignore_Property_Type Actual ) {
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

		#region Ignore_From_Class

		[Test]
		public void Ignore_From_Class() {

			AutoMapper2.CreateMap<Ignore_From_Class_Type1, Ignore_From_Class_Type2>();

			Ignore_From_Class_Type1 source = new Ignore_From_Class_Type1 {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};

			Ignore_From_Class_Type2 destination = AutoMapper2.Map<Ignore_From_Class_Type1, Ignore_From_Class_Type2>( source );

			source.AssertEqual( destination );

		}

		public class Ignore_From_Class_Type1 {
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

			public void AssertEqual( Ignore_From_Class_Type2 Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( 0, Actual.Integer );
				Assert.AreEqual( '\0', Actual.Char );
				Assert.AreEqual( 0.0, Actual.Double );
				Assert.AreEqual( null, Actual.String );
				Assert.AreEqual( Guid.Empty, Actual.Guid );
			}
		}

		public class Ignore_From_Class_Type2 {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }
		}

		#endregion

		#region Ignore_From_Type

		[Test]
		public void Ignore_From_Type() {

			AutoMapper2.CreateMap<Ignore_From_Type_Type1, Ignore_From_Type_Type2>();

			Ignore_From_Type_Type1 source = new Ignore_From_Type_Type1 {
				Property = new Ignore_From_Type_InnerType1 {
					Integer = 1
				}
			};

			Ignore_From_Type_Type2 destination = AutoMapper2.Map<Ignore_From_Type_Type1, Ignore_From_Type_Type2>( source );

			Assert.IsNotNull( destination );
			Assert.IsNull( destination.Property );

		}

		public class Ignore_From_Type_Type1 {
			public Ignore_From_Type_InnerType1 Property { get; set; }
		}

		[IgnoreMap]
		public class Ignore_From_Type_InnerType1 {
			public int Integer { get; set; }

		}

		public class Ignore_From_Type_Type2 {
			public Ignore_From_Type_InnerType2 Property { get; set; }
		}

		public class Ignore_From_Type_InnerType2 {
			public int Integer { get; set; }

		}

		#endregion

		#region Ignore_To_Class

		[Test]
		public void Ignore_To_Class() {

			AutoMapper2.CreateMap<Ignore_To_Class_Type1, Ignore_To_Class_Type2>();

			Ignore_To_Class_Type1 source = new Ignore_To_Class_Type1 {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};

			Ignore_To_Class_Type2 destination = AutoMapper2.Map<Ignore_To_Class_Type1, Ignore_To_Class_Type2>( source );

			source.AssertEqual( destination );

		}

		public class Ignore_To_Class_Type1 {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }

			public void AssertEqual( Ignore_To_Class_Type2 Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( 0, Actual.Integer );
				Assert.AreEqual( '\0', Actual.Char );
				Assert.AreEqual( 0.0, Actual.Double );
				Assert.AreEqual( null, Actual.String );
				Assert.AreEqual( Guid.Empty, Actual.Guid );
			}
		}

		public class Ignore_To_Class_Type2 {
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
		}

		#endregion

		#region Ignore_To_Type

		[Test]
		public void Ignore_To_Type() {

			AutoMapper2.CreateMap<Ignore_To_Type_Type1, Ignore_To_Type_Type2>();

			Ignore_To_Type_Type1 source = new Ignore_To_Type_Type1 {
				Property = new Ignore_To_Type_InnerType1 {
					Integer = 1
				}
			};

			Ignore_To_Type_Type2 destination = AutoMapper2.Map<Ignore_To_Type_Type1, Ignore_To_Type_Type2>( source );

			Assert.IsNotNull( destination );
			Assert.IsNull( destination.Property );

		}

		public class Ignore_To_Type_Type1 {
			public Ignore_To_Type_InnerType1 Property { get; set; }
		}

		public class Ignore_To_Type_InnerType1 {
			public int Integer { get; set; }

		}

		public class Ignore_To_Type_Type2 {
			public Ignore_To_Type_InnerType2 Property { get; set; }
		}

		[IgnoreMap]
		public class Ignore_To_Type_InnerType2 {
			public int Integer { get; set; }

		}

		#endregion

		#region Ignore_Property_Partial

		[Test]
		public void Ignore_Property_Partial() {

			AutoMapper2.CreateMap<Ignore_Property_Partial_Type, Ignore_Property_Partial_Type>();

			Ignore_Property_Partial_Type source = new Ignore_Property_Partial_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};

			Ignore_Property_Partial_Type destination = AutoMapper2.Map<Ignore_Property_Partial_Type, Ignore_Property_Partial_Type>( source );

			source.AssertEqual( destination );

			Ignore_Property_Partial_Type source2 = new Ignore_Property_Partial_Type();
			AutoMapper2.MapBack<Ignore_Property_Partial_Type, Ignore_Property_Partial_Type>( destination, ref source2 );

			destination.AssertEqualBack( source2 );

		}

		public class Ignore_Property_Partial_Type {
			[IgnoreMap(IgnoreDirection.MapBack)]
			public int Integer { get; set; }
			[IgnoreMap( IgnoreDirection.MapBack )]
			public string String { get; set; }
			[IgnoreMap( IgnoreDirection.MapBack )]
			public double Double { get; set; }
			[IgnoreMap( IgnoreDirection.MapBack )]
			public Guid Guid { get; set; }
			[IgnoreMap( IgnoreDirection.MapBack )]
			public char Char { get; set; }

			public void AssertEqual( Ignore_Property_Partial_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, Actual.Integer );
				Assert.AreEqual( this.Char, Actual.Char );
				Assert.AreEqual( this.Double, Actual.Double );
				Assert.AreEqual( this.String, Actual.String );
				Assert.AreEqual( this.Guid, Actual.Guid );
			}
			public void AssertEqualBack( Ignore_Property_Partial_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( 0, Actual.Integer );
				Assert.AreEqual( '\0', Actual.Char );
				Assert.AreEqual( 0.0, Actual.Double );
				Assert.AreEqual( null, Actual.String );
				Assert.AreEqual( Guid.Empty, Actual.Guid );
			}
		}

		#endregion

		#region Ignore_Property_Partial_Back

		[Test]
		public void Ignore_Property_Partial_Back() {

			AutoMapper2.CreateMap<Ignore_Property_Partial_Back_Type, Ignore_Property_Partial_Back_Type>();

			Ignore_Property_Partial_Back_Type source = new Ignore_Property_Partial_Back_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};

			Ignore_Property_Partial_Back_Type destination = AutoMapper2.Map<Ignore_Property_Partial_Back_Type, Ignore_Property_Partial_Back_Type>( source );

			source.AssertEqual( destination );

			Ignore_Property_Partial_Back_Type source2 = new Ignore_Property_Partial_Back_Type();
			AutoMapper2.MapBack<Ignore_Property_Partial_Back_Type, Ignore_Property_Partial_Back_Type>( source, ref source2 );

			source.AssertEqualBack( source2 );

		}

		public class Ignore_Property_Partial_Back_Type {
			[IgnoreMap( IgnoreDirection.Map )]
			public int Integer { get; set; }
			[IgnoreMap( IgnoreDirection.Map )]
			public string String { get; set; }
			[IgnoreMap( IgnoreDirection.Map )]
			public double Double { get; set; }
			[IgnoreMap( IgnoreDirection.Map )]
			public Guid Guid { get; set; }
			[IgnoreMap( IgnoreDirection.Map )]
			public char Char { get; set; }

			public void AssertEqual( Ignore_Property_Partial_Back_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( 0, Actual.Integer );
				Assert.AreEqual( '\0', Actual.Char );
				Assert.AreEqual( 0.0, Actual.Double );
				Assert.AreEqual( null, Actual.String );
				Assert.AreEqual( Guid.Empty, Actual.Guid );
			}
			public void AssertEqualBack( Ignore_Property_Partial_Back_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, Actual.Integer );
				Assert.AreEqual( this.Char, Actual.Char );
				Assert.AreEqual( this.Double, Actual.Double );
				Assert.AreEqual( this.String, Actual.String );
				Assert.AreEqual( this.Guid, Actual.Guid );
			}
		}

		#endregion
		#region Ignore_Property_Partial_Both

		[Test]
		public void Ignore_Property_Partial_Both() {

			AutoMapper2.CreateMap<Ignore_Property_Partial_Both_Type, Ignore_Property_Partial_Both_Type>();

			Ignore_Property_Partial_Both_Type source = new Ignore_Property_Partial_Both_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};

			Ignore_Property_Partial_Both_Type destination = AutoMapper2.Map<Ignore_Property_Partial_Both_Type, Ignore_Property_Partial_Both_Type>( source );

			source.AssertEqual( destination );

			Ignore_Property_Partial_Both_Type source2 = new Ignore_Property_Partial_Both_Type();
			AutoMapper2.MapBack<Ignore_Property_Partial_Both_Type, Ignore_Property_Partial_Both_Type>( source, ref source2 );

			source.AssertEqual( source2 );

		}

		public class Ignore_Property_Partial_Both_Type {
			[IgnoreMap( IgnoreDirection.Map | IgnoreDirection.MapBack )]
			public int Integer { get; set; }
			[IgnoreMap( IgnoreDirection.Map | IgnoreDirection.MapBack )]
			public string String { get; set; }
			[IgnoreMap( IgnoreDirection.Map | IgnoreDirection.MapBack )]
			public double Double { get; set; }
			[IgnoreMap( IgnoreDirection.Map | IgnoreDirection.MapBack )]
			public Guid Guid { get; set; }
			[IgnoreMap( IgnoreDirection.Map | IgnoreDirection.MapBack )]
			public char Char { get; set; }

			public void AssertEqual( Ignore_Property_Partial_Both_Type Actual ) {
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
