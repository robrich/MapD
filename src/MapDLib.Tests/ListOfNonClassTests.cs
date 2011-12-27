namespace MapDLib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class ListOfNonClassTests : BaseTest {

		#region Change_ListInt_Test

		[Test]
		public void Change_ListInt_Test() {

			MapD.Config.CreateMap<List<int>, List<int>>();

			List<int> source = new List<int>() {
				123,
				321
			};

			List<int> destination = MapD.Copy<List<int>, List<int>>( source );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_Null_ListInt_Test

		[Test]
		public void Change_Null_ListInt_Test() {

			MapD.Config.CreateMap<List<int>, List<int>>();

			List<int> source = null;

			List<int> destination = MapD.Copy<List<int>, List<int>>( source );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_Empty_ListInt_Test

		[Test]
		public void Change_Empty_ListInt_Test() {

			MapD.Config.CreateMap<List<int>, List<int>>();

			List<int> source = new List<int>();

			List<int> destination = MapD.Copy<List<int>, List<int>>( source );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_ListDouble_Test

		[Test]
		public void Change_ListDouble_Test() {

			MapD.Config.CreateMap<List<double>, List<double>>();

			List<double> source = new List<double>() {
				123.123,
				321.321
			};

			List<double> destination = MapD.Copy<List<double>, List<double>>( source );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_ListString_Test

		[Test]
		public void Change_ListString_Test() {

			MapD.Config.CreateMap<List<string>, List<string>>();

			List<string> source = new List<string>() {
				"String",
				"gnirtS"
			};

			List<string> destination = MapD.Copy<List<string>, List<string>>( source );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_ListString_PartiallyFilled_Test

		[Test]
		public void Change_ListString_PartiallyFilled_Test() {

			MapD.Config.CreateMap<List<string>, List<string>>();

			List<string> source = new List<string>() {
				"String",
				"gnirtS"
			};
			List<string> destination = new List<string> {
				"String"
			};

			var changes = MapD.Copy<List<string>, List<string>>( source, ref destination );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_ListDateTime_Test

		[Test]
		public void Change_ListDateTime_Test() {

			MapD.Config.CreateMap<List<DateTime>, List<DateTime>>();

			List<DateTime> source = new List<DateTime>() {
				DateTime.Now,
				DateTime.Now.AddDays( 1 )
			};

			List<DateTime> destination = MapD.Copy<List<DateTime>, List<DateTime>>( source );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_ListInt_to_ListDouble_Test

		[Test]
		public void Change_ListInt_to_ListDouble_Test() {

			MapD.Config.CreateMap<List<int>, List<double>>();

			List<int> source = new List<int>() {
				123,
				321
			};
			List<double> expected = new List<double>() {
				123,
				321
			};

			List<double> destination = MapD.Copy<List<int>, List<double>>( source );

			AssertListsAreEqual( expected, destination );

		}

		#endregion

		#region Change_ListString_to_ListDouble_Test

		[Test]
		public void Change_ListString_to_ListDouble_Test() {

			MapD.Config.CreateMap<List<string>, List<double>>();

			List<string> source = new List<string>() {
				"123",
				"321"
			};
			List<double> expected = new List<double>() {
				123,
				321
			};

			List<double> destination = MapD.Copy<List<string>, List<double>>( source );

			AssertListsAreEqual( expected, destination );

		}

		#endregion

	}

}
