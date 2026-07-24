using GorillaExtensions;
using HarmonyLib;
using ShibaGTGenesis.Backend;
using UnityEngine;

namespace Genesis.Backend
{
    [HarmonyPatch(typeof(RequestableOwnershipGuard), "OwnershipRequested")]
    internal class AntiKick : MonoBehaviour
    {
        public static bool enabled;
        private static bool Prefix(string nonce)
        {
            if (enabled)
            {
                if (nonce.Length > 1000)
                {
                    return false;
                }
                return true;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(VRRig), "DroppedByPlayer")]
    internal class AntiCrash : MonoBehaviour
    {
        private static bool Prefix(VRRig __instance, VRRig grabbedByRig, Vector3 throwVelocity)
        {
            if (World.anticrash)
            {
                if (__instance == GorillaTagger.Instance.offlineVRRig)
                {
                    if (!throwVelocity.IsValid(10000))
                        return false;
                    else
                        return true;
                }
                else
                    return true;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(VRRig), "GrabbedByPlayer")]
    internal class AntiGrab : MonoBehaviour
    {
        private static bool Prefix(VRRig __instance, VRRig grabbedByRig, bool grabbedBody, bool grabbedLeftHand, bool grabbedWithLeftHand)
        {
            if (World.antigrab)
            {
                if (__instance == GorillaTagger.Instance.offlineVRRig)
                    return false;
                else
                    return true;
            }
            return true;
        }
    }
}
