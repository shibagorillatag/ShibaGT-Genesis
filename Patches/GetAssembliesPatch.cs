using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShibaGTGenesis.Backend
{
    [HarmonyPatch]
    internal class GetAssembliesPatch
    {
        private static Assembly[] assemblies = null;
        private static bool isInsidePatch = false;

        static MethodBase TargetMethod()
        {
            return typeof(AppDomain).GetMethod("GetAssemblies", BindingFlags.Public | BindingFlags.Instance);
        }

        static bool Prefix(ref Assembly[] __result)
        {
            if (isInsidePatch)
                return true;

            isInsidePatch = true;

            try
            {
                if (assemblies == null)
                {
                    assemblies = AppDomain.CurrentDomain.GetAssemblies();
                }

                Assembly currentAssembly = Assembly.GetExecutingAssembly();
                __result = assemblies.Where(assembly => assembly != currentAssembly).ToArray();

                return false;
            }
            finally
            {
                isInsidePatch = false;
            }
        }
    }
}
