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
using ShibaGTGenesis.Backend;
using GorillaNetworking;

namespace Genesis.Backend
{
    [HarmonyPatch(typeof(GorillaQuitBox), "OnBoxTriggered")]
    internal class QuitboxPatch : MonoBehaviour
    {
        public static bool Prefix()
        {
            if (World.disableQuitbox)
                return false;

            if (World.QuitBoxMOD)
            {
                return false;
            }

            return true;
        }
    }
}
