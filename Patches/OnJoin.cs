using System;
using System.Collections.Generic;
using System.Text;
using Photon.Realtime;
using GorillaNetworking;
using HarmonyLib;
using Mono;
using UnityEngine;
using GTAG_NotificationLib;
using Genesis.UI;
using Photon.Pun;
using System.Runtime.CompilerServices;
using ShibaGTGenesis.Backend;
using Genesis.Utilities;

namespace Genesis.Backend
{

    [HarmonyPatch(typeof(MonkeAgent))]
    [HarmonyPatch("OnPlayerEnteredRoom", MethodType.Normal)]
    internal class OnJoin : HarmonyPatch
    {
        private static void Prefix(NetPlayer newPlayer)
        {
            NotifiLib.SendNotification("<color=blue>[ROOM]:</color> Player " + newPlayer.NickName + " Joined Lobby");
            Back.genesisUsersinGame.Clear();
            foreach (Player p in PhotonNetwork.PlayerListOthers)
            {
                if (p.CustomProperties.ToString().Contains("genesis"))
                    Back.genesisUsersinGame.Add(p, null);
            }
        }
    }

    [HarmonyPatch(typeof(MonkeAgent))]
    [HarmonyPatch("OnPlayerLeftRoom", MethodType.Normal)]
    internal class OnLeave : HarmonyPatch
    {
        private static void Prefix(NetPlayer otherPlayer)
        {
            if (otherPlayer != NetworkSystem.Instance.LocalPlayer)
            {
                NotifiLib.SendNotification("<color=blue>[ROOM]:</color> Player " + otherPlayer.NickName + " Left Lobby");
                Back.genesisUsersinGame.Clear();
                foreach (Player p in PhotonNetwork.PlayerListOthers)
                {
                    if (p.CustomProperties.ToString().Contains("genesis"))
                        Back.genesisUsersinGame.Add(p, null);
                }
                if (PhotonNetwork.IsMasterClient)
                {
                        NotifiLib.SendNotification("<color=yellow>[ROOM]: YOU ARE NOW MASTER CLIENT!</color>");
                }
            }
        }
    }
}