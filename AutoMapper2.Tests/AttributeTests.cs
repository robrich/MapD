namespace MapDLib.Tests {

	#region using
	using System;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class AttributeTests : BaseTest {

		[Test]
		public void IgnoreMap_With_None_Fails() {
			try {
				IgnoreMapAttribute attr = new IgnoreMapAttribute( IgnoreDirection.None );
				Assert.Fail( "IgnroeDirection.None isn't valid" );
			} catch ( ArgumentOutOfRangeException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( "IgnoreDirection", ex.ParamName );
			}
		}

		// These silly tests are purely to get 100% code coverage on the affected classes

		[Test]
		public void IgnorePropertiesIf_With_Invalid_Enum_Fails() {
			try {
				PropertyIs enumval = (PropertyIs)( -1 );
				IgnorePropertiesIfAttribute attr = new IgnorePropertiesIfAttribute( enumval );
				Assert.Fail("An invalid enum isn't valid");
			} catch ( ArgumentOutOfRangeException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( "PropertyIs", ex.ParamName );
			}
		}

		[Test]
		public void RemapProperty_With_Blank_Name_Fails() {
			try {
				RemapPropertyAttribute attr = new RemapPropertyAttribute( null );
				Assert.Fail( "An invalid name isn't valid" );
			} catch ( ArgumentNullException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( "MapPropertyName", ex.ParamName );
			}
		}

		[Test]
		public void MapFrom_With_Blank_Type_Fails() {
			try {
				MapFromAttribute attr = new MapFromAttribute( null );
				Assert.Fail( "A null type isn't valid" );
			} catch ( ArgumentNullException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( "Type", ex.ParamName );
			}
		}

		[Test]
		public void MapListFrom_With_Blank_Type_Fails() {
			try {
				MapListFromListOfAttribute attr = new MapListFromListOfAttribute( null );
				Assert.Fail( "A null type isn't valid" );
			} catch ( ArgumentNullException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( "Type", ex.ParamName );
			}
		}

		/*
		// This is a better test, but is run on test initialization, and therefore can't be "tested" this way

		[MapFrom(null)]
		public class SomeClass {
			
		}
		*/

	}

}
