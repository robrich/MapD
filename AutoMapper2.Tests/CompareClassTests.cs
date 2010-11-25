namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using AutoMapper2Lib;
	using NUnit.Framework;

	#endregion

	public class CompareClassTests : BaseTest {

		#region NoChanges_NoDifferences

		[Test]
		public void NoChanges_NoDifferences() {

			AutoMapper2.CreateMap<Class_With_Properties_Type, Class_With_Properties_Type>();

			Class_With_Properties_Type source = new Class_With_Properties_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};
			Class_With_Properties_Type destination = new Class_With_Properties_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = new Guid( source.Guid.ToString() ) // Avoid shared object reference
			};

			List<PropertyChangedResults> changes = AutoMapper2.Compare<Class_With_Properties_Type, Class_With_Properties_Type>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 0, changes.Count );

			// The classes were identical, so no way to verify it didn't change

		}

		[Test]
		public void DestEmpty_YieldDifferences() {

			AutoMapper2.CreateMap<Class_With_Properties_Type, Class_With_Properties_Type>();

			Class_With_Properties_Type source = new Class_With_Properties_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};
			Class_With_Properties_Type destination = new Class_With_Properties_Type();
			Class_With_Properties_Type destinationReference = new Class_With_Properties_Type();

			List<PropertyChangedResults> changes = AutoMapper2.Compare<Class_With_Properties_Type, Class_With_Properties_Type>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 5, changes.Count ); // One for each property

			// Assert we got the correct comparison results
			foreach ( PropertyChangedResults change in changes ) {
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
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

				switch ( sourceChanges.PropertyName ) {
					case "Integer":
						Assert.AreEqual( typeof(int), sourceChanges.PropertyType );
						Assert.AreEqual( source.Integer.ToString(), sourceChanges.Value );
						Assert.AreEqual( typeof(int), destinationChanges.PropertyType );
						Assert.AreEqual( destination.Integer.ToString(), destinationChanges.Value );
						break;
					case "Char":
						Assert.AreEqual( typeof(char), sourceChanges.PropertyType );
						Assert.AreEqual( source.Char.ToString(), sourceChanges.Value );
						Assert.AreEqual( typeof(char), destinationChanges.PropertyType );
						Assert.AreEqual( null, destinationChanges.Value );
						break;
					case "Double":
						Assert.AreEqual( typeof(double), sourceChanges.PropertyType );
						Assert.AreEqual( source.Double.ToString(), sourceChanges.Value );
						Assert.AreEqual( typeof(double), destinationChanges.PropertyType );
						Assert.AreEqual( destination.Double.ToString(), destinationChanges.Value );
						break;
					case "String":
						Assert.AreEqual( typeof(string), sourceChanges.PropertyType );
						Assert.AreEqual( source.String, sourceChanges.Value );
						Assert.AreEqual( typeof(string), destinationChanges.PropertyType );
						Assert.AreEqual( destination.String, destinationChanges.Value );
						break;
					case "Guid":
						Assert.AreEqual( typeof(Guid), sourceChanges.PropertyType );
						Assert.AreEqual( source.Guid.ToString(), sourceChanges.Value );
						Assert.AreEqual( typeof(Guid), destinationChanges.PropertyType );
						Assert.AreEqual( Guid.Empty.ToString(), destinationChanges.Value );
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

			}

			// Assert the object didn't change
			destinationReference.AssertEqual( destination );

		}

		[Test]
		public void SourceEmpty_YieldDifferences() {

			AutoMapper2.CreateMap<Class_With_Properties_Type, Class_With_Properties_Type>();

			Class_With_Properties_Type source = new Class_With_Properties_Type();
			Class_With_Properties_Type destination = new Class_With_Properties_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};
			Class_With_Properties_Type destinationReference = new Class_With_Properties_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = new Guid( destination.Guid.ToString() ) // Avoid shared object reference
			};

			List<PropertyChangedResults> changes = AutoMapper2.Compare<Class_With_Properties_Type, Class_With_Properties_Type>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 5, changes.Count ); // One for each property

			// Assert we got the correct comparison results
			foreach ( PropertyChangedResults change in changes ) {
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
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

				switch ( sourceChanges.PropertyName ) {
					case "Integer":
						Assert.AreEqual( typeof( int ), sourceChanges.PropertyType );
						Assert.AreEqual( source.Integer.ToString(), sourceChanges.Value );
						Assert.AreEqual( typeof( int ), destinationChanges.PropertyType );
						Assert.AreEqual( destination.Integer.ToString(), destinationChanges.Value );
						break;
					case "Char":
						Assert.AreEqual( typeof( char ), sourceChanges.PropertyType );
						Assert.AreEqual( null, sourceChanges.Value );
						Assert.AreEqual( typeof( char ), destinationChanges.PropertyType );
						Assert.AreEqual( destination.Char.ToString(), destinationChanges.Value );
						break;
					case "Double":
						Assert.AreEqual( typeof( double ), sourceChanges.PropertyType );
						Assert.AreEqual( source.Double.ToString(), sourceChanges.Value );
						Assert.AreEqual( typeof( double ), destinationChanges.PropertyType );
						Assert.AreEqual( destination.Double.ToString(), destinationChanges.Value );
						break;
					case "String":
						Assert.AreEqual( typeof( string ), sourceChanges.PropertyType );
						Assert.AreEqual( source.String, sourceChanges.Value );
						Assert.AreEqual( typeof( string ), destinationChanges.PropertyType );
						Assert.AreEqual( destination.String, destinationChanges.Value );
						break;
					case "Guid":
						Assert.AreEqual( typeof( Guid ), sourceChanges.PropertyType );
						Assert.AreEqual( source.Guid.ToString(), sourceChanges.Value );
						Assert.AreEqual( typeof( Guid ), destinationChanges.PropertyType );
						Assert.AreEqual( destination.Guid.ToString(), destinationChanges.Value );
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

			}

			// Assert the object didn't change
			destinationReference.AssertEqual( destination );

		}

		[Test]
		public void NullSource_YieldDifferences() {

			Class_With_Properties_Type source = null;
			Class_With_Properties_Type destination = new Class_With_Properties_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};
			Class_With_Properties_Type destinationReference = new Class_With_Properties_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = new Guid( destination.Guid.ToString() ) // Avoid shared object reference
			};

			List<PropertyChangedResults> changes = AutoMapper2.Compare<Class_With_Properties_Type, Class_With_Properties_Type>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 1, changes.Count ); // The object

			// Assert we got the correct comparison results
			Assert.IsNotNull( changes[0].Source );
			Assert.IsNotNull( changes[0].Destination );
			PropertyChangedResult sourceChanges = changes[0].Source;
			PropertyChangedResult destinationChanges = changes[0].Destination;

			Assert.AreEqual( source, sourceChanges.Object );
			Assert.AreEqual( typeof( Class_With_Properties_Type ), sourceChanges.ObjectType );
			Assert.AreEqual( "this", sourceChanges.PropertyName );
			Assert.AreEqual( typeof( Class_With_Properties_Type ), sourceChanges.PropertyType );
			Assert.AreEqual( source.ObjectToString(), sourceChanges.Value );

			Assert.AreEqual( destination, destinationChanges.Object );
			Assert.AreEqual( typeof( Class_With_Properties_Type ), destinationChanges.ObjectType );
			Assert.AreEqual( "this", destinationChanges.PropertyName );
			Assert.AreEqual( typeof( Class_With_Properties_Type ), destinationChanges.PropertyType );
			Assert.AreEqual( destination.ObjectToString(), destinationChanges.Value );

			// Assert the object didn't change
			destinationReference.AssertEqual( destination );
		}

		[Test]
		public void NullDestination_YieldDifferences() {

			Class_With_Properties_Type source = new Class_With_Properties_Type {
				Integer = 1234,
				Char = 'c',
				Double = 1234.234,
				String = "String",
				Guid = Guid.NewGuid()
			};
			Class_With_Properties_Type destination = null;

			List<PropertyChangedResults> changes = AutoMapper2.Compare<Class_With_Properties_Type, Class_With_Properties_Type>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 1, changes.Count ); // The object

			// Assert we got the correct comparison results
			Assert.IsNotNull( changes[0].Source );
			Assert.IsNotNull( changes[0].Destination );
			PropertyChangedResult sourceChanges = changes[0].Source;
			PropertyChangedResult destinationChanges = changes[0].Destination;
			
			Assert.AreEqual( source, sourceChanges.Object );
			Assert.AreEqual( typeof( Class_With_Properties_Type ), sourceChanges.ObjectType );
			Assert.AreEqual( "this", sourceChanges.PropertyName );
			Assert.AreEqual( typeof( Class_With_Properties_Type ), sourceChanges.PropertyType );
			Assert.AreEqual( source.ObjectToString(), sourceChanges.Value );

			Assert.AreEqual( destination, destinationChanges.Object );
			Assert.AreEqual( typeof( Class_With_Properties_Type ), destinationChanges.ObjectType );
			Assert.AreEqual( "this", destinationChanges.PropertyName );
			Assert.AreEqual( typeof( Class_With_Properties_Type ), destinationChanges.PropertyType );
			Assert.AreEqual( destination.ObjectToString(), destinationChanges.Value );

			// Assert the object didn't change
			Assert.AreEqual( null, destination );
		}

		public class Class_With_Properties_Type {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public Guid Guid { get; set; }
			public char Char { get; set; }

			public void AssertEqual( Class_With_Properties_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, Actual.Integer );
				Assert.AreEqual( this.Char, Actual.Char );
				Assert.AreEqual( this.Double, Actual.Double );
				Assert.AreEqual( this.String, Actual.String );
				Assert.AreEqual( this.Guid, Actual.Guid );
			}
		}

		#endregion

		#region Dissimilar_Class

		[Test]
		public void Dissimilar_Class_Changed() {

			AutoMapper2.CreateMap<Dissimilar_Type1, Dissimilar_Type2>();

			Dissimilar_Type1 source = new Dissimilar_Type1 {
				Integer = 4321,
				Char = 'S',
				Double = 1423,
				String = "g"
			};
			Dissimilar_Type2 destination = new Dissimilar_Type2();
			Dissimilar_Type2 destinationReference = new Dissimilar_Type2();

			List<PropertyChangedResults> changes = AutoMapper2.Compare<Dissimilar_Type1, Dissimilar_Type2>( source, destination );

			// Assert the object didn't change
			destinationReference.AssertEqual( destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 4, changes.Count ); // One for each property

			// Assert we got the correct comparison results
			foreach ( PropertyChangedResults change in changes ) {
				Assert.IsNotNull( change.Source );
				Assert.IsNotNull( change.Destination );
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

				switch ( sourceChanges.PropertyName ) {
					case "Integer":
						Assert.AreEqual( typeof( int ), sourceChanges.PropertyType );
						Assert.AreEqual( source.Integer.ToString(), sourceChanges.Value );
						Assert.AreEqual( typeof( double ), destinationChanges.PropertyType );
						Assert.AreEqual( destination.Integer.ToString(), destinationChanges.Value );
						break;
					case "Char":
						Assert.AreEqual( typeof( char ), sourceChanges.PropertyType );
						Assert.AreEqual( source.Char.ToString(), sourceChanges.Value );
						Assert.AreEqual( typeof( string ), destinationChanges.PropertyType );
						Assert.AreEqual( destination.Char, destinationChanges.Value );
						break;
					case "Double":
						Assert.AreEqual( typeof( double ), sourceChanges.PropertyType );
						Assert.AreEqual( source.Double.ToString(), sourceChanges.Value );
						Assert.AreEqual( typeof( int ), destinationChanges.PropertyType );
						Assert.AreEqual( destination.Double.ToString(), destinationChanges.Value );
						break;
					case "String":
						Assert.AreEqual( typeof( string ), sourceChanges.PropertyType );
						Assert.AreEqual( source.String, sourceChanges.Value );
						Assert.AreEqual( typeof( char ), destinationChanges.PropertyType );
						Assert.AreEqual( null, destinationChanges.Value );
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

			}

		}

		[Test]
		public void Dissimilar_Class_Unchanged() {

			AutoMapper2.CreateMap<Dissimilar_Type1, Dissimilar_Type2>();

			Dissimilar_Type1 source = new Dissimilar_Type1 {
				String = "a"
			};
			Dissimilar_Type2 destination = new Dissimilar_Type2 {
				String = 'a'
			};
			Dissimilar_Type2 destinationReference = new Dissimilar_Type2 {
				String = 'a'
			};

			List<PropertyChangedResults> changes = AutoMapper2.Compare<Dissimilar_Type1, Dissimilar_Type2>( source, destination );

			Assert.IsNotNull( changes );
			Assert.AreEqual( 0, changes.Count );

			// Verify it didn't change
			destinationReference.AssertEqual( destination );

		}

		public class Dissimilar_Type1 {
			public int Integer { get; set; }
			public string String { get; set; }
			public double Double { get; set; }
			public char Char { get; set; }

		}
		public class Dissimilar_Type2 {
			public double Integer { get; set; }
			public char String { get; set; }
			public int Double { get; set; }
			public string Char { get; set; }

			public void AssertEqual( Dissimilar_Type2 Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Integer, Actual.Integer );
				Assert.AreEqual( this.Char, Actual.Char );
				Assert.AreEqual( this.Double, Actual.Double );
				Assert.AreEqual( this.String, Actual.String );
			}
		}

		#endregion

	}

}
