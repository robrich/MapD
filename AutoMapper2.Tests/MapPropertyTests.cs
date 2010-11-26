namespace AutoMapper2Lib.Tests {

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

			// No call to AutoMapper2.CreateMap<MissingMapType, MissingMapType>() blows up nicely
			try {
				MissingMapType dest = AutoMapper2.Map<MissingMapType, MissingMapType>( new MissingMapType() );
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

			AutoMapper2.CreateMap<InnerClass, InnerClass>();
			AutoMapper2.CreateMap<InnerClass, InnerClass>();
			AutoMapper2.AssertMapCount( 1 );
			
		}
		#endregion

		#region MissingInnerMap
		[Test]
		public void MissingInnerMap_Fails() {

			AutoMapper2.CreateMap<MissingInnerMapType, MissingInnerMapType>();
			// No call to AutoMapper2.CreateMap<MissingInnerMapInnerType, MissingInnerMapInnerType>() blows up nicely
			try {
				MissingInnerMapType dest = AutoMapper2.Map<MissingInnerMapType, MissingInnerMapType>( new MissingInnerMapType() );
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

			AutoMapper2.CreateMap<MissingListOfNonClassMapType, MissingListOfNonClassMapType>();
			// No call to AutoMapper2.CreateMap<List<int>, List<int>>() works fine -- they're not classes
			MissingListOfNonClassMapType dest = AutoMapper2.Map<MissingListOfNonClassMapType, MissingListOfNonClassMapType>(
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

			AutoMapper2.CreateMap<MissingListOfClassMapType, MissingListOfClassMapType>();
			// No call to AutoMapper2.CreateMap<List<MissingListOfClassMapListOfClassType>, List<MissingListOfClassMapListOfClassType>>()
			// or to AutoMapper2.CreateMap<MissingListOfClassMapListOfClassType, MissingListOfClassMapListOfClassType>() blows up nicely
			try {
				MissingListOfClassMapType dest = AutoMapper2.Map<MissingListOfClassMapType, MissingListOfClassMapType>(
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

			AutoMapper2.CreateMap<MissingListOfClassMapType, MissingListOfClassMapType>();
			// No call to AutoMapper2.CreateMap<List<MissingListOfClassMapListOfClassType>, List<MissingListOfClassMapListOfClassType>>() works because we know how to map from List<> to List<>
			AutoMapper2.CreateMap<MissingListOfClassMapListOfClassType, MissingListOfClassMapListOfClassType>();
			try {
				MissingListOfClassMapType dest = AutoMapper2.Map<MissingListOfClassMapType, MissingListOfClassMapType>(
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

			AutoMapper2.CreateMap<MissingListOfClassMapType, MissingListOfClassMapType>();
			AutoMapper2.CreateMap<List<MissingListOfClassMapListOfClassType>, List<MissingListOfClassMapListOfClassType>>();
			// No call to AutoMapper2.CreateMap<MissingListOfClassMapListOfClassType, MissingListOfClassMapListOfClassType>() works because it's auto-created
			MissingListOfClassMapType dest = AutoMapper2.Map<MissingListOfClassMapType, MissingListOfClassMapType>(
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

			AutoMapper2.CreateMap<MissingPrimaryKeyType, MissingPrimaryKeyType>();
			AutoMapper2.CreateMap<List<MissingPrimaryKeyListOfClassType>, List<MissingPrimaryKeyListOfClassType>>();
			// No call to AutoMapper2.CreateMap<MissingPrimaryKeyListOfClassType, MissingPrimaryKeyListOfClassType>() blows up nicely
			try {
				MissingPrimaryKeyType dest = AutoMapper2.Map<MissingPrimaryKeyType, MissingPrimaryKeyType>( new MissingPrimaryKeyType {
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

			AutoMapper2.CreateMap<MissingPropertyType1, MissingPropertyType2>();
			try {
				MissingPropertyType2 dest = AutoMapper2.Map<MissingPropertyType1, MissingPropertyType2>( new MissingPropertyType1() );
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

			AutoMapper2.CreateMap<RedirectedPropertyType1, RedirectedPropertyType2>();
			try {
				RedirectedPropertyType2 dest = AutoMapper2.Map<RedirectedPropertyType1, RedirectedPropertyType2>( new RedirectedPropertyType1() );
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

			AutoMapper2.CreateMap<ReadOnlySourcePropertyType1, ReadOnlySourcePropertyType2>();
			try {
				ReadOnlySourcePropertyType2 dest = AutoMapper2.Map<ReadOnlySourcePropertyType1, ReadOnlySourcePropertyType2>( new ReadOnlySourcePropertyType1() );
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

			AutoMapper2.CreateMap<ReadOnlyDestinationPropertyType1, ReadOnlyDestinationPropertyType2>();
			try {
				ReadOnlyDestinationPropertyType2 dest = AutoMapper2.Map<ReadOnlyDestinationPropertyType1, ReadOnlyDestinationPropertyType2>( new ReadOnlyDestinationPropertyType1() );
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

			AutoMapper2.CreateMap<WriteOnlySourcePropertyType1, WriteOnlySourcePropertyType2>();
			try {
				WriteOnlySourcePropertyType2 dest = AutoMapper2.Map<WriteOnlySourcePropertyType1, WriteOnlySourcePropertyType2>( new WriteOnlySourcePropertyType1() );
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

			AutoMapper2.CreateMap<WriteOnlyDestinationPropertyType1, WriteOnlyDestinationPropertyType2>();
			try {
				WriteOnlyDestinationPropertyType2 dest = AutoMapper2.Map<WriteOnlyDestinationPropertyType1, WriteOnlyDestinationPropertyType2>( new WriteOnlyDestinationPropertyType1() );
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

			AutoMapper2.CreateMap<ListToNonListType1, ListToNonListType2>();
			try {
				ListToNonListType2 dest = AutoMapper2.Map<ListToNonListType1, ListToNonListType2>( new ListToNonListType1() );
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

			AutoMapper2.CreateMap<NonListToListType1, NonListToListType2>();
			try {
				NonListToListType2 dest = AutoMapper2.Map<NonListToListType1, NonListToListType2>( new NonListToListType1() );
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

			AutoMapper2.CreateMap<ClassToNonClassType1, ClassToNonClassType2>();
			try {
				ClassToNonClassType2 dest = AutoMapper2.Map<ClassToNonClassType1, ClassToNonClassType2>( new ClassToNonClassType1() );
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

			AutoMapper2.CreateMap<NonClassToClassType1, NonClassToClassType2>();
			try {
				NonClassToClassType2 dest = AutoMapper2.Map<NonClassToClassType1, NonClassToClassType2>( new NonClassToClassType1() );
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

			AutoMapper2.CreateMap<IncompatiblePropertyType1, IncompatiblePropertyType2>();
			IncompatiblePropertyType2 dest = null;
			IncompatiblePropertyType1 source = new IncompatiblePropertyType1();
			try {
				List<PropertyChangedResults> changes = AutoMapper2.Map<IncompatiblePropertyType1, IncompatiblePropertyType2>( source, ref dest );
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

			AutoMapper2.CreateMap<IncompatibleListPropertyType1, IncompatibleListPropertyType2>();
			try {
				IncompatibleListPropertyType2 dest = AutoMapper2.Map<IncompatibleListPropertyType1, IncompatibleListPropertyType2>( new IncompatibleListPropertyType1() {
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

			AutoMapper2.CreateMap<IncompatibleListClassPropertyType1, IncompatibleListClassPropertyType2>();
			AutoMapper2.CreateMap<IncompatibleListClassInnerType1, IncompatibleListClassInnerType2>();
			try {
				IncompatibleListClassPropertyType2 dest = AutoMapper2.Map<IncompatibleListClassPropertyType1, IncompatibleListClassPropertyType2>( new IncompatibleListClassPropertyType1 {
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

			AutoMapper2.CreateMap<DuplicatePrimaryKeyType, DuplicatePrimaryKeyType>();
			AutoMapper2.CreateMap<List<DuplicatePrimaryKeyInnerType>, List<DuplicatePrimaryKeyInnerType>>();
			try {
				DuplicatePrimaryKeyType dest = AutoMapper2.Map<DuplicatePrimaryKeyType, DuplicatePrimaryKeyType>( new DuplicatePrimaryKeyType {
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

			AutoMapper2.CreateMap<DuplicatePrimaryKeyType, DuplicatePrimaryKeyType>();
			AutoMapper2.CreateMap<List<DuplicatePrimaryKeyInnerType>, List<DuplicatePrimaryKeyInnerType>>();
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
				var changes = AutoMapper2.Map<DuplicatePrimaryKeyType, DuplicatePrimaryKeyType>( source, ref dest );
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

			AutoMapper2.CreateMap<List<DuplicatePrimaryKeyInnerType>, List<DuplicatePrimaryKeyInnerType>>();
			try {
				List<DuplicatePrimaryKeyInnerType> dest = AutoMapper2.Map<List<DuplicatePrimaryKeyInnerType>, List<DuplicatePrimaryKeyInnerType>>(
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

			AutoMapper2.CreateMap<List<DuplicatePrimaryKeyInnerType>, List<DuplicatePrimaryKeyInnerType>>();
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
				var changes = AutoMapper2.Map<List<DuplicatePrimaryKeyInnerType>, List<DuplicatePrimaryKeyInnerType>>( source, ref dest );
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
