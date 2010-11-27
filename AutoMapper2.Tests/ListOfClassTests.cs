namespace AutoMapper2Lib.Tests {

	#region using
	using System.Collections.Generic;
	using System.Linq;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class ListOfClassTests : BaseTest {

		#region ChangeList_Contains_Changes_SameClass_Test

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Test() {

			AutoMapper2.Config.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			List<InnerClassType> source = new List<InnerClassType>() {
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
			};
			List<InnerClassType> destination = new List<InnerClassType>();

			List<PropertyChangedResults> changeList = AutoMapper2.Map<List<InnerClassType>, List<InnerClassType>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				int index = 0;
				foreach ( InnerClassType actualContent in source ) {
					InnerClassType thisContent = (
						from t in destination
						where t.Key == actualContent.Key
						select t
						).FirstOrDefault();
					Assert.IsNotNull( thisContent );
					Assert.AreEqual( thisContent.Key, actualContent.Key );
					Assert.AreEqual( thisContent.Integer, actualContent.Integer );
					Assert.AreEqual( thisContent.Double, actualContent.Double );
					Assert.AreEqual( thisContent.String, actualContent.String );
					index++;
				}
			}

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Back_Test() {

			AutoMapper2.Config.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			List<InnerClassType> source = new List<InnerClassType>() {
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
			};
			List<InnerClassType> destination = new List<InnerClassType>();

			List<PropertyChangedResults> changeList = AutoMapper2.MapBack<List<InnerClassType>, List<InnerClassType>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				int index = 0;
				foreach ( InnerClassType actualContent in source ) {
					InnerClassType thisContent = (
						from t in destination
						where t.Key == actualContent.Key
						select t
						).FirstOrDefault();
					Assert.IsNotNull( thisContent );
					Assert.AreEqual( thisContent.Key, actualContent.Key );
					Assert.AreEqual( thisContent.Integer, actualContent.Integer );
					Assert.AreEqual( thisContent.Double, actualContent.Double );
					Assert.AreEqual( thisContent.String, actualContent.String );
					index++;
				}
			}

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Test_With_Null_Properties() {

			AutoMapper2.Config.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			List<InnerClassType> source = new List<InnerClassType>();
			List<InnerClassType> destination = new List<InnerClassType>();
			List<PropertyChangedResults> changeList = AutoMapper2.Map<List<InnerClassType>, List<InnerClassType>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType actualContent in source ) {
					InnerClassType thisContent = (
						from t in destination
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

		[Test]
		public void ChangeList_Contains_Changes_SameClass_Test_With_Null_Back_Properties() {

			AutoMapper2.Config.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			List<InnerClassType> source = new List<InnerClassType>();
			List<InnerClassType> destination = new List<InnerClassType>();
			List<PropertyChangedResults> changeList = AutoMapper2.Map<List<InnerClassType>, List<InnerClassType>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType actualContent in source ) {
					InnerClassType thisContent = (
						from t in destination
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

		[Test]
		public void ChangeList_Contains_Changes_SameClass_PartiallyFilled_Test() {

			AutoMapper2.Config.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			List<InnerClassType> source = new List<InnerClassType>() {
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
			};
			List<InnerClassType> destination = new List<InnerClassType>() {
				new InnerClassType {
					Key = 1,
					Double = 10.0,
					Integer = 10,
					String = "Ten",
				}
			};

			List<PropertyChangedResults> changeList = AutoMapper2.Map<List<InnerClassType>, List<InnerClassType>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				int index = 0;
				foreach ( InnerClassType actualContent in source ) {
					InnerClassType thisContent = (
						from t in destination
						where t.Key == actualContent.Key
						select t
						).FirstOrDefault();
					Assert.IsNotNull( thisContent );
					Assert.AreEqual( thisContent.Key, actualContent.Key );
					Assert.AreEqual( thisContent.Integer, actualContent.Integer );
					Assert.AreEqual( thisContent.Double, actualContent.Double );
					Assert.AreEqual( thisContent.String, actualContent.String );
					index++;
				}
			}

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_TooFull_Test() {

			AutoMapper2.Config.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			List<InnerClassType> source = new List<InnerClassType>() {
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
			};
			List<InnerClassType> destination = new List<InnerClassType>() {
				new InnerClassType {
					Key = 1,
					Double = 10.0,
					Integer = 10,
					String = "Ten",
				},
				new InnerClassType {
					Key = 2,
					Double = 3.0,
					Integer = 4,
					String = "Five",
				},
				new InnerClassType {
					Key = 4,
					Double = 5.0,
					Integer = 6,
					String = "Seven",
				}
			};

			List<PropertyChangedResults> changeList = AutoMapper2.Map<List<InnerClassType>, List<InnerClassType>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				int index = 0;
				foreach ( InnerClassType actualContent in source ) {
					InnerClassType thisContent = (
						from t in destination
						where t.Key == actualContent.Key
						select t
						).FirstOrDefault();
					Assert.IsNotNull( thisContent );
					Assert.AreEqual( thisContent.Key, actualContent.Key );
					Assert.AreEqual( thisContent.Integer, actualContent.Integer );
					Assert.AreEqual( thisContent.Double, actualContent.Double );
					Assert.AreEqual( thisContent.String, actualContent.String );
					index++;
				}
			}

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_TooFull_Back_Test() {

			AutoMapper2.Config.CreateMap<List<InnerClassType>, List<InnerClassType>>();

			List<InnerClassType> source = new List<InnerClassType>() {
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
			};
			List<InnerClassType> destination = new List<InnerClassType>() {
				new InnerClassType {
					Key = 1,
					Double = 10.0,
					Integer = 10,
					String = "Ten",
				},
				new InnerClassType {
					Key = 2,
					Double = 3.0,
					Integer = 4,
					String = "Five",
				},
				new InnerClassType {
					Key = 4,
					Double = 5.0,
					Integer = 6,
					String = "Seven",
				}
			};

			List<PropertyChangedResults> changeList = AutoMapper2.MapBack<List<InnerClassType>, List<InnerClassType>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( 4, destination.Count );
			if ( source.Count > 0 ) {
				int index = 0;
				foreach ( InnerClassType actualContent in source ) {
					InnerClassType thisContent = (
						from t in destination
						where t.Key == actualContent.Key
						select t
						).FirstOrDefault();
					Assert.IsNotNull( thisContent );
					Assert.AreEqual( thisContent.Key, actualContent.Key );
					Assert.AreEqual( thisContent.Integer, actualContent.Integer );
					Assert.AreEqual( thisContent.Double, actualContent.Double );
					Assert.AreEqual( thisContent.String, actualContent.String );
					index++;
				}
			}

		}

		private class InnerClassType {
			[PrimaryKey]
			public int Key { get; set; }
			public int Integer { get; set; }
			public double Double { get; set; }
			public string String { get; set; }
		}

		#endregion

		#region Class_To_DifferentProperties_DestKey_Class

		[Test]
		public void Class_To_DifferentProperties_DestKey_Class() {

			AutoMapper2.Config.CreateMap<List<InnerClassType1a>, List<InnerClassType2a>>();

			List<InnerClassType1a> source = new List<InnerClassType1a>() {
					new InnerClassType1a {
						Key = 1,
						Double = 2.0,
						Integer = 3,
						String = "4",
					},
					new InnerClassType1a {
						Key = 2,
						Double = 3.0,
						Integer = 4,
						String = "5",
					},
					new InnerClassType1a {
						Key = 3,
						Double = 4.0,
						Integer = 5,
						String = "6",
					}
			};
			List<InnerClassType2a> destinationTemplate = new List<InnerClassType2a>() {
					new InnerClassType2a {
						Key = 1,
						Double = "2",
						Integer = 3.0,
						String = 4,
					},
					new InnerClassType2a {
						Key = 2,
						Double = "3",
						Integer = 4.0,
						String = 5,
					},
					new InnerClassType2a {
						Key = 3,
						Double = "4",
						Integer = 5.0,
						String = 6,
					}
			};

			List<InnerClassType2a> destination = AutoMapper2.Map<List<InnerClassType1a>, List<InnerClassType2a>>( source );


			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2a actualContent in destinationTemplate ) {
					InnerClassType2a thisContent = (
						from t in destination
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

		[Test]
		public void Class_To_DifferentProperties_DestKey_Back_Class() {

			AutoMapper2.Config.CreateMap<List<InnerClassType2a>, List<InnerClassType1a>>();

			List<InnerClassType1a> source = new List<InnerClassType1a>() {
					new InnerClassType1a {
						Key = 1,
						Double = 2.0,
						Integer = 3,
						String = "4",
					},
					new InnerClassType1a {
						Key = 2,
						Double = 3.0,
						Integer = 4,
						String = "5",
					},
					new InnerClassType1a {
						Key = 3,
						Double = 4.0,
						Integer = 5,
						String = "6",
					}
			};
			List<InnerClassType2a> destinationTemplate = new List<InnerClassType2a>() {
					new InnerClassType2a {
						Key = 1,
						Double = "2",
						Integer = 3.0,
						String = 4,
					},
					new InnerClassType2a {
						Key = 2,
						Double = "3",
						Integer = 4.0,
						String = 5,
					},
					new InnerClassType2a {
						Key = 3,
						Double = "4",
						Integer = 5.0,
						String = 6,
					}
			};

			List<InnerClassType2a> destination = new List<InnerClassType2a>();
			var results = AutoMapper2.MapBack<List<InnerClassType2a>, List<InnerClassType1a>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2a actualContent in destinationTemplate ) {
					InnerClassType2a thisContent = (
						from t in destination
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

		[Test]
		public void Class_To_DifferentProperties_DestKey_Class_With_Null_Properties() {

			AutoMapper2.Config.CreateMap<List<InnerClassType1a>, List<InnerClassType2a>>();

			List<InnerClassType1a> source = new List<InnerClassType1a>();

			List<InnerClassType2a> destinationTemplate = new List<InnerClassType2a>();

			List<InnerClassType2a> destination = AutoMapper2.Map<List<InnerClassType1a>, List<InnerClassType2a>>( source );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2a actualContent in destinationTemplate ) {
					InnerClassType2a thisContent = (
						from t in destination
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

		[Test]
		public void Class_To_DifferentProperties_DestKey_Class_With_Null_Properties_Back() {

			AutoMapper2.Config.CreateMap<List<InnerClassType2a>, List<InnerClassType1a>>();

			List<InnerClassType1a> source = new List<InnerClassType1a>();

			List<InnerClassType2a> destinationTemplate = new List<InnerClassType2a>();

			List<InnerClassType2a> destination = new List<InnerClassType2a>();
			var results = AutoMapper2.MapBack<List<InnerClassType2a>, List<InnerClassType1a>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2a actualContent in destinationTemplate ) {
					InnerClassType2a thisContent = (
						from t in destination
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

		private class InnerClassType1a {
			public int Key { get; set; }
			public int Integer { get; set; }
			public double Double { get; set; }
			public string String { get; set; }
		}
		private class InnerClassType2a {
			[PrimaryKey]
			public int Key { get; set; }
			public double Integer { get; set; }
			public string Double { get; set; }
			public int String { get; set; }
		}

		#endregion

		#region Class_To_DifferentProperties_SourceKey_Class

		[Test]
		public void Class_To_DifferentProperties_SourceKey_Class() {

			AutoMapper2.Config.CreateMap<List<InnerClassType1b>, List<InnerClassType2b>>();

			List<InnerClassType1b> source = new List<InnerClassType1b>() {
					new InnerClassType1b {
						Key = 1,
						Double = 2.0,
						Integer = 3,
						String = "4",
					},
					new InnerClassType1b {
						Key = 2,
						Double = 3.0,
						Integer = 4,
						String = "5",
					},
					new InnerClassType1b {
						Key = 3,
						Double = 4.0,
						Integer = 5,
						String = "6",
					}
			};
			List<InnerClassType2b> destinationTemplate = new List<InnerClassType2b>() {
					new InnerClassType2b {
						Key = 1,
						Double = "2",
						Integer = 3.0,
						String = 4,
					},
					new InnerClassType2b {
						Key = 2,
						Double = "3",
						Integer = 4.0,
						String = 5,
					},
					new InnerClassType2b {
						Key = 3,
						Double = "4",
						Integer = 5.0,
						String = 6,
					}
			};

			List<InnerClassType2b> destination = AutoMapper2.Map<List<InnerClassType1b>, List<InnerClassType2b>>( source );


			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2b actualContent in destinationTemplate ) {
					InnerClassType2b thisContent = (
						from t in destination
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

		[Test]
		public void Class_To_DifferentProperties_SourceKey_Class_Back() {

			AutoMapper2.Config.CreateMap<List<InnerClassType2b>, List<InnerClassType1b>>();

			List<InnerClassType1b> source = new List<InnerClassType1b>() {
					new InnerClassType1b {
						Key = 1,
						Double = 2.0,
						Integer = 3,
						String = "4",
					},
					new InnerClassType1b {
						Key = 2,
						Double = 3.0,
						Integer = 4,
						String = "5",
					},
					new InnerClassType1b {
						Key = 3,
						Double = 4.0,
						Integer = 5,
						String = "6",
					}
			};
			List<InnerClassType2b> destinationTemplate = new List<InnerClassType2b>() {
					new InnerClassType2b {
						Key = 1,
						Double = "2",
						Integer = 3.0,
						String = 4,
					},
					new InnerClassType2b {
						Key = 2,
						Double = "3",
						Integer = 4.0,
						String = 5,
					},
					new InnerClassType2b {
						Key = 3,
						Double = "4",
						Integer = 5.0,
						String = 6,
					}
			};

			List<InnerClassType2b> destination = new List<InnerClassType2b>();
			var results = AutoMapper2.MapBack<List<InnerClassType2b>, List<InnerClassType1b>>( source, ref destination );


			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2b actualContent in destinationTemplate ) {
					InnerClassType2b thisContent = (
						from t in destination
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

		[Test]
		public void Class_To_DifferentProperties_SourceKey_Class_With_Null_Properties() {

			AutoMapper2.Config.CreateMap<List<InnerClassType1b>, List<InnerClassType2b>>();

			List<InnerClassType1b> source = new List<InnerClassType1b>();

			List<InnerClassType2b> destinationTemplate = new List<InnerClassType2b>();

			List<InnerClassType2b> destination = AutoMapper2.Map<List<InnerClassType1b>, List<InnerClassType2b>>( source );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2b actualContent in destinationTemplate ) {
					InnerClassType2b thisContent = (
						from t in destination
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

		[Test]
		public void Class_To_DifferentProperties_SourceKey_Class_With_Null_Properties_Back() {

			AutoMapper2.Config.CreateMap<List<InnerClassType2b>, List<InnerClassType1b>>();

			List<InnerClassType1b> source = new List<InnerClassType1b>();

			List<InnerClassType2b> destinationTemplate = new List<InnerClassType2b>();

			List<InnerClassType2b> destination = new List<InnerClassType2b>();
			var results = AutoMapper2.MapBack<List<InnerClassType2b>, List<InnerClassType1b>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2b actualContent in destinationTemplate ) {
					InnerClassType2b thisContent = (
						from t in destination
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

		private class InnerClassType1b {
			[PrimaryKey]
			public int Key { get; set; }
			public int Integer { get; set; }
			public double Double { get; set; }
			public string String { get; set; }
		}
		private class InnerClassType2b {
			public int Key { get; set; }
			public double Integer { get; set; }
			public string Double { get; set; }
			public int String { get; set; }
		}

		#endregion

		#region ChangeList_Contains_Changes_SameClass_MultiKey_Test

		[Test]
		public void ChangeList_Contains_Changes_SameClass_MultiKey_Test() {

			AutoMapper2.Config.CreateMap<List<InnerClassType2>, List<InnerClassType2>>();

			List<InnerClassType2> source = new List<InnerClassType2>() {
				new InnerClassType2 {
					Key = 1,
					Double = 2.0,
					Integer = 3,
					String = "Four",
				},
				new InnerClassType2 {
					Key = 2,
					Double = 3.0,
					Integer = 4,
					String = "Five",
				},
				new InnerClassType2 {
					Key = 3,
					Double = 4.0,
					Integer = 5,
					String = "Six",
				}
			};
			List<InnerClassType2> destination = new List<InnerClassType2>();

			List<PropertyChangedResults> changeList = AutoMapper2.Map<List<InnerClassType2>, List<InnerClassType2>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				int index = 0;
				foreach ( InnerClassType2 actualContent in source ) {
					InnerClassType2 thisContent = (
						from t in destination
						where t.Key == actualContent.Key
						select t
						).FirstOrDefault();
					Assert.IsNotNull( thisContent );
					Assert.AreEqual( thisContent.Key, actualContent.Key );
					Assert.AreEqual( thisContent.Integer, actualContent.Integer );
					Assert.AreEqual( thisContent.Double, actualContent.Double );
					Assert.AreEqual( thisContent.String, actualContent.String );
					index++;
				}
			}

		}

		[Test]
		public void ChangeList_Contains_Changes_SameClass_MultiKey_Test_With_Null_Properties() {

			AutoMapper2.Config.CreateMap<List<InnerClassType2>, List<InnerClassType2>>();

			List<InnerClassType2> source = new List<InnerClassType2>();
			List<InnerClassType2> destination = new List<InnerClassType2>();
			List<PropertyChangedResults> changeList = AutoMapper2.Map<List<InnerClassType2>, List<InnerClassType2>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2 actualContent in source ) {
					InnerClassType2 thisContent = (
						from t in destination
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

		[Test]
		public void ChangeList_Contains_Changes_SameClass_PartiallyFilled_MultiKey_Test() {

			AutoMapper2.Config.CreateMap<List<InnerClassType2>, List<InnerClassType2>>();

			List<InnerClassType2> source = new List<InnerClassType2>() {
				new InnerClassType2 {
					Key = 1,
					Double = 2.0,
					Integer = 3,
					String = "Four",
				},
				new InnerClassType2 {
					Key = 2,
					Double = 3.0,
					Integer = 4,
					String = "Five",
				},
				new InnerClassType2 {
					Key = 3,
					Double = 4.0,
					Integer = 5,
					String = "Six",
				}
			};
			List<InnerClassType2> destination = new List<InnerClassType2>() {
				new InnerClassType2 {
					Key = 1,
					Double = 10.0,
					Integer = 3,
					String = "Ten",
				}
			};

			List<PropertyChangedResults> changeList = AutoMapper2.Map<List<InnerClassType2>, List<InnerClassType2>>( source, ref destination );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				int index = 0;
				foreach ( InnerClassType2 actualContent in source ) {
					InnerClassType2 thisContent = (
						from t in destination
						where t.Key == actualContent.Key
						select t
						).FirstOrDefault();
					Assert.IsNotNull( thisContent );
					Assert.AreEqual( thisContent.Key, actualContent.Key );
					Assert.AreEqual( thisContent.Integer, actualContent.Integer );
					Assert.AreEqual( thisContent.Double, actualContent.Double );
					Assert.AreEqual( thisContent.String, actualContent.String );
					index++;
				}
			}

		}

		private class InnerClassType2 {
			[PrimaryKey]
			public int Key { get; set; }
			[PrimaryKey]
			public int Integer { get; set; }
			public double Double { get; set; }
			public string String { get; set; }
		}

		#endregion

		#region Class_To_DifferentProperties_DestKey_MultiKey_Class

		[Test]
		public void Class_To_DifferentProperties_DestKey_MultiKey_Class() {

			AutoMapper2.Config.CreateMap<List<InnerClassType1c>, List<InnerClassType2c>>();

			List<InnerClassType1c> source = new List<InnerClassType1c>() {
					new InnerClassType1c {
						Key = 1,
						Double = 2.0,
						Integer = 3,
						String = "4",
					},
					new InnerClassType1c {
						Key = 2,
						Double = 3.0,
						Integer = 4,
						String = "5",
					},
					new InnerClassType1c {
						Key = 3,
						Double = 4.0,
						Integer = 5,
						String = "6",
					}
			};
			List<InnerClassType2c> destinationTemplate = new List<InnerClassType2c>() {
					new InnerClassType2c {
						Key = 1,
						Double = "2",
						Integer = 3.0,
						String = 4,
					},
					new InnerClassType2c {
						Key = 2,
						Double = "3",
						Integer = 4.0,
						String = 5,
					},
					new InnerClassType2c {
						Key = 3,
						Double = "4",
						Integer = 5.0,
						String = 6,
					}
			};

			List<InnerClassType2c> destination = AutoMapper2.Map<List<InnerClassType1c>, List<InnerClassType2c>>( source );


			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2c actualContent in destinationTemplate ) {
					InnerClassType2c thisContent = (
						from t in destination
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

		[Test]
		public void Class_To_DifferentProperties_DestKey_MultiKey_Class_With_Null_Properties() {

			AutoMapper2.Config.CreateMap<List<InnerClassType1c>, List<InnerClassType2c>>();

			List<InnerClassType1c> source = new List<InnerClassType1c>();

			List<InnerClassType2c> destinationTemplate = new List<InnerClassType2c>();

			List<InnerClassType2c> destination = AutoMapper2.Map<List<InnerClassType1c>, List<InnerClassType2c>>( source );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2c actualContent in destinationTemplate ) {
					InnerClassType2c thisContent = (
						from t in destination
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

		private class InnerClassType1c {
			public int Key { get; set; }
			public int Integer { get; set; }
			public double Double { get; set; }
			public string String { get; set; }
		}
		private class InnerClassType2c {
			[PrimaryKey]
			public int Key { get; set; }
			[PrimaryKey]
			public double Integer { get; set; }
			public string Double { get; set; }
			public int String { get; set; }
		}

		#endregion

		#region Class_To_DifferentProperties_SourceKey_MultiKey_Class

		[Test]
		public void Class_To_DifferentProperties_SourceKey_MultiKey_Class() {

			AutoMapper2.Config.CreateMap<List<InnerClassType1d>, List<InnerClassType2d>>();

			List<InnerClassType1d> source = new List<InnerClassType1d>() {
					new InnerClassType1d {
						Key = 1,
						Double = 2.0,
						Integer = 3,
						String = "4",
					},
					new InnerClassType1d {
						Key = 2,
						Double = 3.0,
						Integer = 4,
						String = "5",
					},
					new InnerClassType1d {
						Key = 3,
						Double = 4.0,
						Integer = 5,
						String = "6",
					}
			};
			List<InnerClassType2d> destinationTemplate = new List<InnerClassType2d>() {
					new InnerClassType2d {
						Key = 1,
						Double = "2",
						Integer = 3.0,
						String = 4,
					},
					new InnerClassType2d {
						Key = 2,
						Double = "3",
						Integer = 4.0,
						String = 5,
					},
					new InnerClassType2d {
						Key = 3,
						Double = "4",
						Integer = 5.0,
						String = 6,
					}
			};

			List<InnerClassType2d> destination = AutoMapper2.Map<List<InnerClassType1d>, List<InnerClassType2d>>( source );


			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2d actualContent in destinationTemplate ) {
					InnerClassType2d thisContent = (
						from t in destination
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

		[Test]
		public void Class_To_DifferentProperties_SourceKey_Class_MultiKey_With_Null_Properties() {

			AutoMapper2.Config.CreateMap<List<InnerClassType1d>, List<InnerClassType2d>>();

			List<InnerClassType1d> source = new List<InnerClassType1d>();

			List<InnerClassType2d> destinationTemplate = new List<InnerClassType2d>();

			List<InnerClassType2d> destination = AutoMapper2.Map<List<InnerClassType1d>, List<InnerClassType2d>>( source );

			Assert.IsNotNull( destination );
			Assert.AreEqual( source.Count, destination.Count );
			if ( source.Count > 0 ) {
				foreach ( InnerClassType2d actualContent in destinationTemplate ) {
					InnerClassType2d thisContent = (
						from t in destination
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

		private class InnerClassType1d {
			[PrimaryKey]
			public int Key { get; set; }
			[PrimaryKey]
			public int Integer { get; set; }
			public double Double { get; set; }
			public string String { get; set; }
		}
		private class InnerClassType2d {
			public int Key { get; set; }
			public double Integer { get; set; }
			public string Double { get; set; }
			public int String { get; set; }
		}

		#endregion

		#region ListOf_NoPrimaryKey_Fails
		[Test]
		public void ListOf_NoPrimaryKey_Fails() {

			AutoMapper2.Config.CreateMap<List<NoPrimaryKeyClass>, List<NoPrimaryKeyClass>>();

			List<NoPrimaryKeyClass> source = new List<NoPrimaryKeyClass>();
			List<NoPrimaryKeyClass> destination = new List<NoPrimaryKeyClass>();

			try {
				var changes = AutoMapper2.Map<List<NoPrimaryKeyClass>, List<NoPrimaryKeyClass>>( source, ref destination );
				Assert.Fail("A list of class with no primary keys can't map");
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.MissingPrimaryKey, ex.InvalidPropertyReason );
				Assert.IsNull( ex.PropertyInfo );
				Assert.AreEqual( typeof( List<NoPrimaryKeyClass> ), ex.From );
				Assert.AreEqual( typeof( List<NoPrimaryKeyClass> ), ex.To );
			}

		}

		private class NoPrimaryKeyClass {
			public int Property1 { get; set; }
		}
		#endregion

		#region ListOf_NoProperties_Fails
		[Test]
		public void ListOf_NoProperties_Fails() {

			AutoMapper2.Config.CreateMap<List<NoPropertiesType>, List<NoPropertiesType>>();

			List<NoPropertiesType> source = new List<NoPropertiesType>();
			List<NoPropertiesType> destination = new List<NoPropertiesType>();

			try {
				var changes = AutoMapper2.Map<List<NoPropertiesType>, List<NoPropertiesType>>( source, ref destination );
				Assert.Fail( "A list of class with no properties can't map" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.MissingPrimaryKey, ex.InvalidPropertyReason );
				Assert.IsNull( ex.PropertyInfo );
				Assert.AreEqual( typeof( List<NoPropertiesType> ), ex.From );
				Assert.AreEqual( typeof( List<NoPropertiesType> ), ex.To );
			}

		}

		private class NoPropertiesType {
		}
		#endregion

		#region ListOf_NullPrimaryKey_Fails
		[Test]
		public void ListOf_NullPrimaryKey_Source_Fails() {

			AutoMapper2.Config.CreateMap<List<NullPrimaryKeyClass>, List<NullPrimaryKeyClass>>();

			List<NullPrimaryKeyClass> source = new List<NullPrimaryKeyClass> {
				new NullPrimaryKeyClass {
					Property1 = null
				}
			};
			List<NullPrimaryKeyClass> destination = new List<NullPrimaryKeyClass>();

			try {
				var changes = AutoMapper2.Map<List<NullPrimaryKeyClass>, List<NullPrimaryKeyClass>>( source, ref destination );
				Assert.Fail( "A list of class with no primary keys can't map" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.FromPrimaryKeyBlank, ex.InvalidPropertyReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
				Assert.AreEqual( typeof( List<NullPrimaryKeyClass> ), ex.From );
				Assert.AreEqual( typeof( List<NullPrimaryKeyClass> ), ex.To );
			}

		}
		[Test]
		public void ListOf_NullPrimaryKey_Destination_Fails() {

			AutoMapper2.Config.CreateMap<List<NullPrimaryKeyClass>, List<NullPrimaryKeyClass>>();

			List<NullPrimaryKeyClass> source = new List<NullPrimaryKeyClass> {
				new NullPrimaryKeyClass {
					Property1 = 1
				}
			};
			List<NullPrimaryKeyClass> destination = new List<NullPrimaryKeyClass>{
				new NullPrimaryKeyClass {
					Property1 = null
				}
			};

			try {
				var changes = AutoMapper2.Map<List<NullPrimaryKeyClass>, List<NullPrimaryKeyClass>>( source, ref destination );
				Assert.Fail( "A list of class with no primary keys can't map" );
			} catch ( InvalidTypeConversionException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.ToPrimaryKeyBlank, ex.InvalidPropertyReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
				Assert.AreEqual( typeof( List<NullPrimaryKeyClass> ), ex.From );
				Assert.AreEqual( typeof( List<NullPrimaryKeyClass> ), ex.To );
			}

		}

		private class NullPrimaryKeyClass {
			[PrimaryKey]
			public int? Property1 { get; set; }
		}
		#endregion

	}

}
