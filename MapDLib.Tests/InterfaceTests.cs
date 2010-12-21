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
		public void Class_To_Same_Class_With_Null_Properties() {

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

	}

}
