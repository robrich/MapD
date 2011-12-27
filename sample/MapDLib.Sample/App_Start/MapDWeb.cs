[assembly: WebActivator.PreApplicationStartMethod(typeof(MapDLib.Sample.App_Start.MapDWeb), "Start")]

namespace MapDLib.Sample.App_Start {
	using MapDLib;

	public static class MapDWeb {

		public static void Start() {
			MapD.Config.CreateMapsFromAllAssembliesInPath();
		}

	}
}