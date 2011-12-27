namespace MapDLib.Tests {

	#region using
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class IgnorePropertiesIfTests : BaseTest {

		[Test]
		public void Default_Write_Fails() {

			MapD.Config.CreateMap<IgnorePropertiesIfWriteClass, IgnorePropertiesIfWriteClass>();

			try {
				IgnorePropertiesIfWriteClass dest = MapD.Copy<IgnorePropertiesIfWriteClass, IgnorePropertiesIfWriteClass>( new IgnorePropertiesIfWriteClass() );
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

			MapD.Config.CreateMap<IgnorePropertiesIfReadClass, IgnorePropertiesIfReadClass>();

			try {
				IgnorePropertiesIfReadClass dest = MapD.Copy<IgnorePropertiesIfReadClass, IgnorePropertiesIfReadClass>( new IgnorePropertiesIfReadClass() );
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

			MapD.Config.IgnorePropertiesIf = PropertyIs.WriteOnly;
			MapD.Config.CreateMap<IgnorePropertiesIfWriteClass, IgnorePropertiesIfWriteClass>();

			IgnorePropertiesIfWriteClass dest = MapD.Copy<IgnorePropertiesIfWriteClass, IgnorePropertiesIfWriteClass>( new IgnorePropertiesIfWriteClass() );
			// It worked
		}

		[Test]
		public void IgnoreIf_Read_Succeeds() {

			MapD.Config.IgnorePropertiesIf = PropertyIs.ReadOnly;
			MapD.Config.CreateMap<IgnorePropertiesIfReadClass, IgnorePropertiesIfReadClass>();

			IgnorePropertiesIfReadClass dest = MapD.Copy<IgnorePropertiesIfReadClass, IgnorePropertiesIfReadClass>( new IgnorePropertiesIfReadClass() );
			// It worked

		}

		[Test]
		public void IgnoreIf_WriteAndRead_Succeeds() {

			MapD.Config.IgnorePropertiesIf = PropertyIs.WriteOnly | PropertyIs.ReadOnly;
			MapD.Config.CreateMap<IgnorePropertiesIfWriteClass, IgnorePropertiesIfWriteClass>();

			IgnorePropertiesIfWriteClass dest = MapD.Copy<IgnorePropertiesIfWriteClass, IgnorePropertiesIfWriteClass>( new IgnorePropertiesIfWriteClass() );
			// It worked
		}

		[Test]
		public void IgnoreIf_ReadAndWrite_Succeeds() {

			MapD.Config.IgnorePropertiesIf = PropertyIs.ReadOnly | PropertyIs.WriteOnly;
			MapD.Config.CreateMap<IgnorePropertiesIfReadClass, IgnorePropertiesIfReadClass>();

			IgnorePropertiesIfReadClass dest = MapD.Copy<IgnorePropertiesIfReadClass, IgnorePropertiesIfReadClass>( new IgnorePropertiesIfReadClass() );
			// It worked

		}

		private class IgnorePropertiesIfReadClass {
			public int Property1 { set { } }
		}
		private class IgnorePropertiesIfWriteClass {
			public int Property1 { get { return 0; } }
		}

		[Test]
		public void IgnoreIf_Write_OnSourceClass_Succeeds() {

			MapD.Config.CreateMap<IgnorePropertiesIfWriteOnClass, IgnorePropertiesIfReadOnClass>();

			IgnorePropertiesIfReadOnClass dest = MapD.Copy<IgnorePropertiesIfWriteOnClass, IgnorePropertiesIfReadOnClass>( new IgnorePropertiesIfWriteOnClass() );
			// It worked
		}

		[Test]
		public void IgnoreIf_Read_OnSourceClass_Succeeds() {

			MapD.Config.CreateMap<IgnorePropertiesIfReadOnClass, IgnorePropertiesIfWriteOnClass>();

			IgnorePropertiesIfWriteOnClass dest = MapD.Copy<IgnorePropertiesIfReadOnClass, IgnorePropertiesIfWriteOnClass>( new IgnorePropertiesIfReadOnClass() );
			// It worked

		}

		[Test]
		public void IgnoreIf_Write_OnDestinationClass_Succeeds() {

			MapD.Config.CreateMap<IgnorePropertiesIfReadOnClass, IgnorePropertiesIfWriteOnClass>();

			IgnorePropertiesIfWriteOnClass dest = MapD.Copy<IgnorePropertiesIfReadOnClass, IgnorePropertiesIfWriteOnClass>( new IgnorePropertiesIfReadOnClass() );
			// It worked
		}

		[Test]
		public void IgnoreIf_Read_OnDestinationClass_Succeeds() {

			MapD.Config.CreateMap<IgnorePropertiesIfWriteOnClass, IgnorePropertiesIfReadOnClass>();

			IgnorePropertiesIfReadOnClass dest = MapD.Copy<IgnorePropertiesIfWriteOnClass, IgnorePropertiesIfReadOnClass>( new IgnorePropertiesIfWriteOnClass() );
			// It worked

		}

		[IgnorePropertiesIf( PropertyIs.WriteOnly )]
		private class IgnorePropertiesIfReadOnClass {
			public int Property1 { set { } }
		}
		[IgnorePropertiesIf( PropertyIs.ReadOnly )]
		private class IgnorePropertiesIfWriteOnClass {
			public int Property1 { get { return 0; } }
		}

	}

}
