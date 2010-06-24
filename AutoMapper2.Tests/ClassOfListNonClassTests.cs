namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using AutoMapper2Lib;
	using NUnit.Framework;

	#endregion

	public class ClassOfListNonClassTests : BaseTest {
		
		#region ChangeList_Contains_Changes_SameClass_Test

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Test() {

			AutoMapper2.CreateMap<Class_To_Same_Class_Type, Class_To_Same_Class_Type>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type {
				Integer = new List<int> { 1234, 4321 },
				Char = new List<char> { 'c', 'd' },
				Double = new List<double> { 1234.1234, 4321.4321 },
				String = new List<string> { "String", "gnirtS" },
				Guid = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
			};
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type();

			List<PropertyChangedResults> changeList = AutoMapper2.Map<Class_To_Same_Class_Type, Class_To_Same_Class_Type>( source, ref destination );

			source.AssertEqual( destination );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_PartiallyFilled_Test() {

			AutoMapper2.CreateMap<Class_To_Same_Class_Type, Class_To_Same_Class_Type>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type {
				Integer = new List<int> { 1234, 4321 },
				Char = new List<char> { 'c', 'd' },
				Double = new List<double> { 1234.1234, 4321.4321 },
				String = new List<string> { "String", "gnirtS" },
				Guid = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
			};
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type {
				Integer = new List<int> { 1234 },
				Char = new List<char> { 'c' },
				Double = new List<double> { 1234.1234 },
				String = new List<string> { "String" },
				Guid = new List<Guid> { source.Guid[0] }
			};

			List<PropertyChangedResults> changeList = AutoMapper2.Map<Class_To_Same_Class_Type, Class_To_Same_Class_Type>( source, ref destination );

			source.AssertEqual( destination );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Equal_Test() {

			AutoMapper2.CreateMap<Class_To_Same_Class_Type, Class_To_Same_Class_Type>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type {
				Integer = new List<int> { 1234, 4321 },
				Char = new List<char> { 'c', 'd' },
				Double = new List<double> { 1234.1234, 4321.4321 },
				String = new List<string> { "String", "gnirtS" },
				Guid = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
			};
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type {
				Integer = new List<int> { 1234, 4321 },
				Char = new List<char> { 'c', 'd' },
				Double = new List<double> { 1234.1234, 4321.4321 },
				String = new List<string> { "String", "gnirtS" },
				Guid = new List<Guid> { source.Guid[0], source.Guid[1] }
			};

			List<PropertyChangedResults> changeList = AutoMapper2.Map<Class_To_Same_Class_Type, Class_To_Same_Class_Type>( source, ref destination );

			source.AssertEqual( destination );
			Assert.IsNotNull( changeList );
			Assert.AreEqual( 0, changeList.Count );

		}
		
		[Test]
		public void ChangeList_Contains_Changes_SameClass_Test_With_Null_Properties() {

			AutoMapper2.CreateMap<Class_To_Same_Class_Type, Class_To_Same_Class_Type>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type();
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type() {
				Char = new List<char>(),
				Double = new List<double>(),
				Guid = new List<Guid>(),
				Integer = new List<int>(),
				String = new List<string>()
			};
			List<PropertyChangedResults> changeList = AutoMapper2.Map<Class_To_Same_Class_Type, Class_To_Same_Class_Type>( source, ref destination );

			source.AssertEqual( destination );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Back_Test() {

			AutoMapper2.CreateMap<Class_To_Same_Class_Type, Class_To_Same_Class_Type>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type {
				Integer = new List<int> { 1234, 4321 },
				Char = new List<char> { 'c', 'd' },
				Double = new List<double> { 1234.1234, 4321.4321 },
				String = new List<string> { "String", "gnirtS" },
				Guid = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
			};
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type();

			List<PropertyChangedResults> changeList = AutoMapper2.MapBack<Class_To_Same_Class_Type, Class_To_Same_Class_Type>( source, ref destination );

			source.AssertEqual( destination );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Back_PartiallyFilled_Test() {

			AutoMapper2.CreateMap<Class_To_Same_Class_Type, Class_To_Same_Class_Type>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type {
				Integer = new List<int> { 1234, 4321 },
				Char = new List<char> { 'c', 'd' },
				Double = new List<double> { 1234.1234, 4321.4321 },
				String = new List<string> { "String", "gnirtS" },
				Guid = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
			};
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type {
				Integer = new List<int> { 1234 },
				Char = new List<char> { 'c' },
				Double = new List<double> { 1234.1234 },
				String = new List<string> { "String" },
				Guid = new List<Guid> { source.Guid[0] }
			};

			List<PropertyChangedResults> changeList = AutoMapper2.MapBack<Class_To_Same_Class_Type, Class_To_Same_Class_Type>( source, ref destination );

			source.AssertEqual( destination );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Back_Equal_Test() {

			AutoMapper2.CreateMap<Class_To_Same_Class_Type, Class_To_Same_Class_Type>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type {
				Integer = new List<int> { 1234, 4321 },
				Char = new List<char> { 'c', 'd' },
				Double = new List<double> { 1234.1234, 4321.4321 },
				String = new List<string> { "String", "gnirtS" },
				Guid = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
			};
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type {
				Integer = new List<int> { 1234, 4321 },
				Char = new List<char> { 'c', 'd' },
				Double = new List<double> { 1234.1234, 4321.4321 },
				String = new List<string> { "String", "gnirtS" },
				Guid = new List<Guid> { source.Guid[0], source.Guid[1] }
			};

			List<PropertyChangedResults> changeList = AutoMapper2.MapBack<Class_To_Same_Class_Type, Class_To_Same_Class_Type>( source, ref destination );

			source.AssertEqual( destination );
			Assert.IsNotNull( changeList );
			Assert.AreEqual( 0, changeList.Count );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Back_Test_With_Null_Properties() {

			AutoMapper2.CreateMap<Class_To_Same_Class_Type, Class_To_Same_Class_Type>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type();
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type() {
				Char = new List<char>(),
				Double = new List<double>(),
				Guid = new List<Guid>(),
				Integer = new List<int>(),
				String = new List<string>()
			};
			List<PropertyChangedResults> changeList = AutoMapper2.MapBack<Class_To_Same_Class_Type, Class_To_Same_Class_Type>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.IsNotNull( destination.Char );
			Assert.IsNotNull( destination.Double );
			Assert.IsNotNull( destination.Guid );
			Assert.IsNotNull( destination.Integer );
			Assert.IsNotNull( destination.String );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Back_Test_With_Null_Objects() {

			AutoMapper2.CreateMap<Class_To_Same_Class_Type, Class_To_Same_Class_Type>();

			Class_To_Same_Class_Type source = null;
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type() {
				Char = new List<char>(),
				Double = new List<double>(),
				Guid = new List<Guid>(),
				Integer = new List<int>(),
				String = new List<string>()
			};
			List<PropertyChangedResults> changeList = AutoMapper2.MapBack<Class_To_Same_Class_Type, Class_To_Same_Class_Type>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.IsNotNull( destination.Char );
			Assert.IsNotNull( destination.Double );
			Assert.IsNotNull( destination.Guid );
			Assert.IsNotNull( destination.Integer );
			Assert.IsNotNull( destination.String );

		}

		public class Class_To_Same_Class_Type {
			public List<int> Integer { get; set; }
			public List<string> String { get; set; }
			public List<double> Double { get; set; }
			public List<Guid> Guid { get; set; }
			public List<char> Char { get; set; }

			public void AssertEqual( Class_To_Same_Class_Type Actual ) {
				Assert.IsNotNull( Actual );
				AssertListsAreEqual( this.Integer, Actual.Integer );
				AssertListsAreEqual( this.Char, Actual.Char );
				AssertListsAreEqual( this.Double, Actual.Double );
				AssertListsAreEqual( this.String, Actual.String );
				AssertListsAreEqual( this.Guid, Actual.Guid );
			}
		}

		#endregion

		#region Class_To_DifferentProperties_Class

		[Test]
		public void Class_To_DifferentProperties_Class() {

			AutoMapper2.CreateMap<Class_To_DifferentProperties_Class_Type1, Class_To_DifferentProperties_Class_Type2>();

			Class_To_DifferentProperties_Class_Type1 source = new Class_To_DifferentProperties_Class_Type1 {
				Integer = new List<int> { 1234, 4321 },
				Char = new List<char> { 'c', 'd' },
				Double = new List<double> { 1234, 4321 },
				String = new List<string> { "S", "g" },
				Guid = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
				Number = new List<int> { 12345, 54321 }
			};
			Class_To_DifferentProperties_Class_Type2 destinationTemplate = new Class_To_DifferentProperties_Class_Type2 {
				Integer = new List<double> { 1234, 4321 },
				Char = new List<string> { "c", "d" },
				Double = new List<int> { 1234, 4321 },
				String = new List<char> { 'S', 'g' },
				Guid = new List<string> { source.Guid[0].ToString(), source.Guid[1].ToString() },
				Number = new List<string> { "12345", "54321" }
			};

			Class_To_DifferentProperties_Class_Type2 destination = AutoMapper2.Map<Class_To_DifferentProperties_Class_Type1, Class_To_DifferentProperties_Class_Type2>( source );

			destinationTemplate.AssertEqual( destination );

		}

		[Test]
		public void Class_To_DifferentProperties_Class_With_Null_Properties() {

			AutoMapper2.CreateMap<Class_To_DifferentProperties_Class_Type1, Class_To_DifferentProperties_Class_Type2>();

			Class_To_DifferentProperties_Class_Type1 source = new Class_To_DifferentProperties_Class_Type1();
			Class_To_DifferentProperties_Class_Type2 destinationTemplate = new Class_To_DifferentProperties_Class_Type2();

			Class_To_DifferentProperties_Class_Type2 destination = AutoMapper2.Map<Class_To_DifferentProperties_Class_Type1, Class_To_DifferentProperties_Class_Type2>( source );
			destinationTemplate.AssertEqual( destination );

		}

		public class Class_To_DifferentProperties_Class_Type1 {
			public List<int> Integer { get; set; }
			public List<string> String { get; set; }
			public List<double> Double { get; set; }
			public List<Guid> Guid { get; set; }
			public List<char> Char { get; set; }
			public List<int> Number { get; set; }

			/*
			public void AssertEqual( Class_To_DifferentProperties_Class_Type2 Actual ) {
				Assert.IsNotNull( Actual );
				AssertListsAreEqual( this.Integer, Actual.Integer );
				AssertListsAreEqual( this.Char, Actual.Char );
				AssertListsAreEqual( this.Double, Actual.Double );
				AssertListsAreEqual( this.String, Actual.String );
				AssertListsAreEqual( this.Guid, Actual.Guid );
				AssertListsAreEqual( this.Number, Actual.Number );

				Assert.AreEqual( this.Integer, (int)Actual.Integer, string.Format( "Expected: {0}, Actual: {1}", this.Integer, Actual.Integer ) );
				Assert.AreEqual( ( this.Char == '\0' ? null : this.Char.ToString() ), Actual.Char, string.Format( "Expected: {0}, Actual: {1}", this.Char, Actual.Char ) );
				Assert.AreEqual( (int)Math.Round( this.Double, 0 ), Actual.Double, string.Format( "Expected: {0}, Actual: {1}", this.Double, Actual.Double ) );
				Assert.AreEqual( this.String, ( Actual.String == '\0' ? null : Actual.String.ToString() ), string.Format( "Expected: {0}, Actual: {1}", this.String, Actual.String ) );
				Assert.AreEqual( this.Guid.ToString(), Actual.Guid, string.Format( "Expected: {0}, Actual: {1}", this.Guid, Actual.Guid ) );
				Assert.AreEqual( this.Number.ToString(), Actual.Number, string.Format( "Expected: {0}, Actual: {1}", this.Number, Actual.Number ) );
			}
			*/
		}
		public class Class_To_DifferentProperties_Class_Type2 {
			public List<double> Integer { get; set; }
			public List<char> String { get; set; }
			public List<int> Double { get; set; }
			public List<string> Guid { get; set; }
			public List<string> Char { get; set; }
			public List<string> Number { get; set; }

			public void AssertEqual( Class_To_DifferentProperties_Class_Type2 Actual ) {
				Assert.IsNotNull( Actual );
				AssertListsAreEqual( this.Integer, Actual.Integer );
				AssertListsAreEqual( this.Char, Actual.Char );
				AssertListsAreEqual( this.Double, Actual.Double );
				AssertListsAreEqual( this.String, Actual.String );
				AssertListsAreEqual( this.Guid, Actual.Guid );
				AssertListsAreEqual( this.Number, Actual.Number );
			}
		}

		#endregion

	}

}
