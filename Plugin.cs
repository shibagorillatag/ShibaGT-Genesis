using BepInEx;
using Genesis.Backend;
using Genesis.UI;
using HarmonyLib;
using Loading;
using System;
using System.Net;
using System.Net.Sockets;
using Genesis.Utilities;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;
using ShibaGTGenesis.Backend;
using ShibaGTGenesis.UI;
using Photon.Pun;

namespace Genesis
{
    [BepInPlugin(Name, GUID, Version)]
    public class Plugin : BaseUnityPlugin
    {
        public const string Name = "genesis";
        public const string GUID = "org.shibagt.genesis";
        public const string Version = "1.0";

        private static GameObject Gameobject;

        void Awake()
        {
            Harmony harmony = new Harmony(GUID);
            harmony.PatchAll();
            Loader.loaded = true;
        }
    }
    [HarmonyPatch(typeof(GorillaLocomotion.GTPlayer), "FixedUpdate")]
    public class UpdatePatch
    {
        private static bool alreadyInit;
        private static bool isPatchApplied = false;
        public static GameObject Gameobject;
        public static Harmony harm;

        public static void Postfix()
        {
            
            if (!alreadyInit)
            {
                alreadyInit = true;
                Gameobject = new GameObject();
                Gameobject.AddComponent<Plugin>();
                Gameobject.AddComponent<WristMenu>();
                Gameobject.AddComponent<RigShit>();
                Gameobject.AddComponent<GhostPatch>();
                Gameobject.AddComponent<GTAG_NotificationLib.NotifiLib>();
                Gameobject.AddComponent<LegacyGUI>();
                Gameobject.AddComponent<NewGUI>();
                Gameobject.AddComponent<NotifLibGUI>();
                Gameobject.AddComponent<VoiceManager>();
                Gameobject.AddComponent<CoroutineRunner>();
                Back.Load();
                Back.LoadFavorites();
                Object.DontDestroyOnLoad(Gameobject);
            }
        }
    }
}
