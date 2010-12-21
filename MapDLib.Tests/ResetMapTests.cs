namespace MapDLib.Tests {

	#region using
	using System;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class ResetMapTests : BaseTest {

		[Test]
		public void Reset_Resets_Map() {

			MapD.Config.CreateMap<RemapClass,RemapClass>();

			MapD.Config.ResetMap();

			try {
				RemapClass destination = MapD.Copy<RemapClass, RemapClass>( new RemapClass() );
				Assert.Fail("ResetMap didn't remove the map to RemapClass");
			} catch (MissingMapException) {
				// It successfully failed
			}

		}

		[Test]
		public void Reset_Resets_Linq_Property() {

			MapD.Config.CreateMap<RemapClass, RemapClass>();

			MapD.Config.ResetMap();

			MapD.Config.ExcludeLinqProperties = !MapD.Config.ExcludeLinqProperties;
			// It worked
		}

		[Test]
		public void No_Reset_CantSet_Linq_Property() {

			MapD.Config.CreateMap<RemapClass, RemapClass>();

			try {
				MapD.Config.ExcludeLinqProperties = !MapD.Config.ExcludeLinqProperties;
			} catch ( NotSupportedException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsTrue( ex.Message.Contains( "ExcludeLinqProperties" ) );
			}
		}

		[Test]
		public void Reset_Resets_IgnoreProperties_Property() {

			Assert.AreEqual( PropertyIs.NotSet, MapD.Config.IgnorePropertiesIf );
			MapD.Config.CreateMap<RemapClass, RemapClass>();

			MapD.Config.ResetMap();

			MapD.Config.IgnorePropertiesIf = PropertyIs.WriteOnly;
			// It worked
		}

		[Test]
		public void No_Reset_CantSet_IgnoreProperties_Property() {

			MapD.Config.CreateMap<RemapClass, RemapClass>();

			try {
				MapD.Config.IgnorePropertiesIf = PropertyIs.WriteOnly;
			} catch ( NotSupportedException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsTrue( ex.Message.Contains( "IgnorePropertiesIf" ) );
			}
		}

		private class RemapClass {
			public int Property1 { get; set; }
		}

	}

}
