namespace MapDLib.Sample.Tests {
	using MapDLib.Sample.Models;
	using MapDLib.Sample.Some_Other_Project;
	using NUnit.Framework;

	[TestFixture]
	public class Irrelevant_Tests {

		[Test]
		public void This_Test_Is_Irrelevant() {
			// This test is irrelevant but insures the sample classes are referenced by this project
			SomeClass1 c = new SomeClass1();
			SomeClass1ViewModel m = new SomeClass1ViewModel();
			Assert.That( true, Is.EqualTo( true ) );
		}

	}
}