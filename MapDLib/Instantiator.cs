namespace MapDLib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	#endregion

	public static class Instantiator {

		public static object CreateInstance( Type Type ) {
			object instance = null;
			if ( Type.IsClassType() && !Type.IsListOfT() ) {
				// Class types are instantiated via the DI strategy provided
				try {
					instance = MapD.Config.CreationStrategy( Type );
				} catch ( Exception ex ) {
					throw new InstantiationException( "Error creating a " + Type.Name, Type, ex );
				}
				if ( instance == null ) {
					// TODO: If it's a concrete type with a no-arg constructor, use Activator.CreateInstance?
					throw new InstantiationException( "Instantiation of " + Type.Name + " produced null", Type );
				}
			} else {
				// Value types and lists are instantiated the normal way
				instance = Activator.CreateInstance( Type );
			}
			return instance;
		}

	}

}
