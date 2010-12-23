namespace MapDLib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using NUnit.Framework;

	#endregion

	public class InterfaceTests : BaseTest {
		
		[Test]
		public void Interface_To_Same_Interface() {

			MapD.Config.CreationStrategy = type => type == typeof( IInterface_To_Same_Interface_Type ) ? new Interface_To_Same_Interface_Type() : null;
			MapD.Config.CreateMap<IInterface_To_Same_Interface_Type, IInterface_To_Same_Interface_Type>();

			IInterface_To_Same_Interface_Type source = new Interface_To_Same_Interface_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};

			IInterface_To_Same_Interface_Type destination = MapD.Copy<IInterface_To_Same_Interface_Type, IInterface_To_Same_Interface_Type>( source );

			source.AssertEqual( destination );

		}

		[Test]
		public void Interface_To_Same_Interface_With_Null_Properties() {

			MapD.Config.CreationStrategy = type => type == typeof( IInterface_To_Same_Interface_Type ) ? new Interface_To_Same_Interface_Type() : null;
			MapD.Config.CreateMap<IInterface_To_Same_Interface_Type, IInterface_To_Same_Interface_Type>();

			IInterface_To_Same_Interface_Type source = new Interface_To_Same_Interface_Type();
			IInterface_To_Same_Interface_Type destination = MapD.Copy<IInterface_To_Same_Interface_Type, IInterface_To_Same_Interface_Type>( source );
			source.AssertEqual( destination );

		}

		private interface IInterface_To_Same_Interface_Type {
			int Integer { get; set; }
			string String { get; set; }
			double Double { get; set; }
			Guid Guid { get; set; }
			char Char { get; set; }

			void AssertEqual( IInterface_To_Same_Interface_Type Actual );
		}
		private class Interface_To_Same_Interface_Type : IInterface_To_Same_Interface_Type {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }

			public void AssertEqual( IInterface_To_Same_Interface_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, Actual.Integer );
				Assert.AreEqual( this.Char, Actual.Char );
				Assert.AreEqual( this.Double, Actual.Double );
				Assert.AreEqual( this.String, Actual.String );
				Assert.AreEqual( this.Guid, Actual.Guid );
			}
		}

		[Test]
		public void Interface_WriteOnly() {

			MapD.Config.CreationStrategy = type => type == typeof( IInterface_WriteOnly_Type ) ? new Interface_WriteOnly_Type() : null;
			MapD.Config.CreateMap<Interface_WriteOnly_Source_Type, IInterface_WriteOnly_Type>();

			Interface_WriteOnly_Source_Type source = new Interface_WriteOnly_Source_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};

			IInterface_WriteOnly_Type destination = MapD.Copy<Interface_WriteOnly_Source_Type, IInterface_WriteOnly_Type>( source );

			Interface_WriteOnly_Type destinationConcrete = destination as Interface_WriteOnly_Type;

			Assert.IsNotNull( destinationConcrete );
			destinationConcrete.AssertEqual( source );
		}

		[Test]
		public void Interface_WriteOnly_With_Null_Properties() {

			MapD.Config.CreationStrategy = type => type == typeof( IInterface_WriteOnly_Type ) ? new Interface_WriteOnly_Type() : null;
			MapD.Config.CreateMap<Interface_WriteOnly_Source_Type, IInterface_WriteOnly_Type>();

			Interface_WriteOnly_Source_Type source = new Interface_WriteOnly_Source_Type();
			IInterface_WriteOnly_Type destination = MapD.Copy<Interface_WriteOnly_Source_Type, IInterface_WriteOnly_Type>( source );

			Interface_WriteOnly_Type destinationConcrete = destination as Interface_WriteOnly_Type;

			Assert.IsNotNull( destinationConcrete );
			destinationConcrete.AssertEqual( source );
		}

		[Test]
		public void Interface_WriteOnly_Results() {

			MapD.Config.CreationStrategy = type => type == typeof( IInterface_WriteOnly_Type ) ? new Interface_WriteOnly_Type() : null;
			MapD.Config.CreateMap<Interface_WriteOnly_Source_Type, IInterface_WriteOnly_Type>();

			Interface_WriteOnly_Source_Type source = new Interface_WriteOnly_Source_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};
			IInterface_WriteOnly_Type destination = new Interface_WriteOnly_Type();

			List<PropertyChangedResults> results = MapD.Copy<Interface_WriteOnly_Source_Type, IInterface_WriteOnly_Type>( source, ref destination );

			Assert.IsNotNull( results );
			// one for each of the 5 properties
			Assert.AreEqual( 5, results.Count, "Got a different number of results: " + string.Join( ", ", results.Select(i => i.ToString()).ToArray() ) );

			// Skip the "created object" one
			foreach ( var change in results ) {
				Assert.AreEqual( "Can't read original property", change.Destination.PropertyValue );
			}

			Interface_WriteOnly_Type destinationConcrete = destination as Interface_WriteOnly_Type;

			Assert.IsNotNull( destinationConcrete );
			destinationConcrete.AssertEqual( source );
		}

		[IgnoreMap( IgnoreDirection.MapBack )]
		private interface IInterface_WriteOnly_Type {
			int Integer { set; }
			string String { set; }
			double Double { set; }
			Guid Guid { set; }
			char Char { set; }
		}
		private class Interface_WriteOnly_Type : IInterface_WriteOnly_Type {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }

			public void AssertEqual( Interface_WriteOnly_Source_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, Actual.Integer );
				Assert.AreEqual( this.Char, Actual.Char );
				Assert.AreEqual( this.Double, Actual.Double );
				Assert.AreEqual( this.String, Actual.String );
				Assert.AreEqual( this.Guid, Actual.Guid );
			}
		}
		private class Interface_WriteOnly_Source_Type {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }
		}

		[Test]
		public void Interface_ReadOnly() {

			MapD.Config.CreateMap<IInterface_ReadOnly_Type, Interface_ReadOnly_Destination_Type>();

			IInterface_ReadOnly_Type source = new Interface_ReadOnly_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};

			Interface_ReadOnly_Destination_Type destination = MapD.Copy<IInterface_ReadOnly_Type, Interface_ReadOnly_Destination_Type>( source );

			source.AssertEqual( destination );

		}

		[Test]
		public void Interface_ReadOnly_With_Null_Properties() {

			MapD.Config.CreateMap<IInterface_ReadOnly_Type, Interface_ReadOnly_Destination_Type>();

			IInterface_ReadOnly_Type source = new Interface_ReadOnly_Type();
			Interface_ReadOnly_Destination_Type destination = MapD.Copy<IInterface_ReadOnly_Type, Interface_ReadOnly_Destination_Type>( source );
			source.AssertEqual( destination );

		}

		[Test]
		public void Interface_ReadOnly_Results() {

			MapD.Config.CreateMap<IInterface_ReadOnly_Type, Interface_ReadOnly_Destination_Type>();

			IInterface_ReadOnly_Type source = new Interface_ReadOnly_Type();
			Interface_ReadOnly_Destination_Type destination = new Interface_ReadOnly_Destination_Type();
			List<PropertyChangedResults> results = MapD.Copy<IInterface_ReadOnly_Type, Interface_ReadOnly_Destination_Type>( source, ref destination );

			source.AssertEqual( destination );
			Assert.NotNull( results );
			Assert.AreEqual( 0, results.Count );
		}

		[IgnoreMap( IgnoreDirection.MapBack )]
		private interface IInterface_ReadOnly_Type {
			int Integer { get; }
			string String { get; }
			double Double { get; }
			Guid Guid { get; }
			char Char { get; }

			void AssertEqual( Interface_ReadOnly_Destination_Type Actual );
		}
		private class Interface_ReadOnly_Type : IInterface_ReadOnly_Type {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }

			public void AssertEqual( Interface_ReadOnly_Destination_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, Actual.Integer );
				Assert.AreEqual( this.Char, Actual.Char );
				Assert.AreEqual( this.Double, Actual.Double );
				Assert.AreEqual( this.String, Actual.String );
				Assert.AreEqual( this.Guid, Actual.Guid );
			}
		}
		private class Interface_ReadOnly_Destination_Type {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }
		}

	}

}
