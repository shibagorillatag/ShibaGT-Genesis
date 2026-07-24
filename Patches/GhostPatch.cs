using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.Threading;
using System.Net;
using Photon.Pun;
using System.Linq;
using GorillaLocomotion.Gameplay;

namespace Genesis.Backend
{
    [HarmonyPatch(typeof(VRRig), "OnDisable")]
    internal class GhostPatch : MonoBehaviour
    {
        public static bool Prefix(VRRig __instance)
        {
            return !(__instance == GorillaTagger.Instance.offlineVRRig);
        }
    }

    [HarmonyPatch(typeof(VRRigJobManager), "DeregisterVRRig")]
    internal class GhostPatch2 : MonoBehaviour
    {
        public static bool Prefix(VRRig rig)
        {
            return !rig.isOfflineVRRig;
        }
    }

    [HarmonyPatch(typeof(BalloonHoldable), "OwnerPopBalloon")]
    internal class BalloonPopPatch : MonoBehaviour
    {
        public static bool Prefix(BalloonHoldable __instance)
        {
            return __instance.ownerRig != GorillaTagger.Instance.offlineVRRig;
        }
    }
}
