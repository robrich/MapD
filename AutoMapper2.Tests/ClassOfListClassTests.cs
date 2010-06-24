namespace AutoMapper2Lib.Tests {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class ClassOfListClassTests : BaseTest {

		#region ChangeList_Contains_Changes_SameClass_Test

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Test() {

			AutoMapper2.CreateMap<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>();
			AutoMapper2.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			Class_To_Same_Class_Type source = new ClassOfListClassTests.Class_To_Same_Class_Type {
				Content = new List<InnerClassType>() {
					new InnerClassType {
						Key = 1,
						Double = 2.0,
						Integer = 3,
						String = "Four",
					},
					new InnerClassType {
						Key = 2,
						Double = 3.0,
						Integer = 4,
						String = "Five",
					},
					new InnerClassType {
						Key = 3,
						Double = 4.0,
						Integer = 5,
						String = "Six",
					}
				}
			};
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type();

			List<PropertyChangedResults> changeList = AutoMapper2.Map<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>( source, ref destination );

			source.AssertEqual( destination );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Test_With_Null_Properties() {

			AutoMapper2.CreateMap<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>();
			AutoMapper2.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type();
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type() {
				Content = new List<InnerClassType>()
			};
			List<PropertyChangedResults> changeList = AutoMapper2.Map<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>( source, ref destination );

			source.AssertEqual( destination );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_PartiallyFilled_Test() {

			AutoMapper2.CreateMap<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>();
			AutoMapper2.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type {
				Content = new List<InnerClassType>() {
					new InnerClassType {
						Key = 1,
						Double = 2.0,
						Integer = 3,
						String = "Four",
					},
					new InnerClassType {
						Key = 2,
						Double = 3.0,
						Integer = 4,
						String = "Five",
					},
					new InnerClassType {
						Key = 3,
						Double = 4.0,
						Integer = 5,
						String = "Six",
					}
				}
			};
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type {
				Content = new List<InnerClassType>() {
					new InnerClassType {
						Key = 1,
						Double = 10.0,
						Integer = 10,
						String = "Ten",
					}
				}
			};

			List<PropertyChangedResults> changeList = AutoMapper2.Map<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>( source, ref destination );

			source.AssertEqual( destination );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Back_Test() {

			AutoMapper2.CreateMap<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>();
			AutoMapper2.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			Class_To_Same_Class_Type source = new ClassOfListClassTests.Class_To_Same_Class_Type {
				Content = new List<InnerClassType>() {
					new InnerClassType {
						Key = 1,
						Double = 2.0,
						Integer = 3,
						String = "Four",
					},
					new InnerClassType {
						Key = 2,
						Double = 3.0,
						Integer = 4,
						String = "Five",
					},
					new InnerClassType {
						Key = 3,
						Double = 4.0,
						Integer = 5,
						String = "Six",
					}
				}
			};
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type();

			List<PropertyChangedResults> changeList = AutoMapper2.MapBack<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>( source, ref destination );

			source.AssertEqual( destination );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Back_Test_With_Null_Properties() {

			AutoMapper2.CreateMap<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>();
			AutoMapper2.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type();
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type() {
				Content = new List<InnerClassType>()
			};
			List<PropertyChangedResults> changeList = AutoMapper2.MapBack<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.IsNotNull( destination.Content );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Back_Test_With_Null_Objects() {

			AutoMapper2.CreateMap<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>();
			AutoMapper2.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			Class_To_Same_Class_Type source = null;
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type() {
				Content = new List<InnerClassType>()
			};
			List<PropertyChangedResults> changeList = AutoMapper2.MapBack<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.IsNotNull( destination.Content );

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Back_PartiallyFilled_Test() {

			AutoMapper2.CreateMap<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>();
			AutoMapper2.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			Class_To_Same_Class_Type source = new Class_To_Same_Class_Type {
				Content = new List<InnerClassType>() {
					new InnerClassType {
						Key = 1,
						Double = 2.0,
						Integer = 3,
						String = "Four",
					},
					new InnerClassType {
						Key = 2,
						Double = 3.0,
						Integer = 4,
						String = "Five",
					},
					new InnerClassType {
						Key = 3,
						Double = 4.0,
						Integer = 5,
						String = "Six",
					}
				}
			};
			Class_To_Same_Class_Type destination = new Class_To_Same_Class_Type {
				Content = new List<InnerClassType>() {
					new InnerClassType {
						Key = 1,
						Double = 10.0,
						Integer = 10,
						String = "Ten",
					}
				}
			};

			List<PropertyChangedResults> changeList = AutoMapper2.MapBack<ClassOfListClassTests.Class_To_Same_Class_Type, ClassOfListClassTests.Class_To_Same_Class_Type>( source, ref destination );

			source.AssertEqual( destination );

		}

		public class Class_To_Same_Class_Type {
			public List<InnerClassType> Content { get; set; }

			public void AssertEqual( Class_To_Same_Class_Type Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Content == null ? 0 : this.Content.Count, Actual.Content == null ? 0 : Actual.Content.Count );
				if ( this.Content != null && this.Content.Count > 0 ) {
					foreach ( InnerClassType actualContent in Actual.Content ) {
						InnerClassType thisContent = (
							from t in this.Content
							where t.Key == actualContent.Key
							select t
							).FirstOrDefault();
						Assert.IsNotNull( thisContent );
						Assert.AreEqual( thisContent.Key, actualContent.Key );
						Assert.AreEqual( thisContent.Integer, actualContent.Integer );
						Assert.AreEqual( thisContent.Double, actualContent.Double );
						Assert.AreEqual( thisContent.String, actualContent.String );
					}
				}
			}

		}
		public class InnerClassType {
			[PrimaryKey]
			public int Key { get; set; }
			public int Integer { get; set; }
			public double Double { get; set; }
			public string String { get; set; }
		}

		#endregion

		#region Class_To_DifferentProperties_Class

		[Test]
		public void Class_To_DifferentProperties_Class() {

			AutoMapper2.CreateMap<Class_To_DifferentProperties_Class_Type1, Class_To_DifferentProperties_Class_Type2>();
			AutoMapper2.CreateMap<List<InnerClassType1>, List<InnerClassType2>>();

			Class_To_DifferentProperties_Class_Type1 source = new Class_To_DifferentProperties_Class_Type1 {
				Content = new List<InnerClassType1>() {
					new InnerClassType1 {
						Key = 1,
						Double = 2.0,
						Integer = 3,
						String = "4",
					},
					new InnerClassType1 {
						Key = 2,
						Double = 3.0,
						Integer = 4,
						String = "5",
					},
					new InnerClassType1 {
						Key = 3,
						Double = 4.0,
						Integer = 5,
						String = "6",
					}
				}
			};
			Class_To_DifferentProperties_Class_Type2 destinationTemplate = new Class_To_DifferentProperties_Class_Type2 {
				Content = new List<InnerClassType2>() {
					new InnerClassType2 {
						Key = 1,
						Double = "2",
						Integer = 3.0,
						String = 4,
					},
					new InnerClassType2 {
						Key = 2,
						Double = "3",
						Integer = 4.0,
						String = 5,
					},
					new InnerClassType2 {
						Key = 3,
						Double = "4",
						Integer = 5.0,
						String = 6,
					}
				}
			};

			Class_To_DifferentProperties_Class_Type2 destination = AutoMapper2.Map<Class_To_DifferentProperties_Class_Type1, Class_To_DifferentProperties_Class_Type2>( source );

			destinationTemplate.AssertEqual( destination );

		}

		[Test]
		public void Class_To_DifferentProperties_Class_With_Null_Properties() {

			AutoMapper2.CreateMap<Class_To_DifferentProperties_Class_Type1, Class_To_DifferentProperties_Class_Type2>();
			AutoMapper2.CreateMap<List<InnerClassType1>, List<InnerClassType2>>();

			Class_To_DifferentProperties_Class_Type1 source = new Class_To_DifferentProperties_Class_Type1();
			Class_To_DifferentProperties_Class_Type2 destinationTemplate = new Class_To_DifferentProperties_Class_Type2();

			Class_To_DifferentProperties_Class_Type2 destination = AutoMapper2.Map<Class_To_DifferentProperties_Class_Type1, Class_To_DifferentProperties_Class_Type2>( source );
			destinationTemplate.AssertEqual( destination );

		}

		public class Class_To_DifferentProperties_Class_Type1 {
			public List<InnerClassType1> Content { get; set; }

		}
		public class InnerClassType1 {
			[PrimaryKey]
			public int Key { get; set; }
			public int Integer { get; set; }
			public double Double { get; set; }
			public string String { get; set; }
		}
		public class Class_To_DifferentProperties_Class_Type2 {
			public List<InnerClassType2> Content { get; set; }

			public void AssertEqual( Class_To_DifferentProperties_Class_Type2 Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this == null, Actual == null );
				Assert.AreEqual( this.Content == null ? 0 : this.Content.Count, Actual.Content == null ? 0 : Actual.Content.Count );
				if ( this.Content != null && this.Content.Count > 0 ) {
					foreach ( InnerClassType2 actualContent in Actual.Content ) {
						InnerClassType2 thisContent = (
							from t in this.Content
							where t.Key == actualContent.Key
							select t
							).FirstOrDefault();
						Assert.IsNotNull( thisContent );
						Assert.AreEqual( thisContent.Key, actualContent.Key );
						Assert.AreEqual( thisContent.Integer, actualContent.Integer );
						Assert.AreEqual( thisContent.Double, actualContent.Double );
						Assert.AreEqual( thisContent.String, actualContent.String );
					}
				}
			}
		}
		public class InnerClassType2 {
			[PrimaryKey]
			public int Key { get; set; }
			public double Integer { get; set; }
			public string Double { get; set; }
			public int String { get; set; }
		}

		#endregion

	}

}
