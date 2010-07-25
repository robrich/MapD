namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class AssertConfigurationIsValidTests : BaseTest {

		#region MissingMap
		[Test]
		public void MissingMap_Fails() {

			// No call to AutoMapper2.CreateMap<MissingMapType, MissingMapType>() blows up nicely
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "Missing map should fail" );
			} catch ( ArgumentNullException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( "You haven't created any maps", ex.Message );
			}

		}

		public class MissingMapType {
			public int Property1 { get; set; }
		}
		#endregion

		#region MissingInnerMap
		[Test]
		public void MissingInnerMap_Fails() {

			AutoMapper2.CreateMap<MissingInnerMapType, MissingInnerMapType>();
			// No call to AutoMapper2.CreateMap<MissingInnerMapInnerType, MissingInnerMapInnerType>() blows up nicely
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "Missing property is a class map should fail" );
			} catch ( MissingMapException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( typeof( MissingInnerMapInnerType ), ex.FromType );
				Assert.AreEqual( typeof( MissingInnerMapInnerType ), ex.ToType );
			}

		}

		public class MissingInnerMapType {
			public MissingInnerMapInnerType Property1 { get; set; }
		}
		public class MissingInnerMapInnerType {
			public int Property1 { get; set; }
		}
		#endregion

		#region MissingListOfNonClassMap
		[Test]
		public void MissingListOfNonClassMap_Works() {

			AutoMapper2.CreateMap<MissingListOfNonClassMapType, MissingListOfNonClassMapType>();
			// No call to AutoMapper2.CreateMap<List<int>, List<int>>() works fine -- they're not classes
			AutoMapper2.AssertConfigurationIsValid();
			AutoMapper2.AssertMapCount( 1 );

		}

		public class MissingListOfNonClassMapType {
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
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "Missing map for List<class> should fail" );
			} catch ( MissingMapException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( typeof( MissingListOfClassMapListOfClassType ), ex.FromType );
				Assert.AreEqual( typeof( MissingListOfClassMapListOfClassType ), ex.ToType );
			}

		}

		[Test]
		public void MissingListOfClassMapWithInnerClassMap_Fails() {

			AutoMapper2.CreateMap<MissingListOfClassMapType, MissingListOfClassMapType>();
			// No call to AutoMapper2.CreateMap<List<MissingListOfClassMapListOfClassType>, List<MissingListOfClassMapListOfClassType>>() works because we know how to map from List<> to List<>
			AutoMapper2.CreateMap<MissingListOfClassMapListOfClassType, MissingListOfClassMapListOfClassType>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "Missing map for List<class> should fail" );
			} catch ( MissingMapException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( typeof( List<MissingListOfClassMapListOfClassType> ), ex.FromType );
				Assert.AreEqual( typeof( List<MissingListOfClassMapListOfClassType> ), ex.ToType );
			}

		}

		[Test]
		public void MissingListOfClassInnerClassMap_Works() {

			AutoMapper2.CreateMap<MissingListOfClassMapType, MissingListOfClassMapType>();
			AutoMapper2.CreateMap<List<MissingListOfClassMapListOfClassType>, List<MissingListOfClassMapListOfClassType>>();
			// No call to AutoMapper2.CreateMap<MissingListOfClassMapListOfClassType, MissingListOfClassMapListOfClassType>() works because it's auto-created
			AutoMapper2.AssertConfigurationIsValid();
			AutoMapper2.AssertMapCount( 3 );

		}

		public class MissingListOfClassMapType {
			public List<MissingListOfClassMapListOfClassType> Property1 { get; set; }
		}
		public class MissingListOfClassMapListOfClassType {
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
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "Missing map for List<class> should fail" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.MissingPrimaryKey, ex.InvalidPropertyReason );
				Assert.AreEqual( typeof( List<MissingPrimaryKeyListOfClassType> ), ex.From );
				Assert.AreEqual( typeof( List<MissingPrimaryKeyListOfClassType> ), ex.To );
			}

		}

		public class MissingPrimaryKeyType {
			public List<MissingPrimaryKeyListOfClassType> Property1 { get; set; }
		}
		public class MissingPrimaryKeyListOfClassType {
			public int Property1 { get; set; }
		}
		#endregion

		#region MissingProperty
		[Test]
		public void MissingProperty_Fails() {

			AutoMapper2.CreateMap<MissingPropertyType1, MissingPropertyType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "Missing property should fail to map" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property2", ex.PropertyInfo.Name );
			}

		}

		public class MissingPropertyType1 {
			public int Property1 { get; set; }
		}
		public class MissingPropertyType2 {
			public int Property1 { get; set; }
			public int Property2 { get; set; }
		}
		#endregion

		#region RedirectedProperty
		[Test]
		public void RedirectedProperty_Fails() {

			AutoMapper2.CreateMap<RedirectedPropertyType1, RedirectedPropertyType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "Missing redirected property should fail to map" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name ); // Property1 on this object has no match
			}

		}

		public class RedirectedPropertyType1 {
			public int Property1 { get; set; }
		}
		public class RedirectedPropertyType2 {
			[RemapProperty( "Property2" )]
			public int Property1 { get; set; }
		}
		#endregion

		#region ReadOnlySourceProperty
		[Test]
		public void ReadOnlySourceProperty_Fails() {

			AutoMapper2.CreateMap<ReadOnlySourcePropertyType1, ReadOnlySourcePropertyType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "read-only property should fail to map" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		public class ReadOnlySourcePropertyType1 {
			public int Property1 { get { return 0; } }
		}
		public class ReadOnlySourcePropertyType2 {
			public int Property1 { get; set; }
		}
		#endregion

		#region ReadOnlyDestinationProperty
		[Test]
		public void ReadOnlyDestinationProperty_Fails() {

			AutoMapper2.CreateMap<ReadOnlyDestinationPropertyType1, ReadOnlyDestinationPropertyType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "read-only property should fail to map" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		public class ReadOnlyDestinationPropertyType1 {
			public int Property1 { get; set; }
		}
		public class ReadOnlyDestinationPropertyType2 {
			public int Property1 { get { return 0; } }
		}
		#endregion

		#region WriteOnlySourceProperty
		[Test]
		public void WriteOnlySourceProperty_Fails() {

			AutoMapper2.CreateMap<WriteOnlySourcePropertyType1, WriteOnlySourcePropertyType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "write-only property should fail to map" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		public class WriteOnlySourcePropertyType1 {
			public int Property1 { set { } }
		}
		public class WriteOnlySourcePropertyType2 {
			public int Property1 { get; set; }
		}
		#endregion

		#region WriteOnlyDestinationProperty
		[Test]
		public void WriteOnlyDestinationProperty_Fails() {

			AutoMapper2.CreateMap<WriteOnlyDestinationPropertyType1, WriteOnlyDestinationPropertyType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "write-only property should fail to map" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		public class WriteOnlyDestinationPropertyType1 {
			public int Property1 { get; set; }
		}
		public class WriteOnlyDestinationPropertyType2 {
			public int Property1 { set { } }
		}
		#endregion

		#region ListToNonList
		[Test]
		public void ListToNonList_Fails() {

			AutoMapper2.CreateMap<ListToNonListType1, ListToNonListType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "List to NonList property should fail to map" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.ListTypeToNonListType, ex.InvalidPropertyReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		public class ListToNonListType1 {
			public List<int> Property1 { get; set; }
		}
		public class ListToNonListType2 {
			public int Property1 { get; set; }
		}
		#endregion

		#region NonListToList
		[Test]
		public void NonListToList_Fails() {

			AutoMapper2.CreateMap<NonListToListType1, NonListToListType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "NonList to List property should fail to map" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.NonListTypeToListType, ex.InvalidPropertyReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		public class NonListToListType1 {
			public int Property1 { get; set; }
		}
		public class NonListToListType2 {
			public List<int> Property1 { get; set; }
		}
		#endregion

		#region ClassToNonClass
		[Test]
		public void ClassToNonClass_Fails() {

			AutoMapper2.CreateMap<ClassToNonClassType1, ClassToNonClassType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "Class to NonClass property should fail to map" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.ClassTypeToNonClassType, ex.InvalidPropertyReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		public class ClassToNonClassType1 {
			public InnerClass Property1 { get; set; }
		}
		public class ClassToNonClassType2 {
			public int Property1 { get; set; }
		}
		#endregion

		#region NonClassToClass
		[Test]
		public void NonClassToClass_Fails() {

			AutoMapper2.CreateMap<NonClassToClassType1, NonClassToClassType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "NonClass to Class property should fail to map" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.NonClassTypeToClassType, ex.InvalidPropertyReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		public class NonClassToClassType1 {
			public int Property1 { get; set; }
		}
		public class NonClassToClassType2 {
			public InnerClass Property1 { get; set; }
		}
		#endregion

		#region IncompatibleProperty
		[Test]
		public void IncompatibleProperty_Fails() {

			AutoMapper2.CreateMap<IncompatiblePropertyType1, IncompatiblePropertyType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "Incompatible property should fail to map" );
			} catch ( MapFailureException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( MapFailureReason.ConvertTypeFailure, ex.MapFailureReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		public class IncompatiblePropertyType1 {
			public DateTime Property1 { get; set; }
		}
		public class IncompatiblePropertyType2 {
			public Guid Property1 { get; set; }
		}
		#endregion

		#region IncompatibleListProperty
		[Test]
		public void IncompatibleListProperty_Fails() {

			AutoMapper2.CreateMap<IncompatibleListPropertyType1, IncompatibleListPropertyType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "Incompatible property should fail to map" );
			} catch ( MapFailureException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( MapFailureReason.ConvertTypeFailure, ex.MapFailureReason );
			}

		}

		public class IncompatibleListPropertyType1 {
			public List<DateTime> Property1 { get; set; }
		}
		public class IncompatibleListPropertyType2 {
			public List<Guid> Property1 { get; set; }
		}
		#endregion

		#region IncompatibleListClassProperty
		[Test]
		public void IncompatibleListClassProperty_Fails() {

			AutoMapper2.CreateMap<IncompatibleListClassPropertyType1, IncompatibleListClassPropertyType2>();
			AutoMapper2.CreateMap<IncompatibleListClassInnerType1, IncompatibleListClassInnerType2>();
			try {
				AutoMapper2.AssertConfigurationIsValid();
				Assert.Fail( "Incompatible property should fail to map" );
			} catch ( MapFailureException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( MapFailureReason.ConvertTypeFailure, ex.MapFailureReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		public class IncompatibleListClassPropertyType1 {
			public IncompatibleListClassInnerType1 Property1 { get; set; }
		}
		public class IncompatibleListClassPropertyType2 {
			public IncompatibleListClassInnerType2 Property1 { get; set; }
		}
		public class IncompatibleListClassInnerType1 {
			public DateTime Property1 { get; set; }
		}
		public class IncompatibleListClassInnerType2 {
			public Guid Property1 { get; set; }
		}
		#endregion

		#region AssertMapCount_Works
		[Test]
		public void AssertMapCount_Works() {
			AutoMapper2.AssertMapCount( 0 );
			AutoMapper2.CreateMap<InnerClass, InnerClass>();

			try {
				AutoMapper2.AssertMapCount( 0 );
				Assert.Fail( "The map count should now be 1, not 0" );
			} catch ( ArgumentOutOfRangeException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
			}

			AutoMapper2.AssertMapCount( 1 );
		}
		#endregion

		public class InnerClass {
		}

	}

}