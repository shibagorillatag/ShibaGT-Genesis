using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Photon.Realtime;
using Genesis.UI;
using GorillaLocomotion.Gameplay;
using HarmonyLib;
using System.Linq;
using Backtrace.Unity.Model.Breadcrumbs;

namespace Genesis.Utilities
{
    internal class RigShit : MonoBehaviour
    {
        public static Player NetPlayerToPlayer(NetPlayer np)
        {
            return np.GetPlayerRef();
        }

        public static NetPlayer PlayerToNetPlayer(Player np)
        {
            foreach (NetPlayer p in NetworkSystem.Instance.AllNetPlayers)
            {
                if (np.UserId == p.UserId)
                {
                    return p;
                }
            }
            return null;
        }

        public static NetworkView GetNetViewFromVRRig(VRRig p)
        {
            return (NetworkView)Traverse.Create(p).Field("netView").GetValue();
        }

        public static VRRig GetRigFromPlayer(Photon.Realtime.Player p)
        {
            foreach (NetPlayer p2 in NetworkSystem.Instance.AllNetPlayers)
            {
                if (p2.UserId == p.UserId)
                {
                    return RigShit.GetRigFromNetPlayer(p2);
                }
            }
            return null;
        }

        public static VRRig GetRigFromNetPlayer(NetPlayer p)
        {
            return GorillaGameManager.instance.FindPlayerVRRig(p);
        }

        public static PhotonView GetViewFromPlayer(Photon.Realtime.Player p)
        {
            return WristMenu.rig2view(GorillaGameManager.instance.FindPlayerVRRig(p));
        }

        public static VRRig GetOwnVRRig()
        {
            return GorillaTagger.Instance.offlineVRRig;
        }

        public static PhotonView GetViewFromRig(VRRig rig)
        {
            return WristMenu.rig2view(rig);
        }

        public static NetworkView GetNetViewFromRig(VRRig rig)
        {
            return rig2netview(rig);
        }

        public static NetPlayer GetPlayerFromID(string id)
        {
            NetPlayer found = null;
            foreach (Photon.Realtime.Player target in PhotonNetwork.PlayerList)
            {
                if (target.UserId == id)
                {
                    found = target;
                    break;
                }
            }
            return found;
        }

        public static NetworkView rig2netview(VRRig p)
        {
            return Traverse.Create(p).Field("netView").GetValue<NetworkView>();
        }

        public static Photon.Realtime.Player GetPlayerFromRig(VRRig rig)
        {
            return rig.OwningNetPlayer.GetPlayerRef();
        }

        public static NetPlayer GetNetPlayerFromRig(VRRig rig)
        {
            return rig.OwningNetPlayer;
        }

        public static GorillaRopeSwing GetPlayersRope(VRRig rig)
        {
            return (GorillaRopeSwing)Traverse.Create(rig).Field("currentRopeSwing").GetValue();
        }

        private float Distance2D(Vector3 a, Vector3 b)
        {
            Vector2 a2 = new Vector2(a.x, a.z);
            Vector2 b2 = new Vector2(b.x, b.z);
            return Vector2.Distance(a2, b2);
        }

        private RaycastHit[] rayResults = new RaycastHit[1];
        private LayerMask layerMask;

        private bool PlayerNear(VRRig rig, float dist, out float playerDist)
        {
            if (rig == null)
            {
                playerDist = float.PositiveInfinity;
                return false;
            }
            playerDist = this.Distance2D(rig.transform.position, base.transform.position);
            return playerDist < dist && Physics.RaycastNonAlloc(new Ray(base.transform.position, rig.transform.position - base.transform.position), this.rayResults, playerDist, this.layerMask) <= 0;
        }

        private bool ClosestPlayer(in Vector3 myPos, out VRRig outRig)
        {
            //pro skidded from gtag src :fire:
            layerMask = (UnityLayer.Default.ToLayerMask() | UnityLayer.GorillaObject.ToLayerMask());
            float num = float.MaxValue;
            outRig = null;
            foreach (VRRig vrrig in VRRigCache.ActiveRigs)
            {
                float num2 = 0f;
                if (this.PlayerNear(vrrig, GorillaTagManager.instance.tagDistanceThreshold, out num2) && num2 < num)
                {
                    num = num2;
                    outRig = vrrig;
                }
            }
            return num != float.MaxValue;
        }


        public static bool battleIsOnCooldown(VRRig rig)
        {
            return rig.mainSkin.material.name.Contains("hit");
        }

        public static Photon.Realtime.Player GetRandomPlayer(bool includeSelf)
        {
            if (includeSelf)
            {
                Player p = PhotonNetwork.PlayerList[UnityEngine.Random.Range(0, 11)];
                if (p != null)
                {
                    return p;
                }
                return GetRandomPlayer(includeSelf);
            }
            Player p2 = PhotonNetwork.PlayerListOthers[UnityEngine.Random.Range(0, 10)];
            if (p2 != null)
            {
                return p2;
            }
            return GetRandomPlayer(includeSelf);
        }
        public static NetPlayer GetRandomNetPlayer(bool includeSelf)
        {
            if (includeSelf)
            {
                NetPlayer p = NetworkSystem.Instance.PlayerListOthers[UnityEngine.Random.Range(0, NetworkSystem.Instance.PlayerListOthers.Length)];
                if (p != null)
                {
                    return p;
                }
                return GetRandomNetPlayer(includeSelf);
            }
            NetPlayer p2 = NetworkSystem.Instance.PlayerListOthers[UnityEngine.Random.Range(0, NetworkSystem.Instance.PlayerListOthers.Length)];
            if (p2 != null)
            {
                return p2;
            }
            return GetRandomNetPlayer(includeSelf);
        }
    }
}
