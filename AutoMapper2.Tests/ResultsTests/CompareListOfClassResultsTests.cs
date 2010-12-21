namespace MapDLib.Tests.ResultsTests {

	#region using
	using System.Collections.Generic;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class CompareListOfClassResultsTests : BaseTest {

		#region NullSourceandDestList_NoChanges
		[Test]
		public void NullSourceandDestList_NoChanges() {

			MapD.Config.CreateMap<List<SimplePrimaryClass>, List<SimplePrimaryClass>>();

			List<SimplePrimaryClass> source = null;
			List<SimplePrimaryClass> destination = null;

			List<PropertyChangedResults> changes = MapD.Compare<List<SimplePrimaryClass>, List<SimplePrimaryClass>>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 0, changes.Count );

			// Insure destination didn't change
			Assert.AreEqual( null, destination );
		}
		#endregion

		#region NullSourceFullDestList_YieldDifferences
		[Test]
		public void NullSourceFullDestList_YieldDifferences() {

			MapD.Config.CreateMap<List<SimplePrimaryClass>, List<SimplePrimaryClass>>();

			List<SimplePrimaryClass> source = null;
			List<SimplePrimaryClass> destination = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 1,
					Integer = 1
				},
				new SimplePrimaryClass() {
					Key = 2,
					Integer = 2
				}
			};

			List<PropertyChangedResults> changes = MapD.Compare<List<SimplePrimaryClass>, List<SimplePrimaryClass>>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 1, changes.Count );

			// Assert we got the correct comparison results
			Assert.IsNotNull( changes[0].Source );
			Assert.IsNotNull( changes[0].Destination );
			PropertyChangedResult sourceChanges = changes[0].Source;
			PropertyChangedResult destinationChanges = changes[0].Destination;

			Assert.AreEqual( source, sourceChanges.Object );
			Assert.AreEqual( typeof( List<SimplePrimaryClass> ), sourceChanges.ObjectType );
			Assert.AreEqual( "this", sourceChanges.PropertyName );
			Assert.AreEqual( typeof( List<SimplePrimaryClass> ), sourceChanges.PropertyType );
			Assert.AreEqual( source, sourceChanges.PropertyValue );

			Assert.AreEqual( destination, destinationChanges.Object );
			Assert.AreEqual( typeof( List<SimplePrimaryClass> ), destinationChanges.ObjectType );
			Assert.AreEqual( "this", destinationChanges.PropertyName );
			Assert.AreEqual( typeof( List<SimplePrimaryClass> ), destinationChanges.PropertyType );
			Assert.AreEqual( destination, destinationChanges.PropertyValue );

			// Insure destination didn't change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 2, destination.Count );
		}
		#endregion

		#region FullSourceNullDestList_YieldDifferences
		[Test]
		public void FullSourceNullDestList_YieldDifferences() {

			MapD.Config.CreateMap<List<SimplePrimaryClass>, List<SimplePrimaryClass>>();

			List<SimplePrimaryClass> source = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 1,
					Integer = 1
				},
				new SimplePrimaryClass() {
					Key = 2,
					Integer = 2
				}
			};
			List<SimplePrimaryClass> destination = null;

			List<PropertyChangedResults> changes = MapD.Compare<List<SimplePrimaryClass>, List<SimplePrimaryClass>>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 1, changes.Count );

			// Assert we got the correct comparison results
			Assert.IsNotNull( changes[0].Source );
			Assert.IsNotNull( changes[0].Destination );
			PropertyChangedResult sourceChanges = changes[0].Source;
			PropertyChangedResult destinationChanges = changes[0].Destination;

			Assert.AreEqual( source, sourceChanges.Object );
			Assert.AreEqual( typeof( List<SimplePrimaryClass> ), sourceChanges.ObjectType );
			Assert.AreEqual( "this", sourceChanges.PropertyName );
			Assert.AreEqual( typeof( List<SimplePrimaryClass> ), sourceChanges.PropertyType );
			Assert.AreEqual( source, sourceChanges.PropertyValue );

			Assert.AreEqual( destination, destinationChanges.Object );
			Assert.AreEqual( typeof( List<SimplePrimaryClass> ), destinationChanges.ObjectType );
			Assert.AreEqual( "this", destinationChanges.PropertyName );
			Assert.AreEqual( typeof( List<SimplePrimaryClass> ), destinationChanges.PropertyType );
			Assert.AreEqual( destination, destinationChanges.PropertyValue );

			// Insure destination didn't change
			Assert.IsNull( destination );
		}
		#endregion

		#region FullSourceEmptyDestList_YieldDifferences
		[Test]
		public void FullSourceEmptyDestList_YieldDifferences() {

			MapD.Config.CreateMap<List<SimplePrimaryClass>, List<SimplePrimaryClass>>();

			List<SimplePrimaryClass> source = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 1,
					Integer = 1
				},
				new SimplePrimaryClass() {
					Key = 2,
					Integer = 2
				}
			};
			List<SimplePrimaryClass> destination = new List<SimplePrimaryClass>();

			List<PropertyChangedResults> changes = MapD.Compare<List<SimplePrimaryClass>, List<SimplePrimaryClass>>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 2, changes.Count );

			// Assert we got the correct comparison results
			for ( int i = 0; i < changes.Count; i++ ) {
				PropertyChangedResults change = changes[i];
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
				SimplePrimaryClass sourceVal = source[i];
				SimplePrimaryClass destVal = null;

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

				Assert.AreEqual( typeof( SimplePrimaryClass ), sourceChanges.PropertyType );
				Assert.AreEqual( sourceVal, sourceChanges.PropertyValue );
				Assert.AreEqual( typeof( SimplePrimaryClass ), destinationChanges.PropertyType );
				Assert.AreEqual( destVal, destinationChanges.PropertyValue );
			}

			// Insure destination didn't change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 0, destination.Count );
		}
		#endregion

		#region EmptySourceFullDestList_YieldDifferences
		[Test]
		public void EmptySourceFullDestList_YieldDifferences() {

			MapD.Config.CreateMap<List<SimplePrimaryClass>, List<SimplePrimaryClass>>();

			List<SimplePrimaryClass> source = new List<SimplePrimaryClass>();
			List<SimplePrimaryClass> destination = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 1,
					Integer = 1
				},
				new SimplePrimaryClass() {
					Key = 2,
					Integer = 2
				}
			};

			List<PropertyChangedResults> changes = MapD.Compare<List<SimplePrimaryClass>, List<SimplePrimaryClass>>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 2, changes.Count );

			// Assert we got the correct comparison results
			for ( int i = 0; i < changes.Count; i++ ) {
				PropertyChangedResults change = changes[i];
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
				SimplePrimaryClass sourceVal = null;
				SimplePrimaryClass destVal = destination[i];

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

				Assert.AreEqual( typeof( SimplePrimaryClass ), sourceChanges.PropertyType );
				Assert.AreEqual( sourceVal, sourceChanges.PropertyValue );
				Assert.AreEqual( typeof( SimplePrimaryClass ), destinationChanges.PropertyType );
				Assert.AreEqual( destVal, destinationChanges.PropertyValue );
			}

			// Insure destination didn't change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 2, destination.Count );
		}
		#endregion

		#region IdenticalSourceandDestList_NoDifferences
		[Test]
		public void IdenticalSourceandDestList_NoDifferences() {

			MapD.Config.CreateMap<List<SimplePrimaryClass>, List<SimplePrimaryClass>>();

			List<SimplePrimaryClass> source = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 1,
					Integer = 1
				},
				new SimplePrimaryClass() {
					Key = 2,
					Integer = 2
				}
			};
			List<SimplePrimaryClass> destination = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 1,
					Integer = 1
				},
				new SimplePrimaryClass() {
					Key = 2,
					Integer = 2
				}
			};

			List<PropertyChangedResults> changes = MapD.Compare<List<SimplePrimaryClass>, List<SimplePrimaryClass>>( source, destination );

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

			MapD.Config.CreateMap<List<SimplePrimaryClass>, List<SimplePrimaryClass>>();

			List<SimplePrimaryClass> source = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 1,
					Integer = 1
				},
				new SimplePrimaryClass() {
					Key = 2,
					Integer = 2
				}
			};
			List<SimplePrimaryClass> destination = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 2,
					Integer = 2
				}
			};

			List<PropertyChangedResults> changes = MapD.Compare<List<SimplePrimaryClass>, List<SimplePrimaryClass>>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 1, changes.Count );

			// Assert we got the correct comparison results
			for ( int i = 0; i < changes.Count; i++ ) {
				PropertyChangedResults change = changes[i];
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
				SimplePrimaryClass sourceVal = source[i];
				SimplePrimaryClass destVal = null;

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

				Assert.AreEqual( typeof( SimplePrimaryClass ), sourceChanges.PropertyType );
				Assert.AreEqual( sourceVal, sourceChanges.PropertyValue );
				Assert.AreEqual( typeof( SimplePrimaryClass ), destinationChanges.PropertyType );
				Assert.AreEqual( destVal, destinationChanges.PropertyValue );
			}

			// Insure destination didn't change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 1, destination.Count );
		}
		#endregion

		#region ExtraDestList_YieldDifferences
		[Test]
		public void ExtraDestList_YieldDifferences() {

			MapD.Config.CreateMap<List<SimplePrimaryClass>, List<SimplePrimaryClass>>();

			List<SimplePrimaryClass> source = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 2,
					Integer = 2
				}
			};
			List<SimplePrimaryClass> destination = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 1,
					Integer = 1
				},
				new SimplePrimaryClass() {
					Key = 2,
					Integer = 2
				}
			};
			SimplePrimaryClass destOne = destination[0];

			List<PropertyChangedResults> changes = MapD.Compare<List<SimplePrimaryClass>, List<SimplePrimaryClass>>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 1, changes.Count );

			// Assert we got the correct comparison results
			for ( int i = 0; i < changes.Count; i++ ) {
				PropertyChangedResults change = changes[i];
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
				SimplePrimaryClass sourceVal = null;
				SimplePrimaryClass destVal = destOne;

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

				Assert.AreEqual( typeof( SimplePrimaryClass ), sourceChanges.PropertyType );
				Assert.AreEqual( sourceVal, sourceChanges.PropertyValue );
				Assert.AreEqual( typeof( SimplePrimaryClass ), destinationChanges.PropertyType );
				Assert.AreEqual( destVal, destinationChanges.PropertyValue );
			}

			// Insure destination didn't change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 2, destination.Count );
		}
		#endregion

		#region DissimilarList_YieldDifferences
		[Test]
		public void DissimilarList_YieldDifferences() {

			MapD.Config.CreateMap<List<SimplePrimaryClass>, List<SimplePrimaryClass2>>();

			List<SimplePrimaryClass> source = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 1
				},
				new SimplePrimaryClass() {
					Key = 2
				}
			};
			List<SimplePrimaryClass2> destination = new List<SimplePrimaryClass2>() {
				new SimplePrimaryClass2() {
					Key = 2
				}
			};

			List<PropertyChangedResults> changes = MapD.Compare<List<SimplePrimaryClass>, List<SimplePrimaryClass2>>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 1, changes.Count );

			// Assert we got the correct comparison results
			for ( int i = 0; i < changes.Count; i++ ) {
				PropertyChangedResults change = changes[i];
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
				SimplePrimaryClass sourceVal = source[0];
				SimplePrimaryClass destVal = null;

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

				Assert.AreEqual( typeof( SimplePrimaryClass ), sourceChanges.PropertyType );
				Assert.AreEqual( sourceVal, sourceChanges.PropertyValue );
				Assert.AreEqual( typeof( SimplePrimaryClass2 ), destinationChanges.PropertyType );
				Assert.AreEqual( destVal, destinationChanges.PropertyValue );
			}

			// Insure destination didn't change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 1, destination.Count );
		}
		#endregion

		#region DissimilarObject_YieldDifferences
		[Test]
		public void DissimilarObject_YieldDifferences() {

			MapD.Config.CreateMap<List<SimplePrimaryClass>, List<SimplePrimaryClass>>();

			List<SimplePrimaryClass> source = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 1,
					Integer = 1
				}
			};
			List<SimplePrimaryClass> destination = new List<SimplePrimaryClass>() {
				new SimplePrimaryClass() {
					Key = 1,
					Integer = 2
				}
			};
			SimplePrimaryClass sourceOne = source[0];
			SimplePrimaryClass destinationOne = destination[0];

			List<PropertyChangedResults> changes = MapD.Compare<List<SimplePrimaryClass>, List<SimplePrimaryClass>>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 1, changes.Count );

			// Assert we got the correct comparison results
			for ( int i = 0; i < changes.Count; i++ ) {
				PropertyChangedResults change = changes[i];
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
				int sourceVal = 1;
				int destVal = 2;

				PropertyChangedResult sourceChanges = change.Source;
				PropertyChangedResult destinationChanges = change.Destination;

				Assert.AreEqual( sourceOne, sourceChanges.Object );
				Assert.AreEqual( sourceOne.GetType(), sourceChanges.ObjectType );
				Assert.IsNotNull( sourceChanges.PropertyName );
				Assert.IsNotNull( sourceChanges.PropertyType );
				Assert.AreEqual( "Integer", sourceChanges.PropertyName );

				Assert.AreEqual( destinationOne, destinationChanges.Object );
				Assert.AreEqual( destinationOne.GetType(), destinationChanges.ObjectType );
				Assert.IsNotNull( destinationChanges.PropertyName );
				Assert.IsNotNull( destinationChanges.PropertyType );
				Assert.AreEqual( "Integer", destinationChanges.PropertyName );

				Assert.AreEqual( typeof( int ), sourceChanges.PropertyType );
				Assert.AreEqual( sourceVal, sourceChanges.PropertyValue );
				Assert.AreEqual( typeof( int ), destinationChanges.PropertyType );
				Assert.AreEqual( destVal, destinationChanges.PropertyValue );
			}

			// Insure destination didn't change
			Assert.IsNotNull( destination );
			Assert.AreEqual( 1, destination.Count );
		}
		#endregion

		private class SimplePrimaryClass {
			[PrimaryKey]
			public int Key { get; set; }
			public int Integer { get; set; }
		}
		private class SimplePrimaryClass2 {
			[PrimaryKey]
			public int Key { get; set; }
			public int Integer { get; set; }
		}

	}

}
