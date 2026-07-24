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
using Fusion.Analyzer;
using HarmonyLib;
using UnityEngine.XR.Interaction.Toolkit;
using static OVRPlugin;
using GorillaLocomotion;
using System.Reflection;
using Valve.VR.InteractionSystem;
using PlayFab.EventsModels;

namespace ShibaGTGenesis.Backend
{
    internal class PlayerMovement : MonoBehaviour
    {
        private static float pullPower = 0.05f;
        private static bool lasttouchleft = false;
        private static bool lasttouchright = false;
        public static void PullMod()
        {
            if (((!GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true) && lasttouchleft) || (!GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false) && lasttouchright)) && WristMenu.gripDownR)
            {
                Vector3 vel = GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity;
                GorillaLocomotion.GTPlayer.Instance.transform.position += new Vector3(vel.x * pullPower, 0f, vel.z * pullPower);
            }
            lasttouchleft = GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true);
            lasttouchright = GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false);
        }
        public static void GiveHoverboard()
        {
            FreeHoverboardManager.instance.SendDropBoardRPC(GorillaTagger.Instance.offlineVRRig.transform.position, Quaternion.identity, Vector3.zero, Vector3.zero, Color.black);
            FreeHoverboardManager.instance.photonView.RPC("GrabBoard_RPC", RpcTarget.All, new object[]
            {
                PhotonNetwork.LocalPlayer.ActorNumber,
                true
            });
            FreeHoverboardManager.instance.photonView.RPC("GrabBoard_RPC", RpcTarget.All, new object[]
            {
                PhotonNetwork.LocalPlayer.ActorNumber,
                false
            });
        }

        public static void HoverboardSigma(Color c)
        {
            FreeHoverboardManager.instance.SendDropBoardRPC(GorillaTagger.Instance.offlineVRRig.transform.position, Quaternion.identity, Vector3.zero, Vector3.zero, c);
        }

        public static void HoverboardLauncher()
        {
            if (Time.time > gayfloat && WristMenu.gripDownR)
            {
                gayfloat = Time.time + 0.1f;
                FreeHoverboardManager.instance.SendDropBoardRPC(GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position, Quaternion.identity, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 2400f, PlayerMovement.TrueRightHand().forward * Time.deltaTime * 2400f, Color.black);
            }
        }

        public static void FastHoverboard()
        {
            if (WristMenu.gripDownR)
            {
                Traverse.Create(GorillaLocomotion.GTPlayer.Instance).Field("hoverMinGrindSpeed").SetValue(5f);
            }
            else
                Traverse.Create(GorillaLocomotion.GTPlayer.Instance).Field("hoverMinGrindSpeed").SetValue(1f);
        }

        public static void GrabHoverboard()
        {
            FreeHoverboardManager.instance.photonView.RPC("GrabBoard_RPC", RpcTarget.All, new object[]
            {
                PhotonNetwork.LocalPlayer.ActorNumber,
                true
            });
            FreeHoverboardManager.instance.photonView.RPC("GrabBoard_RPC", RpcTarget.All, new object[]
            {
                PhotonNetwork.LocalPlayer.ActorNumber,
                false
            });
        }

        static bool b;
        static float fingDelay;
        static List<FreeHoverboardInstance> allBoards = new List<FreeHoverboardInstance>();

        public static List<FreeHoverboardInstance> GetHoverboards()
        {
            if (Time.time > fingDelay)
            {
                fingDelay = Time.time + 5f;
                allBoards.Clear();
            }
            if (allBoards.Count < 3)
            {
                foreach (FreeHoverboardInstance r in UnityEngine.Object.FindObjectsOfType<FreeHoverboardInstance>())
                {
                    allBoards.Add(r);
                }
            }

            return allBoards;
        }

        static float gayfloat;

        public static void GayHoverboard()
        {
            if (Time.time > gayfloat)
            {
                gayfloat = Time.time + 0.04f;
                if (GorillaTagger.Instance.offlineVRRig.hoverboardVisual != null && GorillaTagger.Instance.offlineVRRig.hoverboardVisual.IsHeld)
                {
                    float h = (Time.frameCount / 180f) % 1f;
                    Color rgbColor = UnityEngine.Color.HSVToRGB(h, 1f, 1f);
                    GorillaTagger.Instance.offlineVRRig.hoverboardVisual.SetIsHeld(GorillaTagger.Instance.offlineVRRig.hoverboardVisual.IsLeftHanded, GorillaTagger.Instance.offlineVRRig.hoverboardVisual.NominalLocalPosition, GorillaTagger.Instance.offlineVRRig.hoverboardVisual.NominalLocalRotation, rgbColor);
                }
            }
        }

        public static void GrabHoverboardFR()
        {
            GorillaTagger.Instance.offlineVRRig.hoverboardVisual.SetIsHeld(false, GorillaTagger.Instance.offlineVRRig.hoverboardVisual.NominalLocalPosition, GorillaTagger.Instance.offlineVRRig.hoverboardVisual.NominalLocalRotation, GorillaTagger.Instance.offlineVRRig.playerColor);
        }

        public static void GrabHoverboards()
        {
            foreach (FreeHoverboardInstance ins in GetHoverboards())
            {
                if (ins.gameObject.activeSelf && GorillaTagger.Instance.offlineVRRig.IsPositionInRange(ins.gameObject.transform.position, 5f))
                {
                    if (Time.time > archfloat)
                    {
                        archfloat = Time.time + 0.1f;

                        bool index = false;
                        if (ins.boardIndex == 0)
                            index = false;

                        if (ins.boardIndex == 1)
                            index = true;

                        FreeHoverboardManager.instance.photonView.RPC("GrabBoard_RPC", RpcTarget.All, new object[]
                        {
                            ins.ownerActorNumber,
                            index
                        });
                    }
                }
            }
        }

       

        static bool punchiGiven;

        static List<BalloonHoldable> balloons = new List<BalloonHoldable>();
        static float archfloat;
        public static List<BalloonHoldable> GetBalloons()
        {
            if (Time.time > archfloat)
            {
                archfloat = Time.time + 1f;
                balloons.Clear();
            }
            if (balloons.Count < 2)
            {
                foreach (BalloonHoldable r in UnityEngine.Object.FindObjectsOfType<BalloonHoldable>())
                {
                    if (r.gameObject.activeSelf)
                        balloons.Add(r);
                }
            }
            return balloons;
        }

        public static void BecomePunchi()
        {

                //LMAMI.

                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-51.4897f, 16.9286f, -120.1083f);
                GorillaTagger.Instance.offlineVRRig.inTryOnRoom = true;

                UnityEngine.Debug.Log("1");

                if (punchiGiven == false)
                {
                    UnityEngine.Debug.Log("gang");
                    CosmeticsController.instance.ApplyCosmeticItemToSet(GorillaTagger.Instance.offlineVRRig.tryOnSet, CosmeticsController.instance.GetItemFromDict("LMAMI."), false, false);
                    CosmeticsController.instance.UpdateWornCosmetics(true);
                    
                }


                if (punchiGiven)
                {
                    foreach (BalloonHoldable b in GetBalloons())
                    {
                        if (b.ownerRig == GorillaTagger.Instance.offlineVRRig && b.gameObject.name == "LMAMI.")
                        {
                            Traverse.Create(b).Field("maxDistanceFromOwner").SetValue(float.MaxValue);
                           // b.ownersh
                            b.currentState = TransferrableObject.PositionState.Dropped;
                            b.gameObject.transform.position = GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position + (GorillaTagger.Instance.headCollider.transform.up * -1f);
                            b.gameObject.transform.rotation = GorillaLocomotion.GTPlayer.Instance.headCollider.transform.rotation;
                        }
                    }
                }

                BalloonHoldable punchi = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/Holdables/PunchingBagAnchor(Clone)/LMAMI.").GetComponent<BalloonHoldable>();
                if (punchi != null)
                {
                    punchiGiven = true;
                    punchi.currentState = TransferrableObject.PositionState.Dropped;
                    punchi.transform.position = GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position;
                    punchi.transform.rotation = GorillaLocomotion.GTPlayer.Instance.headCollider.transform.rotation;
                }
        }

        public static void PunchiDisable()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;
            punchiGiven = false;
        }

        public static bool SpeedBoostBool;

        public static Vector3[] lastLeft = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

        public static Vector3[] lastRight = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

        public static bool loud;
        public static bool no;

        public static void LoudHandtaps()
        {
            GorillaTagger.Instance.handTapVolume = 999f;
        }

        public static void NoHandtaps()
        {
            GorillaTagger.Instance.handTapVolume = 0f;
        }

        public static void OFFLoudHandtaps()
        {
            GorillaTagger.Instance.handTapVolume = 0.1f;
        }

        public static void OFFNoHandtaps()
        {
            GorillaTagger.Instance.handTapVolume = 0.1f;
        }

        public static void SirenTalk()
        {
            if (WristMenu.triggerDownL)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-69.6924f, 2.648f, -70.341f);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void PunchMod()
        {
            int index = -1;
            foreach (VRRig vrrig in VRRigCache.ActiveRigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    index++;

                    Vector3 they = vrrig.rightHandTransform.position;
                    Vector3 notthem = GorillaTagger.Instance.offlineVRRig.head.rigTarget.position;
                    float distance = Vector3.Distance(they, notthem);

                    if (distance < 0.25)
                    {
                        GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += Vector3.Normalize(vrrig.rightHandTransform.position - lastRight[index]) * 10;
                    }
                    lastRight[index] = vrrig.rightHandTransform.position;

                    they = vrrig.leftHandTransform.position;
                    distance = Vector3.Distance(they, notthem);

                    if (distance < 0.25)
                    {
                        GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += Vector3.Normalize(vrrig.rightHandTransform.position - lastLeft[index]) * 10;
                    }
                    lastLeft[index] = vrrig.leftHandTransform.position;
                }
            }
        }

        public static void SpeedBoost()
        {
            if (!SpeedBoostBool)
            {
                foreach (GorillaSurfaceOverride surfascdse in Resources.FindObjectsOfTypeAll<GorillaSurfaceOverride>())
                {
                    if (Settings.speed == 0)
                    {
                        surfascdse.extraVelMaxMultiplier = 1.2f;
                        surfascdse.extraVelMultiplier = 1.1f;
                    }
                    else if (Settings.speed == 1)
                    {
                        surfascdse.extraVelMaxMultiplier = 1.5f;
                        surfascdse.extraVelMultiplier = 1.4f;
                    }
                    else if (Settings.speed == 2)
                    {
                        surfascdse.extraVelMaxMultiplier = 10f;
                        surfascdse.extraVelMultiplier = 10f;
                    }
                }
                SpeedBoostBool = true;
            }
        }

        public static void DisableSpeedBoost()
        {
            if (SpeedBoostBool)
            {
                foreach (GorillaSurfaceOverride surfascdse in Resources.FindObjectsOfTypeAll<GorillaSurfaceOverride>())
                {
                    surfascdse.extraVelMaxMultiplier = 1f;
                    surfascdse.extraVelMultiplier = 1f;
                }
                SpeedBoostBool = false;
            }
        }

        public static void ReallyArms()
        {
            GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(2f, 2f, 2f);
        }

        public static bool triggerplat;
        public static bool toggleplat;
        private static bool toggleon;
        public static bool toggletoggletoggle;
        static bool a;
        static int flySpeed = 17;

        public static void Fly()
        {
            if (WristMenu.bbuttonDown)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += (GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime) * (float)flySpeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public static void HelicopterFly()
        {
            if (WristMenu.bbuttonDown)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += (GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime) * (float)flySpeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.position + new Vector3(0, 0.3f, 0);

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * -1f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * 1f;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));
                }
                catch { }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void TriggerFly()
        {
            if (WristMenu.triggerDownL)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += (GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime) * (float)flySpeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public static void NoClipTriggerFly()
        {
            if (WristMenu.triggerDownL)
            {
                Back.GetButton("nocli").enabled = true;
                GorillaLocomotion.GTPlayer.Instance.transform.position += (GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime) * (float)flySpeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else
                Back.GetButton("nocli").enabled = false;
        }

        public static void BarkFly()
        {
            Back.GetButton("No Gravity").enabled = true;

            var rb = GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody;
            Vector2 xz = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.axis;
            float y = SteamVR_Actions.gorillaTag_RightJoystick2DAxis.axis.y;

            Vector3 inputDirection = new Vector3(xz.x, y, xz.y);
            var playerForward = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.forward;
            playerForward.y = 0;
            var playerRight = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.right;
            playerRight.y = 0;

            var velocity = inputDirection.x * playerRight + y * Vector3.up + inputDirection.z * playerForward;
            velocity *= GorillaLocomotion.GTPlayer.Instance.scale * flySpeed;
            rb.velocity = Vector3.Lerp(rb.velocity, velocity, 0.12875f);
            BarkFlyBool = true;
        }

        public static Vector3 rightgrapplePoint;
        public static Vector3 leftgrapplePoint;
        public static SpringJoint rightjoint;
        public static SpringJoint leftjoint;
        public static bool isLeftGrappling = false;
        public static bool isRightGrappling = false;

        public static void DisableSpiderMonke()
        {
            if (SpiderMonkeBool)
            {
                isLeftGrappling = false;
                UnityEngine.Object.Destroy(leftjoint);
                isRightGrappling = false;
                UnityEngine.Object.Destroy(rightjoint);
                SpiderMonkeBool = false;
            }
        }


        public static void SpiderMonke()
        {
            if (WristMenu.gripDownL)
            {
                if (!isLeftGrappling)
                {
                    isLeftGrappling = true;
                    RaycastHit lefthit;
                    if (Physics.Raycast(GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.forward, out lefthit, 100f))
                    {
                        leftgrapplePoint = lefthit.point;

                        leftjoint = GorillaTagger.Instance.gameObject.AddComponent<SpringJoint>();
                        leftjoint.autoConfigureConnectedAnchor = false;
                        leftjoint.connectedAnchor = leftgrapplePoint;

                        float leftdistanceFromPoint = Vector3.Distance(GorillaTagger.Instance.bodyCollider.attachedRigidbody.position, leftgrapplePoint);

                        leftjoint.maxDistance = leftdistanceFromPoint * 0.8f;
                        leftjoint.minDistance = leftdistanceFromPoint * 0.25f;

                        leftjoint.spring = 10f;
                        leftjoint.damper = 50f;
                        leftjoint.massScale = 12f;
                    }
                }

                GameObject line = new GameObject("Line");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                UnityEngine.Color thecolor = Color.white;
                liner.startColor = thecolor; liner.endColor = thecolor; liner.startWidth = 0.025f; liner.endWidth = 0.025f; liner.positionCount = 2; liner.useWorldSpace = true;
                liner.SetPosition(0, GorillaTagger.Instance.leftHandTransform.position);
                liner.SetPosition(1, leftgrapplePoint);
                liner.material.shader = Shader.Find("GorillaTag/UberShader");
                UnityEngine.Object.Destroy(line, Time.deltaTime);
            }
            else
            {
                isLeftGrappling = false;
                UnityEngine.Object.Destroy(leftjoint);
            }

            if (WristMenu.gripDownR)
            {
                if (!isRightGrappling)
                {
                    isRightGrappling = true;
                    RaycastHit righthit;
                    if (Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.forward, out righthit, 100f))
                    {
                        rightgrapplePoint = righthit.point;

                        rightjoint = GorillaTagger.Instance.gameObject.AddComponent<SpringJoint>();
                        rightjoint.autoConfigureConnectedAnchor = false;
                        rightjoint.connectedAnchor = rightgrapplePoint;

                        float rightdistanceFromPoint = Vector3.Distance(GorillaTagger.Instance.bodyCollider.attachedRigidbody.position, rightgrapplePoint);

                        rightjoint.maxDistance = rightdistanceFromPoint * 0.8f;
                        rightjoint.minDistance = rightdistanceFromPoint * 0.25f;

                        rightjoint.spring = 10f;
                        rightjoint.damper = 50f;
                        rightjoint.massScale = 12f;
                    }
                }

                GameObject line = new GameObject("Line");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                UnityEngine.Color thecolor = Color.white;
                liner.startColor = thecolor; liner.endColor = thecolor; liner.startWidth = 0.025f; liner.endWidth = 0.025f; liner.positionCount = 2; liner.useWorldSpace = true;
                liner.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                liner.SetPosition(1, rightgrapplePoint);
                liner.material.shader = Shader.Find("GorillaTag/UberShader");
                UnityEngine.Object.Destroy(line, Time.deltaTime);
            }
            else
            {
                isRightGrappling = false;
                UnityEngine.Object.Destroy(rightjoint);
            }
            SpiderMonkeBool = true;
        }

        public static void OffBarkFly()
        {
            if (BarkFlyBool)
            {
                Back.GetButton("No Gravity").enabled = false;
                BarkFlyBool = false;
            }
        }

        static bool SpiderMonkeBool;
        static bool telebool;
        static bool swim;
        static bool walk;

        public static void Fling()
        {
            GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(10, 99), Random.Range(10, 99), Random.Range(10, 99)), ForceMode.Impulse);
        }

        public static void SwimEverywhere()
        {
            swim = true;
            GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach").transform.Find("ForestToBeach_Prefab_V4/").gameObject.SetActive(true);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach").transform.Find("ForestToBeach_Prefab_V4/CaveWaterVolume").gameObject.transform.localScale = new Vector3(999f, 999f, 999f);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach").transform.Find("ForestToBeach_Prefab_V4/CaveWaterVolume").gameObject.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position;
            GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach").transform.Find("ForestToBeach_Prefab_V4/CaveWaterVolume").gameObject.transform.localPosition = GorillaTagger.Instance.offlineVRRig.transform.position;
        }

        public static void Bhop()
        {
            if (GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false))
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().AddForce(Vector3.up * 220f, ForceMode.Impulse);
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().AddForce(GorillaTagger.Instance.offlineVRRig.rightHandPlayer.transform.right * 170f, ForceMode.Impulse);
            }
            if (GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true))
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().AddForce(Vector3.up * 220f, ForceMode.Impulse);
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().AddForce(-GorillaTagger.Instance.offlineVRRig.leftHandPlayer.transform.right * 170f, ForceMode.Impulse);
            }
        }

        public static void AutoRun()
        {
            if (WristMenu.gripDownL)
            {
                float time = Time.frameCount;
                GorillaTagger.Instance.rightHandTransform.position = GorillaTagger.Instance.headCollider.transform.position + (GorillaTagger.Instance.headCollider.transform.forward * UnityEngine.Mathf.Cos(time) / 10) + new Vector3(0, -0.5f - (Mathf.Sin(time) / 7), 0) + (GorillaTagger.Instance.headCollider.transform.right * -0.05f);
                GorillaTagger.Instance.leftHandTransform.position = GorillaTagger.Instance.headCollider.transform.position + (GorillaTagger.Instance.headCollider.transform.forward * Mathf.Cos(time + 180) / 10) + new Vector3(0, -0.5f - (Mathf.Sin(time + 180) / 7), 0) + (GorillaTagger.Instance.headCollider.transform.right * 0.05f);
            }
        }

        public static void OFFSwimEverywhere()
        {
            if (swim)
            {
                swim = false;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach").transform.Find("ForestToBeach_Prefab_V4/CaveWaterVolume").gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach").transform.Find("ForestToBeach_Prefab_V4/CaveWaterVolume").gameObject.transform.position = new Vector3(-10.5229f, -1.3839f, -40.1154f);
                GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach").transform.Find("ForestToBeach_Prefab_V4/CaveWaterVolume").gameObject.transform.localPosition = new Vector3(34.7037f, -7.3563f, 29.156f);
            }
        }

        static List<GameObject> waterobj = new List<GameObject>();

        public static void WalkOnWater()
        {
            walk = true;
            int defaul2 = LayerMask.NameToLayer("Default");
            if (waterobj.Count < 2)
            {
                foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
                {
                    if (obj.layer == LayerMask.NameToLayer("Water"))
                    {
                        waterobj.Add(obj);
                    }
                }
            }
            foreach (GameObject obj in waterobj)
            {
                obj.layer = defaul2;
            }
        }

        public static void OFFWalkOnWater()
        {
            if (walk)
            {
                walk = false;
                int defaul2 = LayerMask.NameToLayer("Water");
                foreach (GameObject obj in waterobj)
                {
                    obj.layer = defaul2;
                }
            }
        }

        static float tpfloat;


        public static void TeleportGun()
        {
            var data = GunLib.Shoot();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (tpfloat < Time.time)
                {
                    tpfloat = Time.time + 0.1f;
                    GorillaLocomotion.GTPlayer.Instance.transform.position = data.hitPosition;
                }
            }
        }

        public static void TeleportRandom()
        {
            MeshCollider[] meshColliders = Resources.FindObjectsOfTypeAll<MeshCollider>();
            foreach (MeshCollider coll in meshColliders)
            {
                coll.enabled = false;
            }
            VRRig random = RigShit.GetRigFromPlayer(RigShit.GetRandomPlayer(false));
            GorillaLocomotion.GTPlayer.Instance.transform.position = random.transform.position;
            foreach (MeshCollider coll in meshColliders)
            {
                coll.enabled = true;
            }
        }

        public static Vector2 lerpthing;

        public static void BananaCar()
        {
            Vector2 rjoystick = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.axis;
            lerpthing = Vector2.Lerp(lerpthing, rjoystick, 0.05f);

            Vector3 addition = GorillaTagger.Instance.bodyCollider.transform.forward * lerpthing.y + GorillaTagger.Instance.bodyCollider.transform.right * lerpthing.x;
            Physics.Raycast(GorillaTagger.Instance.bodyCollider.transform.position - new Vector3(0f, 0.2f, 0f), Vector3.down, out var Ray, 512);

            if (Ray.distance < 0.2f && (Mathf.Abs(lerpthing.x) > 0.05f || Mathf.Abs(lerpthing.y) > 0.05f))
            {
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity = addition * 10f;
            }
        }

        public static void offnoclip()
        {
            foreach (MeshCollider meshCollider2 in Resources.FindObjectsOfTypeAll<MeshCollider>())
            {
                meshCollider2.enabled = true;
            }
        }

        public static void SlingshotFly()
        {
            if (WristMenu.gripDownR)
            {
                Rigidbody rigid = GorillaLocomotion.GTPlayer.Instance.gameObject.GetComponent<Rigidbody>();
                if (rigid != null)
                {
                    rigid.AddForce(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * 25f, ForceMode.Acceleration);
                }
            }
        }

        public static void FastSlingshotFly()
        {
            if (WristMenu.gripDownR)
            {
                Rigidbody rigid = GorillaLocomotion.GTPlayer.Instance.gameObject.GetComponent<Rigidbody>();
                if (rigid != null)
                {
                    rigid.AddForce(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * 50f, ForceMode.Acceleration);
                }
            }
        }

        public static void IronMonke()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(115, true, 0.1f);
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().AddForce(new Vector3(15f * GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.right.x, 15f * GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.right.y, 15f * GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.right.z), ForceMode.Acceleration);
                GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 50f * GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity.magnitude, GorillaTagger.Instance.tapHapticDuration);
                CreateEffect(GorillaTagger.Instance.leftHandTransform.position);
            }
            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(115, false, 0.1f);
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().AddForce(new Vector3(15f * GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.right.x, 15f * GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.right.y, 15f * GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.right.z), ForceMode.Acceleration);
                GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 50f * GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity.magnitude, GorillaTagger.Instance.tapHapticDuration);
                CreateEffect(GorillaTagger.Instance.rightHandTransform.position);
            }
        }

        private static void CreateEffect(Vector3 pos)
        {
            GameObject fire = new GameObject("Fire");
            ParticleSystem particle = fire.AddComponent<ParticleSystem>();
            ParticleSystemRenderer renderer = fire.GetComponent<ParticleSystemRenderer>();
            renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
            var main = particle.main;
            main.startLifetime = 0.5f;
            main.startSize = 0.1f;
            main.startSpeed = 1f;
            main.startColor = new ParticleSystem.MinMaxGradient(
                new Gradient
                {
                    colorKeys = new GradientColorKey[]
                    {
                        new GradientColorKey(Color.yellow, 0.0f),
                        new GradientColorKey(Color.red, 1.0f)
                    },
                    alphaKeys = new GradientAlphaKey[]
                    {
                        new GradientAlphaKey(1.0f, 0.0f),
                        new GradientAlphaKey(0.0f, 1.0f)
                    }
                }
            );
            main.loop = false;
            main.duration = 0.5f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            var emission = particle.emission;
            emission.rateOverTime = 50f;
            var shape = particle.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 25f;
            shape.radius = 0.05f;
            var colorovertime = particle.colorOverLifetime;
            colorovertime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.red, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            colorovertime.color = new ParticleSystem.MinMaxGradient(gradient);
            fire.transform.position = pos;
            particle.Play();
            GameObject.Destroy(fire, 0.5f);
        }

        static float rgbelya;
        public static UnityEngine.Color color;
        public static float hue = 0f;
        public static float timer = 0f;
        public static float updateRate = 0f;
        public static float updateTimer = 0f;
        public static bool RandomColor = false;
        public static float CycleSpeed = 0.04f;
        public static float GlowAmount = 1f;
        public static bool noclipplat;

        public static void RGB(bool strobe)
        {
            if (GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(PhotonNetwork.LocalPlayer.UserId))
            {
                if (Time.time > rgbelya + 0.04f && PhotonNetwork.InRoom)
                {
                    rgbelya = Time.time;
                    updateTimer += Time.deltaTime;

                    if (strobe)
                    {
                        if ((double)Time.time > (double)timer)
                        {
                            color = UnityEngine.Random.ColorHSV(0f, 1f, GlowAmount, GlowAmount, GlowAmount, GlowAmount);
                            timer = Time.time + CycleSpeed;
                        }
                    }
                    else
                    {
                        if ((double)hue >= 1.0)
                        {
                            hue = 0f;
                        }
                        hue += CycleSpeed;
                        float h = (Time.frameCount / 180f) % 1f;
                        color = UnityEngine.Color.HSVToRGB(h, 1f, 1f);
                    }

                    if ((double)updateTimer > (double)updateRate)
                    {
                        updateTimer = 999f;
                        GorillaTagger.Instance.UpdateColor(color.r, color.g, color.b);
                         GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[]
                        {
                   color.r,
                   color.g,
                   color.b
                        });
                        OP.NewFlusher();
                    }
                }
            }
        }



        public static async void OutOfStumpRGB(bool strobe)
        {
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.offlineVRRig.transform.position = GorillaComputer.instance.friendJoinCollider.transform.position;
            GorillaTagger.Instance.myVRRig.transform.position = GorillaComputer.instance.friendJoinCollider.transform.position;
            await Task.Delay(250);
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static List<MeshCollider> meshColliders1 = new List<MeshCollider>();
        public static List<BoxCollider> box = new List<BoxCollider>();
        public static bool noclipToggle;

        public static void NoClip(bool force)
        {
            if (force)
            {
                foreach (MeshCollider mesh in UnityEngine.Object.FindObjectsOfType<MeshCollider>())
                    mesh.enabled = false;

                foreach (BoxCollider mesh in UnityEngine.Object.FindObjectsOfType<BoxCollider>())
                    mesh.enabled = false;
            }
            if (!force)
            {
                if (WristMenu.triggerDownL)
                {
                    if (!noclipToggle)
                    {
                        foreach (MeshCollider mesh in UnityEngine.Object.FindObjectsOfType<MeshCollider>())
                            mesh.enabled = false;

                        foreach(BoxCollider mesh in UnityEngine.Object.FindObjectsOfType<BoxCollider>())
                            mesh.enabled = false;

                        noclipToggle = true;
                    }
                }
                else
                {
                    if (noclipToggle)
                    {
                        foreach (MeshCollider mesh in UnityEngine.Object.FindObjectsOfType<MeshCollider>())
                            mesh.enabled = true;

                        foreach (BoxCollider mesh in UnityEngine.Object.FindObjectsOfType<BoxCollider>())
                            mesh.enabled = true;

                        noclipToggle = false;
                    }
                }
            }
        }

        static bool noclipAmazing;

        public static bool invisplat;

        public static (Vector3 position, Quaternion rotation, Vector3 up, Vector3 forward, Vector3 right) TrueLeftHand()
        {
            Quaternion rot = GorillaTagger.Instance.leftHandTransform.rotation * GorillaLocomotion.GTPlayer.Instance.LeftHand.handRotOffset;
            return (GorillaTagger.Instance.leftHandTransform.position + GorillaTagger.Instance.leftHandTransform.rotation * GorillaLocomotion.GTPlayer.Instance.LeftHand.handOffset, rot, rot * Vector3.up, rot * Vector3.forward, rot * Vector3.right);
        }

        public static (Vector3 position, Quaternion rotation, Vector3 up, Vector3 forward, Vector3 right) TrueRightHand()
        {
            Quaternion rot = GorillaTagger.Instance.rightHandTransform.rotation * GorillaLocomotion.GTPlayer.Instance.rightHand.handRotOffset;
            return (GorillaTagger.Instance.rightHandTransform.position + GorillaTagger.Instance.rightHandTransform.rotation * GorillaLocomotion.GTPlayer.Instance.rightHand.handOffset, rot, rot * Vector3.up, rot * Vector3.forward, rot * Vector3.right);
        }

        public static void WorldScale(GameObject obj, Vector3 targetWorldScale)
        {
            Vector3 parentScale = obj.transform.parent.lossyScale;
            obj.transform.localScale = new Vector3(targetWorldScale.x / parentScale.x, targetWorldScale.y / parentScale.y, targetWorldScale.z / parentScale.z);
        }

        public static void FixStickyColliders(GameObject platform) // Object must be at true hand position
        {
            Vector3[] localPositions = new Vector3[]
            {
                new Vector3(0, 1f, 0),
                new Vector3(0, -1f, 0),
                new Vector3(1f, 0, 0),
                new Vector3(-1f, 0, 0),
                new Vector3(0, 0, 1f),
                new Vector3(0, 0, -1f)
            };
            Quaternion[] localRotations = new Quaternion[]
            {
                Quaternion.Euler(90, 0, 0),
                Quaternion.Euler(-90, 0, 0),
                Quaternion.Euler(0, -90, 0),
                Quaternion.Euler(0, 90, 0),
                Quaternion.identity,
                Quaternion.Euler(0, 180, 0)
            };
            for (int i = 0; i < localPositions.Length; i++)
            {
                GameObject side = GameObject.CreatePrimitive(PrimitiveType.Cube);
                float size = 0.025f;
                side.transform.SetParent(platform.transform);
                side.transform.position = localPositions[i] * (size / 2);
                side.transform.rotation = localRotations[i];
                WorldScale(side, new Vector3(size, size, 0.01f));
                side.GetComponent<Renderer>().enabled = false;
            }
        }


        public static void PlatformsThing(bool invis, bool sticky)
        {
            colorKeysPlatformMonke[0].color = Color.red;
            colorKeysPlatformMonke[0].time = 0f;
            colorKeysPlatformMonke[1].color = Color.green;
            colorKeysPlatformMonke[1].time = 0.3f;
            colorKeysPlatformMonke[2].color = Color.blue;
            colorKeysPlatformMonke[2].time = 0.6f;
            colorKeysPlatformMonke[3].color = Color.red;
            colorKeysPlatformMonke[3].time = 1f;
            bool left;
            bool right;
            if (triggerplat)
            {
                left = WristMenu.triggerDownL;
                right = WristMenu.triggerDownR;
            }
            else
            {
                left = WristMenu.gripDownL;
                right = WristMenu.gripDownR;
            }
            if (toggleplat)
            {
                if (WristMenu.triggerDownL && !toggletoggletoggle)
                {
                    toggleon = !toggleon;
                    toggletoggletoggle = true;
                    if (toggleon)
                    {
                        NotifiLib.SendNotification("Platforms Toggled On");
                    }
                    if (!toggleon)
                    {
                        NotifiLib.SendNotification("Platforms Toggled Off");
                    }
                }
                else if (!WristMenu.triggerDownL)
                {
                    toggletoggletoggle = false;
                }
            }
            else
            {
                toggleon = true;
            }

            if (toggleon)
            {
                if (left && jump_left_local == null)
                {
                    if (noclipplat)
                        NoClip(true);

                    jump_left_local = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    jump_left_local.transform.position = new Vector3(0f, -0.0100f, 0f) + GorillaLocomotion.GTPlayer.Instance.leftHand.controllerTransform.position;
                    jump_left_local.transform.rotation = GorillaLocomotion.GTPlayer.Instance.leftHand.controllerTransform.rotation;
                    if (sticky)
                    {
                        jump_left_local.transform.position = TrueLeftHand().position;
                        jump_left_local.transform.rotation = TrueLeftHand().rotation;

                        FixStickyColliders(jump_left_local);
                    }

                    jump_left_local.transform.localScale = scale;
                    if (sticky)
                    {
                        Vector3[] localPositions = new Vector3[]
                                                {
                                new Vector3(0, 1f, 0),
                                new Vector3(0, -1f, 0),
                                new Vector3(1f, 0, 0),
                                new Vector3(-1f, 0, 0),
                                new Vector3(0, 0, 1f),
                                new Vector3(0, 0, -1f)
                                                };
                        Quaternion[] localRotations = new Quaternion[]
                        {
                            Quaternion.Euler(90, 0, 0),
                            Quaternion.Euler(-90, 0, 0),
                            Quaternion.Euler(0, -90, 0),
                            Quaternion.Euler(0, 90, 0),
                            Quaternion.identity,
                            Quaternion.Euler(0, 180, 0)
                        };
                        for (int i = 0; i < localPositions.Length; i++)
                        {
                            GameObject side = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            try
                            {
                                if (jump_left_local.GetComponent<GorillaSurfaceOverride>() != null)
                                {
                                    side.AddComponent<GorillaSurfaceOverride>().overrideIndex = jump_left_local.GetComponent<GorillaSurfaceOverride>().overrideIndex;
                                }
                            }
                            catch { }
                            float size = 0.025f;
                            side.transform.SetParent(jump_left_local.transform);
                            side.transform.position = localPositions[i] * (size / 2);
                            side.transform.rotation = localRotations[i];
                            Vector3 parentScale = side.transform.parent.lossyScale;
                            side.transform.localScale = new Vector3(new Vector3(size, size, 0.01f).x / parentScale.x, new Vector3(size, size, 0.01f).y / parentScale.y, new Vector3(size, size, 0.01f).z / parentScale.z);
                            side.GetComponent<Renderer>().enabled = false;
                        }
                    }
                    if (invis)
                        jump_left_local.GetComponent<Renderer>().enabled = false;
                    else
                    {
                        jump_left_local.AddComponent<ColorChanger>();
                        Gradient gradient2 = new Gradient();
                        gradient2.colorKeys = colorKeysPlatformMonke;
                        jump_left_local.GetComponent<ColorChanger>().colors = gradient2;
                        jump_left_local.GetComponent<ColorChanger>().isRainbow = true;
                        jump_left_local.GetComponent<ColorChanger>().Start();
                    }
                }
                else if (!left)
                {
                    if (jump_left_local != null)
                    {
                        Destroy(jump_left_local);
                        jump_left_local = null;
                        if (noclipplat)
                        {
                            foreach (MeshCollider meshCollider2 in Resources.FindObjectsOfTypeAll<MeshCollider>())
                            {
                                meshCollider2.enabled = true;
                            }
                            foreach (BoxCollider meshCollider2 in Resources.FindObjectsOfTypeAll<BoxCollider>())
                            {
                                meshCollider2.enabled = true;
                            }
                        }
                    }
                }

                if (right && jump_right_local == null)
                {
                    if (noclipplat)
                        NoClip(true);

                    jump_right_local = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    jump_right_local.transform.position = new Vector3(0f, -0.0100f, 0f) + GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position;
                    jump_right_local.transform.rotation = GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.rotation;
                    if (sticky)
                    {
                        jump_right_local.transform.position = TrueRightHand().position;
                        jump_right_local.transform.rotation = TrueRightHand().rotation;

                        FixStickyColliders(jump_right_local);
                    }

                    jump_right_local.transform.localScale = scale;

                    if (sticky)
                    {
                        Vector3[] localPositions = new Vector3[]
                                                {
                                new Vector3(0, 1f, 0),
                                new Vector3(0, -1f, 0),
                                new Vector3(1f, 0, 0),
                                new Vector3(-1f, 0, 0),
                                new Vector3(0, 0, 1f),
                                new Vector3(0, 0, -1f)
                                                };
                        Quaternion[] localRotations = new Quaternion[]
                        {
                            Quaternion.Euler(90, 0, 0),
                            Quaternion.Euler(-90, 0, 0),
                            Quaternion.Euler(0, -90, 0),
                            Quaternion.Euler(0, 90, 0),
                            Quaternion.identity,
                            Quaternion.Euler(0, 180, 0)
                        };
                        for (int i = 0; i < localPositions.Length; i++)
                        {
                            GameObject side = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            try
                            {
                                if (jump_right_local.GetComponent<GorillaSurfaceOverride>() != null)
                                {
                                    side.AddComponent<GorillaSurfaceOverride>().overrideIndex = jump_right_local.GetComponent<GorillaSurfaceOverride>().overrideIndex;
                                }
                            }
                            catch { }
                            float size = 0.025f;
                            side.transform.SetParent(jump_right_local.transform);
                            side.transform.position = localPositions[i] * (size / 2);
                            side.transform.rotation = localRotations[i];
                            Vector3 parentScale = side.transform.parent.lossyScale;
                            side.transform.localScale = new Vector3(new Vector3(size, size, 0.01f).x / parentScale.x, new Vector3(size, size, 0.01f).y / parentScale.y, new Vector3(size, size, 0.01f).z / parentScale.z);
                            side.GetComponent<Renderer>().enabled = false;
                        }
                    }

                    if (invis)
                        jump_right_local.GetComponent<Renderer>().enabled = false;
                    else
                    {
                        jump_right_local.AddComponent<ColorChanger>();
                        jump_right_local.GetComponent<ColorChanger>().isRainbow = true;
                        Gradient gradient2 = new Gradient();
                        gradient2.colorKeys = colorKeysPlatformMonke;
                        jump_right_local.GetComponent<ColorChanger>().colors = gradient2;
                        jump_right_local.GetComponent<ColorChanger>().Start();
                    }
                }
                else if (!right)
                {
                    if (jump_right_local != null)
                    {
                        Destroy(jump_right_local);
                        jump_right_local = null;
                        if (noclipplat)
                        {
                            foreach (MeshCollider meshCollider2 in Resources.FindObjectsOfTypeAll<MeshCollider>())
                            {
                                meshCollider2.enabled = true;
                            }
                            foreach (BoxCollider meshCollider2 in Resources.FindObjectsOfTypeAll<BoxCollider>())
                            {
                                meshCollider2.enabled = true;
                            }
                        }
                    }
                }
            }
        }

        private static Vector3 scale = new Vector3(0.0125f, 0.28f, 0.3825f);

        private static Vector3 stickscale = new Vector3(0.0155f, 0.68f, 0.5925f);

        private static bool once_left;

        private static bool once_right;

        private static bool once_left_false;

        private static bool once_right_false;

        private static bool once_networking;

        private static GameObject[] jump_left_network = new GameObject[9999];

        private static GameObject[] jump_right_network = new GameObject[9999];

        private static GameObject jump_left_local = null;

        private static GameObject jump_right_local = null;

        private static GradientColorKey[] colorKeysPlatformMonke = new GradientColorKey[4];

        public static bool BarkFlyBool;

        public static bool rainbowHoverboards;

        static Color HoverColor()
        {
            return rainbowHoverboards ? Color.HSVToRGB(Time.time * 0.5f % 1f, 1f, 1f) : Color.black;
        }

        static void AllowHover()
        {
            if (GorillaLocomotion.GTPlayer.Instance != null)
                GorillaLocomotion.GTPlayer.Instance.SetHoverAllowed(true);
        }

        static float hoverAuraDelay;
        static bool hoverAuraFlip;
        public static void HoverboardAura()
        {
            if (FreeHoverboardManager.instance == null) return;
            if (Time.time > hoverAuraDelay)
            {
                hoverAuraDelay = Time.time + 0.05f;
                hoverAuraFlip = !hoverAuraFlip;
                float ang = (Time.time * 360f + (hoverAuraFlip ? 180f : 0f)) * Mathf.Deg2Rad;
                Vector3 center = GorillaTagger.Instance.bodyCollider.transform.position;
                Vector3 pos = center + new Vector3(Mathf.Cos(ang) * 1.4f, 0.2f, Mathf.Sin(ang) * 1.4f);
                Quaternion rot = Quaternion.LookRotation(new Vector3(-Mathf.Sin(ang), 0f, Mathf.Cos(ang)), Vector3.up);
                AllowHover();
                FreeHoverboardManager.instance.SendDropBoardRPC(pos, rot, Vector3.zero, Vector3.zero, HoverColor());
            }
        }

        static float hoverTornadoDelay;
        static bool hoverTornadoFlip;
        public static void HoverboardTornado()
        {
            if (FreeHoverboardManager.instance == null) return;
            if (Time.time > hoverTornadoDelay)
            {
                hoverTornadoDelay = Time.time + 0.03f;
                hoverTornadoFlip = !hoverTornadoFlip;
                float ang = (Time.time * 720f + (hoverTornadoFlip ? 180f : 0f)) * Mathf.Deg2Rad;
                float h = (Time.time * 3f + (hoverTornadoFlip ? 1.5f : 0f)) % 4f;
                float radius = 1.2f + h * 0.2f;
                Vector3 center = GorillaTagger.Instance.bodyCollider.transform.position;
                Vector3 pos = center + new Vector3(Mathf.Cos(ang) * radius, h, Mathf.Sin(ang) * radius);
                Quaternion rot = Quaternion.LookRotation(new Vector3(-Mathf.Sin(ang), 0f, Mathf.Cos(ang)), Vector3.up);
                AllowHover();
                FreeHoverboardManager.instance.SendDropBoardRPC(pos, rot, Vector3.zero, Vector3.zero, HoverColor());
            }
        }

        static float hoverTrailDelay;
        static Vector3 hoverTrailLastPos;
        public static void HoverboardTrail()
        {
            if (FreeHoverboardManager.instance == null) return;
            if (Time.time > hoverTrailDelay)
            {
                Vector3 me = GorillaTagger.Instance.bodyCollider.transform.position;
                if (Vector3.Distance(hoverTrailLastPos, me) > 0.6f)
                {
                    hoverTrailDelay = Time.time + 0.08f;
                    hoverTrailLastPos = me;
                    AllowHover();
                    FreeHoverboardManager.instance.SendDropBoardRPC(me + Vector3.down * 0.3f, Quaternion.identity, Vector3.zero, Vector3.zero, HoverColor());
                }
            }
        }

        static float hoverRainDelay;
        public static void HoverboardRain()
        {
            if (FreeHoverboardManager.instance == null) return;
            if (Time.time > hoverRainDelay)
            {
                hoverRainDelay = Time.time + 1f;
                Vector3 above = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(UnityEngine.Random.Range(-3f, 3f), 8f, UnityEngine.Random.Range(-3f, 3f));
                AllowHover();
                FreeHoverboardManager.instance.SendDropBoardRPC(above, Quaternion.identity, Vector3.down * 30f, Vector3.down * 30f, HoverColor());
            }
        }

        static float hoverGunDelay;
        public static void HoverboardSnipe()
        {
            if (FreeHoverboardManager.instance == null) return;
            var gun = Genesis.Utilities.GunLib.Shoot();
            if (gun.isShooting && gun.isTriggered)
            {
                if (Time.time > hoverGunDelay)
                {
                    hoverGunDelay = Time.time + 1f;
                    Vector3 from = TrueRightHand().position;
                    Vector3 dir = (gun.hitPosition - from).normalized;
                    AllowHover();
                    FreeHoverboardManager.instance.SendDropBoardRPC(from, Quaternion.LookRotation(dir), dir * 250f, dir * 250f, HoverColor());
                }
            }
        }

        static float hoverUpDelay;
        public static void HoverboardUpLauncher()
        {
            if (FreeHoverboardManager.instance == null) return;
            if (Time.time > hoverUpDelay)
            {
                hoverUpDelay = Time.time + 1f;
                AllowHover();
                FreeHoverboardManager.instance.SendDropBoardRPC(TrueRightHand().position, Quaternion.identity, Vector3.up * 200f, Vector3.up * 200f, HoverColor());
            }
        }

        static float hoverHandDelay;
        public static void HoverboardHandTrack()
        {
            if (FreeHoverboardManager.instance == null) return;
            if (Time.time > hoverHandDelay)
            {
                hoverHandDelay = Time.time + 0.05f;
                var lh = TrueLeftHand();
                AllowHover();
                FreeHoverboardManager.instance.SendDropBoardRPC(lh.position, lh.rotation, Vector3.zero, Vector3.zero, HoverColor());
            }
        }

        static float hoverAnnoyDelay;
        public static void HoverboardAnnoyGun()
        {
            if (FreeHoverboardManager.instance == null) return;
            var gun = Genesis.Utilities.GunLib.ShootLock();
            if (gun.isShooting && gun.isTriggered && gun.isLocked)
            {
                if (Time.time > hoverAnnoyDelay)
                {
                    hoverAnnoyDelay = Time.time + 0.1f;
                    Vector3 pos = gun.lockedPlayer.headMesh.transform.position + Vector3.up * 0.3f;
                    AllowHover();
                    FreeHoverboardManager.instance.SendDropBoardRPC(pos, Quaternion.identity, Vector3.down * 5f, Vector3.down * 5f, HoverColor());
                }
            }
        }

        static float hoverCannonDelay;
        public static void HoverboardCannon()
        {
            if (FreeHoverboardManager.instance == null) return;
            if (Time.time > hoverCannonDelay)
            {
                hoverCannonDelay = Time.time + 1f;
                var rh = TrueRightHand();
                AllowHover();
                FreeHoverboardManager.instance.SendDropBoardRPC(rh.position, Quaternion.LookRotation(rh.forward), rh.forward * 180f, rh.forward * 180f, HoverColor());
            }
        }

        static float hoverSpamDelay;
        public static void HoverboardSpam()
        {
            if (FreeHoverboardManager.instance == null) return;
            if (Time.time > hoverSpamDelay)
            {
                hoverSpamDelay = Time.time + 0.1f;
                AllowHover();
                FreeHoverboardManager.instance.SendDropBoardRPC(TrueRightHand().position, Quaternion.identity, Vector3.zero, Vector3.zero, HoverColor());
            }
        }

        static float hoverStackDelay;
        static bool hoverStackFlip;
        public static void HoverboardStack()
        {
            if (FreeHoverboardManager.instance == null) return;
            if (Time.time > hoverStackDelay)
            {
                hoverStackDelay = Time.time + 0.25f;
                hoverStackFlip = !hoverStackFlip;
                Vector3 me = GorillaTagger.Instance.bodyCollider.transform.position;
                float h = 1.5f + (Time.time * 4f + (hoverStackFlip ? 2f : 0f)) % 6f;
                AllowHover();
                FreeHoverboardManager.instance.SendDropBoardRPC(me + Vector3.up * h, Quaternion.identity, Vector3.zero, Vector3.zero, HoverColor());
            }
        }

        static float hoverPulseDelay;
        static bool hoverPulseFlip;
        public static void HoverboardPulse()
        {
            if (FreeHoverboardManager.instance == null) return;
            if (Time.time > hoverPulseDelay)
            {
                hoverPulseDelay = Time.time + 0.06f;
                hoverPulseFlip = !hoverPulseFlip;
                Transform body = GorillaTagger.Instance.bodyCollider.transform;
                Vector3 side = body.right * (hoverPulseFlip ? 1.2f : -1.2f);
                Vector3 pos = body.position + side + Vector3.up * 0.3f;
                Quaternion rot = Quaternion.LookRotation(body.forward, Vector3.up);
                AllowHover();
                FreeHoverboardManager.instance.SendDropBoardRPC(pos, rot, Vector3.zero, Vector3.zero, HoverColor());
            }
        }

        public static void HoverboardFollowGun()
        {
            if (FreeHoverboardManager.instance == null) return;
            var gun = Genesis.Utilities.GunLib.Shoot();
            if (gun.isShooting)
            {
                if (Time.time > hoverGunDelay)
                {
                    hoverGunDelay = Time.time + 0.08f;
                    AllowHover();
                    FreeHoverboardManager.instance.SendDropBoardRPC(gun.hitPosition + Vector3.up * 0.2f, Quaternion.identity, Vector3.zero, Vector3.zero, HoverColor());
                }
            }
        }

        static float hoverTagDelay;
        static bool hoverTagFlip;
        public static void HoverboardTagAlong()
        {
            if (FreeHoverboardManager.instance == null) return;
            if (Time.time > hoverTagDelay)
            {
                hoverTagDelay = Time.time + 0.05f;
                hoverTagFlip = !hoverTagFlip;
                Transform body = GorillaTagger.Instance.bodyCollider.transform;
                Vector3 side = body.right * (hoverTagFlip ? 0.8f : -0.8f);
                Vector3 pos = body.position + side;
                AllowHover();
                FreeHoverboardManager.instance.SendDropBoardRPC(pos, body.rotation, Vector3.zero, Vector3.zero, HoverColor());
            }
        }

        public static void RainbowHoverboardsOn() { rainbowHoverboards = true; }
        public static void RainbowHoverboardsOff() { rainbowHoverboards = false; }
    }
}
