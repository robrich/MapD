namespace MapDLib.Tests {

	#region using
	using System;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class NonClassTests : BaseTest {

		[Test]
		public void Can_Map_Int_To_Double() {
			int source = 1;
			double dest = 0;
			double destTemplate = 1.0;

			dest = MapD.CopyType<int, double>( source );

			Assert.AreEqual( destTemplate, dest );
		}

		[Test]
		public void Can_Map_Double_To_String() {
			double source = 1.0;
			string dest = null;
			string destTemplate = source.ToString();

			dest = MapD.CopyType<double, string>( source );

			Assert.AreEqual( destTemplate, dest );
		}

		[Test]
		public void Cant_Map_ValueTypes_With_MapType() {

			try {
				ValueTypeClass dest = MapD.CopyType<ValueTypeClass, ValueTypeClass>( new ValueTypeClass() );
				Assert.Fail("Class types can't use CopyType<>");
			} catch (NotSupportedException ex) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsTrue( ex.Message.Contains( "call Copy()" ) );
			}

		}

		private class ValueTypeClass {
		}

	}

}
