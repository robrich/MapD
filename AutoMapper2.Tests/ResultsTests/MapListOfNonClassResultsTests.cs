namespace AutoMapper2Lib.Tests.ResultsTests {

	#region using
	using System.Collections.Generic;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class MapListOfNonClassResultsTests : BaseTest {

		#region NullSourceandDestList_NoChanges
		[Test]
		public void NullSourceandDestList_NoChanges() {

			AutoMapper2.CreateMap<List<int>, List<int>>();

			List<int> source = null;
			List<int> destination = null;

			List<PropertyChangedResults> changes = AutoMapper2.Map<List<int>, List<int>>( source, ref destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 0, changes.Count );

			// Insure destination didn't change
			Assert.AreEqual( null, destination );
		}
		#endregion

		#region NullSourceFullDestList_YieldDifferences
		[Test]
		public void NullSourceFullDestList_YieldDifferences() {

			AutoMapper2.CreateMap<List<int>, List<int>>();

			List<int> source = null;
			List<int> destination = new List<int>() {
				1,
				2
			};
			List<int> destinationRef = destination;

			List<PropertyChangedResults> changes = AutoMapper2.Map<List<int>, List<int>>( source, ref destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 1, changes.Count );

			// Assert we got the correct comparison results
			Assert.IsNotNull( changes[0].Source );
			Assert.IsNotNull( changes[0].Destination );
			PropertyChangedResult sourceChanges = changes[0].Source;
			PropertyChangedResult destinationChanges = changes[0].Destination;

			Assert.AreEqual( source, sourceChanges.Object );
			Assert.AreEqual( typeof( List<int> ), sourceChanges.ObjectType );
			Assert.AreEqual( "this", sourceChanges.PropertyName );
			Assert.AreEqual( typeof( List<int> ), sourceChanges.PropertyType );
			Assert.AreEqual( source, sourceChanges.PropertyValue );

			Assert.AreEqual( destinationRef, destinationChanges.Object );
			Assert.AreEqual( typeof( List<int> ), destinationChanges.ObjectType );
			Assert.AreEqual( "this", destinationChanges.PropertyName );
			Assert.AreEqual( typeof( List<int> ), destinationChanges.PropertyType );
			Assert.AreEqual( destinationRef, destinationChanges.PropertyValue );

			// Insure destination did change
			Assert.IsNull( destination );
		}
		#endregion

		#region FullSourceNullDestList_YieldDifferences
		[Test]
		public void FullSourceNullDestList_YieldDifferences() {

			AutoMapper2.CreateMap<List<int>, List<int>>();

			List<int> source = new List<int>() {
				1,
				2
			};
			List<int> destination = null;
			List<int> destinationRef = destination;

			List<PropertyChangedResults> changes = AutoMapper2.Map<List<int>, List<int>>( source, ref destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 3, changes.Count ); // 1 to instanciate the object, two for each entry added

			// Assert we got the correct comparison results
			Assert.IsNotNull( changes[0].Source );
			Assert.IsNotNull( changes[0].Destination );
			PropertyChangedResult sourceChanges = changes[0].Source;
			PropertyChangedResult destinationChanges = changes[0].Destination;

			Assert.AreEqual( source, sourceChanges.Object );
			Assert.AreEqual( typeof( List<int> ), sourceChanges.ObjectType );
			Assert.AreEqual( "this", sourceChanges.PropertyName );
			Assert.AreEqual( typeof( List<int> ), sourceChanges.PropertyType );
			Assert.AreEqual( source, sourceChanges.PropertyValue );

			Assert.AreEqual( destinationRef, destinationChanges.Object );
			Assert.AreEqual( typeof( List<int> ), destinationChanges.ObjectType );
			Assert.AreEqual( "this", destinationChanges.PropertyName );
			Assert.AreEqual( typeof( List<int> ), destinationChanges.PropertyType );
			Assert.AreEqual( destinationRef, destinationChanges.PropertyValue );

			// Insure destination did change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 2, destination.Count );
		}
		#endregion

		#region FullSourceEmptyDestList_YieldDifferences
		[Test]
		public void FullSourceEmptyDestList_YieldDifferences() {

			AutoMapper2.CreateMap<List<int>, List<int>>();

			List<int> source = new List<int>() {
				1,
				2
			};
			List<int> destination = new List<int>();

			List<PropertyChangedResults> changes = AutoMapper2.Map<List<int>, List<int>>( source, ref destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 2, changes.Count );

			// Assert we got the correct comparison results
			for ( int i = 0; i < changes.Count; i++ ) {
				PropertyChangedResults change = changes[i];
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
				int sourceVal = source[i];
				object destVal = null;

				PropertyChangedResult sourceChanges = change.Source;
				PropertyChangedResult destinationChanges = change.Destination;

				Assert.AreEqual( source, sourceChanges.Object );
				Assert.AreEqual( source.GetType(), sourceChanges.ObjectType );
				Assert.IsNotNull( sourceChanges.PropertyName );
				Assert.IsNotNull( sourceChanges.PropertyType );

				Assert.AreEqual( destination, destinationChanges.Object );
				Assert.AreEqual( destination.GetType(), destinationChanges.ObjectType );
				Assert.IsNotNull( destinationChanges.PropertyName );
				Assert.IsNotNull( destinationChanges.PropertyType );

				Assert.AreEqual( typeof( int ), sourceChanges.PropertyType );
				Assert.AreEqual( sourceVal, sourceChanges.PropertyValue );
				Assert.AreEqual( typeof( int ), destinationChanges.PropertyType );
				Assert.AreEqual( destVal, destinationChanges.PropertyValue );
			}

			// Insure destination did change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 2, destination.Count );
		}
		#endregion

		#region EmptySourceFullDestList_YieldDifferences
		[Test]
		public void EmptySourceFullDestList_YieldDifferences() {

			AutoMapper2.CreateMap<List<int>, List<int>>();

			List<int> source = new List<int>();
			List<int> destination = new List<int>() {
				1,
				2
			};

			List<PropertyChangedResults> changes = AutoMapper2.Map<List<int>, List<int>>( source, ref destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 2, changes.Count );

			// Assert we got the correct comparison results
			for ( int i = 0; i < changes.Count; i++ ) {
				PropertyChangedResults change = changes[i];
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
				object sourceVal = null;
				int destVal = i + 1;

				PropertyChangedResult sourceChanges = change.Source;
				PropertyChangedResult destinationChanges = change.Destination;

				Assert.AreEqual( source, sourceChanges.Object );
				Assert.AreEqual( source.GetType(), sourceChanges.ObjectType );
				Assert.IsNotNull( sourceChanges.PropertyName );
				Assert.IsNotNull( sourceChanges.PropertyType );

				Assert.AreEqual( destination, destinationChanges.Object );
				Assert.AreEqual( destination.GetType(), destinationChanges.ObjectType );
				Assert.IsNotNull( destinationChanges.PropertyName );
				Assert.IsNotNull( destinationChanges.PropertyType );

				Assert.AreEqual( typeof( int ), sourceChanges.PropertyType );
				Assert.AreEqual( sourceVal, sourceChanges.PropertyValue );
				Assert.AreEqual( typeof( int ), destinationChanges.PropertyType );
				Assert.AreEqual( destVal, destinationChanges.PropertyValue );
			}

			// Insure destination did change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 0, destination.Count );
		}
		#endregion

		#region IdenticalSourceandDestList_NoDifferences
		[Test]
		public void IdenticalSourceandDestList_NoDifferences() {

			AutoMapper2.CreateMap<List<int>, List<int>>();

			List<int> source = new List<int>() {
				1,
				2
			};
			List<int> destination = new List<int>() {
				1,
				2
			};

			List<PropertyChangedResults> changes = AutoMapper2.Map<List<int>, List<int>>( source, ref destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 0, changes.Count );

			// Insure destination didn't change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 2, destination.Count );
		}
		#endregion

		#region ExtraSourceList_YieldDifferences
		[Test]
		public void ExtraSourceList_YieldDifferences() {

			AutoMapper2.CreateMap<List<int>, List<int>>();

			List<int> source = new List<int>() {
				1,
				2
			};
			List<int> destination = new List<int>() {
				2
			};

			List<PropertyChangedResults> changes = AutoMapper2.Map<List<int>, List<int>>( source, ref destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 1, changes.Count );

			// Assert we got the correct comparison results
			for ( int i = 0; i < changes.Count; i++ ) {
				PropertyChangedResults change = changes[i];
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
				int sourceVal = 1;
				object destVal = null;

				PropertyChangedResult sourceChanges = change.Source;
				PropertyChangedResult destinationChanges = change.Destination;

				Assert.AreEqual( source, sourceChanges.Object );
				Assert.AreEqual( source.GetType(), sourceChanges.ObjectType );
				Assert.IsNotNull( sourceChanges.PropertyName );
				Assert.IsNotNull( sourceChanges.PropertyType );

				Assert.AreEqual( destination, destinationChanges.Object );
				Assert.AreEqual( destination.GetType(), destinationChanges.ObjectType );
				Assert.IsNotNull( destinationChanges.PropertyName );
				Assert.IsNotNull( destinationChanges.PropertyType );

				Assert.AreEqual( typeof( int ), sourceChanges.PropertyType );
				Assert.AreEqual( sourceVal, sourceChanges.PropertyValue );
				Assert.AreEqual( typeof( int ), destinationChanges.PropertyType );
				Assert.AreEqual( destVal, destinationChanges.PropertyValue );
			}

			// Insure destination did change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 2, destination.Count );
		}
		#endregion

		#region ExtraDestList_YieldDifferences
		[Test]
		public void ExtraDestList_YieldDifferences() {

			AutoMapper2.CreateMap<List<int>, List<int>>();

			List<int> source = new List<int>() {
				2
			};
			List<int> destination = new List<int>() {
				1,
				2
			};

			List<PropertyChangedResults> changes = AutoMapper2.Map<List<int>, List<int>>( source, ref destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 1, changes.Count );

			// Assert we got the correct comparison results
			for ( int i = 0; i < changes.Count; i++ ) {
				PropertyChangedResults change = changes[i];
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
				object sourceVal = null;
				int destVal = 1;

				PropertyChangedResult sourceChanges = change.Source;
				PropertyChangedResult destinationChanges = change.Destination;

				Assert.AreEqual( source, sourceChanges.Object );
				Assert.AreEqual( source.GetType(), sourceChanges.ObjectType );
				Assert.IsNotNull( sourceChanges.PropertyName );
				Assert.IsNotNull( sourceChanges.PropertyType );

				Assert.AreEqual( destination, destinationChanges.Object );
				Assert.AreEqual( destination.GetType(), destinationChanges.ObjectType );
				Assert.IsNotNull( destinationChanges.PropertyName );
				Assert.IsNotNull( destinationChanges.PropertyType );

				Assert.AreEqual( typeof( int ), sourceChanges.PropertyType );
				Assert.AreEqual( sourceVal, sourceChanges.PropertyValue );
				Assert.AreEqual( typeof( int ), destinationChanges.PropertyType );
				Assert.AreEqual( destVal, destinationChanges.PropertyValue );
			}

			// Insure destination did change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 1, destination.Count );
		}
		#endregion

		#region DissimilarList_YieldDifferences
		[Test]
		public void DissimilarList_YieldDifferences() {

			AutoMapper2.CreateMap<List<int>, List<double>>();

			List<int> source = new List<int>() {
				1,
				2
			};
			List<double > destination = new List<double>() {
				2
			};

			List<PropertyChangedResults> changes = AutoMapper2.Map<List<int>, List<double>>( source, ref destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 1, changes.Count );

			// Assert we got the correct comparison results
			for ( int i = 0; i < changes.Count; i++ ) {
				PropertyChangedResults change = changes[i];
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
				int sourceVal = 1;
				object destVal = null;

				PropertyChangedResult sourceChanges = change.Source;
				PropertyChangedResult destinationChanges = change.Destination;

				Assert.AreEqual( source, sourceChanges.Object );
				Assert.AreEqual( source.GetType(), sourceChanges.ObjectType );
				Assert.IsNotNull( sourceChanges.PropertyName );
				Assert.IsNotNull( sourceChanges.PropertyType );

				Assert.AreEqual( destination, destinationChanges.Object );
				Assert.AreEqual( destination.GetType(), destinationChanges.ObjectType );
				Assert.IsNotNull( destinationChanges.PropertyName );
				Assert.IsNotNull( destinationChanges.PropertyType );

				Assert.AreEqual( typeof( int ), sourceChanges.PropertyType );
				Assert.AreEqual( sourceVal, sourceChanges.PropertyValue );
				Assert.AreEqual( typeof( double ), destinationChanges.PropertyType );
				Assert.AreEqual( destVal, destinationChanges.PropertyValue );
			}

			// Insure destination did change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 2, destination.Count );
		}
		#endregion

	}

}
