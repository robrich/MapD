namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class ResetMapTests : BaseTest {

		[Test]
		public void Reset_Resets_Map() {

			AutoMapper2.Config.CreateMap<RemapClass,RemapClass>();

			AutoMapper2.Config.ResetMap();

			try {
				RemapClass destination = AutoMapper2.Map<RemapClass, RemapClass>( new RemapClass() );
				Assert.Fail("ResetMap didn't remove the map to RemapClass");
			} catch (MissingMapException) {
				// It successfully failed
			}

		}

		[Test]
		public void Reset_Resets_Linq_Property() {

			AutoMapper2.Config.CreateMap<RemapClass, RemapClass>();

			AutoMapper2.Config.ResetMap();

			AutoMapper2.Config.ExcludeLinqProperties = !AutoMapper2.Config.ExcludeLinqProperties;
			// It worked
		}

		[Test]
		public void No_Reset_CantSet_Linq_Property() {

			AutoMapper2.Config.CreateMap<RemapClass, RemapClass>();

			try {
				AutoMapper2.Config.ExcludeLinqProperties = !AutoMapper2.Config.ExcludeLinqProperties;
			} catch ( NotSupportedException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsTrue( ex.Message.Contains( "ExcludeLinqProperties" ) );
			}
		}

		[Test]
		public void Reset_Resets_IgnoreProperties_Property() {

			Assert.AreEqual( PropertyIs.NotSet, AutoMapper2.Config.IgnorePropertiesIf );
			AutoMapper2.Config.CreateMap<RemapClass, RemapClass>();

			AutoMapper2.Config.ResetMap();

			AutoMapper2.Config.IgnorePropertiesIf = PropertyIs.WriteOnly;
			// It worked
		}

		[Test]
		public void No_Reset_CantSet_IgnoreProperties_Property() {

			AutoMapper2.Config.CreateMap<RemapClass, RemapClass>();

			try {
				AutoMapper2.Config.IgnorePropertiesIf = PropertyIs.WriteOnly;
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
