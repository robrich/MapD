namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class MapFromAttributeTests : BaseTest {

		[Test]
		public void MapFromAttribute_Works() {

			AutoMapper2.CreateMaps();

			AutoMapper2.AssertMapCount( 4 ); // FRAGILE: As new tests are added that do this, we need to adjust this number
			
		}

		[Test]
		public void Can_Map_SameType_Via_Attribute() {

			AutoMapper2.CreateMaps();

			MapFromAttributeType source = new MapFromAttributeType {
				Property1 = 1,
				Property2 = 2,
				Property3 = 3
			};

			MapFromAttributeType destination = AutoMapper2.Map<MapFromAttributeType, MapFromAttributeType>( source );
			source.AssertEqual( destination );
			
		}

		[Test]
		public void Can_Map_SameType_Via_Passed_Assembly() {

			AutoMapper2.CreateMaps( Assembly.GetExecutingAssembly() );

			MapFromAttributeType source = new MapFromAttributeType {
				Property1 = 1,
				Property2 = 2,
				Property3 = 3
			};

			MapFromAttributeType destination = AutoMapper2.Map<MapFromAttributeType, MapFromAttributeType>( source );
			source.AssertEqual( destination );

		}

		[Test]
		public void Can_Map_List_SameType_Via_Attribute() {

			AutoMapper2.CreateMaps();

			List<MapFromAttributeType> source = new List<MapFromAttributeType> {
				new MapFromAttributeType {
					Property1 = 1,
					Property2 = 2,
					Property3 = 3
				},
				new MapFromAttributeType {
					Property1 = 2,
					Property2 = 3,
					Property3 = 4
				}
			};

			List<MapFromAttributeType> destination = AutoMapper2.Map<List<MapFromAttributeType>, List<MapFromAttributeType>>( source );
			Assert.IsNotNull( destination );
			Assert.AreEqual( 2, destination.Count );
			source[0].AssertEqual( destination[0] );
			source[1].AssertEqual( destination[1] );

		}

		[MapFrom( typeof( MapFromAttributeType ) )]
		[MapListFromListOf( typeof( MapFromAttributeType ) )]
		public class MapFromAttributeType {
			[PrimaryKey]
			public int Property1 { get; set; }
			public int Property2 { get; set; }
			public int Property3 { get; set; }

			public void AssertEqual(MapFromAttributeType Actual) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Property1, Actual.Property1 );
				Assert.AreEqual( this.Property2, Actual.Property2 );
				Assert.AreEqual( this.Property3, Actual.Property3 );
			}
		}

		[Test]
		public void Can_Map_DifferentTypes_Via_Attribute() {

			AutoMapper2.CreateMaps();

			MapFromAttributeType2 source = new MapFromAttributeType2 {
				Property1 = 1,
				Property2 = 2,
				Property3 = 3
			};

			MapFromAttributeType3 destination = AutoMapper2.Map<MapFromAttributeType2, MapFromAttributeType3>( source );
			source.AssertEqual( destination );

		}

		[Test]
		public void Can_Map_List_DifferentTypes_Via_Attribute() {

			AutoMapper2.CreateMaps();

			List<MapFromAttributeType2> source = new List<MapFromAttributeType2> {
				new MapFromAttributeType2 {
					Property1 = 1,
					Property2 = 2,
					Property3 = 3
				},
				new MapFromAttributeType2 {
					Property1 = 2,
					Property2 = 3,
					Property3 = 4
				}
			};

			List<MapFromAttributeType3> destination = AutoMapper2.Map<List<MapFromAttributeType2>, List<MapFromAttributeType3>>( source );
			Assert.IsNotNull( destination );
			Assert.AreEqual( 2, destination.Count );
			source[0].AssertEqual( destination[0] );
			source[1].AssertEqual( destination[1] );

		}

		public class MapFromAttributeType2 {
			[PrimaryKey]
			public int Property1 { get; set; }
			public int Property2 { get; set; }
			public int Property3 { get; set; }
			public int Property4 { get; set; }

			public void AssertEqual( MapFromAttributeType3 Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Property1, Actual.Property1 );
				Assert.AreEqual( this.Property2.ToString(), Actual.Property2 );
				Assert.AreEqual( (double)this.Property3, Actual.Property3 );
			}
		}
		[MapFrom( typeof( MapFromAttributeType2 ) )]
		[MapListFromListOf( typeof( MapFromAttributeType2 ) )]
		public class MapFromAttributeType3 {
			public int Property1 { get; set; }
			public string Property2 { get; set; }
			public double Property3 { get; set; }

		}

	}

}
