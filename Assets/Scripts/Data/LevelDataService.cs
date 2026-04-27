using UnityEngine;

namespace SnowmanCount.Data
{
    public static class LevelDataService
    {
        private static ILevelDataProvider provider;

        public static void Initialize(ILevelDataProvider dataProvider)
        {
            provider = dataProvider;
            Debug.Log($"[LevelDataService] Initialized with {dataProvider.GetType().Name}");
        }

        public static ILevelDataProvider Provider
        {
            get
            {
                if (provider == null)
                {
                    Debug.LogWarning("[LevelDataService] No provider registered. Using default NPOI provider.");
                    provider = new NpoiLevelDataProvider();
                }
                return provider;
            }
        }
    }
}
