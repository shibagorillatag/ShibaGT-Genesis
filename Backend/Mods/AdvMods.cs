using ExitGames.Client.Photon;
using Genesis.UI;
using GorillaNetworking;
using GTAG_NotificationLib;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using Genesis.Utilities;
using Valve.VR;
using PlayFab.ClientModels;
using System.Text.RegularExpressions;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.Animations.Rigging;
using GorillaTagScripts;
using UnityEngine.XR.Interaction.Toolkit;
using System.Reflection;

namespace ShibaGTGenesis.Backend
{
    internal class AdvMods : MonoBehaviour
    {
        static float Killfloat;

        public static void KillAll()
        {
            if (Killfloat < Time.time)
            {
                Killfloat = Time.time + 3.5f;
                foreach (VRRig rig in VRRigCache.ActiveRigs)
                {
                    if (rig.mainSkin.material.name.Contains("orangealive") && GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.Contains("blue") || rig.mainSkin.material.name.Contains("bluealive") && GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.Contains("orange"))
                    {
                        GameObject gameObject = ObjectPools.instance.Instantiate(-1674517839);
                        SlingshotProjectile component = gameObject.GetComponent<SlingshotProjectile>();
                        Color throwableProjectileColor = GorillaTagger.Instance.offlineVRRig.GetThrowableProjectileColor(false);
                        component.Launch(rig.transform.position, Vector3.zero, PhotonNetwork.LocalPlayer, false, false, 0, 0.5f, false, throwableProjectileColor);
                    }
                }
            }
        }

        public static void KillGun()
        {
            var data = GunLib.ShootLock();
            if (data != null)
            {
                if (data.isShooting && data.isTriggered)
                {
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        if (Killfloat < Time.time)
                        {
                            Killfloat = Time.time + 1f;
                            if (data.lockedPlayer != null)
                            {
                                GameObject gameObject = ObjectPools.instance.Instantiate(-1674517839);
                                SlingshotProjectile component = gameObject.GetComponent<SlingshotProjectile>();
                                Color throwableProjectileColor = GorillaTagger.Instance.offlineVRRig.GetThrowableProjectileColor(false);
                                component.Launch(data.lockedPlayer.transform.position, Vector3.zero, PhotonNetwork.LocalPlayer, false, false, 0, 0.5f, false, throwableProjectileColor);
                            }
                        }
                    }
                    else
                    {
                        GorillaPaintbrawlManager gorillaBattleManager = GameObject.Find("GT Systems/GameModeSystem/Gorilla Battle Manager").GetComponent<GorillaPaintbrawlManager>();
                        if (gorillaBattleManager.playerLives[RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber] > 0)
                        {
                            gorillaBattleManager.playerLives[RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber] = gorillaBattleManager.playerLives[RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber] - 1;
                        }
                    }
                }

                if (!data.isTriggered)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static bool IsInfected(VRRig rig)
        {
            return rig.mainSkin.name.ToLower().Contains("fect");
        }

        public static void InstantTag(VRRig Player)
        {
            if (Player == GorillaTagger.Instance.offlineVRRig) return;

            if (!IsInfected(Player))
            {
                typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = Player.transform.position;
                typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
                GorillaGameModes.GameMode.ReportTag(Player.Creator);
            }

            OP.NewFlusher();
        }

        public static void TagGun()
        {

            var data = GunLib.ShootLock();
            if (data != null)
            {
                if (data.isShooting && data.isTriggered)
                {
                    /*
                    if (!data.lockedPlayer.mainSkin.material.name.Contains("fected"))
                    {
                        GorillaTagger.Instance.offlineVRRig.enabled = false;
                        GorillaTagger.Instance.offlineVRRig.transform.position = data.lockedPlayer.transform.position;
                        GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.transform.position = data.lockedPlayer.transform.position;
                        GorillaGameModes.GameMode.ReportTag(RigShit.GetNetPlayerFromRig(data.lockedPlayer));
                    }
                    else
                    {
                        data.isTriggered = false; data.isShooting = false; data.lockedPlayer = null; data.isLocked = false;
                        GunLib.GunCleanUp();
                        GorillaTagger.Instance.offlineVRRig.enabled = true;
                    }
                    */

                    InstantTag(data.lockedPlayer);
                }

                if (!data.isTriggered)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void HuntTagGun()
        {
            var data = GunLib.ShootLock();
            if (data != null)
            {
                if (data.isShooting && data.isTriggered)
                {
                    GorillaGameModes.GameMode.ReportTag(RigShit.GetNetPlayerFromRig(data.lockedPlayer));
                }

                if (!data.isTriggered)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void AmbushAll()
        {
            if (!GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.ToLower().Contains("stealth"))
            {
                NotifiLib.SendNotification("<color=blue>[GENESIS]</color> Get ambushed!");
                Back.GetButton("Ambush All").enabled = false;
                return;
            }
            else
            {
                bool EveryoneTagged = false;
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (!vrrig.mainSkin.material.name.ToLower().Contains("stealth"))
                    {
                        EveryoneTagged = true;
                        break;
                    }
                }
                if (EveryoneTagged)
                {
                    foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                    {
                        if (!vrrig.mainSkin.material.name.ToLower().Contains("stealth"))
                        {
                            GorillaTagger.Instance.offlineVRRig.enabled = false;
                            GorillaTagger.Instance.offlineVRRig.transform.position = vrrig.transform.position;
                            GorillaTagger.Instance.myVRRig.transform.position = vrrig.transform.position;

                            Vector3 you = GorillaTagger.Instance.offlineVRRig.transform.position;
                            Vector3 rig = vrrig.transform.position;
                            float distance = Vector3.Distance(rig, you);

                            if (GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.ToLower().Contains("stealth") && !vrrig.mainSkin.material.name.ToLower().Contains("stealth") && distance < 1.667)
                            {
                                GorillaGameModes.GameMode.ReportTag(RigShit.GetNetPlayerFromRig(vrrig));
                            }
                        }
                    }
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                    Back.GetButton("Ambush All").enabled = false;
                }
            }
        }

        public static void AmbushGun()
        {
            var data = GunLib.ShootLock();
            if (data != null)
            {
                if (data.isShooting && data.isTriggered)
                {
                    if (!GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.ToLower().Contains("stealth"))
                    {
                        NotifiLib.SendNotification("<color=blue>[GENESIS]</color> Get ambushed!");
                        Back.GetButton("Ambush Gun");
                        return;
                    }
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = data.lockedPlayer.transform.position;
                    GorillaGameModes.GameMode.ReportTag(RigShit.GetNetPlayerFromRig(data.lockedPlayer));
                }

                if (!data.isTriggered)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }


        public static void Untagself()
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                NotifiLib.SendNotification("<color=red>[UNTAG]</color> Become master");
                return;
            }

            GorillaTagManager.instance.gameObject.GetComponent<GorillaTagManager>().currentInfected.Remove(PhotonNetwork.LocalPlayer);
        }

        public static void UntagAll()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    GorillaTagManager.instance.gameObject.GetComponent<GorillaTagManager>().currentInfected.Remove(p);
                }
            }
            else
            {
                NotifiLib.SendNotification("<color=red>[UNTAG]</color> Become master");
                return;
            }
        }

        public static void UntagGun()
        {
            var data = GunLib.ShootLock();
            if (data != null)
            {
                if (data.isShooting && data.isTriggered)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GorillaTagManager.instance.gameObject.GetComponent<GorillaTagManager>().currentInfected.Remove(RigShit.GetPlayerFromRig(data.lockedPlayer));
                    }
                    else
                    {
                        NotifiLib.SendNotification("<color=red>[UNTAG]</color> Become master");
                        return;
                    }
                }

                if (!data.isTriggered)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void VibrateGun()
        {
            var data = GunLib.ShootLock();
            if (data != null)
            {
                if (data.isShooting && data.isTriggered)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if(Time.time > Slowfloat)
                        {
                            BetaSetStatus(1, new RaiseEventOptions { TargetActors = new int[1] { RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber } });
                            Slowfloat = Time.time + 4f;
                        }
                    }
                    else
                    {
                        NotifiLib.SendNotification("<color=red>[VIBRATE]</color> Become master");
                        return;
                    }
                }
            }
        }

        public static void SlowGun()
        {
            var data = GunLib.ShootLock();
            if (data != null)
            {
                if (data.isShooting && data.isTriggered)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (Time.time > Slowfloat)
                        {
                            BetaSetStatus(0, new RaiseEventOptions { TargetActors = new int[1] { RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber } });
                            Slowfloat = Time.time + 4f;
                        }
                    }
                    else
                    {
                        NotifiLib.SendNotification("<color=red>[VIBRATE]</color> Become master");
                        return;
                    }
                }
            }
        }

        public static void DestroyGun()
        {
            var data = GunLib.ShootLock();
            if (data != null)
            {
                if (data.isShooting && data.isTriggered)
                {
                    if (Time.time > Slowfloat)
                    {
                        PhotonNetwork.OpRemoveCompleteCacheOfPlayer(RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber);
                        NotifiLib.SendNotification("<color=blue>[genesis]</color> Finished! Any new player that joins, cannot see the player you shot!");
                        Slowfloat = Time.time + 3f;
                    }
                }
            }
        }

        static float tagdelay;
        static float Slowfloat;

        public static void BetaSetStatus(int state, RaiseEventOptions balls)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("<color=red>[ADV]</color> Must be master!");
            }
            else
            {
                object[] statusSendData = new object[1];
                statusSendData[0] = state;
                object[] sendEventData = new object[3];
                sendEventData[0] = PhotonNetwork.ServerTimestamp;
                sendEventData[1] = (byte)2;
                sendEventData[2] = statusSendData;
                PhotonNetwork.RaiseEvent(3, sendEventData, balls, SendOptions.SendUnreliable);
            }
        }


        public static void SlowAll()
        {
            if (Time.time > Slowfloat)
            {
                BetaSetStatus(0, new RaiseEventOptions { Receivers = ReceiverGroup.Others });
                OP.NewFlusher();
                Slowfloat = Time.time + 4f;
            }
        }
        public static void DestroyAll()
        {
            foreach (Player p in PhotonNetwork.PlayerListOthers)
            {
                PhotonNetwork.OpRemoveCompleteCacheOfPlayer(p.ActorNumber);
            }
            NotifiLib.SendNotification("<color=blue>[genesis]</color> Finished! Any new player that joins, cannot see anyone but you!");
        }

        static float tagFloat;

        public static void VibrateAll()
        {
            if (Time.time > Slowfloat)
            {
                BetaSetStatus(1, new RaiseEventOptions { Receivers = ReceiverGroup.Others });
                OP.NewFlusher();
                Slowfloat = Time.time + 4f;
            }
        }

        public static void AutoTagSelf()
        {
            if (!GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.Contains("fected"))
            {

                    Back.GetButton("Tag Self").enabled = true;
                
            }
        }

        public static void TagSelf()
        {
            foreach (GorillaTagManager gorillaTagManager in GameObject.FindObjectsOfType<GorillaTagManager>())
            {
                if (gorillaTagManager.currentInfected.Contains(PhotonNetwork.LocalPlayer))
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                    Back.GetButton("tag sel").enabled = false;
                }
                else
                {
                    foreach (VRRig rig in VRRigCache.ActiveRigs)
                    {
                        if (rig.mainSkin.material.name.Contains("fected"))
                        {
                            GorillaTagger.Instance.offlineVRRig.enabled = false;
                            GorillaTagger.Instance.offlineVRRig.transform.position = rig.rightHandTransform.position;
                            GorillaTagger.Instance.myVRRig.transform.position = rig.rightHandTransform.position;
                        }
                    }
                }
            }
        }

        public static void AmbushSelf()
        {
            foreach (GorillaAmbushManager gorillaTagManager in GameObject.FindObjectsOfType<GorillaAmbushManager>())
            {
                if (gorillaTagManager.currentInfected.Contains(PhotonNetwork.LocalPlayer))
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                    Back.GetButton("Ambush Self").enabled = false;
                }
                else
                {
                    foreach (VRRig rig in VRRigCache.ActiveRigs)
                    {
                        if (rig.mainSkin.material.name.ToLower().Contains("stealth"))
                        {
                            GorillaTagger.Instance.offlineVRRig.enabled = false;
                            GorillaTagger.Instance.offlineVRRig.transform.position = rig.rightHandTransform.position;
                            GorillaTagger.Instance.myVRRig.transform.position = rig.rightHandTransform.position;
                        }
                    }
                }
            }
        }

        public static void AntiTag()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;
            if (!GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.Contains("fected"))
            {
                foreach (VRRig rig in VRRigCache.ActiveRigs)
                {
                    if (rig.mainSkin.material.name.Contains("fected"))
                    {
                        if (Vector3.Distance(rig.transform.position, GorillaTagger.Instance.offlineVRRig.transform.position) <= 7)
                        {
                            GorillaTagger.Instance.offlineVRRig.enabled = false;
                            GorillaTagger.Instance.offlineVRRig.transform.position = GorillaLocomotion.GTPlayer.Instance.transform.position - new Vector3(0, 7, 0);
                        }
                    }
                }
            }
        }

        public static void AntiAmbush()
        {
            if (GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.ToLower().Contains("stealth"))
            {
                return;
            }
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig.mainSkin.material.name.ToLower().Contains("stealth"))
                {
                    if (Vector3.Distance(rig.transform.position, GorillaTagger.Instance.offlineVRRig.transform.position) <= 7)
                    {
                        GorillaTagger.Instance.offlineVRRig.enabled = false;
                        GorillaTagger.Instance.offlineVRRig.transform.position = GorillaLocomotion.GTPlayer.Instance.transform.position - new Vector3(0, 7, 0);
                    }
                }
            }
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void Aimbot()
        {
            VRRig targetRig = null;
            foreach (VRRig vrrig in VRRigCache.ActiveRigs)
            {
                if (GorillaTagger.Instance.offlineVRRig.setMatIndex == 4)
                {
                    if (vrrig.setMatIndex == 8)
                    {
                        float dis = Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, vrrig.transform.position);
                        if (dis < 7)
                            targetRig = vrrig;
                    }
                }
                if (GorillaTagger.Instance.offlineVRRig.setMatIndex == 8)
                {
                    if (vrrig.setMatIndex == 4)
                    {
                        float dis = Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, vrrig.transform.position);
                        if (dis < 7)
                            targetRig = vrrig;
                    }
                }
            }
            foreach (SlingshotProjectile slingshotProjectile in GameObject.Find("Environment Objects/05Maze_PersistentObjects/GlobalObjectPools").GetComponentsInChildren<SlingshotProjectile>())
            {
                if (slingshotProjectile.projectileOwner.IsLocal)
                {
                    slingshotProjectile.gameObject.transform.position = targetRig.headConstraint.transform.position;
                }
            }
        }

        public static void TagAura()
        {
            

            if (tagdelay < Time.time)
            {

                tagdelay = Time.time + 0.2f;
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    Vector3 you = GorillaTagger.Instance.offlineVRRig.transform.position;
                    Vector3 rig = vrrig.headMesh.transform.position;
                    float distance = Vector3.Distance(rig, you);
                    if (GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.Contains("fected") && !vrrig.mainSkin.material.name.Contains("fected") && GorillaLocomotion.GTPlayer.Instance.disableMovement == false && distance < GorillaGameManager.instance.tagDistanceThreshold - 0.5f)
                    {
                        GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.transform.position = vrrig.transform.position;
                        GorillaGameModes.GameMode.ReportTag(RigShit.GetNetPlayerFromRig(vrrig));
                    }
                }
            }
        }

        public static void GripTagAura()
        {
            if (WristMenu.gripDownL)
            {
                if (tagdelay < Time.time)
                {
                    tagdelay = Time.time + 0.2f;
                    foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                    {
                        Vector3 you = GorillaTagger.Instance.offlineVRRig.transform.position;
                        Vector3 rig = vrrig.headMesh.transform.position;
                        float distance = Vector3.Distance(rig, you);
                        if (GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.Contains("fected") && !vrrig.mainSkin.material.name.Contains("fected") && GorillaLocomotion.GTPlayer.Instance.disableMovement == false && distance < GorillaGameManager.instance.tagDistanceThreshold - 0.5f)
                        {
                            GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.transform.position = vrrig.transform.position;
                            GorillaGameModes.GameMode.ReportTag(RigShit.GetNetPlayerFromRig(vrrig));
                        }
                    }
                }
            }
        }

        public static bool aurashit;

        public static void OFFTagAura()
        {
            if (aurashit)
            {
                foreach (VRRig rig in VRRigCache.ActiveRigs)
                {
                    if (rig.transform.Find("vis"))
                    {

                        Destroy(rig.transform.Find("vis").gameObject);

                    }
                }
                aurashit = false;
            }
        }

        public static void KillAura()
        {
            foreach (VRRig vrrig in VRRigCache.ActiveRigs)
            {
                Vector3 you = GorillaTagger.Instance.offlineVRRig.transform.position;
                Vector3 rig = vrrig.headMesh.transform.position;
                float distance = Vector3.Distance(rig, you);
                GorillaPaintbrawlManager p = GorillaPaintbrawlManager.instance.GetComponent<GorillaPaintbrawlManager>();
                if (!p.OnSameTeam(p.GetPlayerStatus(PhotonNetwork.LocalPlayer), p.GetPlayerStatus(RigShit.GetPlayerFromRig(vrrig))) && GorillaLocomotion.GTPlayer.Instance.disableMovement == false && distance < GorillaGameManager.instance.tagDistanceThreshold)
                {
                    if (Killfloat < Time.time)
                    {
                        Killfloat = Time.time + 1f;
                        GameObject gameObject = ObjectPools.instance.Instantiate(-1674517839);
                        SlingshotProjectile component = gameObject.GetComponent<SlingshotProjectile>();
                        Color throwableProjectileColor = GorillaTagger.Instance.offlineVRRig.GetThrowableProjectileColor(false);
                        component.Launch(vrrig.transform.position, Vector3.zero, PhotonNetwork.LocalPlayer, false, false, 0, 0.5f, false, throwableProjectileColor);
                    }
                }
            }
        }

        public static void AmbushAura()
        {
            if (tagdelay < Time.time)
            {
                tagdelay = Time.time + 0.2f;
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    Vector3 you = GorillaTagger.Instance.offlineVRRig.transform.position;
                    Vector3 rig = vrrig.headMesh.transform.position;
                    float distance = Vector3.Distance(rig, you);
                    if (GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.ToLower().Contains("stealth") && !vrrig.mainSkin.material.name.ToLower().Contains("stealth") && GorillaLocomotion.GTPlayer.Instance.disableMovement == false && distance < GorillaGameManager.instance.tagDistanceThreshold - 0.5f)
                    {
                        GorillaGameModes.GameMode.ReportTag(RigShit.GetNetPlayerFromRig(vrrig));
                    }
                }
            }
        }

        public static void HuntTagAura()
        {
            if (tagdelay < Time.time)
            {
                tagdelay = Time.time + 0.2f;
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    Vector3 you = GorillaTagger.Instance.offlineVRRig.transform.position;
                    Vector3 rig = vrrig.headMesh.transform.position;
                    float distance = Vector3.Distance(rig, you);
                    if (!GorillaHuntManager.instance.gameObject.GetComponent<GorillaHuntManager>().currentHunted.Contains(RigShit.GetPlayerFromRig(vrrig)) && GorillaLocomotion.GTPlayer.Instance.disableMovement == false && distance < GorillaGameManager.instance.tagDistanceThreshold - 0.5f)
                    {
                        GorillaGameModes.GameMode.ReportTag(RigShit.GetNetPlayerFromRig(vrrig));
                    }
                }
            }
        }

        public static void NoTagFreeze()
        {
            GorillaLocomotion.GTPlayer.Instance.disableMovement = false;
        }

        public static void TagAll()
        {
            if (!GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.Contains("fected"))
            {
                NotifiLib.SendNotification("<color=red>[TAG ALL]</color> You must be tagged!");
                Back.GetButtonInCate("Tag Al", 10).enabled = false;
            }
            else
            {
                bool EveryoneTagged = false;
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (!vrrig.mainSkin.material.name.Contains("fected"))
                    {
                        EveryoneTagged = true;
                        break;
                    }
                }
                if (EveryoneTagged)
                {
                    foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                    {
                        /*
                        if (!vrrig.mainSkin.material.name.Contains("fected"))
                        {
                            GorillaTagger.Instance.offlineVRRig.enabled = false;
                            GorillaTagger.Instance.offlineVRRig.transform.position = vrrig.transform.position;
                            GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.transform.position = vrrig.transform.position;
                            GorillaGameModes.GameMode.ReportTag(RigShit.GetNetPlayerFromRig(vrrig));
                        }
                        */

                        InstantTag(vrrig);
                    }
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                    Back.GetButton("Tag Al").enabled = false;
                }
            }
        }

        public static void AutoTagAll()
        {
            if (GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.Contains("fected"))
            {
                if (!GorillaLocomotion.GTPlayer.Instance.disableMovement)
                {
                    bool EveryoneTagged = false;
                    foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                    {
                        if (!vrrig.mainSkin.material.name.Contains("fected"))
                        {

                            EveryoneTagged = true;
                            break;
                        }
                    }
                    if (EveryoneTagged)
                    {
                        foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                        {
                            if (!vrrig.mainSkin.material.name.Contains("fected"))
                            {
                                GorillaTagger.Instance.offlineVRRig.enabled = false;
                                GorillaTagger.Instance.offlineVRRig.transform.position = vrrig.transform.position;
                                GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.transform.position = vrrig.transform.position;
                                GorillaGameModes.GameMode.ReportTag(RigShit.GetNetPlayerFromRig(vrrig));
                            }
                        }
                    }
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void HuntTagAll()
        {
            if (tagdelay < Time.time)
            {
                tagdelay = Time.time + 0.5f;
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    GorillaGameModes.GameMode.ReportTag(RigShit.GetNetPlayerFromRig(vrrig));
                }
                OP.NewFlusher();
            }
        }
    }
}
