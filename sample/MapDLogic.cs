namespace MapDLib.Sample.HowItWorks {
	using System;
	using System.Reflection;

	// This is roughly how MapD works

	public static class MapDLogic {

		public static void Copy<T>( T Source, ref T Destination ) where T : class {

			if ( Source == null ) {
				throw new ArgumentNullException( "Source" );
			}
			if ( Destination == null ) {
				Destination = (T)Activator.CreateInstance( typeof(T) );
			}

			PropertyInfo[] properties = typeof(T).GetProperties();

			foreach ( PropertyInfo property in properties ) {

				if ( !property.CanRead || !property.CanWrite ) {
					continue; // Can't read or write this one
				}

				object sourceval = property.GetValue( Source, null );
				object destval = property.GetValue( Destination, null );

				if ( ( sourceval == null ) != ( destval == null ) || ( sourceval != null && !sourceval.Equals( destval ) ) ) {
					// It changed
					property.SetValue( Destination, sourceval, null );
				}

			}

		}

	}
}