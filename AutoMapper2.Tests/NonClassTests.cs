namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class NonClassTests : BaseTest {

		[Test]
		public void Can_Map_Int_To_Double() {
			int source = 1;
			double dest = 0;
			double destTemplate = 1.0;

			dest = AutoMapper2.MapType<int, double>( source );

			Assert.AreEqual( destTemplate, dest );
		}

		[Test]
		public void Can_Map_Double_To_String() {
			double source = 1.0;
			string dest = null;
			string destTemplate = source.ToString();

			dest = AutoMapper2.MapType<double, string>( source );

			Assert.AreEqual( destTemplate, dest );
		}

		[Test]
		public void Cant_Map_ValueTypes_With_MapType() {

			try {
				ValueTypeClass dest = AutoMapper2.MapType<ValueTypeClass, ValueTypeClass>( new ValueTypeClass() );
				Assert.Fail("Class types can't use MapType<>");
			} catch (NotSupportedException ex) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsTrue( ex.Message.Contains( "call Map()" ) );
			}

		}

		public class ValueTypeClass {
		}

	}

}
