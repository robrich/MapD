namespace MapDLib.Tests {

	#region using
	using System.Data.Linq.Mapping;
	using System.Data.Objects.DataClasses;
	using NUnit.Framework;

	#endregion

	public class LinqToPropertyTests : BaseTest {

		[Test]
		public void LinqToSqlPropertyIncluded() {

			MapD.Config.ExcludeLinqProperties = false;
			MapD.Config.CreateMap<LinqToSqlPropertyClass, LinqToSqlPropertyClass>();

			LinqToSqlPropertyClass source = new LinqToSqlPropertyClass {
				RegularProperty = 5,
				LinqProperty = 10
			};

			LinqToSqlPropertyClass destination = MapD.Copy<LinqToSqlPropertyClass, LinqToSqlPropertyClass>( source );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.RegularProperty, destination.RegularProperty );
			Assert.AreEqual( source.LinqProperty, destination.LinqProperty );
			
		}

		[Test]
		public void LinqToSqlPropertyExcluded() {

			MapD.Config.ExcludeLinqProperties = true;
			MapD.Config.CreateMap<LinqToSqlPropertyClass, LinqToSqlPropertyClass>();

			LinqToSqlPropertyClass source = new LinqToSqlPropertyClass {
				RegularProperty = 5,
				LinqProperty = 10
			};

			LinqToSqlPropertyClass destination = MapD.Copy<LinqToSqlPropertyClass, LinqToSqlPropertyClass>( source );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.RegularProperty, destination.RegularProperty );
			Assert.AreEqual( 0, destination.LinqProperty );

		}

		private class LinqToSqlPropertyClass {
			public int RegularProperty { get; set; }
			[Association]
			public int LinqProperty { get; set; }
		}


		//[Test]
		[Ignore("This test fails as the properties on EntityCollection<> and EntitySet<> are not all Get/Set")]
		public void LinqToEntitiesPropertyIncluded() {

			MapD.Config.ExcludeLinqProperties = false;
			MapD.Config.CreateMap<LinqToEntitiesPropertyClass, LinqToEntitiesPropertyClass>();
			MapD.Config.CreateMap<EntityCollection<LinqToEntitiesRelatedClass>, EntityCollection<LinqToEntitiesRelatedClass>>();
			MapD.Config.CreateMap<EntityReference<LinqToEntitiesRelatedClass>, EntityReference<LinqToEntitiesRelatedClass>>();

			// TODO: Find a way to populate the LinqToEntities properties and verify they get copied

			LinqToEntitiesPropertyClass source = new LinqToEntitiesPropertyClass {
				RegularProperty = 5,
				LinqProperty1 = 10,
				LinqProperty2 = new EntityCollection<LinqToEntitiesRelatedClass>() /* {
					new LinqToEntitiesRelatedClass {
						Property1 = 1,
						Property2 = 2
					},
					new LinqToEntitiesRelatedClass {
						Property1 = 3,
						Property2 = 4
					}
				} */ ,
				LinqProperty3 = new EntityReference<LinqToEntitiesRelatedClass>() /* {
					EntityKey = new EntityKey("Setname", "KeyName", 1),
					Value = new LinqToEntitiesRelatedClass {
						Property1 = 1,
						Property2 = 2
					}
				} */
			};
			LinqToEntitiesPropertyClass destination = null;

			var results = MapD.Copy<LinqToEntitiesPropertyClass, LinqToEntitiesPropertyClass>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.RegularProperty, destination.RegularProperty );
			Assert.AreEqual( source.LinqProperty1, destination.LinqProperty1 );
			Assert.IsNotNull( destination.LinqProperty2 );
			Assert.IsNotNull( destination.LinqProperty3 );

			/*
			Assert.AreEqual( 2, destination.LinqProperty2.Count );
			LinqToEntitiesRelatedClass inner = destination.LinqProperty2.First();
			Assert.IsNotNull( inner );
			Assert.AreEqual( 1, inner.Property1 );
			Assert.AreEqual( 2, inner.Property2 );
			inner = destination.LinqProperty2.Last();
			Assert.IsNotNull( inner );
			Assert.AreEqual( 3, inner.Property1 );
			Assert.AreEqual( 4, inner.Property2 );
			*/

			/*
			Assert.IsNotNull( destination.LinqProperty3 );
			LinqToEntitiesRelatedClass inner = destination.LinqProperty3.Value;
			Assert.IsNotNull( inner );
			Assert.AreEqual( 1, inner.Property1 );
			Assert.AreEqual( 2, inner.Property2 );
			*/

		}

		[Test]
		public void LinqToEntitiesPropertyExcluded() {

			MapD.Config.ExcludeLinqProperties = true;
			MapD.Config.CreateMap<LinqToEntitiesPropertyClass, LinqToEntitiesPropertyClass>();

			LinqToEntitiesPropertyClass source = new LinqToEntitiesPropertyClass {
				RegularProperty = 5,
				LinqProperty1 = 10,
				LinqProperty2 = new EntityCollection<LinqToEntitiesRelatedClass>() /* {
					new LinqToEntitiesRelatedClass {
						Property1 = 1,
						Property2 = 2
					},
					new LinqToEntitiesRelatedClass {
						Property1 = 3,
						Property2 = 4
					}
				} */ ,
				LinqProperty3 = new EntityReference<LinqToEntitiesRelatedClass>() /* {
					EntityKey = new EntityKey( "Setname", "KeyName", 1 ),
					Value = new LinqToEntitiesRelatedClass {
						Property1 = 1,
						Property2 = 2
					}
				} */
			};

			LinqToEntitiesPropertyClass destination = MapD.Copy<LinqToEntitiesPropertyClass, LinqToEntitiesPropertyClass>( source );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.RegularProperty, destination.RegularProperty );
			Assert.AreEqual( 0, destination.LinqProperty1 );
			Assert.IsNull( destination.LinqProperty2 );
			Assert.IsNull( destination.LinqProperty3 );

		}

		private class LinqToEntitiesPropertyClass {
			public int RegularProperty { get; set; }
			[EdmRelationshipNavigationProperty( "namespace", "name", "role" )]
			public int LinqProperty1 { get; set; }
			public EntityCollection<LinqToEntitiesRelatedClass> LinqProperty2 { get; set; }
			public EntityReference<LinqToEntitiesRelatedClass> LinqProperty3 { get; set; }
		}

		private class LinqToEntitiesRelatedClass : IEntityWithRelationships {
			[IgnoreMap] // TODO: Find a way to make this not necessary
			public RelationshipManager RelationshipManager { get; set; }
			public int Property1 { get; set; }
			public double Property2 { get; set; }
		}

	}

}
