namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using AutoMapper2Lib;
	using NUnit.Framework;

	#endregion

	public class ClassToClassTests : BaseTest {

		#region Class_To_Same_Class

		[Test]
		public void Class_To_Same_Class() {

			AutoMapper2.CreateMap<Class_To_Same_Class_Type, Class_To_Same_Class_Type>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};

			Class_To_Same_Class_Type destination = AutoMapper2.Map<Class_To_Same_Class_Type, Class_To_Same_Class_Type>( source );

			source.AssertEqual( destination );

		}

		[Test]
		public void Class_To_Same_Class_With_Null_Properties() {

			AutoMapper2.CreateMap<Class_To_Same_Class_Type, Class_To_Same_Class_Type>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type();
			Class_To_Same_Class_Type destination = AutoMapper2.Map<Class_To_Same_Class_Type, Class_To_Same_Class_Type>( source );
			source.AssertEqual( destination );

		}

		public class Class_To_Same_Class_Type {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }

			public void AssertEqual( Class_To_Same_Class_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, Actual.Integer );
				Assert.AreEqual( this.Char, Actual.Char );
				Assert.AreEqual( this.Double, Actual.Double );
				Assert.AreEqual( this.String, Actual.String );
				Assert.AreEqual( this.Guid, Actual.Guid );
			}
		}

		#endregion

		#region Class_To_Similar_Class

		[Test]
		public void Class_To_Similar_Class() {

			AutoMapper2.CreateMap<Class_To_Similar_Class_Type1, Class_To_Similar_Class_Type2>();

			Class_To_Similar_Class_Type1 source = new Class_To_Similar_Class_Type1 {
				Integer = 4321,
				Char = 'S',
				Double = 4321.4321,
				String = "gnirtS",
				Guid = Guid.NewGuid()
			};

			Class_To_Similar_Class_Type2 destination = AutoMapper2.Map<Class_To_Similar_Class_Type1, Class_To_Similar_Class_Type2>( source );

			source.AssertEqual( destination );

		}

		[Test]
		public void Class_To_Similar_Class_With_Null_Properties() {

			AutoMapper2.CreateMap<Class_To_Similar_Class_Type1, Class_To_Similar_Class_Type2>();

			Class_To_Similar_Class_Type1 source = new Class_To_Similar_Class_Type1();
			Class_To_Similar_Class_Type2 destination = AutoMapper2.Map<Class_To_Similar_Class_Type1, Class_To_Similar_Class_Type2>( source );
			source.AssertEqual( destination );

		}

		public class Class_To_Similar_Class_Type1 {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }

			public void AssertEqual( Class_To_Similar_Class_Type2 Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, Actual.Integer );
				Assert.AreEqual( this.Char, Actual.Char );
				Assert.AreEqual( this.Double, Actual.Double );
				Assert.AreEqual( this.String, Actual.String );
				Assert.AreEqual( this.Guid, Actual.Guid );
			}
		}
		public class Class_To_Similar_Class_Type2 {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }
		}

		#endregion

		#region Class_To_DifferentProperties_Class

		[Test]
		public void Class_To_DifferentProperties_Class() {

			AutoMapper2.CreateMap<Class_To_DifferentProperties_Class_Type1, Class_To_DifferentProperties_Class_Type2>();

			Class_To_DifferentProperties_Class_Type1 source = new Class_To_DifferentProperties_Class_Type1 {
				Integer = 4321,
				Char = 'S',
				Double = 4321,
				String = "g",
				Guid = Guid.NewGuid(),
				Number = 12345
			};

			Class_To_DifferentProperties_Class_Type2 destination = AutoMapper2.Map<Class_To_DifferentProperties_Class_Type1, Class_To_DifferentProperties_Class_Type2>( source );

			source.AssertEqual( destination );

		}

		[Test]
		public void Class_To_DifferentProperties_Class_With_Null_Properties() {

			AutoMapper2.CreateMap<Class_To_DifferentProperties_Class_Type1, Class_To_DifferentProperties_Class_Type2>();

			Class_To_DifferentProperties_Class_Type1 source = new Class_To_DifferentProperties_Class_Type1();
			Class_To_DifferentProperties_Class_Type2 destination = AutoMapper2.Map<Class_To_DifferentProperties_Class_Type1, Class_To_DifferentProperties_Class_Type2>( source );
			source.AssertEqual( destination );

		}

		public enum Class_To_DifferentProperties_Enum_Type1 {
			Item1,
			Item2,
			Item3
		}

		public class Class_To_DifferentProperties_Class_Type1 {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }
			public int Number { get; set; }

			public void AssertEqual( Class_To_DifferentProperties_Class_Type2 Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, (int)Actual.Integer, string.Format( "Expected: {0}, Actual: {1}", this.Integer, Actual.Integer ) );
				Assert.AreEqual( ( this.Char == '\0' ? null : this.Char.ToString() ), Actual.Char, string.Format( "Expected: {0}, Actual: {1}", this.Char, Actual.Char ) );
				Assert.AreEqual( (int)Math.Round( this.Double, 0 ), Actual.Double, string.Format( "Expected: {0}, Actual: {1}", this.Double, Actual.Double ) );
				Assert.AreEqual( this.String, ( Actual.String == '\0' ? null : Actual.String.ToString() ), string.Format( "Expected: {0}, Actual: {1}", this.String, Actual.String ) );
				Assert.AreEqual( this.Guid.ToString(), Actual.Guid, string.Format( "Expected: {0}, Actual: {1}", this.Guid, Actual.Guid ) );
				Assert.AreEqual( this.Number.ToString(), Actual.Number, string.Format( "Expected: {0}, Actual: {1}", this.Number, Actual.Number ) );
			}
		}
		public class Class_To_DifferentProperties_Class_Type2 {
			public double Integer { get; set; }
			public char String { get; set; }
			public int Double { get; set; }
			public string Guid { get; set; }
			public string Char { get; set; }
			public string Number { get; set; }
		}

		#endregion

		#region Class_To_LessProperties and MoreProperties_Class

		[Test]
		public void Class_To_LessProperties_Class() {

			AutoMapper2.CreateMap<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_LessProps_Type>();

			Class_To_Similar_Class_MoreProps_Type source = new Class_To_Similar_Class_MoreProps_Type {
				Integer = 4321,
				Char = 'S',
				Double = 4321.4321,
				String = "gnirtS",
				Guid = Guid.NewGuid(),
				Integer2 = 1234,
				Char2 = 'c',
				Double2 = 1234.1234,
				String2 = "String",
				Guid2 = Guid.NewGuid()
			};

			Class_To_Similar_Class_LessProps_Type destination = AutoMapper2.Map<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_LessProps_Type>( source );

			source.AssertEqual( destination );

		}

		[Test]
		public void Class_To_LessProperties_Class_With_Null_Properties() {

			AutoMapper2.CreateMap<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_LessProps_Type>();

			Class_To_Similar_Class_MoreProps_Type source = new Class_To_Similar_Class_MoreProps_Type();
			Class_To_Similar_Class_LessProps_Type destination = AutoMapper2.Map<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_LessProps_Type>( source );
			source.AssertEqual( destination );

		}

		[Test]
		public void Class_To_MoreProperties_Class() {

			AutoMapper2.CreateMap<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_LessProps_Type>();

			Class_To_Similar_Class_LessProps_Type source = new Class_To_Similar_Class_LessProps_Type {
				Integer = 4321,
				Char = 'S',
				Double = 4321.4321,
				String = "gnirtS",
				Guid = Guid.NewGuid(),
			};
			Class_To_Similar_Class_MoreProps_Type destination = new Class_To_Similar_Class_MoreProps_Type();

			var changeList = AutoMapper2.MapBack<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_LessProps_Type>( source, destination );
			source.AssertEqual( destination, new Class_To_Similar_Class_MoreProps_Type() );
		}

		[Test]
		public void Class_To_MoreProperties_Class_With_Null_Properties() {

			AutoMapper2.CreateMap<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_LessProps_Type>();

			Class_To_Similar_Class_LessProps_Type source = new Class_To_Similar_Class_LessProps_Type();
			Class_To_Similar_Class_MoreProps_Type destination = new Class_To_Similar_Class_MoreProps_Type();

			var changeList = AutoMapper2.MapBack<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_LessProps_Type>( source, destination );
			source.AssertEqual( destination, new Class_To_Similar_Class_MoreProps_Type() );
		}

		[Test]
		public void Class_To_MoreProperties_With_Template_Class() {

			AutoMapper2.CreateMap<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_MoreProps_Type>();
			AutoMapper2.CreateMap<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_LessProps_Type>();

			Class_To_Similar_Class_LessProps_Type source = new Class_To_Similar_Class_LessProps_Type {
				Integer = 4321,
				Char = 'S',
				Double = 4321.4321,
				String = "gnirtS",
				Guid = Guid.NewGuid(),
			};
			Class_To_Similar_Class_MoreProps_Type destTemplate = new Class_To_Similar_Class_MoreProps_Type {
				Integer = 4321,
				Char = 'S',
				Double = 4321.4321,
				String = "gnirtS",
				Guid = Guid.NewGuid(),
				Integer2 = 1234,
				Char2 = 'c',
				Double2 = 1234.1234,
				String2 = "String",
				Guid2 = Guid.NewGuid(),
			};
			// Copy destTemplate since to avoid changing the base comparison
			// If this doesn't work, other tests would've failed too
			Class_To_Similar_Class_MoreProps_Type destination = AutoMapper2.Map<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_MoreProps_Type>( destTemplate );
			Assert.IsNotNull( destination );

			var changeList = AutoMapper2.MapBack<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_LessProps_Type>( source, destination );

			source.AssertEqual( destination, destTemplate );

		}

		[Test]
		public void Class_To_MoreProperties_With_Template_Class_With_Null_Properties() {

			AutoMapper2.CreateMap<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_MoreProps_Type>();
			AutoMapper2.CreateMap<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_LessProps_Type>();

			Class_To_Similar_Class_LessProps_Type source = new Class_To_Similar_Class_LessProps_Type();
			Class_To_Similar_Class_MoreProps_Type destTemplate = new Class_To_Similar_Class_MoreProps_Type {
				Integer = 4321,
				Char = 'S',
				Double = 4321.4321,
				String = "gnirtS",
				Guid = Guid.NewGuid(),
				Integer2 = 1234,
				Char2 = 'c',
				Double2 = 1234.1234,
				String2 = "String",
				Guid2 = Guid.NewGuid(),
			};
			// Copy destTemplate since to avoid changing the base comparison
			// If this doesn't work, other tests would've failed too
			Class_To_Similar_Class_MoreProps_Type destination = AutoMapper2.Map<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_MoreProps_Type>( destTemplate );
			Assert.IsNotNull( destination );

			var changeList = AutoMapper2.MapBack<Class_To_Similar_Class_MoreProps_Type, Class_To_Similar_Class_LessProps_Type>( source, destination );
			source.AssertEqual( destination, destTemplate );

		}

		public class Class_To_Similar_Class_MoreProps_Type {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }
			public int Integer2 { get; set; }
			public string String2 { get; set; }
			public double Double2 { get; set; }
			public Guid Guid2 { get; set; }
			public char Char2 { get; set; }

			public void AssertEqual( Class_To_Similar_Class_LessProps_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, Actual.Integer );
				Assert.AreEqual( this.Char, Actual.Char );
				Assert.AreEqual( this.Double, Actual.Double );
				Assert.AreEqual( this.String, Actual.String );
				Assert.AreEqual( this.Guid, Actual.Guid );
			}
		}
		public class Class_To_Similar_Class_LessProps_Type {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }

			public void AssertEqual( Class_To_Similar_Class_MoreProps_Type Actual, Class_To_Similar_Class_MoreProps_Type Template ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, Actual.Integer );
				Assert.AreEqual( this.Char, Actual.Char );
				Assert.AreEqual( this.Double, Actual.Double );
				Assert.AreEqual( this.String, Actual.String );
				Assert.AreEqual( this.Guid, Actual.Guid );
				Assert.AreEqual( Template.Integer2, Actual.Integer2 );
				Assert.AreEqual( Template.Char2, Actual.Char2 );
				Assert.AreEqual( Template.Double2, Actual.Double2 );
				Assert.AreEqual( Template.String2, Actual.String2 );
				Assert.AreEqual( Template.Guid2, Actual.Guid2 );
			}
		}

		#endregion
		
	}

}
