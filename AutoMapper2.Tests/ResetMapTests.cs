namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class ResetMapTests {

		[Test]
		public void RemapResetsMap() {

			AutoMapper2.CreateMap<RemapClass,RemapClass>();

			AutoMapper2.ResetMap();

			try {
				RemapClass destination = AutoMapper2.Map<RemapClass, RemapClass>( new RemapClass() );
				Assert.Fail();
			} catch (MissingMapException) {
				// It successfully failed
			}

		}

		[Test]
		public void RemapResetsLinqProperty() {

			AutoMapper2.CreateMap<RemapClass, RemapClass>();

			AutoMapper2.ResetMap();

			AutoMapper2.ExcludeLinqProperties = !AutoMapper2.ExcludeLinqProperties;
			// It worked
		}

		public class RemapClass {
			public int Property1 { get; set; }
		}

	}

}
