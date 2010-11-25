namespace AutoMapper2Lib.Tests {

	#region using
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class IgnorePropertiesIfTests : BaseTest {

		[Test]
		public void Default_Write_Fails() {

			AutoMapper2.CreateMap<IgnorePropertiesIfWriteClass, IgnorePropertiesIfWriteClass>();

			try {
				IgnorePropertiesIfWriteClass dest = AutoMapper2.Map<IgnorePropertiesIfWriteClass, IgnorePropertiesIfWriteClass>( new IgnorePropertiesIfWriteClass() );
				Assert.Fail( "A read-only property didn't fail" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.CantWrite, ex.InvalidPropertyReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		[Test]
		public void Default_Read_Fails() {

			AutoMapper2.CreateMap<IgnorePropertiesIfReadClass, IgnorePropertiesIfReadClass>();

			try {
				IgnorePropertiesIfReadClass dest = AutoMapper2.Map<IgnorePropertiesIfReadClass, IgnorePropertiesIfReadClass>( new IgnorePropertiesIfReadClass() );
				Assert.Fail( "A write-only property didn't fail" );
			} catch ( InvalidPropertyException ex ) {
				Assert.IsNotNull( ex );
				Assert.IsNotNull( ex.Message );
				Assert.AreEqual( InvalidPropertyReason.CantRead, ex.InvalidPropertyReason );
				Assert.IsNotNull( ex.PropertyInfo );
				Assert.AreEqual( "Property1", ex.PropertyInfo.Name );
			}

		}

		[Test]
		public void IgnoreIf_Write_Succeeds() {

			AutoMapper2.IgnorePropertiesIf = PropertyIs.WriteOnly;
			AutoMapper2.CreateMap<IgnorePropertiesIfWriteClass, IgnorePropertiesIfWriteClass>();

			IgnorePropertiesIfWriteClass dest = AutoMapper2.Map<IgnorePropertiesIfWriteClass, IgnorePropertiesIfWriteClass>( new IgnorePropertiesIfWriteClass() );
			// It worked
		}

		[Test]
		public void IgnoreIf_Read_Succeeds() {

			AutoMapper2.IgnorePropertiesIf = PropertyIs.ReadOnly;
			AutoMapper2.CreateMap<IgnorePropertiesIfReadClass, IgnorePropertiesIfReadClass>();

			IgnorePropertiesIfReadClass dest = AutoMapper2.Map<IgnorePropertiesIfReadClass, IgnorePropertiesIfReadClass>( new IgnorePropertiesIfReadClass() );
			// It worked

		}

		[Test]
		public void IgnoreIf_WriteAndRead_Succeeds() {

			AutoMapper2.IgnorePropertiesIf = PropertyIs.WriteOnly | PropertyIs.ReadOnly;
			AutoMapper2.CreateMap<IgnorePropertiesIfWriteClass, IgnorePropertiesIfWriteClass>();

			IgnorePropertiesIfWriteClass dest = AutoMapper2.Map<IgnorePropertiesIfWriteClass, IgnorePropertiesIfWriteClass>( new IgnorePropertiesIfWriteClass() );
			// It worked
		}

		[Test]
		public void IgnoreIf_ReadAndWrite_Succeeds() {

			AutoMapper2.IgnorePropertiesIf = PropertyIs.ReadOnly | PropertyIs.WriteOnly;
			AutoMapper2.CreateMap<IgnorePropertiesIfReadClass, IgnorePropertiesIfReadClass>();

			IgnorePropertiesIfReadClass dest = AutoMapper2.Map<IgnorePropertiesIfReadClass, IgnorePropertiesIfReadClass>( new IgnorePropertiesIfReadClass() );
			// It worked

		}

		public class IgnorePropertiesIfReadClass {
			public int Property1 { set { } }
		}
		public class IgnorePropertiesIfWriteClass {
			public int Property1 { get { return 0; } }
		}

		[Test]
		public void IgnoreIf_Write_OnSourceClass_Succeeds() {

			AutoMapper2.CreateMap<IgnorePropertiesIfWriteOnClass, IgnorePropertiesIfReadOnClass>();

			IgnorePropertiesIfReadOnClass dest = AutoMapper2.Map<IgnorePropertiesIfWriteOnClass, IgnorePropertiesIfReadOnClass>( new IgnorePropertiesIfWriteOnClass() );
			// It worked
		}

		[Test]
		public void IgnoreIf_Read_OnSourceClass_Succeeds() {

			AutoMapper2.CreateMap<IgnorePropertiesIfReadOnClass, IgnorePropertiesIfWriteOnClass>();

			IgnorePropertiesIfWriteOnClass dest = AutoMapper2.Map<IgnorePropertiesIfReadOnClass, IgnorePropertiesIfWriteOnClass>( new IgnorePropertiesIfReadOnClass() );
			// It worked

		}

		[Test]
		public void IgnoreIf_Write_OnDestinationClass_Succeeds() {

			AutoMapper2.CreateMap<IgnorePropertiesIfReadOnClass, IgnorePropertiesIfWriteOnClass>();

			IgnorePropertiesIfWriteOnClass dest = AutoMapper2.Map<IgnorePropertiesIfReadOnClass, IgnorePropertiesIfWriteOnClass>( new IgnorePropertiesIfReadOnClass() );
			// It worked
		}

		[Test]
		public void IgnoreIf_Read_OnDestinationClass_Succeeds() {

			AutoMapper2.CreateMap<IgnorePropertiesIfWriteOnClass, IgnorePropertiesIfReadOnClass>();

			IgnorePropertiesIfReadOnClass dest = AutoMapper2.Map<IgnorePropertiesIfWriteOnClass, IgnorePropertiesIfReadOnClass>( new IgnorePropertiesIfWriteOnClass() );
			// It worked

		}

		[IgnorePropertiesIf( PropertyIs.WriteOnly )]
		public class IgnorePropertiesIfReadOnClass {
			public int Property1 { set { } }
		}
		[IgnorePropertiesIf( PropertyIs.ReadOnly )]
		public class IgnorePropertiesIfWriteOnClass {
			public int Property1 { get { return 0; } }
		}

	}

}
