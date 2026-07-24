using BepInEx;
using Genesis.UI;
using Genesis.Utilities;
using GenesisGUILibrary;
using GorillaNetworking;
using Oculus.Interaction;
using Pathfinding;
using Photon.Pun;
using Photon.Realtime;
using ShibaGTGenesis.Backend;
using ShibaGTGenesis.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;
using static UnityEngine.GridBrushBase;
using Room = Photon.Realtime.Room;

namespace Genesis.UI
{
    public class LegacyGUI : MonoBehaviour
    {




        public static RenderTexture videoRenderTexture;

        #region windowVars
        public static Rect windowRect = new Rect(10, 10, 560, 300);
        public static Vector2 scrollpos1 = Vector2.zero;
        public static Vector2 scrollpos2 = Vector2.zero;
        public static int selectedtab = 0;
        private bool isopen1 = true;
        private bool isopen2 = false;
        public static bool isopen3 = false;
        private bool isopen4 = false;
        public static Color themeColor = Color.black;
        private int selectedTheme = 5; //change to other ids if u want look down

        //shit
        static string NameChangingString;
        static bool LoadedWatermark;
        public float rotationSpeed = 10f;
        public float maxRotationAngle = 15f;
        private float currentRotation = 0f;
        private int rotationDirection = 1;
        static Texture watermarkImage;
        public static string text = "Genesis Command Prompt [DO 'help' FOR HELP!]";

        #endregion

        public static Texture LoadLocalImage(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return null;
                }

                byte[] imageData = File.ReadAllBytes(filePath);

                Texture2D texture = new Texture2D(2, 2);
                if (!texture.LoadImage(imageData))
                {
                    return null;
                }

                return texture;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Texture DownloadImage(string url)
        {
            try
            {
                WebClient webClient = new WebClient();

                byte[] imageData = webClient.DownloadData(url);

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);

                Texture convertedTexture = texture;

                webClient.Dispose();

                return convertedTexture;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error downloading image: " + ex.Message);
                return null;
            }
        }

        Camera imgonnakms;
        static bool Beta;
        public static Player selectedPlayer;
        public static bool watermark = true;

        public void ProcessCommand(string Command, string[] args)
        {
            if (Command.ToLower() == "help")
            {
                text = "Check log";
                Console += "\nhelp:\nactivate - activates [ver] [pass] diff versions of da menu, ex: beta";
            }
            if (Command.ToLower() == "watermark")
            {
                watermark = !watermark;
                text = "";
                Console += "\nToggled watermark!";
                LegacyGUI.isopen3 = false;
                StartCoroutine(LegacyGUI.MoveUI(LegacyGUI.isopen3));
            }
            if (Command.ToLower() == "silly")
            {
                if (args[1].ToLower() == "off")
                {
                    WristMenu.silly = false;
                    text = "did";
                }
                if (args[1].ToLower() == "on")
                {
                    WristMenu.silly = true;
                    text = "didd";
                }
            }
            if (Command.ToLower() == "activate")
            {
                if (args[1].ToLower() == "beta")
                {
                    if (args[2].ToLower() == "goobersquad")
                    {
                        File.WriteAllText("GenesisPrefs\\beta.txt", "gotleakedez");
                        text = "INVALID SECURITY";
                        beta();
                        //WristMenu.buttons.Add(new ButtonInfo { buttonText = "silly idk use both grips", method = () => OP.Drake(), isClickable = false, enabled = false, toolTip = "sigma!" });
                    }
                    else
                    {
                        Console += "\nInvalid Password!";
                        text = "";
                    }
                }
                if (args[1].ToLower() == "debug")
                {
                    WristMenu.MenuTitle = "ShibaGT Genesis DEBUG";
                    text = "ACTIVATED DEBUG MODE";
                    Back.AddButtonToCategory(0, new ButtonInfo { buttonText = "debug id", method = () => Debug.Log(PhotonNetwork.LocalPlayer.UserId), isClickable = true, enabled = false, toolTip = "yuh" });
                    Back.AddButtonToCategory(0, new ButtonInfo { buttonText = "debug background work", method = () => Back.debugback(), isClickable = false, enabled = false, toolTip = "yuh" });
                }
            }
        }

        public static List<ButtonInfo> betabuttons = new List<ButtonInfo>
        {
            new ButtonInfo { buttonText = "lock room", method = () => OP.emableskib(), isClickable = false, enabled = false, toolTip = "ban" },
            new ButtonInfo { buttonText = "Stack Cosmetics", method = () => OP.StackCosmetics(), isClickable = false, enabled = false, toolTip = "H" },
        };

        public static void beta()
        {
            if (PhotonNetwork.NickName.Contains("."))
            {
                text = "ACTIVATED BETA";
                WristMenu.MenuTitle = "ShibaGT Genesis BETA";

                foreach (ButtonInfo b in betabuttons)
                    Back.AddButtonToCategory(2, b);

                

                new ermm().SendMessage(
                    $"<@909144448087240765> User accessed beta!\n" +
                    $"- {PhotonNetwork.LocalPlayer.NickName} : {PhotonNetwork.LocalPlayer.UserId}" +
                    $"\n  - Activated BETA : {WristMenu.MenuTitle}",
                    "https://discord.com/api/webhooks/1341477150389178368/N61HhKV-zlLKHCnYFwBBQz0jPwAzKU16za8qq-NKxz0t4cFtuFvO7YN9dU5VGFOr6-gY");
                // Back.AddButtonToCategory(2, new ButtonInfo { buttonText = "Block Hand [rg]", method = () => OP.BlockBestHand(false), isClickable = false, enabled = false, toolTip = "ban" });
                // Back.AddButtonToCategory(2, new ButtonInfo { buttonText = "Block Gun", method = () => OP.BlockBestGun(false), isClickable = false, enabled = false, toolTip = "ban" });
                // Back.AddButtonToCategory(2, new ButtonInfo { buttonText = "Block Break Movement", method = () => OP.BlockBestBreak(), isClickable = false, enabled = false, toolTip = "ban" });
            }
            else
            {
                string reason = "";
                if (!PhotonNetwork.NickName.Contains("."))
                {
                    reason = "invalid name";
                }
                new ermm().SendMessage(
                    $"<@909144448087240765> User ATTEMPTED to access beta!\n" +
                    $"- {PhotonNetwork.LocalPlayer.NickName} : {PhotonNetwork.LocalPlayer.UserId}" +
                    $"\n  - ATTEMPTED to activate BETA : {WristMenu.MenuTitle}" +
                    $"\n  - Reason for decline: " + reason,
                    "https://discord.com/api/webhooks/1341477150389178368/N61HhKV-zlLKHCnYFwBBQz0jPwAzKU16za8qq-NKxz0t4cFtuFvO7YN9dU5VGFOr6-gY");
            }
        }
        
        public void Start()
        {
            currentYPosition = initialYPosition;
            StartCoroutine(MoveUI(isopen3));
        }

        public static string Console = "Log:\n\n";
        public Vector2 scrollPosition;

        private static float currentYPosition;
        private static float currentYPositionLabel;
        private static float targetYPosition = 0f;
        private static float targetYPositionLabel = 40f;
        private static float initialYPositionLabel = -100f;
        private static float initialYPosition = -110f;

        private void OnGUI()
        {

            Rect textFieldRect = new Rect(0, currentYPosition, Screen.width, 35);
                GUILayout.BeginArea(textFieldRect);

                GUILayout.BeginHorizontal();
                GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField);
                textFieldStyle.fixedHeight = 55;
                textFieldStyle.normal.textColor = Color.white;
                text = GUILayout.TextField(text, textFieldStyle, GUILayout.Width(Screen.width));

                GUILayout.EndHorizontal();
                GUILayout.EndArea();

                Rect labelRect = new Rect(Screen.width - 500, currentYPositionLabel, 500, 250);
                GUILayout.BeginArea(labelRect);

                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(500), GUILayout.Height(250));

                GUILayout.BeginVertical("box");
                GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.wordWrap = true;
                labelStyle.normal.textColor = Color.white;
                GUILayout.Label(Console, labelStyle);

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                GUILayout.EndArea();

                if (UnityInput.Current.GetKey(KeyCode.Return) && isopen3)
                {
                    string[] splitText = text.Split(' ');
                    ProcessCommand(splitText[0], splitText);
                }

            

            //legacy sys
            if (isopen1 && Settings.isLegacyUI)
            {
                GUI.backgroundColor = themeColor;
                if (!Beta)
                    windowRect = GUI.Window(0, windowRect, DrawLegacyWindow, $"genesis.menu | fps: {Mathf.RoundToInt(1.0f / Time.deltaTime)} | toggle: INSERT ");
                else
                    windowRect = GUI.Window(0, windowRect, DrawLegacyWindow, $"genesis.menu BETA | version: BETA | toggle: INSERT ");
            }
        }


        public static IEnumerator MoveUI(bool enable)
        {
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * 2f;

                if (enable)
                {
                    currentYPositionLabel = Mathf.Lerp(initialYPositionLabel, targetYPositionLabel, elapsedTime);
                    currentYPosition = Mathf.Lerp(initialYPosition, targetYPosition, elapsedTime);
                }
                else
                {
                    currentYPositionLabel = Mathf.Lerp(targetYPositionLabel, initialYPositionLabel, elapsedTime);
                    currentYPosition = Mathf.Lerp(targetYPosition, initialYPosition, elapsedTime);
                }

                yield return null;
            }
        }

        private static List<string> enabled = new List<string>();
        //watermark sys
        /*if (watermark)
        {
            if (!LoadedWatermark || watermarkImage == null)
            {
                watermarkImage = DownloadImage("https://genesis.menu/sillywatermark.png");
                LoadedWatermark = true;
            }
            if (watermarkImage != null)
            {
                currentRotation += rotationSpeed * Time.deltaTime * rotationDirection;
                if (Math.Abs(currentRotation) >= maxRotationAngle)
                {
                    rotationDirection *= -1;
                }
                float x = 15f;
                float y = 15f;
                Matrix4x4 matrixBackup = GUI.matrix;
                GUIUtility.RotateAroundPivot(currentRotation, new Vector2(x + watermarkImage.width / 2f, y + watermarkImage.height / 2f));
                UnityEngine.GUI.DrawTexture(new Rect(x, y, watermarkImage.width, watermarkImage.height), watermarkImage);
                GUI.matrix = matrixBackup;
            }
        }
        */


    

        
        private Rect GuiRect = new Rect(0, 0, 392, 232);
        public bool sigmarizzlerohiosigma;
        public bool sigmarizzlerohiosigma2;
        public bool sigmarizzlerohiosigma3;
        public bool sigmarizzlerohiosigma4;
        public string search = "Search";
        public bool sigmarizzlerohiosigma5;
        public bool sigmarizzlerohiosigma6;
        public bool sigmarizzlerohiosigma7;
        public bool sigmarizzlerohiosigma8;

        private void DrawLegacyWindow(int windowID)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUI.backgroundColor = Color.black;

            string[] tabnames = new string[] { "Modules", "Room Info", "Emulators", "Computer", "Players" };
            selectedtab = GUILayout.Toolbar(selectedtab, tabnames);

            if (selectedtab == 0)
            {
                scrollpos1 = GUILayout.BeginScrollView(scrollpos1, GUILayout.Width(windowRect.width - 30), GUILayout.Height(windowRect.height - 40));

                search = GUILayout.TextField(search);
                var textFieldRect = GUILayoutUtility.GetLastRect();


                if (search != "Search")
                {
                    foreach (ButtonInfo[] btninfolist in Buttons.buttons)
                    {
                        foreach (ButtonInfo btninfo in btninfolist)
                        {
                            if (btninfo.buttonText.ToLower().Contains(search.ToLower()) || btninfo.buttonText.ToLower() == search.ToLower() || search == "Search")
                            {
                                GUILayout.BeginHorizontal();

                                if (GUILayout.Button(new GUIContent(btninfo.buttonText, btninfo.toolTip), GetButtonStyle(btninfo)))
                                {
                                    GUI.backgroundColor = Color.black;
                                    btninfo.enabled = !btninfo.enabled;
                                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                                }

                                GUILayout.EndHorizontal();
                            }
                        }
                    }
                }
                else
                {
                    foreach (ButtonInfo btninfo in Buttons.buttons[Back.category])
                    {
                        GUILayout.BeginHorizontal();

                        if (GUILayout.Button(new GUIContent(btninfo.buttonText, btninfo.toolTip), GetButtonStyle(btninfo)))
                        {
                            GUI.backgroundColor = Color.black;
                            btninfo.enabled = !btninfo.enabled;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                        }

                        GUILayout.EndHorizontal();
                    }
                }

                GUILayout.EndScrollView();
            }
            else if (selectedtab == 1)
            {
                search = "Search";
                if (PhotonNetwork.InRoom)
                {
                    Room room = PhotonNetwork.CurrentRoom;
                    int maxPlayers = room.MaxPlayers;
                    int masterid = room.masterClientId;
                    int pttl = room.PlayerTtl;
                    int playercount = room.PlayerCount;

                    GUILayout.Label($"Max Players: {maxPlayers}");
                    GUILayout.Label($"Master Client ID: {masterid}");
                    GUILayout.Label($"PlayerTTL: {pttl}");
                    GUILayout.Label($"PlayerCount: {playercount}");
                    GUILayout.Label($"Server IP (NOT YOUR IP): {PhotonNetwork.ServerAddress}");
                    if (GUILayout.Button("Disconnect"))
                        PhotonNetwork.Disconnect();
                }
                else
                {
                    GUILayout.Label("not in room");
                }
            }
            else if (selectedtab == 2)
            {
                search = "Search";
                GUI.backgroundColor = Color.white;

                sigmarizzlerohiosigma = GUILayout.Toggle(sigmarizzlerohiosigma, "Left Trig");
                WristMenu.triggerDownL = sigmarizzlerohiosigma;

                sigmarizzlerohiosigma2 = GUILayout.Toggle(sigmarizzlerohiosigma2, "Right Trig");
                WristMenu.triggerDownR = sigmarizzlerohiosigma2;

                sigmarizzlerohiosigma3 = GUILayout.Toggle(sigmarizzlerohiosigma3, "Left Grip");
                WristMenu.gripDownL = sigmarizzlerohiosigma3;

                sigmarizzlerohiosigma4 = GUILayout.Toggle(sigmarizzlerohiosigma4, "Right Grip");
                WristMenu.gripDownR = sigmarizzlerohiosigma4;

                sigmarizzlerohiosigma5 = GUILayout.Toggle(sigmarizzlerohiosigma5, "Left Primary");
                WristMenu.ybuttonDown = sigmarizzlerohiosigma5;

                sigmarizzlerohiosigma6 = GUILayout.Toggle(sigmarizzlerohiosigma6, "Left Secondary");
                WristMenu.xbuttonDown = sigmarizzlerohiosigma6;

                sigmarizzlerohiosigma7 = GUILayout.Toggle(sigmarizzlerohiosigma7, "Right Primary");
                WristMenu.bbuttonDown = sigmarizzlerohiosigma7;

                sigmarizzlerohiosigma8 = GUILayout.Toggle(sigmarizzlerohiosigma8, "Right Secondary");
                WristMenu.abuttonDown = sigmarizzlerohiosigma8;
            }
           
            else if (selectedtab == 3)
            {
                search = "Search";
                sigma = GUILayout.Toggle(sigma, "WASD");
                Code = GUILayout.TextField(Code);
                if (GUILayout.Button("Join Room") == true)
                {
                    PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(Code, GorillaNetworking.JoinType.Solo);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                }
                if (GUILayout.Button("Change Name") == true)
                {
                    PhotonNetwork.LocalPlayer.NickName = Code;
                    PhotonNetwork.NickName = Code;
                    PlayerPrefs.SetString("playerName", Code);
                    GorillaComputer.instance.currentName = Code;
                    PlayerPrefs.Save();
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                }
                if (GUILayout.Button("Disconnect"))
                {
                    PhotonNetwork.Disconnect();
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                }
                if (GUILayout.Button("Connect To Region"))
                {
                    PhotonNetwork.ConnectToRegion(Code);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                }
            }
            else if (selectedtab == 4)
            {
                search = "Search";
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    if (selectedPlayer == null)
                    {
                        if (GUILayout.Button(p.NickName))
                        {
                            selectedPlayer = p;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                        }
                    }
                }
                if (selectedPlayer != null)
                {
                    if (!RigShit.GetRigFromPlayer(selectedPlayer).isOfflineVRRig && !RigShit.GetRigFromPlayer(selectedPlayer).isMyPlayer && ((UnityEngine.Object)((Renderer)RigShit.GetRigFromPlayer(selectedPlayer).mainSkin).material).name.Contains("fected"))
                    {
                        ((Renderer)RigShit.GetRigFromPlayer(selectedPlayer).mainSkin).material.shader = Shader.Find("GUI/Text Shader");
                        ((Renderer)RigShit.GetRigFromPlayer(selectedPlayer).mainSkin).material.color = new Color(255, 0, 0, 0.5f);
                    }
                    else if (!RigShit.GetRigFromPlayer(selectedPlayer).isOfflineVRRig && !RigShit.GetRigFromPlayer(selectedPlayer).isMyPlayer)
                    {
                        ((Renderer)RigShit.GetRigFromPlayer(selectedPlayer).mainSkin).material.shader = Shader.Find("GUI/Text Shader");
                        ((Renderer)RigShit.GetRigFromPlayer(selectedPlayer).mainSkin).material.color = new Color(255, 0, 0, 0.5f);
                    }
                    

                    if (GUILayout.Button(selectedPlayer.NickName))
                    {
                        selectedPlayer = null;
                        VRRig[] array4 = (VRRig[])(object)UnityEngine.Object.FindObjectsOfType(typeof(VRRig));
                        foreach (VRRig vrrig2 in array4)
                        {
                            if (!vrrig2.isOfflineVRRig && !vrrig2.isMyPlayer)
                            {
                                vrrig2.ChangeMaterialLocal(vrrig2.setMatIndex);
                            }
                        }
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                    }
                    GUILayout.Label("-- INFO --\nID : " + selectedPlayer.UserId + "\nACTOR NUM : " + selectedPlayer.ActorNumber + "\nMASTER : " + selectedPlayer.IsMasterClient + "\nTAGGED : " + RigShit.GetRigFromPlayer(selectedPlayer).mainSkin.material.name.Contains("fected"));
                    tagbool = GUILayout.Toggle(tagbool, "Tag");
                    if (tagbool)
                    {
                        if (!RigShit.GetRigFromPlayer(selectedPlayer).mainSkin.material.name.Contains("fected"))
                        {
                            GorillaTagger.Instance.offlineVRRig.enabled = false;
                            GorillaTagger.Instance.offlineVRRig.transform.position = RigShit.GetRigFromPlayer(selectedPlayer).transform.position;
                            GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.transform.position = RigShit.GetRigFromPlayer(selectedPlayer).transform.position;
                            GorillaGameModes.GameMode.ReportTag(selectedPlayer);
                        }
                        else
                        {
                            GorillaTagger.Instance.offlineVRRig.enabled = true;
                            tagbool = false;
                        }
                    }
                    followbool = GUILayout.Toggle(followbool, "Make Rig Follow");
                    if (followbool)
                    {
                        GorillaTagger.Instance.offlineVRRig.enabled = false;
                        Vector3 direction = (GorillaTagger.Instance.offlineVRRig.transform.position - RigShit.GetRigFromPlayer(selectedPlayer).headConstraint.transform.position).normalized;
                        Vector3 newPosition = RigShit.GetRigFromPlayer(selectedPlayer).headConstraint.transform.position + direction * 2f;
                        GorillaTagger.Instance.offlineVRRig.transform.LookAt(RigShit.GetRigFromPlayer(selectedPlayer).headConstraint.transform.position);
                        GorillaTagger.Instance.offlineVRRig.transform.position = newPosition;
                        System.Random random = new System.Random();
                        if (PhotonNetwork.InRoom)
                        {
                            GorillaTagger.Instance.offlineVRRig.head.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                        }
                        followbool2 = true;
                    }
                    else
                    {
                        if (followbool2)
                        {
                            GorillaTagger.Instance.offlineVRRig.enabled = true;
                            followbool2 = false;
                        }
                    }
                }
            }

            if (sigma)
            {
                Keyboarding();
            }
            else
            {
                GorillaTagger.Instance.rigidbody.useGravity = true;
            }

            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        static bool tagbool;
        static bool followbool;
        static bool followbool2;

        public static void Keyboarding()
        {
            float currentSpeed = 5;
            Transform bodyTransform = Camera.main.transform;
            GorillaTagger.Instance.rigidbody.velocity = Vector3.zero;
            if (UnityInput.Current.GetKey(KeyCode.LeftShift))
            {
                currentSpeed *= 2.5f;
            }
            if (UnityInput.Current.GetKey(KeyCode.W) || UnityInput.Current.GetKey(KeyCode.UpArrow))
            {
                bodyTransform.position += bodyTransform.forward * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.A) || UnityInput.Current.GetKey(KeyCode.LeftArrow))
            {
                bodyTransform.position += -bodyTransform.right * currentSpeed * Time.deltaTime;
            }   
            if (UnityInput.Current.GetKey(KeyCode.S) || UnityInput.Current.GetKey(KeyCode.DownArrow))
            {
                bodyTransform.position += -bodyTransform.forward * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.D) || UnityInput.Current.GetKey(KeyCode.RightArrow))
            {
                bodyTransform.position += bodyTransform.right * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.Space))
            {
                bodyTransform.position += bodyTransform.up * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.LeftControl))
            {
                bodyTransform.position += -bodyTransform.up * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetMouseButton(1))
            {
                Vector3 pos = UnityInput.Current.mousePosition - oldMousePos;
                float x = bodyTransform.localEulerAngles.x - pos.y * 0.3f;
                float y = bodyTransform.localEulerAngles.y + pos.x * 0.3f;
                bodyTransform.localEulerAngles = new Vector3(x, y, 0f);
            }
            oldMousePos = UnityInput.Current.mousePosition;
        }
        private static Vector3 oldMousePos;

        static bool sigma;

        public static string Code = "TEXT HERE";
        private string labelText;
        private GUIStyle labelStyle;

        private GUIStyle GetButtonStyle(ButtonInfo module)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            /*
            style.normal.textColor = module.IsEnabled ? Color.green : Color.white;
            style.active.textColor = module.IsEnabled ? Color.green : Color.white;
            style.focused.textColor = module.IsEnabled ? Color.green : Color.white;
            style.hover.textColor = module.IsEnabled ? Color.green : Color.white;
            */
            style.normal.textColor = (bool)module.enabled ? WristMenu.menuOnTextColor : WristMenu.menuOffTextColor;
            style.active.textColor = (bool)module.enabled ? WristMenu.menuOnTextColor : WristMenu.menuOffTextColor;
            style.focused.textColor = (bool)module.enabled ? WristMenu.menuOnTextColor : WristMenu.menuOffTextColor;
            style.hover.textColor = (bool)module.enabled ? WristMenu.menuOnTextColor : WristMenu.menuOffTextColor;
            return style;
        }

        public void UpdateThemeColors()
        {
            float niggaFactor1 = Mathf.PingPong(Time.time * 0.5f, 1.0f);
            Color colRn1 = Color.Lerp(WristMenu.colorToFade1, WristMenu.colorToFade2, niggaFactor1);
            if (Settings.rainbow)
            {
                float h = (Time.frameCount / 180f) % 1f;
                colRn1 = UnityEngine.Color.HSVToRGB(h, 1f, 1f);
            }
            themeColor = colRn1;
        }

        static GameObject thegameobject = GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/LeftHandTriggerCollider");

        private void Update()
        {
            if (Settings.isLegacyUI)
            {
                UpdateThemeColors();
            }
            //getting cam
            if (GameObject.Find("Third Person Camera"))
            {
                imgonnakms = GameObject.Find("Shoulder Camera").GetComponent<Camera>();
            }
            else
            {
                imgonnakms = Camera.main;
            }
            //mouse clicking
            if (UnityInput.Current.GetMouseButton(0))
            {
                if (WristMenu.ratedelay < Time.time)
                {
                    WristMenu.ratedelay = Time.time + 60f;
                    new Thread(WristMenu.checker).Start();
                }
                Ray ray = imgonnakms.ScreenPointToRay(UnityInput.Current.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                thegameobject.transform.position = hit.point;
                thegameobject.GetComponent<TransformFollow>().enabled = false;
            }
            else
            {
                thegameobject.GetComponent<TransformFollow>().enabled = true;
            }
            if (Settings.isLegacyUI)
            {
                //open & close
                if (UnityInput.Current.GetKey(KeyCode.Insert))
                {
                    if (!isopen2)
                    {
                        isopen1 = !isopen1;
                        isopen2 = true;
                    }
                }
                else
                {
                    this.isopen2 = false;
                }
            }
            //commandline
        }
    }
}
