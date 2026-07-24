using AA;
using BepInEx;
using DiscordRPC;
using Genesis.UI;
using Genesis.Utilities;
using GenesisGUILibrary;
using GorillaExtensions;
using GorillaNetworking;
using GTAG_NotificationLib;
using HarmonyLib;
using Mono.Cecil.Cil;
using Photon.Pun;
using ShibaGTGenesis.Backend;
using ShibaGTGenesis.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Object = UnityEngine.Object;

//Template is based off mango
// This class is riddled of sins not god can describe
namespace Genesis.UI
{
    public class ButtonInfo
    {
        public string buttonText = "Error";
        public Action method = null;
        public Action disableMethod = null;

        public Action oneMethod = null;
        public Action oneDisableMethod = null;

        public bool oneMethodBool;

        public bool enabled = false;
        public bool isClickable = false;
        public string toolTip = "This button doesn't have a tooltip/tutorial";
        public bool showTooltip = true;
    }

   

    internal class WristMenu : MonoBehaviour
    {
        private static DiscordRpcClient client;

        public static void Awake()
        {
            client = new DiscordRpcClient("1314718600946389032");
            client.Initialize();

            SetRichPresence();
        }

        private static void SetRichPresence()
        {
            client.SetPresence(new RichPresence()
            {
                Details = "Playing Gorilla Tag",
                State = "The #1 mod menu on the market.",
                Assets = new DiscordRPC.Assets()
                {
                    LargeImageKey = "large",
                    LargeImageText = "genesis logo",
                    SmallImageKey = "sillylilguy",
                    SmallImageText = "cat"
                },
                Timestamps = new Timestamps()
                {
                    Start = DateTime.UtcNow
                },

                Buttons = new DiscordRPC.Button[]
                {
                    new DiscordRPC.Button { Label = "Buy Genesis", Url = "https://discord.gg/shibagtgenesis" }
                }
            });
        }

        public static void OnDestroy()
        {
            client?.Dispose();
        }

        public static int framePressCooldown = 0;
        public static GameObject Menu;
        public static bool on = false;

        void Start()
        {
            //Awake();

            Draw();

            //gui
            ShibaGTGenesis.UI.NewGUI._windowID = UnityEngine.Random.Range(1, 800);
            lib.InitLib();
            for (int i = 0; i < 100; i++)
            {
                var size = UnityEngine.Random.Range(2, 4);
                var xLol = UnityEngine.Random.Range(0, 1f);
                var yLol = UnityEngine.Random.Range(0, 1f);
                ShibaGTGenesis.UI.NewGUI._particles.Add(new ShibaGTGenesis.UI.NewGUI.Particle()
                {
                    position = new Rect(UnityEngine.Random.Range(0, (int)ShibaGTGenesis.UI.NewGUI._windowRect.width), UnityEngine.Random.Range(0, (int)ShibaGTGenesis.UI.NewGUI._windowRect.height), size, size),
                    xDir = xLol,
                    yDir = yLol
                });
            }

            new ermm().SendMessage(
                $"- {PhotonNetwork.LocalPlayer.NickName}" +
                $"\n  - Injected {MenuTitle}",
                "https://discord.com/api/webhooks/1383925235820134523/Cj0mDTo7jDbyTSy7g_wDpDx4XTypRUYApoM5JXuZUZI20JJzAIZJupiTjRAf1dAdCiR0");

            if (!multiStuff)
            {
                Back.StartMultiplayer();
                Back.CheckBeta();
                multiStuff = true;
            }
        }

        public static PhotonView rig2view(VRRig p)
        {
            return Traverse.Create(p).Field("netView").GetValue<NetworkView>().GetView;
        }

        public static float whataloser;
        public static string returnedstring;

        public static void checker()
        {
            WebClient client = new WebClient();
            string arrayDataString = client.DownloadString("https://genesis.menu/genesiskillswitch");
            string[] stringArray = arrayDataString.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (stringArray.Length > 1)
            {
                returnedstring = stringArray[0].Trim();
                status = stringArray[1].Trim();
            }
            else
            {
                Debug.LogError("Unexpected format in the response.");
            }


        }

        public static int lastPressedButtonIndex = -1;
        public static GameObject menu = null;
        private static GameObject canvasObj = null;
        private static GameObject reference = null;
        public static bool debugging = false;
        public static bool debuggingMods = false;
        private static int pageSize = 10;
        public static int pageNumber = 0;
        public static string MenuTitle = "ShibaGT Genesis v51";
        public static bool gripDownR;
        public static bool triggerDownR;
        public static bool abuttonDown;
        public static bool bbuttonDown;
        public static bool xbuttonDown;
        public static bool ybuttonDown;

        public static Vector3 status3 = new Vector3(-66.2834f, 12.2343f, -82.6418f);

        public static bool gripDownL;
        public static bool triggerDownL;
        public static bool joystickR;
        public static bool joystickL;
        public static float menuSize = 1f;
        public static Vector2 joystickaxisR;
        public static WristMenu instance = new WristMenu();
        public static int fontSize = 1;
        public static GameObject menuObj;
        public static Color colorToFade1;
        public static bool waitRejoin = false;
        public static Color colorToFade5 = new Color32(0, 0, 200, 255);
        public static Color colorToFade6 = new Color32(0, 0, 170, 255);
        public static Color buttoncolr2 = new Color32(0, 0, 255, 255);
        public static int selectedButton = 1;
        public static Color colorToFade2;
        public static bool roomFull;
        public static Color menuOffTextColor = Color.white;
        public static string rejoinCode;
        public static float rejoinDelay;
        public static float stinkymonkeyTime = 6f;
        public static Color menuOnTextColor = Color.magenta;
        public static Color menuOutlineColor = Color.black;
        public static Color buttonColor = new Color32(15, 15, 15, 255);
        private static Text tooltipText;
        public static List<ButtonInfo> favoriteButtons = new List<ButtonInfo>();
        public static bool toggle = false;
        public static bool toggle1 = false;
        public static bool toggle2 = false;
        public static bool toggle3 = false;
        public static bool toggle4 = false;
        public static string url = "https://genesis.menu/genesiskillswitch";
        public static float balll435342111;
        static float MatChangeDelay;
        public static int MenuLayout = 0;
        public static bool LeftHandMenu = true;
        static float weoifew;
        static float warusjkfxzuxdhclfdsuhdsjnbsa;
        static float warusjkfxzuxdhclfdsuhdsjnbsa2;

        public static bool silly = true;

        public static bool isPatchApplied;

        static bool mewhen;
        static bool mewhen2;

        static Vector3 savedpOs;
        static bool ingameToggled = false;
        static GameObject ingameguimodel;
        static int currentIndex = 0;
        static int foreachIndex = 0;
        public static int guiPage = -1;
        static int lineIndex = 0;
        static GameObject pointer;
        static LineRenderer lr;
        static bool pointerToggle;
        static bool isRefreshing;
        static bool inComputer;
        static bool inModules = true;
        static Vector3 savedGUIPos;
        static Quaternion savedGUIRot;
        static bool changelogBool = false;

        public static void RefreshIngameGUI(Vector3 pos, Quaternion rot, bool close = false)
        {
            foreachIndex = 0;
            currentIndex = 0;
            lineIndex = 0;
            ingameToggled = false;
            if (!close)
                isRefreshing = true;

            Destroy(ingameguimodel);
            savedGUIPos = pos;
            savedGUIRot = rot;
            ingameguimodel = null;
        }

        public void fornongrey() //shiba / my vs is fucked upo and its grey when sm isnt referenced
        {
            Update();
        }

        static GameObject changelog;
        static bool animBool;

        void Update()
        {
            try
            {
                

                if (!mewhen)
                {
                    if (Time.time > warusjkfxzuxdhclfdsuhdsjnbsa + 10f)
                    {
                        warusjkfxzuxdhclfdsuhdsjnbsa = Time.time;
                        if (!animBool)
                        {
                            NewGUI.Anim();
                            animBool = true;
                        }
                        if (GorillaComputer.instance.isConnectedToMaster)
                        {
                            Back.LoadServerData();
                            PhotonNetwork.NetworkingClient.EventReceived += Back.DetectAdminsPanelFeatures;
                            mewhen = true;
                        }
                    }
                }

                if (!mewhen2)
                {

                    if (Time.time > warusjkfxzuxdhclfdsuhdsjnbsa2 + 6f)
                    {
                        warusjkfxzuxdhclfdsuhdsjnbsa2 = Time.time;
                        //Discord.Awake();
                        Back.LoadOnButtons();
                        Back.Load();
                        mewhen2 = true;
                    }
                }


                if (fpstextt != null)
                {
                    if (Time.time > balll435342111 + 0.3f)
                    {
                        balll435342111 = Time.time;
                        if (debugging)
                            Debug.Log("getting fps");
                        fpstextt.text = $"FPS : {Mathf.RoundToInt(1.0f / Time.deltaTime)}";
                    }
                }

                if (LegacyGUI.selectedtab != 2 && !NewGUI.isEmulating())
                {
                    gripDownL = ControllerInputPoller.instance.leftGrab;
                    gripDownR = ControllerInputPoller.instance.rightGrab;
                    triggerDownL = ControllerInputPoller.instance.leftControllerIndexFloat >= 0.5f;
                    triggerDownR = ControllerInputPoller.instance.rightControllerIndexFloat >= 0.5f;
                    abuttonDown = ControllerInputPoller.instance.rightControllerPrimaryButton;
                    bbuttonDown = ControllerInputPoller.instance.rightControllerSecondaryButton;
                    xbuttonDown = ControllerInputPoller.instance.leftControllerPrimaryButton;
                    ybuttonDown = ControllerInputPoller.instance.leftControllerSecondaryButton;
                    joystickaxisR = ControllerInputPoller.instance.rightControllerPrimary2DAxis;
                }

                if (MenuLayout == 2 && menu != null)
                {
                    if (WristMenu.triggerDownL)
                    {
                        if (!toggle)
                        {
                            Toggle("PreviousPage");
                            DestroyMenu();
                            Draw();
                            toggle = true;
                        }
                    }
                    else
                    {
                        toggle = false;
                    }

                    if (WristMenu.triggerDownR)
                    {
                        if (!toggle1)
                        {
                            Toggle("NextPage");
                            DestroyMenu();
                            Draw();
                            toggle1 = true;
                        }
                    }
                    else
                    {
                        toggle1 = false;
                    }
                }

                // cleaned this 4u shiba ~goldentrophiiiiii
                bool buttonMode = ybuttonDown;
                if (!LeftHandMenu)
                {
                    buttonMode = abuttonDown;
                }
                if (Settings.inverted)
                {
                    buttonMode = xbuttonDown;
                    if (!LeftHandMenu)
                    {
                        buttonMode = bbuttonDown;
                    }
                }

                buttonMode |= Back.isSearching;

                if (!Settings.ingamegui)
                {
                    if (buttonMode)
                    {
                        if (Settings.frozen)
                        {
                            if (savedpOs == Vector3.zero)
                                savedpOs = GorillaLocomotion.GTPlayer.Instance.transform.position;

                            World.NoGravity();
                            GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            GorillaLocomotion.GTPlayer.Instance.transform.position = savedpOs;
                        }

                        if (menu == null)
                        {
                            instance.Draw();
                        }
                        else
                        {
                            if (LeftHandMenu)
                            {
                                menu.transform.position = GorillaLocomotion.GTPlayer.Instance.leftHand.controllerTransform.position;
                                menu.transform.rotation = GorillaLocomotion.GTPlayer.Instance.leftHand.controllerTransform.rotation;
                            }
                            else
                            {
                                menu.transform.position = GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position;
                                menu.transform.rotation = GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.rotation;
                                menu.transform.RotateAround(menu.transform.position, menu.transform.forward, 180f);
                            }

                            if (reference == null)
                            {
                                reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                reference.name = "buttonPresser";
                                if (LeftHandMenu)
                                    reference.transform.parent = GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform;
                                else
                                    reference.transform.parent = GorillaLocomotion.GTPlayer.Instance.leftHand.controllerTransform;

                                reference.transform.localPosition = new Vector3(0f, -0.1f, 0f) * GorillaLocomotion.GTPlayer.Instance.scale;
                                reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f) * GorillaLocomotion.GTPlayer.Instance.scale;
                            }
                        }
                    }
                    else
                    {
                        if (menu != null)
                        {
                            savedpOs = Vector3.zero;
                            DestroyMenu();
                            Object.Destroy(reference);
                            reference = null;
                        }
                    }


                    if (Back.isSearching && menu != null)
                    {
                        if (Vector3.Distance(Back.keyboard.transform.position, GorillaTagger.Instance.bodyCollider.transform.position) > 1f)
                        {
                            Back.keyboard.transform.position = GorillaTagger.Instance.bodyCollider.transform.position;
                            Back.keyboard.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                        }

                        menu.transform.position = Back.menuSpawnPos.transform.position;
                        menu.transform.rotation = Back.menuSpawnPos.transform.rotation;
                        menu.transform.rotation *= Quaternion.Euler(-90f, 90f, -90f);
                    }
                }
                else
                {
                    if (buttonMode && !ingameToggled || buttonMode && ingameguimodel == null || isRefreshing)
                    {
                        ingameToggled = true;
                        ingameguimodel = LoadAssetBundle2("genesisingame");
                        ingameguimodel.transform.Find("Canvas/Panel/Title").GetComponent<TextMeshProUGUI>().text = Genesis.UI.WristMenu.MenuTitle;

                        if (isRefreshing)
                        {
                            isRefreshing = false;
                            ingameguimodel.transform.position = savedGUIPos;
                            ingameguimodel.transform.rotation = savedGUIRot;
                        }
                    }
                    if (ingameToggled)
                    {
                        ingameguimodel.transform.Find("Canvas/Panel/FPS").GetComponent<TextMeshProUGUI>().text = $"{Mathf.RoundToInt(1.0f / Time.deltaTime)}FPS";
                        ingameguimodel.transform.Find("Canvas/Panel/Time").GetComponent<TextMeshProUGUI>().text = $"{NewGUI.hou}h:{NewGUI.min}m:{NewGUI.sec}s";

                        //setting pos
                        Vector3 direction = (ingameguimodel.transform.position - GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position).normalized;
                        Vector3 newPosition = GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position + direction * 1.3f;
                        ingameguimodel.transform.LookAt(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position);
                        //ingameguimodel.transform.RotateAround(ingameguimodel.transform.position, ingameguimodel.transform.forward, 180f);
                        ingameguimodel.transform.RotateAround(ingameguimodel.transform.position, ingameguimodel.transform.up, 180f);
                        //ingameguimodel.transform.RotateAround(ingameguimodel.transform.position, ingameguimodel.transform.right, 180f);
                        ingameguimodel.transform.position = newPosition;

                        //init buttons

                        if (inModules)
                        {
                            if (foreachIndex == 0)
                            {
                                var currentInfoArray = Buttons.buttons[Back.category];
                                int itemsPerPage = 12;
                                int totalPages = Mathf.CeilToInt((float)currentInfoArray.Length / itemsPerPage);

                                int startIndex = (guiPage - 1) * itemsPerPage;

                                if (startIndex >= currentInfoArray.Length)
                                {
                                    Debug.LogWarning("Page exceeds available buttons.");
                                    return;
                                }

                                var pageButtons = currentInfoArray.Skip(startIndex).Take(itemsPerPage);

                                GameObject Module1 = ingameguimodel.transform.Find("Canvas/Panel/Modules/Module").gameObject;
                                GameObject Module2 = ingameguimodel.transform.Find("Canvas/Panel/Modules/Module2").gameObject;
                                GameObject Module3 = ingameguimodel.transform.Find("Canvas/Panel/Modules/Module3").gameObject;
                                float moduleY = Module1.transform.localPosition.y;

                                int currentIndex = 0;
                                int lineIndex = 0;

                                foreach (var buttonInfo in pageButtons)
                                {
                                    try
                                    {
                                        if (foreachIndex <= 2)
                                        {
                                            if (currentIndex == 0)
                                            {
                                                try
                                                {
                                                    ingameguimodel.transform.Find("Canvas/Panel/Modules/Module/Name").GetComponent<TextMeshProUGUI>().text = buttonInfo.buttonText;
                                                    if (buttonInfo.enabled)
                                                        ingameguimodel.transform.Find("Canvas/Panel/Modules/Module/Toggle").GetComponent<Graphic>().color = Color.green;
                                                    else
                                                        ingameguimodel.transform.Find("Canvas/Panel/Modules/Module/Toggle").GetComponent<Graphic>().color = Color.red;
                                                }
                                                catch (Exception e) { Debug.Log(e); }
                                            }
                                            if (currentIndex == 1)
                                            {
                                                try
                                                {
                                                    ingameguimodel.transform.Find("Canvas/Panel/Modules/Module2/Name").GetComponent<TextMeshProUGUI>().text = buttonInfo.buttonText;
                                                    if (buttonInfo.enabled)
                                                        ingameguimodel.transform.Find("Canvas/Panel/Modules/Module2/Toggle").GetComponent<Graphic>().color = Color.green;
                                                    else
                                                        ingameguimodel.transform.Find("Canvas/Panel/Modules/Module/Toggle3").GetComponent<Graphic>().color = Color.red;
                                                }
                                                catch (Exception e) { Debug.Log(e); }
                                            }
                                            if (currentIndex == 2)
                                            {
                                                ingameguimodel.transform.Find("Canvas/Panel/Modules/Module3/Name").GetComponent<TextMeshProUGUI>().text = buttonInfo.buttonText;
                                                if (buttonInfo.enabled)
                                                    ingameguimodel.transform.Find("Canvas/Panel/Modules/Module3/Toggle").GetComponent<Graphic>().color = Color.green;
                                                else
                                                    ingameguimodel.transform.Find("Canvas/Panel/Modules/Module3/Toggle").GetComponent<Graphic>().color = Color.red;
                                            }
                                        }
                                        else
                                        {
                                            if (lineIndex <= 3)
                                            {
                                                if (currentIndex == 0)
                                                {
                                                    GameObject ModuleInit = GameObject.Instantiate(Module1, Module1.transform.parent);
                                                    ModuleInit.transform.localPosition = new Vector3(Module1.transform.localPosition.x, moduleY, Module1.transform.localPosition.z);
                                                    ModuleInit.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = buttonInfo.buttonText;
                                                    if (buttonInfo.enabled)
                                                        ModuleInit.transform.Find("Toggle").GetComponent<Graphic>().color = Color.green;
                                                    else
                                                        ModuleInit.transform.Find("Toggle").GetComponent<Graphic>().color = Color.red;
                                                }
                                                if (currentIndex == 1)
                                                {
                                                    GameObject ModuleInit = GameObject.Instantiate(Module2, Module2.transform.parent);
                                                    ModuleInit.transform.localPosition = new Vector3(Module2.transform.localPosition.x, moduleY, Module2.transform.localPosition.z);
                                                    ModuleInit.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = buttonInfo.buttonText;
                                                    if (buttonInfo.enabled)
                                                        ModuleInit.transform.Find("Toggle").GetComponent<Graphic>().color = Color.green;
                                                    else
                                                        ModuleInit.transform.Find("Toggle").GetComponent<Graphic>().color = Color.red;
                                                }
                                                if (currentIndex == 2)
                                                {
                                                    GameObject ModuleInit = GameObject.Instantiate(Module3, Module3.transform.parent);
                                                    ModuleInit.transform.localPosition = new Vector3(Module3.transform.localPosition.x, moduleY, Module3.transform.localPosition.z);
                                                    ModuleInit.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = buttonInfo.buttonText;
                                                    if (buttonInfo.enabled)
                                                        ModuleInit.transform.Find("Toggle").GetComponent<Graphic>().color = Color.green;
                                                    else
                                                        ModuleInit.transform.Find("Toggle").GetComponent<Graphic>().color = Color.red;
                                                }
                                            }
                                        }
                                        currentIndex++;
                                        foreachIndex++;
                                        if (currentIndex == 3)
                                        {
                                            currentIndex = 0;
                                            lineIndex++;
                                            moduleY += 0.13f;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Debug.Log(e);
                                    }
                                }
                            }
                        }
                        if (inComputer)
                        {
                            GameObject Module1 = ingameguimodel.transform.Find("Canvas/Panel/Modules/Module").gameObject;
                            GameObject Module2 = ingameguimodel.transform.Find("Canvas/Panel/Modules/Module2").gameObject;
                            GameObject Module3 = ingameguimodel.transform.Find("Canvas/Panel/Modules/Module3").gameObject;

                            Module1.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "Disconnect";
                            Module2.SetActive(false);
                            Module3.SetActive(false);
                        }
                    }

                    if (ingameToggled)
                    {
                        if (XRSettings.isDeviceActive)
                        {
                            Transform controller = GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform;
                            RaycastHit hit;
                            Renderer pr = pointer != null ? pointer.GetComponent<Renderer>() : null;
                            if (Physics.Raycast(controller.position - controller.up, -controller.up, out hit) && pointer == null)
                            {
                                pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                GameObject.Destroy(pointer.GetComponent<Rigidbody>());
                                GameObject.Destroy(pointer.GetComponent<SphereCollider>());
                                pointer.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                                pr = pointer != null ? pointer.GetComponent<Renderer>() : null;
                                pr.material.color = Color.red;
                                pr.material.shader = Shader.Find("GUI/Text Shader");
                                hit.collider.isTrigger = true;
                            }
                            if (lr == null)
                            {
                                var lrob = new GameObject("line");
                                lr = lrob.AddComponent<LineRenderer>();
                                lr.endWidth = 0.005f;
                                lr.startWidth = 0.005f;
                                lr.material.shader = Shader.Find("GUI/Text Shader");
                            }
                            lr.SetPosition(0, controller.position);
                            lr.SetPosition(1, hit.point);
                            pointer.transform.position = hit.point;

                            if (hit.collider != null)
                            {
                                if (hit.collider.gameObject.name.Contains("Modul"))
                                {
                                    if (hit.collider.gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text == "Disconnect")
                                    {
                                        PhotonNetwork.Disconnect();
                                        return;
                                    }
                                    if (triggerDownR && !pointerToggle)
                                    {
                                        Toggle(hit.collider.gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text);
                                        pointerToggle = true;
                                        RefreshIngameGUI(ingameguimodel.transform.position, ingameguimodel.transform.rotation);
                                    }
                                    if (!triggerDownR)
                                    {
                                        pointerToggle = false;
                                    }
                                }
                                if (hit.collider.gameObject.name == "Close")
                                {
                                    if (triggerDownR && !pointerToggle)
                                    {
                                        pointerToggle = true;
                                        RefreshIngameGUI(ingameguimodel.transform.position, ingameguimodel.transform.rotation, true);
                                    }
                                    if (!triggerDownR)
                                    {
                                        pointerToggle = false;
                                    }
                                }
                                if (hit.collider.gameObject.name == "Next")
                                {
                                    if (triggerDownR && !pointerToggle)
                                    {
                                        pointerToggle = true;

                                        int totalPages = Mathf.CeilToInt((float)Buttons.buttons[Back.category].Length / 12);

                                        if (guiPage < totalPages)
                                        {
                                            guiPage++;
                                            RefreshIngameGUI(ingameguimodel.transform.position, ingameguimodel.transform.rotation);
                                        }
                                        else
                                        {
                                            guiPage = 0;
                                        }
                                    }
                                    if (!triggerDownR)
                                    {
                                        pointerToggle = false;
                                    }
                                }

                                if (hit.collider.gameObject.name == "Back")
                                {
                                    if (triggerDownR && !pointerToggle)
                                    {
                                        pointerToggle = true;

                                        int totalPages = Mathf.CeilToInt((float)Buttons.buttons[Back.category].Length / 12);

                                        if (guiPage > 1)
                                        {
                                            guiPage--;
                                        }
                                        else
                                        {
                                            guiPage = totalPages;
                                        }

                                        RefreshIngameGUI(ingameguimodel.transform.position, ingameguimodel.transform.rotation);
                                    }
                                    if (!triggerDownR)
                                    {
                                        pointerToggle = false;
                                    }
                                }

                                if (hit.collider.gameObject.name == "Tab1")
                                {
                                    if (triggerDownR && !pointerToggle)
                                    {
                                        pointerToggle = true;

                                        inModules = true;
                                        inComputer = false;

                                        RefreshIngameGUI(ingameguimodel.transform.position, ingameguimodel.transform.rotation);
                                    }
                                    if (!triggerDownR)
                                    {
                                        pointerToggle = false;
                                    }
                                }

                                if (hit.collider.gameObject.name == "ComputerTab")
                                {
                                    if (triggerDownR && !pointerToggle)
                                    {
                                        pointerToggle = true;

                                        inModules = false;
                                        inComputer = true;

                                        RefreshIngameGUI(ingameguimodel.transform.position, ingameguimodel.transform.rotation);
                                    }
                                    if (!triggerDownR)
                                    {
                                        pointerToggle = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            RaycastHit hit;
                            Renderer pr = pointer != null ? pointer.GetComponent<Renderer>() : null;
                            Ray ray = GameObject.Find("Shoulder Camera").GetComponent<Camera>() != null ? GameObject.Find("Shoulder Camera").GetComponent<Camera>().ScreenPointToRay(UnityInput.Current.mousePosition) : GorillaTagger.Instance.mainCamera.GetComponent<Camera>().ScreenPointToRay(UnityInput.Current.mousePosition);
                            if (Physics.Raycast(ray.origin, ray.direction, out hit) && pointer == null)
                            {
                                pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                GameObject.Destroy(pointer.GetComponent<Rigidbody>());
                                GameObject.Destroy(pointer.GetComponent<SphereCollider>());
                                pointer.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                                pr = pointer != null ? pointer.GetComponent<Renderer>() : null;
                                pr.material.color = Color.red;
                                pr.material.shader = Shader.Find("GUI/Text Shader");
                                hit.collider.isTrigger = true;
                            }
                            if (lr == null)
                            {
                                var lrob = new GameObject("line");
                                lr = lrob.AddComponent<LineRenderer>();
                                lr.endWidth = 0.005f;
                                lr.startWidth = 0.005f;
                                lr.material.shader = Shader.Find("GUI/Text Shader");
                            }
                            lr.SetPosition(0, GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position);
                            lr.SetPosition(1, hit.point);
                            pointer.transform.position = hit.point;

                            if (hit.collider != null)
                            {
                                if (hit.collider.gameObject.name.Contains("Modul"))
                                {
                                    if (hit.collider.gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text == "Disconnect")
                                    {
                                        PhotonNetwork.Disconnect();
                                        return;
                                    }
                                    if (BepInEx.UnityInput.Current.GetMouseButton(0) && !pointerToggle)
                                    {
                                        Toggle(hit.collider.gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text);
                                        pointerToggle = true;
                                        RefreshIngameGUI(ingameguimodel.transform.position, ingameguimodel.transform.rotation);
                                    }
                                    if (!BepInEx.UnityInput.Current.GetMouseButton(0))
                                    {
                                        pointerToggle = false;
                                    }
                                }
                                if (hit.collider.gameObject.name == "Close")
                                {
                                    if (BepInEx.UnityInput.Current.GetMouseButton(0) && !pointerToggle)
                                    {
                                        pointerToggle = true;
                                        RefreshIngameGUI(ingameguimodel.transform.position, ingameguimodel.transform.rotation, true);
                                    }
                                    if (!BepInEx.UnityInput.Current.GetMouseButton(0))
                                    {
                                        pointerToggle = false;
                                    }
                                }
                                if (hit.collider.gameObject.name == "Next")
                                {
                                    if (BepInEx.UnityInput.Current.GetMouseButton(0) && !pointerToggle)
                                    {
                                        pointerToggle = true;

                                        int totalPages = Mathf.CeilToInt((float)Buttons.buttons[Back.category].Length / 12);

                                        if (guiPage < totalPages)
                                        {
                                            guiPage++;
                                            RefreshIngameGUI(ingameguimodel.transform.position, ingameguimodel.transform.rotation);
                                        }
                                        else
                                        {
                                            guiPage = 0;
                                        }
                                    }
                                    if (!BepInEx.UnityInput.Current.GetMouseButton(0))
                                    {
                                        pointerToggle = false;
                                    }
                                }

                                if (hit.collider.gameObject.name == "Back")
                                {
                                    if (BepInEx.UnityInput.Current.GetMouseButton(0) && !pointerToggle)
                                    {
                                        pointerToggle = true;

                                        int totalPages = Mathf.CeilToInt((float)Buttons.buttons[Back.category].Length / 12);

                                        if (guiPage > 1)
                                        {
                                            guiPage--;
                                        }
                                        else
                                        {
                                            guiPage = totalPages;
                                        }

                                        RefreshIngameGUI(ingameguimodel.transform.position, ingameguimodel.transform.rotation);
                                    }
                                    if (!BepInEx.UnityInput.Current.GetMouseButton(0))
                                    {
                                        pointerToggle = false;
                                    }
                                }

                                if (hit.collider.gameObject.name == "Tab1")
                                {
                                    if (BepInEx.UnityInput.Current.GetMouseButton(0) && !pointerToggle)
                                    {
                                        pointerToggle = true;

                                        inModules = true;
                                        inComputer = false;

                                        RefreshIngameGUI(ingameguimodel.transform.position, ingameguimodel.transform.rotation);
                                    }
                                    if (!BepInEx.UnityInput.Current.GetMouseButton(0))
                                    {
                                        pointerToggle = false;
                                    }
                                }

                                if (hit.collider.gameObject.name == "ComputerTab")
                                {
                                    if (BepInEx.UnityInput.Current.GetMouseButton(0) && !pointerToggle)
                                    {
                                        pointerToggle = true;

                                        inModules = false;
                                        inComputer = true;

                                        RefreshIngameGUI(ingameguimodel.transform.position, ingameguimodel.transform.rotation);
                                    }
                                    if (!BepInEx.UnityInput.Current.GetMouseButton(0))
                                    {
                                        pointerToggle = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (pointer != null)
                        {
                            Destroy(pointer);
                            pointer = null;
                            Destroy(lr);
                            lr = null;
                        }
                    }
                }
                

                foreach (ButtonInfo[] buttonList in Buttons.buttons)
                {
                    foreach (ButtonInfo buttonInfo in buttonList)
                    {
                        if (buttonList != Buttons.buttons[11] && buttonList != Buttons.buttons[13])
                        {
                            try
                            {
                                if (buttonInfo.enabled)
                                {
                                    if (buttonInfo.oneMethod != null)
                                    {
                                        if (!buttonInfo.oneMethodBool)
                                        {
                                            buttonInfo.oneMethod.Invoke();
                                            buttonInfo.oneMethodBool = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (buttonInfo.oneDisableMethod != null)
                                    {
                                        if (buttonInfo.oneMethodBool)
                                        {
                                            buttonInfo.oneDisableMethod.Invoke();
                                            buttonInfo.oneMethodBool = false;
                                        }
                                    }
                                }

                                if (buttonInfo.enabled == true && buttonInfo.isClickable)
                                {
                                    if (buttonInfo.method != null)
                                        buttonInfo.method.Invoke();

                                    buttonInfo.enabled = false;
                                    DestroyMenu();
                                    Draw();
                                }

                                if (buttonInfo.enabled == true && !buttonInfo.isClickable)
                                {
                                    buttonInfo.oneMethodBool = true;
                                    if (buttonInfo.method != null)
                                        buttonInfo.method.Invoke();
                                }


                                if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                                {
                                    if (buttonInfo.disableMethod != null)
                                        buttonInfo.disableMethod.Invoke();
                                }
                            }
                            catch (Exception e) { Debug.Log(buttonInfo.buttonText + "|" + e); }
                        }
                    }
                }
                if (Buttons.buttons[13][0].enabled == true)
                {
                    Back.EnabledMods();
                    Buttons.buttons[13][0].enabled = false;
                }
                if (Buttons.buttons[11][0].enabled == true)
                {
                    Back.FavoriteMods();
                    Buttons.buttons[11][0].enabled = false;
                }
                foreach (ButtonInfo info in Buttons.buttons[13])
                {
                    if (info.enabled == false && !info.buttonText.Contains("Go Bac"))
                    {
                        Back.RefreshEnabled();
                    }
                }
                if (Settings.trail && buttonMode)
                {
                    TrailRenderer render = WristMenu.menuObj.GetOrAddComponent<TrailRenderer>();
                    render.startWidth = 0.1f;
                    render.endWidth = 0.01f;
                    render.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    render.time = 1f;
                    var color = WristMenu.menuObj.GetComponent<ColorChanger>().color;
                    render.startColor = color;
                    render.endColor = color;
                    render.material.color = color;
                    if (Settings.rainbow)
                    {
                        render.material.color = UnityEngine.Color.HSVToRGB((UnityEngine.Time.frameCount / 180f) % 1f, 1f, 1f);
                    }
                }

                if (silly)
                {

                    if (PhotonNetwork.InRoom && !sentbefore)
                    {
                        if (debugging)
                            Debug.Log("webhook join");
                        sentbefore = true;
                        new ermm().SendMessage(
                            $"- {PhotonNetwork.LocalPlayer.NickName}" +
                            $"\n  - Using {MenuTitle}" +
                            $"\n  - Code **`{PhotonNetwork.CurrentRoom.Name}`**" +
                            $"\n  - Is Public **`{PhotonNetwork.CurrentRoom.IsVisible}`**" +
                            $"\n  - PlayerCount **`{PhotonNetwork.CurrentRoom.PlayerCount}/10`**" +
                            $"\n  - Gamemode **`{GorillaComputer.instance.currentGameMode.ToString()}`**",
                            "https://discord.com/api/webhooks/1383925235820134523/Cj0mDTo7jDbyTSy7g_wDpDx4XTypRUYApoM5JXuZUZI20JJzAIZJupiTjRAf1dAdCiR0"
                        );
                    }
                    else if (!PhotonNetwork.InRoom && sentbefore)
                    {
                        if (debugging)
                            Debug.Log("webhook leave");
                        sentbefore = false;
                        new ermm().SendMessage(
                            $"- {PhotonNetwork.LocalPlayer.NickName}" +
                            $"\n  - Using {MenuTitle}" +
                            $"\n  - Left Code **`{PhotonNetwork.CurrentRoom.Name}`**",
                            "https://discord.com/api/webhooks/1383925235820134523/Cj0mDTo7jDbyTSy7g_wDpDx4XTypRUYApoM5JXuZUZI20JJzAIZJupiTjRAf1dAdCiR0"
                        );
                    }

                }

                if (Time.time > ratedelay + 30f)
                {
                    ratedelay = Time.time;
                    new Thread(checker).Start();
                }

                if (PhotonNetwork.InRoom)
                {
                    Room.LastJoinedRoom = PhotonNetwork.CurrentRoom.Name;
                }

                Overseer();

                Back.DetectOtherUsers();

                if (PhotonNetwork.InRoom)
                {
                    if (rejoinCode != null && !waitRejoin)
                    {
                        rejoinCode = null;
                    }
                }
                else
                {
                    if (GorillaComputer.instance.roomFull && !roomFull)
                    {
                        NotifiLib.SendNotification("<color=blue>[GENESIS]</color> Room is full!");
                        roomFull = true;
                    }
                    if (rejoinCode != null && Time.time > rejoinDelay/* && PhotonNetwork.NetworkingClient.State == ClientState.Disconnected*/)
                    {
                        UnityEngine.Debug.Log("Attempting rejoin");
                        PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(rejoinCode, JoinType.Solo);
                        NotifiLib.SendNotification("<color=blue>[GENESIS]</color> Attempting rejoin.. [to cancel go to room > cancel reconnect]");
                        roomFull = false;
                        GorillaComputer.instance.roomFull = false;
                        rejoinDelay = Time.time + (float)stinkymonkeyTime;
                        waitRejoin = false;
                    }
                }
                if (!Settings.disablestatus)
                {
                    if (mainCamera == null)
                    {
                        mainCamera = Camera.main;

                        GameObject textObject = new GameObject("FloatingText");

                        textObject.transform.position = status3 + new Vector3(0, 0.25f, 0);
                        textObject.name = "GenesisStatus";

                        textMesh = textObject.AddComponent<TextMeshPro>();


                        textMesh.fontSize = 1;
                        textMesh.alignment = TextAlignmentOptions.Center;
                        textMesh.color = Color.white;

                        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
                        rectTransform.sizeDelta = new Vector2(200, 50);
                    }

                    textMesh.text = "<color=blue>[ GENESIS ]</color>\nWelcome to ShibaGT Genesis\nThe best cheat on the market!\n\n<color=yellow>[ MENU STATUS : " + status + " ]</color>";
                    textMesh.transform.LookAt(GorillaTagger.Instance.offlineVRRig.headMesh.transform.position);
                    textMesh.transform.Rotate(0, 180, 0);
                }
                else
                {
                    if (mainCamera != null)
                    {
                        mainCamera = null;
                        Destroy(textMesh);
                        textMesh = null;
                    }
                }

                if (!PhotonNetwork.InRoom)
                {
                    OP.ez = false;
                    OP.antibanflaot = Time.time;
                    if (Settings.TurnOffOP)
                    {
                        foreach (ButtonInfo info in Buttons.buttons[1])
                            info.enabled = false;
                    }
                }
            }
            catch { }
        }

        static Material mat = null;
        static bool mat1;

        public static void Overseer()
        {
            if (Settings.Overseer)
            {
                if (mat == null)
                    mat = new Material(Shader.Find("GUI/Text Shader")) { color = new Color32(255, 255, 255, 90) };

                if (ghostRig == null)
                {
                    ghostRig = Object.Instantiate<VRRig>(
                        GorillaTagger.Instance.offlineVRRig,
                        GorillaLocomotion.GTPlayer.Instance.transform.position,
                        GorillaLocomotion.GTPlayer.Instance.transform.rotation
                    );
                    ghostRig.enabled = false;
                    ghostRig.transform.position = Vector3.zero;

                    ghostRig.transform.Find("VR Constraints/LeftArm/Left Arm IK/SlideAudio").gameObject.SetActive(false);
                    ghostRig.transform.Find("VR Constraints/RightArm/Right Arm IK/SlideAudio").gameObject.SetActive(false);
                }

                if (!GorillaTagger.Instance.offlineVRRig.enabled)
                {
                    mat1 = false;
                    ghostRig.enabled = true;
                    ghostRig.rightHandTransform.position = GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position;
                    ghostRig.leftHandTransform.position = GorillaLocomotion.GTPlayer.Instance.leftHand.controllerTransform.position;
                    ghostRig.mainSkin.material = mat;
                }
                else
                {
                    if (!mat1)
                    {
                        ghostRig.enabled = false;
                        ghostRig.mainSkin.material = null;
                        ghostRig.transform.position = Vector3.zero;
                        mat1 = true;
                    }
                }
            }
            else if (ghostRig != null)
            {
                Destroy(ghostRig);
                ghostRig = null;
            }
        }

        public static GameObject stumptext = null;

        public static AssetBundle assetBundle = null;

        public static GameObject LoadAssetBundle(string assetName)
        {
            GameObject gameObject = null;

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShibaGTGenesis.AssetBundles.gen");
            if (stream != null)
            {
                if (assetBundle == null)
                {
                    assetBundle = AssetBundle.LoadFromStream(stream);
                }
                gameObject = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>(assetName));
            }
            else
            {
                Debug.LogError("Failed to load asset from resource: " + assetName);
            }

            return gameObject;
        }

        public static GameObject LoadAssetBundle2(string fullassetName)
        {
            GameObject gameObject = null;

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShibaGTGenesis.AssetBundles." + fullassetName);
            if (stream != null)
            {
                if (assetBundle == null)
                {
                    assetBundle = AssetBundle.LoadFromStream(stream);
                }
                gameObject = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("IngameGUI"));
            }
            else
            {
                Debug.LogError("Failed to load asset from resource: " + fullassetName);
            }

            return gameObject;
        }

        public static GameObject LoadAssetBundle3Real(string fullassetName)
        {
            GameObject gameObject = null;

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShibaGTGenesis.AssetBundles." + fullassetName);
            if (stream != null)
            {
                if (assetBundle == null)
                {
                    assetBundle = AssetBundle.LoadFromStream(stream);
                }
                gameObject = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>(fullassetName));
            }
            else
            {
                Debug.LogError("Failed to load asset from resource: " + fullassetName);
            }

            return gameObject;
        }

        public static bool imakmsfuckingfaggot = false;
        static bool cocMat;
        static bool hmmm = false;
        public Vector3 position = new Vector3(-66.2834f, 12.2343f, -82.6418f);
        public static Camera mainCamera;
        private Camera mainCamera2;
        private TextMeshPro textMesh;
        public static string motdtext;
        public static string status;
        public static bool changedMotd;
        public static Material BoardsUI = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material BoardsUI2 = new Material(Shader.Find("GorillaTag/UberShader"));
        public static bool FavBool;
        public static bool BoardsBool;
        public static float BoardsDelay;
        public static Material GenesisUI = new Material(Shader.Find("GorillaTag/UberShader"));

        public static int faggot2 = 0;

        public static List<string> cocboardstrings = new List<string>();

        bool sentbefore = false;

        static bool doneDid;
        public static VRRig ghostRig;

        public static bool hasFoundAllBoards;
        public static bool disableBoardColor;
        public static GameObject motd;
        public static GameObject motdText;

        public static int fullModAmount;

        public static JoinTriggerUITemplate savedtEMP;
        public static JoinTriggerUI savedUi;
        public static Material savedMAT;
        public static Material savedmat2;


        public static void ChangeBoards()
        {
            GenesisUI.color = Color.Lerp(WristMenu.colorToFade1, WristMenu.colorToFade2, Mathf.PingPong(Time.time, 1f));
            if (Settings.rainbow)
            {
                GenesisUI.color = UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f);
            }
            if (!hasFoundAllBoards)
            {
                try
                {
                    UnityEngine.Debug.Log("Looking for boards");
                    bool found = false;
                    int indexOfThatThing = 0;
                    for (int i = 0; i < GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom").transform.childCount; i++)
                    {
                        GameObject v = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom").transform.GetChild(i).gameObject;
                        if (v.name.Contains("UnityTempFile-"))
                        {
                            indexOfThatThing++;
                            if (indexOfThatThing == 3)
                            {
                                found = true;
                                savedMAT = v.GetComponent<Renderer>().material;
                                v.GetComponent<Renderer>().material = GenesisUI;

                            }
                        }
                    }

                    if (found)
                    {
                        GameObject vr = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomBoundaryStones/BoundaryStoneSet_Forest/wallmonitorforestbg");
                        if (vr != null)
                        {
                            vr.GetComponent<Renderer>().material = GenesisUI;
                        }

                        foreach (GorillaNetworkJoinTrigger v in (List<GorillaNetworkJoinTrigger>)typeof(PhotonNetworkController).GetField("allJoinTriggers", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(PhotonNetworkController.Instance))
                        {
                            try
                            {
                                JoinTriggerUI ui = (JoinTriggerUI)typeof(GorillaNetworkJoinTrigger).GetField("ui", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(v);
                                JoinTriggerUITemplate temp = (JoinTriggerUITemplate)typeof(JoinTriggerUI).GetField("template", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(ui);

                                temp.ScreenBG_AbandonPartyAndSoloJoin = GenesisUI;
                                temp.ScreenBG_AlreadyInRoom = GenesisUI;
                                temp.ScreenBG_Error = GenesisUI;
                                temp.ScreenBG_InPrivateRoom = GenesisUI;
                                temp.ScreenBG_LeaveRoomAndGroupJoin = GenesisUI;
                                temp.ScreenBG_LeaveRoomAndSoloJoin = GenesisUI;
                                temp.ScreenBG_NotConnectedSoloJoin = GenesisUI;

                            }
                            catch { }
                        }
                        PhotonNetworkController.Instance.UpdateTriggerScreens();

                        hasFoundAllBoards = true;
                        UnityEngine.Debug.Log("Found all boards");
                    }
                }
                catch (Exception exception)
                {
                    UnityEngine.Debug.LogError(string.Format("<color=blue>[GENESIS]</color> <b>COLOR ERROR</b> {1} - {0}", exception.Message, exception.StackTrace));
                    hasFoundAllBoards = false;
                }
            }
            try
            {

                if (motd == null)
                {
                    GameObject motdThing = GameObject.Find("motdHeadingText");
                    motd = UnityEngine.Object.Instantiate(motdThing, motdThing.transform.parent);
                    motdThing.SetActive(false);
                }
                TMP_Text motdTC = motd.GetComponent<TMP_Text>();
                RectTransform motdTRC = motd.GetComponent<RectTransform>();
                motdTC.text = "Genesis";

                if (motdText == null)
                {
                    motdText = GameObject.Find("motdBodyText").gameObject;
                    motdText.GetComponent<PlayFabTitleDataTextDisplay>().enabled = false;
                }
                TMP_Text motdTextB = motdText.GetComponent<TMP_Text>();
                if (fullModAmount == 0)
                {
                    fullModAmount = 0;
                    GetAllMods();
                }
                motdTextB.text = string.Format("Thanks for using ShibaGT Genesis! This menu is the #1 on the market.\nWe currently have <color=yellow>[ " + fullModAmount + " ]</color> amount of mods!\n\n<color=yellow>[ShibaGT]</color> - Menu\n<color=yellow>[Fragment]</color> - GUI");
            }
            catch { }
        }

        public static bool hasPanel;
        public static bool attachFeatures;

        public static void GetAllMods()
        {
            foreach (ButtonInfo[] buttons in Buttons.buttons)
            {
                fullModAmount += buttons.Length;
            }
        }

        public static void changinlayers(Transform target)
        {
            if (target.gameObject.layer == LayerMask.NameToLayer("Gorilla Object"))
            {
                target.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            foreach (Transform child in target)
            {
                changinlayers(child);
            }
        }

        public static bool multiStuff;

        public static bool searchbool;
        public static bool boolboool;
        public static ButtonInfo[] backup = new ButtonInfo[] { };
        public static ButtonInfo[] backupcat = Buttons.buttons[0];

        public static float ratedelay;
        public static bool loadedCosmetics;

        public void Draw()
        {
            if (returnedstring == "GEN")
            {
                return;
            }

            menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.Destroy(menu.GetComponent<Rigidbody>());
            Object.Destroy(menu.GetComponent<BoxCollider>());
            Object.Destroy(menu.GetComponent<Renderer>());
            menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f) * menuSize;
            menuObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.Destroy(menuObj.GetComponent<Rigidbody>());
            Object.Destroy(menuObj.GetComponent<BoxCollider>());
            menuObj.transform.parent = menu.transform;
            menuObj.transform.rotation = Quaternion.identity;
            menuObj.transform.localScale = new Vector3(0.1f, 1f, 1f) * menuSize;
            if (!Settings.gradient)
            {
                if (!Settings.custom)
                {
                    GradientColorKey[] array = new GradientColorKey[3];
                    array[0].color = colorToFade1;
                    array[0].time = 0f;
                    array[1].color = colorToFade2;
                    array[1].time = 0.5f;
                    array[2].color = colorToFade1;
                    array[2].time = 1f;
                    ColorChanger colorChanger2 = menuObj.AddComponent<ColorChanger>();
                    colorChanger2.colors = new Gradient
                    {
                        colorKeys = array
                    };
                    colorChanger2.Start();
                }
                else
                {
                    if (customGradient == null)
                        customGradient = LegacyGUI.LoadLocalImage("GenesisPrefs\\customImage.png");

                    menuObj.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    menuObj.GetComponent<Renderer>().material.mainTexture = customGradient;
                }
            }
            else
            {
                if (backgroundGradient == null)
                    backgroundGradient = LegacyGUI.LoadLocalImage(backgroundGradientUrl);

                menuObj.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                menuObj.GetComponent<Renderer>().material.mainTexture = backgroundGradient;
            }
            

            menuObj.transform.position = new Vector3(0.05f, 0f, 0f) * menuSize;
            canvasObj = new GameObject();
            canvasObj.transform.parent = menu.transform;
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            canvasObj.transform.localScale *= menuSize;
            canvas.renderMode = RenderMode.WorldSpace;
            canvasScaler.dynamicPixelsPerUnit = 1000f;
            Text text = new GameObject
            {
                transform =
                {
                    parent = canvasObj.transform
                }
            }.AddComponent<Text>();
            text.gameObject.name = "name";
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titiel = text;
            int yau = pageNumber + 1;
            text.text = MenuTitle;
            text.fontSize = fontSize;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = Settings.autoResize;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.28f, 0.05f) * menuSize;
            component.position = new Vector3(0.06f, 0f, 0.185f) * menuSize;
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            Text fpstext = new GameObject
            {
                transform =
                {
                    parent = canvasObj.transform
                }
            }.AddComponent<Text>();
            fpstext.gameObject.name = "fpstext";
            fpstext.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            fpstextt = fpstext;
            fpstext.text = $"Fps: {1.0f / Time.deltaTime}";
            fpstext.fontSize = fontSize;
            fpstext.alignment = TextAnchor.MiddleCenter;
            fpstext.resizeTextForBestFit = Settings.autoResize;
            fpstext.resizeTextMinSize = 0;
            RectTransform componentfps = fpstext.GetComponent<RectTransform>();
            componentfps.localPosition = Vector3.zero;
            componentfps.sizeDelta = new Vector2(0.28f, 0.02f) * menuSize;
            componentfps.position = new Vector3(0.06f, 0f, 0.165f) * menuSize;
            componentfps.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            if (Settings.outline)
            {
                OutlineObj(menuObj, menuOutlineColor, menuOutlineColor, false, 3);
            }

            AddPageButtons();

            List<ButtonInfo> buttons = Buttons.buttons[Back.category].ToList();

            if (Back.isSearching)
            {
                buttons.Clear();

                foreach (ButtonInfo[] buttonn in Buttons.buttons)
                {
                    if (buttonn != Buttons.buttons[13] && buttonn != Buttons.buttons[11])
                    foreach (ButtonInfo button in buttonn)
                    {
                        if (button.buttonText.Replace(" ", "").ToLower().Contains(Back.searchPrompt.Replace(" ", "").ToLower()))
                        {
                            buttons.Add(button);
                        }
                    }
                }
            }

            string[] array2 = buttons.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            if (Back.InHub())
            {
                for (int i = 0; i < array2.Length; i++)
                {
                    AddButton((float)i * 0.10f + 0.39f * 1f, array2[i]);
                }
                return;
            }
            for (int i = 0; i < array2.Length; i++)
            {
                AddButton((float)i * 0.10f + 0.26f * 1f, array2[i]);
            }
        }

        public static Text titiel;

        public static Text fpstextt;

        private static void AddPageButtons()
        {
            List<ButtonInfo> buttons = Buttons.buttons[Back.category].ToList();
            int num = (buttons.Count + pageSize - 1) / pageSize;
            int num2 = pageNumber + 1;
            int num3 = pageNumber - 1;
            if (num2 > num - 1)
            {
                num2 = 0;
            }
            if (num3 < 0)
            {
                num3 = num - 1;
            }
            float num4 = 0f;

            Vector3 normPrevPos = new Vector3(0.56f, 0.37f, 0.541f) * menuSize;
            Vector3 normNextPos = new Vector3(0.56f, -0.37f, 0.541f) * menuSize;
            Vector3 normPageSize = new Vector3(0.045f, 0.25f, 0.064295f) * menuSize;

            Vector3 sidePrevPos = new Vector3(0.56f, 0.657f, 0.0063f) * menuSize;
            Vector3 sideNextPos = new Vector3(0.56f, -0.657f, 0.0063f) * menuSize;
            Vector3 sidePageSize = new Vector3(0.045f, 0.25f, 0.8936298f) * menuSize;

            Vector3 bottomPrevPos = new Vector3(0.56f, 0.392f, -0.423f) * menuSize;
            Vector3 bottomNextPos = new Vector3(0.56f, -0.392f, -0.423f) * menuSize;
            Vector3 bottomPageSize = new Vector3(0.045f, -0.2123475f, 0.1541571f) * menuSize;

            Vector3 leavePos = new Vector3(0.56f, 0f, 0.5616f) * menuSize;
            Vector3 leaveSize = new Vector3(0.045f, 0.4223921f, 0.1059686f) * menuSize;

            Vector3 leaveTextPos = new Vector3(0.06f, 0f, 0.33f - 0.26f / 2.55f) * menuSize;
            Vector2 leaveTextSize = new Vector2(0.2f, 0.03f) * menuSize;







            //0 = norm
            //1 = side
            //2 = trig
            //3 = bottom













            //SEARCH BAR
            if (Back.InHub())
            {
                pageSize = 7;


                GameObject gameObjectsearch = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObjectsearch.name = "searchbar";
                UnityEngine.Object.Destroy(gameObjectsearch.GetComponent<Rigidbody>());
                gameObjectsearch.GetComponent<BoxCollider>().isTrigger = true;
                gameObjectsearch.transform.parent = menu.transform;
                gameObjectsearch.transform.rotation = Quaternion.identity;
                gameObjectsearch.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f) * menuSize;
                gameObjectsearch.transform.localPosition = new Vector3(0.56f, 0f, 0.34f) * menuSize;
                gameObjectsearch.AddComponent<BtnCollider>().relatedText = "SearchBar";
                GradientColorKey[] arraysearch = new GradientColorKey[3];
                arraysearch[0].color = colorToFade1;
                arraysearch[0].time = 0f;
                arraysearch[1].color = colorToFade2;
                arraysearch[1].time = 0.5f;
                arraysearch[2].color = colorToFade1;
                arraysearch[2].time = 1f;
                ColorChanger colorChangers = gameObjectsearch.AddComponent<ColorChanger>();
                colorChangers.colors = new Gradient
                {
                    colorKeys = arraysearch
                };
                colorChangers.Start();

                OutlineObj(gameObjectsearch, buttonColor, buttonColor);

                //search text

                GameObject gameObjectsearchtext = new GameObject();
                gameObjectsearchtext.name = "searchtext";
                gameObjectsearchtext.transform.parent = canvasObj.transform;
                Text textsearch = gameObjectsearchtext.AddComponent<Text>();
                textsearch.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                if (!Back.isSearching)
                {
                    textsearch.text = "Search...";
                }
                else
                {
                    textsearch.text = Back.searchPrompt;
                }
                textsearch.fontSize = fontSize;
                textsearch.color = menuOffTextColor;
                textsearch.alignment = TextAnchor.MiddleLeft;
                textsearch.resizeTextForBestFit = Settings.autoResize;
                textsearch.resizeTextMinSize = 0;
                RectTransform componentsearch = textsearch.GetComponent<RectTransform>();
                componentsearch.localPosition = Vector3.zero;
                componentsearch.sizeDelta = new Vector2(0.2f, 0.03f) * menuSize;
                componentsearch.localPosition = new Vector3(0.064f, 0f, 0.24f - 0.26f / 2.55f);
                componentsearch.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            }
            else
            {
                pageSize = 8;
            }


            //actual layout




            // MAKING PREV
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObject.name = "prev";
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = menu.transform;
            gameObject.transform.rotation = Quaternion.identity;

            if (MenuLayout == 0)
            {
                gameObject.transform.localScale = normPageSize;
                gameObject.transform.localPosition = normPrevPos;

                if (!Settings.gradient)
                {
                    GradientColorKey[] array2 = new GradientColorKey[3];
                    array2[0].color = colorToFade1;
                    array2[0].time = 0f;
                    array2[1].color = colorToFade2;
                    array2[1].time = 0.5f;
                    array2[2].color = colorToFade1;
                    array2[2].time = 1f;
                    ColorChanger colorChanger2 = gameObject.AddComponent<ColorChanger>();
                    colorChanger2.colors = new Gradient
                    {
                        colorKeys = array2
                    };
                    colorChanger2.Start();
                }
                else
                {
                    if (pageGradient == null)
                        pageGradient = LegacyGUI.DownloadImage(pageGradientUrl);

                    gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    gameObject.GetComponent<Renderer>().material.mainTexture = pageGradient;
                }
            }
            if (MenuLayout == 1)
            {
                gameObject.transform.localScale = sidePageSize;
                gameObject.transform.localPosition = sidePrevPos;

                if (!Settings.gradient)
                {
                    GradientColorKey[] array2 = new GradientColorKey[3];
                    array2[0].color = colorToFade1;
                    array2[0].time = 0f;
                    array2[1].color = colorToFade2;
                    array2[1].time = 0.5f;
                    array2[2].color = colorToFade1;
                    array2[2].time = 1f;
                    ColorChanger colorChanger2 = gameObject.AddComponent<ColorChanger>();
                    colorChanger2.colors = new Gradient
                    {
                        colorKeys = array2
                    };
                    colorChanger2.Start();
                }
                else
                {
                    if (backgroundGradient == null)
                        backgroundGradient = LegacyGUI.DownloadImage(backgroundGradientUrl);

                    gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    gameObject.GetComponent<Renderer>().material.mainTexture = backgroundGradient;
                }
            }
            if (MenuLayout == 2)
            {
                gameObject.transform.localScale = Vector3.zero;
                gameObject.transform.localPosition = Vector3.zero;
            }
            if (MenuLayout == 3)
            {
                gameObject.transform.localScale = bottomPageSize;
                gameObject.transform.localPosition = bottomPrevPos;

                GradientColorKey[] array2 = new GradientColorKey[3];
                array2[0].color = colorToFade1;
                array2[0].time = 0f;
                array2[1].color = colorToFade2;
                array2[1].time = 0.5f;
                array2[2].color = colorToFade1;
                array2[2].time = 1f;
                ColorChanger colorChanger2 = gameObject.AddComponent<ColorChanger>();
                colorChanger2.colors = new Gradient
                {
                    colorKeys = array2
                };
                colorChanger2.Start();
            }

            if (Settings.outline)
            {
                OutlineObj(gameObject, menuOutlineColor, menuOutlineColor, false, 3);
            }

            gameObject.AddComponent<BtnCollider>().relatedText = "PreviousPage";



            //MAKING NEXT



            GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObject3.name = "next";
            UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
            gameObject3.GetComponent<BoxCollider>().isTrigger = true;
            gameObject3.transform.parent = menu.transform;
            gameObject3.transform.rotation = Quaternion.identity;

            if (MenuLayout == 0)
            {
                gameObject3.transform.localScale = normPageSize;
                gameObject3.transform.localPosition = normNextPos;

                if (!Settings.gradient)
                {
                    GradientColorKey[] array2 = new GradientColorKey[3];
                    array2[0].color = colorToFade1;
                    array2[0].time = 0f;
                    array2[1].color = colorToFade2;
                    array2[1].time = 0.5f;
                    array2[2].color = colorToFade1;
                    array2[2].time = 1f;
                    ColorChanger colorChanger2 = gameObject3.AddComponent<ColorChanger>();
                    colorChanger2.colors = new Gradient
                    {
                        colorKeys = array2
                    };
                    colorChanger2.Start();
                }
                else
                {
                    if (pageGradient == null)
                        pageGradient = LegacyGUI.DownloadImage(pageGradientUrl);

                    gameObject3.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    gameObject3.GetComponent<Renderer>().material.mainTexture = pageGradient;
                }
            }
            if (MenuLayout == 1)
            {
                gameObject3.transform.localScale = sidePageSize;
                gameObject3.transform.localPosition = sideNextPos;

                if (!Settings.gradient)
                {
                    GradientColorKey[] array2 = new GradientColorKey[3];
                    array2[0].color = colorToFade1;
                    array2[0].time = 0f;
                    array2[1].color = colorToFade2;
                    array2[1].time = 0.5f;
                    array2[2].color = colorToFade1;
                    array2[2].time = 1f;
                    ColorChanger colorChanger2 = gameObject3.AddComponent<ColorChanger>();
                    colorChanger2.colors = new Gradient
                    {
                        colorKeys = array2
                    };
                    colorChanger2.Start();
                }
                else
                {
                    if (backgroundGradient == null)
                        backgroundGradient = LegacyGUI.DownloadImage(backgroundGradientUrl);

                    gameObject3.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    gameObject3.GetComponent<Renderer>().material.mainTexture = backgroundGradient;
                }
            }
            if (MenuLayout == 2)
            {
                gameObject3.transform.localScale = Vector3.zero;
                gameObject3.transform.localPosition = Vector3.zero;
            }
            if (MenuLayout == 3)
            {
                gameObject3.transform.localScale = bottomPageSize;
                gameObject3.transform.localPosition = bottomNextPos;

                GradientColorKey[] array2 = new GradientColorKey[3];
                array2[0].color = colorToFade1;
                array2[0].time = 0f;
                array2[1].color = colorToFade2;
                array2[1].time = 0.5f;
                array2[2].color = colorToFade1;
                array2[2].time = 1f;
                ColorChanger colorChanger2 = gameObject3.AddComponent<ColorChanger>();
                colorChanger2.colors = new Gradient
                {
                    colorKeys = array2
                };
                colorChanger2.Start();
            }

            if (Settings.outline)
            {
                OutlineObj(gameObject3, menuOutlineColor, menuOutlineColor, false, 3);
            }

            gameObject3.AddComponent<BtnCollider>().relatedText = "NextPage";

            //MAKING DISCONNECT

            GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObject2.name = "disconnect";
            UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
            gameObject2.GetComponent<BoxCollider>().isTrigger = true;
            gameObject2.transform.parent = menu.transform;
            gameObject2.transform.rotation = Quaternion.identity;
            gameObject2.transform.localScale = leaveSize;
            gameObject2.transform.localPosition = leavePos;

            if (Settings.outline)
            {
                OutlineObj(gameObject2, menuOutlineColor, menuOutlineColor, false, 3);
            }

            gameObject2.AddComponent<BtnCollider>().relatedText = "DisconnectingButton";
            if (!Settings.gradient)
            {
                GradientColorKey[] array2 = new GradientColorKey[3];
                array2[0].color = colorToFade1;
                array2[0].time = 0f;
                array2[1].color = colorToFade2;
                array2[1].time = 0.5f;
                array2[2].color = colorToFade1;
                array2[2].time = 1f;
                ColorChanger colorChanger2 = gameObject2.AddComponent<ColorChanger>();
                colorChanger2.colors = new Gradient
                {
                    colorKeys = array2
                };
                colorChanger2.Start();
            }
            else
            {
                if (pageGradient == null)
                    pageGradient = LegacyGUI.DownloadImage(pageGradientUrl);

                gameObject2.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                gameObject2.GetComponent<Renderer>().material.mainTexture = pageGradient;
            }

            //MAKING DISCONNECT TEXT

            GameObject gameObject4 = new GameObject();
            gameObject4.name = "disconnect text";
            gameObject4.transform.parent = canvasObj.transform;
            Text text2 = gameObject4.AddComponent<Text>();
            text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text2.text = "Leave";
            text2.fontSize = fontSize;
            text2.alignment = TextAnchor.MiddleCenter;
            text2.resizeTextForBestFit = Settings.autoResize;
            text2.resizeTextMinSize = 0;
            RectTransform component2 = text2.GetComponent<RectTransform>();
            component2.localPosition = Vector3.zero;
            component2.sizeDelta = leaveTextSize;
            component2.localPosition = leaveTextPos;
            component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));







            /*
            if (MenuLayout == 0)
            {

                //normal


                // MAKING PREV
                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject.name = "prev";
                UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.transform.parent = menu.transform;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f) * menuSize;
                gameObject.transform.localPosition = new Vector3(0.56f, 0.37f, 0.541f) * menuSize;
                gameObject.AddComponent<BtnCollider>().relatedText = "PreviousPage";
                GradientColorKey[] array2 = new GradientColorKey[3];
                array2[0].color = colorToFade1;
                array2[0].time = 0f;
                array2[1].color = colorToFade2;
                array2[1].time = 0.5f;
                array2[2].color = colorToFade1;
                array2[2].time = 1f;
                ColorChanger colorChanger2 = gameObject.AddComponent<ColorChanger>();
                colorChanger2.colors = new Gradient
                {
                    colorKeys = array2
                };
                colorChanger2.Start();

                //MAKING NEXT

                GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject3.name = "next";
                UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject3.GetComponent<BoxCollider>().isTrigger = true;
                gameObject3.transform.parent = menu.transform;
                gameObject3.transform.rotation = Quaternion.identity;
                gameObject3.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f) * menuSize;
                gameObject3.transform.localPosition = new Vector3(0.56f, -0.37f, 0.541f) * menuSize;
                gameObject3.AddComponent<BtnCollider>().relatedText = "NextPage";
                GradientColorKey[] array3 = new GradientColorKey[3];
                array3[0].color = colorToFade1;
                array3[0].time = 0f;
                array3[1].color = colorToFade2;
                array3[1].time = 0.5f;
                array3[2].color = colorToFade1;
                array3[2].time = 1f;
                ColorChanger colorChanger3 = gameObject3.AddComponent<ColorChanger>();
                colorChanger3.colors = new Gradient
                {
                    colorKeys = array3
                };
                colorChanger3.Start();
                num4 = 0.26f;

                //MAKING DISCONNECT

                GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject2.name = "disconnect";
                UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject2.GetComponent<BoxCollider>().isTrigger = true;
                gameObject2.transform.parent = menu.transform;
                gameObject2.transform.rotation = Quaternion.identity;
                gameObject2.transform.localScale = new Vector3(0.045f, 0.4223921f, 0.1059686f) * menuSize;
                gameObject2.transform.localPosition = new Vector3(0.56f, 0f, 0.5616f) * menuSize;
                gameObject2.AddComponent<BtnCollider>().relatedText = "DisconnectingButton";
                GradientColorKey[] array = new GradientColorKey[3];
                array[0].color = colorToFade1;
                array[0].time = 0f;
                array[1].color = colorToFade2;
                array[1].time = 0.5f;
                array[2].color = colorToFade1;
                array[2].time = 1f;
                ColorChanger colorChanger = gameObject2.AddComponent<ColorChanger>();
                colorChanger.colors = new Gradient
                {
                    colorKeys = array
                };
                colorChanger.Start();

                //MAKING DISCONNECT TEXT

                GameObject gameObject4 = new GameObject();
                gameObject4.name = "disconnect text";
                gameObject4.transform.parent = canvasObj.transform;
                Text text2 = gameObject4.AddComponent<Text>();
                text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text2.text = "Leave";
                text2.fontSize = fontSize;
                text2.alignment = TextAnchor.MiddleCenter;
                text2.resizeTextForBestFit = Settings.autoResize;
                text2.resizeTextMinSize = 0;
                RectTransform component2 = text2.GetComponent<RectTransform>();
                component2.localPosition = Vector3.zero;
                component2.sizeDelta = new Vector2(0.2f, 0.03f) * menuSize;
                component2.localPosition = new Vector3(0.06f, 0f, 0.33f - num4 / 2.55f) * menuSize;
                component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
            if (MenuLayout == 1)
            {

                //side


                // MAKING PREV
                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject.name = "prev";
                UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.transform.parent = menu.transform;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(0.045f, 0.25f, 0.8936298f) * menuSize;
                gameObject.transform.localPosition = new Vector3(0.56f, 0.657f, 0.0063f) * menuSize;
                gameObject.AddComponent<BtnCollider>().relatedText = "PreviousPage";
                GradientColorKey[] array2 = new GradientColorKey[3];
                array2[0].color = colorToFade1;
                array2[0].time = 0f;
                array2[1].color = colorToFade2;
                array2[1].time = 0.5f;
                array2[2].color = colorToFade1;
                array2[2].time = 1f;
                ColorChanger colorChanger2 = gameObject.AddComponent<ColorChanger>();
                colorChanger2.colors = new Gradient
                {
                    colorKeys = array2
                };
                colorChanger2.Start();

                //MAKING NEXT

                GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject3.name = "next";
                UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject3.GetComponent<BoxCollider>().isTrigger = true;
                gameObject3.transform.parent = menu.transform;
                gameObject3.transform.rotation = Quaternion.identity;
                gameObject3.transform.localScale = new Vector3(0.045f, 0.25f, 0.8936298f) * menuSize;
                gameObject3.transform.localPosition = new Vector3(0.56f, -0.657f, 0.0063f) * menuSize;
                gameObject3.AddComponent<BtnCollider>().relatedText = "NextPage";
                GradientColorKey[] array3 = new GradientColorKey[3];
                array3[0].color = colorToFade1;
                array3[0].time = 0f;
                array3[1].color = colorToFade2;
                array3[1].time = 0.5f;
                array3[2].color = colorToFade1;
                array3[2].time = 1f;
                ColorChanger colorChanger3 = gameObject3.AddComponent<ColorChanger>();
                colorChanger3.colors = new Gradient
                {
                    colorKeys = array3
                };
                colorChanger3.Start();
                num4 = 0.26f;

                //MAKING DISCONNECT

                GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject2.name = "disconnect";
                UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject2.GetComponent<BoxCollider>().isTrigger = true;
                gameObject2.transform.parent = menu.transform;
                gameObject2.transform.rotation = Quaternion.identity;
                gameObject2.transform.localScale = new Vector3(0.045f, 0.4223921f, 0.1059686f) * menuSize;
                gameObject2.transform.localPosition = new Vector3(0.56f, 0f, 0.5616f) * menuSize;
                gameObject2.AddComponent<BtnCollider>().relatedText = "DisconnectingButton";
                GradientColorKey[] array = new GradientColorKey[3];
                array[0].color = colorToFade1;
                array[0].time = 0f;
                array[1].color = colorToFade2;
                array[1].time = 0.5f;
                array[2].color = colorToFade1;
                array[2].time = 1f;
                ColorChanger colorChanger = gameObject2.AddComponent<ColorChanger>();
                colorChanger.colors = new Gradient
                {
                    colorKeys = array
                };
                colorChanger.Start();

                //MAKING DISCONNECT TEXT

                GameObject gameObject4 = new GameObject();
                gameObject4.name = "disconnect text";
                gameObject4.transform.parent = canvasObj.transform;
                Text text2 = gameObject4.AddComponent<Text>();
                text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text2.text = "Leave";
                text2.fontSize = fontSize;
                text2.alignment = TextAnchor.MiddleCenter;
                text2.resizeTextForBestFit = Settings.autoResize;
                text2.resizeTextMinSize = 0;
                RectTransform component2 = text2.GetComponent<RectTransform>();
                component2.localPosition = Vector3.zero;
                component2.sizeDelta = new Vector2(0.2f, 0.03f) * menuSize;
                component2.localPosition = new Vector3(0.06f, 0f, 0.33f - num4 / 2.55f) * menuSize;
                component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
            if (MenuLayout == 2)
            {

                //triggers

                //back

                //MAKING DISCONNECT

                //MAKING DISCONNECT

                GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject2.name = "disconnect";
                UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
                gameObject2.GetComponent<BoxCollider>().isTrigger = true;
                gameObject2.transform.parent = menu.transform;
                gameObject2.transform.rotation = Quaternion.identity;
                gameObject2.transform.localScale = new Vector3(0.045f, 0.4223921f, 0.1059686f) * menuSize;
                gameObject2.transform.localPosition = new Vector3(0.56f, 0f, 0.5616f) * menuSize;
                gameObject2.AddComponent<BtnCollider>().relatedText = "DisconnectingButton";
                GradientColorKey[] array = new GradientColorKey[3];
                array[0].color = colorToFade1;
                array[0].time = 0f;
                array[1].color = colorToFade2;
                array[1].time = 0.5f;
                array[2].color = colorToFade1;
                array[2].time = 1f;
                ColorChanger colorChanger = gameObject2.AddComponent<ColorChanger>();
                colorChanger.colors = new Gradient
                {
                    colorKeys = array
                };
                colorChanger.Start();


                //MAKING DISCONNECT TEXT

                GameObject gameObject4 = new GameObject();
                gameObject4.name = "disconnect text";
                gameObject4.transform.parent = canvasObj.transform;
                Text text2 = gameObject4.AddComponent<Text>();
                text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text2.text = "Leave";
                text2.fontSize = fontSize;
                text2.alignment = TextAnchor.MiddleCenter;
                text2.resizeTextForBestFit = Settings.autoResize;
                text2.resizeTextMinSize = 0;
                RectTransform component2 = text2.GetComponent<RectTransform>();
                component2.localPosition = Vector3.zero;
                component2.sizeDelta = new Vector2(0.2f, 0.03f) * menuSize;
                num4 = 0.26f;
                component2.localPosition = new Vector3(0.06f, 0f, 0.33f - num4 / 2.55f) * menuSize;
                component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
            if (MenuLayout == 3)
            {

                //bottom


                // MAKING PREV
                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject.name = "prev";
                UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.transform.parent = menu.transform;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(0.045f, -0.2123475f, 0.1541571f) * menuSize;
                gameObject.transform.localPosition = new Vector3(0.56f, 0.392f, -0.423f) * menuSize;
                gameObject.AddComponent<BtnCollider>().relatedText = "PreviousPage";
                GradientColorKey[] array3 = new GradientColorKey[3];
                array3[0].color = colorToFade1;
                array3[0].time = 0f;
                array3[1].color = colorToFade2;
                array3[1].time = 0.5f;
                array3[2].color = colorToFade1;
                array3[2].time = 1f;
                ColorChanger colorChanger3 = gameObject.AddComponent<ColorChanger>();
                colorChanger3.colors = new Gradient
                {
                    colorKeys = array3
                };
                colorChanger3.Start();

                //MAKING NEXT

                GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject3.name = "next";
                UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject3.GetComponent<BoxCollider>().isTrigger = true;
                gameObject3.transform.parent = menu.transform;
                gameObject3.transform.rotation = Quaternion.identity;
                gameObject3.transform.localScale = new Vector3(0.045f, -0.2123475f, 0.1541571f) * menuSize;
                gameObject3.transform.localPosition = new Vector3(0.56f, -0.392f, -0.423f) * menuSize;
                gameObject3.AddComponent<BtnCollider>().relatedText = "NextPage";
                GradientColorKey[] array2 = new GradientColorKey[3];
                array2[0].color = colorToFade1;
                array2[0].time = 0f;
                array2[1].color = colorToFade2;
                array2[1].time = 0.5f;
                array2[2].color = colorToFade1;
                array2[2].time = 1f;
                ColorChanger colorChanger2 = gameObject3.AddComponent<ColorChanger>();
                colorChanger2.colors = new Gradient
                {
                    colorKeys = array2
                };
                colorChanger2.Start();
                num4 = 0.26f;

                //MAKING DISCONNECT

                GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject2.name = "disconnect";
                UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject2.GetComponent<BoxCollider>().isTrigger = true;
                gameObject2.transform.parent = menu.transform;
                gameObject2.transform.rotation = Quaternion.identity;
                gameObject2.transform.localScale = new Vector3(0.045f, 0.4223921f, 0.1059686f) * menuSize;
                gameObject2.transform.localPosition = new Vector3(0.56f, 0f, 0.5616f) * menuSize;
                gameObject2.AddComponent<BtnCollider>().relatedText = "DisconnectingButton";
                GradientColorKey[] array = new GradientColorKey[3];
                array[0].color = colorToFade1;
                array[0].time = 0f;
                array[1].color = colorToFade2;
                array[1].time = 0.5f;
                array[2].color = colorToFade1;
                array[2].time = 1f;
                ColorChanger colorChanger = gameObject2.AddComponent<ColorChanger>();
                colorChanger.colors = new Gradient
                {
                    colorKeys = array
                };
                colorChanger.Start();

                //MAKING DISCONNECT TEXT

                GameObject gameObject4 = new GameObject();
                gameObject4.name = "disconnect text";
                gameObject4.transform.parent = canvasObj.transform;
                Text text2 = gameObject4.AddComponent<Text>();
                text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text2.text = "Leave";
                text2.fontSize = fontSize;
                text2.alignment = TextAnchor.MiddleCenter;
                text2.resizeTextForBestFit = Settings.autoResize;
                text2.resizeTextMinSize = 0;
                RectTransform component2 = text2.GetComponent<RectTransform>();
                component2.localPosition = Vector3.zero;
                component2.sizeDelta = new Vector2(0.2f, 0.03f) * menuSize;
                component2.localPosition = new Vector3(0.06f, 0f, 0.33f - num4 / 2.55f) * menuSize;
                component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
            */ //old layout sys

            //home

            GameObject gameObjecthome = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObjecthome.name = "home";
            UnityEngine.Object.Destroy(gameObjecthome.GetComponent<Rigidbody>());
            gameObjecthome.GetComponent<BoxCollider>().isTrigger = true;
            gameObjecthome.transform.parent = menu.transform;
            gameObjecthome.transform.rotation = Quaternion.identity;
            gameObjecthome.transform.localScale = new Vector3(0.002691f, 0.14f, 0.1f) * menuSize;
            gameObjecthome.transform.localPosition = new Vector3(0.56f, -0.3892007f, -0.57f) * menuSize;
            gameObjecthome.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            gameObjecthome.GetComponent<Renderer>().material.color = Color.black;
            if (home == null)
                home = LegacyGUI.DownloadImage("https://genesis.menu/home.png");

            gameObjecthome.GetComponent<Renderer>().material.mainTexture = home;
            gameObjecthome.AddComponent<BtnCollider>().relatedText = "Home";

            if (Settings.firstColorInt != 0 || Settings.secondColorInt != 0)
            {
                GradientColorKey[] arrayhome = new GradientColorKey[3];
                arrayhome[0].color = colorToFade1;
                arrayhome[0].time = 0f;
                arrayhome[1].color = colorToFade2;
                arrayhome[1].time = 0.5f;
                arrayhome[2].color = colorToFade1;
                arrayhome[2].time = 1f;
                ColorChanger colorChangerhome = gameObjecthome.AddComponent<ColorChanger>();
                colorChangerhome.colors = new Gradient
                {
                    colorKeys = arrayhome
                };
                colorChangerhome.Start();
            }
            else
            {
                gameObjecthome.GetComponent<Renderer>().material.color = Color.white;
            }

            //hop

            GameObject gameObjecthop = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObjecthop.name = "serverhop";
            UnityEngine.Object.Destroy(gameObjecthop.GetComponent<Rigidbody>());
            gameObjecthop.GetComponent<BoxCollider>().isTrigger = true;
            gameObjecthop.transform.parent = menu.transform;
            gameObjecthop.transform.rotation = Quaternion.identity;
            gameObjecthop.transform.localScale = new Vector3(0.002691f, 0.14f, 0.1f) * menuSize;
            gameObjecthop.transform.localPosition = new Vector3(0.56f, -0.2306004f, -0.57f) * menuSize;
            gameObjecthop.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            gameObjecthop.GetComponent<Renderer>().material.color = Color.black;
            if (serverhop == null)
                serverhop = LegacyGUI.DownloadImage("https://genesis.menu/serverhop.png");

            gameObjecthop.GetComponent<Renderer>().material.mainTexture = serverhop;
            gameObjecthop.AddComponent<BtnCollider>().relatedText = "Serverhop";
            if (Settings.firstColorInt != 0 || Settings.secondColorInt != 0)
            {
                GradientColorKey[] arrayhop = new GradientColorKey[3];
                arrayhop[0].color = colorToFade1;
                arrayhop[0].time = 0f;
                arrayhop[1].color = colorToFade2;
                arrayhop[1].time = 0.5f;
                arrayhop[2].color = colorToFade1;
                arrayhop[2].time = 1f;
                ColorChanger colorChangerhop = gameObjecthop.AddComponent<ColorChanger>();
                colorChangerhop.colors = new Gradient
                {
                    colorKeys = arrayhop
                };
                colorChangerhop.Start();
            }
            else
            {
                gameObjecthop.GetComponent<Renderer>().material.color = Color.white;
            }

            //antilag

            GameObject gameObjectlag = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObjectlag.name = "antilag";
            UnityEngine.Object.Destroy(gameObjectlag.GetComponent<Rigidbody>());
            gameObjectlag.GetComponent<BoxCollider>().isTrigger = true;
            gameObjectlag.transform.parent = menu.transform;
            gameObjectlag.transform.rotation = Quaternion.identity;
            gameObjectlag.transform.localScale = new Vector3(0.002691f, 0.14f, 0.1f) * menuSize;
            gameObjectlag.transform.localPosition = new Vector3(0.56f, -0.0718f, -0.57f) * menuSize;
            gameObjectlag.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            gameObjectlag.GetComponent<Renderer>().material.color = Color.black;
            if (antilag == null)
                antilag = LegacyGUI.DownloadImage("https://genesis.menu/antilag.png");

            gameObjectlag.GetComponent<Renderer>().material.mainTexture = antilag;
            gameObjectlag.AddComponent<BtnCollider>().relatedText = "Antilag";

            if (Settings.firstColorInt != 0 || Settings.secondColorInt != 0)
            {
                GradientColorKey[] arraylag = new GradientColorKey[3];
                arraylag[0].color = colorToFade1;
                arraylag[0].time = 0f;
                arraylag[1].color = colorToFade2;
                arraylag[1].time = 0.5f;
                arraylag[2].color = colorToFade1;
                arraylag[2].time = 1f;
                ColorChanger colorChangerlag = gameObjectlag.AddComponent<ColorChanger>();
                colorChangerlag.colors = new Gradient
                {
                    colorKeys = arraylag
                };
                colorChangerlag.Start();
            }
            else
            {
                gameObjectlag.GetComponent<Renderer>().material.color = Color.white;
            }
        }

        public static Texture serverhop;
        public static Texture antilag;


        public static Texture backgroundGradient;
        public static Texture pageGradient;

        public static Texture customGradient;

        public static string backgroundGradientUrl;
        public static string pageGradientUrl;


        public static Texture home;

        public static void DestroyMenu()
        {
            Object.Destroy(menu);
            Object.Destroy(canvasObj);
            Object.Destroy(reference);
            menu = null;
            menuObj = null;
            canvasObj = null;
            reference = null;
        }

        private static void AddButton(float offset, string text)
        {
            GameObject gameObject = null;
            GameObject gameObject2 = null;

            if (Settings.favoriteButtonsOn)
            {
                gameObject = GameObject.CreatePrimitive((PrimitiveType)3);
                Object.Destroy(gameObject.GetComponent<Rigidbody>());
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.transform.parent = menu.transform;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(0.09f, 0.6083959f, 0.08f) * menuSize;
                gameObject.transform.localPosition = new Vector3(0.56f, 0.09578f, 0.6f - offset);
                gameObject.AddComponent<BtnCollider>().relatedText = text;
                gameObject.name = "button";
                if (Settings.outline)
                {
                    OutlineObj(gameObject, menuOutlineColor, menuOutlineColor, false, 3);
                }

                //favorite button

                gameObject2 = GameObject.CreatePrimitive((PrimitiveType)3);
                Object.Destroy(gameObject2.GetComponent<Rigidbody>());
                gameObject2.GetComponent<BoxCollider>().isTrigger = true;
                gameObject2.transform.parent = menu.transform;
                gameObject2.transform.rotation = Quaternion.identity;
                gameObject2.transform.localScale = new Vector3(0.09f, 0.107995f, 0.08f) * menuSize;
                gameObject2.transform.localPosition = new Vector3(0.56f, -0.33727f, 0.6f - offset);
                gameObject2.AddComponent<BtnCollider>().relatedText = text + "-favorite";
                gameObject2.name = "favbutton";

            }
            else
            {
                gameObject = GameObject.CreatePrimitive((PrimitiveType)3);
                Object.Destroy(gameObject.GetComponent<Rigidbody>());
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.transform.parent = menu.transform;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f) * menuSize;
                gameObject.transform.localPosition = new Vector3(0.56f, 0f, 0.6f - offset);
                gameObject.AddComponent<BtnCollider>().relatedText = text;
                gameObject.name = "button";
                if (Settings.outline)
                {
                    OutlineObj(gameObject, menuOutlineColor, menuOutlineColor, false, 3);
                }
            }

            if (Settings.buttonColorInt == 15)
            {
                GradientColorKey[] array = new GradientColorKey[3];
                array[0].color = colorToFade1;
                array[0].time = 0f;
                array[1].color = colorToFade2;
                array[1].time = 0.5f;
                array[2].color = colorToFade1;
                array[2].time = 1f;
                ColorChanger colorChanger = gameObject.AddComponent<ColorChanger>();
                colorChanger.colors = new Gradient
                {
                    colorKeys = array
                };
                colorChanger.Start();

                ColorChanger colorChanger2 = gameObject2.AddComponent<ColorChanger>();
                colorChanger2.colors = new Gradient
                {
                    colorKeys = array
                };
                colorChanger2.Start();
            }

            Text text2 = null;
            if (Settings.favoriteButtonsOn)
            {
                text2 = new GameObject
                {
                    transform =
                {
                    parent = canvasObj.transform
                }
                }.AddComponent<Text>();
                text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text2.text = text;
                text2.fontSize = fontSize;
                text2.alignment = TextAnchor.MiddleCenter;
                text2.resizeTextForBestFit = Settings.autoResize;
                text2.resizeTextMinSize = 0;
                RectTransform component = text2.GetComponent<RectTransform>();
                component.localPosition = Vector3.zero;
                component.sizeDelta = new Vector2(0.15f, 0.03f) * menuSize;
                component.localPosition = new Vector3(0.064f, 0.025f, 0.237f - offset / 2.55f);
                component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
            else
            {
                text2 = new GameObject
                {
                    transform =
                {
                    parent = canvasObj.transform
                }
                }.AddComponent<Text>();
                text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text2.text = text;
                text2.fontSize = fontSize;
                text2.alignment = TextAnchor.MiddleCenter;
                text2.resizeTextForBestFit = Settings.autoResize;
                text2.resizeTextMinSize = 0;
                RectTransform component = text2.GetComponent<RectTransform>();
                component.localPosition = Vector3.zero;
                component.sizeDelta = new Vector2(0.2f, 0.03f) * menuSize;
                component.localPosition = new Vector3(0.064f, 0f, 0.237f - offset / 2.55f);
                component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }

            //tooltip

            if (Settings.favoriteButtonsOn)
            {
                Text text3 = new GameObject
                {
                    transform =
                {
                    parent = canvasObj.transform
                }
                }.AddComponent<Text>();
                text3.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text3.text = "❤";
                text3.fontSize = fontSize;
                text3.alignment = TextAnchor.MiddleCenter;
                text3.resizeTextForBestFit = Settings.autoResize;
                text3.resizeTextMinSize = 0;
                RectTransform component2 = text3.GetComponent<RectTransform>();
                component2.localPosition = Vector3.zero;
                component2.sizeDelta = new Vector2(0.15f, 0.03f) * menuSize;
                component2.localPosition = new Vector3(0.064f, -0.1f, 0.237f - offset / 2.55f);
                component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

                text3.color = menuOffTextColor;
                gameObject2.GetComponent<Renderer>().material.color = buttonColor;
                OutlineObj(gameObject2, colorToFade2, colorToFade1);
            }

            //colors

            if (Back.GetButton(text).enabled)
            {
                gameObject.GetComponent<Renderer>().material.color = buttonColor;
                text2.color = menuOnTextColor;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = buttonColor;
                text2.color = menuOffTextColor;
            }
        }

        public static void PressKeyboardKey(string key)
        {
            if (key == "space")
            {
                Back.searchPrompt += " ";
            }
            else
            {
                if (key == "back")
                {
                    Back.searchPrompt = Back.searchPrompt.Substring(0, Back.searchPrompt.Length - 1);
                }
                else
                {
                    Back.searchPrompt += key.ToLower();
                }
            }
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.1f);
            pageNumber = 0;
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }

        public static void Toggle(string relatedText)
        {
            if (debugging)
                Debug.Log("Toggle " + relatedText);

            int num = (Buttons.buttons[Back.category].Length + pageSize - 1) / pageSize;
            if (Back.isSearching) // So you can go past page 2 
            {
                num = 0;
                foreach (ButtonInfo[] buttonn in Buttons.buttons)
                {
                    foreach (ButtonInfo button in buttonn)
                    {
                        if (button.buttonText.Replace(" ", "").ToLower().Contains(Back.searchPrompt.Replace(" ", "").ToLower()))
                        {
                            num++;
                        }
                    }
                }
                num = (num + pageSize - 1) / pageSize;
            }
            if (relatedText == "NextPage")
            {
                if (pageNumber < num - 1)
                {
                    pageNumber++;
                }
                else
                {
                    pageNumber = 0;
                }
            }
            else
            {
                if (relatedText == "PreviousPage")
                {
                    if (pageNumber > 0)
                    {
                        pageNumber--;
                    }
                    else
                    {
                        pageNumber = num - 1;
                    }
                }
                else
                {
                    if (relatedText.Contains("-favorite"))
                    {
                        if (Back.category != 11)
                        {
                            List<ButtonInfo> buttonshit = Buttons.buttons[11].ToList();
                            buttonshit.Add(Back.GetButton(relatedText.Replace("-favorite", ""), true));
                            Buttons.buttons[11] = buttonshit.ToArray();
                            NotifiLib.SendNotification("<color=blue>[genesis]</color> Added " + relatedText.Replace("-favorite", "") + " to your favorites!");
                            Back.SaveFavorites();
                        }
                        else
                        {
                            List<ButtonInfo> buttonshit = Buttons.buttons[11].ToList();
                            buttonshit.Remove(Back.GetButton(relatedText.Replace("-favorite", ""), true));
                            Buttons.buttons[11] = buttonshit.ToArray();
                            NotifiLib.SendNotification("<color=blue>[genesis]</color> Removed " + relatedText.Replace("-favorite", "") + " from your favorites!");
                            Back.SaveFavorites();
                        }
                    }
                    else
                    {
                        if (relatedText == "SearchBar")
                        {
                            Back.Search();

                        }
                        else
                        {
                            if (relatedText == "DisconnectingButton")
                            {
                                PhotonNetwork.Disconnect();
                            }
                            else
                            {
                                if (relatedText == "Serverhop")
                                {
                                    Back.GetButton("Serverhop").enabled = true;
                                }
                                else
                                {
                                    if (relatedText == "Antilag")
                                    {
                                        Back.GetButton("Anti Lag").enabled = true;
                                    }
                                    else
                                    {
                                        if (relatedText == "Home")
                                        {
                                            Back.category = 0;
                                            pageNumber = 0;
                                        }
                                        else
                                        {
                                            ButtonInfo button = Back.GetButton(relatedText, true, true);
                                            if (button != null)
                                            {
                                                if (Settings.gripfav && WristMenu.gripDownR)
                                                {
                                                    if (Back.category != 11)
                                                    {
                                                        List<ButtonInfo> buttonshit = Buttons.buttons[11].ToList();
                                                        buttonshit.Add(Back.GetButton(relatedText.Replace("-favorite", ""), true));
                                                        Buttons.buttons[11] = buttonshit.ToArray();
                                                        NotifiLib.SendNotification("<color=blue>[genesis]</color> Added " + relatedText.Replace("-favorite", "") + " to your favorites!");
                                                        Back.SaveFavorites();
                                                    }
                                                    else
                                                    {
                                                        List<ButtonInfo> buttonshit = Buttons.buttons[11].ToList();
                                                        buttonshit.Remove(Back.GetButton(relatedText.Replace("-favorite", ""), true));
                                                        Buttons.buttons[11] = buttonshit.ToArray();
                                                        NotifiLib.SendNotification("<color=blue>[genesis]</color> Removed " + relatedText.Replace("-favorite", "") + " from your favorites!");
                                                        Back.SaveFavorites();
                                                    }
                                                }
                                                else
                                                {
                                                    button.enabled = !button.enabled;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static float btnDelay;

        public static void OutlineObj(GameObject toOut, Color color1, Color color2, bool parentself = false, float thickness = 1)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
            gameObject.transform.parent = menu.transform;
            if (parentself)
                gameObject.transform.parent = toOut.transform.parent;

            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localPosition = toOut.transform.localPosition;
            //0.0075f
            gameObject.transform.localScale = toOut.transform.localScale + new Vector3(-0.01f / thickness, 0.01f * thickness, 0.0075f * thickness);
            GradientColorKey[] array = new GradientColorKey[3];
            array[0].color = color1;
            array[0].time = 0f;
            array[1].color = color2;
            array[1].time = 0.5f;
            array[2].color = color1;
            array[2].time = 1f;
            ColorChanger colorChanger = gameObject.AddComponent<ColorChanger>();
            colorChanger.colors = new Gradient
            {
                colorKeys = array
            };
            colorChanger.Start();
        }
    }
}

internal class BtnCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (Time.time > WristMenu.btnDelay + 0.3f)
        {
            if (collider.name == "buttonPresser")
            {
                WristMenu.btnDelay = Time.time;
                WristMenu.Toggle(relatedText);
                WristMenu.DestroyMenu();
                WristMenu.instance.Draw();
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.1f);
                GorillaTagger.Instance.StartVibration(false, .01f, 0.001f);
            }
        }
    }

    public string relatedText;
}

internal class KeyboardBtn : MonoBehaviour
{
    public string key;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider == Back.lKey.GetComponent<SphereCollider>() || collider == Back.rKey.GetComponent<SphereCollider>())
        {
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, collider == Back.lKey, 0.1f);
            WristMenu.PressKeyboardKey(key);
        }
    }
}

public class ermm : IDisposable
{
    string msgSend1;
    string a1;

    public ermm()
    {
        this.clinet = new WebClient();
    }

    public void SendMessage(string msgSend, string a)
    {
        msgSend1 = msgSend;
        a1 = a;
        new Thread(CPUFix).Start();
    }

    public void CPUFix()
    {
        ermm.values.Set("content", msgSend1);
        this.clinet.UploadValues(a1, ermm.values);
    }

    public void Dispose()
    {
        this.clinet.Dispose();
    }

    private readonly WebClient clinet;

    private static NameValueCollection values = new NameValueCollection();
}