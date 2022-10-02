using UnityEngine;

namespace JvLib.Services
{
    public static class Svc
    {
        public static class Ref
        {
            public static ServiceReference<Project.Gameplay.GameplayServiceManager> Gameplay
                 = new ServiceReference<Project.Gameplay.GameplayServiceManager>();
            public static ServiceReference<Project.Generation.WorldServiceManager> World
                 = new ServiceReference<Project.Generation.WorldServiceManager>();
        }

        public static Project.Gameplay.GameplayServiceManager Gameplay
        {
            get
            {
                return Ref.Gameplay.Reference;
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
            Ref.Gameplay = new ServiceReference<Project.Gameplay.GameplayServiceManager>();
            Ref.World = new ServiceReference<Project.Generation.WorldServiceManager>();
        }
    }
}

