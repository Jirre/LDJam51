using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEditorInternal;

namespace System.Reflection
{
    public static class AppDomainExtensions
    {
        private static readonly MethodInfo isUnityExtensionsInitializedMethod
            = typeof(InternalEditorUtility).GetMethod("IsUnityExtensionsInitialized",
                BindingFlags.Static | BindingFlags.NonPublic);

        private static IList<Type> cachedPlayerTypes;
        private static IList<Type> cachedEditorTypes;

        public static IList<Type> GetAllTypes(this AppDomain appDomain, AssembliesType assembliesType,
            bool mayUseCache = true)
        {
            if (mayUseCache && assembliesType == AssembliesType.Player && cachedPlayerTypes != null)
                return cachedPlayerTypes;

            if (mayUseCache && assembliesType == AssembliesType.Editor && cachedEditorTypes != null)
                return cachedEditorTypes;

            if (!IsUnityExtensionsInitialized())
                return null;

            UnityEditor.Compilation.Assembly[] unityAssemblies = CompilationPipeline.GetAssemblies(assembliesType);

            // Set a "high" pre allocated memory to reduce amount of reallocation.
            List<Type> result = new List<Type>(6000);
            Assembly[] assemblies = appDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];

                bool isUnityAssembly = false;
                for (int j = 0; j < unityAssemblies.Length; j++)
                {
                    UnityEditor.Compilation.Assembly unityAssembly = unityAssemblies[j];
                    AssemblyName name = assembly.GetName();
                    if (unityAssembly.name == name.Name)
                    {
                        isUnityAssembly = true;
                        break;
                    }
                }

                if (isUnityAssembly)
                    result.AddRange(assembly.GetLoadableTypes());
            }

            switch (assembliesType)
            {
                case AssembliesType.Editor:
                    cachedEditorTypes = result.AsReadOnly();
                    return cachedEditorTypes;
                case AssembliesType.Player:
                    cachedPlayerTypes = result.AsReadOnly();
                    return cachedPlayerTypes;
            }

            return result.AsReadOnly();
        }

        private static bool IsUnityExtensionsInitialized()
        {
            if (isUnityExtensionsInitializedMethod == null)
                return true;

            return (bool)isUnityExtensionsInitializedMethod.Invoke(null, null);
        }
    }
}
