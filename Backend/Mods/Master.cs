using ExitGames.Client.Photon;
using Genesis.Backend;
using Genesis.UI;
using Genesis.Utilities;
using GorillaGameModes;
using GTAG_NotificationLib;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ShibaGTGenesis.Backend
{
    internal class Master : MonoBehaviour
    {

        public static float matspamdelay;

        public static float sounddelay;
        public static bool soundbool;
        public static int soundint;

        public static void SoundSpammer()
        {
            if (WristMenu.gripDownL)
            {
                if (!soundbool)
                {
                    soundint--;
                    if (soundint <= 0)
                    { soundint = 0; }
                    NotifiLib.SendNotification("set sound id to " + soundint);
                    soundbool = true;
                }
            }
            if (WristMenu.gripDownR)
            {
                if (!soundbool)
                {
                    soundint++;
                    if (soundint >= 12)
                    { soundint = 12; }
                    NotifiLib.SendNotification("set sound id to " + soundint);
                    soundbool = true;
                }
            }
            if (!WristMenu.gripDownL && !WristMenu.gripDownR)
            {
                soundbool = false;
            }

            if (WristMenu.triggerDownL || WristMenu.triggerDownR)
            {
                if (sounddelay < Time.time)
                {
                    sounddelay = Time.time + 0.1f;
                    SoundSpam(soundint);
                }
            }
        }

        public static void Untagself()
        {
            GorillaTagManager.instance.gameObject.GetComponent<GorillaTagManager>().currentInfected.Remove(PhotonNetwork.LocalPlayer);
        }

        public static async void InvisAll()
        {
            ChangeGamemode(4, true);
            var e = GorillaTagManager.instance.GetComponent<GorillaTagManager>();
            e.currentInfected.Add(NetworkSystem.Instance.LocalPlayer);
            await Task.Delay(500);
            foreach (NetPlayer p in NetworkSystem.Instance.PlayerListOthers)
            {
                if (e.currentInfected.Contains(p))
                {
                    e.currentInfected.Remove(p);
                }
                e.currentInfected.Add(p);
            }
            Untagself();
        }

        private static object[] sendSoundDataOther = new object[4];
        private static readonly object[] sendEventData = new object[3];

        public static readonly NetEventOptions neoAll = new NetEventOptions
        {
            Reciever = NetEventOptions.RecieverTarget.all
        };

        public static void SoundSpam(int index)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                sendSoundDataOther[0] = index;
                sendSoundDataOther[1] = 9999f;
                sendSoundDataOther[2] = false;
                sendSoundDataOther[3] = PhotonNetwork.LocalPlayer.ActorNumber;
                object obj = sendSoundDataOther;

                sendEventData[0] = NetworkSystem.Instance.ServerTimestamp;
                sendEventData[1] = (byte)3;
                sendEventData[2] = obj;
                NetworkSystemRaiseEvent.RaiseEvent(3, sendEventData, neoAll, true);

                OP.NewFlusher();
            }
        }

        public static async void SkeletonAll()
        {
            ChangeGamemode(6, true);
            var e = GorillaTagManager.instance.GetComponent<GorillaTagManager>();
            e.currentInfected.Add(NetworkSystem.Instance.LocalPlayer);
            await Task.Delay(500);
            foreach (NetPlayer p in NetworkSystem.Instance.PlayerListOthers)
            {
                if (e.currentInfected.Contains(p))
                {
                    e.currentInfected.Remove(p);
                }
                e.currentInfected.Add(p);
            }
            Untagself();
        }

        public static void ChangeGamemode(int gamemodeName, bool invis = false)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameModePatch.enabled = true;

                if (gamemodeName == 5)
                    gamemodeName = 6;

                Dictionary<int, string> gamemodeDict = new Dictionary<int, string>();
                gamemodeDict.Add(0, "Casual");
                gamemodeDict.Add(1, "Infection");
                gamemodeDict.Add(2, "Hunt");
                gamemodeDict.Add(3, "Paintbrawl");
                gamemodeDict.Add(4, "Ambush");
                gamemodeDict.Add(5, "Freeze Tag");
                gamemodeDict.Add(6, "Ghost");
                gamemodeDict.Add(7, "Guardian");

                Hashtable hash = new Hashtable();

                string gameModeKey = gamemodeDict[gamemodeName];

                UnityEngine.Debug.Log(gameModeKey);

                hash.Add("gameMode", "forestDEFAULT" + gameModeKey);

                GorillaGameManager.instance.checkCooldown = 0f;
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

                NetworkView netView = (NetworkView)Traverse.Create(typeof(GameMode)).Field("activeNetworkHandler").Field("netView").GetValue();
                NetworkSystem.Instance.NetDestroy(netView.gameObject);
                GorillaGameManager ggs = (GorillaGameManager)Traverse.Create(typeof(GameMode)).Field("activeGameMode").GetValue();

                Traverse.Create(typeof(GameMode)).Field("activeGameMode").SetValue(null);
                Traverse.Create(typeof(GameMode)).Field("activeNetworkHandler").SetValue(null);

                //GameMode.ResetGameModes();
                GameMode.LoadGameMode(gamemodeName);
            }

            OP.NewFlusher();
        }
        static float gmspamdelay;

        public static void GMSpam()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (gmspamdelay < Time.time)
                {
                    gmspamdelay = Time.time + 1f;

                    int randomint = UnityEngine.Random.Range(0, 8);
                    ChangeGamemode(randomint);
                    foreach (NetPlayer p in NetworkSystem.Instance.PlayerListOthers)
                        GorillaTagManager.instance.GetComponent<GorillaTagManager>().currentInfected.Add(p);
                }
            }
        }

        public static void MatSpamAll()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (Time.time > matspamdelay)
                {
                    matspamdelay = Time.time + 0.1f;
                    var e = GorillaTagManager.instance.GetComponent<GorillaTagManager>();
                    foreach (var p in NetworkSystem.Instance.PlayerListOthers)
                    {
                        if (e.currentInfected.Contains(p))
                        {
                            e.currentInfected.Remove(p);
                        }
                        else
                        {
                            e.AddInfectedPlayer(p, false);
                            e.currentInfected.Add(p);
                        }
                    }
                    OP.NewFlusher();
                }
            }
        }
        public static void MatSpamGun()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var data = GunLib.ShootLock();
                if (data == null)
                    return;
                if (data.isShooting && data.isTriggered)
                {
                    if (Time.time > matspamdelay)
                    {
                        matspamdelay = Time.time + 0.1f;
                        Player p = RigShit.GetPlayerFromRig(data.lockedPlayer);
                        var e = GorillaTagManager.instance.GetComponent<GorillaTagManager>();
                        if (e.currentInfected.Contains(p))
                        {
                            e.currentInfected.Remove(p);
                        }
                        else
                        {
                            e.currentInfected.Add(p);
                        }
                        OP.NewFlusher();
                    }
                }
            }
        }

        public static void UntagAura()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var mgr = GorillaGameManager.instance as GorillaTagManager;
            if (mgr == null) return;
            float threshold = mgr.tagDistanceThreshold - 0.5f;
            Vector3 me = GorillaTagger.Instance.offlineVRRig.transform.position;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig == null || rig.isOfflineVRRig) continue;
                var plr = rig.Creator;
                if (plr == null) continue;
                if (Vector3.Distance(rig.transform.position, me) < threshold)
                {
                    if (mgr.isCurrentlyTag && mgr.currentIt == plr)
                        mgr.currentIt = null;
                    else if (!mgr.isCurrentlyTag && mgr.currentInfected.Contains(plr))
                        mgr.currentInfected.Remove(plr);
                }
            }
        }

        static GreyZoneManager cachedGreyZone;
        static GreyZoneManager GetGreyZone()
        {
            if (cachedGreyZone != null) return cachedGreyZone;
            var go = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/GreyZoneManager");
            cachedGreyZone = go != null ? go.GetComponent<GreyZoneManager>() : null;
            return cachedGreyZone;
        }

        static float flashdelay;
        static bool flashFlip;
        public static void FlashScreenAll()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var gz = GetGreyZone();
            if (gz == null) return;
            if (Time.time > flashdelay + 0.5f)
            {
                flashdelay = Time.time;
                flashFlip = !flashFlip;
                if (flashFlip)
                    gz.ActivateGreyZoneAuthority();
                else
                    gz.DeactivateGreyZoneAuthority();
                PhotonNetwork.RunViewUpdate();
            }
        }

        public static void ActivateGreyZone()
        {
            var gz = GetGreyZone();
            if (PhotonNetwork.IsMasterClient && gz != null)
                gz.ActivateGreyZoneAuthority();
        }

        public static void DeactivateGreyZone()
        {
            var gz = GetGreyZone();
            if (PhotonNetwork.IsMasterClient && gz != null)
                gz.DeactivateGreyZoneAuthority();
        }

        static float lavaSpazDelay;
        static bool lavaSpazFlip;
        static InfectionLavaController CachedLavaController()
        {
            var go = GameObject.Find("Rewind_2024-02_Forest/VIMForestLava (prefab)/ILavaYou_PrefabV/ForestLavaController");
            return go != null ? go.GetComponent<InfectionLavaController>() : null;
        }
        public static void SpazLava()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (Time.time > lavaSpazDelay + 0.3f)
            {
                lavaSpazDelay = Time.time;
                lavaSpazFlip = !lavaSpazFlip;
                var ctrl = CachedLavaController();
                if (ctrl != null)
                    ctrl.JumpToState(lavaSpazFlip ? InfectionLavaController.RisingLavaState.Full : InfectionLavaController.RisingLavaState.Drained);
            }
        }

        public static void LavaSwimGun()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var data = GunLib.ShootLock();
            if (data == null) return;
            if (data.isShooting && data.isTriggered && data.isLocked)
            {
                var ctrl = CachedLavaController();
                if (ctrl == null) return;
                ctrl.reliableState.stateStartTime += 500;
                RoomSystem.SendLavaSyncToPlayer((byte)ctrl.zone, (byte)InfectionLavaController.RisingLavaState.Full, ctrl.reliableState.stateStartTime, ctrl.reliableState.activationProgress, ctrl.lavaActivationVoteCount, ctrl.lavaActivationVotePlayerIds, data.lockedPlayer.Creator);
            }
        }

        public static void LavaSwimAll()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var ctrl = CachedLavaController();
            if (ctrl == null) return;
            ctrl.reliableState.stateStartTime += 500;
            RoomSystem.SendLavaSync((byte)ctrl.zone, (byte)InfectionLavaController.RisingLavaState.Full, ctrl.reliableState.stateStartTime, ctrl.reliableState.activationProgress, ctrl.lavaActivationVoteCount, ctrl.lavaActivationVotePlayerIds);
        }

        public static void DestroyPvGun()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var data = GunLib.Shoot();
            if (data == null) return;
            if (data.isShooting && data.isTriggered)
            {
                if (data.RaycastHit.collider == null) return;
                var pv = data.RaycastHit.collider.GetComponentInParent<PhotonView>();
                if (pv != null)
                {
                    var hash = new Hashtable();
                    hash[(byte)0] = pv.ViewID;
                    PhotonNetwork.RaiseEventInternal(204, hash, new RaiseEventOptions { Receivers = ReceiverGroup.All }, ExitGames.Client.Photon.SendOptions.SendReliable);
                }
            }
        }
    }
}