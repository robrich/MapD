namespace AutoMapper2Lib.Tests {

	#region using
	using System.Collections.Generic;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public abstract class BaseTest {

		[SetUp]
		public void Init() {
			AutoMapper2.ResetMap();
		}

		protected static void AssertListsAreEqual<T>( List<T> Expected, List<T> Actual ) {
			Assert.AreEqual( Expected == null, Actual == null );
			Assert.AreEqual( Expected == null ? 0 : Expected.Count, Actual == null ? 0 : Actual.Count );
			if ( Expected != null && Expected.Count > 0 ) {
				for ( int i = 0; i < Expected.Count; i++ ) {
					Assert.AreEqual( Expected[i], Actual[i] );
				}
			}
		}

	}

}
