namespace MapDLib.Tests {

	#region using
	using System.Collections.Generic;
	using System.Reflection;
	using MapDLib.TestsResource;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class MapFromAttributeTests : BaseTest {

		// FRAGILE: As new tests are added that do this, we need to adjust these numbers
		private const int MappedClassesCount = 6;
		private const int MappedResourceClassesCount = 2;
		
		[Test]
		public void MapFromAttribute_Works() {

			MapD.Config.CreateMapsFromCallingAssembly();

			Assert.AreEqual( MappedClassesCount, MapD.Assert.MapCount );

		}

		[Test]
		public void MapFromAttribute_AllAssemblies_Works() {

			// TODO: Why do I have to use each assembly before this works?
			MapFromAttributeResourceType t = new MapFromAttributeResourceType();

			MapD.Config.CreateMapsFromAllLoadedAssemblies();
			
			Assert.AreEqual( MappedClassesCount + MappedResourceClassesCount, MapD.Assert.MapCount );

		}

		[Test]
		public void MapFromAttribute_AssemblyPath_Works() {

			MapD.Config.CreateMapsFromAllAssembliesInPath( null, "^MapD" ); // meaning "the current directory"

			Assert.AreEqual( MappedClassesCount + MappedResourceClassesCount, MapD.Assert.MapCount );

			// Now use something and insure it also works as expected
			MapFromAttributeResourceType t = new MapFromAttributeResourceType();
			Assert.IsNotNull( t );

		}

		[Test]
		public void Resources_Can_Map_SameType_Via_Attribute() {

			MapFromAttributeResourceType source = new MapFromAttributeResourceType {
				Property1 = 1,
				Property2 = 2,
				Property3 = 3
			};

			// TODO: Why do I have to use each assembly before this works?
			MapD.Config.CreateMapsFromAllLoadedAssemblies();

			MapFromAttributeResourceType destination = MapD.Copy<MapFromAttributeResourceType, MapFromAttributeResourceType>( source );
			source.AssertEqual( destination );

		}

		[Test]
		public void Can_Map_SameType_Via_Attribute() {

			MapD.Config.CreateMapsFromCallingAssembly();

			MapFromAttributeType source = new MapFromAttributeType {
				Property1 = 1,
				Property2 = 2,
				Property3 = 3
			};

			MapFromAttributeType destination = MapD.Copy<MapFromAttributeType, MapFromAttributeType>( source );
			source.AssertEqual( destination );
			
		}

		[Test]
		public void Can_Map_FromSelf_Via_Attribute() {

			MapD.Config.CreateMapsFromCallingAssembly();

			MapFromSelfAttributeType source = new MapFromSelfAttributeType {
				Property1 = 1,
				Property2 = 2,
				Property3 = 3
			};

			MapFromSelfAttributeType destination = MapD.Copy<MapFromSelfAttributeType, MapFromSelfAttributeType>( source );
			source.AssertEqual( destination );

		}

		[Test]
		public void Can_Map_SameType_Via_Passed_Assembly() {

			MapD.Config.CreateMapsFromAssembly( Assembly.GetExecutingAssembly() );

			MapFromAttributeType source = new MapFromAttributeType {
				Property1 = 1,
				Property2 = 2,
				Property3 = 3
			};

			MapFromAttributeType destination = MapD.Copy<MapFromAttributeType, MapFromAttributeType>( source );
			source.AssertEqual( destination );

		}

		[Test]
		public void Can_Map_List_SameType_Via_Attribute() {

			MapD.Config.CreateMapsFromCallingAssembly();

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

			List<MapFromAttributeType> destination = MapD.Copy<List<MapFromAttributeType>, List<MapFromAttributeType>>( source );
			Assert.IsNotNull( destination );
			Assert.AreEqual( 2, destination.Count );
			source[0].AssertEqual( destination[0] );
			source[1].AssertEqual( destination[1] );

		}

		[Test]
		public void Can_Map_List_Self_Via_Attribute() {

			MapD.Config.CreateMapsFromCallingAssembly();

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

			List<MapFromSelfAttributeType> destination = MapD.Copy<List<MapFromSelfAttributeType>, List<MapFromSelfAttributeType>>( source );
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

			MapD.Config.CreateMapsFromCallingAssembly();

			MapFromAttributeType2 source = new MapFromAttributeType2 {
				Property1 = 1,
				Property2 = 2,
				Property3 = 3
			};

			MapFromAttributeType3 destination = MapD.Copy<MapFromAttributeType2, MapFromAttributeType3>( source );
			source.AssertEqual( destination );

		}

		[Test]
		public void Can_Map_List_DifferentTypes_Via_Attribute() {

			MapD.Config.CreateMapsFromCallingAssembly();

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

			List<MapFromAttributeType3> destination = MapD.Copy<List<MapFromAttributeType2>, List<MapFromAttributeType3>>( source );
			Assert.IsNotNull( destination );
			Assert.AreEqual( 2, destination.Count );
			source[0].AssertEqual( destination[0] );
			source[1].AssertEqual( destination[1] );

		}

		private class MapFromAttributeType2 {
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
