namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using AutoMapper2Lib;
	using NUnit.Framework;

	#endregion

	public class MapperParameterTests : BaseTest {

		[Test]
		public void MapBack_Fails_With_NonNull_Returns_NonNull() {
			AutoMapper2.Config.CreateMap<object, object>();
			object source = new object();
			object dest = null;
			AutoMapper2.MapBack<object, object>( source, ref dest );
			Assert.IsNotNull( dest );
		}

		[Test]
		public void MapBack_With_Null_Returns_Null() {
			AutoMapper2.Config.CreateMap<object, object>();
			object source = null;
			object dest = null;
			AutoMapper2.MapBack<object, object>( source, ref dest );
			Assert.IsNull( dest );
		}

		[Test]
		public void Map_With_Null_Returns_Null() {
			AutoMapper2.Config.CreateMap<object, object>();
			object result = AutoMapper2.Map<object, object>( null );
			Assert.IsNull( result );
		}

		[Test]
		public void CreateMap() {
			AutoMapper2.Config.CreateMap<object, object>();
			// If it didn't throw, we're fine
		}

		[Test]
		public void EmptyConfiguration_Invalid() {
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
				Assert.Fail( "Null configuration was fine" );
			} catch ( ArgumentNullException ex ) {
				Assert.AreEqual( "You haven't created any maps", ex.Message );
				// It successfully failed
			}
		}

		[Test]
		public void SingleConfiguration_Valid() {
			AutoMapper2.Config.CreateMap<object, object>();
			object o = AutoMapper2.Map<object, object>( new object() );
			AutoMapper2.Assert.AssertConfigurationIsValid();
		}

	}

}
