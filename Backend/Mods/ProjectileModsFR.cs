using ExitGames.Client.Photon;
using Genesis.UI;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ShibaGTGenesis.Backend
{
    internal class ProjectileModsFR : MonoBehaviour
    {
        public static int projectileChooser = 0;
        public static int projectileToCall = 214;

        public static int colorChooser = 0;
        public static Color colorToCall = new Color(0,0,0,123);

        public static void ChangeColor(bool loading)
        {
            var buttonInfo = Back.GetButton("Projectile Color : ");
            if (!loading)
            {
                colorChooser++;
            }
            if (colorChooser == 0)
            {
                buttonInfo.buttonText = "Projectile Color : Default";
                colorToCall = new Color(0, 0, 0, 123);
            }
            if (colorChooser == 1)
            {
                buttonInfo.buttonText = "Projectile Color : Blue";
                colorToCall = new Color(0, 0, 255, 255);
            }
            if (colorChooser == 2)
            {
                buttonInfo.buttonText = "Projectile Color : Cyan";
                colorToCall = new Color(0, 255, 255, 255);
            }
            if (colorChooser == 3)
            {
                buttonInfo.buttonText = "Projectile Color : Gray";
                colorToCall = new Color(128, 128, 128, 255);
            }
            if (colorChooser == 4)
            {
                buttonInfo.buttonText = "Projectile Color : Green";
                colorToCall = new Color(0, 255, 0, 255);
            }
            if (colorChooser == 5)
            {
                buttonInfo.buttonText = "Projectile Color : Magenta";
                colorToCall = new Color(255, 0, 255, 255);
            }
            if (colorChooser == 6)
            {
                buttonInfo.buttonText = "Projectile Color : Red";
                colorToCall = Color.red;
                colorToCall = new Color(255, 0, 0, 255);
            }
            if (colorChooser == 7)
            {
                buttonInfo.buttonText = "Projectile Color : White";
                colorToCall = Color.white;
                colorToCall = new Color(255, 255, 255, 255);
            }
            if (colorChooser == 8)
            {
                buttonInfo.buttonText = "Projectile Color : Yellow";
                colorToCall = new Color(255, 255, 0, 255);
            }
            if (colorChooser == 9)
            {
                buttonInfo.buttonText = "Projectile Color : Black";
                colorToCall = Color.black;
            }
            if (colorChooser == 10)
            {
                colorChooser = 0;
                buttonInfo.buttonText = "Projectile Color : Default";
                colorToCall = new Color(0,0,0,123);
            }
            buttonInfo.enabled = false;
        }

        public static void ChangeProjectile(bool loading)
        {
            try
            {
                var buttonInfo = Back.GetButton("Projectile Type : ");
                if (!loading)
                {
                    projectileChooser++;
                }

                if (projectileChooser == 0)
                {
                    buttonInfo.buttonText = "Projectile Type : Deadshot";
                    projectileToCall = 214;
                }
                if (projectileChooser == 1)
                {
                    buttonInfo.buttonText = "Projectile Type : Molten";
                    projectileToCall = 219;
                }
                if (projectileChooser == 2)
                {
                    buttonInfo.buttonText = "Projectile Type : Ice";
                    projectileToCall = 217;
                }
                if (projectileChooser == 3)
                {
                    buttonInfo.buttonText = "Projectile Type : Leaf";
                    projectileToCall = 218;
                }
                if (projectileChooser == 4)
                {
                    buttonInfo.buttonText = "Projectile Type : Deadshot";
                    projectileToCall = 214;
                    projectileChooser = 0;
                }
                buttonInfo.enabled = false;
            }
            catch(Exception e) { Debug.Log(e); }
        }

        static int counter;
        static float cooldown;

        public static Dictionary<int, string> IndexNameDict = new Dictionary<int, string>
        {
            { 214, "LMABB." },
            { 219, "LMAGJ." },
            { 217, "LMADC." },
            { 218, "LMADU." }
        };

        public static void SlingshotCall(int cosmeticID, Vector3 pos, Vector3 vel, Color color)
        {
            if (WristMenu.gripDownR)
            {
                if (GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[cosmeticID].gameObject.activeSelf)
                {
                    if (Time.time > cooldown)
                    {
                        cooldown = Time.time + 0.155f;

                        GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[cosmeticID].storedZone = BodyDockPositions.DropPositions.RightArm;
                        GorillaTagger.Instance.offlineVRRig.projectileWeapon.currentState = TransferrableObject.PositionState.InRightHand;
                        var mRef = typeof(VRRig).Assembly.GetType("ProjectileTracker").GetMethod("IncrementLocalPlayerProjectileCount", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                        object[] projectileSendData = { pos, vel, 0, counter, true, (byte)color.r, (byte)color.g, (byte)color.b, (byte)color.a };
                        object[] projectileSendData2 = { pos, vel, 0, counter, false, (byte)color.r, (byte)color.g, (byte)color.b, (byte)color.a };

                        GameObject.Find("Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Cosmetics Room Triggers").SetActive(true);

                        GorillaTagger.Instance.offlineVRRig.transform.position = pos;

                        counter = (int)mRef.Invoke(null, System.Array.Empty<object>());

                        typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
                        if (color.a == 123)
                            PhotonNetwork.RaiseEvent(3, new object[] { PhotonNetwork.ServerTimestamp, (byte)0, projectileSendData2 }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendUnreliable);
                        else
                            PhotonNetwork.RaiseEvent(3, new object[] { PhotonNetwork.ServerTimestamp, (byte)0, projectileSendData }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendUnreliable);

                        OP.NewFlusher();
                    }
                    else
                    {
                        GorillaTagger.Instance.offlineVRRig.transform.position = pos;
                        typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
                    }
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-51.4897f, 16.9286f, -120.1083f);

                    GorillaTagger.Instance.offlineVRRig.SetActiveTransferrableObjectIndex(1, cosmeticID);
                    GameObject gameObject2 = GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[cosmeticID].gameObject;
                    gameObject2.SetActive(true);
                    GorillaTagger.Instance.offlineVRRig.inTryOnRoom = true;
                    GorillaTagger.Instance.offlineVRRig.myBodyDockPositions.allObjects[cosmeticID].storedZone = BodyDockPositions.DropPositions.RightArm;
                    string itemName = "LMABB.";
                    IndexNameDict.TryGetValue(cosmeticID, out itemName);

                    CosmeticsController.instance.ApplyCosmeticItemToSet(GorillaTagger.Instance.offlineVRRig.tryOnSet, CosmeticsController.instance.GetItemFromDict(itemName), false, false);
                    CosmeticsController.instance.ApplyCosmeticItemToSet(GorillaTagger.Instance.offlineVRRig.cosmeticSet, CosmeticsController.instance.GetItemFromDict(itemName), false, false);
                    CosmeticsController.instance.ApplyCosmeticItemToSet(CosmeticsController.instance.currentWornSet, CosmeticsController.instance.GetItemFromDict(itemName), false, false);

                    CosmeticsController.instance.UpdateWornCosmetics(true);
                    typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

                    OP.NewFlusher();
                }

                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-51.4897f, 16.9286f, -120.1083f);
            }
            else
                GorillaTagger.Instance.offlineVRRig.enabled = true;
        }
    }
}
