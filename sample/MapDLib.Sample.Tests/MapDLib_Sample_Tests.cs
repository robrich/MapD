namespace MapDLib.Sample.Tests {
	using NUnit.Framework;

	[TestFixture]
	public class MapDLib_Sample_Tests {

		[Test]
		public void TestMappings() {
			// Fire up MapD
			MapD.Config.CreateMapsFromAllAssembliesInPath();

			// Ask MapD to validate everything it discovered
			// If any properties don't map correctly, the exception thrown will tell you where and why
			MapD.Assert.AssertConfigurationIsValid();
		}

	}
}