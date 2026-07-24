using ExitGames.Client.Photon;
using Fusion.Photon.Realtime;
using Genesis.UI;
using Genesis.Utilities;
using GorillaExtensions;
using GorillaGameModes;
using GorillaNetworking;
using GorillaTag;
using GorillaTagScripts;
using GTAG_NotificationLib;
using HarmonyLib;
using Ionic.Zlib;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using static OVRColocationSession;
using Random = UnityEngine.Random;

namespace ShibaGTGenesis.Backend
{
    internal class OP : MonoBehaviour
    {
        public static void FlingGrabGun()
        {
            var data = GunLib.ShootLock();
            if (data.isTriggered && data.isShooting)
            {
                GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", data.lockedPlayer.Creator, new Vector3(UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(-20, 20)));
            }
        }

        public static void SpazGrabAll()
        {
            GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RpcTarget.Others, new Vector3(UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(-20, 20)));
        }

        public static void FlyGun()
        {
            var data = GunLib.ShootLock();
            if (data.isTriggered && data.isShooting)
            {
                GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", data.lockedPlayer.Creator, Vector3.up * 99f);
            }
        }

        static float GrabCooldown;

        public static void FlyAll()
        {
            GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RpcTarget.Others, Vector3.up * 99f);
        }

        public static void BringGun()
        {
            var data = GunLib.ShootLock();
            if (data.isTriggered && data.isShooting)
            {
                var vel = (GorillaTagger.Instance.offlineVRRig.transform.position - RigShit.GetRigFromPlayer(data.lockedPlayer.Creator.GetPlayerRef()).transform.position).normalized * 20f;
                GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", data.lockedPlayer.Creator, vel);
            }
        }

        public static void BringAll()
        {
            if (GrabCooldown < Time.time)
            {
                GrabCooldown = Time.time + 0.5f;
                foreach (NetPlayer p in NetworkSystem.Instance.PlayerListOthers)
                {
                    var vel = (GorillaTagger.Instance.offlineVRRig.transform.position - RigShit.GetRigFromPlayer(p.GetPlayerRef()).transform.position).normalized * 20f;
                    GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", p, vel);
                }
            }
        }

        public static Coroutine crashcoru;

        public static void GrabBullyAll()
        {
            if (GrabCooldown < Time.time)
            {
                GrabCooldown = Time.time + 0.5f;
                foreach (Player p in PhotonNetwork.PlayerListOthers)
                {
                    var rig = RigShit.GetRigFromPlayer(p);
                    if (rig.transform.position.y < 55)
                        GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", p, Vector3.up * 999);
                }
            }
        }

        public static void GrabCrashAll()
        {
            if (GrabCooldown < Time.time)
            {
                GrabCooldown = Time.time + 1f;
                foreach (Player p in PhotonNetwork.PlayerListOthers)
                {
                    var rig = RigShit.GetRigFromPlayer(p);
                    if (rig.transform.position.y < 52 && rig.transform.position.x < 26)
                        GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", p, Vector3.up * 999);
                    else
                    {
                        if (rig.transform.position.x < 26)
                            GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", p, Vector3.right * 999);
                    }
                }
            }
        }

        public static void GrabCrashPlayer(Player p)
        {
            if (GrabCooldown < Time.time)
            {
                GrabCooldown = Time.time + 0.5f;
                var rig = RigShit.GetRigFromPlayer(p);
                if (rig.transform.position.y < 52 && rig.transform.position.x < 26)
                    GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", p, Vector3.up * 999);
                else
                {
                    if (rig.transform.position.x < 26)
                        GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", p, Vector3.right * 999);
                }
            }

        }

        public static void GrabCrashGun()
        {
            var data = GunLib.ShootLock();
            if (data.isLocked && data.isShooting)
            {
                var p = data.lockedPlayer.Creator.GetPlayerRef();
                if (GrabCooldown < Time.time)
                {
                    GrabCooldown = Time.time + 1f;
                    var rig = RigShit.GetRigFromPlayer(p);
                    if (rig.transform.position.y < 52 && rig.transform.position.x < 26)
                        GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", p, Vector3.up * 999);
                    else
                    {
                        if (rig.transform.position.x < 26)
                            GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", p, Vector3.right * 999);
                    }

                }
            }
        }


        static NetPlayer selectedPlayer;
        static bool selectBool;

        public static void MovePlayer()
        {
            var data = GunLib.Shoot();
            if (data.isLocked && data.isShooting)
            {
                var rig = RigShit.GetRigFromPlayer(selectedPlayer.GetPlayerRef());

                var vel = (data.hitPosition - rig.transform.position).normalized * 20f;
                GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", selectedPlayer, vel);
            }
        }

        public static void SelectPlayer()
        {
            var data = GunLib.ShootLock();
            if (data.isLocked && data.isShooting)
            {
                selectedPlayer = data.lockedPlayer.Creator;
                if (!selectBool)
                {
                    NotifiLib.SendNotification("Selected Player: " + selectedPlayer.NickName);
                    selectBool = true;
                }
            }
            else
                selectBool = false;
        }

        static List<VRRig> rigsexcludeself = new List<VRRig>();
        public static void MoveAllToGun()
        {
            var data = GunLib.Shoot();
            if (data.isTriggered && data.isShooting)
            {
                foreach (var rig in VRRigCache.ActiveRigs)
                {
                    if (!rigsexcludeself.Contains(rig) && !rig.isOfflineVRRig)
                    {
                        rigsexcludeself.Add(rig);
                    }
                }
                var target = rigsexcludeself[UnityEngine.Random.Range(0, rigsexcludeself.Count)];

                var direction = data.hitPosition + new Vector3(0f, 2f, 0f) - target.transform.position;
                var distance = direction.magnitude;
                direction.Normalize();
                var speed = 20;
                var vel = direction * speed;

                GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", target.Creator, vel);
            }
        }

        public static Vector3[] lastLeft = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        public static Vector3[] lastRight = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

        public static void PunchMod()
        {
            int num = -1;
            foreach (var rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isOfflineVRRig)
                {
                    var vrrig = GorillaTagger.Instance.offlineVRRig;
                    num++;
                    Vector3 position = vrrig.rightHandTransform.position;
                    Vector3 position2 = rig.head.rigTarget.position;
                    if (Vector3.Distance(position, position2) < 0.25f)
                    {
                        var vel = Vector3.Normalize(vrrig.rightHandTransform.position - lastRight[num]) * 20f;
                        GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", rig.Creator.GetPlayerRef(), vel);
                    }
                    lastRight[num] = vrrig.rightHandTransform.position;
                    if (Vector3.Distance(vrrig.leftHandTransform.position, position2) < 0.25f)
                    {
                        var vel = Vector3.Normalize(vrrig.leftHandTransform.position - lastLeft[num]) * 20f;
                        GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", rig.Creator.GetPlayerRef(), vel);
                    }
                    lastLeft[num] = vrrig.leftHandTransform.position;
                }
            }
        }
        public static void SlowGunFling()
        {
            var data = GunLib.Shoot();
            if (data.isTriggered && data.isShooting)
            {
                GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", data.lockedPlayer.Creator, Vector3.down * 20f);
            }
        }
        public static void SlowAllFling()
        {
            GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RpcTarget.Others, Vector3.down * 20f);
        }
        public static void OrbitAll()
        {
            foreach (var rig in VRRigCache.ActiveRigs)
            {
                if (!rigsexcludeself.Contains(rig) && !rig.isOfflineVRRig)
                {
                    rigsexcludeself.Add(rig);
                }
            }
            var pos = GorillaTagger.Instance.headCollider.transform.position + new Vector3(Mathf.Cos((float)Time.frameCount / 30f), 1f, Mathf.Sin((float)Time.frameCount / 30f));
            var target = rigsexcludeself[UnityEngine.Random.Range(0, rigsexcludeself.Count)];

            var direction = pos - target.transform.position;
            var distance = direction.magnitude;
            direction.Normalize();
            var speed = 20;
            var vel = direction * speed;

            GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", target.Creator, vel);
        }

        public static void FreezeGunFling()
        {
            var data = GunLib.ShootLock();
            if (data.isLocked && data.isShooting)
            {
                GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", data.lockedPlayer.Creator, Vector3.zero);
            }
        }
        public static void FreezeAllFling()
        {
            GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RpcTarget.Others, Vector3.zero);
        }















        public static void SkeleCrash()
        {
            GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton").GetComponent<SecondLookSkeletonSynchValues>().SendRPC("RemoteActivateGhost", RpcTarget.Others, null);
            GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton").GetComponent<SecondLookSkeletonSynchValues>().SendRPC("RemotePlayerSeen", RpcTarget.Others, null);
            GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton").GetComponent<SecondLookSkeletonSynchValues>().SendRPC("RemotePlayerCaught", RpcTarget.Others, null);
            GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton").GetComponent<SecondLookSkeletonSynchValues>().SendRPC("RemotePlayerSeen", RpcTarget.Others, null);

            GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton").GetComponent<PhotonView>().RPC("RemoteActivateGhost", RpcTarget.Others, null);
            GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton").GetComponent<PhotonView>().RPC("RemotePlayerSeen", RpcTarget.Others, null);
            GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton").GetComponent<PhotonView>().RPC("RemotePlayerCaught", RpcTarget.Others, null);
            GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton").GetComponent<PhotonView>().RPC("RemotePlayerSeen", RpcTarget.Others, null);
        }

        public static void CamCrashGun(float delay)
        {
            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered)
            {
                if (!data.lockedPlayer.isOfflineVRRig)
                {
                    if (Time.time > cooldown2)
                    {
                        cooldown2 = Time.time + delay;
                        PhotonNetwork.NetworkingClient.OpRaiseEvent(202, new ExitGames.Client.Photon.Hashtable
                   {
                       {
                           0, "Player Network Controller"
                       },
                       {
                           6, PhotonNetwork.ServerTimestamp
                       },
                        {
                           4, new int[] { GorillaTagger.Instance.myVRRig.gameObject.transform.Find("LCKNetworkedSocialCamera").GetComponent<PhotonView>().ViewID, RigShit.GetViewFromRig(data.lockedPlayer).gameObject.transform.Find("LCKNetworkedSocialCamera").GetComponent<PhotonView>().ViewID, GorillaTagger.Instance.myVRRig.gameObject.transform.Find("LCKNetworkedSocialCamera").GetComponent<PhotonView>().ViewID, RigShit.GetViewFromRig(data.lockedPlayer).gameObject.transform.Find("LCKNetworkedSocialCamera").GetComponent<PhotonView>().ViewID }
                       },
                       {
                           7, RigShit.GetViewFromRig(data.lockedPlayer).gameObject.transform.Find("LCKNetworkedSocialCamera").GetComponent<PhotonView>().ViewID
                       },
                   }, new RaiseEventOptions
                   {
                       TargetActors = new int[] { data.lockedPlayer.Creator.ActorNumber },
                       CachingOption = EventCaching.AddToRoomCacheGlobal
                   }, SendOptions.SendReliable);
                        NewFlusher();
                    }
                }
            }
        }

        public static void CamLagGun()
        {
            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered)
            {
                if (!data.lockedPlayer.isOfflineVRRig)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        PhotonNetwork.NetworkingClient.OpRaiseEvent(202, new ExitGames.Client.Photon.Hashtable
                 {
                     {
                         0, "Player Network Controller"
                     },
                     {
                         6, PhotonNetwork.ServerTimestamp
                     },
                     {
                         7, RigShit.GetViewFromRig(data.lockedPlayer).ViewID
                     },
                 }, new RaiseEventOptions
                 {
                     TargetActors = new int[] { data.lockedPlayer.Creator.ActorNumber },
                     CachingOption = EventCaching.AddToRoomCacheGlobal
                 }, SendOptions.SendReliable);
                    }
                    NewFlusher();
                }
            }
        }

        public static void RedGun()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("You need to be master!");
                return;
            }

            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered)
            {
                var tid = 1;

                var ins = MonkeBallGame.Instance;

                var state = (int)MonkeBallGame.GameState.Playing;
                var endtime = PhotonNetwork.Time + ins.gameDuration;
                var np = new NetPlayer[] { data.lockedPlayer.Creator };
                var pids = new int[np.Length];
                var pt = new int[np.Length];
                for (int i = 0; i < np.Length; i++)
                {
                    pids[i] = np[i].ActorNumber;
                    pt[i] = tid;
                }
                var scores = new int[ins.team.Count];
                for (int i = 0; i < scores.Length; i++)
                {
                    scores[i] = 0;
                }
                var ballCount = ins.startingBalls.Count;
                var posrot = new long[ballCount];
                var vel = new long[ballCount];
                for (int i = 0; i < ballCount; i++)
                {
                    var ball = ins.startingBalls[i];
                    posrot[i] = BitPackUtils.PackHandPosRotForNetwork(ball.transform.position, ball.transform.rotation);
                    vel[i] = BitPackUtils.PackWorldPosForNetwork(ball.gameBall.GetVelocity());
                }
                ins.photonView.RPC("RequestSetGameStateRPC", RpcTarget.All,
                    new object[] { state, endtime, pids, pt, scores, posrot, vel });
            }
        }
        public static void BlueGun()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("You need to be master!");
                return;
            }

            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered)
            {
                var tid = 0;

                var ins = MonkeBallGame.Instance;

                var state = (int)MonkeBallGame.GameState.Playing;
                var endtime = PhotonNetwork.Time + ins.gameDuration;
                var np = new NetPlayer[] { data.lockedPlayer.Creator };
                var pids = new int[np.Length];
                var pt = new int[np.Length];
                for (int i = 0; i < np.Length; i++)
                {
                    pids[i] = np[i].ActorNumber;
                    pt[i] = tid;
                }
                var scores = new int[ins.team.Count];
                for (int i = 0; i < scores.Length; i++)
                {
                    scores[i] = 0;
                }
                var ballCount = ins.startingBalls.Count;
                var posrot = new long[ballCount];
                var vel = new long[ballCount];
                for (int i = 0; i < ballCount; i++)
                {
                    var ball = ins.startingBalls[i];
                    posrot[i] = BitPackUtils.PackHandPosRotForNetwork(ball.transform.position, ball.transform.rotation);
                    vel[i] = BitPackUtils.PackWorldPosForNetwork(ball.gameBall.GetVelocity());
                }
                ins.photonView.RPC("RequestSetGameStateRPC", RpcTarget.All,
                    new object[] { state, endtime, pids, pt, scores, posrot, vel });
            }
        }
        public static void RedAll()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("You need to be master!");
                return;
            }

            var tid = 1;

            var ins = MonkeBallGame.Instance;

            var state = (int)MonkeBallGame.GameState.Playing;
            var endtime = PhotonNetwork.Time + ins.gameDuration;
            var np = NetworkSystem.Instance.AllNetPlayers;
            var pids = new int[np.Length];
            var pt = new int[np.Length];
            for (int i = 0; i < np.Length; i++)
            {
                pids[i] = np[i].ActorNumber;
                pt[i] = tid;
            }
            var scores = new int[ins.team.Count];
            for (int i = 0; i < scores.Length; i++)
            {
                scores[i] = 0;
            }
            var ballCount = ins.startingBalls.Count;
            var posrot = new long[ballCount];
            var vel = new long[ballCount];
            for (int i = 0; i < ballCount; i++)
            {
                var ball = ins.startingBalls[i];
                posrot[i] = BitPackUtils.PackHandPosRotForNetwork(ball.transform.position, ball.transform.rotation);
                vel[i] = BitPackUtils.PackWorldPosForNetwork(ball.gameBall.GetVelocity());
            }
            ins.photonView.RPC("RequestSetGameStateRPC", RpcTarget.All,
                new object[] { state, endtime, pids, pt, scores, posrot, vel });
        }
        public static void BlueAll()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("You need to be master!");
                return;
            }
            var tid = 0;

            var ins = MonkeBallGame.Instance;

            var state = (int)MonkeBallGame.GameState.Playing;
            var endtime = PhotonNetwork.Time + ins.gameDuration;
            var np = NetworkSystem.Instance.AllNetPlayers;
            var pids = new int[np.Length];
            var pt = new int[np.Length];
            for (int i = 0; i < np.Length; i++)
            {
                pids[i] = np[i].ActorNumber;
                pt[i] = tid;
            }
            var scores = new int[ins.team.Count];
            for (int i = 0; i < scores.Length; i++)
            {
                scores[i] = 0;
            }
            var ballCount = ins.startingBalls.Count;
            var posrot = new long[ballCount];
            var vel = new long[ballCount];
            for (int i = 0; i < ballCount; i++)
            {
                var ball = ins.startingBalls[i];
                posrot[i] = BitPackUtils.PackHandPosRotForNetwork(ball.transform.position, ball.transform.rotation);
                vel[i] = BitPackUtils.PackWorldPosForNetwork(ball.gameBall.GetVelocity());
            }
            ins.photonView.RPC("RequestSetGameStateRPC", RpcTarget.All,
                new object[] { state, endtime, pids, pt, scores, posrot, vel });
        }

        public static void CamCrashAll(float delay)
        {
            var rand = RigShit.GetRandomPlayer(false);
            var rand2 = RigShit.GetRandomPlayer(false);

            if (rand2 == rand)
            {
                return;
            }

            PhotonNetwork.NetworkingClient.OpRaiseEvent(202, new ExitGames.Client.Photon.Hashtable
                   {
                       {
                           0, "Player Network Controller"
                       },
                       {
                           6, PhotonNetwork.ServerTimestamp
                       },
                        {
                           4, new int[] { RigShit.GetViewFromPlayer(rand2).gameObject.transform.Find("LCKNetworkedSocialCamera").GetComponent<PhotonView>().ViewID,
                               RigShit.GetViewFromPlayer(rand).gameObject.transform.Find("LCKNetworkedSocialCamera").GetComponent<PhotonView>().ViewID,
                               RigShit.GetViewFromPlayer(rand2).gameObject.transform.Find("LCKNetworkedSocialCamera").GetComponent<PhotonView>().ViewID,
                               RigShit.GetViewFromPlayer(rand).gameObject.transform.Find("LCKNetworkedSocialCamera").GetComponent<PhotonView>().ViewID }
                       },
                       {
                           7, RigShit.GetViewFromPlayer(rand).gameObject.transform.Find("LCKNetworkedSocialCamera").GetComponent<PhotonView>().ViewID
                       },
                   }, new RaiseEventOptions
                   {
                       TargetActors = new int[] { rand.ActorNumber },
                       CachingOption = EventCaching.AddToRoomCacheGlobal
                   }, SendOptions.SendReliable);
            NewFlusher();

        }


        public static void CamLagAll()
        {
            var rand = RigShit.GetRandomPlayer(false);
            for (int i = 0; i < 3; i++)
            {
                PhotonNetwork.NetworkingClient.OpRaiseEvent(202, new ExitGames.Client.Photon.Hashtable
                 {
                     {
                         0, "Player Network Controller"
                     },
                     {
                         6, PhotonNetwork.ServerTimestamp
                     },
                     {
                         7, RigShit.GetViewFromPlayer(rand).ViewID
                     },
                 }, new RaiseEventOptions
                 {
                     TargetActors = new int[] { rand.ActorNumber },
                     CachingOption = EventCaching.AddToRoomCacheGlobal
                 }, SendOptions.SendReliable);
            }
            NewFlusher();

        }

        public static void EnableLucy()
        {
            if (!GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton/Offset/SpookySkeletonParent").activeSelf)
            {
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton").GetComponent<SecondLookSkeletonSynchValues>().SendRPC("RemoteActivateGhost", RpcTarget.MasterClient, Array.Empty<object>());
            }
        }
        public static void NoClipGun()
        {
            var data = GunLib.ShootLock();
            if (data.isShooting && data.isLocked)
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    NotifiLib.SendNotification("You need to be master!");
                    return;
                }
                EnableLucy();
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton/Offset/SpookySkeletonParent").transform.position = data.hitPosition;
            }
        }
        public static void MoveSkeletonGun()
        {
            var shmegma = GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton").GetComponent<SecondLookSkeleton>();
            var data = GunLib.Shoot();
            if (data.isShooting && data.isTriggered)
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    NotifiLib.SendNotification("You need to be master!");
                    return;
                }
                EnableLucy();
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton/Offset/SpookySkeletonParent").transform.rotation = GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.rotation;
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/MinesSecondLookSkeleton/Offset/SpookySkeletonParent").transform.position = data.hitPosition;
                shmegma.enabled = false;
            }
            else
            {
                shmegma.enabled = true;
            }
        }

        private static readonly object[] projectileSendData = new object[9];

        public static readonly NetEventOptions neoAll = new NetEventOptions
        {
            Reciever = NetEventOptions.RecieverTarget.all
        };

        public static int counter = 0;

        public static void SlingshotCall(int cosmeticID, Vector3 pos, Vector3 vel, Color color)
        {
            if (WristMenu.gripDownR)
            {
                if (GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[cosmeticID].gameObject.activeSelf)
                {
                    if (Time.time > cooldown2)
                    {
                        cooldown2 = Time.time + 0.15f;

                        GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[cosmeticID].storedZone = BodyDockPositions.DropPositions.RightArm;
                        GorillaTagger.Instance.offlineVRRig.projectileWeapon.currentState = TransferrableObject.PositionState.InRightHand;
                        var mRef = typeof(VRRig).Assembly.GetType("ProjectileTracker").GetMethod("IncrementLocalPlayerProjectileCount", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                        object[] projectileSendData = { pos, vel, 0, counter, true, (byte)color.r, (byte)color.g, (byte)color.b, (byte)color.a };

                        GorillaTagger.Instance.offlineVRRig.transform.position = pos;

                        counter = (int)mRef.Invoke(null, System.Array.Empty<object>());

                        typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
                        PhotonNetwork.RaiseEvent(3, new object[] { PhotonNetwork.ServerTimestamp, (byte)0, projectileSendData }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendUnreliable);

                        NewFlusher();
                    }
                    else
                    {
                        GorillaTagger.Instance.offlineVRRig.transform.position = pos;
                        typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
                    }
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.SetActiveTransferrableObjectIndex(1, cosmeticID);
                    GameObject gameObject2 = GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[cosmeticID].gameObject;
                    gameObject2.SetActive(true);
                    GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[cosmeticID].storedZone = BodyDockPositions.DropPositions.RightArm;
                    CosmeticsController.instance.ApplyCosmeticItemToSet(GorillaTagger.Instance.offlineVRRig.tryOnSet, CosmeticsController.instance.GetItemFromDict("LMABB."), false, false);
                    CosmeticsController.instance.UpdateWornCosmetics(true);
                    typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

                    NewFlusher();
                }
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-51.4897f, 16.9286f, -120.1083f);
                typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
            }
            else
                GorillaTagger.Instance.offlineVRRig.enabled = true;
        }






        static float cooldown2;


        public static void FirecrackerSpam()
        {
            if (WristMenu.gripDownR)
            {
                SendFirecracker(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 100.4f);
            }
            else
                GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void FirecrackerMinigun()
        {
            if (WristMenu.gripDownR)
            {
                SendFirecracker(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 500.4f);
            }
            else
                GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void FirecrackerRain()
        {
            if (WristMenu.gripDownR)
            {
                SendFirecracker(RainPosGen(6), Vector3.down);
            }
            else
                GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void FirecrackerGun()
        {
            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered)
            {
                Vector3 funn = (data.hitPosition - GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position).normalized;
                float penis = 15f;
                funn *= penis;
                SendFirecracker(PlayerMovement.TrueRightHand().position, funn);

            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void BombSpam()
        {
            if (WristMenu.gripDownR)
            {
                SendBomb(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 100.4f);
            }
            else
                GorillaTagger.Instance.offlineVRRig.enabled = true;
        }
        public static void BombMinigun()
        {
            if (WristMenu.gripDownR)
            {
                SendBomb(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 500.4f);
            }
            else
                GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void BombGun()
        {
            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered)
            {
                Vector3 funn = (data.hitPosition - GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position).normalized;
                float penis = 15f;
                funn *= penis;
                SendBomb(PlayerMovement.TrueRightHand().position, funn);

            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        private static float delay3;
        public static void CrashAll(float delay, int forr)
        {
            if (Time.time > delay3)
            {
                delay3 = Time.time + delay;
                var torpc = FriendshipGroupDetection.Instance.photonView;
                for (int i = 0; i < forr; i++)
                {
                    torpc.RPC("NotifyPartyMerging", RpcTarget.Others, new object[] { null });
                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }
            NewFlusher();
        }
        public static void CrashAllEle()
        {
            if (Time.time > delay3)
            {
                SetTick(9999);
                for (int i = 0; i < 950; i++)
                {
                    string shuffler = UnityEngine.Random.Range(0, 99).ToString().PadLeft(2, '0') + UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');
                    string keyStr = UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');

                    object[] groupJoinSendData = new object[2];
                    groupJoinSendData[0] = shuffler;
                    groupJoinSendData[1] = keyStr;
                    NetEventOptions netEventOptions = new NetEventOptions { Reciever = NetEventOptions.RecieverTarget.others };

                    RoomSystem.SendEvent(11, groupJoinSendData, netEventOptions, false);
                }

                delay3 = Time.time + 2f;

                NewFlusher();
            }
        }
        public static void CrashGunEle()
        {
            var data = GunLib.ShootLock();
            if (data.isLocked && data.isTriggered)
            {
                if (Time.time > delay3)
                {
                    SetTick(9999);
                    for (int i = 0; i < 950; i++)
                    {
                        string shuffler = UnityEngine.Random.Range(0, 99).ToString().PadLeft(2, '0') + UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');
                        string keyStr = UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');

                        object[] groupJoinSendData = new object[2];
                        groupJoinSendData[0] = shuffler;
                        groupJoinSendData[1] = keyStr;
                        NetEventOptions netEventOptions = new NetEventOptions { TargetActors = new int[] { data.lockedPlayer.OwningNetPlayer.ActorNumber } };

                        RoomSystem.SendEvent(11, groupJoinSendData, netEventOptions, false);
                    }

                    delay3 = Time.time + 2f;

                    NewFlusher();
                }
            }
        }
        public static void LagGunV2(float delay, int forr)
        {
            var data = GunLib.ShootLock();
            if (data.isLocked && data.isTriggered)
            {
                if (Time.time > delay3)
                {
                    for (int i = 0; i < forr; i++)
                    {
                        string shuffler = UnityEngine.Random.Range(0, 99).ToString().PadLeft(2, '0') + UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');
                        string keyStr = UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');

                        object[] groupJoinSendData = new object[2];
                        groupJoinSendData[0] = shuffler;
                        groupJoinSendData[1] = keyStr;
                        NetEventOptions netEventOptions = new NetEventOptions { TargetActors = new[] { data.lockedPlayer.OwningNetPlayer.ActorNumber } };

                        RoomSystem.SendEvent(11, groupJoinSendData, netEventOptions, false);
                    }

                    delay3 = Time.time + delay;
                    NewFlusher();
                }
            }
        }

        public static void LagAllV2(float delay, int forr)
        {
            if (Time.time > delay3)
            {
                for (int i = 0; i < forr; i++)
                {
                    string shuffler = UnityEngine.Random.Range(0, 99).ToString().PadLeft(2, '0') + UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');
                    string keyStr = UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');

                    object[] groupJoinSendData = new object[2];
                    groupJoinSendData[0] = shuffler;
                    groupJoinSendData[1] = keyStr;
                    NetEventOptions netEventOptions = new NetEventOptions { Reciever = NetEventOptions.RecieverTarget.others };

                    RoomSystem.SendEvent(11, groupJoinSendData, netEventOptions, false);
                }

                delay3 = Time.time + delay;
                NewFlusher();
            }
        }

        public static void CrashGun(float delay, int forr)
        {
            var data = GunLib.ShootLock();
            if (data.isLocked && data.isTriggered)
            {
                if (Time.time > delay3)
                {
                    delay3 = Time.time + delay;
                    var torpc = FriendshipGroupDetection.Instance.photonView;
                    for (int i = 0; i < forr; i++)
                    {
                        torpc.RPC("NotifyPartyMerging", data.lockedPlayer.OwningNetPlayer.GetPlayerRef(), new object[] { null });
                        PhotonNetwork.SendAllOutgoingCommands();
                    }
                }
            }
            NewFlusher();
        }
        public static void FlingGun()
        {
            var data = GunLib.ShootLock();
            if (data.isShooting && data.isLocked)
            {
                EnableSnowballs();
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = data.hitPosition + new Vector3(0f, -2f, 0f);
                if (Time.time > cooldown)
                {
                    NewFlusher();

                    object[] source = new object[]
                    {
                         data.lockedPlayer.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f)).normalized / 1.7f,
                         new Vector3(0f, -50f, 0f),
                         5f
                    };
                    object[] eventContent = source.Prepend(StaticHash.Compute("SnowballThrowable",
                        "SnowballThrowEventRight",
                        GorillaTagger.Instance.myVRRig.ViewID.ToString())).ToArray<object>();
                    PhotonNetwork.RaiseEvent(176, eventContent, new RaiseEventOptions
                    {
                        TargetActors = new int[] { data.lockedPlayer.Creator.ActorNumber }
                    }, SendOptions.SendReliable);
                    cooldown = Time.time + 0.30f;
                }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                DisableSnowballs();
            }
        }

        public static async void SendFirecracker(Vector3 pos, Vector3 vel)
        {
            if (Time.time > cooldown2)
            {
                cooldown2 = Time.time + 0.25f;


                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-52.0345f, 17.6741f, -119.6294f);
                GorillaTagger.Instance.offlineVRRig.enabled = false;


                typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

                GorillaTagger.Instance.offlineVRRig.SetActiveTransferrableObjectIndex(1, 587);
                GameObject gameObject2 = GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[587].gameObject;
                gameObject2.SetActive(true);
                GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[587].storedZone = BodyDockPositions.DropPositions.RightArm;
                GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[587].currentState = TransferrableObject.PositionState.InRightHand;

                GorillaTagger.Instance.offlineVRRig.transform.position = pos + new Vector3(0, -3, 0);
                typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

                object[] source = new object[]
                {
                     pos,
                     Quaternion.Euler(UnityEngine.Random.Range(-360f, 360f),UnityEngine.Random.Range(-360f, 360f),UnityEngine.Random.Range(-360f, 360f)),
                     vel,
                     1f
                };
                var rd = GameObject
                    .Find(
                        "Player Objects/Local VRRig/Local Gorilla Player/RigAnchor/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/TransferrableItemRightHand/FireCrackersAnchor(Clone)/LMANZ.")
                    .GetComponent<RubberDuckEvents>();

                await Task.Delay(100);

                rd.Activate.RaiseAll(source);

                //GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-52.0345f, 17.6741f, -119.6294f);
                typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
            }
            NewFlusher();
        }



        static float bombdelay;

        public static async void SendBomb(Vector3 pos, Vector3 vel)
        {
            if (Time.time > bombdelay)
            {
                bombdelay = Time.time + 0.25f;


                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-52.0345f, 17.6741f, -119.6294f);
                GorillaTagger.Instance.offlineVRRig.enabled = false;


                typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

                GorillaTagger.Instance.offlineVRRig.SetActiveTransferrableObjectIndex(1, 600);
                GameObject gameObject2 = GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[600].gameObject;
                gameObject2.SetActive(true);
                GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[600].storedZone = BodyDockPositions.DropPositions.RightArm;
                GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[600].currentState = TransferrableObject.PositionState.InRightHand;

                GorillaTagger.Instance.offlineVRRig.transform.position = pos + new Vector3(0, -3, 0);
                typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

                object[] source = new object[]
                {
                 pos,
                 Quaternion.Euler(UnityEngine.Random.Range(-360f, 360f),UnityEngine.Random.Range(-360f, 360f),UnityEngine.Random.Range(-360f, 360f)),
                 vel,
                 1f
                };
                var rd = GameObject
                    .Find(
                        "Player Objects/Local VRRig/Local Gorilla Player/RigAnchor/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/TransferrableItemRightHand/SmokeBomb_Anchor Variant(Clone)/LMAOM.")
                    .GetComponent<RubberDuckEvents>();

                await Task.Delay(100);

                rd.Activate.RaiseAll(source);

                //GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-52.0345f, 17.6741f, -119.6294f);
                typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
            }
            NewFlusher();
        }

        static float heartdelay;
        static float fatdelay;
        static float normaldelay;


        public static void SendThrowableHeart(Vector3 pos, Vector3 vel, Quaternion rot, float forcescale = 1f, bool visible = true, bool delaything = true)
        {
            if (Time.time > cooldown2)
            {
                Timeout = !Timeout;
                if (delaything)
                    cooldown2 = Time.time + 0.25f;
            }
            if (Timeout)
            {
                if (Time.time > heartdelay)
                {
                    heartdelay = Time.time + 0.35f;

                    GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-52.0345f, 17.6741f, -119.6294f);
                    GorillaTagger.Instance.offlineVRRig.enabled = false;


                    typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

                    GorillaTagger.Instance.offlineVRRig.SetActiveTransferrableObjectIndex(1, 603);
                    GameObject gameObject2 = GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[603].gameObject;
                    gameObject2.SetActive(true);
                    GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[603].storedZone = BodyDockPositions.DropPositions.RightArm;
                    GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[603].currentState = TransferrableObject.PositionState.InRightHand;

                    GorillaTagger.Instance.offlineVRRig.transform.position = pos + new Vector3(0, -3, 0);
                    typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

                    object[] source = new object[]
                    {
                        StaticHash.Compute("PaperPlaneThrowable".GetStaticHash(), "LaunchProjectileLocal".GetStaticHash()),
                        TransferrableObject.PositionState.InRightHand,
                        pos,
                        rot,
                        vel
                    };
                    PhotonNetwork.RaiseEvent(176, source, new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others
                    }, SendOptions.SendReliable);
                    if (visible)
                    {
                        var b = ObjectPools.instance.Instantiate(GameObject.Find("Environment Objects/05Maze_PersistentObjects/GlobalObjectPools/ThrowableHeart_Projectile Variant(PoolIndex=29)"));
                        b.transform.localScale = Vector3.one;
                        b.transform.localScale *= HarmonyLib.Traverse.Create(GorillaLocomotion.GTPlayer.Instance).Field("nativeScale").GetValue<float>() * forcescale;
                        b.GetComponent<PaperPlaneProjectile>().Launch(pos, rot, vel);
                    }
                }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.SetActiveTransferrableObjectIndex(1, -1);
                typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
            }
            NewFlusher();
        }

        static float cooooooldown;

        public static void SendThrowablePlane(Vector3 pos, Vector3 vel, Quaternion rot, float forcescale = 1f, bool visible = true, bool delaything = true)
        {
            if (Time.time > cooldown2)
            {
                Timeout = !Timeout;
                if (delaything)
                    cooldown2 = Time.time + 0.25f;
            }
            if (Timeout)
            {
                if (Time.time > normaldelay)
                {
                    normaldelay = Time.time + 0.35f;

                    GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-52.0345f, 17.6741f, -119.6294f);
                    GorillaTagger.Instance.offlineVRRig.enabled = false;


                    typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

                    GorillaTagger.Instance.offlineVRRig.SetActiveTransferrableObjectIndex(1, 173);
                    GameObject gameObject2 = GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[173].gameObject;
                    gameObject2.SetActive(true);
                    GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[173].storedZone = BodyDockPositions.DropPositions.RightArm;
                    GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[173].currentState = TransferrableObject.PositionState.InRightHand;

                    GorillaTagger.Instance.offlineVRRig.transform.position = pos + new Vector3(0, -3, 0);
                    typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

                    object[] source = new object[]
                    {
                        StaticHash.Compute("PaperPlaneThrowable".GetStaticHash(), "LaunchProjectileLocal".GetStaticHash()),
                        TransferrableObject.PositionState.InRightHand,
                        pos,
                        rot,
                        vel
                    };
                    PhotonNetwork.RaiseEvent(176, source, new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others
                    }, SendOptions.SendReliable);
                    if (visible)
                    {
                        var b = ObjectPools.instance.Instantiate(GameObject.Find("Environment Objects/05Maze_PersistentObjects/GlobalObjectPools/PaperAirplane_Projectile(PoolIndex=3)"));
                        b.transform.localScale = Vector3.one;
                        b.transform.localScale *= HarmonyLib.Traverse.Create(GorillaLocomotion.GTPlayer.Instance).Field("nativeScale").GetValue<float>() * forcescale;
                        b.GetComponent<PaperPlaneProjectile>().Launch(pos, rot, vel);
                    }
                }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.SetActiveTransferrableObjectIndex(1, -1);
                typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
            }
            NewFlusher();
        }



        public static void SendThrowableFatPlane(Vector3 pos, Vector3 vel, Quaternion rot, float forcescale = 1f, bool visible = true, bool delaything = true)
        {
            if (Time.time > cooldown2)
            {
                Timeout = !Timeout;
                if (delaything)
                    cooldown2 = Time.time + 0.25f;
            }
            if (Timeout)
            {
                if (Time.time > fatdelay)
                {
                    fatdelay = Time.time + 0.35f;

                    GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-52.0345f, 17.6741f, -119.6294f);
                    GorillaTagger.Instance.offlineVRRig.enabled = false;


                    typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

                    GorillaTagger.Instance.offlineVRRig.SetActiveTransferrableObjectIndex(1, 597);
                    GameObject gameObject2 = GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[597].gameObject;
                    gameObject2.SetActive(true);
                    GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[597].storedZone = BodyDockPositions.DropPositions.RightArm;
                    GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[597].currentState = TransferrableObject.PositionState.InRightHand;

                    GorillaTagger.Instance.offlineVRRig.transform.position = pos + new Vector3(0, -3, 0);
                    typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

                    object[] source = new object[]
                    {
                    StaticHash.Compute("PaperPlaneThrowable".GetStaticHash(), "LaunchProjectileLocal".GetStaticHash()),
                    TransferrableObject.PositionState.InRightHand,
                    pos,
                    rot,
                    vel
                    };
                    PhotonNetwork.RaiseEvent(176, source, new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others
                    }, SendOptions.SendReliable);
                    if (visible)
                    {
                        var b = ObjectPools.instance.Instantiate(GameObject.Find("Environment Objects/05Maze_PersistentObjects/GlobalObjectPools/PaperAirplaneSquare_Projectile(PoolIndex=31)"));
                        b.transform.localScale = Vector3.one;
                        b.transform.localScale *= HarmonyLib.Traverse.Create(GorillaLocomotion.GTPlayer.Instance).Field("nativeScale").GetValue<float>() * forcescale;
                        b.GetComponent<PaperPlaneProjectile>().Launch(pos, rot, vel);
                    }
                }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.SetActiveTransferrableObjectIndex(1, -1);
                typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
            }
            NewFlusher();
        }
        public static void PaperplaneMinigun()
        {
            if (WristMenu.gripDownR)
            {
                SendThrowablePlane(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 9000.4f, Quaternion.identity);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void BothMinigun()
        {
            if (WristMenu.gripDownR)
            {
                SendThrowablePlane(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 9000.4f, Quaternion.identity);
            }
            if (WristMenu.gripDownL)
            {
                SendThrowableFatPlane(PlayerMovement.TrueLeftHand().position, PlayerMovement.TrueLeftHand().forward * Time.deltaTime * 9000.4f, Quaternion.identity);
            }
            if (!WristMenu.gripDownR && !WristMenu.gripDownL)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void PaperplaneSpam()
        {
            if (WristMenu.gripDownR)
            {
                SendThrowablePlane(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 5f, Quaternion.identity);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        private static int archiveIncrement;
        public static int GetProjectileIncrement(Vector3 Position, Vector3 Velocity, float Scale)
        {
            try
            {
                GameObject SlingshotProjectileGameObject = new GameObject("SlingshotProjectileHolder");
                SlingshotProjectile SlingshotProjectile = SlingshotProjectileGameObject.AddComponent<SlingshotProjectile>();

                Type ProjectileTracker = typeof(GorillaLocomotion.GTPlayer).Assembly.GetType("ProjectileTracker");
                Type ProjectileInfo = ProjectileTracker.GetNestedType("ProjectileInfo", BindingFlags.Public | BindingFlags.Instance);
                object LocalProjectileInfo = Activator.CreateInstance(ProjectileInfo, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[] { PhotonNetwork.Time, Velocity, Position, Scale, SlingshotProjectile }, null);

                object m_localProjectiles = ProjectileTracker.GetField("m_localProjectiles", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

                MethodInfo AddAndIncrement = m_localProjectiles.GetType().GetMethod("AddAndIncrement", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                int Data = (int)AddAndIncrement.Invoke(m_localProjectiles, new object[] { LocalProjectileInfo });

                UnityEngine.Object.Destroy(SlingshotProjectileGameObject);
                return Data;
            }
            catch
            {
                archiveIncrement += 1;
                return archiveIncrement;
            }
        }

        private static GameObject BasePlane;
        private static Vector3 startVelocity;
        private static int characterIndex;
        public static float sprayPaperPlanesDelay;

        public static void PaperPlaneText()
        {
            string targetString = PhotonNetwork.NickName.ToUpper();

            if (WristMenu.gripDownR)
            {
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-51.4897f, 16.9286f, -120.1083f);
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                if (BasePlane == null)
                {
                    sprayPaperPlanesDelay = Time.time + 0.5f;
                    characterIndex = 0;
                    startVelocity = GorillaTagger.Instance.rightHandTransform.forward * 3f;

                    BasePlane = ObjectPools.instance.Instantiate(GameObject.Find("Environment Objects/05Maze_PersistentObjects/GlobalObjectPools/PaperAirplane_Projectile(PoolIndex=3)"));
                    BasePlane.GetComponent<PaperPlaneProjectile>().Launch(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.rotation, startVelocity);
                }

                if (Time.time > sprayPaperPlanesDelay)
                {
                    sprayPaperPlanesDelay = Time.time + 0.1f;
                    bool[][] characterData = Letters[targetString[characterIndex].ToString()];

                    for (int i = 0; i < characterData.Length; i++)
                    {
                        bool[] column = characterData[i];

                        for (int j = 0; j < column.Length; j++)
                        {
                            bool currentIndex = column[j];
                            Vector3 offset = new Vector3((j * 0.2f) + (characterIndex * 1.2f), i * -0.2f, 0f);

                            if (currentIndex)
                            {
                                SendThrowableHeart(BasePlane.transform.position + (BasePlane.transform.rotation * offset), startVelocity, GorillaTagger.Instance.rightHandTransform.rotation, 1, true, false);
                                SendThrowableHeart(BasePlane.transform.position + (BasePlane.transform.rotation * offset), startVelocity, GorillaTagger.Instance.rightHandTransform.rotation, 1, true, false);
                            }
                        }
                    }

                    characterIndex++;
                }
            }
            else
            {
                if (!GorillaTagger.Instance.offlineVRRig.enabled)
                    GorillaTagger.Instance.offlineVRRig.enabled = true;

                if (BasePlane != null)
                    BasePlane = null;
            }
        }

        public static void PaperPlaneTextGun()
        {
            string targetString = PhotonNetwork.NickName.ToUpper();

            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered)
            {
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-51.4897f, 16.9286f, -120.1083f);
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                if (BasePlane == null)
                {
                    sprayPaperPlanesDelay = Time.time + 0.5f;
                    characterIndex = 0;
                    startVelocity = new Vector3(0f, 0.1f, 0f);

                    BasePlane = ObjectPools.instance.Instantiate(GameObject.Find("Environment Objects/05Maze_PersistentObjects/GlobalObjectPools/PaperAirplane_Projectile(PoolIndex=3)"));
                    BasePlane.GetComponent<PaperPlaneProjectile>().Launch(data.hitPosition + new Vector3(0f, 1f, 0f), Quaternion.identity, startVelocity);
                }

                if (Time.time > sprayPaperPlanesDelay)
                {
                    sprayPaperPlanesDelay = Time.time + 0.1f;
                    bool[][] characterData = Letters[targetString[characterIndex].ToString()];

                    for (int i = 0; i < characterData.Length; i++)
                    {
                        bool[] column = characterData[i];

                        for (int j = 0; j < column.Length; j++)
                        {
                            bool currentIndex = column[j];
                            Vector3 offset = new Vector3((j * 0.2f) + (characterIndex * 1.2f), i * -0.2f, 0f);

                            if (currentIndex)
                            {
                                SendThrowablePlane(BasePlane.transform.position + offset, startVelocity, GorillaTagger.Instance.rightHandTransform.rotation, 1, true, false);
                                SendThrowablePlane(BasePlane.transform.position + offset, startVelocity, GorillaTagger.Instance.rightHandTransform.rotation, 1, true, false);
                            }
                        }
                    }

                    characterIndex++;
                }
            }
            else
            {
                if (!GorillaTagger.Instance.offlineVRRig.enabled)
                    GorillaTagger.Instance.offlineVRRig.enabled = true;

                if (BasePlane != null)
                    BasePlane = null;
            }
        }



        private static Dictionary<string, bool[][]> Letters = new Dictionary<string, bool[][]> {
        { "A", new bool[][] {
            new bool[] { false, true, true, true, false },
            new bool[] { true, false, false, false, true },
            new bool[] { true, true, true, true, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
        } },
        { "B", new bool[][] {
            new bool[] { true, true, true, true, false },
            new bool[] { true, false, false, false, true },
            new bool[] { true, true, true, true, false },
            new bool[] { true, false, false, false, true },
            new bool[] { true, true, true, true, true },
        } },
        { "C", new bool[][] {
            new bool[] { false, true, true, true, true },
            new bool[] { true, false, false, false, false },
            new bool[] { true, false, false, false, false },
            new bool[] { true, false, false, false, false },
            new bool[] { false, true, true, true, true },
        } },
        { "D", new bool[][] {
            new bool[] { true, true, true, true, false },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, true, true, true, false },
        } },
        { "E", new bool[][] {
            new bool[] { true, true, true, true, true },
            new bool[] { true, false, false, false, false },
            new bool[] { true, true, true, false, false },
            new bool[] { true, false, false, false, false },
            new bool[] { true, true, true, true, true },
        } },
        { "F", new bool[][] {
            new bool[] { true, true, true, true, true },
            new bool[] { true, false, false, false, false },
            new bool[] { true, true, true, false, false },
            new bool[] { true, false, false, false, false },
            new bool[] { true, false, false, false, false },
        } },
        { "G", new bool[][] {
            new bool[] { true, true, true, true, true },
            new bool[] { true, false, false, false, false },
            new bool[] { true, false, false, true, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, true, true, true, true },
        } },
        { "H", new bool[][] {
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, true, true, true, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
        } },
        { "I", new bool[][] {
            new bool[] { true, true, true, true, true },
            new bool[] { false, false, true, false, false },
            new bool[] { false, false, true, false, false },
            new bool[] { false, false, true, false, false },
            new bool[] { true, true, true, true, true },
        } },
        { "J", new bool[][] {
            new bool[] { false, false, false, false, true },
            new bool[] { false, false, false, false, true },
            new bool[] { false, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { false, true, true, true, false },
        } },
        { "K", new bool[][] {
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, true, false },
            new bool[] { true, true, true, false, false },
            new bool[] { true, false, false, true, false },
            new bool[] { true, false, false, false, true },
        } },
        { "L", new bool[][] {
            new bool[] { true, false, false, false, false },
            new bool[] { true, false, false, false, false },
            new bool[] { true, false, false, false, false },
            new bool[] { true, false, false, false, false },
            new bool[] { true, true, true, true, true },
        } },
        { "M", new bool[][] {
            new bool[] { true, true, false, true, true },
            new bool[] { true, false, true, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
        } },
        { "N", new bool[][] {
            new bool[] { true, false, false, false, true },
            new bool[] { true, true, false, false, true },
            new bool[] { true, false, true, false, true },
            new bool[] { true, false, false, true, true },
            new bool[] { true, false, false, false, true },
        } },
        { "O", new bool[][] {
            new bool[] { false, true, true, true, false },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { false, true, true, true, false },
        } },
        { "P", new bool[][] {
            new bool[] { true, true, true, true, false },
            new bool[] { true, false, false, false, true },
            new bool[] { true, true, true, true, false },
            new bool[] { true, false, false, false, false },
            new bool[] { true, false, false, false, false },
        } },
        { "Q", new bool[][] {
            new bool[] { false, true, true, true, false },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, true, false, true },
            new bool[] { true, false, false, true, false },
            new bool[] { false, true, true, false, true },
        } },
        { "R", new bool[][] {
            new bool[] { true, true, true, true, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, true, true, true, true },
            new bool[] { true, false, false, true, false },
            new bool[] { true, false, false, false, true },
        } },
        { "S", new bool[][] {
            new bool[] { true, true, true, true, true },
            new bool[] { true, false, false, false, false },
            new bool[] { true, true, true, true, true },
            new bool[] { false, false, false, false, true },
            new bool[] { true, true, true, true, true },
        } },
        { "T", new bool[][] {
            new bool[] { true, true, true, true, true },
            new bool[] { false, false, true, false, false },
            new bool[] { false, false, true, false, false },
            new bool[] { false, false, true, false, false },
            new bool[] { false, false, true, false, false },
        } },
        { "U", new bool[][] {
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { false, true, true, true, false },
        } },
        { "V", new bool[][] {
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { false, true, false, true, false },
            new bool[] { false, true, false, true, false },
            new bool[] { false, false, true, false, false },
        } },
        { "W", new bool[][] {
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, false, false, true },
            new bool[] { true, false, true, false, true },
            new bool[] { false, true, false, true, false },
        } },
        { "X", new bool[][] {
            new bool[] { true, false, false, false, true },
            new bool[] { false, true, false, true, false },
            new bool[] { false, false, true, false, false },
            new bool[] { false, true, false, true, false },
            new bool[] { true, false, false, false, true },
        } },
        { "Y", new bool[][] {
            new bool[] { true, false, false, false, true },
            new bool[] { false, true, false, true, false },
            new bool[] { false, false, true, false, false },
            new bool[] { false, false, true, false, false },
            new bool[] { false, false, true, false, false },
        } },
        { "Z", new bool[][] {
            new bool[] { true, true, true, true, true },
            new bool[] { false, false, false, true, false },
            new bool[] { false, false, true, false, false },
            new bool[] { false, true, false, false, false },
            new bool[] { true, true, true, true, true },
        } },
        { ".", new bool[][] {
            new bool[] { false, false, false, false, false },
            new bool[] { false, false, false, false, false },
            new bool[] { false, false, false, false, false },
            new bool[] { false, false, false, false, false },
            new bool[] { false, false, true, false, false },
        } },
        { "/", new bool[][] {
            new bool[] { false, false, false, false, true },
            new bool[] { false, false, false, true, false },
            new bool[] { false, false, true, false, false },
            new bool[] { false, true, false, false, false },
            new bool[] { true, false, false, false, false },
        } },
        { "%", new bool[][] {
            new bool[] { true, true, true, false, true },
            new bool[] { false, false, true, false, true },
            new bool[] { true, true, true, true, true },
            new bool[] { true, false, true, false, false },
            new bool[] { true, false, true, true, true },
        } },
        { " ", new bool[][] {
            new bool[] { false, false, false, false, false },
            new bool[] { false, false, false, false, false },
            new bool[] { false, false, false, false, false },
            new bool[] { false, false, false, false, false },
            new bool[] { false, false, false, false, false },
        } },
    };

        public static void PlaneSpray(int rand)
        {
            if (Time.time > rah)
            {
                rah = Time.time + 0.05f;
                if (WristMenu.gripDownR)
                {
                    var righthand = PlayerMovement.TrueRightHand();

                    for (int i = 0; i < 7; i++)
                    {
                        whichproj++;

                        Vector3 spawnPosition = PlayerMovement.TrueRightHand().position;

                        if (whichproj == 0)
                        {
                            Vector3 vel = righthand.forward * Time.deltaTime * 1000f;
                            Vector3 randup = righthand.up * Time.deltaTime * Random.Range(-rand, rand);
                            Vector3 randright = righthand.right * Time.deltaTime * Random.Range(-rand, rand);
                            vel += randup + randright;
                            SendThrowablePlane(spawnPosition, vel, Quaternion.identity);
                        }
                        if (whichproj == 1)
                        {
                            Vector3 vel = righthand.forward * Time.deltaTime * 1000f;
                            Vector3 randup = righthand.up * Time.deltaTime * Random.Range(-rand, rand);
                            Vector3 randright = righthand.right * Time.deltaTime * Random.Range(-rand, rand);
                            vel += randup + randright;
                            SendThrowableFatPlane(spawnPosition, vel, Quaternion.identity);
                        }
                        if (whichproj == 2)
                        {
                            Vector3 vel = righthand.forward * Time.deltaTime * 1000f;
                            Vector3 randup = righthand.up * Time.deltaTime * Random.Range(-rand, rand);
                            Vector3 randright = righthand.right * Time.deltaTime * Random.Range(-rand, rand);
                            vel += randup + randright;
                            SendThrowableHeart(spawnPosition, vel, Quaternion.identity);
                            whichproj = -1;
                        }
                    }
                }
                else
                {
                    if (WristMenu.gripDownL)
                    {
                        var righthand = PlayerMovement.TrueLeftHand();

                        for (int i = 0; i < 7; i++)
                        {
                            whichproj++;

                            Vector3 spawnPosition = PlayerMovement.TrueLeftHand().position;

                            if (whichproj == 0)
                            {
                                Vector3 vel = righthand.forward * Time.deltaTime * 1000f;
                                Vector3 randup = righthand.up * Time.deltaTime * Random.Range(-rand, rand);
                                Vector3 randright = righthand.right * Time.deltaTime * Random.Range(-rand, rand);
                                vel += randup + randright;
                                SendThrowablePlane(spawnPosition, vel, Quaternion.identity);
                            }
                            if (whichproj == 1)
                            {
                                Vector3 vel = righthand.forward * Time.deltaTime * 1000f;
                                Vector3 randup = righthand.up * Time.deltaTime * Random.Range(-rand, rand);
                                Vector3 randright = righthand.right * Time.deltaTime * Random.Range(-rand, rand);
                                vel += randup + randright;
                                SendThrowableFatPlane(spawnPosition, vel, Quaternion.identity);
                            }
                            if (whichproj == 2)
                            {
                                Vector3 vel = righthand.forward * Time.deltaTime * 1000f;
                                Vector3 randup = righthand.up * Time.deltaTime * Random.Range(-rand, rand);
                                Vector3 randright = righthand.right * Time.deltaTime * Random.Range(-rand, rand);
                                vel += randup + randright;
                                SendThrowableHeart(spawnPosition, vel, Quaternion.identity);
                                whichproj = -1;
                            }
                        }
                    }
                    else
                        GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void HeartStarMinigun()
        {
            if (WristMenu.gripDownR)
            {
                SendThrowableHeart(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 9000.4f, Quaternion.identity);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void HeartStarSpam()
        {
            if (WristMenu.gripDownR)
            {
                SendThrowableHeart(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward, PlayerMovement.TrueRightHand().rotation);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void HeartStarStill()
        {
            if (WristMenu.gripDownR)
            {
                SendThrowableHeart(PlayerMovement.TrueRightHand().position, new Vector3(0.1f, 0.1f, 0.1f), PlayerMovement.TrueRightHand().rotation);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void FatStill()
        {
            if (WristMenu.gripDownR)
            {
                SendThrowableFatPlane(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward, PlayerMovement.TrueRightHand().rotation);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        static float rah;
        public static float circleRadius = 0.7f; // The radius of the circle
        public static int numberOfObjects = 16; // Number of objects to form the hollow circle

        static int whichproj = -1;

        public static void PaperplaneShotgun()
        {
            if (Time.time > rah)
            {
                rah = Time.time + 0.1f;

                if (WristMenu.gripDownR)
                {
                    for (int i = 0; i < numberOfObjects; i++)
                    {
                        whichproj++;

                        float angle = i * (360f / numberOfObjects) * Mathf.Deg2Rad;

                        Vector3 positionOffset = new Vector3(
                            Mathf.Cos(angle) * circleRadius,
                            Mathf.Sin(angle) * circleRadius,
                            0f
                        );

                        Vector3 spawnPosition = PlayerMovement.TrueRightHand().position + PlayerMovement.TrueRightHand().rotation * positionOffset;

                        if (whichproj == 0)
                        {
                            SendThrowablePlane(spawnPosition, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 5000.4f, Quaternion.identity);
                        }
                        if (whichproj == 1)
                        {
                            SendThrowableFatPlane(spawnPosition, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 5000.4f, Quaternion.identity);
                        }
                        if (whichproj == 2)
                        {
                            SendThrowableHeart(spawnPosition, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 5000.4f, Quaternion.identity);
                            whichproj = -1;
                        }
                    }
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void PlaneWind()
        {
            if (WristMenu.gripDownR)
            {
                //fat

                //-78.038 24.4695 -73.8944
                //-23.9312 9.96 -72.0501



                Vector3 pos = new Vector3(Random.Range(-78, -23), Random.Range(24, 10), Random.Range(-73, -72));

                SendThrowableFatPlane(pos, Vector3.forward * 10f, Quaternion.identity);

                //-78.038 24.4695 -73.8944
                //-23.9312 9.96 -72.0501

                Vector3 pos2 = new Vector3(Random.Range(-78, -23), Random.Range(24, 10), Random.Range(-73, -72));

                SendThrowablePlane(pos2, Vector3.forward * 10f, Quaternion.identity);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        static float planeshit;

        public static void SpazPlanes()
        {
            if (WristMenu.gripDownR)
            {
                //fat



                Quaternion e = Random.rotationUniform;

                Vector3 behindDirection = e * -Vector3.forward;  // Move backward in local space
                float speed = 5f;
                Vector3 behindVelocity = behindDirection * speed;

                SendThrowableFatPlane(PlayerMovement.TrueRightHand().position, behindVelocity, e);

                //heart

                Quaternion e2 = Random.rotationUniform;

                Vector3 behindDirection2 = e2 * -Vector3.forward;  // Move backward in local space
                float speed2 = 5f;
                Vector3 behindVelocity2 = behindDirection2 * speed2;

                SendThrowableHeart(PlayerMovement.TrueRightHand().position, behindVelocity2, e2);

            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }


        public static void PlaneStill()
        {
            if (WristMenu.gripDownR)
            {
                SendThrowablePlane(PlayerMovement.TrueRightHand().position, new Vector3(0.1f, 0.1f, 0.1f), PlayerMovement.TrueRightHand().rotation);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void FatPlaneSpam()
        {
            if (WristMenu.gripDownR)
            {
                SendThrowableFatPlane(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 5f, Quaternion.identity);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void FatPlaneMinigun()
        {
            if (WristMenu.gripDownR)
            {
                SendThrowableFatPlane(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 9000.4f, Quaternion.identity);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void PaperplaneGun()
        {
            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered)
            {
                Vector3 funn = (data.hitPosition - GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position).normalized;
                float penis = 5f;
                funn *= penis;
                SendThrowablePlane(PlayerMovement.TrueRightHand().position, funn, Quaternion.identity);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void HeartStarGun()
        {
            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered)
            {
                Vector3 funn = (data.hitPosition - GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position).normalized;
                float penis = 5f;
                funn *= penis;
                SendThrowableHeart(PlayerMovement.TrueRightHand().position, funn, Quaternion.identity);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void FatPlaneGun()
        {
            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered)
            {
                Vector3 funn = (data.hitPosition - GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position).normalized;
                float penis = 5f;
                funn *= penis;
                SendThrowableFatPlane(PlayerMovement.TrueRightHand().position, funn, Quaternion.identity);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static double size;

        public static void DSizeChanger()
        {
            if (size != 1f)
            {
                if (Time.time > ShootDelay)
                {
                    ShootDelay = Time.time + 1.05f;
                    size = 1f;
                }
            }
        }

        public static void SizeChanger()
        {
            size = Mathf.Clamp((float)size, 0.1f, 10f);
            if (WristMenu.triggerDownR && Time.time > cooldown2)
            {
                if (size >= 10f)
                {
                    size = 9.9f;
                    return;
                }
                size += 0.1;
                cooldown2 = Time.time + 0.02f;
                HarmonyLib.Traverse.Create(GorillaLocomotion.GTPlayer.Instance).Field("nativeScale").SetValue((float)size);
            }
            if (WristMenu.triggerDownL && Time.time > cooldown2)
            {
                if (size <= 0.1f)
                {
                    size = 0.1f;
                    return;
                }
                size -= 0.1;
                cooldown2 = Time.time + 0.02f;
                HarmonyLib.Traverse.Create(GorillaLocomotion.GTPlayer.Instance).Field("nativeScale").SetValue((float)size);
            }
            if (WristMenu.ybuttonDown)
            {
                size = 1f;
                HarmonyLib.Traverse.Create(GorillaLocomotion.GTPlayer.Instance).Field("nativeScale").SetValue((float)size);
            }
            if (Time.time > ShootDelay)
            {
                ShootDelay = Time.time + 1.01f;
                HarmonyLib.Traverse.Create(GorillaLocomotion.GTPlayer.Instance).Field("nativeScale").SetValue((float)size);
            }
            size = Mathf.Clamp((float)size, 0.1f, 10f);
        }


        public static void MaxQuestPoints()
        {
            if (GorillaTagger.Instance != null && GorillaTagger.Instance.offlineVRRig != null)
                GorillaTagger.Instance.offlineVRRig.SetQuestScore(99999);
        }

        public static void MaxRank()
        {
            if (GorillaTagger.Instance != null && GorillaTagger.Instance.myVRRig != null)
                GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateRankedInfo", RpcTarget.All, 4000f, 7, 7);
        }



















        public static void SnowballSpeedboostGiverGun()
        {
            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered && Time.time > cooldown)
            {
                EnableSnowballs();
                PhotonNetwork.SerializationRate = 30;
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = data.lockedPlayer.transform.position - new Vector3(0, 4f, 0);

                Vector3 pos = data.lockedPlayer.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f)).normalized / 1.7f;
                Vector3 vel = new Vector3(0f, -500f, 0f);

                object[] source = new object[]
                {
                   pos, vel, 5f
                };
                object[] eventContent = source.Prepend(StaticHash.Compute("SnowballThrowable", "SnowballThrowEventRight", GorillaTagger.Instance.myVRRig.ViewID.ToString())).ToArray<object>();
                PhotonNetwork.RaiseEvent(176, eventContent, new RaiseEventOptions
                {
                    TargetActors = new int[] { data.lockedPlayer.Creator.ActorNumber }
                }, SendOptions.SendReliable);
                NewFlusher();
                cooldown = Time.time + 0.2f;
            }
            if (!data.isTriggered)
            {
                PhotonNetwork.SerializationRate = 10;
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void FlingAllFromPoint()
        {
            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered && Time.time > cooldown)
            {
                EnableSnowballs();
                object[] source = new object[]
                {
                   data.hitPosition + new Vector3(0f, 9f, 0f), new Vector3(0f, -9000f, 0f), 130f
                };
                object[] eventContent = source.Prepend(StaticHash.Compute("SnowballThrowable", "SnowballThrowEventRight", GorillaTagger.Instance.myVRRig.ViewID.ToString())).ToArray<object>();
                PhotonNetwork.RaiseEvent(176, eventContent, new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                }, SendOptions.SendReliable);
                cooldown = Time.time + 0.2f;
            }
        }

        public static void FlingGun2()
        {
            var data = GunLib.ShootLock();
            if (data.isShooting && data.isTriggered && Time.time > cooldown)
            {
                EnableSnowballs();
                PhotonNetwork.SerializationRate = 30;
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = data.lockedPlayer.transform.position - new Vector3(0, 4f, 0);
                object[] source = new object[]
                {
                    data.hitPosition + new Vector3(0f, 4f, 0f), new Vector3(0f, -50, 0f), 120f
                };
                object[] eventContent = source.Prepend(StaticHash.Compute("SnowballThrowable", "SnowballThrowEventRight", GorillaTagger.Instance.myVRRig.ViewID.ToString())).ToArray<object>();
                PhotonNetwork.RaiseEvent(176, eventContent, new RaiseEventOptions
                {
                    TargetActors = new int[] { data.lockedPlayer.Creator.ActorNumber }
                }, SendOptions.SendReliable);
                cooldown = Time.time + 0.2f;
            }
        }

        public static void EnableSnowballs()
        {
            GorillaTagger.Instance.offlineVRRig.RightThrowableProjectileIndex = 0;
            GorillaTagger.Instance.offlineVRRig.LeftThrowableProjectileIndex = 0;
            var go = GameObject.Find("Local Gorilla Player/RigAnchor/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/TransferrableItemRightHand/GrowingSnowballRightAnchor(Clone)/LMACF. RIGHT.");
            //var go2 = GameObject.Find("Local Gorilla Player/RigAnchor/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L/TransferrableItemLeftHand/GrowingSnowballLeftAnchor(Clone)/LMACF. LEFT.");
            GrowingSnowballThrowable goOb = null;
            GrowingSnowballThrowable goOb2 = null;
            if (!go)
            {
                go = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/GrowingSnowballRightAnchor(Clone)/LMACF. RIGHT.");
                go.gameObject.SetActive(true);
                return;
            }
            else
            {
                goOb = go.GetComponent<GrowingSnowballThrowable>();
            }
            /*
            if (!go2)
            {
               // go2 = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/GrowingSnowballRightAnchor(Clone)/LMACF. LEFT.");
                //go2.gameObject.SetActive(true);
                return;
            }
            else
            {
                goOb2 = go2.GetComponent<GrowingSnowballThrowable>();
            }
            */
            goOb.IncreaseSize(5);
            //goOb2.IncreaseSize(5);
        }

        public static void DisableSnowballs()
        {
            GameObject go = null;
            try
            {
                go = GameObject.Find("Local Gorilla Player/RigAnchor/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/TransferrableItemRightHand/GrowingSnowballRightAnchor(Clone)/LMACF. RIGHT.").gameObject;
            }
            catch { go = null; }

            GrowingSnowballThrowable goOb = null;
            if (!go)
            {
                go = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/GrowingSnowballRightAnchor(Clone)/LMACF. RIGHT.");
                go.GetComponent<GrowingSnowballThrowable>().SetSnowballActiveLocal(false);
                return;
            }
            else
            {
                goOb = go.GetComponent<GrowingSnowballThrowable>();
                goOb.SetSnowballActiveLocal(false);
            }

            GameObject go2 = null;
            try
            {
                go2 = GameObject.Find("Local Gorilla Player/RigAnchor/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L/TransferrableItemLeftHand/GrowingSnowballLeftAnchor(Clone)/LMACE. LEFT.").gameObject;
            }
            catch { go2 = null; }

            GrowingSnowballThrowable goOb2 = null;
            if (!go2)
            {
                go2 = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/GrowingSnowballLeftAnchor(Clone)/LMACE. LEFT.");
                go2.GetComponent<GrowingSnowballThrowable>().SetSnowballActiveLocal(false);
                return;
            }
            else
            {
                goOb2 = go2.GetComponent<GrowingSnowballThrowable>();
                goOb2.SetSnowballActiveLocal(false);
            }
        }


        public static float sillyfloat;
        static bool sillybool;

        private static float CptiDelay = 0f;



        public static List<BuilderPiece> pieces = new List<BuilderPiece>();

        public static List<BuilderPiece> GetPieces()
        {

            if (Time.time > archfloat)
            {
                archfloat = Time.time + 1f;
                pieces.Clear();
            }
            if (pieces.Count < 2)
            {
                foreach (BuilderPiece r in UnityEngine.Object.FindObjectsOfType<BuilderPiece>())
                {
                    if (r.gameObject.activeSelf)
                        pieces.Add(r);
                }
            }
            return pieces;
        }


        
        static float ShootDelay;
        static bool Timeout;

       
        

        static float breaak;
        static float a;
        public enum PieceTypes
        {
            CastleTrapDoor = -57667458,
            CastleBallista = -1310012383,
            CastleDoorFunctional = -2116490732,
            CastleDoorway01 = -1961511959,
            CastleLadder01 = 1459246571,
            CastlePillar01 = 1008893018,
            CastleRoofCap2x2 = 38505905,
            CastleRoofCap4x4 = 2104851213,
            CastleRampartCornerThin2x2_01 = 405537871,
            CastleRampart1x8_01 = -2031647881,
            CastleRampart2x6_01 = -1508642259,
            CastleRampart1x4_01 = -666919797,
            CastleRampart1x3_02 = -1206998199,
            CastleRampart1x3_01 = 19594543,
            CastleFlag = 948224013,
            CastleStatue02 = 1477544431,
            CastleStatue01 = 1102387446,
            CastleLampPost01 = 654087561,
            CastleCart01 = 2080623204,
            CastleAwning01 = -297072615,
            CastleStairs01 = 1685601286,
            ArmShelf = 1858470402,
            Terrain8x16 = -566818631,
            Terrain8x8 = 1925587737,
            Terrain4x8 = -1441835191,
            TerrainShort8x16 = -1324502924,
            TerrainShort8x8 = 111152940,
            TerrainShort4x8 = 798264081,
            TerrainQuarter8x16 = -1821684029,
            TerrainQuarter8x8 = 1895524638,
            TerrainQuarter4x8 = 1961336659,
            RoofCap4x4x6 = 539529939,
            RoofCap4x4x3 = -1535427925,
            RoofCap2x2 = 1210710592,
            RoofCap1x1 = 1228919111,
            Slide2x6 = 252298128,
            Ladder01 = -648273975,
            Floor02 = 1120512569,
            Wind01 = 532163265,
            RopeSwing01 = -845420418,
            TreeTop = 1700948013,
            TreeMiddle = 2059548340,
            TreeBottom = -1447051713,
            Fence01 = 1134055607,
            Roof02 = 1700655257,
            ColumnShortSideBumps = -1724819324,
            ColumnShort01 = -1218055069,
            WallShort03 = 251444537,
            WallShort1x3 = -1446121736,
            WallShort02 = -1927069002,
            WallShort01 = -385891195,
            Stairs01 = -196038879,
            Roof01 = -993249117,
            Window1x4 = 1145900217,
            Window1x3 = 1859614656,
            Doorway01 = 1821589092,
            Column01 = 776850392,
            Wall02 = 661312857,
            Wall1x3 = 1701825380,
            Wall03 = -1621444201,
            Wall01 = 1924370326,
            Beam03 = -1193326485,
            Beam02 = -1194390666,
            Beam01 = -751675075,
            BigFloor = -902878798,
            IceBig = 439542361,
            Plot = -1
        }

        public static void KickOnReport()
        {
            UnityEngine.Debug.developerConsoleEnabled = true;
            foreach (var report in GorillaScoreboardTotalUpdater.instance.reportDict)
            {
                if (!report.Value.pressedReport) continue;

                int reportedActorNumber = report.Key;
                UnityEngine.Debug.Log($"Found reported player with ActorNumber: {reportedActorNumber}");

                NetPlayer reportedPlayer = NetworkSystem.Instance.AllNetPlayers
                    .FirstOrDefault(player => player.ActorNumber == reportedActorNumber);

                if (reportedPlayer == null)
                {
                    UnityEngine.Debug.LogError($"Reported player with ActorNumber {reportedActorNumber} not found in the network.");
                    continue;
                }

                UnityEngine.Debug.Log($"Reported player found: {reportedPlayer.ActorNumber}");

                VRRig targetVRRig = RigShit.GetRigFromNetPlayer(reportedPlayer);

                if (targetVRRig == null)
                {
                    UnityEngine.Debug.LogError($"VRRig not found for player with ActorNumber: {reportedActorNumber} If this happens one more time im gonna shoot myself.");
                    continue;
                }

                UnityEngine.Debug.Log($"VRRig found for reported player: {reportedActorNumber}");

                var networkView = RigShit.GetViewFromRig(targetVRRig);

                if (networkView == null)
                {
                    UnityEngine.Debug.LogError($"PhotonView/NetworkView not found for VRRig associated with ActorNumber: {reportedActorNumber}");
                    continue;
                }

                UnityEngine.Debug.Log($"PhotonView/NetworkView found. Sending RPCs for ActorNumber: {reportedActorNumber}");

                try
                {
                    if (Time.time > ihave2kidsinmycloset)
                    {
                        ihave2kidsinmycloset = Time.time + 0.1f;
                        RequestableOwnershipGuard requestable = GetRandomGuard();
                        if (requestable.currentState == NetworkingState.IsClient)
                            requestable.photonView.RPC("OwnershipRequested", requestable.actualOwner.GetPlayerRef(), new object[] { requestable.ownershipRequestNonce });

                        if (requestable.gameObject != null && requestable.gameObject.activeSelf)
                        {
                            if (requestable.currentState == NetworkingState.IsOwner)
                            {
                                var view = Traverse.Create(requestable).Property("netView").GetValue<NetworkView>();
                                if (view != null)
                                {
                                    requestable.TransferOwnership(networkView.Owner, Guid.NewGuid().ToString());
                                    view.SendRPC("OwnershipRequested", networkView.Owner, new object[] { "TTTTTTTTTTTT".PadRight(1962) });
                                }
                            }
                        }
                        NewFlusher();
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"Error while sending SHITPCS for ActorNumber {reportedActorNumber}: {ex.Message}");
                }

                break;
            }
        }

        public static void SmallGuard()
        {
            if (sillyfloat < Time.time)
            {
                sillyfloat = Time.time + 4f;
                if (PhotonNetwork.IsMasterClient)
                {
                    foreach (GorillaGuardianZoneManager gorillaGuardianZoneManager in GorillaGuardianZoneManager.zoneManagers)
                    {
                        if (gorillaGuardianZoneManager.enabled)
                        {
                            Back.GetButton("Always Guardian").enabled = false;

                            if (gorillaGuardianZoneManager.CurrentGuardian != NetworkSystem.Instance.LocalPlayer)
                            {
                                NotifiLib.SendNotification("Activated Small Guardian, you are guardian! Just small...");
                                Traverse.Create(gorillaGuardianZoneManager).Field("guardianPlayer").SetValue(NetworkSystem.Instance.LocalPlayer);
                            }
                        }
                    }
                }
                else
                {
                    NotifiLib.SendNotification("<color=blue>[genesis]</color> become master!");
                    Back.GetButton("Small Guardian").enabled = false;
                }
            }
        }

        public static void MuteAll()
        {
            foreach (Player p in PhotonNetwork.PlayerListOthers)
            {
                var pp = RigShit.GetViewFromPlayer(p);
                pp.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                PhotonNetwork.Destroy(pp);
            }

            NewFlusher();
        }

        private static void CalcPieceLocalPosAndRot(Vector3 worldPosition, Quaternion worldRotation, Transform attachPoint, out Vector3 localPosition, out Quaternion localRotation)
        {
            Quaternion rotation = attachPoint.transform.rotation;
            Vector3 position = attachPoint.transform.position;
            localRotation = Quaternion.Inverse(rotation) * worldRotation;
            localPosition = Quaternion.Inverse(rotation) * (worldPosition - position);
        }



        static float blocksDelay2;
        static float blocksDelay;

        public static List<BuilderPiece> p = new List<BuilderPiece>();

        public static int savedPiece;

        

        static object[] sendEventData = new object[3];

        internal static void SendEvent(in byte code, in object evData, bool reliable)
        {
            SendEvent(code, evData, NetworkSystemRaiseEvent.neoOthers, reliable);
        }

        internal static void SendEvent(in byte code, in object evData, in NetEventOptions neo, bool reliable)
        {
            sendEventData[0] = NetworkSystem.Instance.ServerTimestamp;
            sendEventData[1] = code;
            sendEventData[2] = evData;
            NetworkSystemRaiseEvent.RaiseEvent(3, sendEventData, neo, reliable);
        }

        static object[] reportTouchSendData = new object[1];

        public static void Tst()
        {
            reportTouchSendData[0] = new Vector3(0, float.NaN, 0);
            byte b = 8;
            object obj = reportTouchSendData;
            SendEvent(b, obj, false);

        }

       

        public static bool didThat = false;

        public static bool sendElfNotificatio = true;

        public static void ElfSpamGun(int speed)
        {
            var data = GunLib.Shoot();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-51.4897f, 16.9286f, -120.1083f);
                GorillaTagger.Instance.offlineVRRig.inTryOnRoom = true;



                Vector3 funn = (data.hitPosition - GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position).normalized;
                float penis = speed;
                funn *= penis;

                if (didThat == false)
                {
                    UnityEngine.Debug.Log("gang");
                    CosmeticsController.instance.ApplyCosmeticItemToSet(GorillaTagger.Instance.offlineVRRig.tryOnSet, CosmeticsController.instance.GetItemFromDict("LMANE."), false, false);
                    CosmeticsController.instance.UpdateWornCosmetics(true);
                }


                ElfLauncher elfLauncher = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/RigAnchor/rig/body/TransferrableItemRightShoulder/DropZoneAnchor/ElfLauncherAnchor(Clone)/LMANE.").GetComponent<ElfLauncher>();

                if (elfLauncher != null)
                {
                    didThat = true;
                    object[] eventContent = new object[]
                            {
                                    (int)Traverse.Create(((RubberDuckEvents)Traverse.Create(elfLauncher).Field("_events").GetValue()).Activate).Field("_eventId").GetValue(),
                                    GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position,
                                    funn
                            };
                    PhotonNetwork.RaiseEvent(176, eventContent, new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.All
                    }, new SendOptions
                    {
                        Reliability = false,
                        Encrypt = true
                    });
                    UnityEngine.Debug.Log("done");
                }
            }
            else
            {
                didThat = false;
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                didElf = false;
            }
        }

        public static Vector3 RainPosGen(int dist = 3)
        {
            float randomX = UnityEngine.Random.Range(-dist, dist);
            float randomZ = UnityEngine.Random.Range(-dist, dist);
            Vector3 newPosition = GorillaLocomotion.GTPlayer.Instance.transform.position + new Vector3(randomX, dist, randomZ);
            newPosition.y = Mathf.Min(newPosition.y, GorillaLocomotion.GTPlayer.Instance.transform.position.y + dist);
            return newPosition;
        }

        public static Vector3 RainForGen()
        {
            //-74.4588 34.1516 -49.6544
            //-40.8105 37.3303 -77.8638

            float randomX = UnityEngine.Random.Range(-74.4588f, -40.8105f);
            float randomZ = UnityEngine.Random.Range(-49.6544f, -77.8638f);
            Vector3 newPosition = new Vector3(randomX, 35f, randomZ);
            return newPosition;
        }

        public static Vector3 FountainGen()
        {
            float minSpeed = 1f;
            float maxSpeed = 2f;
            float upwardForce = 2f;

            Vector3 randomDirection = UnityEngine.Random.onUnitSphere;

            float randomSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);

            randomDirection.y += upwardForce;

            randomDirection.Normalize();
            Vector3 randomVelocity = randomDirection * randomSpeed;

            return randomVelocity;
        }

        public static void ElfSpam(Vector3 pos, Vector3 vel, bool activation, int amount = 1, bool others = false)
        {
            if (activation)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-51.4897f, 16.9286f, -120.1083f);
                GorillaTagger.Instance.offlineVRRig.inTryOnRoom = true;

                if (didThat == false)
                {
                    UnityEngine.Debug.Log("gang");
                    CosmeticsController.instance.ApplyCosmeticItemToSet(GorillaTagger.Instance.offlineVRRig.tryOnSet, CosmeticsController.instance.GetItemFromDict("LMANE."), false, false);
                    CosmeticsController.instance.UpdateWornCosmetics(true);
                }

                ElfLauncher elfLauncher = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/RigAnchor/rig/body/TransferrableItemRightShoulder/DropZoneAnchor/ElfLauncherAnchor(Clone)/LMANE.").GetComponent<ElfLauncher>();

                if (elfLauncher != null)
                {
                    didThat = true;
                    for (int i = 0; i < amount; i++)
                    {
                        object[] eventContent = new object[]
                            {
                                    (int)Traverse.Create(((RubberDuckEvents)Traverse.Create(elfLauncher).Field("_events").GetValue()).Activate).Field("_eventId").GetValue(),
                                    pos,
                                    vel
                            };
                        PhotonNetwork.RaiseEvent(176, eventContent, new RaiseEventOptions
                        {
                            Receivers = ReceiverGroup.All
                        }, new SendOptions
                        {
                            Reliability = false,
                            Encrypt = true
                        });
                    }
                }
            }
            else
            {
                didThat = false;
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                //didThat = false;
            }
        }

        static bool didElf;

        public static void ElfCrash(bool activation)
        {
            if (activation)
            {
                if (Time.time > dumbdelay)
                {
                    dumbdelay = Time.time + 0.3f;
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-51.4897f, 16.9286f, -120.1083f);
                    GorillaTagger.Instance.offlineVRRig.inTryOnRoom = true;

                    if (didThat == false)
                    {
                        UnityEngine.Debug.Log("gang");
                        CosmeticsController.instance.ApplyCosmeticItemToSet(GorillaTagger.Instance.offlineVRRig.tryOnSet, CosmeticsController.instance.GetItemFromDict("LMANE."), false, false);
                        CosmeticsController.instance.UpdateWornCosmetics(true);
                    }

                    ElfLauncher elfLauncher = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/RigAnchor/rig/body/TransferrableItemRightShoulder/DropZoneAnchor/ElfLauncherAnchor(Clone)/LMANE.").GetComponent<ElfLauncher>();


                    if (elfLauncher != null)
                    {
                        didThat = true;
                        NewFlusher();
                        for (int j = 0; j < 175; j++)
                        {
                            object[] eventContent = new object[]
                            {
                                    (int)Traverse.Create(((RubberDuckEvents)Traverse.Create(elfLauncher).Field("_events").GetValue()).Activate).Field("_eventId").GetValue(),
                                    new Vector3(float.NaN, float.NaN, float.NaN),
                                    Vector3.down
                            };
                            PhotonNetwork.RaiseEvent(176, eventContent, new RaiseEventOptions
                            {
                                Receivers = ReceiverGroup.Others
                            }, new SendOptions
                            {
                                Reliability = false,
                                Encrypt = true
                            });
                        }

                    }
                }
            }
            else
            {
                didThat = false;
                didElf = false;
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void ElfCrashGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > dumbdelay)
                {
                    dumbdelay = Time.time + 0.3f;
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-51.4897f, 16.9286f, -120.1083f);
                    GorillaTagger.Instance.offlineVRRig.inTryOnRoom = true;

                    if (didThat == false)
                    {
                        UnityEngine.Debug.Log("gang");
                        CosmeticsController.instance.ApplyCosmeticItemToSet(GorillaTagger.Instance.offlineVRRig.tryOnSet, CosmeticsController.instance.GetItemFromDict("LMANE."), false, false);
                        CosmeticsController.instance.UpdateWornCosmetics(true);
                    }

                    ElfLauncher elfLauncher = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/RigAnchor/rig/body/TransferrableItemRightShoulder/DropZoneAnchor/ElfLauncherAnchor(Clone)/LMANE.").GetComponent<ElfLauncher>();

                    if (elfLauncher != null)
                    {
                        TransferrableObject transferrableObject = (TransferrableObject)Traverse.Create(elfLauncher).Field("parentHoldable").GetValue();
                        if (transferrableObject.IsLocalObject())
                        {
                            didThat = true;
                            NewFlusher();
                            for (int j = 0; j < 175; j++)
                            {
                                object[] eventContent = new object[]
                                {
                                (int)Traverse.Create(((RubberDuckEvents)Traverse.Create(elfLauncher).Field("_events").GetValue()).Activate).Field("_eventId").GetValue(),
                                new Vector3(float.NaN, float.NaN, float.NaN),
                                Vector3.down
                                };
                                PhotonNetwork.RaiseEvent(176, eventContent, new RaiseEventOptions
                                {
                                    TargetActors = new int[] { RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber }
                                }, new SendOptions
                                {
                                    Reliability = false,
                                    Encrypt = true
                                });
                            }

                        }
                    }
                }
            }
            else
            {
                didThat = false;
                didElf = false;
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }







        public static void StackCosmetics()
        {
            Timde += 1f;
            if (Timde > 60f)
            {
                archiveCosmetics = new string[] { "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", };

                CosmeticsController.instance.currentWornSet = new CosmeticsController.CosmeticSet(new string[] { "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI." }, CosmeticsController.instance);
                GorillaTagger.Instance.offlineVRRig.cosmeticSet = new CosmeticsController.CosmeticSet(new string[] { "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI." }, CosmeticsController.instance);

                GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryOn", RpcTarget.All, new string[] { "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI.", "LBAAI." }, CosmeticsController.instance.tryOnSet.ToDisplayNameArray());

                foreach (CheckoutCartButton m in GameObject.FindObjectsOfType<CheckoutCartButton>())
                {
                    if (m.isOn)
                    {
                        m.ButtonActivationWithHand(false);
                    }
                }
                foreach (TryOnBundleButton m in GameObject.FindObjectsOfType<TryOnBundleButton>())
                {
                    if (m.isOn)
                    {
                        m.ButtonActivationWithHand(false);
                    }
                }
                foreach (FittingRoomButton m in GameObject.FindObjectsOfType<FittingRoomButton>())
                {
                    if (m.isOn)
                    {
                        m.ButtonActivationWithHand(false);
                    }
                }
                Timde = 0f;
            }
        }
        public static float Timde = 60f;

        public static void LiveEvent(bool enable)
        {
            if (enable)
            {
                var e = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/GreyZoneManager").GetComponent<GreyZoneManager>();
                Traverse.Create(e).Field("photonConnectedDuringActivation").SetValue(true);
                Traverse.Create(e).Field("greyZoneAvailableDayOfYear").SetValue((int)0);
                e.ActivateGreyZoneAuthority();
            }
            if (!enable)
            {
                var e = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Halloween2024_PersistentObjects/GreyZoneManager").GetComponent<GreyZoneManager>();
                Traverse.Create(e).Field("photonConnectedDuringActivation").SetValue(true);
                Traverse.Create(e).Field("greyZoneAvailableDayOfYear").SetValue((int)0);
                e.DeactivateGreyZoneAuthority();
            }
        }





































        public static List<FittingRoomButton> savedButtons = new List<FittingRoomButton>();
        public static List<TryOnBundleButton> savedBundles = new List<TryOnBundleButton>();
        public static void saveTryon()
        {
            savedButtons.Clear();
            savedBundles.Clear();
            foreach (FittingRoomButton b in UnityEngine.Object.FindObjectsOfType<FittingRoomButton>())
            {
                if (b.gameObject.activeSelf)
                {
                    if (b.isOn)
                    {
                        savedButtons.Add(b);
                    }
                }
            }
            foreach (TryOnBundleButton b in UnityEngine.Object.FindObjectsOfType<TryOnBundleButton>())
            {
                if (b.gameObject.activeSelf)
                {
                    if (b.isOn)
                    {
                        savedBundles.Add(b);
                    }
                }
            }
            NotifiLib.SendNotification("Saved all of your currently enabled tryon cosmetics!");
        }
        public static async void loadTryon()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-51.7329f, 17.7275f, -119.8192f);
            await Task.Delay(500);
            archiveCosmetics = CosmeticsController.instance.currentWornSet.ToDisplayNameArray();
            foreach (FittingRoomButton b in savedButtons)
            {
                b.debounceTime = 0f;
                if (b.isOn)
                {
                    b.ButtonActivationWithHand(false);
                    b.ButtonActivationWithHand(false);
                }
                if (!b.isOn)
                {
                    b.ButtonActivationWithHand(false);
                }
            }
            foreach (TryOnBundleButton b in savedBundles)
            {
                b.debounceTime = 0f;
                if (b.isOn)
                {
                    b.ButtonActivationWithHand(false);
                    b.ButtonActivationWithHand(false);
                }
                if (!b.isOn)
                {
                    b.ButtonActivationWithHand(false);
                }
            }
            CosmeticsController.instance.currentWornSet = new CosmeticsController.CosmeticSet(new string[] { }, CosmeticsController.instance);
            //GorillaTagger.Instance.offlineVRRig.cosmeticSet = new CosmeticsController.CosmeticSet(new string[] { }, CosmeticsController.instance);
            GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", RpcTarget.All, new object[] { new string[16] { "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU." }, new string[16] { "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU." } });
            await Task.Delay(250);
            GorillaTagger.Instance.offlineVRRig.enabled = true;
            NotifiLib.SendNotification("<color=blue>[GENESIS]</color> Finished! Everyone can see your cosmetics!");
        }

        public static async void unloadTryon()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-51.7329f, 17.7275f, -119.8192f);
            await Task.Delay(250);

            CosmeticsController.instance.currentWornSet = new CosmeticsController.CosmeticSet(archiveCosmetics, CosmeticsController.instance);
            GorillaTagger.Instance.offlineVRRig.cosmeticSet = new CosmeticsController.CosmeticSet(archiveCosmetics, CosmeticsController.instance);

            try
            {
                GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", RpcTarget.All, new object[] { archiveCosmetics, CosmeticsController.instance.tryOnSet.ToDisplayNameArray() });
            }
            catch { }
            await Task.Delay(250);
            GorillaTagger.Instance.offlineVRRig.enabled = true;
            NotifiLib.SendNotification("<color=blue>[GENESIS]</color> Finished! Your cosmetics have been removed!");
        }

        static private GameObject vrrigCacheObject;

        public static void emableskib()
        {
            if (CrashDelay < Time.time)
            {
                CrashDelay = Time.time + 0.1f;
                byte eventCode = 9;
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                WebFlags flags = new WebFlags(1);
                raiseEventOptions.Flags = flags;
                object[] eventContent = new object[0];
                PhotonNetwork.RaiseEvent(eventCode, eventContent, raiseEventOptions, SendOptions.SendReliable);

            }
        }

        private static readonly RaiseEventOptions gReceiversOthers = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others
        };

        public static void emableskib3()
        {
            if (WristMenu.triggerDownL)
            {
                var trig = GameObject.Find("SmokeTrailPotStove_Functional Variant/StoveFlame_FX (1)").GetComponent<TriggerOnJump>();
                var _events = Traverse.Create(trig).Field("_events").GetValue<RubberDuckEvents>();
                _events.Activate.RaiseOthers(Array.Empty<object>());
                _events.Activate.RaiseOthers(Array.Empty<object>());
                _events.Activate.RaiseOthers(Array.Empty<object>());
            }
        }

        public static void FaggotLag(int f, float c, Player p = null)
        {
            NewFlusher();
            if (Time.time > cooldown2)
            {
                cooldown2 = Time.time + c;
                GameEntityManager GameEntityManager = GameObject.Find("Networking Scripts/GhostReactorManager").GetComponent<GameEntityManager>();
                for (int i = 0; i < f; i++)
                {
                    if (p != null)
                        PhotonNetwork.NetworkingClient.LoadBalancingPeer.OpRaiseEvent(3, new ExitGames.Client.Photon.Hashtable(), new RaiseEventOptions { TargetActors = new int[] { p.ActorNumber } }, SendOptions.SendUnreliable);
                    else
                        PhotonNetwork.NetworkingClient.LoadBalancingPeer.OpRaiseEvent(3, new ExitGames.Client.Photon.Hashtable(), new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
                }
            }
        }

        public static void FaggotLagGun(int f, float c)
        {
            var data = GunLib.ShootLock();
            if (data.isLocked && data.isShooting)
            {
                if (data.lockedPlayer != GorillaTagger.Instance.offlineVRRig)
                {
                    FaggotLag(f, c, RigShit.GetPlayerFromRig(data.lockedPlayer));
                }
            }
        }
        public static void c204(float delayy, float foramount)
        {
            if (Time.time > cooldown)
            {
                cooldown = Time.time + delayy;
                for (int i = 0; i < foramount; i++)
                {
                    PhotonNetwork.NetworkingClient.OpRaiseEvent(204, new object[] {
                        -357418581,
                        76
                    }, new RaiseEventOptions { CachingOption = EventCaching.DoNotCache, Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
                }
            }
            NewFlusher();
        }

        public static void c204G(float delayy, float foramount)
        {
            var data = GunLib.ShootLock();
            if (data.isLocked)
            {
                if (!data.lockedPlayer.isOfflineVRRig)
                {
                    if (Time.time > cooldown)
                    {
                        cooldown = Time.time + delayy;
                        for (int i = 0; i < foramount; i++)
                        {
                            PhotonNetwork.NetworkingClient.OpRaiseEvent(204, new object[] {
                                -357418581,
                                76
                            }, new RaiseEventOptions { CachingOption = EventCaching.DoNotCache, TargetActors = new int[] { RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber } }, SendOptions.SendReliable);
                        }
                    }
                }
                NewFlusher();
            }
        }

        public static void c202()
        {
            if (CrashDelay < Time.time)
            {
                CrashDelay = Time.time + 0.07f;
                foreach (Player p in PhotonNetwork.PlayerListOthers)
                {
                    byte eventCode = 202;
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                    raiseEventOptions.Receivers = ReceiverGroup.Others;

                    PhotonNetwork.NetworkingClient.OpRaiseEvent(eventCode, null, raiseEventOptions, SendOptions.SendUnreliable);
                }
            }
        }


        public static bool servertimestampPatch = false;

        static void ClearBuffer()
        {
            PhotonNetwork.NetworkingClient.OpRaiseEvent(
                200, //rpc code
                null, //no datat needed
                new RaiseEventOptions
                {
                    CachingOption = EventCaching.RemoveCache, //remove all cache related with 200 / rpc
                    TargetActors = new int[] { PhotonNetwork.LocalPlayer.ActorNumber } //clears ours nothinj else
                },
                SendOptions.SendReliable // :thumbsup:
            );
        }

        static float dddddddddflaot;

        public static void emableskib2()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > dddddddddflaot)
                {
                    dddddddddflaot = Time.time + 7f;
                    KickView(data.lockedPlayer);
                }
            }
        }

        static PhotonHandler savedHandler = null;

        public static async void KickView(VRRig rig)
        {
            if (savedHandler == null)
            {
                savedHandler = GameObject.Find("PhotonMono").GetComponent<PhotonHandler>();
            }
            if (savedHandler != null && rig != null && !rig.isOfflineVRRig)
            {
                NotifiLib.SendNotification("Kicking... Please don't hold the gun on them!");

                PhotonView photonView = RigShit.GetViewFromRig(rig);
                Player kickingPlayer = rig.Creator.GetPlayerRef();

                Traverse.Create(savedHandler)
                    .Field("nextSendTickCountOnSerialize")
                    .SetValue((int)(Time.realtimeSinceStartup * 9999));
                Traverse.Create(savedHandler)
                    .Field("SendAsap")
                    .SetValue(true);
                Traverse.Create(savedHandler)
                    .Field("MaxDatagrams")
                    .SetValue(255);

                await Task.Delay(600);

                for (int i = 0; i < 3950; i++)
                {
                    photonView.RpcSecure("RPC_RequestMaterialColor", RpcTarget.Others, true, new object[] { kickingPlayer });
                }

                await Task.Delay(5000);

                PhotonNetwork.SendAllOutgoingCommands();
                NewFlusher();

                NotifiLib.SendNotification("The player should have been kicked!");

                await Task.Delay(1000);

                Traverse.Create(savedHandler)
                   .Field("nextSendTickCountOnSerialize")
                   .SetValue((int)(Time.realtimeSinceStartup * 1000f));


                photonView = null;
                kickingPlayer = null;
            }
        }

        public static void MasterInfoTest()
        {
            foreach (FortuneTeller t in UnityEngine.Object.FindObjectsOfType<FortuneTeller>())
            { 
                var latestFortune = Traverse.Create(t).Field("latestFortune").GetValue<FortuneResults.FortuneResult>();

                t.photonView.RPC("TriggerNewFortuneRPC", RpcTarget.All, new object[]
                {
                    (int)latestFortune.fortuneType,
                    latestFortune.resultIndex
                });
            }  //call rpc
            // this rpc is editied to GRAB the info and change it to the masters i
        }

        public static void SetTick(float tick)
        {
            Traverse.Create(GameObject.Find("PhotonMono").GetComponent<PhotonHandler>()).Field("nextSendTickCountOnSerialize").SetValue((int)(Time.realtimeSinceStartup * tick));
        }

        public static async void PacketChoke(bool repeat = true)
        {
            int num = (repeat ? 600 : 500);
            int num2 = (repeat ? 8005 : 9000);
            PhotonHandler component = GameObject.Find("PhotonMono").GetComponent<PhotonHandler>();
            bool flag = component;
            if (flag)
            {
                Traverse.Create(component).Field("nextSendTickCountOnSerialize").SetValue((int)(Time.realtimeSinceStartup * 9999f));
                await Task.Delay(num);
                for (int i = 0; i < 3950; i++)
                {
                    GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", repeat ? RpcTarget.OthersBuffered : RpcTarget.Others, new object[4]);
                }
                await Task.Delay(num2);
                Traverse.Create(component).Field("nextSendTickCountOnSerialize").SetValue((int)(Time.realtimeSinceStartup * 1000f));

            }
        }


        public static void silly2()
        {
            timer += 1f;
            if (!kickboo9l)
            {
                kickboo9l = true;
                NotifiLib.SendNotification("Wait for someone to leave or join!");
            }
            if (timer > 200f)
            {
                int[] targetActors = { };

                foreach (Player p in PhotonNetwork.PlayerListOthers)
                {
                    if (!targetActors.Contains(p.ActorNumber))
                    {
                        targetActors.AddItem(p.ActorNumber);
                    }
                }

                foreach (Player p in PhotonNetwork.PlayerListOthers)
                {
                    WebFlags flags = new WebFlags(255);
                    NetEventOptions options = new NetEventOptions
                    {
                        TargetActors = targetActors,
                        Reciever = NetEventOptions.RecieverTarget.master,
                        Flags = flags
                    };
                    string[] array = new string[MonkeAgent.instance.cachedPlayerList.Length];
                    int num = 0;
                    NetPlayer[] array2 = MonkeAgent.instance.cachedPlayerList;
                    foreach (NetPlayer netPlayer in array2)
                    {
                        array[num] = netPlayer.UserId;
                        num++;
                    }

                    object[] data = new object[7]
                    {
                        NetworkSystem.Instance.RoomStringStripped(),
                        array,
                        NetworkSystem.Instance.MasterClient.UserId,
                        p.UserId,
                        p.NickName,
                        "inappropriate tag data being sent splash effect",
                        NetworkSystemConfig.AppVersion
                    };
                    NetworkSystemRaiseEvent.RaiseEvent(255, data, options, reliable: true);
                    if (Time.time > CrashDelay2 + 0.2f)
                    {
                        CrashDelay2 = Time.time;
                        if (masterPos == RigShit.GetRigFromNetPlayer(NetworkSystem.Instance.MasterClient).transform.position)
                        {
                            NotifiLib.SendNotification("Froze everyone!");
                            Back.GetButton("Freeze All").enabled = false;
                        }
                        masterPos = RigShit.GetRigFromNetPlayer(NetworkSystem.Instance.MasterClient).transform.position;
                    }
                }
                timer = 0f;
            }
        }

        static bool kickboo9l;

        public static void silly3()
        {
            if (!kickboo9l)
            {
                kickboo9l = true;
                NotifiLib.SendNotification("Wait a little bit!");
            }
            if (Time.time > CrashDelay2 + 0f)
            {
                CrashDelay2 = Time.time;
                if (masterPos == RigShit.GetRigFromNetPlayer(NetworkSystem.Instance.MasterClient).transform.position)
                {
                    NotifiLib.SendNotification("Kicking everyone... Wait!");
                    Back.GetButtonInCate("Kick All", 2).enabled = false;
                }
                masterPos = RigShit.GetRigFromNetPlayer(NetworkSystem.Instance.MasterClient).transform.position;
            }
            if (CosmeticsController.instance != null)
            {
                CosmeticsController.instance.unlockedCosmetics.Clear();

                CosmeticsController.instance.UpdateMyCosmetics();
            }
        }

        public static float timer = 0f;

        public static bool freezebool;

        public static Vector3 masterPos;

        public static void silly()
        {
            if (Time.time > CrashDelay + 0.11f)
            {
                CrashDelay = Time.time;
                if (!freezebool)
                {
                    freezebool = true;
                    NotifiLib.SendNotification("Wait for them to freeze!");
                }

                int[] targetActors = { };

                foreach (Player p in PhotonNetwork.PlayerListOthers)
                {
                    if (!targetActors.Contains(p.ActorNumber))
                    {
                        targetActors.AddItem(p.ActorNumber);
                    }
                }

                //GorillaPlayerScoreboardLine.MutePlayer(plr.UserId, plr.NickName, UnityEngine.Random.Range(0, 1));
                foreach (NetPlayer plr in NetworkSystem.Instance.PlayerListOthers)
                {
                    WebFlags flags = new WebFlags(1);
                    NetEventOptions options = new NetEventOptions
                    {
                        TargetActors = targetActors,
                        Reciever = NetEventOptions.RecieverTarget.master,
                        Flags = flags
                    };
                    byte code = 51;
                    object[] data = new object[6]
                    {
                        plr.UserId,
                        1,
                        plr.NickName,
                        NetworkSystem.Instance.LocalPlayer.NickName,
                        !NetworkSystem.Instance.SessionIsPrivate,
                        NetworkSystem.Instance.RoomStringStripped()
                    };
                    NetworkSystemRaiseEvent.RaiseEvent(code, data, options, reliable: true);

                    if (Time.time > CrashDelay2 + 0.2f)
                    {
                        CrashDelay2 = Time.time;
                        if (masterPos == RigShit.GetRigFromNetPlayer(NetworkSystem.Instance.MasterClient).transform.position)
                        {
                            NotifiLib.SendNotification("Froze everyone!");
                            Back.GetButton("Freeze All").enabled = false;
                        }
                        masterPos = RigShit.GetRigFromNetPlayer(NetworkSystem.Instance.MasterClient).transform.position;
                    }
                }
            }
        }


        public static string eee = "";
        public static void GetPlayFabIdFromPhotonUserId(string photonUserId, VRRig chosenRig)
        {
            eee = chosenRig.OwningNetPlayer.UserId;
        }

        public static void grabAccDateForPlayer(VRRig rig, bool file)
        {
            var e = new GetPlayerCombinedInfoRequestParams
            {
                GetUserAccountInfo = true,
            };
            GetPlayFabIdFromPhotonUserId(GetPhotonViewFromRig(rig).Owner.UserId, rig);
            var r = new GetPlayerCombinedInfoRequest
            {
                PlayFabId = eee,
                InfoRequestParameters = e
            };

            PlayFabClientAPI.GetPlayerCombinedInfo(r,
            result =>
            {
                if (file)
                {
                    NotifiLib.SendNotification("<color=blue>[genesis]</color> Check your PC for the info!");
                    File.WriteAllText("infoonplayer.txt", "Username : " + rig.playerNameVisible + "\nDate Account Made : " + result.InfoResultPayload.AccountInfo.Created.ToString() + "\nPlayfab ID : " + result.InfoResultPayload.AccountInfo.PlayFabId.ToString());
                    Process.Start("infoonplayer.txt");
                }
                else
                {
                    NotifiLib.SendNotification("User : " + rig.playerNameVisible + "\nDate Account Made : " + result.InfoResultPayload.AccountInfo.Created.ToString() + "\nPlayfab ID : " + result.InfoResultPayload.AccountInfo.PlayFabId.ToString());
                }
            },
            error =>
            {
            });
        }

        public static PhotonView GetPhotonViewFromRig(VRRig p)
        {
            NetworkView m = (NetworkView)Traverse.Create(p).Field("netView").GetValue();
            return m.GetView;
        }



        public static bool info;

        public static void SuperInfectionKickMasterClient()
        {
            if (NetworkSystem.Instance.IsMasterClient)
                return;

            byte[] byteData = new byte[15360];
            MemoryStream memoryStream = new MemoryStream(byteData);
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(4);

            for (int i = 0; i < 4; i++)
            {
                binaryWriter.Write(SuperInfectionManager.activeSuperInfectionManager.gameEntityManager.itemPrefabFactory.Keys.ToArray().GetRandomItem());
                binaryWriter.Write(53295);
                binaryWriter.Write(BitPackUtils.PackQuaternionForNetwork(GorillaTagger.Instance.bodyCollider.transform.rotation));
                binaryWriter.Write(0L);
                binaryWriter.Write((byte)3);
            }

            byte[] bytes = GZipStream.CompressBuffer(byteData);
            byte[] padding = new byte[1960];

            Buffer.BlockCopy(bytes, 0, padding, 0, bytes.Length);
            bytes = padding;

            SuperInfectionManager.activeSuperInfectionManager.gameEntityManager.SendRPC(
                "JoinWithItemsRPC",
                RpcTarget.MasterClient,
                bytes,
                new int[0],
                NetworkSystem.Instance.LocalPlayer.ActorNumber
            );

            NewFlusher();
        }

        private static Coroutine siKickAllCoroutine;
        public static System.Collections.IEnumerator SIKickAllCoroutine()
        {
            while (PhotonNetwork.InRoom && !NetworkSystem.Instance.IsMasterClient)
            {
                int masterActor = NetworkSystem.Instance.MasterClient.ActorNumber;
                SuperInfectionKickMasterClient();

                float timeDelay = Time.time + 1f;
                yield return new WaitUntil(() => !PhotonNetwork.CurrentRoom.Players.ContainsKey(masterActor) || Time.time > timeDelay);
            }

            siKickAllCoroutine = null;
            yield break;
        }

        public static void SuperInfectionKickAll()
        {
            if (siKickAllCoroutine != null)
                CoroutineRunner.instance.StopCoroutine(siKickAllCoroutine);

            siKickAllCoroutine = CoroutineRunner.instance.StartCoroutine(SIKickAllCoroutine());
        }

        public static void InfoGun(bool file)
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (!info)
                {
                    grabAccDateForPlayer(data.lockedPlayer, file);
                    info = true;
                }
            }
            else
            {
                if (info)
                    info = false;
            }
        }

        public static bool frez;

        public static void Freeze()
        {
            frez = !frez;
            foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
            {
                line.PressButton(frez, GorillaPlayerLineButton.ButtonType.Mute);
            }
        }

        public static void sillyGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (!info)
                {
                    if (Time.time > CrashDelay + 0.1f)
                    {
                        CrashDelay = Time.time;
                        Freeze();
                    }
                    info = true;
                }
            }
            else
            {
                if (info)
                    info = false;
            }
        }

        public static bool AutoMasterBool;
        public static string NameChangerString;
        public static float SettingNameDelay;
        public static float MatDelay;
        public static float NameDelay;
        public static float ParticleDelay;
        public static bool removedSelf;
        public static string savedUser;


        public static void RemoveSelfFromBoard()
        {
            if (!PhotonNetwork.InRoom)
            {
                if (!removedSelf)
                {
                    savedUser = PhotonNetwork.LocalPlayer.NickName;
                    PhotonNetwork.LocalPlayer.NickName = "";
                    PhotonNetwork.NickName = "";
                    NotifiLib.SendNotification("<color=blue>[genesis]</color> Join a lobby, then wait for a few people to leave/join!");
                    removedSelf = true;
                }
            }
            else
            {
                if (!removedSelf)
                {
                    NotifiLib.SendNotification("<color=blue>[genesis]</color> You must NOT be in a lobby!");
                    Back.GetButton("remove self from").enabled = false;
                }
            }
        }

        public static void BreakLobby()
        {
            if (CrashDelay < Time.time)
            {
                CrashDelay = Time.time + 5f;
                foreach (Player p in PhotonNetwork.PlayerListOthers)
                {
                    Hashtable propertiesToSet = new Hashtable
                 {
                     {
                         "didTutorial",
                         false
                     },
                 };
                    p.SetCustomProperties(propertiesToSet);
                }
                NotifiLib.SendNotification("<color=blue>[genesis]</color> broke the lobby blehh");
            }
        }

        public static void BreakInfection()
        {
            if (CrashDelay < Time.time)
            {
                CrashDelay = Time.time + 5f;
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    Hashtable propertiesToSet = new Hashtable
                 {
                     {
                         "didTutorial",
                         false
                     },
                 };
                    p.SetCustomProperties(propertiesToSet);
                }
                NotifiLib.SendNotification("<color=blue>[genesis]</color> broke the lobby blehh");
            }
        }

        public static Hashtable rpcEvent = new ExitGames.Client.Photon.Hashtable();
        private static readonly object keyByteZero = 0;

        // Token: 0x04000061 RID: 97
        private static readonly object keyByteOne = 1;

        // Token: 0x04000062 RID: 98
        private static readonly object keyByteTwo = 2;

        // Token: 0x04000063 RID: 99
        private static readonly object keyByteThree = 3;

        // Token: 0x04000064 RID: 100
        private static readonly object keyByteFour = 4;

        // Token: 0x04000065 RID: 101
        private static readonly object keyByteFive = 5;

        // Token: 0x04000066 RID: 102
        private static readonly object keyByteSix = 6;

        // Token: 0x04000067 RID: 103
        private static readonly object keyByteSeven = 7;

        // Token: 0x04000068 RID: 104
        private static readonly object keyByteEight = 8;

        private static Dictionary<string, int> rpcShortcuts;

        public static float eeee;




        public static void FixLobby()
        {
            if (CrashDelay < Time.time)
            {
                CrashDelay = Time.time + 5f;
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    Hashtable propertiesToSet = new Hashtable
                 {
                     {
                         "didTutorial",
                         true
                     },
                 };
                    p.SetCustomProperties(propertiesToSet);
                }
                NotifiLib.SendNotification("<color=blue>[genesis]</color> fixed the lobby blehh");
            }
        }

        public static void OffRemoveSelf()
        {
            if (removedSelf)
            {
                PhotonNetwork.LocalPlayer.NickName = savedUser;
                PhotonNetwork.NickName = savedUser;
                PlayerPrefs.SetString("playerName", savedUser);
                GorillaComputer.instance.currentName = savedUser;
                PlayerPrefs.Save();
                removedSelf = false;
            }
        }

        public static async void KickModders()
        {
            List<GorillaPlayerScoreboardLine> lines = new List<GorillaPlayerScoreboardLine>();
            foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
            {
                if (line.linePlayer != NetworkSystem.Instance.LocalPlayer && line.linePlayer != null)
                {
                    lines.Add(line);
                }
            }
            foreach (GorillaPlayerScoreboardLine line in lines)
            {
                if (line.linePlayer != NetworkSystem.Instance.LocalPlayer && line.linePlayer != null)
                {
                    Transform report = line.reportButton.gameObject.transform;
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = report.position;
                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = report.position;
                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = report.position;
                }

                // Implement a 0.1-second delay between iterations
                await Task.Delay(200);
            }

            GorillaTagger.Instance.offlineVRRig.enabled = true;
            NotifiLib.SendNotification("<color=blue>[genesis]</color> Kicked all modders with antireport!");
        }

        public static void Particle(Vector3 pos, bool move)
        {
            if (ParticleDelay < Time.time)
            {
                ParticleDelay = Time.time + 0.12f;
                if (GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.ToLower().Contains("stealth"))
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    if (move)
                        GorillaTagger.Instance.offlineVRRig.transform.position = pos + new Vector3(0f, 2f, 0f);
                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = pos;
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = pos;
                    GorillaTagger.Instance.myVRRig.SendRPC("OnHandTapRPC", RpcTarget.All, new object[]
                    {
                232,
                false,
                9999f,
                Utils.PackVector3ToLong(new Vector3(99999f, 99999f, 99999f))
                    });
                    OP.NewFlusher();
                }
            }
        }

        public static void ParticleGun()
        {
            var data = GunLib.Shoot();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (!GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.ToLower().Contains("stealth"))
                {
                    NotifiLib.SendNotification("<color=blue>[GENESIS]</color> You must be ambushed!");
                    return;
                }
                Particle(data.hitPosition, true);
            }
            if (!data.isTriggered)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void ParticleAroundSelf()
        {
            if (!GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.ToLower().Contains("stealth"))
            {
                NotifiLib.SendNotification("<color=blue>[GENESIS]</color> You must be ambushed!");
                return;
            }

            Vector3 randomDirection = Random.insideUnitSphere.normalized * Random.Range(0, 3);
            Particle(GorillaTagger.Instance.offlineVRRig.transform.position + randomDirection, false);
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void ParticleAroundPlayerGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (!GorillaTagger.Instance.offlineVRRig.mainSkin.material.name.ToLower().Contains("stealth"))
                {
                    NotifiLib.SendNotification("<color=blue>[GENESIS]</color> You must be ambushed!");
                    return;
                }

                Vector3 randomDirection = Random.insideUnitSphere.normalized * Random.Range(0, 3);
                Particle(data.lockedPlayer.transform.position + randomDirection, true);
            }
            if (!data.isTriggered)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void MatAll()
        {
            if (MatDelay < Time.time)
            {
                MatDelay = Time.time + 0.3f;
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    GorillaTagManager gorillaTagManager = GorillaGameManager.instance.GetComponent<GorillaTagManager>();
                    foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                    {
                        if (gorillaTagManager.currentInfected.Contains(player))
                        {
                            gorillaTagManager.ClearInfectionState();
                        }
                        if (!gorillaTagManager.currentInfected.Contains(player))
                        {
                            gorillaTagManager.AddInfectedPlayer(player);
                        }
                    }
                }
                else
                {
                    NotifiLib.SendNotification("<color=red>[MAT SPAM]</color> Become master!");
                }
            }
        }

        public static void BreakGamemode()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameObject.Find("Gorilla Tag Manager").GetComponent<GorillaTagManager>().currentInfected.Clear();
            }
            else
            {
                NotifiLib.SendNotification("<color=red>[GAMEMODE BREAKER]</color> Become master!");
            }
        }

        public static bool IsModded()
        {
            return true;
        }

        static float cooldown;

        static bool done;


        public static void SnowballLauncher()
        {
            if (WristMenu.gripDownR && Time.time > cooldown)
            {
                GorillaTagger.Instance.offlineVRRig.RightThrowableProjectileIndex = 0;
                GorillaTagger.Instance.offlineVRRig.LeftThrowableProjectileIndex = 0;
                //EnableSnowballs();
                cooldown = Time.time + 0.07f;
                done = !done;
                if (done)
                {
                    GameObject go = null;
                    try
                    {
                        go = GameObject.Find("Local Gorilla Player/RigAnchor/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/TransferrableItemRightHand/GrowingSnowballRightAnchor(Clone)/LMACF. RIGHT.").gameObject;
                    }
                    catch { go = null; }

                    GrowingSnowballThrowable goOb = null;
                    if (!go)
                    {
                        go = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/GrowingSnowballRightAnchor(Clone)/LMACF. RIGHT.");
                        go.gameObject.SetActive(true);
                        return;
                    }
                    else
                    {
                        goOb = go.GetComponent<GrowingSnowballThrowable>();
                    }
                    goOb.SetSnowballActiveLocal(true);
                    goOb.IncreaseSize(5);
                    var photonevent = Traverse.Create(goOb).Field("snowballThrowEvent").GetValue<PhotonEvent>();
                    object[] source = new object[]
                    {
                        PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 2400f, GetProjectileIncrement(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 2400f, 5f)
                    };
                    photonevent.RaiseAll(source);
                }
                else
                {
                    GameObject go = null;
                    try
                    {
                        go = GameObject.Find("Local Gorilla Player/RigAnchor/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L/TransferrableItemLeftHand/GrowingSnowballLeftAnchor(Clone)/LMACE. LEFT.").gameObject;
                    }
                    catch { go = null; }

                    GrowingSnowballThrowable goOb = null;
                    if (!go)
                    {
                        go = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/GrowingSnowballLeftAnchor(Clone)/LMACE. LEFT.");
                        go.gameObject.SetActive(true);
                        return;
                    }
                    else
                    {
                        goOb = go.GetComponent<GrowingSnowballThrowable>();
                    }
                    goOb.SetSnowballActiveLocal(true);
                    goOb.IncreaseSize(5);
                    var photonevent = Traverse.Create(goOb).Field("snowballThrowEvent").GetValue<PhotonEvent>();
                    object[] source = new object[]
                    {
                        PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 2400f, GetProjectileIncrement(PlayerMovement.TrueRightHand().position, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 2400f, 5f)
                    };
                    photonevent.RaiseAll(source);
                }
                NewFlusher();
            }
            if (!WristMenu.gripDownR)
            {
                DisableSnowballs();
            }
        }

        static float delay;

        public static void StumpTeleSpam()
        {
            if (WristMenu.triggerDownR)
                TeleportSpammer((short)Random.Range(0, 8), "Environment Objects/LocalObjects_Prefab/City_WorkingPrefab/Arcade_prefab/MainRoom/VRArea/ModIOArcadeTeleporter/NetObject_VRTeleporter");
        }

        public static void ArcadeTeleSpam()
        {
            if (WristMenu.triggerDownR)
                TeleportSpammer(0);
        }

        public static void TeleportSpammer(short index, string telename = "Environment Objects/LocalObjects_Prefab/TreeRoom/StumpVRHeadset/ModIOArcadeTeleporter (1)/NetObject_VRTeleporter")
        {
            if (Time.time > delay)
            {
                PhotonView tele = GameObject.Find(telename).GetComponent<Photon.Pun.PhotonView>();
                delay = Time.time + 0.1f;

                tele.RPC("ActivateTeleportVFX", Photon.Pun.RpcTarget.All, new object[] { (short)index });
                tele.RPC("ActivateReturnVFX", Photon.Pun.RpcTarget.All, new object[] { (short)index });
                NewFlusher();
            }
        }

        public static void SnowballRain()
        {
            if (WristMenu.gripDownR && Time.time > cooldown)
            {
                GorillaTagger.Instance.offlineVRRig.RightThrowableProjectileIndex = 0;
                GorillaTagger.Instance.offlineVRRig.LeftThrowableProjectileIndex = 0;
                cooldown = Time.time + 0.1f;
                //EnableSnowballs();
                done = !done;
                Vector3 Pos = OP.RainPosGen();
                if (done)
                {
                    GameObject go = null;
                    try
                    {
                        go = GameObject.Find("Local Gorilla Player/RigAnchor/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/TransferrableItemRightHand/GrowingSnowballRightAnchor(Clone)/LMACF. RIGHT.").gameObject;
                    }
                    catch { go = null; }

                    GrowingSnowballThrowable goOb = null;
                    if (!go)
                    {
                        go = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/GrowingSnowballRightAnchor(Clone)/LMACF. RIGHT.");
                        go.gameObject.SetActive(true);
                        return;
                    }
                    else
                    {
                        goOb = go.GetComponent<GrowingSnowballThrowable>();
                    }
                    goOb.SetSnowballActiveLocal(true);
                    goOb.IncreaseSize(5);
                    var photonevent = Traverse.Create(goOb).Field("snowballThrowEvent").GetValue<PhotonEvent>();
                    object[] source = new object[]
                    {
                        Pos, Vector3.zero, GetProjectileIncrement(Pos, Vector3.zero, 5f)
                    };
                    photonevent.RaiseAll(source);
                }
                else
                {
                    GameObject go = null;
                    try
                    {
                        go = GameObject.Find("Local Gorilla Player/RigAnchor/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L/TransferrableItemLeftHand/GrowingSnowballLeftAnchor(Clone)/LMACF. LEFT.").gameObject;
                    }
                    catch { go = null; }

                    GrowingSnowballThrowable goOb = null;
                    if (!go)
                    {
                        go = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/GrowingSnowballLeftAnchor(Clone)/LMACF. LEFT.");
                        go.gameObject.SetActive(true);
                        return;
                    }
                    else
                    {
                        goOb = go.GetComponent<GrowingSnowballThrowable>();
                    }
                    goOb.SetSnowballActiveLocal(true);
                    goOb.IncreaseSize(5);
                    var photonevent = Traverse.Create(goOb).Field("snowballThrowEvent").GetValue<PhotonEvent>();
                    object[] source = new object[]
                    {
                        Pos, Vector3.zero, GetProjectileIncrement(Pos, Vector3.zero, 5f)
                    };
                    photonevent.RaiseAll(source);
                }
                NewFlusher();
            }

            if (!WristMenu.gripDownR)
            {
                DisableSnowballs();
            }
        }



        public static void SnowballGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered && Time.time > cooldown)
            {
                GorillaTagger.Instance.offlineVRRig.RightThrowableProjectileIndex = 0;
                GorillaTagger.Instance.offlineVRRig.LeftThrowableProjectileIndex = 0;
                // EnableSnowballs();
                cooldown = Time.time + 0.05f;
                done = !done;
                if (done)
                {
                    GameObject go = null;
                    try
                    {
                        go = GameObject.Find("Local Gorilla Player/RigAnchor/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/TransferrableItemRightHand/GrowingSnowballRightAnchor(Clone)/LMACF. RIGHT.").gameObject;
                    }
                    catch { go = null; }

                    GrowingSnowballThrowable goOb = null;
                    if (!go)
                    {
                        go = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/GrowingSnowballRightAnchor(Clone)/LMACF. RIGHT.");
                        go.gameObject.SetActive(true);
                        return;
                    }
                    else
                    {
                        goOb = go.GetComponent<GrowingSnowballThrowable>();
                    }
                    goOb.SetSnowballActiveLocal(true);
                    goOb.IncreaseSize(5);
                    Vector3 funn = (data.hitPosition - PlayerMovement.TrueRightHand().position).normalized;
                    float penis = 35;
                    funn *= penis;
                    var photonevent = Traverse.Create(goOb).Field("snowballThrowEvent").GetValue<PhotonEvent>();
                    object[] source = new object[]
                    {
                        PlayerMovement.TrueRightHand().position, funn, GetProjectileIncrement(PlayerMovement.TrueRightHand().position, funn, 5f)
                    };
                    photonevent.RaiseAll(source);
                }
                else
                {
                    GameObject go = null;
                    try
                    {
                        go = GameObject.Find("Local Gorilla Player/RigAnchor/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.R/palm.01.L/TransferrableItemLeftHand/GrowingSnowballLeftAnchor(Clone)/LMACF. LEFT.").gameObject;
                    }
                    catch { go = null; }

                    GrowingSnowballThrowable goOb = null;
                    if (!go)
                    {
                        go = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/GrowingSnowballLeftAnchor(Clone)/LMACF. LEFT.");
                        go.gameObject.SetActive(true);
                        return;
                    }
                    else
                    {
                        goOb = go.GetComponent<GrowingSnowballThrowable>();
                    }
                    goOb.SetSnowballActiveLocal(true);
                    goOb.IncreaseSize(5);
                    Vector3 funn = (data.hitPosition - PlayerMovement.TrueRightHand().position).normalized;
                    float penis = 35;
                    funn *= penis;
                    var photonevent = Traverse.Create(goOb).Field("snowballThrowEvent").GetValue<PhotonEvent>();
                    object[] source = new object[]
                    {
                        PlayerMovement.TrueRightHand().position, funn, GetProjectileIncrement(PlayerMovement.TrueRightHand().position, funn, 5f)
                    };
                    photonevent.RaiseAll(source);
                }
                NewFlusher();
            }

            if (!data.isShooting && !data.isTriggered)
            {
                DisableSnowballs();
            }
        }




















        public static void GMSpam()
        {
            if (MatDelay < Time.time)
            {
                MatDelay = Time.time + 0.15f;
                if (PhotonNetwork.CurrentRoom.CustomProperties.ToString().Contains("INFECTION"))
                {
                    GorillaTagManager hunt = GorillaGameManager.instance.GetComponent<GorillaTagManager>();
                    hunt.ClearInfectionState();
                }
                int randomint = UnityEngine.Random.Range(1, 4);
                if (randomint == 1)
                    ChangeGamemode("Battle");
                if (randomint == 2)
                    ChangeGamemode("Infection");
                if (randomint == 3)
                    ChangeGamemode("Hunt");

            }

            if (PhotonNetwork.CurrentRoom.CustomProperties.ToString().Contains("HUNT"))
            {
                GorillaHuntManager hunt = GorillaGameManager.instance.GetComponent<GorillaHuntManager>();
                hunt.currentHunted = NetworkSystem.Instance.AllNetPlayers.ToList();
            }
            if (PhotonNetwork.CurrentRoom.CustomProperties.ToString().Contains("INFECTION"))
            {
                GorillaTagManager hunt = GorillaGameManager.instance.GetComponent<GorillaTagManager>();
                hunt.currentInfected = NetworkSystem.Instance.AllNetPlayers.ToList();
            }
        }























        public static bool ez;

        public static float antibanflaot;

        public static void AutoAntiban()
        {
            if (Time.time > antibanflaot + 5f && PhotonNetwork.CurrentRoom.Players.Count == 10 && !ez)
            {
                Buttons.buttons[1][1].enabled = true;
                ez = true;
            }
        }

        public static short PackColor(Color col)
        {
            return (short)(Mathf.RoundToInt(col.r * 9f) + Mathf.RoundToInt(col.g * 9f) * 10 + Mathf.RoundToInt(col.b * 9f) * 100);
        }

        private static readonly ExitGames.Client.Photon.Hashtable removeFilter;
        private static readonly RaiseEventOptions ServerCleanOptions;

        private static readonly Hashtable rpcFilterByViewId = new Hashtable();

        private static readonly RaiseEventOptions OpCleanRpcBufferOptions = new RaiseEventOptions
        {
            CachingOption = EventCaching.RemoveFromRoomCache
        };

        internal static RaiseEventOptions SendToAllOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };



        public static void ChangeGamemode(string gamemodeName)
        {

            if (IsModded())
            {
                if (gamemodeName.Contains("Battl"))
                {
                    GorillaPaintbrawlManager component = GameObject.Find("Gorilla Battle Manager").GetComponent<GorillaPaintbrawlManager>();
                    component.StartBattle();
                    Hashtable hash = new Hashtable();
                    hash.Add("gameMode", "DEFAULT_MODDED_MODDED_forestcitybasementcanyonsmountainsbeachskycaves_BATTLEPAINTBRAWL");
                    GorillaGameManager.instance.checkCooldown = 0f;
                    PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetCustomPropertiesOfRoom(hash, null, null);
                }

                if (gamemodeName.Contains("Infectio"))
                {
                    GorillaTagManager component = GameObject.Find("Gorilla Tag Manager").GetComponent<GorillaTagManager>();
                    component.StartPlaying();
                    Hashtable hash = new Hashtable();
                    hash.Add("gameMode", "DEFAULT_MODDED_MODDED_forestcitybasementcanyonsmountainsbeachskycaves_INFECTION");
                    GorillaGameManager.instance.checkCooldown = 0f;
                    PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetCustomPropertiesOfRoom(hash, null, null);
                }

                if (gamemodeName.Contains("Hun"))
                {
                    GorillaHuntManager component = GameObject.Find("Gorilla Hunt Manager").GetComponent<GorillaHuntManager>();
                    component.StartHunt();
                    Hashtable hash = new Hashtable();
                    hash.Add("gameMode", "DEFAULT_MODDED_MODDED_forestcitybasementcanyonsmountainsbeachskycaves_HUNT");
                    GorillaGameManager.instance.checkCooldown = 0f;
                    PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetCustomPropertiesOfRoom(hash, null, null);
                }

                if (gamemodeName.Contains("Casua"))
                {
                    Hashtable hash = new Hashtable();
                    hash.Add("gameMode", "DEFAULT_MODDED_MODDED_forestcitybasementcanyonsmountainsbeachskycaves");
                    GorillaGameManager.instance.checkCooldown = 0f;
                    PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetCustomPropertiesOfRoom(hash, null, null);
                }

                NewFlusher();
            }
            else
            {
                NotifiLib.SendNotification("<color=red>[GAMEMODE CHANGER]</color> Activate Antiban!");
            }
        }

        public static void GetOwnerShip(PhotonView view)
        {
            view.GetComponent<RequestableOwnershipGuard>().PlayerHasAuthority(PhotonNetwork.LocalPlayer);
            view.OwnerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
            view.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
            view.ViewID = GorillaTagger.Instance.myVRRig.ViewID;
            view.RequestOwnership();
            view.TransferOwnership(PhotonNetwork.LocalPlayer);
            view.GetComponent<RequestableOwnershipGuard>().actualOwner = PhotonNetwork.LocalPlayer;
            view.GetComponent<RequestableOwnershipGuard>().currentOwner = PhotonNetwork.LocalPlayer;
            view.GetComponent<RequestableOwnershipGuard>().creator = PhotonNetwork.LocalPlayer;
            view.GetComponent<RequestableOwnershipGuard>().SetCreator(PhotonNetwork.LocalPlayer);
        }



        public static void BanPlayer(VRRig rig)
        {
            Player p = RigShit.GetPlayerFromRig(rig);

            foreach (FortuneTeller t in UnityEngine.Object.FindObjectsOfType<FortuneTeller>())
            {
                t.photonView.RPC("RequestFortuneRPC", p, null);
            }
        }

        public static void BanGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                BanPlayer(data.lockedPlayer);
            }
        }
        public static void MatGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (MatDelay < Time.time)
                {
                    MatDelay = Time.time + 0.5f;
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        GorillaTagManager gorillaTagManager = GorillaGameManager.instance.GetComponent<GorillaTagManager>();
                        if (gorillaTagManager.currentInfected.Contains(RigShit.GetPlayerFromRig(data.lockedPlayer)))
                        {
                            gorillaTagManager.ClearInfectionState();
                        }
                        if (!gorillaTagManager.currentInfected.Contains(RigShit.GetPlayerFromRig(data.lockedPlayer)))
                        {
                            gorillaTagManager.AddInfectedPlayer(RigShit.GetPlayerFromRig(data.lockedPlayer));
                        }
                    }
                    else
                    {
                        NotifiLib.SendNotification("<color=red>[MAT SPAM]</color> Become master!");
                    }
                }
            }
        }

        public static void NameGun()
        {
            if (!PhotonNetwork.CurrentRoom.CustomProperties.ToString().Contains("MODDED"))
            {
                //NotifiLib.SendNotification("<color=red>[NAME CHANGER]</color> Turn on antiban!");
                //return;
            }
            if (NameChangerString == null)
            {
                if (SettingNameDelay < Time.time)
                {
                    SettingNameDelay = Time.time + 5f;
                    NotifiLib.SendNotification("<color=blue>[NAME CHANGER]</color> Assign the thing to change their names to on the gui! [ if it doesnt show up, then use name change all first! ]");
                    return;
                }
            }
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (NameDelay < Time.time)
                {
                    NameDelay = Time.time + 0.05f;
                    Hashtable hashtable = new Hashtable();
                    hashtable[byte.MaxValue] = NameChangerString;
                    Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
                    dictionary.Add(251, hashtable);
                    dictionary.Add(254, RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber);
                    dictionary.Add(250, true);
                    PhotonNetwork.CurrentRoom.LoadBalancingClient.LoadBalancingPeer.SendOperation(252, dictionary, SendOptions.SendUnreliable);
                    NewFlusher();
                }
            }
        }

        static float CrashDelay;
        static float CrashDelay2;

        public static void PlaneCrashAll()
        {
            if (WristMenu.triggerDownL)
                PlaneCrash(null, true);
            else
            {
                NewFlusher();
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void PlaneCrash(VRRig rig, bool all = false)
        {
            for (int i = 4; i >= 0; i--)
            {
                SendThrowablePlane(new Vector3(9999f, 9999f, 9999f), new Vector3(9999f, 9999f, 9999f), Quaternion.identity, 1, false);
                SendThrowableFatPlane(new Vector3(9999f, 9999f, 9999f), new Vector3(9999f, 9999f, 9999f), Random.rotation, 1, false);
                SendThrowableHeart(new Vector3(9999f, 9999f, 9999f), new Vector3(9999f, 9999f, 9999f), Random.rotation, 1, false);
                NewFlusher();
            }
        }

        public static void BetaKickGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;

            if (data.isLocked)
            {
                BetaKick(data.lockedPlayer);
            }
        }

        public static void BetaKick(VRRig rig)
        {
            GorillaNetworkDisconnectTrigger gorillaNetworkDisconnectTrigger = new GorillaNetworkDisconnectTrigger();
            gorillaNetworkDisconnectTrigger.componentTarget = rig.gameObject;
            gorillaNetworkDisconnectTrigger.componentTypeToRemove = "VRRig";

            gorillaNetworkDisconnectTrigger.transform.position = rig.gameObject.transform.position;

            gorillaNetworkDisconnectTrigger.OnBoxTriggered();
        }

        public static void LagAll2()
        {
            if (WristMenu.triggerDownL)
            {
                if (CrashDelay < Time.time && PhotonNetwork.InRoom)
                {
                    CrashDelay = Time.time + 0.05f;
                    foreach (Player p in PhotonNetwork.PlayerListOthers)
                    {
                        PhotonNetwork.RaiseEvent(0, null, new RaiseEventOptions
                        {
                            CachingOption = EventCaching.DoNotCache,
                            TargetActors = new int[]
                            {
                                p.ActorNumber
                            }
                        }, SendOptions.SendReliable);

                        var pp = RigShit.GetViewFromPlayer(p);
                        pp.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                        PhotonNetwork.Destroy(pp);
                    }
                }
            }
        }

        public static void MuteAllSS()
        {
            if (WristMenu.triggerDownL)
            {
                if (CrashDelay < Time.time && PhotonNetwork.InRoom)
                {
                    CrashDelay = Time.time + 0.3f;
                    foreach (Player p in PhotonNetwork.PlayerListOthers)
                    {
                        PhotonNetwork.RaiseEvent(0, null, new RaiseEventOptions
                        {
                            CachingOption = EventCaching.DoNotCache,
                            TargetActors = new int[]
                            {
                                p.ActorNumber
                            }
                        }, SendOptions.SendReliable);

                        var pp = RigShit.GetViewFromPlayer(p);
                        pp.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                        PhotonNetwork.Destroy(pp);
                    }
                }
            }
        }



        public static void LagAura()
        {
            if (Time.time > CrashDelay + 0.3f)
            {
                CrashDelay = Time.time;
                foreach (VRRig player in VRRigCache.ActiveRigs)
                {
                    if (Vector3.Distance(GorillaTagger.Instance.offlineVRRig.transform.position, player.transform.position) < 4)
                    {
                        LagView(player);
                    }
                }
            }
        }

        public static void LagOnYouTouch()
        {
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isOfflineVRRig)
                {
                    var myrig = GorillaTagger.Instance.offlineVRRig;
                    var distance = Vector3.Distance(myrig.rightHandTransform.position, rig.transform.position);
                    var distance2 = Vector3.Distance(myrig.leftHandTransform.position, rig.transform.position);

                    if (distance < 0.3f)
                    {
                        LagView(rig);
                        UnityEngine.Debug.unityLogger.logEnabled = false;
                    }
                    if (distance2 < 0.3f)
                    {
                        LagView(rig);
                        UnityEngine.Debug.unityLogger.logEnabled = false;
                    }
                }
            }
        }

        public static void KickOnYouTouch()
        {
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isOfflineVRRig)
                {
                    var myrig = GorillaTagger.Instance.offlineVRRig;
                    var distance = Vector3.Distance(myrig.rightHandTransform.position, rig.transform.position);
                    var distance2 = Vector3.Distance(myrig.leftHandTransform.position, rig.transform.position);

                    if (distance < 0.3f)
                    {
                        Kick(rig);
                    }
                    if (distance2 < 0.3f)
                    {
                        Kick(rig);
                    }
                }
            }
        }

        public static void Lag2OnYouTouch()
        {
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isOfflineVRRig)
                {
                    var myrig = GorillaTagger.Instance.offlineVRRig;
                    var distance = Vector3.Distance(myrig.rightHandTransform.position, rig.transform.position);
                    var distance2 = Vector3.Distance(myrig.leftHandTransform.position, rig.transform.position);

                    if (distance < 0.3f)
                    {
                        LagView2(rig, false);
                        UnityEngine.Debug.unityLogger.logEnabled = false;
                    }
                    if (distance2 < 0.3f)
                    {
                        LagView2(rig, false);
                        UnityEngine.Debug.unityLogger.logEnabled = false;
                    }
                }
            }
        }

        public static void KillIRLOnYouTouch()
        {
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isOfflineVRRig)
                {
                    var myrig = GorillaTagger.Instance.offlineVRRig;
                    var distance = Vector3.Distance(myrig.rightHandTransform.position, rig.transform.position);
                    var distance2 = Vector3.Distance(myrig.leftHandTransform.position, rig.transform.position);

                    if (distance < 0.3f)
                    {
                        if (Time.time > dumbdelay)
                        {
                            dumbdelay = Time.time + 0.1f;
                            GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                            if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                            {

                                GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RpcTarget.Others, new object[] { true, false, false });
                                GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RpcTarget.Others, new object[] { new Vector3(0.00f, float.NaN, 0.00f) });

                                NewFlusher();
                            }
                            else
                            {
                                NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                            }
                        }
                        UnityEngine.Debug.unityLogger.logEnabled = false;
                    }
                    if (distance2 < 0.3f)
                    {
                        if (Time.time > dumbdelay)
                        {
                            dumbdelay = Time.time + 0.1f;
                            GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                            if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                            {

                                GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RpcTarget.Others, new object[] { true, false, false });
                                GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RpcTarget.Others, new object[] { new Vector3(0.00f, float.NaN, 0.00f) });

                                NewFlusher();
                            }
                            else
                            {
                                NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                            }
                        }
                        UnityEngine.Debug.unityLogger.logEnabled = false;
                    }
                }
            }
        }

        public static void KickTouch()
        {
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isOfflineVRRig)
                {
                    Vector3 adjustedLocalHandPosition = GorillaTagger.Instance.offlineVRRig.transform.position;
                    float distance = Vector3.Distance(adjustedLocalHandPosition, rig.gameObject.transform.Find("rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R").position);
                    float distance2 = Vector3.Distance(adjustedLocalHandPosition, rig.gameObject.transform.Find("rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L").position);

                    if (distance < 0.3f)
                    {
                        Kick(rig);
                    }
                    if (distance2 < 0.3f)
                    {
                        Kick(rig);
                    }
                }
            }
        }

        public static void LagTouch()
        {
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isOfflineVRRig)
                {
                    Vector3 adjustedLocalHandPosition = GorillaTagger.Instance.offlineVRRig.transform.position;
                    float distance = Vector3.Distance(adjustedLocalHandPosition, rig.gameObject.transform.Find("rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R").position);
                    float distance2 = Vector3.Distance(adjustedLocalHandPosition, rig.gameObject.transform.Find("rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L").position);

                    if (distance < 0.3f)
                    {
                        LagView(rig);
                        UnityEngine.Debug.unityLogger.logEnabled = false;
                    }
                    if (distance2 < 0.3f)
                    {
                        LagView(rig);
                        UnityEngine.Debug.unityLogger.logEnabled = false;
                    }
                }
            }
        }

        public static void Lag2Touch()
        {
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isOfflineVRRig)
                {
                    Vector3 adjustedLocalHandPosition = GorillaTagger.Instance.offlineVRRig.transform.position;
                    float distance = Vector3.Distance(adjustedLocalHandPosition, rig.gameObject.transform.Find("rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R").position);
                    float distance2 = Vector3.Distance(adjustedLocalHandPosition, rig.gameObject.transform.Find("rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L").position);

                    if (distance < 0.3f)
                    {
                        LagView2(rig, false);
                        UnityEngine.Debug.unityLogger.logEnabled = false;
                    }
                    if (distance2 < 0.3f)
                    {
                        LagView2(rig, false);
                        UnityEngine.Debug.unityLogger.logEnabled = false;
                    }
                }
            }
        }

        public static void FloatGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;

            if (data.isLocked)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    foreach (MonkeyeAI ai in GameObject.Find("Environment Objects/05Maze_PersistentObjects/BasementMaze").GetComponentsInChildren<MonkeyeAI>())
                    {
                        PhotonView view = (PhotonView)Traverse.Create(ai).Field("_view").GetValue();
                        view.GetComponent<RequestableOwnershipGuard>().TransferOwnership(PhotonNetwork.LocalPlayer, "");
                        view.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                        view.OwnerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                    }
                    GameObject.Find("Environment Objects/05Maze_PersistentObjects/BasementMaze/Monkeye_Prefab_Angry").transform.position = data.lockedPlayer.transform.position + GorillaTagger.Instance.offlineVRRig.transform.forward * 0.09f;
                    GameObject.Find("Environment Objects/05Maze_PersistentObjects/BasementMaze/Monkeye_Prefab_Sleepy").transform.position = data.lockedPlayer.transform.position - GorillaTagger.Instance.offlineVRRig.transform.forward * 0.09f;
                    GameObject.Find("Environment Objects/05Maze_PersistentObjects/BasementMaze/Monkeye_Prefab_Keen").transform.position = data.lockedPlayer.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * 0.09f;
                    GameObject.Find("Environment Objects/05Maze_PersistentObjects/BasementMaze/Monkeye_Prefab_Tweaky").transform.position = data.lockedPlayer.transform.position - GorillaTagger.Instance.offlineVRRig.transform.right * 0.09f;
                }
                else
                {
                    NotifiLib.SendNotification("<color=red>[GUN]</color> Become master!");
                }
            }
        }

        public static bool kickbool;



        public static PhotonView kickskib;



        private static StringBuilder reusableSB = new StringBuilder();
        private static string ToStringStripped(Photon.Realtime.Room room)
        {
            reusableSB.Clear();
            reusableSB.AppendFormat("Room: '{0}' ", (room.Name.Length < 20) ? room.Name : room.Name.Remove(20));
            reusableSB.AppendFormat("{0},{1} {3}/{2} players.", new object[]
            {
                room.IsVisible ? "visible" : "hidden",
                room.IsOpen ? "open" : "closed",
                room.MaxPlayers,
                room.PlayerCount
            });
            reusableSB.Append("\ncustomProps: {");
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode", out object GameModeType))
            {
                if (GameModeType != null && GameModeType is string)
                {
                    string GameMode = GameModeType.ToString();
                    reusableSB.AppendFormat("joinedGameMode={0}, ", (GameMode.Length < 50) ? GameMode : GameMode.Remove(50));
                }
            }
            System.Collections.IDictionary customProperties = room.CustomProperties;
            if (customProperties.Contains("gameMode"))
            {
                object obj = customProperties["gameMode"];
                if (obj == null)
                {
                    reusableSB.AppendFormat("gameMode=null}", Array.Empty<object>());
                }
                else
                {
                    string text = obj as string;
                    if (text != null)
                    {
                        reusableSB.AppendFormat("gameMode={0}", (text.Length < 50) ? text : text.Remove(50));
                    }
                }
            }
            reusableSB.Append("}");
            return reusableSB.ToString();
        }

        private static int[] targetActors = new int[]
        {
            -1
        };

        public static void BeachballOwnership()
        {
            Traverse.Create(PhotonNetwork.LocalPlayer).Field("actorNumber").SetValue(-1);
        }

        static bool movement;

        public static void fuckingkilleveryone()
        {
            if (WristMenu.triggerDownL)
            {
                if (!movement)
                {
                    NotifiLib.SendNotification("Activated platforms so you can move!");
                    Back.GetButton("Platforms").enabled = true;
                }

                movement = true;

                foreach (MonkeyeAI aiw in GameObject.Find("Environment Objects/05Maze_PersistentObjects/BasementMaze").GetComponentsInChildren<MonkeyeAI>())
                {
                    OP.NewFlusher();
                    aiw.enabled = true;
                    PhotonView plyer = aiw.GetComponent<PhotonView>();
                    plyer.OwnerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                    plyer.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                    plyer.RequestOwnership();
                    plyer.TransferOwnership(PhotonNetwork.LocalPlayer);
                    plyer.GetComponent<RequestableOwnershipGuard>().actualOwner = PhotonNetwork.LocalPlayer;
                    plyer.GetComponent<RequestableOwnershipGuard>().currentOwner = PhotonNetwork.LocalPlayer;
                    plyer.GetComponent<RequestableOwnershipGuard>().RequestTheCurrentOwnerFromAuthority();
                    plyer.GetComponent<RequestableOwnershipGuard>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    aiw.transform.position = new Vector3(9999999999999f, 9999999999999f, 9999999999999f);
                    aiw.SetState(MonkeyeAI_ReplState.EStates.Sleeping);
                    aiw.patrolPts.Clear();
                    PhotonMessageInfo n = new PhotonMessageInfo();
                    plyer.GetComponent<RequestableOwnershipGuard>().TransferOwnershipFromToRPC(PhotonNetwork.LocalPlayer, plyer.GetComponent<RequestableOwnershipGuard>().ownershipRequestNonce, n);
                }
            }
            else
            {
                if (movement)
                {
                    Back.GetButton("Platforms").enabled = false;
                    foreach (MonkeyeAI aiw in GameObject.Find("Environment Objects/05Maze_PersistentObjects/BasementMaze").GetComponentsInChildren<MonkeyeAI>())
                    {

                        aiw.transform.position = new Vector3(0f, 0f, 0f);
                        aiw.enabled = false;

                    }
                    movement = false;
                }
            }
        }

        public static void FloatGun2()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;

            if (data.isLocked)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    foreach (MonkeyeAI ai in GameObject.Find("Environment Objects/05Maze_PersistentObjects/BasementMaze").GetComponentsInChildren<MonkeyeAI>())
                    {
                        PhotonView view = (PhotonView)Traverse.Create(ai).Field("_view").GetValue();
                        view.GetComponent<RequestableOwnershipGuard>().TransferOwnership(PhotonNetwork.LocalPlayer, "");
                        view.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                        view.OwnerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                    }
                    Vector3[] positions = new Vector3[]
                    {
                            data.lockedPlayer.transform.position + GorillaTagger.Instance.offlineVRRig.transform.forward * 0.09f,
                            data.lockedPlayer.transform.position - GorillaTagger.Instance.offlineVRRig.transform.forward * 0.09f,
                            data.lockedPlayer.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * 0.09f,
                            data.lockedPlayer.transform.position - GorillaTagger.Instance.offlineVRRig.transform.right * 0.09f,
                    };
                    var monsters = UnityEngine.Object.FindObjectsOfType<MonkeyeAI>();
                    for (int i = monsters.Length - 1; i >= 0; i--)
                    {
                        monsters[i].gameObject.transform.position = positions[i];
                    }
                }
                else
                {
                    NotifiLib.SendNotification("<color=red>[GUN]</color> Become master!");
                }
            }
        }

        public static float skibdelay;

        private static string[] archiveCosmetics = null;

        public static async void AllCosmeticsTest()
        {
            for (int i = 0; i < 16; i++)
            {
                if (CosmeticsController.instance.allCosmetics[UnityEngine.Random.Range(1, CosmeticsController.instance.allCosmetics.Count)].canTryOn)
                {
                    CosmeticsController.instance.tryOnSet.items[i] = CosmeticsController.instance.allCosmetics[UnityEngine.Random.Range(1, CosmeticsController.instance.allCosmetics.Count)];
                }
            }
            CosmeticsController.instance.UpdateShoppingCart();
            foreach (FittingRoomButton b in GameObject.Find("Environment Objects/LocalObjects_Prefab/City_WorkingPrefab/CosmeticsRoomAnchor/nicegorillastore_prefab/DressingRoom_Mirrors_Prefab/ShoppingCart/Anchor").GetComponentsInChildren<FittingRoomButton>())
            {
                b.ButtonActivationWithHand(false);
            }
            NewFlusher();
            await Task.Delay(500);
        }

        public static void newthingoff()
        {
            if (newthingbool)
            {
                CosmeticsController.instance.currentWornSet = new CosmeticsController.CosmeticSet(archiveCosmetics, CosmeticsController.instance);
                GorillaTagger.Instance.offlineVRRig.cosmeticSet = new CosmeticsController.CosmeticSet(archiveCosmetics, CosmeticsController.instance);
                foreach (var cosmetic in archiveCosmetics)
                {
                    UnityEngine.Debug.Log(cosmetic);
                }
                GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", RpcTarget.All, new object[] { archiveCosmetics, CosmeticsController.instance.tryOnSet.ToDisplayNameArray() });
                newthingbool = false;
            }
        }

        public static bool newthingbool;

        public static void newthing()
        {
            if (!newthingbool)
            {
                newthingbool = true;
                archiveCosmetics = CosmeticsController.instance.currentWornSet.ToDisplayNameArray();
                for (int i = 16; i >= 0; i--)
                {
                    if (archiveCosmetics.Length != 16)
                    {
                        archiveCosmetics.AddItem("LMAJU.");
                        UnityEngine.Debug.Log("added item | " + archiveCosmetics.Length);
                    }
                    else
                        break;
                }
                CosmeticsController.instance.currentWornSet = new CosmeticsController.CosmeticSet(new string[] { }, CosmeticsController.instance);
                //GorillaTagger.Instance.offlineVRRig.cosmeticSet = new CosmeticsController.CosmeticSet(new string[] { }, CosmeticsController.instance);
                GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", RpcTarget.All, new object[] { new string[16] { "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU." }, new string[16] { "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU." } });
                NotifiLib.SendNotification("<color=blue>[GENESIS]</color> Finished! Walk out of the tryon area and everyone can see the cosmetics that u put on when u clicked it!");
            }
        }

        static bool newthing3;

        public static TappableGuardianIdol[] archivetgi = null;

        public static float lastRecievedTime;
        public static float dumbdelay;
        public static float dumbdelay3;

        public static TappableGuardianIdol[] GetGuardianIdols()
        {
            if (Time.time > lastRecievedTime)
            {
                archivetgi = null;
                lastRecievedTime = Time.time + 30f;
            }
            if (archivetgi == null)
            {
                archivetgi = UnityEngine.Object.FindObjectsOfType<TappableGuardianIdol>();
            }
            return archivetgi;
        }

        public static void GrabGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > dumbdelay)
                {
                    if (data.lockedPlayer && data.lockedPlayer != GorillaTagger.Instance.offlineVRRig)
                    {
                        dumbdelay = Time.time + 0.1f;
                        GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                        if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                        {
                            RigShit.GetNetViewFromRig(data.lockedPlayer).SendRPC("GrabbedByPlayer", RigShit.GetPlayerFromRig(data.lockedPlayer), new object[] { true, false, false });
                            NewFlusher();
                        }
                        else
                        {
                            NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                        }
                    }
                }
            }
        }

        public static void GrabAll()
        {
            if (Time.time > dumbdelay && WristMenu.gripDownR)
            {
                dumbdelay = Time.time + 0.1f;
                GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                {

                    GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RpcTarget.All, new object[] { true, false, false });

                    NewFlusher();
                }
                else
                {
                    NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                }
            }
        }

        static NetPlayer crashedplayer;

        public static void CrashGunGuard()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > dumbdelay)
                {
                    if (data.lockedPlayer && !data.lockedPlayer.isOfflineVRRig)
                    {
                        GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                        if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                        {
                            dumbdelay = Time.time + 5f;
                            isCrashing = true;
                            crashedplayer = RigShit.GetNetPlayerFromRig(data.lockedPlayer);
                            GorillaTagger.Instance.StartCoroutine(CrashGunRoutine());
                        }
                        else
                        {
                            NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                        }
                    }
                }
            }
        }

        public static void CrashAllGuard()
        {
            if (Time.time > dumbdelay)
            {
                dumbdelay = Time.time + 5f;
                GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                {
                    isCrashing = true;
                    GorillaTagger.Instance.StartCoroutine(CrashAllRoutine());
                }
                else
                {
                    NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                }
            }
        }

        private static System.Collections.IEnumerator CrashAllRoutine()
        {
            NewFlusher();
            GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RpcTarget.All, new object[] { true, false, false });
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-61.036f, 68.9908f, -59.5745f);
            GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RpcTarget.All, new object[] { true, false, false });
            yield return new WaitForSeconds(1f);
            GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RpcTarget.All, new object[] { true, false, false });
            yield return new WaitForSeconds(2f);
            NewFlusher();
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RpcTarget.All, new object[] { true, false, false });
            GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-82.0279f, 77.8523f, -4.5606f);
            yield return new WaitForSeconds(1f);
            GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RpcTarget.All, new object[] { true, false, false });
            yield return new WaitForSeconds(2f);
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RpcTarget.All, new object[] { new Vector3(0, -55, 0) });
            yield return new WaitForSeconds(1f);
            GorillaTagger.Instance.offlineVRRig.enabled = true;
            NewFlusher();
            isCrashing = false;
        }

        private static System.Collections.IEnumerator CrashGunRoutine()
        {
            NewFlusher();
            isCrashing = true;
            GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", crashedplayer, new object[] { true, false, false });
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-61.036f, 68.9908f, -59.5745f);
            GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", crashedplayer, new object[] { true, false, false });
            yield return new WaitForSeconds(1f);
            GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", crashedplayer, new object[] { true, false, false });
            yield return new WaitForSeconds(2f);
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            NewFlusher();
            GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", crashedplayer, new object[] { true, false, false });
            GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-82.0279f, 77.8523f, -4.5606f);
            yield return new WaitForSeconds(1f);
            GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", crashedplayer, new object[] { true, false, false });
            yield return new WaitForSeconds(2f);
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", crashedplayer, new object[] { new Vector3(0, -55, 0) });
            yield return new WaitForSeconds(1f);
            GorillaTagger.Instance.offlineVRRig.enabled = true;
            NewFlusher();
            isCrashing = false;
            crashedplayer = null;
        }


        public static void ReleaseAll()
        {
            if (Time.time > dumbdelay && WristMenu.triggerDownR)
            {
                dumbdelay = Time.time + 0.1f;
                GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                {
                    GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RpcTarget.All, new object[] { new Vector3(0, -55, 0) });

                    NewFlusher();
                }
                else
                {
                    NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                }
            }
        }

        public static void FlingAll()
        {
            if (Time.time > dumbdelay)
            {
                dumbdelay = Time.time + 0.1f;
                GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                {
                    GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RpcTarget.Others, new object[] { new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20)) });

                    NewFlusher();
                }
                else
                {
                    NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                }
            }
        }

        public static void FlingAll2()
        {
            if (Time.time > dumbdelay)
            {
                dumbdelay = Time.time + 0.1f;
                GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                {

                    GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RpcTarget.Others, new object[] { true, false, false });
                    GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RpcTarget.Others, new object[] { new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20)) });

                    NewFlusher();
                }
                else
                {
                    NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                }
            }
        }

        public static void KillIRLAll()
        {
            if (Time.time > dumbdelay)
            {
                dumbdelay = Time.time + 0.2f;
                GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                {

                    GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RpcTarget.Others, new object[] { true, true, true });
                    GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RpcTarget.Others, new object[] { new Vector3(float.NaN, float.NaN, float.NaN) });

                    NewFlusher();
                }
                else
                {
                    NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                }
            }
        }

        public static void BlackAll()
        {
            if (Time.time > dumbdelay)
            {
                dumbdelay = Time.time + 2f;
                GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                {

                    GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RpcTarget.Others, new object[] { false, true, true });
                    GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RpcTarget.Others, new object[] { new Vector3(-333333, float.PositiveInfinity, float.PositiveInfinity) });

                    NewFlusher();
                }
                else
                {
                    NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                }
            }
        }

        public static void KillIRLGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > dumbdelay)
                {
                    dumbdelay = Time.time + 2f;
                    GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                    if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                    {
                        if (data.lockedPlayer != GorillaTagger.Instance.offlineVRRig)
                        {
                            GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RigShit.GetNetPlayerFromRig(data.lockedPlayer), new object[] { true, true, true });
                            GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RigShit.GetNetPlayerFromRig(data.lockedPlayer), new object[] { new Vector3(float.NaN, float.NaN, float.NaN) });
                        }

                        NewFlusher();
                    }
                    else
                    {
                        NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                    }
                }
            }
        }

        public static void BlackGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > dumbdelay)
                {
                    dumbdelay = Time.time + 0.2f;
                    GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                    if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                    {
                        if (data.lockedPlayer != GorillaTagger.Instance.offlineVRRig)
                        {
                            GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RigShit.GetNetPlayerFromRig(data.lockedPlayer), new object[] { true, false, false });
                            GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RigShit.GetNetPlayerFromRig(data.lockedPlayer), new object[] { new Vector3(-333333, float.PositiveInfinity, float.PositiveInfinity) });
                        }

                        NewFlusher();
                    }
                    else
                    {
                        NotifiLib.SendNotification("<color=blue>[genesis]</color> You must be a guardian");
                    }
                }
            }
        }

        public static bool isCrashing;

        public static void AlwaysGuardian()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (Time.time > dumbdelay3)
                {
                    dumbdelay3 = Time.time + 5f;
                    foreach (GorillaGuardianZoneManager gorillaGuardianZoneManager in GorillaGuardianZoneManager.zoneManagers)
                    {
                        if (gorillaGuardianZoneManager.enabled)
                        {
                            gorillaGuardianZoneManager.SetGuardian(NetworkSystem.Instance.LocalPlayer);
                            NewFlusher();
                        }
                    }
                }
            }
            else
            {
                foreach (TappableGuardianIdol tgi in GetGuardianIdols())
                {
                    if (tgi.isChangingPositions == false)
                    {
                        GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                        if (!gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer)) // gzm.enabled && 
                        {
                            GorillaTagger.Instance.offlineVRRig.enabled = false;
                            GorillaTagger.Instance.offlineVRRig.transform.position = tgi.transform.position;

                            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));
                            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));

                            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.forward * 3f;
                            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.forward * 3f;
                            //float loltime = (float)Traverse.Create(gzm).Field("_currentActivationTime").GetValue();
                            NewFlusher();
                            tgi.manager.photonView.RPC("SendOnTapRPC", RpcTarget.All, tgi.tappableId, UnityEngine.Random.Range(0.2f, 0.4f));
                        }
                    }
                    else
                    {
                        if (!isCrashing)
                            GorillaTagger.Instance.offlineVRRig.enabled = true;
                    }

                    if (tgi.isChangingPositions == true)
                    {
                        if (!isCrashing)
                            GorillaTagger.Instance.offlineVRRig.enabled = true;
                    }
                }
            }
        }

        public static void AntiGuardian()
        {
            foreach (TappableGuardianIdol tgi in GetGuardianIdols())
            {
                if (tgi.isChangingPositions == false)
                {
                    GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                    if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                    {
                        foreach (VRRig rig in VRRigCache.ActiveRigs)
                        {
                            if (!rig.isOfflineVRRig)
                            {
                                if (Vector3.Distance(rig.transform.position, tgi.gameObject.transform.position) < 2.5f)
                                {
                                    Vector3 e = rig.transform.position - new Vector3(0, 0.3f, 0) + -rig.transform.forward * 1f;
                                    GorillaTagger.Instance.myVRRig.SendRPC("GrabbedByPlayer", RigShit.GetPlayerFromRig(rig), new object[] { true, false, false });
                                    GorillaTagger.Instance.myVRRig.SendRPC("DroppedByPlayer", RigShit.GetPlayerFromRig(rig), new object[] { e });
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void GuardianAll()
        {
            try
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    int i = 0;
                    foreach (GorillaGuardianZoneManager gorillaGuardianZoneManager in GorillaGuardianZoneManager.zoneManagers)
                    {
                        if (gorillaGuardianZoneManager.enabled)
                        {
                            gorillaGuardianZoneManager.SetGuardian(PhotonNetwork.PlayerList[i]);
                            i++;
                        }
                    }
                }
                else { NotifiLib.SendNotification("Become master!"); }
            }
            catch ( Exception e )
            { }
        }

        static float thingdelay1;
        static float thingdelay2;

        public static void Effect(bool slam, bool slap, Vector3 pos, Vector3 dir)
        {
            GorillaGuardianManager gman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
            if (gman.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
            {
                NetworkView view = Traverse.Create(typeof(GameMode)).Field("activeNetworkHandler").Field("netView").GetValue<NetworkView>();
                if (thingdelay1 < Time.time)
                {
                    thingdelay1 = Time.time + 0.08f;
                    if (slap)
                        view.SendRPC("ShowSlapEffects", RpcTarget.All, new object[] { pos, dir });

                    if (slam)
                        view.SendRPC("ShowSlamEffect", RpcTarget.All, new object[] { pos, dir });
                }
                NewFlusher();
            }
            else
            {
                NotifiLib.SendNotification("Become guardian!");
            }
        }

        public static void BothEffectsGun()
        {
            var data = GunLib.Shoot();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                Effect(true, true, data.hitPosition, GorillaTagger.Instance.offlineVRRig.transform.forward);
            }
        }

        public static void SlamEffectsGun()
        {
            var data = GunLib.Shoot();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                Effect(true, false, data.hitPosition, GorillaTagger.Instance.offlineVRRig.transform.forward);
            }
        }

        public static void SlapEffectsGun()
        {
            var data = GunLib.Shoot();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                Effect(false, true, data.hitPosition, GorillaTagger.Instance.offlineVRRig.transform.forward); 
            }
        }

        public static void BothEffects()
        {
            if (WristMenu.gripDownR)
                Effect(true, true, GorillaTagger.Instance.offlineVRRig.rightHandTransform.position, GorillaTagger.Instance.offlineVRRig.rightHandTransform.forward);
        }

        public static void SlamEffect()
        {
            if (WristMenu.gripDownR)
                Effect(true, false, GorillaTagger.Instance.offlineVRRig.rightHandTransform.position, GorillaTagger.Instance.offlineVRRig.rightHandTransform.forward);
        }

        public static void SlapEffect()
        {
            if (WristMenu.gripDownR)
                Effect(false, true, GorillaTagger.Instance.offlineVRRig.rightHandTransform.position, GorillaTagger.Instance.offlineVRRig.rightHandTransform.forward);
        }

        private static float angle;
        private static float orbitSpeed = 12f;

        public static void SlapEffectsHalo()
        {
            angle += orbitSpeed * Time.deltaTime;
            float x = GorillaTagger.Instance.offlineVRRig.transform.position.x + 1f * Mathf.Cos(angle);
            float y = GorillaTagger.Instance.offlineVRRig.transform.position.y + 1f;
            float z = GorillaTagger.Instance.offlineVRRig.transform.position.z + 1f * Mathf.Sin(angle);
            Vector3 funny = new Vector3(x, y, z);
            Effect(false, true, funny, new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)));
        }

        public static void SlamEffectsHalo()
        {
            angle += orbitSpeed * Time.deltaTime;
            float x = GorillaTagger.Instance.offlineVRRig.transform.position.x + 1f * Mathf.Cos(angle);
            float y = GorillaTagger.Instance.offlineVRRig.transform.position.y + 1f;
            float z = GorillaTagger.Instance.offlineVRRig.transform.position.z + 1f * Mathf.Sin(angle);
            Vector3 funny = new Vector3(x, y, z);
            Effect(true, false, funny, new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)));
        }

        public static void BothEffectsHalo()
        {
            angle += orbitSpeed * Time.deltaTime;
            float x = GorillaTagger.Instance.offlineVRRig.transform.position.x + 1f * Mathf.Cos(angle);
            float y = GorillaTagger.Instance.offlineVRRig.transform.position.y + 1f;
            float z = GorillaTagger.Instance.offlineVRRig.transform.position.z + 1f * Mathf.Sin(angle);
            Vector3 funny = new Vector3(x, y, z);
            Effect(true, true, funny, new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)));
        }

        public static void FreezeGun()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("<color=red>[FREEZE]</color> Become master!");
                return;
            }
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                foreach (MonkeyeAI ai in UnityEngine.Object.FindObjectsOfType<MonkeyeAI>())
                {
                    PhotonView view = (PhotonView)Traverse.Create(ai).Field("_view").GetValue();
                    view.GetComponent<RequestableOwnershipGuard>().TransferOwnership(PhotonNetwork.LocalPlayer, "");
                    ai.SetChasePlayer(data.lockedPlayer);
                    ai.transform.position = data.lockedPlayer.transform.position;
                }
            }
        }



        public static GTColor.HSVRanges braceletRandomColorHSVRanges;

        public static Color myBraceletColor;

        public static void Drake()
        {
            if (WristMenu.gripDownR)
            {
                Vector3 pos = GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position;
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-49.4993f, 18.2303f, -117.3594f);
                if (CrashDelay < Time.time)
                {
                    CrashDelay = Time.time + 1f;
                    PaperPlaneThrowable ez;
                    if (GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/FireballAnchor/LMAJM.") == null)
                        ez = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/body/shoulder.R/upper_arm.R/forearm.R/Right Arm Item Anchor/DropZoneAnchor/FireballAnchor/LMAJM.").GetComponent<PaperPlaneThrowable>();
                    else
                        ez = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/FireballAnchor/LMAJM.").GetComponent<PaperPlaneThrowable>();

                    GorillaTagger.Instance.offlineVRRig.SetActiveTransferrableObjectIndex(1, 202);
                    GorillaTagger.Instance.offlineVRRig.SetTransferrablePosStates(1, TransferrableObject.PositionState.OnRightArm);
                    GorillaTagger.Instance.offlineVRRig.SetTransferrableItemStates(1, (TransferrableObject.ItemStates)0);
                    GorillaTagger.Instance.offlineVRRig.SetTransferrableDockPosition(1, BodyDockPositions.DropPositions.RightArm);
                    GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.EnableTransferrableGameObject(202, BodyDockPositions.DropPositions.RightArm, TransferrableObject.PositionState.OnRightArm);


                    int num = GorillaTagger.Instance.myVRRig.ViewID;
                    Vector3 vector = pos;
                    Quaternion rotation = Quaternion.identity;

                    Traverse.Create(ez).Field("_renderer").GetValue<Renderer>().forceRenderingOff = false;
                    Traverse.Create(ez).Field("gLaunchRPC").GetValue<PhotonEvent>().RaiseAll(new object[] { num, vector, rotation, GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.forward * Time.deltaTime * 0.1f });

                    GorillaTagger.Instance.offlineVRRig.enabled = true;

                    NewFlusher();
                }
            }
            if (WristMenu.gripDownL)
            {
                Vector3 pos = GorillaLocomotion.GTPlayer.Instance.leftHand.controllerTransform.position;
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-49.4993f, 18.2303f, -117.3594f);
                if (CrashDelay < Time.time)
                {
                    CrashDelay = Time.time + 1f;
                    PaperPlaneThrowable ez;
                    if (GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/FireballAnchor/LMAJM.") == null)
                        ez = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/body/shoulder.R/upper_arm.R/forearm.R/Right Arm Item Anchor/DropZoneAnchor/FireballAnchor/LMAJM.").GetComponent<PaperPlaneThrowable>();
                    else
                        ez = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/FireballAnchor/LMAJM.").GetComponent<PaperPlaneThrowable>();

                    

                    GorillaTagger.Instance.offlineVRRig.SetActiveTransferrableObjectIndex(1, 202);
                    GorillaTagger.Instance.offlineVRRig.SetTransferrablePosStates(1, TransferrableObject.PositionState.OnRightArm);
                    GorillaTagger.Instance.offlineVRRig.SetTransferrableItemStates(1, (TransferrableObject.ItemStates)0);
                    GorillaTagger.Instance.offlineVRRig.SetTransferrableDockPosition(1, BodyDockPositions.DropPositions.RightArm);
                    GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.EnableTransferrableGameObject(202, BodyDockPositions.DropPositions.RightArm, TransferrableObject.PositionState.OnRightArm);


                    int num = GorillaTagger.Instance.myVRRig.ViewID;
                    Vector3 vector = pos;
                    Quaternion rotation = Quaternion.identity;

                    Traverse.Create(ez).Field("_renderer").GetValue<Renderer>().forceRenderingOff = false;
                    Traverse.Create(ez).Field("gLaunchRPC").GetValue<PhotonEvent>().RaiseAll(new object[] { num, vector, rotation, GorillaLocomotion.GTPlayer.Instance.leftHand.controllerTransform.forward * Time.deltaTime * 0.5f });

                    GorillaTagger.Instance.offlineVRRig.enabled = true;

                    NewFlusher();
                }
            }
        }

        public static void LagGun(int version)
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                LagView(data.lockedPlayer, false, version);
            }
        }

        public static void CrashGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                LagView(data.lockedPlayer, false, 1, true);
            }
        }

        public static void LagGun2()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                LagView2(data.lockedPlayer, false);

            }
        }

        public static void MuteGunSS()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                LagView2(data.lockedPlayer, true);

            }
        }

        public static void NameAll()
        {
            //if (NameChangerString == null)
            ////{
            //    if (SettingNameDelay < Time.time)
                //{
                    //SettingNameDelay = Time.time + 5f;
                    //NotifiLib.SendNotification("<color=blue>[NAME CHANGER]</color> Assign the thing to change their names to on the gui!");
                    //return;
                //}
            //}
            if (!PhotonNetwork.CurrentRoom.CustomProperties.ToString().Contains("MODDED"))
            {
                //NotifiLib.SendNotification("<color=red>[CRASH]</color> Turn on antiban!");
                //return;
            }
            if (NameDelay < Time.time)
            {
                NameDelay = Time.time + 0.05f;
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable[byte.MaxValue] = PhotonNetwork.LocalPlayer.NickName;
                    Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
                    dictionary.Add(251, hashtable);
                    dictionary.Add(254, p.ActorNumber);
                    dictionary.Add(250, true);
                    PhotonNetwork.CurrentRoom.LoadBalancingClient.LoadBalancingPeer.SendOperation(252, dictionary, SendOptions.SendUnreliable);
                }
                NewFlusher();
            }
        }

        public static void AntibanStatus()
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ToString().Contains("MODDED") && !PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("<color=green>[ANTIBAN STATUS]</color> Antiban is enabled, but not set master.");
                return;
            }
            if (PhotonNetwork.CurrentRoom.CustomProperties.ToString().Contains("MODDED") && PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("<color=green>[ANTIBAN STATUS]</color> Antiban is enabled, and you're master!");
                return;
            }
            if (!PhotonNetwork.CurrentRoom.CustomProperties.ToString().Contains("MODDED"))
            {
                NotifiLib.SendNotification("<color=green>[ANTIBAN STATUS]</color> Antiban is NOT enabled.");
                return;
            }
            if (PhotonNetwork.IsMasterClient)
            {
                NotifiLib.SendNotification("<color=green>[ANTIBAN STATUS]</color> Antiban is NOT enabled, but you're master!");
            }
        }

        public static void AutoSetMaster()
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ToString().Contains("MODDED"))
            {
                if (!AutoMasterBool)
                {
                    PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer);
                    NotifiLib.SendNotification("<color=red>[SET MASTER]</color> Set Master enabled!");
                    AutoMasterBool = true;
                }
            }
        }

        static float ihave2kidsinmycloset;

        static List<RequestableOwnershipGuard> archGuard = new List<RequestableOwnershipGuard>();
        static float archfloat;
        public static RequestableOwnershipGuard GetRandomGuard()
        {
            if (Time.time > archfloat)
            {
                archfloat = Time.time + 10f;
                archGuard.Clear();
            }
            if (archGuard.Count < 2)
            {
                foreach (RequestableOwnershipGuard r in UnityEngine.Object.FindObjectsOfType<RequestableOwnershipGuard>())
                {
                    if (r.gameObject.activeSelf)
                        archGuard.Add(r);
                }
            }
            return archGuard[Random.Range(0, archGuard.Count)];
        }

        public static void Kick(VRRig ptokick)
        {
            if (Time.time > ihave2kidsinmycloset)
            {
                Player p = RigShit.GetPlayerFromRig(ptokick);
                ihave2kidsinmycloset = Time.time + 0.1f;
                RequestableOwnershipGuard requestable = GetRandomGuard();
                if (requestable.currentState == NetworkingState.IsClient)
                    requestable.photonView.RPC("OwnershipRequested", requestable.actualOwner.GetPlayerRef(), new object[] { requestable.ownershipRequestNonce });

                if (requestable.gameObject != null && requestable.gameObject.activeSelf)
                {
                    if (requestable.currentState == NetworkingState.IsOwner)
                    {
                        var view = Traverse.Create(requestable).Property("netView").GetValue<NetworkView>();
                        if (view != null)
                        {
                            requestable.TransferOwnership(p, Guid.NewGuid().ToString());
                            view.SendRPC("OwnershipRequested", p, new object[] { "TTTTTTTTTTTT".PadRight(1962) });
                        }
                    }
                }
                NewFlusher();
            }
        }

        public static void KickGun()
        {
            var data = GunLib.ShootLock();
            if (data.isLocked && data.isTriggered && Time.time > delay3)
            {
                if (!data.lockedPlayer.isOfflineVRRig)
                {
                    NetPlayer player = RigShit.GetPlayerFromRig(data.lockedPlayer);
                    delay3 = Time.time + 0.5f;

                    bool sessionIsPrivate = NetworkSystem.Instance.SessionIsPrivate;
                    if (sessionIsPrivate)
                        SetRoomStatus(false);

                    CoroutineRunner.instance.StartCoroutine(KickDelay(() =>
                    {
                        PhotonNetworkController.Instance.shuffler = UnityEngine.Random.Range(0, 99).ToString().PadLeft(2, '0') + UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');
                        PhotonNetworkController.Instance.keyStr = UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');

                        BetaShuttleFollowCommand(RigShit.NetPlayerToPlayer(player));
                        NewFlusher();
                    }, () =>
                    {
                        GorillaComputer.instance.primaryTriggersByZone.TryGetValue(GorillaComputer.instance.allowedMapsToJoin[0], out GorillaNetworkJoinTrigger trigger);
                        PhotonNetworkController.Instance.AttemptToJoinPublicRoom(trigger, GorillaNetworking.JoinType.JoinWithElevator);
                    }, sessionIsPrivate ? 0.5f : 0f));
                }
            }
        }
        public static void SetRoomStatus(bool status) // <3 kingofnetflix
        {
            Dictionary<byte, object> operationParameters = new Dictionary<byte, object>
            {
                {
                    251,
                    new ExitGames.Client.Photon.Hashtable {
                    {
                        254,
                        !status
                    } }
                },
                { 250, true },
                { 231, null }
            };
            PhotonNetwork.CurrentRoom.LoadBalancingClient.LoadBalancingPeer.SendOperation(252, operationParameters, SendOptions.SendReliable);
            string nickName = PhotonNetwork.LocalPlayer.NickName;
            PhotonNetwork.LocalPlayer.NickName = "update";
            PhotonNetwork.LocalPlayer.NickName = nickName;
        }
        public static string GenerateRandomString(int length = 4)
        {
            string random = "";
            for (int i = 0; i < length; i++)
            {
                int rand = UnityEngine.Random.Range(0, 36);
                char c = rand < 26
                    ? (char)('A' + rand)
                    : (char)('0' + (rand - 26));
                random += c;
            }

            return random;
        }
        public static System.Collections.IEnumerator KickDelay(Action action, Action action2, float extraDelay = 0f, bool changeQueue = true)
        {
            PhotonNetworkController.Instance.FriendIDList.Clear();
            yield return new WaitForSeconds(extraDelay);

            string queueArchive = GorillaComputer.instance.currentQueue;
            if (changeQueue)
                GorillaComputer.instance.currentQueue = GenerateRandomString();

            action?.Invoke();
            yield return new WaitForSeconds(0.3f);
            action2?.Invoke();
            yield return new WaitForSeconds(1f);

            if (changeQueue)
                GorillaComputer.instance.currentQueue = queueArchive;

            yield return new WaitForSeconds(30f);
        }

        public static void KickAll()
        {
            if (PhotonNetwork.InRoom)
            {
                bool sessionIsPrivate = NetworkSystem.Instance.SessionIsPrivate;
                if (sessionIsPrivate)
                    SetRoomStatus(false);

                CoroutineRunner.instance.StartCoroutine(KickDelay(() =>
                {
                    PhotonNetworkController.Instance.shuffler = UnityEngine.Random.Range(0, 99).ToString().PadLeft(2, '0') + UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');
                    PhotonNetworkController.Instance.keyStr = UnityEngine.Random.Range(0, 99999999).ToString().PadLeft(8, '0');

                    foreach (VRRig rig in VRRigCache.ActiveRigs.Where(rig => !rig.isOfflineVRRig))
                        BetaShuttleFollowCommand(RigShit.NetPlayerToPlayer(RigShit.GetPlayerFromRig(rig)));

                    NewFlusher();
                }, () =>
                {
                    GorillaComputer.instance.primaryTriggersByZone.TryGetValue(GorillaComputer.instance.allowedMapsToJoin[0], out GorillaNetworkJoinTrigger trigger);
                    PhotonNetworkController.Instance.AttemptToJoinPublicRoom(trigger, GorillaNetworking.JoinType.JoinWithElevator);
                }, sessionIsPrivate ? 0.5f : 0f));
            }
            else
                NotifiLib.SendNotification("You must be in a room.");
        }

        public static void BetaShuttleFollowCommand(Player player)
        {
            if (NetworkSystem.Instance.SessionIsPrivate)
            {
                NotifiLib.SendNotification("You must be in a public room.");
                return;
            }

            PhotonNetworkController.Instance.FriendIDList.Add(player.UserId);

            object[] groupJoinSendData = new object[2];
            groupJoinSendData[0] = PhotonNetworkController.Instance.shuffler;
            groupJoinSendData[1] = PhotonNetworkController.Instance.keyStr;
            NetEventOptions netEventOptions = new NetEventOptions { TargetActors = new[] { player.ActorNumber } };

            RoomSystem.SendEvent(11, groupJoinSendData, netEventOptions, false);
        }

        static float kickerflaot;

        public static void CEREAL(Player p, bool all = false)
        {
            if (Time.time > kickerflaot)
            {
                kickerflaot = Time.time + 1f;
                PhotonView cerealmmyummy = GameObject.Find("Environment Objects/LocalObjects_Prefab/VirtualStump_CustomMapLobby/ModIOMapsTerminal/NetworkObject").GetComponent<PhotonView>();

                if (all)
                {
                    cerealmmyummy.RPC("SetRoomMapRPC", RpcTarget.Others, new object[] { (long)4258844 });

                    cerealmmyummy.RPC("UnloadMapRPC", RpcTarget.Others, new object[] { });
                }
                else
                {
                    cerealmmyummy.RPC("SetRoomMapRPC", p, new object[] { (long)4258844 });

                    cerealmmyummy.RPC("UnloadMapRPC", p, new object[] { });
                }

                NewFlusher();
            }
        }
        

        static float flusherDelay;

        public static void NewFlusher()
        {
            if (flusherDelay < Time.time)
            {
                flusherDelay = Time.time + 0.5f;
                PhotonNetwork.MaxResendsBeforeDisconnect = int.MaxValue;
                PhotonNetwork.OpCleanActorRpcBuffer(PhotonNetwork.LocalPlayer.ActorNumber);
                PhotonNetwork.SendAllOutgoingCommands();
                MonkeAgent.instance.OnPlayerLeftRoom(PhotonNetwork.LocalPlayer);
            }
        }

        public static CosmeticsController conskib = null;

        public static void LagView2(VRRig pv, bool nodelay)
        {
            if (CrashDelay < Time.time)
            {
                if (!nodelay)
                    CrashDelay = Time.time + 0.03f;

                PhotonNetwork.RaiseEvent(0, null, new RaiseEventOptions
                {
                    CachingOption = EventCaching.DoNotCache,
                    TargetActors = new int[]
                    {
                                pv.OwningNetPlayer.ActorNumber
                    }
                }, SendOptions.SendReliable);

                PhotonView view = RigShit.GetViewFromRig(pv);
                view.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;

                PhotonNetwork.Destroy(view); // Trust
                NewFlusher();
            }
        }

        static float newdleay;

        public static void LagView(VRRig pv, bool all = false, int version = 1, bool both = false)
        {
            if (Time.time > newdleay)
            {
                if (!both)
                    newdleay = Time.time + 0.050f;
                else
                    newdleay = Time.time + 0.1f;
                for (int i = 0; i < 27; i++)
                {
                    if (all)
                    {
                        if (both)
                        {
                            FriendshipGroupDetection.Instance.photonView.RPC("RequestPartyGameMode", RpcTarget.Others, new object[] { "Infection" } );
                            FriendshipGroupDetection.Instance.photonView.RPC("NotifyPartyGameModeChanged", RpcTarget.Others, new object[] { "Infection" } );
                            NewFlusher();
                            return;
                        }
                        if (version == 1)
                            FriendshipGroupDetection.Instance.photonView.RPC("RequestPartyGameMode", RpcTarget.Others, new object[] { "Infection" });
                        else
                            FriendshipGroupDetection.Instance.photonView.RPC("NotifyPartyGameModeChanged", RpcTarget.Others, new object[] { "Infection" });
                    }
                    else
                    {
                        if (both)
                        {
                            FriendshipGroupDetection.Instance.photonView.RPC("RequestPartyGameMode", RigShit.GetPlayerFromRig(pv), new object[] { "Infection" });
                            FriendshipGroupDetection.Instance.photonView.RPC("NotifyPartyGameModeChanged", RigShit.GetPlayerFromRig(pv), new object[] { "Infection" });
                            NewFlusher();
                            return;
                        }
                        if (version == 1)
                            FriendshipGroupDetection.Instance.photonView.RPC("RequestPartyGameMode", RigShit.GetPlayerFromRig(pv), new object[] { "Infection" });
                        else
                            FriendshipGroupDetection.Instance.photonView.RPC("NotifyPartyGameModeChanged", RigShit.GetPlayerFromRig(pv), new object[] { "Infection" });
                    } 
                }
                NewFlusher();
            }
        }

        public static void SetMaster()
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }

        public static void Antiban()
        {
            Hashtable propertiesToSet = new Hashtable
             {
                 {
                     "gameMode",
                     "forestCOMPETITIVEINFECTIONMODDED_MODDED"
                 },
                 {

                     "platform",
                     "OTHER"
                 }
             };
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
            {

                FunctionName = "RoomClosed",
                FunctionParameter = new
                {
                    GameId = PhotonNetwork.CurrentRoom.Name,
                    AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime,
                    Region = Regex.Replace(PhotonNetwork.CloudRegion, "[^a-zA-Z0-9]", "").ToUpper(),
                    UserId = PhotonNetwork.MasterClient.UserId,
                    ActorNr = PhotonNetwork.MasterClient.ActorNumber,
                    AppVersion = PhotonNetwork.AppVersion,
                    Type = "ClientDisconnect"
                }
            }, delegate (PlayFab.ClientModels.ExecuteCloudScriptResult result)
            {
                NotifiLib.SendNotification("<color=red>[ANTIBAN]</color> ran antiban holy moly");
                PhotonNetwork.CurrentRoom.SetCustomProperties(propertiesToSet, null, null);
            }, delegate (PlayFabError error)
            {
                UnityEngine.Debug.Log(error.Error);
            }, null, null);
        }


            public static void DisableTrigs()
        {
            if (PhotonNetwork.CurrentRoom.IsOpen == false)
            {
                NotifiLib.SendNotification("<color=red>[ANTIBAN]</color> Detected in privates!");
                return;
            }

            if (IsModded())
            {
                Hashtable hash = new Hashtable();
                hash.Add("gameMode", "DEFAULT_MODDED_MODDED_forestcitybasementcanyonsmountainsbeachskycaves_INFECTION");
                GorillaGameManager.instance.checkCooldown = 0f;
                PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetCustomPropertiesOfRoom(hash, null, null);
            }
            else
            {
                NotifiLib.SendNotification("<color=red>[DISABLE TRIGS]</color> Activate Antiban!");
            }
        }
    }
}
