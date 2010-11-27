namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class AssertConfigurationIsValidTests : BaseTest {

		#region MissingMap
		[Test]
		public void MissingMap_Fails() {

			// No call to AutoMapper2.Config.CreateMap<MissingMapType, MissingMapType>() blows up nicely
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
				Assert.Fail( "Missing map should fail" );
			} catch ( ArgumentNullException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( "You haven't created any maps", ex.Message );
			}

		}

		private class MissingMapType {
			public int Property1 { get; set; }
		}
		#endregion

		#region MissingInnerMap
		[Test]
		public void MissingInnerMap_Fails() {

			AutoMapper2.Config.CreateMap<MissingInnerMapType, MissingInnerMapType>();
			// No call to AutoMapper2.Config.CreateMap<MissingInnerMapInnerType, MissingInnerMapInnerType>() blows up nicely
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<MissingListOfNonClassMapType, MissingListOfNonClassMapType>();
			// No call to AutoMapper2.Config.CreateMap<List<int>, List<int>>() works fine -- they're not classes
			AutoMapper2.Assert.AssertConfigurationIsValid();
			Assert.AreEqual( 1, AutoMapper2.Assert.MapCount );

		}

		private class MissingListOfNonClassMapType {
			public List<int> Property1 { get; set; }
		}
		#endregion

		#region MissingListOfClassMap
		[Test]
		public void MissingListOfClassMap_Fails() {

			AutoMapper2.Config.CreateMap<MissingListOfClassMapType, MissingListOfClassMapType>();
			// No call to AutoMapper2.Config.CreateMap<List<MissingListOfClassMapListOfClassType>, List<MissingListOfClassMapListOfClassType>>()
			// or to AutoMapper2.Config.CreateMap<MissingListOfClassMapListOfClassType, MissingListOfClassMapListOfClassType>() blows up nicely
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<MissingListOfClassMapType, MissingListOfClassMapType>();
			// No call to AutoMapper2.Config.CreateMap<List<MissingListOfClassMapListOfClassType>, List<MissingListOfClassMapListOfClassType>>() works because we know how to map from List<> to List<>
			AutoMapper2.Config.CreateMap<MissingListOfClassMapListOfClassType, MissingListOfClassMapListOfClassType>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<MissingListOfClassMapType, MissingListOfClassMapType>();
			AutoMapper2.Config.CreateMap<List<MissingListOfClassMapListOfClassType>, List<MissingListOfClassMapListOfClassType>>();
			// No call to AutoMapper2.Config.CreateMap<MissingListOfClassMapListOfClassType, MissingListOfClassMapListOfClassType>() works because it's auto-created
			AutoMapper2.Assert.AssertConfigurationIsValid();
			Assert.AreEqual( 3, AutoMapper2.Assert.MapCount );

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

			AutoMapper2.Config.CreateMap<MissingPrimaryKeyType, MissingPrimaryKeyType>();
			AutoMapper2.Config.CreateMap<List<MissingPrimaryKeyListOfClassType>, List<MissingPrimaryKeyListOfClassType>>();
			// No call to AutoMapper2.Config.CreateMap<MissingPrimaryKeyListOfClassType, MissingPrimaryKeyListOfClassType>() blows up nicely
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<MissingPropertyType1, MissingPropertyType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<RedirectedPropertyType1, RedirectedPropertyType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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
			[RemapProperty( "Property2" )]
			public int Property1 { get; set; }
		}
		#endregion

		#region ReadOnlySourceProperty
		[Test]
		public void ReadOnlySourceProperty_Fails() {

			AutoMapper2.Config.CreateMap<ReadOnlySourcePropertyType1, ReadOnlySourcePropertyType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<ReadOnlyDestinationPropertyType1, ReadOnlyDestinationPropertyType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<WriteOnlySourcePropertyType1, WriteOnlySourcePropertyType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<WriteOnlyDestinationPropertyType1, WriteOnlyDestinationPropertyType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<ListToNonListType1, ListToNonListType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<NonListToListType1, NonListToListType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<ClassToNonClassType1, ClassToNonClassType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<NonClassToClassType1, NonClassToClassType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<IncompatiblePropertyType1, IncompatiblePropertyType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
				Assert.Fail( "Incompatible property should fail to map" );
			} catch ( MapFailureException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( MapFailureReason.ConvertTypeFailure, ex.MapFailureReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
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

			AutoMapper2.Config.CreateMap<IncompatibleListPropertyType1, IncompatibleListPropertyType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

			AutoMapper2.Config.CreateMap<IncompatibleListClassPropertyType1, IncompatibleListClassPropertyType2>();
			AutoMapper2.Config.CreateMap<IncompatibleListClassInnerType1, IncompatibleListClassInnerType2>();
			try {
				AutoMapper2.Assert.AssertConfigurationIsValid();
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

		#region AssertMapCount_Works
		[Test]
		public void AssertMapCount_Works() {
			Assert.AreEqual( 0, AutoMapper2.Assert.MapCount );
			AutoMapper2.Config.CreateMap<InnerClass, InnerClass>();
			Assert.AreEqual( 1, AutoMapper2.Assert.MapCount );
		}
		#endregion

		private class InnerClass {
		}

	}

}
