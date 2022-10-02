using UnityEngine;

namespace JvLib.Services
{
    public static class Svc
    {
        public static class Ref
        {
            public static ServiceReference<Project.Gameplay.GameplayServiceManager> GameplayServiceManager
                 = new ServiceReference<Project.Gameplay.GameplayServiceManager>();
            public static ServiceReference<Project.Generation.WorldServiceManager> World
                 = new ServiceReference<Project.Generation.WorldServiceManager>();
        }

        public static Project.Gameplay.GameplayServiceManager GameplayServiceManager
        {
            get
            {
                return Ref.GameplayServiceManager.Reference;
            }
        }
        public static Project.Generation.WorldServiceManager World
        {
            get
            {
                return Ref.World.Reference;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void ClearCache()
        {
            Ref.GameplayServiceManager = new ServiceReference<Project.Gameplay.GameplayServiceManager>();
            Ref.World = new ServiceReference<Project.Generation.WorldServiceManager>();
        }
    }
}

