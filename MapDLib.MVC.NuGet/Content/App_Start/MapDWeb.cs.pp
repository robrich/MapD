[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.MapDWeb), "Start")]

namespace $rootnamespace$.App_Start {
	using MapDLib;

	public static class MapDWeb {

		public static void Start() {
			MapD.Config.CreateMapsFromAllAssembliesInPath();
		}

	}
}