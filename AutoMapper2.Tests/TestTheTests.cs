namespace MapDLib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using NUnit.Framework;

	#endregion

	public class TestTheTests : BaseTest {

		[Test]
		public void AllClassesDeriveFromBaseTest() {

			List<Type> types = this.GetAllTestClasses();
			foreach ( Type type in types ) {
	
				string typeName = type.FullName;

				if ( type.IsNested ) {
					continue; // TODO: Make them private instead
				}

				object[] attributes = type.GetCustomAttributes( true );
				Assert.IsNotNull( attributes, typeName + " has no attributes, so it isn't a TestFixture"  );
				Assert.Greater( attributes.Length, 0, typeName + " has no attributes, so it isn't a TestFixture" );

				bool testFixtureAttribute = false;
				foreach ( object attribute in attributes ) {
					if ( attribute is TestFixtureAttribute ) {
						testFixtureAttribute = true;
						break;
					}
				}
				Assert.IsTrue( testFixtureAttribute, typeName + " doesn't have a TestFixture attribute" );

			}

		}

		[Test]
		public void AllMethodsAreTests() {

			List<string> objectMethods = typeof(object).GetMethods( BindingFlags.Public | BindingFlags.Instance ).Select( m => m.Name ).ToList();

			List<Type> types = this.GetAllTestClasses();
			foreach ( Type type in types ) {

				string typeName = type.FullName;

				object[] attributes = type.GetCustomAttributes( true );
				if ( attributes == null || attributes.Length == 0 ) {
					continue;
				}

				bool testFixtureAttribute = false;
				foreach ( object attribute in attributes ) {
					if ( attribute is TestFixtureAttribute ) {
						testFixtureAttribute = true;
					}
				}
				if ( !testFixtureAttribute ) {
					continue;
				}

				MethodInfo[] methods = type.GetMethods( BindingFlags.Public | BindingFlags.Instance );

				Assert.IsNotNull( methods, typeName + " has no methods, so it has no tests" );
				Assert.Greater( methods.Length, 0, typeName + " has no methods, so it has no tests" );

				foreach ( MethodInfo method in methods ) {

					string methodName = typeName + "." + method.Name + "()";

					if ( objectMethods.Contains( method.Name ) ) {
						continue; // object methods are exempt
					}

					attributes = method.GetCustomAttributes( true );
					Assert.IsNotNull( attributes, methodName + " has no attributes, so it isn't a Test" );
					Assert.Greater( attributes.Length, 0, methodName + " has no attributes, so it isn't a Test" );

					bool testAttribute = false;
					foreach ( object attribute in attributes ) {
						if ( attribute is TestAttribute
							|| attribute is TestCaseAttribute
							|| attribute is SetUpAttribute
							|| attribute is TearDownAttribute
							|| attribute is SetUpFixtureAttribute
							|| attribute is TestFixtureTearDownAttribute
							|| attribute is IgnoreAttribute
							|| attribute is TestFixtureSetUpAttribute ) {
							testAttribute = true;
							break;
						}
					}
					Assert.IsTrue( testAttribute, methodName + " doesn't have a Test attribute" );
				
				}

			}

		}

		private List<Type> GetAllTestClasses() {

			List<Type> results = Assembly.GetExecutingAssembly().GetExportedTypes().Where( type => type.IsClass && !type.IsNotPublic && !type.IsAbstract ).ToList();

			Assert.Greater( results.Count, 0 );
			return results;
		}

	}

}
