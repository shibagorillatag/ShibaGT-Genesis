using Genesis.UI;
using Genesis.Utilities;
using GorillaNetworking;
using GTAG_NotificationLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEngine.UI.GridLayoutGroup;

namespace ShibaGTGenesis.Backend
{
    internal class Legit : MonoBehaviour
    {
        public static new List<ForceVolume> ForceVolumes = new List<ForceVolume>();
        public static bool WindBool;
        public static float LagFloat;
        static bool LagBool;
        static float Killfloat;

        static bool whenever;

        public static void FakeLag()
        {
            if (LagFloat < Time.time)
            {
                LagFloat = Time.time + 0.2f;
                int randomint = UnityEngine.Random.Range(1, 3);
                if (randomint == 1)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
                LagBool = true;
            }
        }

        public static void HZ()
        {
            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
            {   }
        }

        public static void SlideControl()
        {
            GorillaLocomotion.GTPlayer.Instance.slideControl = 1f;
        }

        public static void OFFSlideControl()
        {
            GorillaLocomotion.GTPlayer.Instance.slideControl = 0.00425f;
        }

        public static void OFFFakeLag()
        {
            if (LagBool)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                LagBool = true;
            }
        }

        static Vector3 lastPos = Vector3.zero;
        static Quaternion lastRot;

        static new List<Vector3> macroList = new List<Vector3>();
        static new List<Quaternion> macroRList = new List<Quaternion>();
        static bool playingMacroBool;
        static bool recordingMacroBool;
        static int macroInt;
        static bool forceoff;
        static Vector3 savedPos;

        public static void Macro(bool outofbody)
        {
            if (WristMenu.triggerDownL)
            {
                if (!recordingMacroBool)
                {
                    recordingMacroBool = true;
                    NotifiLib.SendNotification("Recording..");
                    savedPos = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.position;
                }
                if (outofbody)
                    macroList.Add(GorillaTagger.Instance.offlineVRRig.transform.position);
                else
                    macroList.Add(GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.position);

                macroRList.Add(GorillaTagger.Instance.offlineVRRig.transform.rotation);
            }
            else
            {
                if (recordingMacroBool)
                {
                    recordingMacroBool = false ;
                    NotifiLib.SendNotification("Stopped recording.");
                }
            }

            if (WristMenu.triggerDownR)
            {
                if (!playingMacroBool)
                {
                    playingMacroBool = true;
                    NotifiLib.SendNotification("Playing..");
                }

                if (outofbody)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = macroList[macroInt];
                    GorillaTagger.Instance.offlineVRRig.transform.rotation = macroRList[macroInt];
                    macroInt++;
                }
                else
                {
                    if (macroList[macroInt + 1] != null)
                    { macroInt++; }
                }
            }
            else
            {
                if (playingMacroBool)
                {
                    playingMacroBool = false;
                    NotifiLib.SendNotification("Stopped playing.");
                    if (outofbody)
                        GorillaTagger.Instance.offlineVRRig.enabled = true;
                    macroInt = 0;

                }
            }
        }

        public static void ClearMacro()
        {
            macroList.Clear();
        }

        public static void Desync()
        {
            if (LagFloat < Time.time)
            {
                desync = true;

                LagFloat = Time.time + 0.04f;
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = lastPos;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = lastRot;
                lastPos = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.position + new Vector3(0, 0, 0);
                lastRot = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.leftHandTransform.position = WristMenu.ghostRig.leftHandTransform.position;
                GorillaTagger.Instance.offlineVRRig.rightHandTransform.position = WristMenu.ghostRig.rightHandTransform.position;
            }
        }

        static bool desync;

        public static void OFFDesync()
        {
            if (desync)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                desync = true;
            }
        }

        public static void RemoveWind()
        {
            if (ForceVolumes.Count == 0)
            {
                foreach (ForceVolume vol in GameObject.Find("Environment Objects/LocalObjects_Prefab/").GetComponentsInChildren<ForceVolume>())
                {
                    ForceVolumes.Add(vol);
                }
            }
            foreach (var vol in ForceVolumes) 
            {
                vol.enabled = false;
            }
            WindBool = true;
        }

        public static void OFFRemoveWind()
        {
            if (WindBool)
            {
                foreach (var vol in ForceVolumes)
                {
                    vol.enabled = true;
                }
                WindBool = false;
            }
        }



        public static void NoFingers()
        {
            Debug.Log("ez");

            ControllerInputPoller.instance.leftControllerGripFloat = 0f;
            ControllerInputPoller.instance.rightControllerGripFloat = 0f;
            ControllerInputPoller.instance.leftControllerIndexFloat = 0f;
            ControllerInputPoller.instance.rightControllerIndexFloat = 0f;
            ControllerInputPoller.instance.leftControllerPrimaryButton = false;
            ControllerInputPoller.instance.leftControllerSecondaryButton = false;
            ControllerInputPoller.instance.rightControllerPrimaryButton = false;
            ControllerInputPoller.instance.rightControllerSecondaryButton = false;
        }

        public static void SteamArms()
        {
            GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
        }

        public static void DisableSteamArms()
        {
            if (Back.GetButton("Really Long Arms").enabled == false)
                GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        public static Vector3 walkPos;
        public static Vector3 walkNormal;

        static bool FakeOculusBool;

        public static void FakeOculus()
        {
            if (WristMenu.bbuttonDown)
            {
                FakeOculusBool = true;
                GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/LeftHand Controller").SetActive(false);
                GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/RightHand Controller").SetActive(false);
            }
            else
            {
                if (FakeOculusBool)
                {
                    GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/LeftHand Controller").SetActive(true);
                    GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/RightHand Controller").SetActive(true);
                    FakeOculusBool = false;
                }
            }
        }

        public static void WallWalk()
        {
            if ((GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true) || GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false)) && WristMenu.gripDownR)
            {
                FieldInfo fieldInfo = typeof(GorillaLocomotion.GTPlayer).GetField("lastHitInfoHand", BindingFlags.NonPublic | BindingFlags.Instance);
                RaycastHit ray = (RaycastHit)fieldInfo.GetValue(GorillaLocomotion.GTPlayer.Instance);
                walkPos = ray.point;
                walkNormal = ray.normal;
            }

            if (!WristMenu.gripDownL)
            {
                walkPos = Vector3.zero;
            }

            if (walkPos != Vector3.zero)
            {
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(walkNormal * -9.81f, ForceMode.Acceleration);
            }
        }
    }
}
