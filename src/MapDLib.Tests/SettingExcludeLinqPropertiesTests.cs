namespace MapDLib.Tests {

	#region using
	using System;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class SettingExcludeLinqPropertiesTests : BaseTest {

		[Test]
		public void ExcludeLinqProperties_Defaults_True() {
			Assert.AreEqual( true, MapD.Config.ExcludeLinqProperties );
		}

		[Test]
		public void ExcludeLinqProperties_Unlocked_Before_Map() {

			bool val = !MapD.Config.ExcludeLinqProperties;
			MapD.Config.ExcludeLinqProperties = val;

			MapD.Config.CreateMap<Irrelevant_Class, Irrelevant_Class>();

			Assert.AreEqual( val, MapD.Config.ExcludeLinqProperties );

			// Since we haven't called Copy(), this handles "before" for all 3 Copy* functions
		}

		[Test]
		public void ExcludeLinqProperties_Locked_After_Map1() {

			bool val = !MapD.Config.ExcludeLinqProperties;
			MapD.Config.CreateMap<Irrelevant_Class, Irrelevant_Class>();
			var dest = MapD.Copy<Irrelevant_Class, Irrelevant_Class>( new Irrelevant_Class() );

			try {
				MapD.Config.ExcludeLinqProperties = val;
				Assert.Fail( "Setting ExcludeLinqProperties after calling Copy should fail" );
			} catch (NotSupportedException ex) {
				Assert.IsTrue( ex.Message.Contains( "ExcludeLinqProperties" ), "It failed correctly, but didn't show a helpful message" );
			}

		}

		[Test]
		public void ExcludeLinqProperties_Locked_After_Map2() {

			bool val = !MapD.Config.ExcludeLinqProperties;
			MapD.Config.CreateMap<Irrelevant_Class, Irrelevant_Class>();
			Irrelevant_Class dest = new Irrelevant_Class();
			var changes = MapD.Copy<Irrelevant_Class, Irrelevant_Class>( new Irrelevant_Class(), ref dest );

			try {
				MapD.Config.ExcludeLinqProperties = val;
				Assert.Fail( "Setting ExcludeLinqProperties after calling Copy should fail" );
			} catch ( NotSupportedException ex ) {
				Assert.IsTrue( ex.Message.Contains( "ExcludeLinqProperties" ), "It failed correctly, but didn't show a helpful message" );
			}

		}

		[Test]
		public void ExcludeLinqProperties_Locked_After_MapBack() {

			bool val = !MapD.Config.ExcludeLinqProperties;
			MapD.Config.CreateMap<Irrelevant_Class, Irrelevant_Class>();
			Irrelevant_Class dest = new Irrelevant_Class();
			var changes = MapD.CopyBack<Irrelevant_Class, Irrelevant_Class>( new Irrelevant_Class(), ref dest );

			try {
				MapD.Config.ExcludeLinqProperties = val;
				Assert.Fail( "Setting ExcludeLinqProperties after calling Copy should fail" );
			} catch ( NotSupportedException ex ) {
				Assert.IsTrue( ex.Message.Contains( "ExcludeLinqProperties" ), "It failed correctly, but didn't show a helpful message" );
			}

		}

		private class Irrelevant_Class {
			public bool Boolean { get; set; }
		}

	}

}
