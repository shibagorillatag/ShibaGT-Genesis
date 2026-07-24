using GTAG_NotificationLib;
using HarmonyLib;
using Photon.Pun;
using PlayFab;
using ShibaGTGenesis.Backend;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Genesis.Backend
{
    [HarmonyPatch(typeof(GorillaGameManager), "ValidGameMode")]
    public class GameModePatch
    {
        public static bool enabled = false;

        public static bool Prefix(ref bool __result)
        {
            if (enabled)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(MonkeAgent), "SendReport")]
    internal class anticheatnotif : MonoBehaviour
    {
        private static bool Prefix(string susReason, string susId, string susNick)
        {
            if (susId == PhotonNetwork.LocalPlayer.UserId)
            {
                if (!susReason.ToLower().Contains("empty"))
                    NotifiLib.SendNotification("<color=red>[ANTICHEAT] REPORTED FOR: " + susReason + "</color>");
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(MonkeAgent), "IncrementRPCCallLocal")]
    internal class rpcdebug : MonoBehaviour
    {
        private static bool Prefix(PhotonMessageInfoWrapped infoWrapped, string rpcFunction)
        {
            if (infoWrapped.senderID == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                if (Settings.rpcdebugging)
                    NotifiLib.SendNotification("<color=red>[RPC]</color> RPC called by client: " + rpcFunction);
            }
            return false;
        }
    }
}
