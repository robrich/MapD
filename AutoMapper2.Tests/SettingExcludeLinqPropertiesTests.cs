namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class SettingExcludeLinqPropertiesTests : BaseTest {

		[Test]
		public void ExcludeLinqProperties_Defaults_True() {
			Assert.AreEqual( true, AutoMapper2.ExcludeLinqProperties );
		}

		[Test]
		public void ExcludeLinqProperties_Unlocked_Before_Map() {

			bool val = !AutoMapper2.ExcludeLinqProperties;
			AutoMapper2.ExcludeLinqProperties = val;

			AutoMapper2.CreateMap<Irrelevant_Class, Irrelevant_Class>();

			Assert.AreEqual( val, AutoMapper2.ExcludeLinqProperties );

			// Since we haven't called Map(), this handles "before" for all 3 Map* functions
		}

		[Test]
		public void ExcludeLinqProperties_Locked_After_Map1() {

			bool val = !AutoMapper2.ExcludeLinqProperties;
			AutoMapper2.CreateMap<Irrelevant_Class, Irrelevant_Class>();
			var dest = AutoMapper2.Map<Irrelevant_Class, Irrelevant_Class>( new Irrelevant_Class() );

			try {
				AutoMapper2.ExcludeLinqProperties = val;
				Assert.Fail( "Setting ExcludeLinqProperties after calling Map should fail" );
			} catch (NotSupportedException ex) {
				Assert.IsTrue( ex.Message.Contains( "ExcludeLinqProperties" ), "It failed correctly, but didn't show a helpful message" );
			}

		}

		[Test]
		public void ExcludeLinqProperties_Locked_After_Map2() {

			bool val = !AutoMapper2.ExcludeLinqProperties;
			AutoMapper2.CreateMap<Irrelevant_Class, Irrelevant_Class>();
			Irrelevant_Class dest = new Irrelevant_Class();
			var changes = AutoMapper2.Map<Irrelevant_Class, Irrelevant_Class>( new Irrelevant_Class(), ref dest );

			try {
				AutoMapper2.ExcludeLinqProperties = val;
				Assert.Fail( "Setting ExcludeLinqProperties after calling Map should fail" );
			} catch ( NotSupportedException ex ) {
				Assert.IsTrue( ex.Message.Contains( "ExcludeLinqProperties" ), "It failed correctly, but didn't show a helpful message" );
			}

		}

		[Test]
		public void ExcludeLinqProperties_Locked_After_MapBack() {

			bool val = !AutoMapper2.ExcludeLinqProperties;
			AutoMapper2.CreateMap<Irrelevant_Class, Irrelevant_Class>();
			Irrelevant_Class dest = new Irrelevant_Class();
			var changes = AutoMapper2.MapBack<Irrelevant_Class, Irrelevant_Class>( new Irrelevant_Class(), ref dest );

			try {
				AutoMapper2.ExcludeLinqProperties = val;
				Assert.Fail( "Setting ExcludeLinqProperties after calling Map should fail" );
			} catch ( NotSupportedException ex ) {
				Assert.IsTrue( ex.Message.Contains( "ExcludeLinqProperties" ), "It failed correctly, but didn't show a helpful message" );
			}

		}

		public class Irrelevant_Class {
			public bool Boolean { get; set; }
		}

	}

}
