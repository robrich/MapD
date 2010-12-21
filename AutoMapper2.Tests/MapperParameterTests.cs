namespace MapDLib.Tests {

	#region using
	using System;
	using MapDLib;
	using NUnit.Framework;

	#endregion

	public class MapperParameterTests : BaseTest {

		[Test]
		public void MapBack_Fails_With_NonNull_Returns_NonNull() {
			MapD.Config.CreateMap<object, object>();
			object source = new object();
			object dest = null;
			MapD.CopyBack<object, object>( source, ref dest );
			Assert.IsNotNull( dest );
		}

		[Test]
		public void MapBack_With_Null_Returns_Null() {
			MapD.Config.CreateMap<object, object>();
			object source = null;
			object dest = null;
			MapD.CopyBack<object, object>( source, ref dest );
			Assert.IsNull( dest );
		}

		[Test]
		public void Map_With_Null_Returns_Null() {
			MapD.Config.CreateMap<object, object>();
			object result = MapD.Copy<object, object>( null );
			Assert.IsNull( result );
		}

		[Test]
		public void CreateMap() {
			MapD.Config.CreateMap<object, object>();
			// If it didn't throw, we're fine
		}

		[Test]
		public void EmptyConfiguration_Invalid() {
			try {
				MapD.Assert.AssertConfigurationIsValid();
				Assert.Fail( "Null configuration was fine" );
			} catch ( ArgumentNullException ex ) {
				Assert.AreEqual( "You haven't created any maps", ex.Message );
				// It successfully failed
			}
		}

		[Test]
		public void SingleConfiguration_Valid() {
			MapD.Config.CreateMap<object, object>();
			object o = MapD.Copy<object, object>( new object() );
			MapD.Assert.AssertConfigurationIsValid();
		}

	}

}
