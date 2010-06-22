namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using AutoMapper2Lib;
	using NUnit.Framework;

	#endregion

	public class MapperParameterTests : BaseTest {

		[Test]
		public void MapBack_Fails_With_NonNull_Returns_NonNull() {
			AutoMapper2.CreateMap<object, object>();
			object source = new object();
			object dest = null;
			AutoMapper2.MapBack<object, object>( source, ref dest );
			Assert.IsNotNull( dest );
		}

		[Test]
		public void MapBack_With_Null_Returns_Null() {
			AutoMapper2.CreateMap<object, object>();
			object source = null;
			object dest = null;
			object result = AutoMapper2.MapBack<object, object>( source, ref dest );
			Assert.IsNull( result );
		}

		[Test]
		public void Map_With_Null_Returns_Null() {
			AutoMapper2.CreateMap<object, object>();
			object result = AutoMapper2.Map<object, object>( null );
			Assert.IsNull( result );
		}

		[Test]
		public void CreateMap() {
			AutoMapper2.CreateMap<object, object>();
			// If it didn't throw, we're fine
		}

		[Test]
		public void EmptyConfiguration_Invalid() {
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "Null configuration was fine" );
			} catch ( ArgumentNullException ex ) {
				Assert.AreEqual( "You've not created any maps", ex.Message );
				// It successfully failed
			}
		}

		[Test]
		public void SingleConfiguration_Valid() {
			AutoMapper2.CreateMap<object, object>();
			object o = AutoMapper2.Map<object, object>( new object() );
			AutoMapper2.AssertConfigurationIsValid();
		}

	}

}