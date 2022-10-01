using UnityEngine;

namespace JvLib.Services
{
    public static class Svc
    {
        public static class Ref
        {
            public static ServiceReference<Project.Generation.WorldGeneratorServiceManager> Generator
                 = new ServiceReference<Project.Generation.WorldGeneratorServiceManager>();
        }

        public static Project.Generation.WorldGeneratorServiceManager Generator
        {
            get
            {
                return Ref.Generator.Reference;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void ClearCache()
        {
            Ref.Generator = new ServiceReference<Project.Generation.WorldGeneratorServiceManager>();
        }
    }
}

