using ExitGames.Client.Photon;
using Genesis.UI;
using Genesis.Utilities;
using GorillaNetworking;
using GTAG_NotificationLib;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace ShibaGTGenesis.Backend
{
    internal class Back : MonoBehaviour
    {
        public static void debugging()
        {
            foreach (var c in CosmeticsController.instance.allCosmetics)
            {
                Debug.Log($"Cosmetic found : {c.overrideDisplayName} : {c.itemName}");
            }
        }

        public static void debugmods()
        {
            WristMenu.debuggingMods = true;
        }

        public static void offdebugmods()
        {
            WristMenu.debuggingMods = false;
        }

        public static void debugback()
        {
            WristMenu.debugging = true;
            WristMenu.debuggingMods = true;
        }

        public static int category = 0;

        public static bool AntibanMainBool;
        public static bool AntibanNotifBool;
        public static bool AntibanJoinNotifBool;
        public static bool AntibanDoneBool;
        public static bool AntibanBool;
        public static bool AutoMasterBool;
        public static string LastJoinedRoom;
        public static float AntibanJoinDelay;
        public static float AntibanDelay;

        public static GameObject pointer;
        public static RaycastHit raycastHit;
        public static VRRig lockedrig;

        public static string[] AmongUs2 = new string[]
        {
            @"
            ⠀⠀⠀⠀⠀⢀⣴⡾⠿⠿⠿⠿⢶⣦⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
            ⠀⠀⠀⠀⢠⣿⠁⠀⠀⠀⣀⣀⣀⣈⣻⣷⡄⠀⠀⠀⠀⠀⠀⠀⠀
            ⠀⠀⠀⠀⣾⡇⠀⠀⣾⣟⠛⠋⠉⠉⠙⠛⢷⣄⠀⠀⠀⠀⠀⠀⠀
            ⢀⣤⣴⣶⣿⠀⠀⢸⣿⣿⣧⠀⠀⠀⠀⢀⣀⢹⡆⠀⠀⠀⠀⠀⠀
            ⢸⡏⠀⢸⣿⠀⠀⠀⢿⣿⣿⣷⣶⣶⣿⣿⣿⣿⠃⠀⠀⠀⠀⠀⠀
            ⣼⡇⠀⢸⣿⠀⠀⠀⠈⠻⠿⣿⣿⠿⠿⠛⢻⡇⠀⠀⠀⠀⠀⠀⠀
            ⣿⡇⠀⢸⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣤⣼⣷⣶⣶⣶⣤⡀⠀⠀
            ⣿⡇⠀⢸⣿⠀⠀⠀⠀⠀⠀⣀⣴⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⡀
            ⢻⡇⠀⢸⣿⠀⠀⠀⠀⢀⣾⣿⣿⣿⣿⣿⣿⣿⡿⠿⣿⣿⣿⣿⡇
            ⠈⠻⠷⠾⣿⠀⠀⠀⠀⣾⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⢸⣿⣿⣿⣇
            ⠀⠀⠀⠀⣿⠀⠀⠀⠀⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃⠀⢸⣿⣿⣿⡿
            ⠀⠀⠀⠀⢿⣧⣀⣠⣴⡿⠙⠛⠿⠿⠿⠿⠉⠀⠀⢠⣿⣿⣿⣿⠇
            ⠀⠀⠀⠀⠀⢈⣩⣭⣥⣤⣤⣤⣤⣤⣤⣤⣤⣤⣶⣿⣿⣿⣿⠏⠀
            ⠀⠀⠀⠀⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠋⠀⠀
            ⠀⠀⠀⢸⣿⣿⣿⡟⠛⠛⠛⠛⠛⠛⠛⠛⠛⠛⠛⠋⠁⠀⠀⠀⠀
            ⠀⠀⠀⢸⣿⣿⣿⣷⣄⣀⣀⣀⣀⣀⣀⣀⣀⣀⡀⠀⠀⠀⠀⠀⠀
            ⠀⠀⠀⠀⠻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣦⡀⠀⠀⠀
            ⠀⠀⠀⠀⠀⠈⠛⠿⠿⣿⣿⣿⣿⣿⠿⠿⢿⣿⣿⣿⣿⣿⡄⠀⠀
            ⠀⠀⠀⠀⠀⠀⢀⣀⣀⣀⡀⠀⠀⠀⠀⠀⠀⢀⣹⣿⣿⣿⡇⠀⠀
            ⠀⠀⠀⠀⠀⢰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠁⠀⠀
            ⠀⠀⠀⠀⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠛⠁⠀⠀⠀
            ⠀⠀⠀⠀⣿⣿⣿⣿⠁⠀⠀⠀⠀⠀⠉⠉⠁⢤⣤⣤⣤⣤⣤⣤⡀
            ⠀⠀⠀⠀⢿⣿⣿⣿⣷⣶⣶⣶⣶⣾⣿⣿⣿⣆⢻⣿⣿⣿⣿⣿⡇
            ⠀⠀⠀⠀⠈⠻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⠻⣿⣿⣿⡿⠁
            ⠀⠀⠀⠀⠀⠀⠈⠙⠛⠛⠛⠛⠛⠛⠛⠛⠛⠛⠉⠀⠙⠛⠉⠀⠀
            "
        };



        //search sys

        public static bool isSearching;

        public static string searchPrompt = "";

        public static GameObject keyboard;

        public static Transform menuSpawnPos;

        public static GameObject lKey;
        public static GameObject rKey;

        public static GameObject shiba;

        public static void AddOwn()
        {
            NotifiLib.SendNotification("check your pc screen");
            File.WriteAllText("howtoaddplugins.txt", "Find plugins inside of the Plugin Library (mod in the category) in the discord server\nNOTE: DO NOT USE THE COMMUNITY PLUGINS IF YOU DO NOT TRUST THE CREATORS!\nCOMMUNITY PLUGINS COULD BE **RATS**!!!");
            System.Diagnostics.Process.Start("howtoaddplugins.txt");
            //File.Delete("ez.txt");
        }

        public static void OnEnablePlugin(string dllName)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllName);
                Debug.Log($"Loaded assembly: {assembly.FullName}");

                var types = assembly.GetTypes();
                Debug.Log($"Found {types.Length} types in assembly: {assembly.FullName}");

                foreach (var type in types)
                {
                    Debug.Log($"Checking type: {type.FullName}");

                    var method = type.GetMethod("onEnable", BindingFlags.Public | BindingFlags.Static);
                    if (method != null)
                    {
                        Debug.Log($"Found 'onEnable' method in type: {type.FullName}");

                        method.Invoke(null, null);
                        Debug.Log($"Successfully invoked 'onEnable' in type: {type.FullName}");
                    }
                    else
                    {
                        Debug.Log($"No 'onEnable' method found in type: {type.FullName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error processing DLL '{dllName}': {ex.Message}");
            }
        }

        public static void OnDisablePlugin(string dllName)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllName);
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    var method = type.GetMethod("onDisable", BindingFlags.Public | BindingFlags.Static);
                    if (method != null)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error processing DLL '{dllName}': {ex.Message}");
            }
        }

        public static void UpdatePlugin(string dllName)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllName);

                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    var method = type.GetMethod("update", BindingFlags.Public | BindingFlags.Static);
                    if (method != null)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error processing DLL '{dllName}': {ex.Message}");
            }
        }

        public static void LoadAndEnablePlugins()
        {
            string folderPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "GenesisPrefs\\genesisPlugins");
            if (!Directory.Exists(folderPath))
            {
                Debug.LogError($"Folder '{folderPath}' does not exist. No plugins to load.");
                return;
            }

            var dllFiles = Directory.GetFiles(folderPath, "*.dll");

            var e = Buttons.buttons[15].ToList();
            e.Clear();
            Buttons.buttons[15] = e.ToArray();
            AddButtonToCategory(15, new ButtonInfo { buttonText = "Custom Plugins", method = () => ShibaGTGenesis.Backend.Back.Plugins(), isClickable = true, enabled = false, toolTip = "Go to enabled mods!" });
            AddButtonToCategory(15, new ButtonInfo { buttonText = "How To Add Plugins", method = () => ShibaGTGenesis.Backend.Back.AddOwn(), isClickable = true, enabled = false, toolTip = "loads ur sounds!" });
            AddButtonToCategory(15, new ButtonInfo { buttonText = "Plugin Library", method = () => ShibaGTGenesis.Backend.Back.PluginLibrary(), isClickable = true, enabled = false, toolTip = "download ur sounds!" });
            AddButtonToCategory(15, new ButtonInfo { buttonText = "Load Plugins", method = () => ShibaGTGenesis.Backend.Back.LoadAndEnablePlugins(), isClickable = true, enabled = false, toolTip = "loads ur plugins!" });

            foreach (var dllFile in dllFiles)
            {
                Debug.Log(dllFile);
                try
                {
                    Back.AddButtonToCategory(15, new ButtonInfo { buttonText = Path.GetFileName(dllFile), oneMethod = () => OnEnablePlugin(dllFile), oneDisableMethod = () => OnDisablePlugin(dllFile), method = () => UpdatePlugin(dllFile) });
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error adding Plugin : '{dllFile}': {ex.Message}");
                }
            }
        }

        public static void Search()
        {
            isSearching = !isSearching;
            searchPrompt = "";

            if (isSearching)
            {
                if (keyboard == null)
                {
                    keyboard = WristMenu.LoadAssetBundle("keyboard");
                    keyboard.transform.position = GorillaTagger.Instance.bodyCollider.transform.position;
                    keyboard.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

                    menuSpawnPos = keyboard.transform.Find("MenuSpawnPosition").transform;

                    foreach (Transform trans in keyboard.transform.Find("fard").GetComponentInChildren<Transform>())
                    {
                        try
                        {
                            if (trans.name != "Canvas")
                            {
                                GradientColorKey[] array = new GradientColorKey[3];
                                array[0].color = WristMenu.colorToFade1;
                                array[0].time = 0f;
                                array[1].color = WristMenu.colorToFade2;
                                array[1].time = 0.5f;
                                array[2].color = WristMenu.colorToFade1;
                                array[2].time = 1f;
                                Genesis.Utilities.ColorChanger colorChanger = trans.AddComponent<Genesis.Utilities.ColorChanger>();
                                colorChanger.colors = new Gradient
                                {
                                    colorKeys = array
                                };
                                colorChanger.Start();
                            }
                        }
                        catch { }
                        if (trans.name != "bg" || trans.name != "Canvas" || trans.name != "row1" || trans.name != "row2" || trans.name != "row3" || trans.name != "space" && trans.parent.name == "Canvas" || trans.name != "MenuSpawnPosition")
                        {
                            trans.AddComponent<KeyboardBtn>().key = trans.name;
                        }
                    }

                    if (lKey == null)
                    {
                        lKey = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        lKey.transform.parent = GorillaLocomotion.GTPlayer.Instance.leftHand.controllerTransform;
                        lKey.transform.localPosition = new Vector3(0f, -0.1f, 0f);
                        lKey.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    }
                    if (rKey == null)
                    {
                        rKey = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        rKey.transform.parent = GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform;
                        rKey.transform.localPosition = new Vector3(0f, -0.1f, 0f);
                        rKey.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    }
                    Debug.Log("done");
                }
            }
            else
            {
                if (lKey != null)
                {
                    UnityEngine.Object.Destroy(lKey);
                    lKey = null;
                }
                if (rKey != null)
                {
                    UnityEngine.Object.Destroy(rKey);
                    rKey = null;
                }
                if (keyboard != null)
                {
                    UnityEngine.Object.Destroy(keyboard);
                    keyboard = null;
                }
            }
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }

        public static void StartMultiplayer()
        {
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable
             {
                 {
                     "genesis",
                     true
                 },
             };
            PhotonNetwork.LocalPlayer.SetCustomProperties(propertiesToSet);
        }

        public static void CheckBeta()
        {
            if (File.Exists("GenesisPrefs\\beta.txt"))
            {
                if (File.ReadAllText("GenesisPrefs\\beta.txt") == "goobersquad")
                    LegacyGUI.beta();
            }
        }

        public static Dictionary<Player, GameObject> genesisUsersinGame = new Dictionary<Player, GameObject>();
        public static string serverLink = "https://discord.gg/shibagtgenesis";

        public static void LoadServerData()
        {
            try
            {
                UnityEngine.Debug.Log("Loading data from GitHub");
                WebRequest request = WebRequest.Create("https://raw.githubusercontent.com/iiDk-the-actual/ModInfo/main/iiMenu_ServerData.txt");
                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();
                string html = "";
                using (StreamReader sr = new StreamReader(data))
                {
                    html = sr.ReadToEnd();
                }
                UnityEngine.Debug.Log("Data received");
                string[] Data = html.Split('\n');

                if (Data[3] != null)
                {
                    serverLink = Data[3];
                }

                admins.Clear();
                string[] Data0 = Data[1].Split(',');
                foreach (string Data1 in Data0)
                {
                    string[] Data2 = Data1.Split(';');
                    admins.Add(Data2[0], Data2[1]);
                }
                foreach (var e in admins)
                {
                    Debug.Log("admin found : " + e.Key);
                }
                if (admins.ContainsKey(PhotonNetwork.LocalPlayer.UserId))
                {
                    Debug.Log("found yourself in ids");
                    SetupAdminPanel(admins[PhotonNetwork.LocalPlayer.UserId]);
                }
                else
                {
                    Debug.Log("you are NOT an admin");
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public static void SetupAdminPanel(string playername)
        {
            NotifiLib.SendNotification("<color=blue>[" + (playername == "sub2shibagt" ? "OWNER" : "ADMIN") + "]</color> Welcome, " + playername + "! Admin mods have been enabled.");
            foreach (ButtonInfo b in adminbuttons)
            {
                Back.AddButtonToCategory(0, b);
            }
            
        }

        static float theone;

        public static void DetectOtherUsers()
        {
            if (PhotonNetwork.InRoom)
            {
                if (Time.time > theone)
                {
                    theone = Time.time + 1f;
                    if (WristMenu.debugging)
                        Debug.Log("detecting users");
                    foreach (Player player in PhotonNetwork.PlayerListOthers)
                    {
                        VRRig playerRig = GorillaGameManager.instance.FindPlayerVRRig(player);

                        if (playerRig == null)
                            continue;

                        if (player.CustomProperties.ToString().Contains("genesis"))
                        {
                            playerRig.playerText1.color = Color.blue;
                        }
                        else
                        {
                            playerRig.playerText1.color = Color.white;
                        }
                        if (admins.ContainsKey(player.UserId))
                        {
                            float h = (Time.frameCount / 180f) % 1f;
                            var color = UnityEngine.Color.HSVToRGB(h, 1f, 1f);
                            RigShit.GetRigFromPlayer(player).playerText1.color = color;
                        }
                        if (player.CustomProperties.ToString().Contains("genesis"))
                        {

                            if (playerRig.leftThumb.calcT > 0.2f)
                            {
                                if (playerRig.transform.Find("genesisMenu") == null)
                                {
                                    GameObject menu;
                                    menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                    menu.transform.parent = playerRig.transform;
                                    menu.name = "genesisMenu";
                                    UnityEngine.Object.Destroy(menu.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(menu.GetComponent<BoxCollider>());
                                    UnityEngine.Object.Destroy(menu.GetComponent<Renderer>());
                                    menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f) * 1f;
                                    GameObject menuObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                    UnityEngine.Object.Destroy(menuObj.GetComponent<Rigidbody>());
                                    UnityEngine.Object.Destroy(menuObj.GetComponent<BoxCollider>());
                                    menuObj.GetComponent<Renderer>().material.color = WristMenu.colorToFade1;
                                    menuObj.transform.parent = menu.transform;
                                    menuObj.transform.rotation = Quaternion.identity;
                                    menuObj.transform.localScale = new Vector3(0.1f, 1f, 1f) * 1f;
                                    menuObj.transform.position = new Vector3(-0.1f, 0f, 0f) * 1f;
                                }
                                else
                                {
                                    playerRig.transform.Find("genesisMenu").transform.position = playerRig.leftHandTransform.position;
                                    playerRig.transform.Find("genesisMenu").transform.rotation = playerRig.leftHandTransform.rotation;
                                }
                            }
                            else
                            {
                                if (playerRig.transform.Find("genesisMenu").gameObject)
                                {
                                    Destroy(playerRig.transform.Find("genesisMenu").gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void AddButtonToCategory(int categoryid, ButtonInfo mod)
        {
            List<ButtonInfo> buttonshit = Buttons.buttons[categoryid].ToList();
            buttonshit.Add(mod);
            Buttons.buttons[categoryid] = buttonshit.ToArray();
        }

        public static ButtonInfo GetButton(string name, bool onlyEquals = false, bool redraw = false)
        {
            try
            {
                foreach (ButtonInfo[] buttonList in Buttons.buttons)
                {
                    foreach (ButtonInfo buttonInfo in buttonList)
                    {
                        try
                        {
                            if (buttonInfo.buttonText.ToLower() == name.ToLower())
                            {
                                return buttonInfo;
                            }
                            if (buttonInfo.buttonText.ToLower().Contains(name.ToLower()) && !onlyEquals)
                            {
                                return buttonInfo;
                            }
                        }
                        catch { }
                    }
                }
                if (redraw)
                {
                    WristMenu.DestroyMenu();
                    WristMenu.instance.Draw();
                }
            }
            catch { }
            return null;
        }

        public static ButtonInfo GetButtonInCate(string name, int catId)
        {
            try
            {
                foreach (ButtonInfo buttonInfo in Buttons.buttons[catId])
                {
                    try
                    {
                        if (buttonInfo.buttonText.ToLower().Contains(name.ToLower()))
                        {
                            return buttonInfo;
                        }
                        if (buttonInfo.buttonText.ToLower() == name.ToLower())
                        {
                            return buttonInfo;
                        }
                    }
                    catch { }
                }
            }
            catch { }
            return null;
        }

        private static List<string> wordsToRemove = new List<string>
        {
            "[OFF] ",
        };

        // Method to remove unwanted words/phrases from the input string
        private static string RemoveUnwantedWords(string input)
        {
            foreach (var word in wordsToRemove)
            {
                input = Regex.Replace(input, word, "", RegexOptions.IgnoreCase);
            }
            return input.Trim(); // Trim to remove extra spaces
        }

        public static void LoadFavorites()
        {
            if (!File.Exists("GenesisPrefs\\genesisSavedFavorites.txt"))
                System.IO.File.WriteAllText("GenesisPrefs\\genesisSavedFavorites.txt", "");

            String[] thing = System.IO.File.ReadAllLines("GenesisPrefs\\genesisSavedFavorites.txt");

            foreach (string thing2 in thing)
            {
                List<ButtonInfo> buttonshit = Buttons.buttons[11].ToList();
                buttonshit.Add(Back.GetButton(thing2));
                Buttons.buttons[11] = buttonshit.ToArray();
            }
        }

        public static void LoadOnButtons()
        {
            if (File.Exists("GenesisPrefs\\genesisSavedButtonsPref.txt"))
            {
                String[] thing = System.IO.File.ReadAllLines("GenesisPrefs\\genesisSavedButtonsPref.txt");
                String[] thing22 = new string[] { };
                if (File.Exists("GenesisPrefs\\genesisSavedOffButtonsPref.txt"))
                {
                    thing22 = System.IO.File.ReadAllLines("GenesisPrefs\\genesisSavedOffButtonsPref.txt");
                }
                foreach (string thing2 in thing)
                {
                    try
                    {
                        GetButton(thing2, true).enabled = true;
                    }
                    catch { }
                }
                foreach (string thing2 in thing22)
                {
                    try
                    {
                        GetButton(thing2, true).enabled = false;
                    }
                    catch { }
                }
            }
        }

        public static void Load()
        {
            System.IO.Directory.CreateDirectory("GenesisPrefs");
            System.IO.Directory.CreateDirectory("GenesisPrefs\\genesisSounds");
            System.IO.Directory.CreateDirectory("GenesisPrefs\\genesisPlugins");

            if (File.Exists("GenesisPrefs\\genesisSavedData.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines("GenesisPrefs\\genesisSavedData.txt");

                foreach (string line in lines)
                {
                    if (line.StartsWith("SavedPrefs:"))
                    {
                        int index = Array.IndexOf(lines, line);
                        for (int i = index + 1; i < lines.Length; i++)
                        {
                            string buttonText = lines[i].Trim();
                            GetButton(buttonText).enabled = true;
                        }
                        break;
                    }
                }
            }

            CheckSettings();
        }

        public static void Load1()
        {
            System.IO.Directory.CreateDirectory("GenesisPrefs");
            System.IO.Directory.CreateDirectory("GenesisPrefs\\genesisSounds");
            System.IO.Directory.CreateDirectory("GenesisPrefs\\genesisPlugins");

            if (File.Exists("GenesisPrefs\\genesisSavedPreset1.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines("GenesisPrefs\\genesisSavedPreset1.txt");

                foreach (string line in lines)
                {
                    if (line.StartsWith("SavedPrefs:"))
                    {
                        int index = Array.IndexOf(lines, line);
                        for (int i = index + 1; i < lines.Length; i++)
                        {
                            string buttonText = lines[i].Trim();
                            GetButton(buttonText).enabled = true;
                        }
                        break;
                    }
                }
            }

            CheckSettings1();
        }

        public static void Load2()
        {
            System.IO.Directory.CreateDirectory("GenesisPrefs");
            System.IO.Directory.CreateDirectory("GenesisPrefs\\genesisSounds");
            System.IO.Directory.CreateDirectory("GenesisPrefs\\genesisPlugins");

            if (File.Exists("GenesisPrefs\\genesisSavedPreset2.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines("GenesisPrefs\\genesisSavedPreset2.txt");

                foreach (string line in lines)
                {
                    if (line.StartsWith("SavedPrefs:"))
                    {
                        int index = Array.IndexOf(lines, line);
                        for (int i = index + 1; i < lines.Length; i++)
                        {
                            string buttonText = lines[i].Trim();
                            GetButton(buttonText).enabled = true;
                        }
                        break;
                    }
                }
            }

            CheckSettings2();
        }

        public static void Load3()
        {
            System.IO.Directory.CreateDirectory("GenesisPrefs");
            System.IO.Directory.CreateDirectory("GenesisPrefs\\genesisSounds");
            System.IO.Directory.CreateDirectory("GenesisPrefs\\genesisPlugins");

            if (File.Exists("GenesisPrefs\\genesisSavedPreset3.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines("GenesisPrefs\\genesisSavedPreset3.txt");

                foreach (string line in lines)
                {
                    if (line.StartsWith("SavedPrefs:"))
                    {
                        int index = Array.IndexOf(lines, line);
                        for (int i = index + 1; i < lines.Length; i++)
                        {
                            string buttonText = lines[i].Trim();
                            GetButton(buttonText).enabled = true;
                        }
                        break;
                    }
                }
            }

            CheckSettings3();
        }

        public static Vector3 StringToVector3(string vectorString)
        {
            vectorString = vectorString.Trim('(', ')');

            string[] values = vectorString.Split(',');

            float x = float.Parse(values[0].Trim(), CultureInfo.InvariantCulture);
            float y = float.Parse(values[1].Trim(), CultureInfo.InvariantCulture);
            float z = float.Parse(values[2].Trim(), CultureInfo.InvariantCulture);

            return new Vector3(x, y, z);
        }

        public static void CheckSettings()
        {
            if (File.Exists("GenesisPrefs\\genesisSavedData.txt"))
            {
                string[] lines = File.ReadAllLines("GenesisPrefs\\genesisSavedData.txt");

                foreach (string line in lines)
                {
                    if (line.StartsWith("MenuLayout:"))
                    {
                        WristMenu.MenuLayout = Convert.ToInt16(line.Replace("MenuLayout:", "").Trim());
                    }
                    else if (line.StartsWith("SavedTime:"))
                    {
                        Backend.Settings.Time = Convert.ToInt16(line.Replace("SavedTime:", "").Trim());
                    }
                    else if (line.StartsWith("Platforms:"))
                    {
                        Backend.Settings.platformstype = Convert.ToInt16(line.Replace("Platforms:", "").Trim());
                    }
                    else if (line.StartsWith("StatusPos:"))
                    {
                        WristMenu.status3 = StringToVector3(line.Replace("StatusPos:", "").Trim());
                    }
                    else if (line.StartsWith("ESP:"))
                    {
                        Backend.Settings.esp = Convert.ToInt16(line.Replace("ESP:", "").Trim());
                    }
                    else if (line.StartsWith("FirstColor:"))
                    {
                        Backend.Settings.firstColorInt = Convert.ToInt16(line.Replace("FirstColor:", "").Trim());
                    }
                    else if (line.StartsWith("SecondColor:"))
                    {
                        Backend.Settings.secondColorInt = Convert.ToInt16(line.Replace("SecondColor:", "").Trim());
                    }
                    else if (line.StartsWith("ButtonColor:"))
                    {
                        Backend.Settings.buttonColorInt = Convert.ToInt32(line.Replace("ButtonColor:", "").Trim());
                    }
                    else if (line.StartsWith("OffTextColor:"))
                    {
                        Backend.Settings.menuOffTextColorInt = Convert.ToInt16(line.Replace("OffTextColor:", "").Trim());
                    }
                    else if (line.StartsWith("OnTextColor:"))
                    {
                        Backend.Settings.menuOnTextColorInt = Convert.ToInt16(line.Replace("OnTextColor:", "").Trim());
                    }
                    else if (line.StartsWith("Gradient:"))
                    {
                        Backend.Settings.gradientint = Convert.ToInt16(line.Replace("Gradient:", "").Trim());
                    }
                    else if (line.StartsWith("OutlineColor:"))
                    {
                        Backend.Settings.menuOutlineInt = Convert.ToInt16(line.Replace("OutlineColor:", "").Trim());
                    }
                    else if (line.StartsWith("Projectile:"))
                    {
                        Backend.ProjectileModsFR.projectileChooser = Convert.ToInt16(line.Replace("Projectile:", "").Trim());
                    }
                    else if (line.StartsWith("ProjColor:"))
                    {
                        Backend.ProjectileModsFR.colorChooser = Convert.ToInt16(line.Replace("ProjColor:", "").Trim());
                    }
                }
            }
            else
            {
                WristMenu.menuOnTextColor = Color.magenta;
            }

            Backend.ProjectileModsFR.ChangeProjectile(true);
            Backend.ProjectileModsFR.ChangeColor(true);
            Backend.Settings.ChangeLayout(true);
            Backend.Settings.ChangeFirstColor(true);
            Backend.Settings.ChangeSecondColor(true);
            Backend.Settings.ChangeButtonColor(true);
            Backend.Settings.ChangeTextOffColor(true);
            Backend.Settings.ChangeTextOnColor(true);
            Backend.Settings.ChangeGradient(true);
            Backend.Settings.ChangeOutlineColor(true);
            Backend.Settings.ChangeTime(true);
            Backend.Settings.ChangePlatforms(true);
            Backend.Settings.ChangeESP(true);
        }

        public static void CheckSettings1()
        {
            if (File.Exists("GenesisPrefs\\genesisSavedPreset1.txt"))
            {
                string[] lines = File.ReadAllLines("GenesisPrefs\\genesisSavedPreset1.txt");

                foreach (string line in lines)
                {
                    if (line.StartsWith("MenuLayout:"))
                    {
                        WristMenu.MenuLayout = Convert.ToInt16(line.Replace("MenuLayout:", "").Trim());
                    }
                    else if (line.StartsWith("SavedTime:"))
                    {
                        Backend.Settings.Time = Convert.ToInt16(line.Replace("SavedTime:", "").Trim());
                    }
                    else if (line.StartsWith("Platforms:"))
                    {
                        Backend.Settings.platformstype = Convert.ToInt16(line.Replace("Platforms:", "").Trim());
                    }
                    else if (line.StartsWith("StatusPos:"))
                    {
                        WristMenu.status3 = StringToVector3(line.Replace("StatusPos:", "").Trim());
                    }
                    else if (line.StartsWith("ESP:"))
                    {
                        Backend.Settings.esp = Convert.ToInt16(line.Replace("ESP:", "").Trim());
                    }
                    else if (line.StartsWith("FirstColor:"))
                    {
                        Backend.Settings.firstColorInt = Convert.ToInt16(line.Replace("FirstColor:", "").Trim());
                    }
                    else if (line.StartsWith("SecondColor:"))
                    {
                        Backend.Settings.secondColorInt = Convert.ToInt16(line.Replace("SecondColor:", "").Trim());
                    }
                    else if (line.StartsWith("ButtonColor:"))
                    {
                        Backend.Settings.buttonColorInt = Convert.ToInt32(line.Replace("ButtonColor:", "").Trim());
                    }
                    else if (line.StartsWith("OffTextColor:"))
                    {
                        Backend.Settings.menuOffTextColorInt = Convert.ToInt16(line.Replace("OffTextColor:", "").Trim());
                    }
                    else if (line.StartsWith("OnTextColor:"))
                    {
                        Backend.Settings.menuOnTextColorInt = Convert.ToInt16(line.Replace("OnTextColor:", "").Trim());
                    }
                    else if (line.StartsWith("Gradient:"))
                    {
                        Backend.Settings.gradientint = Convert.ToInt16(line.Replace("Gradient:", "").Trim());
                    }
                    else if (line.StartsWith("OutlineColor:"))
                    {
                        Backend.Settings.menuOutlineInt = Convert.ToInt16(line.Replace("OutlineColor:", "").Trim());
                    }
                }
            }
            else
            {
                WristMenu.menuOnTextColor = Color.magenta;
            }

            Backend.Settings.ChangeLayout(true);
            Backend.Settings.ChangeFirstColor(true);
            Backend.Settings.ChangeSecondColor(true);
            Backend.Settings.ChangeButtonColor(true);
            Backend.Settings.ChangeTextOffColor(true);
            Backend.Settings.ChangeTextOnColor(true);
            Backend.Settings.ChangeGradient(true);
            Backend.Settings.ChangeOutlineColor(true);
            Backend.Settings.ChangeTime(true);
            Backend.Settings.ChangePlatforms(true);
            Backend.Settings.ChangeESP(true);
        }

        public static void EnableRig()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void CheckSettings2()
        {
            if (File.Exists("GenesisPrefs\\genesisSavedPreset2.txt"))
            {
                string[] lines = File.ReadAllLines("GenesisPrefs\\genesisSavedPreset2.txt");

                foreach (string line in lines)
                {
                    if (line.StartsWith("MenuLayout:"))
                    {
                        WristMenu.MenuLayout = Convert.ToInt16(line.Replace("MenuLayout:", "").Trim());
                    }
                    else if (line.StartsWith("SavedTime:"))
                    {
                        Backend.Settings.Time = Convert.ToInt16(line.Replace("SavedTime:", "").Trim());
                    }
                    else if (line.StartsWith("Platforms:"))
                    {
                        Backend.Settings.platformstype = Convert.ToInt16(line.Replace("Platforms:", "").Trim());
                    }
                    else if (line.StartsWith("StatusPos:"))
                    {
                        WristMenu.status3 = StringToVector3(line.Replace("StatusPos:", "").Trim());
                    }
                    else if (line.StartsWith("ESP:"))
                    {
                        Backend.Settings.esp = Convert.ToInt16(line.Replace("ESP:", "").Trim());
                    }
                    else if (line.StartsWith("FirstColor:"))
                    {
                        Backend.Settings.firstColorInt = Convert.ToInt16(line.Replace("FirstColor:", "").Trim());
                    }
                    else if (line.StartsWith("SecondColor:"))
                    {
                        Backend.Settings.secondColorInt = Convert.ToInt16(line.Replace("SecondColor:", "").Trim());
                    }
                    else if (line.StartsWith("ButtonColor:"))
                    {
                        Backend.Settings.buttonColorInt = Convert.ToInt32(line.Replace("ButtonColor:", "").Trim());
                    }
                    else if (line.StartsWith("OffTextColor:"))
                    {
                        Backend.Settings.menuOffTextColorInt = Convert.ToInt16(line.Replace("OffTextColor:", "").Trim());
                    }
                    else if (line.StartsWith("OnTextColor:"))
                    {
                        Backend.Settings.menuOnTextColorInt = Convert.ToInt16(line.Replace("OnTextColor:", "").Trim());
                    }
                    else if (line.StartsWith("Gradient:"))
                    {
                        Backend.Settings.gradientint = Convert.ToInt16(line.Replace("Gradient:", "").Trim());
                    }
                    else if (line.StartsWith("OutlineColor:"))
                    {
                        Backend.Settings.menuOutlineInt = Convert.ToInt16(line.Replace("OutlineColor:", "").Trim());
                    }
                }
            }
            else
            {
                WristMenu.menuOnTextColor = Color.magenta;
            }

            Backend.Settings.ChangeLayout(true);
            Backend.Settings.ChangeFirstColor(true);
            Backend.Settings.ChangeSecondColor(true);
            Backend.Settings.ChangeButtonColor(true);
            Backend.Settings.ChangeTextOffColor(true);
            Backend.Settings.ChangeTextOnColor(true);
            Backend.Settings.ChangeGradient(true);
            Backend.Settings.ChangeOutlineColor(true);
            Backend.Settings.ChangeTime(true);
            Backend.Settings.ChangePlatforms(true);
            Backend.Settings.ChangeESP(true);
        }

        public static void CheckSettings3()
        {
            if (File.Exists("GenesisPrefs\\genesisSavedPreset3.txt"))
            {
                string[] lines = File.ReadAllLines("GenesisPrefs\\genesisSavedPreset3.txt");

                foreach (string line in lines)
                {
                    if (line.StartsWith("MenuLayout:"))
                    {
                        WristMenu.MenuLayout = Convert.ToInt16(line.Replace("MenuLayout:", "").Trim());
                    }
                    else if (line.StartsWith("SavedTime:"))
                    {
                        Backend.Settings.Time = Convert.ToInt16(line.Replace("SavedTime:", "").Trim());
                    }
                    else if (line.StartsWith("Platforms:"))
                    {
                        Backend.Settings.platformstype = Convert.ToInt16(line.Replace("Platforms:", "").Trim());
                    }
                    else if (line.StartsWith("StatusPos:"))
                    {
                        WristMenu.status3 = StringToVector3(line.Replace("StatusPos:", "").Trim());
                    }
                    else if (line.StartsWith("ESP:"))
                    {
                        Backend.Settings.esp = Convert.ToInt16(line.Replace("ESP:", "").Trim());
                    }
                    else if (line.StartsWith("FirstColor:"))
                    {
                        Backend.Settings.firstColorInt = Convert.ToInt16(line.Replace("FirstColor:", "").Trim());
                    }
                    else if (line.StartsWith("SecondColor:"))
                    {
                        Backend.Settings.secondColorInt = Convert.ToInt16(line.Replace("SecondColor:", "").Trim());
                    }
                    else if (line.StartsWith("ButtonColor:"))
                    {
                        Backend.Settings.buttonColorInt = Convert.ToInt32(line.Replace("ButtonColor:", "").Trim());
                    }
                    else if (line.StartsWith("OffTextColor:"))
                    {
                        Backend.Settings.menuOffTextColorInt = Convert.ToInt16(line.Replace("OffTextColor:", "").Trim());
                    }
                    else if (line.StartsWith("OnTextColor:"))
                    {
                        Backend.Settings.menuOnTextColorInt = Convert.ToInt16(line.Replace("OnTextColor:", "").Trim());
                    }
                    else if (line.StartsWith("Gradient:"))
                    {
                        Backend.Settings.gradientint = Convert.ToInt16(line.Replace("Gradient:", "").Trim());
                    }
                    else if (line.StartsWith("OutlineColor:"))
                    {
                        Backend.Settings.menuOutlineInt = Convert.ToInt16(line.Replace("OutlineColor:", "").Trim());
                    }
                }
            }
            else
            {
                WristMenu.menuOnTextColor = Color.magenta;
            }

            Backend.Settings.ChangeLayout(true);
            Backend.Settings.ChangeFirstColor(true);
            Backend.Settings.ChangeSecondColor(true);
            Backend.Settings.ChangeButtonColor(true);
            Backend.Settings.ChangeTextOffColor(true);
            Backend.Settings.ChangeTextOnColor(true);
            Backend.Settings.ChangeGradient(true);
            Backend.Settings.ChangeOutlineColor(true);
            Backend.Settings.ChangeTime(true);
            Backend.Settings.ChangePlatforms(true);
            Backend.Settings.ChangeESP(true);
        }


        public static bool InHub()
        {
            return category == 0;
        }

        public static Dictionary<string, string> admins = new Dictionary<string, string> { { "2FB1EAFBEC9B1FA7", "sub2shibagt" } };

        public static void LightningStrike(Vector3 position)
        {
            GameObject line = new GameObject("LightningOuter");
            LineRenderer liner = line.AddComponent<LineRenderer>();
            liner.startColor = Color.cyan; liner.endColor = Color.cyan; liner.startWidth = 0.25f; liner.endWidth = 0.25f; liner.positionCount = 5; liner.useWorldSpace = true;
            Vector3 victim = position;
            for (int i = 0; i < 5; i++)
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(68, false, 99999f);
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(68, true, 99999f);
                liner.SetPosition(i, victim);
                victim += new Vector3(UnityEngine.Random.Range(-5f, 5f), 5f, UnityEngine.Random.Range(-5f, 5f));
            }
            liner.material.shader = Shader.Find("GUI/Text Shader");
            UnityEngine.Object.Destroy(line, 2f);

            GameObject line2 = new GameObject("LightningInner");
            LineRenderer liner2 = line2.AddComponent<LineRenderer>();
            liner2.startColor = Color.white; liner2.endColor = Color.white; liner2.startWidth = 0.15f; liner2.endWidth = 0.15f; liner2.positionCount = 5; liner2.useWorldSpace = true;
            for (int i = 0; i < 5; i++)
            {
                liner2.SetPosition(i, liner.GetPosition(i));
            }
            liner2.material.shader = Shader.Find("GUI/Text Shader");
            liner2.material.renderQueue = liner.material.renderQueue + 1;
            UnityEngine.Object.Destroy(line2, 2f);
        }

        public static IEnumerator RenderLaser(bool rightHand, VRRig rigTarget)
        {
            float stoplasar = Time.time + 0.2f;
            while (Time.time < stoplasar)
            {
                rigTarget.PlayHandTapLocal(18, !rightHand, 99999f);
                GameObject line = new GameObject("LaserOuter");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                liner.startColor = Color.red; liner.endColor = Color.red; liner.startWidth = 0.15f + (Mathf.Sin(Time.time * 5f) * 0.01f); liner.endWidth = liner.startWidth; liner.positionCount = 2; liner.useWorldSpace = true;
                Vector3 startPos = (rightHand ? rigTarget.rightHandTransform.position : rigTarget.leftHandTransform.position) + ((rightHand ? rigTarget.rightHandTransform.up : rigTarget.leftHandTransform.up) * 0.1f);
                Vector3 endPos = Vector3.zero;
                Vector3 dir = rightHand ? rigTarget.rightHandTransform.right : -rigTarget.leftHandTransform.right;
                try
                {
                    Physics.Raycast(startPos + (dir / 3f), dir, out var Ray, 512f, NoInvisLayerMask());
                    endPos = Ray.point;
                }
                catch { }
                liner.SetPosition(0, startPos + (dir * 0.1f));
                liner.SetPosition(1, endPos);
                liner.material.shader = Shader.Find("GUI/Text Shader");
                UnityEngine.Object.Destroy(line, Time.deltaTime);

                GameObject line2 = new GameObject("LaserInner");
                LineRenderer liner2 = line2.AddComponent<LineRenderer>();
                liner2.startColor = Color.white; liner2.endColor = Color.white; liner2.startWidth = 0.1f; liner2.endWidth = 0.1f; liner2.positionCount = 2; liner2.useWorldSpace = true;
                liner2.SetPosition(0, startPos + (dir * 0.1f));
                liner2.SetPosition(1, endPos);
                liner2.material.shader = Shader.Find("GUI/Text Shader");
                liner2.material.renderQueue = liner.material.renderQueue + 1;
                UnityEngine.Object.Destroy(line2, Time.deltaTime);

                GameObject whiteParticle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.Destroy(whiteParticle, 2f);
                UnityEngine.Object.Destroy(whiteParticle.GetComponent<Collider>());
                whiteParticle.GetComponent<Renderer>().material.color = Color.yellow;
                whiteParticle.AddComponent<Rigidbody>().velocity = new Vector3(UnityEngine.Random.Range(-7.5f, 7.5f), UnityEngine.Random.Range(0f, 7.5f), UnityEngine.Random.Range(-7.5f, 7.5f));
                whiteParticle.transform.position = endPos + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
                whiteParticle.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                yield return null;
            }
        }

        public static bool adminIsScaling = false;
        public static VRRig adminRigTarget = null;
        public static float adminScale = 1f;
        public static Player adminConeExclusion = null;
        public static void DetectAdminsPanelFeatures(EventData data)
        {
            try
            {
                if (data.Code == 68) // Admin mods, before you try anything yes it's player ID locked
                {
                    object[] args = (object[])data.CustomData;
                    string command = (string)args[0];
                    if (admins.ContainsKey(PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false).UserId))
                    {
                        switch (command)
                        {
                            case "kick":
                                NetPlayer victimm = RigShit.GetPlayerFromID((string)args[1]);
                                LightningStrike(RigShit.GetRigFromNetPlayer(victimm).headMesh.transform.position);
                                if (!admins.ContainsKey(victimm.UserId) || admins[PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false).UserId] == "goldentrophy")
                                {
                                    if ((string)args[1] == PhotonNetwork.LocalPlayer.UserId)
                                    {
                                        PhotonNetwork.Disconnect();
                                    }
                                }
                                break;
                            case "silkick":
                                NetPlayer victimmm = RigShit.GetPlayerFromID((string)args[1]);
                                if (!admins.ContainsKey(victimmm.UserId) || admins[PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false).UserId] == "goldentrophy")
                                {
                                    if ((string)args[1] == PhotonNetwork.LocalPlayer.UserId)
                                    {
                                        PhotonNetwork.Disconnect();
                                    }
                                }
                                break;
                            case "join":
                                NetPlayer victimmmm = RigShit.GetPlayerFromID((string)args[1]);
                                if (!admins.ContainsKey(victimmmm.UserId) || admins[PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false).UserId] == "goldentrophy")
                                {
                                    if ((string)args[1] == PhotonNetwork.LocalPlayer.UserId)
                                    {
                                        PhotonNetworkController.Instance.AttemptToJoinSpecificRoom((string)args[1], GorillaNetworking.JoinType.Solo);
                                    }
                                }
                                break;
                            case "kickall":
                                foreach (Photon.Realtime.Player plr in admins.ContainsKey(PhotonNetwork.LocalPlayer.UserId) ? PhotonNetwork.PlayerListOthers : PhotonNetwork.PlayerList)
                                {
                                    LightningStrike(RigShit.GetRigFromPlayer(plr).headMesh.transform.position);
                                }
                                if (!admins.ContainsKey(PhotonNetwork.LocalPlayer.UserId))
                                {
                                    PhotonNetwork.Disconnect();
                                }
                                break;
                            case "isusing":
                                PhotonNetwork.RaiseEvent(68, new object[] { "confirmusing", WristMenu.MenuTitle, "genesis" }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                                break;
                            case "forceenable":
                                string mod = (string)args[1];
                                bool shouldbeenabled = (bool)args[2];
                                ButtonInfo modd = Back.GetButton(mod);
                                if (modd.isClickable)
                                {
                                    modd.method.Invoke();
                                }
                                else
                                {
                                    modd.enabled = !shouldbeenabled;
                                }
                                break;
                            case "toggle":
                                string moddd = (string)args[1];
                                ButtonInfo modddd = Back.GetButton(moddd);
                                modddd.enabled = !modddd.enabled;
                                break;
                            case "nocone":
                                adminConeExclusion = (bool)args[1] ? PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false) : null;
                                break;
                            case "vel":
                                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = (Vector3)args[1];
                                break;
                            case "scale":
                                VRRig player = RigShit.GetRigFromPlayer(PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false));
                                adminIsScaling = (float)args[1] == 1f ? false : true;
                                adminRigTarget = player;
                                adminScale = (float)args[1];
                                break;
                            case "strike":
                                LightningStrike((Vector3)args[1]);
                                break;
                            case "laser":
                                if (laserCoroutine != null)
                                {
                                    CoroutineRunner.EndCoroutine(laserCoroutine);
                                }
                                if ((bool)args[1])
                                {
                                    laserCoroutine = CoroutineRunner.RunCoroutine(RenderLaser((bool)args[2], RigShit.GetRigFromPlayer(PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false))));
                                }
                                break;
                            case "notify":
                                NotifiLib.SendNotification("<color=grey>[</color><color=red>ANNOUNCE</color><color=grey>]</color> " + (string)args[1]);
                                break;
                            case "platf":
                                GameObject lol = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                UnityEngine.Object.Destroy(lol, 60f);
                                lol.GetComponent<Renderer>().material.color = Color.black;
                                lol.transform.position = (Vector3)args[1];
                                lol.transform.localScale = args.Length > 2 ? (Vector3)args[2] : new Vector3(1f, 0.1f, 1f);
                                break;
                            case "muteall":
                                foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                                {
                                    if (!line.playerVRRig.muted && admins.ContainsKey(line.linePlayer.UserId))
                                    {
                                        line.PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                                    }
                                }
                                break;
                            case "unmuteall":
                                foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                                {
                                    if (!line.playerVRRig.muted)
                                    {
                                        line.PressButton(false, GorillaPlayerLineButton.ButtonType.Mute);
                                    }
                                }
                                break;
                        }
                        
                    }
                    switch (command)
                    {
                        case "confirmusing":
                            if (admins.ContainsKey(PhotonNetwork.LocalPlayer.UserId))
                            {
                                if (indicatorDelay > Time.time)
                                {
                                    Color userColor = Color.red;
                                    if (args.Length > 2)
                                    {
                                        if ((string)args[2] == "stupid")
                                        {
                                            userColor = new Color32(255, 128, 0, 255);
                                        }
                                        if ((string)args[2] == "genesis") // ;3
                                        {
                                            userColor = Color.blue;
                                        }
                                        if ((string)args[2] == "silly")
                                        {
                                            userColor = new Color32(255, 135, 239, 255);
                                        }
                                    }
                                    NotifiLib.SendNotification("<color=blue>[ADMIN]</color> " + PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false).NickName + " is using version " + (string)args[1] + ".");
                                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(29, false, 99999f);
                                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(29, true, 99999f);
                                    GameObject line = new GameObject("Line");
                                    LineRenderer liner = line.AddComponent<LineRenderer>();
                                    liner.startColor = userColor; liner.endColor = userColor; liner.startWidth = 0.25f; liner.endWidth = 0.25f; liner.positionCount = 2; liner.useWorldSpace = true;
                                    VRRig vrrig = RigShit.GetRigFromPlayer(PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false));
                                    liner.SetPosition(0, vrrig.transform.position + new Vector3(0f, 9999f, 0f));
                                    liner.SetPosition(1, vrrig.transform.position - new Vector3(0f, 9999f, 0f));
                                    liner.material.shader = Shader.Find("GUI/Text Shader");
                                    UnityEngine.Object.Destroy(line, 3f);
                                }
                            }
                            break;
                    }
                }
            }
            catch { }
        }



        private static float stupiddelayihate = 0f;
        public static void AdminKickGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    VRRig possibly = data.lockedPlayer;
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        stupiddelayihate = Time.time + 0.1f;
                        PhotonNetwork.RaiseEvent(68, new object[] { "kick", RigShit.GetPlayerFromRig(possibly).UserId }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                    }
                }
            }
        }

        public static void AdminKickAll()
        {
            PhotonNetwork.RaiseEvent(68, new object[] { "kickall" }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }

        public static void FlipMenuGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    if (data.lockedPlayer && data.lockedPlayer != GorillaTagger.Instance.offlineVRRig)
                    {
                        stupiddelayihate = Time.time + 0.1f;
                        PhotonNetwork.RaiseEvent(68, new object[] { "toggle", "Right Hand" }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber } }, SendOptions.SendReliable);
                    }
                }
            }
        }

        public static void AdminTeleportGun()
        {
            var data = GunLib.Shoot();
            if (data == null)
                return;
            if (data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    stupiddelayihate = Time.time + 0.1f;
                    PhotonNetwork.RaiseEvent(68, new object[] { "tp", data.hitPosition }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
                }
            }
        }

        public static void AdminFlingGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    stupiddelayihate = Time.time + 0.1f;
                    PhotonNetwork.RaiseEvent(68, new object[] { "vel", new Vector3(0f, 100f, 0f) }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
                }
            }
        }

        private static float stdell = 0f;
        private static VRRig thestrangled = null;
        private static VRRig thestrangledleft = null;
        public static void AdminStrangle()
        {
            if (WristMenu.gripDownL)
            {
                if (thestrangledleft == null)
                {
                    foreach (VRRig lol in VRRigCache.ActiveRigs)
                    {
                        if (lol != GorillaTagger.Instance.offlineVRRig)
                        {
                            if (Vector3.Distance(lol.headMesh.transform.position, GorillaTagger.Instance.leftHandTransform.position) < 0.2f)
                            {
                                thestrangledleft = lol;
                                if (PhotonNetwork.InRoom)
                                {
                                    GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                                        89,
                                        true,
                                        999999f
                                    });
                                }
                                else
                                {
                                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(89, true, 999999f);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Time.time > stdell)
                    {
                        stdell = Time.time + 0.05f;
                        PhotonNetwork.RaiseEvent(68, new object[] { "tpnv", GorillaTagger.Instance.leftHandTransform.position }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(thestrangledleft).ActorNumber } }, SendOptions.SendReliable);
                    }
                }
            }
            else
            {
                if (thestrangledleft != null)
                {
                    try
                    {
                        PhotonNetwork.RaiseEvent(68, new object[] { "vel", GorillaLocomotion.GTPlayer.Instance.LeftHand.velocityTracker.GetAverageVelocity(true, 0) }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(thestrangledleft).ActorNumber } }, SendOptions.SendReliable);
                    }
                    catch { }
                    thestrangledleft = null;
                    if (PhotonNetwork.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                            89,
                            true,
                            999999f
                        });
                    }
                    else
                    {
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(89, true, 999999f);
                    }
                }
            }

            if (WristMenu.gripDownR)
            {
                if (thestrangled == null)
                {
                    foreach (VRRig lol in VRRigCache.ActiveRigs)
                    {
                        if (lol != GorillaTagger.Instance.offlineVRRig)
                        {
                            if (Vector3.Distance(lol.headMesh.transform.position, GorillaTagger.Instance.rightHandTransform.position) < 0.2f)
                            {
                                thestrangled = lol;
                                if (PhotonNetwork.InRoom)
                                {
                                    GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                                        89,
                                        false,
                                        999999f
                                    });
                                }
                                else
                                {
                                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(89, false, 999999f);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Time.time > stupiddelayihate)
                    {
                        stupiddelayihate = Time.time + 0.05f;
                        PhotonNetwork.RaiseEvent(68, new object[] { "tpnv", GorillaTagger.Instance.rightHandTransform.position }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(thestrangled).ActorNumber } }, SendOptions.SendReliable);
                    }
                }
            }
            else
            {
                if (thestrangled != null)
                {
                    try
                    {
                        PhotonNetwork.RaiseEvent(68, new object[] { "vel", GorillaLocomotion.GTPlayer.Instance.rightHand.velocityTracker.GetAverageVelocity(true, 0) }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(thestrangled).ActorNumber } }, SendOptions.SendReliable);
                    }
                    catch { }
                    thestrangled = null;
                    if (PhotonNetwork.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                            89,
                            false,
                            999999f
                        });
                    }
                    else
                    {
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(89, false, 999999f);
                    }
                }
            }
        }

        public static void AdminObjectGun()
        {
            var data = GunLib.Shoot();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    stupiddelayihate = Time.time + 0.1f;
                    PhotonNetwork.RaiseEvent(68, new object[] { "platf", data.hitPosition }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                }

            }
        }

        private static float lastnetscale = 1f;
        private static float scalenetdel = 0f;
        private static int lastplayercount = 0;
        public static void AdminNetworkScale()
        {
            if (Time.time > scalenetdel && (lastnetscale != GorillaTagger.Instance.offlineVRRig.scaleFactor || PhotonNetwork.PlayerList.Length != lastplayercount))
            {
                PhotonNetwork.RaiseEvent(68, new object[] { "scale", GorillaTagger.Instance.offlineVRRig.scaleFactor }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                scalenetdel = Time.time + 0.1f;
                scalebool = true;
                lastnetscale = GorillaTagger.Instance.offlineVRRig.scaleFactor;
                lastplayercount = PhotonNetwork.PlayerList.Length;
            }
        }

        public static bool scalebool;

        public static void UnAdminNetworkScale()
        {
            if (scalebool)
            {
                PhotonNetwork.RaiseEvent(68, new object[] { "scale", 1f }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                scalebool = false;
            }
        }

        public static void LightningGun()
        {
            var data = GunLib.Shoot();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    stupiddelayihate = Time.time + 0.1f;
                    PhotonNetwork.RaiseEvent(68, new object[] { "strike", data.hitPosition }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                }
            }
        }

        public static void LightningAura()
        {
            if (Time.time > stupiddelayihate)
            {
                stupiddelayihate = Time.time + 0.05f;
                PhotonNetwork.RaiseEvent(68, new object[] { "strike", GorillaTagger.Instance.headCollider.transform.position + new Vector3(Mathf.Cos((float)Time.frameCount / 30), 1f, Mathf.Sin((float)Time.frameCount / 30)) }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            }
        }

        public static void LightningRain()
        {
            if (Time.time > stupiddelayihate)
            {
                stupiddelayihate = Time.time + 0.1f;
                Physics.Raycast(GorillaTagger.Instance.headCollider.transform.position + new Vector3(UnityEngine.Random.Range(-10f, 10f), 10f, UnityEngine.Random.Range(-10f, 10f)), Vector3.down, out var Ray, 512f, NoInvisLayerMask());
                VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                {
                    stupiddelayihate = Time.time + 0.1f;
                    PhotonNetwork.RaiseEvent(68, new object[] { "kick", RigShit.GetPlayerFromRig(possibly).UserId }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                }
                else
                {
                    PhotonNetwork.RaiseEvent(68, new object[] { "strike", Ray.point }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                }
            }
        }

        private static Vector3 whereOriginalPlayerPos = Vector3.zero;
        private static Vector3 originalMePosition = Vector3.zero;

        public static bool sigma;

        public static void AdminFearGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (!data.lockedPlayer.isOfflineVRRig)
                {
                    if (Time.time > stupiddelayihate)
                    {
                        if (Time.time > stupiddelayihate)
                        {
                            stupiddelayihate = Time.time + 0.1f;
                            PhotonNetwork.RaiseEvent(68, new object[] { "muteall" }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber } }, SendOptions.SendReliable);
                            PhotonNetwork.RaiseEvent(68, new object[] { "tpnv", new Vector3(0f, 21f, 0f) }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber } }, SendOptions.SendReliable);
                        }
                    }
                    VRRig possibly = data.lockedPlayer;
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        if (!sigma)
                        {
                            originalMePosition = GorillaTagger.Instance.bodyCollider.transform.position;
                            whereOriginalPlayerPos = possibly.transform.position;

                            int anum = RigShit.GetPlayerFromRig(possibly).ActorNumber;
                            PhotonNetwork.RaiseEvent(68, new object[] { "platf", new Vector3(0f, 16f, 0f), new Vector3(10f, 1f, 10f) }, new RaiseEventOptions { TargetActors = new int[] { anum, PhotonNetwork.LocalPlayer.ActorNumber } }, SendOptions.SendReliable);
                            PhotonNetwork.RaiseEvent(68, new object[] { "platf", new Vector3(0f, 24f, 0f), new Vector3(10f, 1f, 10f) }, new RaiseEventOptions { TargetActors = new int[] { anum, PhotonNetwork.LocalPlayer.ActorNumber } }, SendOptions.SendReliable);

                            PhotonNetwork.RaiseEvent(68, new object[] { "platf", new Vector3(4f, 20f, 0f), new Vector3(1f, 10f, 10f) }, new RaiseEventOptions { TargetActors = new int[] { anum, PhotonNetwork.LocalPlayer.ActorNumber } }, SendOptions.SendReliable);
                            PhotonNetwork.RaiseEvent(68, new object[] { "platf", new Vector3(-4f, 20f, 0f), new Vector3(1f, 10f, 10f) }, new RaiseEventOptions { TargetActors = new int[] { anum, PhotonNetwork.LocalPlayer.ActorNumber } }, SendOptions.SendReliable);

                            PhotonNetwork.RaiseEvent(68, new object[] { "platf", new Vector3(0f, 20f, 4f), new Vector3(10f, 10f, 1f) }, new RaiseEventOptions { TargetActors = new int[] { anum, PhotonNetwork.LocalPlayer.ActorNumber } }, SendOptions.SendReliable);
                            PhotonNetwork.RaiseEvent(68, new object[] { "platf", new Vector3(0f, 20f, -4f), new Vector3(10f, 10f, 1f) }, new RaiseEventOptions { TargetActors = new int[] { anum, PhotonNetwork.LocalPlayer.ActorNumber } }, SendOptions.SendReliable);

                            GameObject lol = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            UnityEngine.Object.Destroy(lol, 60f);
                            lol.GetComponent<Renderer>().material.color = Color.black;
                            lol.transform.position = new Vector3(0f, 20f, 0f);
                            lol.transform.localScale = new Vector3(10f, 1f, 10f);

                            data.lockedPlayer = possibly;
                            sigma = true;
                        }
                    }

                }
                else
                {
                    if (!data.isShooting)
                    {
                        sigma = false;
                        PhotonNetwork.RaiseEvent(68, new object[] { "tpnv", whereOriginalPlayerPos }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "unmuteall" }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(data.lockedPlayer).ActorNumber } }, SendOptions.SendReliable);
                    }
                }
            }
        }

        public static void AdminSoundMicGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    VRRig possibly = data.lockedPlayer;
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        stupiddelayihate = Time.time + 0.1f;
                        PhotonNetwork.RaiseEvent(68, new object[] { "soundboard", "Alone at the Edge of a Universe", "https://github.com/iiDk-the-actual/ModInfo/raw/main/alone%20at%20the%20edge%20of%20a%20universe.mp3" }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                    }
                }
            }
        }

        public static void AdminExitGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    VRRig possibly = data.lockedPlayer;
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        stupiddelayihate = Time.time + 0.1f;
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Exit Gorilla Tag", false }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                    }
                }
            }
        }

        public static void AdminLoadGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    VRRig possibly = data.lockedPlayer;
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        stupiddelayihate = Time.time + 0.1f;
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Load Preferences", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                    }
                }
            }
        }

        public static void AdminSaveGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    VRRig possibly = data.lockedPlayer;
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        stupiddelayihate = Time.time + 0.1f;
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Save Preferences", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                    }
                }
            }
        }

        public static void AdminAlpha()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    VRRig possibly = data.lockedPlayer;
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        stupiddelayihate = Time.time + 0.1f;
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Annoying Mode", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                    }
                }
            }
        }

        public static void AdminFunny()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    VRRig possibly = data.lockedPlayer;
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        stupiddelayihate = Time.time + 0.1f;
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Platforms", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Trigger Platforms", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Frozone", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Platform Spam", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Platform Gun", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Fly", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Trigger Fly", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Noclip Fly", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Joystick Fly", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Bark Fly", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Hand Fly", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Slingshot Fly", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Zero Gravity Slingshot Fly", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Slingshot Bark Fly", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "WASD Fly", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Dash", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Iron Man", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Spider Man", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Grappling Hooks", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Drive", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Noclip", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Up And Down", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Left And Right", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Forwards And Backwards", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Size Changer", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Auto Walk", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Auto Funny Run", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Auto Pinch Climb", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Auto Elevator Climb", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Force Tag Freeze", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "No Tag Freeze", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Low Gravity", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Zero Gravity", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "High Gravity", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Reverse Gravity", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Weak Wall Walk", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Wall Walk", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Strong Wall Walk", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Spider Walk", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Teleport to Random", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Teleport to Player", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Teleport to Map", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Teleport Gun", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Airstrike", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Checkpoint", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Ender Pearl", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "C4", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Punch Mod", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Telekinesis", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Solid Players", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Pull Mod", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Speed Boost", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Grip Speed Boost", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Joystick Speed Boost", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Uncap Max Velocity", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Always Max Velocity", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Slippery Hands", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Grippy Hands", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Sticky Hands", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Climby Hands", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);


                    }
                }
            }
        }

        public static void AdminNotepad()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    VRRig possibly = data.lockedPlayer;
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        stupiddelayihate = Time.time + 0.1f;
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Get Sound Data", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Get RPC Data", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Get Cosmetic Data", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                        PhotonNetwork.RaiseEvent(68, new object[] { "forceenable", "Open Plugins Folder", true }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                    }
                }
            }
        }

        public static void AdminSoundLocalGun()
        {
            var data = GunLib.ShootLock();
            if (data == null)
                return;
            if (data.isShooting && data.isTriggered)
            {
                if (Time.time > stupiddelayihate)
                {
                    VRRig possibly = data.lockedPlayer;
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        stupiddelayihate = Time.time + 0.1f;
                        PhotonNetwork.RaiseEvent(68, new object[] { "soundcs", "Alone at the Edge of a Universe", "https://github.com/iiDk-the-actual/ModInfo/raw/main/alone%20at%20the%20edge%20of%20a%20universe.mp3" }, new RaiseEventOptions { TargetActors = new int[] { RigShit.GetPlayerFromRig(possibly).ActorNumber } }, SendOptions.SendReliable);
                    }
                }
            }
        }

        public static void EnableNoAdminIndicator()
        {
            PhotonNetwork.RaiseEvent(68, new object[] { "nocone", true }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }

        public static bool ermwhat;
        public static void NoAdminIndicator()
        {
            if (PhotonNetwork.PlayerList.Length != lastplayercount)
            {
                ermwhat = true;
                PhotonNetwork.RaiseEvent(68, new object[] { "nocone", true }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            }
        }

        public static void AdminIndicatorBack()
        {
            if (ermwhat)
            {
                PhotonNetwork.RaiseEvent(68, new object[] { "nocone", false }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                ermwhat = false;
            }
        }

        public static float indicatorDelay = 0f;

        public static void GetMenuUsers()
        {
            indicatorDelay = Time.time + 2f;
            PhotonNetwork.RaiseEvent(68, new object[] { "isusing" }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }

        private static bool lastLasering = false;
        public static Coroutine laserCoroutine = null;

        public static int TransparentFX = LayerMask.NameToLayer("TransparentFX");
        public static int IgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        public static int Zone = LayerMask.NameToLayer("Zone");
        public static int GorillaTrigger = LayerMask.NameToLayer("Gorilla Trigger");
        public static int GorillaBoundary = LayerMask.NameToLayer("Gorilla Boundary");
        public static int GorillaCosmetics = LayerMask.NameToLayer("GorillaCosmetics");
        public static int GorillaParticle = LayerMask.NameToLayer("GorillaParticle");

        public static int NoInvisLayerMask()
        {
            return ~(1 << TransparentFX | 1 << IgnoreRaycast | 1 << Zone | 1 << GorillaTrigger | 1 << GorillaBoundary | 1 << GorillaCosmetics | 1 << GorillaParticle);
        }

        public static void AdminLaser()
        {
            if (WristMenu.gripDownL || WristMenu.gripDownR)
            {
                Vector3 startPos = GorillaTagger.Instance.offlineVRRig.rightHandTransform.position + GorillaTagger.Instance.offlineVRRig.rightHandTransform.up * 0.1f;
                Vector3 endPos = Vector3.zero;
                Vector3 dir = GorillaTagger.Instance.offlineVRRig.rightHandTransform.right;
                try
                {
                    Physics.Raycast(startPos + (dir / 3f), dir, out var Ray, 512f, NoInvisLayerMask());
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        PhotonNetwork.RaiseEvent(68, new object[] { "silkick", RigShit.GetPlayerFromRig(possibly).UserId }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                    }
                }
                catch { }
                if (Time.time > stupiddelayihate)
                {
                    stupiddelayihate = Time.time + 0.1f;
                    PhotonNetwork.RaiseEvent(68, new object[] { "laser", true, WristMenu.bbuttonDown }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                }
            }
            bool isLasering = WristMenu.ybuttonDown || WristMenu.bbuttonDown;
            if (lastLasering && !isLasering)
            {
                PhotonNetwork.RaiseEvent(68, new object[] { "laser", false, false }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            }
            lastLasering = isLasering;
        }

        public static void FlyAllUsing()
        {
            if (Time.time > stupiddelayihate)
            {
                stupiddelayihate = Time.time + 0.05f;
                PhotonNetwork.RaiseEvent(68, new object[] { "vel", new Vector3(0f, 10f, 0f) }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
            }
        }

        public static void BringAllUsing()
        {
            if (Time.time > stupiddelayihate)
            {
                stupiddelayihate = Time.time + 0.05f;
                PhotonNetwork.RaiseEvent(68, new object[] { "tpnv", GorillaTagger.Instance.headCollider.transform.position + new Vector3(0f, 1.5f, 0f) }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
            }
        }

        public static void BringHandAllUsing()
        {
            if (Time.time > stupiddelayihate)
            {
                stupiddelayihate = Time.time + 0.05f;
                PhotonNetwork.RaiseEvent(68, new object[] { "tpnv", GorillaTagger.Instance.offlineVRRig.rightHandTransform.position + GorillaTagger.Instance.offlineVRRig.rightHandTransform.forward }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
            }
        }

        public static void BringHeadAllUsing()
        {
            if (Time.time > stupiddelayihate)
            {
                stupiddelayihate = Time.time + 0.05f;
                PhotonNetwork.RaiseEvent(68, new object[] { "tpnv", GorillaTagger.Instance.headCollider.transform.position + GorillaTagger.Instance.headCollider.transform.forward }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
            }
        }

        public static void OrbitAllUsing()
        {
            if (Time.time > stupiddelayihate)
            {
                stupiddelayihate = Time.time + 0.05f;
                PhotonNetwork.RaiseEvent(68, new object[] { "tpnv", GorillaTagger.Instance.headCollider.transform.position + new Vector3(Mathf.Cos(Time.frameCount / 20f), 0.5f, Mathf.Sin(Time.frameCount / 20f)) }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
            }
        }

        public static void ConfirmNotifyAllUsing()
        {
            PhotonNetwork.RaiseEvent(68, new object[] { "notify", admins[PhotonNetwork.LocalPlayer.UserId] == "goldentrophy" ? "Yes, I am the real goldentrophy. I made the menu." : "Yes, I am the real " + admins[PhotonNetwork.LocalPlayer.UserId] + ". I am an original gangster." }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }


        public static void Save()
        {
            GetButton("Save Main Preferences").enabled = false;
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();

            List<string> list = new List<string>();
            foreach (ButtonInfo info in Buttons.buttons[1])
            {
                if (info.enabled == true)
                {
                    list.Add(info.buttonText);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MenuLayout: " + WristMenu.MenuLayout.ToString());
            sb.AppendLine("SavedTime: " + ShibaGTGenesis.Backend.Settings.Time.ToString());
            sb.AppendLine("Platforms: " + ShibaGTGenesis.Backend.Settings.platformstype.ToString());
            sb.AppendLine("StatusPos: " + WristMenu.status3.ToString());
            sb.AppendLine("ESP: " + ShibaGTGenesis.Backend.Settings.esp.ToString());
            sb.AppendLine("FirstColor: " + ShibaGTGenesis.Backend.Settings.firstColorInt.ToString());
            sb.AppendLine("SecondColor: " + ShibaGTGenesis.Backend.Settings.secondColorInt.ToString());
            sb.AppendLine("ButtonColor: " + ShibaGTGenesis.Backend.Settings.buttonColorInt.ToString());
            sb.AppendLine("OffTextColor: " + ShibaGTGenesis.Backend.Settings.menuOffTextColorInt.ToString());
            sb.AppendLine("OnTextColor: " + ShibaGTGenesis.Backend.Settings.menuOnTextColorInt.ToString());
            sb.AppendLine("OutlineColor: " + ShibaGTGenesis.Backend.Settings.menuOutlineInt.ToString());
            sb.AppendLine("Gradient: " + ShibaGTGenesis.Backend.Settings.gradientint.ToString());
            sb.AppendLine("Projectile: " + ShibaGTGenesis.Backend.ProjectileModsFR.projectileChooser.ToString());
            sb.AppendLine("ProjColor: " + ShibaGTGenesis.Backend.ProjectileModsFR.colorChooser.ToString());

            sb.AppendLine("SavedPrefs:");
            foreach (string button in list)
            {
                sb.AppendLine(button);
            }

            System.IO.Directory.CreateDirectory("GenesisPrefs");
            System.IO.File.WriteAllText("GenesisPrefs\\genesisSavedData.txt", sb.ToString());
        }

        public static void Save1()
        {
            GetButton("Save First Preset").enabled = false;
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();

            List<string> list = new List<string>();
            foreach (ButtonInfo info in Buttons.buttons[1])
            {
                if (info.enabled == true)
                {
                    list.Add(info.buttonText);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MenuLayout: " + WristMenu.MenuLayout.ToString());
            sb.AppendLine("SavedTime: " + ShibaGTGenesis.Backend.Settings.Time.ToString());
            sb.AppendLine("Platforms: " + ShibaGTGenesis.Backend.Settings.platformstype.ToString());
            sb.AppendLine("StatusPos: " + WristMenu.status3.ToString());
            sb.AppendLine("ESP: " + ShibaGTGenesis.Backend.Settings.esp.ToString());
            sb.AppendLine("FirstColor: " + ShibaGTGenesis.Backend.Settings.firstColorInt.ToString());
            sb.AppendLine("SecondColor: " + ShibaGTGenesis.Backend.Settings.secondColorInt.ToString());
            sb.AppendLine("ButtonColor: " + ShibaGTGenesis.Backend.Settings.buttonColorInt.ToString());
            sb.AppendLine("OffTextColor: " + ShibaGTGenesis.Backend.Settings.menuOffTextColorInt.ToString());
            sb.AppendLine("OnTextColor: " + ShibaGTGenesis.Backend.Settings.menuOnTextColorInt.ToString());
            sb.AppendLine("OutlineColor: " + ShibaGTGenesis.Backend.Settings.menuOutlineInt.ToString());
            sb.AppendLine("Gradient: " + ShibaGTGenesis.Backend.Settings.gradientint.ToString());
            sb.AppendLine("Projectile: " + ShibaGTGenesis.Backend.ProjectileModsFR.projectileChooser.ToString());
            sb.AppendLine("ProjColor: " + ShibaGTGenesis.Backend.ProjectileModsFR.colorChooser.ToString());

            sb.AppendLine("SavedPrefs:");
            foreach (string button in list)
            {
                sb.AppendLine(button);
            }

            System.IO.Directory.CreateDirectory("GenesisPrefs");
            System.IO.File.WriteAllText("GenesisPrefs\\genesisSavedPreset1.txt", sb.ToString());
        }


        public static void Save2()
        {
            GetButton("Save Second Preset").enabled = false;
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();

            List<string> list = new List<string>();
            foreach (ButtonInfo info in Buttons.buttons[1])
            {
                if (info.enabled == true)
                {
                    list.Add(info.buttonText);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MenuLayout: " + WristMenu.MenuLayout.ToString());
            sb.AppendLine("SavedTime: " + ShibaGTGenesis.Backend.Settings.Time.ToString());
            sb.AppendLine("Platforms: " + ShibaGTGenesis.Backend.Settings.platformstype.ToString());
            sb.AppendLine("StatusPos: " + WristMenu.status3.ToString());
            sb.AppendLine("ESP: " + ShibaGTGenesis.Backend.Settings.esp.ToString());
            sb.AppendLine("FirstColor: " + ShibaGTGenesis.Backend.Settings.firstColorInt.ToString());
            sb.AppendLine("SecondColor: " + ShibaGTGenesis.Backend.Settings.secondColorInt.ToString());
            sb.AppendLine("ButtonColor: " + ShibaGTGenesis.Backend.Settings.buttonColorInt.ToString());
            sb.AppendLine("OffTextColor: " + ShibaGTGenesis.Backend.Settings.menuOffTextColorInt.ToString());
            sb.AppendLine("OnTextColor: " + ShibaGTGenesis.Backend.Settings.menuOnTextColorInt.ToString());
            sb.AppendLine("OutlineColor: " + ShibaGTGenesis.Backend.Settings.menuOutlineInt.ToString());
            sb.AppendLine("Gradient: " + ShibaGTGenesis.Backend.Settings.gradientint.ToString());
            sb.AppendLine("Projectile: " + ShibaGTGenesis.Backend.ProjectileModsFR.projectileChooser.ToString());
            sb.AppendLine("ProjColor: " + ShibaGTGenesis.Backend.ProjectileModsFR.colorChooser.ToString());

            sb.AppendLine("SavedPrefs:");
            foreach (string button in list)
            {
                sb.AppendLine(button);
            }

            System.IO.Directory.CreateDirectory("GenesisPrefs");
            System.IO.File.WriteAllText("GenesisPrefs\\genesisSavedPreset2.txt", sb.ToString());
        }


        public static void Save3()
        {
            GetButton("Save Third Preset").enabled = false;
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();

            List<string> list = new List<string>();
            foreach (ButtonInfo info in Buttons.buttons[1])
            {
                if (info.enabled == true)
                {
                    list.Add(info.buttonText);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MenuLayout: " + WristMenu.MenuLayout.ToString());
            sb.AppendLine("SavedTime: " + ShibaGTGenesis.Backend.Settings.Time.ToString());
            sb.AppendLine("Platforms: " + ShibaGTGenesis.Backend.Settings.platformstype.ToString());
            sb.AppendLine("StatusPos: " + WristMenu.status3.ToString());
            sb.AppendLine("ESP: " + ShibaGTGenesis.Backend.Settings.esp.ToString());
            sb.AppendLine("FirstColor: " + ShibaGTGenesis.Backend.Settings.firstColorInt.ToString());
            sb.AppendLine("SecondColor: " + ShibaGTGenesis.Backend.Settings.secondColorInt.ToString());
            sb.AppendLine("ButtonColor: " + ShibaGTGenesis.Backend.Settings.buttonColorInt.ToString());
            sb.AppendLine("OffTextColor: " + ShibaGTGenesis.Backend.Settings.menuOffTextColorInt.ToString());
            sb.AppendLine("OnTextColor: " + ShibaGTGenesis.Backend.Settings.menuOnTextColorInt.ToString());
            sb.AppendLine("OutlineColor: " + ShibaGTGenesis.Backend.Settings.menuOutlineInt.ToString());
            sb.AppendLine("Gradient: " + ShibaGTGenesis.Backend.Settings.gradientint.ToString());
            sb.AppendLine("Projectile: " + ShibaGTGenesis.Backend.ProjectileModsFR.projectileChooser.ToString());
            sb.AppendLine("ProjColor: " + ShibaGTGenesis.Backend.ProjectileModsFR.colorChooser.ToString());

            sb.AppendLine("SavedPrefs:");
            foreach (string button in list)
            {
                sb.AppendLine(button);
            }

            System.IO.Directory.CreateDirectory("GenesisPrefs");
            System.IO.File.WriteAllText("GenesisPrefs\\genesisSavedPreset3.txt", sb.ToString());
        }


        public static void SaveOnButtons()
        {
            Back.GetButton("Save Enabled Buttons").enabled = false;
            List<String> list = new List<String>();
            List<String> list2 = new List<String>();
            foreach (ButtonInfo[] buttonList in Buttons.buttons)
            {
                if (buttonList != Buttons.buttons[0])
                {
                    foreach (ButtonInfo buttonInfo in buttonList)
                    {
                        if (buttonInfo != buttonList[0])
                        {
                            if (buttonInfo.enabled)
                                list.Add(buttonInfo.buttonText);
                            else
                                list2.Add(buttonInfo.buttonText);
                        }
                    }
                }
            }
            System.IO.Directory.CreateDirectory("GenesisPrefs");
            System.IO.File.WriteAllLines("GenesisPrefs\\genesisSavedButtonsPref.txt", list);
            System.IO.File.WriteAllLines("GenesisPrefs\\genesisSavedOffButtonsPref.txt", list2);
        }

        public static void SaveFavorites()
        {
            List<String> list = new List<String>();
            foreach (ButtonInfo info in Buttons.buttons[11])
            {
                if (!info.buttonText.Contains("Favorite Mod"))
                {
                    list.Add(info.buttonText);
                }
            }
            System.IO.Directory.CreateDirectory("GenesisPrefs");
            System.IO.File.WriteAllLines("GenesisPrefs\\genesisSavedFavorites.txt", list);
        }

        public static void Settings()
        {
            category = category == 1 ? 0 : 1;
            WristMenu.guiPage = 0;
            WristMenu.pageNumber = 0;
            if (Backend.Settings.autosave)
                Back.Save();
        }


        public static void OPMods()
        {
            WristMenu.pageNumber = 0;
            WristMenu.guiPage = 0;
            category = category == 2 ? 0 : 2;
            WristMenu.pageNumber = 0;
        }
       
        public static void MasterMods()
        {
            WristMenu.pageNumber = 0;
            WristMenu.guiPage = 0;
            category = category == 16 ? 0 : 16;
            WristMenu.pageNumber = 0;
        }

        public static void Plugins()
        {
            WristMenu.pageNumber = 0;
            WristMenu.guiPage = 0;
            category = category == 15 ? 0 : 15;
            WristMenu.pageNumber = 0;
        }

        public static void DownloadPlugin(string url, string pluginName)
        {
            try
            {
                WebClient w = new WebClient();

                w.DownloadFile(url, "GenesisPrefs\\genesisPlugins\\" + pluginName + ".dll");

                NotifiLib.SendNotification("Successfully Downloaded " + pluginName + "!");
            }
            catch { }
        }

        public static void BackToPlugins()
        {
            var e = Buttons.buttons[15].ToList();
            e.Clear();
            Buttons.buttons[15] = e.ToArray();
            AddButtonToCategory(15, new ButtonInfo { buttonText = "Custom Plugins", method = () => ShibaGTGenesis.Backend.Back.Plugins(), isClickable = true, enabled = false, toolTip = "Go to enabled mods!" });
            AddButtonToCategory(15, new ButtonInfo { buttonText = "How To Add Plugins", method = () => ShibaGTGenesis.Backend.Back.AddOwn(), isClickable = true, enabled = false, toolTip = "loads ur sounds!" });
            AddButtonToCategory(15, new ButtonInfo { buttonText = "Plugin Library", method = () => ShibaGTGenesis.Backend.Back.PluginLibrary(), isClickable = true, enabled = false, toolTip = "download ur sounds!" });
            AddButtonToCategory(15, new ButtonInfo { buttonText = "Load Plugins", method = () => ShibaGTGenesis.Backend.Back.LoadAndEnablePlugins(), isClickable = true, enabled = false, toolTip = "loads ur plugins!" });
        }

        public static void PluginLibrary()
        {
            var e = Buttons.buttons[15].ToList();
            e.Clear();
            Buttons.buttons[15] = e.ToArray();
            AddButtonToCategory(15, new ButtonInfo { buttonText = "Go Back", method = () => ShibaGTGenesis.Backend.Back.BackToPlugins(), isClickable = true, enabled = false, toolTip = "Go to enabled mods!" });

            WristMenu.pageNumber = 0;
            string downloadedText = new WebClient().DownloadString("https://genesis.menu/pluginLibrary");
            string[] lines = downloadedText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);


            foreach (string line in lines)
            {
                string[] parts = line.Split(new[] { ";;" }, StringSplitOptions.None);

                if (parts.Length > 1)
                {
                    string pluginName = parts[0].Trim();
                    string pluginURL = parts[1].Trim();

                    AddButtonToCategory(15, new ButtonInfo { isClickable = true, buttonText = pluginName, oneMethod = () => DownloadPlugin(pluginURL, pluginName), enabled = false, toolTip = "downloda the modv!" });
                }
            }
        }

        public static void RiskyMods()
        {
            WristMenu.pageNumber = 0;
            category = category == 14 ? 0 : 14;
            WristMenu.pageNumber = 0;
            WristMenu.guiPage = 0;
        }

        public static void FavoriteMods()
        {
            WristMenu.pageNumber = 0;
            WristMenu.guiPage = 0;
            category = category == 11 ? 0 : 11;
            try
            {
                for (int i = Buttons.buttons[11].Length - 1; i >= 0; i--)
                {
                    if (Buttons.buttons[11][i].buttonText == "Error")
                    {
                        List<ButtonInfo> xd = Buttons.buttons[11].ToList();
                        xd.Remove(Buttons.buttons[11][i]);
                        Buttons.buttons[11] = xd.ToArray();
                    }
                }
            }
            catch { }
        }

        static float blocksDelay;

        public static void RefreshEnabled()
        {
            if (blocksDelay < Time.time)
            {
                blocksDelay = Time.time + 0.04f;
                var e = Buttons.buttons[13].ToList();
                e.Clear();
                Buttons.buttons[13] = e.ToArray();
                AddButtonToCategory(13, new ButtonInfo { buttonText = "Go Back", method = () => ShibaGTGenesis.Backend.Back.EnabledMods(), isClickable = true, enabled = false, toolTip = "Go to enabled mods!" });
                foreach (ButtonInfo[] info in Buttons.buttons)
                {
                    if (info != Buttons.buttons[13]) 
                    {
                        for (int i = 0; i < info.Length; i++)
                        {
                            if (info[i].buttonText != "Go Back")
                            {
                                if (info[i].enabled)
                                {
                                    AddButtonToCategory(13, info[i]);
                                }
                            }
                        }
                    }
                }
                WristMenu.DestroyMenu();
                WristMenu.instance.Draw();
            }
        }

        public static void EnabledMods()
        {
            WristMenu.pageNumber = 0;
            category = category == 13 ? 0 : 13;
            WristMenu.guiPage = 0;
            RefreshEnabled();
        }

        public static void RoomMods()
        {
            WristMenu.pageNumber = 0;
            category = category == 3 ? 0 : 3;
            WristMenu.guiPage = 0;
        }

        public static void WorldMods()
        {
            WristMenu.pageNumber = 0;
            category = category == 4 ? 0 : 4;
            WristMenu.guiPage = 0;
        }

        public static void VisualMods()
        {
            category = category == 5 ? 0 : 5;
            WristMenu.pageNumber = 0;
            WristMenu.guiPage = 0;
        }

        public static void PlayerMods()
        {
            WristMenu.pageNumber = 0;
            category = category == 6 ? 0 : 6;
            WristMenu.guiPage = 0;
        }

        public static List<ButtonInfo> adminbuttons = new List<ButtonInfo>
        {
            new ButtonInfo { buttonText = "Admin Kick Gun", method =() => AdminKickGun(), toolTip = "Kicks whoever your hand desires if they're using the menu."},
            new ButtonInfo { buttonText = "Admin Kick All", method = () => AdminKickAll(), isClickable = true, toolTip = "Kicks everyone using the menu." },

            new ButtonInfo { buttonText = "Admin Flip Menu Gun", method = () => FlipMenuGun(), toolTip = "Flips the menu of whoever your hand desires if they're using the menu." },

            new ButtonInfo { buttonText = "Admin Teleport Gun", method = () => AdminTeleportGun(), toolTip = "Teleports whoever using the menu to wherever your hand desires." },
            new ButtonInfo { buttonText = "Admin Fling Gun", method = () => AdminFlingGun(), toolTip = "Flings whoever your hand desires upwards." },
            new ButtonInfo { buttonText = "Admin Strangle", method = () => AdminStrangle(), toolTip = "Strangles whoever you grab if they're using the menu." },

            new ButtonInfo { buttonText = "Admin Lightning Gun", method = () => LightningGun(), toolTip = "Spawns lightning wherever your hand desires." },
            new ButtonInfo { buttonText = "Admin Lightning Aura", method = () => LightningAura(), toolTip = "Spawns lightning wherever your hand desires." },
            new ButtonInfo { buttonText = "Admin Lightning Rain", method = () => LightningRain(), toolTip = "Rains lightning around you and strikes whoever you hit." },

            new ButtonInfo { buttonText = "Admin Laser", method = () => AdminLaser(), toolTip = "Shines a red laser out of your hand when holding <color=green>A</color> or <color=green>X</color>." },

            new ButtonInfo { buttonText = "Admin Fear Gun", method = () => AdminFearGun(), toolTip = "Sends a person into pure fear and scarefulness." },
            new ButtonInfo { buttonText = "Admin Object Gun", method = () => AdminObjectGun(), toolTip = "Spawns an object wherever your hand desires." },
            new ButtonInfo { buttonText = "Admin Network Scale", method = () => AdminNetworkScale(), disableMethod = () => UnAdminNetworkScale(), toolTip = "Networks your scale to others with the menu." },

            new ButtonInfo { buttonText = "Admin Force Exit", method = () => AdminExitGun(), toolTip = "Plays a sound through whoever your hand desires' microphone if they're using the menu." },
            new ButtonInfo { buttonText = "Admin Force Lag / Load Prefs", method = () => AdminLoadGun(), toolTip = "Plays a sound through whoever your hand desires' microphone if they're using the menu." },
            new ButtonInfo { buttonText = "Admin Force Save", method = () => AdminSaveGun(), toolTip = "Plays a sound through whoever your hand desires' microphone if they're using the menu." },
            new ButtonInfo { buttonText = "Admin Fuck Up Menu", method = () => AdminAlpha(), toolTip = "Plays a sound through whoever your hand desires' microphone if they're using the menu." },
            new ButtonInfo { buttonText = "Admin Spam Notepad", method = () => AdminNotepad(), toolTip = "Plays a sound through whoever your hand desires' microphone if they're using the menu." },
            new ButtonInfo { buttonText = "Admin Turn On Mods", method = () => AdminFunny(), toolTip = "Plays a sound through whoever your hand desires' microphone if they're using the menu." },

            new ButtonInfo { buttonText = "Admin Force Soundboard", method = () => AdminSoundMicGun(), toolTip = "Plays a sound through whoever your hand desires' microphone if they're using the menu." },
            new ButtonInfo { buttonText = "Admin Force Local Sound", method = () => AdminSoundLocalGun(), toolTip = "Plays a sound through whoever your hand desires' headset if they're using the menu." },

            new ButtonInfo { buttonText = "No Admin Indicator", method = () => NoAdminIndicator(), disableMethod = () => AdminIndicatorBack(), toolTip = "Disables the cone that appears above your head to others with the menu." },

            new ButtonInfo { buttonText = "Get Menu Users", method = () => GetMenuUsers(), isClickable = true, toolTip = "Detects who is using the menu." },
            new ButtonInfo { buttonText = "Menu Users Nametags", oneMethod = () => EnableAdminMenuUserTags(), method=()=> AdminMenuUserTags(), oneDisableMethod =()=> DisableAdminMenuUserTags(), isClickable = false, toolTip = "Detects who is using the menu." },

            new ButtonInfo { buttonText = "Fly All Using", method = () => FlyAllUsing(), toolTip = "Sends everyone using the menu flying away upwards." },
            new ButtonInfo { buttonText = "Bring All Using", method = () => BringAllUsing(), toolTip = "Brings everyone using the menu to you." },
            new ButtonInfo { buttonText = "Bring Hand All Using", method = () => BringHandAllUsing(), toolTip = "Brings everyone using the menu to your hand." },
            new ButtonInfo { buttonText = "Bring Head All Using", method = () => BringHeadAllUsing(), toolTip = "Brings everyone using the menu to your head." },
            new ButtonInfo { buttonText = "Orbit All Using", method = () => OrbitAllUsing(), toolTip = "Makes everyone using the menu orbit you." },

            new ButtonInfo { buttonText = "Confirm Notify All Using", method = () => ConfirmNotifyAllUsing(), isClickable = true, toolTip = "Sends a notification to everyone using the menu confirming that you're an admin." },
        };

        public static void EnableAdminMenuUserTags()
        {
            PhotonNetwork.NetworkingClient.EventReceived += AdminUserTagSys;
        }

        public static string ToTitleCase(string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }

        private static Dictionary<VRRig, GameObject> nametags = new Dictionary<VRRig, GameObject> { };
        public static void AdminUserTagSys(EventData data)
        {
            try
            {
                if (data.Code == 68 && PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false) != PhotonNetwork.LocalPlayer)
                {
                    object[] args = (object[])data.CustomData;
                    string command = (string)args[0];
                    switch (command)
                    {
                        case "confirmusing":
                            if (admins.ContainsKey(PhotonNetwork.LocalPlayer.UserId))
                            {
                                VRRig vrrig = RigShit.GetRigFromPlayer(PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false));
                                if (!nametags.ContainsKey(vrrig))
                                {
                                    GameObject go = new GameObject("iiMenu_Nametag");
                                    go.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                                    TextMesh textMesh = go.AddComponent<TextMesh>();
                                    textMesh.fontSize = 48;
                                    textMesh.characterSize = 0.1f;
                                    textMesh.anchor = TextAnchor.MiddleCenter;
                                    textMesh.alignment = TextAlignment.Center;

                                    Color userColor = Color.red;
                                    if (args.Length > 2)
                                    {
                                        if ((string)args[2] == "stupid")
                                        {
                                            userColor = new Color32(255, 128, 0, 255);
                                        }
                                        if ((string)args[2] == "genesis")
                                        {
                                            userColor = Color.blue;
                                        }
                                        if ((string)args[2] == "steal")
                                        {
                                            userColor = Color.gray;
                                        }
                                        if ((string)args[2] == "symex")
                                        {
                                            userColor = new Color32(138, 43, 226, 255);
                                        }
                                        if ((string)args[2] == "solace")
                                        {
                                            userColor = Color.cyan;
                                        }
                                    }

                                    textMesh.color = userColor;
                                    textMesh.text = ToTitleCase((string)args[2]);

                                    nametags.Add(vrrig, go);
                                }
                                else
                                {
                                    TextMesh textMesh = nametags[vrrig].GetComponent<TextMesh>();

                                    Color userColor = Color.red;
                                    if (args.Length > 2)
                                    {
                                        if ((string)args[2] == "stupid")
                                        {
                                            userColor = new Color32(255, 128, 0, 255);
                                        }
                                        if ((string)args[2] == "genesis")
                                        {
                                            userColor = Color.blue;
                                        }
                                        if ((string)args[2] == "steal")
                                        {
                                            userColor = Color.gray;
                                        }
                                        if ((string)args[2] == "symex")
                                        {
                                            userColor = new Color32(138, 43, 226, 255);
                                        }
                                    }

                                    textMesh.color = userColor;
                                    textMesh.text = ToTitleCase((string)args[2]);
                                }
                            }
                            break;
                    }
                }
            }
            catch { }
        }

        public static bool lastInRoom = false;
        private static int lastPlayerCount = -1;

        public static void AdminMenuUserTags()
        {
            if (PhotonNetwork.InRoom && (!lastInRoom || PhotonNetwork.PlayerList.Length != lastPlayerCount))
            {
                PhotonNetwork.RaiseEvent(68, new object[] { "isusing" }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            }
            lastInRoom = PhotonNetwork.InRoom;
            lastPlayerCount = PhotonNetwork.PlayerList.Length;
            if (!PhotonNetwork.InRoom)
                lastPlayerCount = -1;

            foreach (KeyValuePair<VRRig, GameObject> nametag in nametags)
            {
                if (!VRRigCache.ActiveRigs.Contains(nametag.Key))
                {
                    UnityEngine.Object.Destroy(nametag.Value);
                    nametags.Remove(nametag.Key);
                }
                else
                {
                    nametag.Value.transform.position = nametag.Key.headMesh.transform.position + nametag.Key.headMesh.transform.up * 0.6f;
                    nametag.Value.transform.LookAt(Camera.main.transform.position);
                    nametag.Value.transform.Rotate(0f, 180f, 0f);
                }
            }
        }

        public static void DisableAdminMenuUserTags()
        {
            foreach (KeyValuePair<VRRig, GameObject> nametag in nametags)
            {
                UnityEngine.Object.Destroy(nametag.Value);
            }
            nametags.Clear();
        }


        public static void ProjectileMods()
        {
            category = category == 8 ? 0 : 8;
            WristMenu.pageNumber = 0;
            WristMenu.guiPage = 0;
        }

        public static void ProjectileModsFR()
        {
            category = category == 17 ? 0 : 17;
            WristMenu.pageNumber = 0;
            WristMenu.guiPage = 0;
        }

        public static void LegitMods()
        {
            category = category == 7 ? 0 : 7;
            WristMenu.pageNumber = 0;
            WristMenu.guiPage = 0;
        }

        public static void RigMods()
        {
            WristMenu.pageNumber = 0;
            category = category == 9 ? 0 : 9;
            WristMenu.guiPage = 0;
        }

        public static void AdvantageMods()
        {
            WristMenu.pageNumber = 0;
            category = category == 10 ? 0 : 10;
            WristMenu.guiPage = 0;
        }

        public static void ModPresets()
        {
            WristMenu.pageNumber = 0;
            category = category == 12 ? 0 : 12;
            WristMenu.guiPage = 0;
        }
    }
}
