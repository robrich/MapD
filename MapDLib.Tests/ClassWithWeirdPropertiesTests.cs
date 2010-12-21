namespace MapDLib.Tests {

	#region using
	using System;
	using MapDLib;
	using NUnit.Framework;

	#endregion

	public class ClassWithWeirdPropertiesTests : BaseTest {

		#region EnumProperties_Class

		[Test]
		public void EnumProperties_Class() {

			MapD.Config.CreateMap<EnumProperties_Class_Type1, EnumProperties_Class_Type2>();

			EnumProperties_Class_Type1 source = new EnumProperties_Class_Type1 {
				Enum1 = EnumProperties_Class_Enum.Item1,
				Enum2 = EnumProperties_Class_Enum.Item2,
				Enum3 = "Item3",
				Enum4 = 1,
				Enum5 = EnumProperties_Class_Enum.Item2
			};

			EnumProperties_Class_Type2 destination = MapD.Copy<EnumProperties_Class_Type1, EnumProperties_Class_Type2>( source );

			source.AssertEqual( destination );

		}

		[Test]
		public void EnumProperties_Class_SetToNull() {

			MapD.Config.CreateMap<EnumProperties_Class_Type1, EnumProperties_Class_Type2>();

			EnumProperties_Class_Type1 source = new EnumProperties_Class_Type1();
			EnumProperties_Class_Type2 destination = MapD.Copy<EnumProperties_Class_Type1, EnumProperties_Class_Type2>( source );
			source.AssertEqual( destination );

		}

		public enum EnumProperties_Class_Enum {
			NotSet = 0,
			Item1,
			Item2,
			Item3
		}

		private class EnumProperties_Class_Type1 {
			public EnumProperties_Class_Enum Enum1 { get; set; }
			public EnumProperties_Class_Enum Enum2 { get; set; }
			public string Enum3 { get; set; }
			public int Enum4 { get; set; }
			public EnumProperties_Class_Enum Enum5 { get; set; }

			public void AssertEqual( EnumProperties_Class_Type2 Actual ) {
				Assert.IsNotNull( Actual );
				Assert.AreEqual( this.Enum1.ToString(), Actual.Enum1, string.Format( "Expected: {0}, Actual: {1}", this.Enum1, Actual.Enum1 ) );
				Assert.AreEqual( this.Enum2, Actual.Enum2, string.Format( "Expected: {0}, Actual: {1}", this.Enum2, Actual.Enum2 ) );
				string enum3 = this.Enum3 ?? EnumProperties_Class_Enum.NotSet.ToString(); // null turns into NotSet
				Assert.AreEqual( enum3, Actual.Enum3.ToString(), string.Format( "Expected: {0}, Actual: {1}", this.Enum3, Actual.Enum3 ) );
				Assert.AreEqual( this.Enum4, (int)Actual.Enum4, string.Format( "Expected: {0}, Actual: {1}", this.Enum4, Actual.Enum4 ) );
				Assert.AreEqual( (int)this.Enum5, Actual.Enum5, string.Format( "Expected: {0}, Actual: {1}", this.Enum5, Actual.Enum5 ) );
			}
		}
		private class EnumProperties_Class_Type2 {
			public string Enum1 { get; set; }
			public EnumProperties_Class_Enum Enum2 { get; set; }
			public EnumProperties_Class_Enum Enum3 { get; set; }
			public EnumProperties_Class_Enum Enum4 { get; set; }
			public int Enum5 { get; set; }
		}

		#endregion

		#region NullableIntProperties_Class

		[Test]
		public void NullableIntProperties_Class() {

			MapD.Config.CreateMap<NullableIntProperties_Class_Type1, NullableIntProperties_Class_Type2>();

			NullableIntProperties_Class_Type1 source = new NullableIntProperties_Class_Type1 {
				Int1 = 123,
				Int2 = 321,
				StringToInt = 1234,
				IntToString = "4321"
			};

			NullableIntProperties_Class_Type2 destination = MapD.Copy<NullableIntProperties_Class_Type1, NullableIntProperties_Class_Type2>( source );

			source.AssertEqual( destination );

		}

		[Test]
		public void NullableIntProperties_Class_SetToNull() {

			MapD.Config.CreateMap<NullableIntProperties_Class_Type1, NullableIntProperties_Class_Type2>();

			NullableIntProperties_Class_Type1 source = new NullableIntProperties_Class_Type1();
			NullableIntProperties_Class_Type2 destination = MapD.Copy<NullableIntProperties_Class_Type1, NullableIntProperties_Class_Type2>( source );
			source.AssertEqual( destination );

		}

		private class NullableIntProperties_Class_Type1 {
			public int Int1 { get; set; }
			public int? Int2 { get; set; }
			public string IntToString { get; set; }
			public int? StringToInt { get; set; }

			public void AssertEqual( NullableIntProperties_Class_Type2 Actual ) {
				Assert.IsNotNull( Actual );
				Assert.IsNotNull( Actual.Int1 ); // int i2 = 0 => int? i2 != null
				Assert.AreEqual( this.Int1, Actual.Int1 ?? 0, string.Format( "Expected: {0}, Actual: {1}", this.Int1, Actual.Int1 ) );
				Assert.AreEqual( this.Int2 ?? 0, Actual.Int2, string.Format( "Expected: {0}, Actual: {1}", this.Int2, Actual.Int2 ) );
				Assert.AreEqual( this.IntToString == null, Actual.IntToString == null, string.Format( "Expected: {0}, Actual: {1}", this.IntToString, Actual.IntToString ) );
				if ( this.IntToString != null ) {
					Assert.AreEqual( this.IntToString, Actual.IntToString.ToString(), string.Format( "Expected: {0}, Actual: {1}", this.IntToString, Actual.IntToString ) );
				}
				Assert.AreEqual( this.StringToInt == null, Actual.StringToInt == null, string.Format( "Expected: {0}, Actual: {1}", this.StringToInt, Actual.StringToInt ) );
				if ( this.IntToString != null ) {
					Assert.AreEqual( this.StringToInt.ToString(), Actual.StringToInt, string.Format( "Expected: {0}, Actual: {1}", this.StringToInt, Actual.StringToInt ) );
				}
			}
		}
		private class NullableIntProperties_Class_Type2 {
			public int? Int1 { get; set; }
			public int Int2 { get; set; }
			public int? IntToString { get; set; }
			public string StringToInt { get; set; }
		}

		#endregion

		#region NullableDoubleProperties_Class

		[Test]
		public void NullableDoubleProperties_Class() {

			MapD.Config.CreateMap<NullableDoubleProperties_Class_Type1, NullableDoubleProperties_Class_Type2>();

			NullableDoubleProperties_Class_Type1 source = new NullableDoubleProperties_Class_Type1 {
				Double1 = 123.123,
				Double2 = 321.321,
				StringToDouble = 1234.1234,
				DoubleToString = "4321.4321"
			};

			NullableDoubleProperties_Class_Type2 destination = MapD.Copy<NullableDoubleProperties_Class_Type1, NullableDoubleProperties_Class_Type2>( source );

			source.AssertEqual( destination );

		}

		[Test]
		public void NullableDoubleProperties_Class_SetToNull() {

			MapD.Config.CreateMap<NullableDoubleProperties_Class_Type1, NullableDoubleProperties_Class_Type2>();

			NullableDoubleProperties_Class_Type1 source = new NullableDoubleProperties_Class_Type1();
			NullableDoubleProperties_Class_Type2 destination = MapD.Copy<NullableDoubleProperties_Class_Type1, NullableDoubleProperties_Class_Type2>( source );
			source.AssertEqual( destination );

		}

		private class NullableDoubleProperties_Class_Type1 {
			public double Double1 { get; set; }
			public double? Double2 { get; set; }
			public string DoubleToString { get; set; }
			public double? StringToDouble { get; set; }

			public void AssertEqual( NullableDoubleProperties_Class_Type2 Actual ) {
				Assert.IsNotNull( Actual.Double1 ); // double i2 = 0 => double? i2 != null
				Assert.AreEqual( this.Double1, Actual.Double1 ?? 0, string.Format( "Expected: {0}, Actual: {1}", this.Double1, Actual.Double1 ) );
				Assert.AreEqual( this.Double2 ?? 0, Actual.Double2, string.Format( "Expected: {0}, Actual: {1}", this.Double2, Actual.Double2 ) );
				Assert.AreEqual( this.DoubleToString == null, Actual.DoubleToString == null, string.Format( "Expected: {0}, Actual: {1}", this.DoubleToString, Actual.DoubleToString ) );
				if ( this.DoubleToString != null ) {
					Assert.AreEqual( this.DoubleToString, Actual.DoubleToString.ToString(), string.Format( "Expected: {0}, Actual: {1}", this.DoubleToString, Actual.DoubleToString ) );
				}
				Assert.AreEqual( this.StringToDouble == null, Actual.StringToDouble == null, string.Format( "Expected: {0}, Actual: {1}", this.StringToDouble, Actual.StringToDouble ) );
				if ( this.DoubleToString != null ) {
					Assert.AreEqual( this.StringToDouble.ToString(), Actual.StringToDouble, string.Format( "Expected: {0}, Actual: {1}", this.StringToDouble, Actual.StringToDouble ) );
				}
			}
		}
		private class NullableDoubleProperties_Class_Type2 {
			public double? Double1 { get; set; }
			public double Double2 { get; set; }
			public double? DoubleToString { get; set; }
			public string StringToDouble { get; set; }
		}

		#endregion

		#region NullableDateTimeProperties_Class

		[Test]
		public void NullableDateTimeProperties_Class() {

			MapD.Config.CreateMap<NullableDateTimeProperties_Class_Type1, NullableDateTimeProperties_Class_Type2>();

			NullableDateTimeProperties_Class_Type1 source = new NullableDateTimeProperties_Class_Type1 {
				DateTime1 = DateTime.Now,
				DateTime2 = DateTime.Now.AddDays( 1 ),
				StringToDateTime = DateTime.Now.AddDays( -1 ),
				DateTimeToString = DateTime.Now.AddDays( -1 ).ToString()
			};

			NullableDateTimeProperties_Class_Type2 destination = MapD.Copy<NullableDateTimeProperties_Class_Type1, NullableDateTimeProperties_Class_Type2>( source );

			source.AssertEqual( destination );

		}

		[Test]
		public void NullableDateTimeProperties_Class_SetToNull() {

			MapD.Config.CreateMap<NullableDateTimeProperties_Class_Type1, NullableDateTimeProperties_Class_Type2>();

			NullableDateTimeProperties_Class_Type1 source = new NullableDateTimeProperties_Class_Type1();
			NullableDateTimeProperties_Class_Type2 destination = MapD.Copy<NullableDateTimeProperties_Class_Type1, NullableDateTimeProperties_Class_Type2>( source );
			source.AssertEqual( destination );

		}

		private class NullableDateTimeProperties_Class_Type1 {
			public DateTime DateTime1 { get; set; }
			public DateTime? DateTime2 { get; set; }
			public string DateTimeToString { get; set; }
			public DateTime? StringToDateTime { get; set; }

			public void AssertEqual( NullableDateTimeProperties_Class_Type2 Actual ) {
				Assert.IsNotNull( Actual.DateTime1 ); // DateTime i2 = new DateTime() => double? i2 != null
				Assert.AreEqual( this.DateTime1, Actual.DateTime1.Value, string.Format( "Expected: {0}, Actual: {1}", this.DateTime1, Actual.DateTime1 ) );
				// DaetTime? i1 = null => DateTime i1 = new DateTime() or new DateTime( 1, 1, 1 );
				Assert.AreEqual( this.DateTime2 ?? new DateTime(), Actual.DateTime2, string.Format( "Expected: {0}, Actual: {1}", this.DateTime2, Actual.DateTime2 ) );
				Assert.AreEqual( this.DateTimeToString == null, Actual.DateTimeToString == null, string.Format( "Expected: {0}, Actual: {1}", this.DateTimeToString, Actual.DateTimeToString ) );
				if ( this.DateTimeToString != null ) {
					Assert.AreEqual( this.DateTimeToString, Actual.DateTimeToString.ToString(), string.Format( "Expected: {0}, Actual: {1}", this.DateTimeToString, Actual.DateTimeToString ) );
				}
				Assert.AreEqual( this.StringToDateTime == null, Actual.StringToDateTime == null, string.Format( "Expected: {0}, Actual: {1}", this.StringToDateTime, Actual.StringToDateTime ) );
				if ( this.DateTimeToString != null ) {
					Assert.AreEqual( this.StringToDateTime.ToString(), Actual.StringToDateTime, string.Format( "Expected: {0}, Actual: {1}", this.StringToDateTime, Actual.StringToDateTime ) );
				}
			}
		}
		private class NullableDateTimeProperties_Class_Type2 {
			public DateTime? DateTime1 { get; set; }
			public DateTime DateTime2 { get; set; }
			public DateTime? DateTimeToString { get; set; }
			public string StringToDateTime { get; set; }
		}

		#endregion

	}

}
