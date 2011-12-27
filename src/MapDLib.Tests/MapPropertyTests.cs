namespace MapDLib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class MapPropertyTests : BaseTest {

		// FRAGILE: Matches AssertConfigurationIsValid tests

		#region MissingMap
		[Test]
		public void MissingMap_Fails() {

			// No call to MapD.Config.CreateMap<MissingMapType, MissingMapType>() blows up nicely
			try {
				MissingMapType dest = MapD.Copy<MissingMapType, MissingMapType>( new MissingMapType() );
				Assert.Fail( "Missing map should fail" );
			} catch ( MissingMapException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( typeof( MissingMapType ), ex.FromType );
				Assert.AreEqual( typeof( MissingMapType ), ex.ToType );
			}

		}

		private class MissingMapType {
			public int Property1 { get; set; }
		}
		#endregion

		#region DuplicateMap_Works
		[Test]
		public void DuplicateMap_Works() {

			MapD.Config.CreateMap<InnerClass, InnerClass>();
			MapD.Config.CreateMap<InnerClass, InnerClass>();
			Assert.AreEqual( 1, MapD.Assert.MapCount );
			
		}
		#endregion

		#region MissingInnerMap
		[Test]
		public void MissingInnerMap_Fails() {

			MapD.Config.CreateMap<MissingInnerMapType, MissingInnerMapType>();
			// No call to MapD.Config.CreateMap<MissingInnerMapInnerType, MissingInnerMapInnerType>() blows up nicely
			try {
				MissingInnerMapType dest = MapD.Copy<MissingInnerMapType, MissingInnerMapType>( new MissingInnerMapType() );
				Assert.Fail( "Missing property is a class map should fail" );
			} catch ( MissingMapException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( typeof( MissingInnerMapInnerType ), ex.FromType );
				Assert.AreEqual( typeof( MissingInnerMapInnerType ), ex.ToType );
			}

		}

		private class MissingInnerMapType {
			public MissingInnerMapInnerType Property1 { get; set; }
		}
		private class MissingInnerMapInnerType {
			public int Property1 { get; set; }
		}
		#endregion

		#region MissingListOfNonClassMap
		[Test]
		public void MissingListOfNonClassMap_Works() {

			MapD.Config.CreateMap<MissingListOfNonClassMapType, MissingListOfNonClassMapType>();
			// No call to MapD.Config.CreateMap<List<int>, List<int>>() works fine -- they're not classes
			MissingListOfNonClassMapType dest = MapD.Copy<MissingListOfNonClassMapType, MissingListOfNonClassMapType>(
				new MissingListOfNonClassMapType {
					Property1 = new List<int> {
						default( int )
					}
				} );

		}

		private class MissingListOfNonClassMapType {
			public List<int> Property1 { get; set; }
		}
		#endregion

		#region MissingListOfClassMap
		[Test]
		public void MissingListOfClassMap_Fails() {

			MapD.Config.CreateMap<MissingListOfClassMapType, MissingListOfClassMapType>();
			// No call to MapD.Config.CreateMap<List<MissingListOfClassMapListOfClassType>, List<MissingListOfClassMapListOfClassType>>()
			// or to MapD.Config.CreateMap<MissingListOfClassMapListOfClassType, MissingListOfClassMapListOfClassType>() blows up nicely
			try {
				MissingListOfClassMapType dest = MapD.Copy<MissingListOfClassMapType, MissingListOfClassMapType>(
					new MissingListOfClassMapType {
						Property1 = new List<MissingListOfClassMapListOfClassType> {
							new MissingListOfClassMapListOfClassType()
						}
					} );
				Assert.Fail( "Missing map for List<class> should fail" );
			} catch ( MissingMapException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( typeof(MissingListOfClassMapListOfClassType), ex.FromType );
				Assert.AreEqual( typeof(MissingListOfClassMapListOfClassType), ex.ToType );
			}

		}

		[Test]
		public void MissingListOfClassMapWithInnerClassMap_Fails() {

			MapD.Config.CreateMap<MissingListOfClassMapType, MissingListOfClassMapType>();
			// No call to MapD.Config.CreateMap<List<MissingListOfClassMapListOfClassType>, List<MissingListOfClassMapListOfClassType>>() works because we know how to map from List<> to List<>
			MapD.Config.CreateMap<MissingListOfClassMapListOfClassType, MissingListOfClassMapListOfClassType>();
			try {
				MissingListOfClassMapType dest = MapD.Copy<MissingListOfClassMapType, MissingListOfClassMapType>(
					new MissingListOfClassMapType {
						Property1 = new List<MissingListOfClassMapListOfClassType> {
							new MissingListOfClassMapListOfClassType()
						}
					} );
				Assert.Fail( "Missing map for List<class> should fail" );
			} catch ( MissingMapException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( typeof(List<MissingListOfClassMapListOfClassType>), ex.FromType );
				Assert.AreEqual( typeof(List<MissingListOfClassMapListOfClassType>), ex.ToType );
			}

		}

		[Test]
		public void MissingListOfClassInnerClassMap_Works() {

			MapD.Config.CreateMap<MissingListOfClassMapType, MissingListOfClassMapType>();
			MapD.Config.CreateMap<List<MissingListOfClassMapListOfClassType>, List<MissingListOfClassMapListOfClassType>>();
			// No call to MapD.Config.CreateMap<MissingListOfClassMapListOfClassType, MissingListOfClassMapListOfClassType>() works because it's auto-created
			MissingListOfClassMapType dest = MapD.Copy<MissingListOfClassMapType, MissingListOfClassMapType>(
				new MissingListOfClassMapType {
					Property1 = new List<MissingListOfClassMapListOfClassType> {
						new MissingListOfClassMapListOfClassType()
					}
				} );

		}

		private class MissingListOfClassMapType {
			public List<MissingListOfClassMapListOfClassType> Property1 { get; set; }
		}
		private class MissingListOfClassMapListOfClassType {
			[PrimaryKey]
			public int Property1 { get; set; }
		}
		#endregion

		#region MissingPrimaryKey
		[Test]
		public void MissingPrimaryKey_Fails() {

			MapD.Config.CreateMap<MissingPrimaryKeyType, MissingPrimaryKeyType>();
			MapD.Config.CreateMap<List<MissingPrimaryKeyListOfClassType>, List<MissingPrimaryKeyListOfClassType>>();
			// No call to MapD.Config.CreateMap<MissingPrimaryKeyListOfClassType, MissingPrimaryKeyListOfClassType>() blows up nicely
			try {
				MissingPrimaryKeyType dest = MapD.Copy<MissingPrimaryKeyType, MissingPrimaryKeyType>( new MissingPrimaryKeyType {
					Property1 = new List<MissingPrimaryKeyListOfClassType> {
						new MissingPrimaryKeyListOfClassType()
					}
				} );
				Assert.Fail( "Missing map for List<class> should fail" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.MissingPrimaryKey, ex.InvalidPropertyReason );
				Assert.AreEqual( typeof( List<MissingPrimaryKeyListOfClassType> ), ex.From );
				Assert.AreEqual( typeof( List<MissingPrimaryKeyListOfClassType> ), ex.To );
			}

		}

		private class MissingPrimaryKeyType {
			public List<MissingPrimaryKeyListOfClassType> Property1 { get; set; }
		}
		private class MissingPrimaryKeyListOfClassType {
			public int Property1 { get; set; }
		}
		#endregion

		#region MissingProperty
		[Test]
		public void MissingProperty_Fails() {

			MapD.Config.CreateMap<MissingPropertyType1, MissingPropertyType2>();
			try {
				MissingPropertyType2 dest = MapD.Copy<MissingPropertyType1, MissingPropertyType2>( new MissingPropertyType1() );
				Assert.Fail( "Missing property should fail to map" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property2", ex.PropertyInfo.Name );
			}

		}

		private class MissingPropertyType1 {
			public int Property1 { get; set; }
		}
		private class MissingPropertyType2 {
			public int Property1 { get; set; }
			public int Property2 { get; set; }
		}
		#endregion

		#region RedirectedProperty
		[Test]
		public void RedirectedProperty_Fails() {

			MapD.Config.CreateMap<RedirectedPropertyType1, RedirectedPropertyType2>();
			try {
				RedirectedPropertyType2 dest = MapD.Copy<RedirectedPropertyType1, RedirectedPropertyType2>( new RedirectedPropertyType1() );
				Assert.Fail( "Missing redirected property should fail to map" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name ); // Property1 on this object has no match
			}

		}

		private class RedirectedPropertyType1 {
			public int Property1 { get; set; }
		}
		private class RedirectedPropertyType2 {
			[RemapProperty("Property2")]
			public int Property1 { get; set; }
		}
		#endregion

		#region ReadOnlySourceProperty
		[Test]
		public void ReadOnlySourceProperty_Fails() {

			MapD.Config.CreateMap<ReadOnlySourcePropertyType1, ReadOnlySourcePropertyType2>();
			try {
				ReadOnlySourcePropertyType2 dest = MapD.Copy<ReadOnlySourcePropertyType1, ReadOnlySourcePropertyType2>( new ReadOnlySourcePropertyType1() );
				Assert.Fail( "read-only property should fail to map" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		private class ReadOnlySourcePropertyType1 {
			public int Property1 { get { return 0; } }
		}
		private class ReadOnlySourcePropertyType2 {
			public int Property1 { get; set; }
		}
		#endregion

		#region ReadOnlyDestinationProperty
		[Test]
		public void ReadOnlyDestinationProperty_Fails() {

			MapD.Config.CreateMap<ReadOnlyDestinationPropertyType1, ReadOnlyDestinationPropertyType2>();
			try {
				ReadOnlyDestinationPropertyType2 dest = MapD.Copy<ReadOnlyDestinationPropertyType1, ReadOnlyDestinationPropertyType2>( new ReadOnlyDestinationPropertyType1() );
				Assert.Fail( "read-only property should fail to map" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		private class ReadOnlyDestinationPropertyType1 {
			public int Property1 { get; set; }
		}
		private class ReadOnlyDestinationPropertyType2 {
			public int Property1 { get { return 0; } }
		}
		#endregion

		#region WriteOnlySourceProperty
		[Test]
		public void WriteOnlySourceProperty_Fails() {

			MapD.Config.CreateMap<WriteOnlySourcePropertyType1, WriteOnlySourcePropertyType2>();
			try {
				WriteOnlySourcePropertyType2 dest = MapD.Copy<WriteOnlySourcePropertyType1, WriteOnlySourcePropertyType2>( new WriteOnlySourcePropertyType1() );
				Assert.Fail( "write-only property should fail to map" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		private class WriteOnlySourcePropertyType1 {
			public int Property1 { set { } }
		}
		private class WriteOnlySourcePropertyType2 {
			public int Property1 { get; set; }
		}
		#endregion

		#region WriteOnlyDestinationProperty
		[Test]
		public void WriteOnlyDestinationProperty_Fails() {

			MapD.Config.CreateMap<WriteOnlyDestinationPropertyType1, WriteOnlyDestinationPropertyType2>();
			try {
				WriteOnlyDestinationPropertyType2 dest = MapD.Copy<WriteOnlyDestinationPropertyType1, WriteOnlyDestinationPropertyType2>( new WriteOnlyDestinationPropertyType1() );
				Assert.Fail( "write-only property should fail to map" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		private class WriteOnlyDestinationPropertyType1 {
			public int Property1 { get; set; }
		}
		private class WriteOnlyDestinationPropertyType2 {
			public int Property1 { set { } }
		}
		#endregion

		#region ListToNonList
		[Test]
		public void ListToNonList_Fails() {

			MapD.Config.CreateMap<ListToNonListType1, ListToNonListType2>();
			try {
				ListToNonListType2 dest = MapD.Copy<ListToNonListType1, ListToNonListType2>( new ListToNonListType1() );
				Assert.Fail( "List to NonList property should fail to map" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.ListTypeToNonListType, ex.InvalidPropertyReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		private class ListToNonListType1 {
			public List<int> Property1 { get; set; }
		}
		private class ListToNonListType2 {
			public int Property1 { get; set; }
		}
		#endregion

		#region NonListToList
		[Test]
		public void NonListToList_Fails() {

			MapD.Config.CreateMap<NonListToListType1, NonListToListType2>();
			try {
				NonListToListType2 dest = MapD.Copy<NonListToListType1, NonListToListType2>( new NonListToListType1() );
				Assert.Fail( "NonList to List property should fail to map" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.NonListTypeToListType, ex.InvalidPropertyReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		private class NonListToListType1 {
			public int Property1 { get; set; }
		}
		private class NonListToListType2 {
			public List<int> Property1 { get; set; }
		}
		#endregion

		#region ClassToNonClass
		[Test]
		public void ClassToNonClass_Fails() {

			MapD.Config.CreateMap<ClassToNonClassType1, ClassToNonClassType2>();
			try {
				ClassToNonClassType2 dest = MapD.Copy<ClassToNonClassType1, ClassToNonClassType2>( new ClassToNonClassType1() );
				Assert.Fail( "Class to NonClass property should fail to map" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.ClassTypeToNonClassType, ex.InvalidPropertyReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		private class ClassToNonClassType1 {
			public InnerClass Property1 { get; set; }
		}
		private class ClassToNonClassType2 {
			public int Property1 { get; set; }
		}
		#endregion

		#region NonClassToClass
		[Test]
		public void NonClassToClass_Fails() {

			MapD.Config.CreateMap<NonClassToClassType1, NonClassToClassType2>();
			try {
				NonClassToClassType2 dest = MapD.Copy<NonClassToClassType1, NonClassToClassType2>( new NonClassToClassType1() );
				Assert.Fail( "NonClass to Class property should fail to map" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.NonClassTypeToClassType, ex.InvalidPropertyReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		private class NonClassToClassType1 {
			public int Property1 { get; set; }
		}
		private class NonClassToClassType2 {
			public InnerClass Property1 { get; set; }
		}
		#endregion

		#region IncompatibleProperty
		[Test]
		public void IncompatibleProperty_Fails() {

			MapD.Config.CreateMap<IncompatiblePropertyType1, IncompatiblePropertyType2>();
			IncompatiblePropertyType2 dest = null;
			IncompatiblePropertyType1 source = new IncompatiblePropertyType1();
			try {
				List<PropertyChangedResults> changes = MapD.Copy<IncompatiblePropertyType1, IncompatiblePropertyType2>( source, ref dest );
				Assert.Fail( "Incompatible property should fail to map" );
			} catch ( MapFailureException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( MapFailureReason.ConvertTypeFailure, ex.MapFailureReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
				Assert.IsNotNull( ex.Target );
				Assert.AreEqual( dest, ex.Target );
				Assert.AreEqual( source.Property1, ex.Value );
			}

		}

		private class IncompatiblePropertyType1 {
			public DateTime Property1 { get; set; }
		}
		private class IncompatiblePropertyType2 {
			public Guid Property1 { get; set; }
		}
		#endregion

		#region IncompatibleListProperty
		[Test]
		public void IncompatibleListProperty_Fails() {

			MapD.Config.CreateMap<IncompatibleListPropertyType1, IncompatibleListPropertyType2>();
			try {
				IncompatibleListPropertyType2 dest = MapD.Copy<IncompatibleListPropertyType1, IncompatibleListPropertyType2>( new IncompatibleListPropertyType1() {
					Property1 = new List<DateTime>() {
						default(DateTime)
					}
				} );
				Assert.Fail( "Incompatible property should fail to map" );
			} catch ( MapFailureException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( MapFailureReason.ConvertTypeFailure, ex.MapFailureReason );
			}

		}

		private class IncompatibleListPropertyType1 {
			public List<DateTime> Property1 { get; set; }
		}
		private class IncompatibleListPropertyType2 {
			public List<Guid> Property1 { get; set; }
		}
		#endregion

		#region IncompatibleListClassProperty
		[Test]
		public void IncompatibleListClassProperty_Fails() {

			MapD.Config.CreateMap<IncompatibleListClassPropertyType1, IncompatibleListClassPropertyType2>();
			MapD.Config.CreateMap<IncompatibleListClassInnerType1, IncompatibleListClassInnerType2>();
			try {
				IncompatibleListClassPropertyType2 dest = MapD.Copy<IncompatibleListClassPropertyType1, IncompatibleListClassPropertyType2>( new IncompatibleListClassPropertyType1 {
					Property1 = new IncompatibleListClassInnerType1()
				} );
				Assert.Fail( "Incompatible property should fail to map" );
			} catch ( MapFailureException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( MapFailureReason.ConvertTypeFailure, ex.MapFailureReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		private class IncompatibleListClassPropertyType1 {
			public IncompatibleListClassInnerType1 Property1 { get; set; }
		}
		private class IncompatibleListClassPropertyType2 {
			public IncompatibleListClassInnerType2 Property1 { get; set; }
		}
		private class IncompatibleListClassInnerType1 {
			public DateTime Property1 { get; set; }
		}
		private class IncompatibleListClassInnerType2 {
			public Guid Property1 { get; set; }
		}
		#endregion

		#region DupliatePrimaryKey

		[Test]
		public void Duplicate_Source_PrimaryKey_Class_Fails() {

			MapD.Config.CreateMap<DuplicatePrimaryKeyType, DuplicatePrimaryKeyType>();
			MapD.Config.CreateMap<List<DuplicatePrimaryKeyInnerType>, List<DuplicatePrimaryKeyInnerType>>();
			try {
				DuplicatePrimaryKeyType dest = MapD.Copy<DuplicatePrimaryKeyType, DuplicatePrimaryKeyType>( new DuplicatePrimaryKeyType {
					Property1 = new List<DuplicatePrimaryKeyInnerType>() {
						new DuplicatePrimaryKeyInnerType {
							Key = 1
						},
						new DuplicatePrimaryKeyInnerType {
							Key = 1
						}
					}
				} );
				Assert.Fail( "Duplicate primary key should fail to map" );
			} catch ( MapFailureException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( MapFailureReason.DuplicateFromPrimaryKey, ex.MapFailureReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Key", ex.PropertyInfo.Name );
			}

		}

		[Test]
		public void Duplicate_Destination_PrimaryKey_Class_Fails() {

			MapD.Config.CreateMap<DuplicatePrimaryKeyType, DuplicatePrimaryKeyType>();
			MapD.Config.CreateMap<List<DuplicatePrimaryKeyInnerType>, List<DuplicatePrimaryKeyInnerType>>();
			try {
				DuplicatePrimaryKeyType source = new DuplicatePrimaryKeyType {
					Property1 = new List<DuplicatePrimaryKeyInnerType>() {
						new DuplicatePrimaryKeyInnerType {
							Key = 1
						}
					}
				};
				DuplicatePrimaryKeyType dest = new DuplicatePrimaryKeyType {
					Property1 = new List<DuplicatePrimaryKeyInnerType>() {
						new DuplicatePrimaryKeyInnerType {
							Key = 1
						},
						new DuplicatePrimaryKeyInnerType {
							Key = 1
						}
					}
				};
				var changes = MapD.Copy<DuplicatePrimaryKeyType, DuplicatePrimaryKeyType>( source, ref dest );
				Assert.Fail( "Duplicate primary key should fail to map" );
			} catch ( MapFailureException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( MapFailureReason.DuplicateToPrimaryKey, ex.MapFailureReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Key", ex.PropertyInfo.Name );
			}

		}

		[Test]
		public void Duplicate_Source_PrimaryKey_List_Fails() {

			MapD.Config.CreateMap<List<DuplicatePrimaryKeyInnerType>, List<DuplicatePrimaryKeyInnerType>>();
			try {
				List<DuplicatePrimaryKeyInnerType> dest = MapD.Copy<List<DuplicatePrimaryKeyInnerType>, List<DuplicatePrimaryKeyInnerType>>(
					new List<DuplicatePrimaryKeyInnerType> {
						new DuplicatePrimaryKeyInnerType {
							Key = 1
						},
						new DuplicatePrimaryKeyInnerType {
							Key = 1
						}
					} );
				Assert.Fail( "Duplicate primary key should fail to map" );
			} catch ( MapFailureException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( MapFailureReason.DuplicateFromPrimaryKey, ex.MapFailureReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Key", ex.PropertyInfo.Name );
			}

		}

		[Test]
		public void Duplicate_Destination_PrimaryKey_List_Fails() {

			MapD.Config.CreateMap<List<DuplicatePrimaryKeyInnerType>, List<DuplicatePrimaryKeyInnerType>>();
			try {
				List<DuplicatePrimaryKeyInnerType> source = new List<DuplicatePrimaryKeyInnerType>() {
					new DuplicatePrimaryKeyInnerType {
						Key = 1
					}
				};
				List<DuplicatePrimaryKeyInnerType> dest = new List<DuplicatePrimaryKeyInnerType>() {
					new DuplicatePrimaryKeyInnerType {
						Key = 1
					},
					new DuplicatePrimaryKeyInnerType {
						Key = 1
					}
				};
				var changes = MapD.Copy<List<DuplicatePrimaryKeyInnerType>, List<DuplicatePrimaryKeyInnerType>>( source, ref dest );
				Assert.Fail( "Duplicate primary key should fail to map" );
			} catch ( MapFailureException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( MapFailureReason.DuplicateToPrimaryKey, ex.MapFailureReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Key", ex.PropertyInfo.Name );
			}

		}

		private class DuplicatePrimaryKeyType {
			public List<DuplicatePrimaryKeyInnerType> Property1 { get; set; }
		}
		private class DuplicatePrimaryKeyInnerType {
			[PrimaryKey]
			public int Key { get; set; }
		}
		#endregion

		private class InnerClass {
		}

	}

}
