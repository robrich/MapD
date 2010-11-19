namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using AutoMapper2Lib.TestsResource;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class MapFromAttributeTests : BaseTest {

		// FRAGILE: As new tests are added that do this, we need to adjust these numbers
		private const int MappedClassesCount = 6;
		private const int MappedResourceClassesCount = 2;
		
		[Test]
		public void MapFromAttribute_Works() {

			AutoMapper2.CreateMaps();

			AutoMapper2.AssertMapCount( MappedClassesCount );

		}

		[Test]
		public void MapFromAttribute_AllAssemblies_Works() {

			// TODO: Why do I have to use each assembly before this works?
			MapFromAttributeResourceType t = new MapFromAttributeResourceType();

			AutoMapper2.CreateAllMaps();

			AutoMapper2.AssertMapCount( MappedClassesCount + MappedResourceClassesCount );

		}

		[Test]
		public void Resources_Can_Map_SameType_Via_Attribute() {

			MapFromAttributeResourceType source = new MapFromAttributeResourceType {
				Property1 = 1,
				Property2 = 2,
				Property3 = 3
			};

			// TODO: Why do I have to use each assembly before this works?
			AutoMapper2.CreateAllMaps();

			MapFromAttributeResourceType destination = AutoMapper2.Map<MapFromAttributeResourceType, MapFromAttributeResourceType>( source );
			source.AssertEqual( destination );

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
		public void Can_Map_FromSelf_Via_Attribute() {

			AutoMapper2.CreateMaps();

			MapFromSelfAttributeType source = new MapFromSelfAttributeType {
				Property1 = 1,
				Property2 = 2,
				Property3 = 3
			};

			MapFromSelfAttributeType destination = AutoMapper2.Map<MapFromSelfAttributeType, MapFromSelfAttributeType>( source );
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

		[Test]
		public void Can_Map_List_Self_Via_Attribute() {

			AutoMapper2.CreateMaps();

			List<MapFromSelfAttributeType> source = new List<MapFromSelfAttributeType> {
				new MapFromSelfAttributeType {
					Property1 = 1,
					Property2 = 2,
					Property3 = 3
				},
				new MapFromSelfAttributeType {
					Property1 = 2,
					Property2 = 3,
					Property3 = 4
				}
			};

			List<MapFromSelfAttributeType> destination = AutoMapper2.Map<List<MapFromSelfAttributeType>, List<MapFromSelfAttributeType>>( source );
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

		[MapFromSelf]
		[MapListFromListOfSelf]
		public class MapFromSelfAttributeType {
			[PrimaryKey]
			public int Property1 { get; set; }
			public int Property2 { get; set; }
			public int Property3 { get; set; }

			public void AssertEqual( MapFromSelfAttributeType Actual ) {
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
