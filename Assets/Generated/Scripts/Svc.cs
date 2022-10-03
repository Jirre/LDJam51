using UnityEngine;

namespace JvLib.Services
{
    public static class Svc
    {
        public static class Ref
        {
            public static ServiceReference<Project.Audio.AudioServiceManager> Audio
                 = new ServiceReference<Project.Audio.AudioServiceManager>();
            public static ServiceReference<Project.Gameplay.GameplayServiceManager> Gameplay
                 = new ServiceReference<Project.Gameplay.GameplayServiceManager>();
            public static ServiceReference<JvLib.Pooling.Objects.ObjectPoolServiceManager> ObjectPools
                 = new ServiceReference<JvLib.Pooling.Objects.ObjectPoolServiceManager>();
            public static ServiceReference<JvLib.Pooling.Particles.ParticlePoolServiceManager> ParticlePools
                 = new ServiceReference<JvLib.Pooling.Particles.ParticlePoolServiceManager>();
            public static ServiceReference<Project.UI.UIServiceManager> UI
                 = new ServiceReference<Project.UI.UIServiceManager>();
            public static ServiceReference<Project.Generation.WorldServiceManager> World
                 = new ServiceReference<Project.Generation.WorldServiceManager>();
        }

        public static Project.Audio.AudioServiceManager Audio
        {
            get
            {
                return Ref.Audio.Reference;
            }
        }
        public static Project.Gameplay.GameplayServiceManager Gameplay
        {
            get
            {
                return Ref.Gameplay.Reference;
            }
        }
        public static JvLib.Pooling.Objects.ObjectPoolServiceManager ObjectPools
        {
            get
            {
                return Ref.ObjectPools.Reference;
            }
        }
        public static JvLib.Pooling.Particles.ParticlePoolServiceManager ParticlePools
        {
            get
            {
                return Ref.ParticlePools.Reference;
            }
        }
        public static Project.UI.UIServiceManager UI
        {
            get
            {
                return Ref.UI.Reference;
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
            Ref.Audio = new ServiceReference<Project.Audio.AudioServiceManager>();
            Ref.Gameplay = new ServiceReference<Project.Gameplay.GameplayServiceManager>();
            Ref.ObjectPools = new ServiceReference<JvLib.Pooling.Objects.ObjectPoolServiceManager>();
            Ref.ParticlePools = new ServiceReference<JvLib.Pooling.Particles.ParticlePoolServiceManager>();
            Ref.UI = new ServiceReference<Project.UI.UIServiceManager>();
            Ref.World = new ServiceReference<Project.Generation.WorldServiceManager>();
        }
    }
}

