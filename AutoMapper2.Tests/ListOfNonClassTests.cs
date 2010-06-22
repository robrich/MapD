namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class ListOfNonClassTests : BaseTest {

		#region Change_ListInt_Test

		[Test]
		public void Change_ListInt_Test() {

			AutoMapper2.CreateMap<List<int>, List<int>>();

			List<int> source = new List<int>() {
				123,
				321
			};

			List<int> destination = AutoMapper2.Map<List<int>, List<int>>( source );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_Null_ListInt_Test

		[Test]
		public void Change_Null_ListInt_Test() {

			AutoMapper2.CreateMap<List<int>, List<int>>();

			List<int> source = null;

			List<int> destination = AutoMapper2.Map<List<int>, List<int>>( source );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_Empty_ListInt_Test

		[Test]
		public void Change_Empty_ListInt_Test() {

			AutoMapper2.CreateMap<List<int>, List<int>>();

			List<int> source = new List<int>();

			List<int> destination = AutoMapper2.Map<List<int>, List<int>>( source );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_ListDouble_Test

		[Test]
		public void Change_ListDouble_Test() {

			AutoMapper2.CreateMap<List<double>, List<double>>();

			List<double> source = new List<double>() {
				123.123,
				321.321
			};

			List<double> destination = AutoMapper2.Map<List<double>, List<double>>( source );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_ListString_Test

		[Test]
		public void Change_ListString_Test() {

			AutoMapper2.CreateMap<List<string>, List<string>>();

			List<string> source = new List<string>() {
				"String",
				"gnirtS"
			};

			List<string> destination = AutoMapper2.Map<List<string>, List<string>>( source );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_ListString_PartiallyFilled_Test

		[Test]
		public void Change_ListString_PartiallyFilled_Test() {

			AutoMapper2.CreateMap<List<string>, List<string>>();

			List<string> source = new List<string>() {
				"String",
				"gnirtS"
			};
			List<string> destination = new List<string> {
				"String"
			};

			var changes = AutoMapper2.Map<List<string>, List<string>>( source, ref destination );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_ListDateTime_Test

		[Test]
		public void Change_ListDateTime_Test() {

			AutoMapper2.CreateMap<List<DateTime>, List<DateTime>>();

			List<DateTime> source = new List<DateTime>() {
				DateTime.Now,
				DateTime.Now.AddDays( 1 )
			};

			List<DateTime> destination = AutoMapper2.Map<List<DateTime>, List<DateTime>>( source );

			AssertListsAreEqual( source, destination );

		}

		#endregion

		#region Change_ListInt_to_ListDouble_Test

		[Test]
		public void Change_ListInt_to_ListDouble_Test() {

			AutoMapper2.CreateMap<List<int>, List<double>>();

			List<int> source = new List<int>() {
				123,
				321
			};
			List<double> expected = new List<double>() {
				123,
				321
			};

			List<double> destination = AutoMapper2.Map<List<int>, List<double>>( source );

			AssertListsAreEqual( expected, destination );

		}

		#endregion

		#region Change_ListString_to_ListDouble_Test

		[Test]
		public void Change_ListString_to_ListDouble_Test() {

			AutoMapper2.CreateMap<List<string>, List<double>>();

			List<string> source = new List<string>() {
				"123",
				"321"
			};
			List<double> expected = new List<double>() {
				123,
				321
			};

			List<double> destination = AutoMapper2.Map<List<string>, List<double>>( source );

			AssertListsAreEqual( expected, destination );

		}

		#endregion

	}

}
