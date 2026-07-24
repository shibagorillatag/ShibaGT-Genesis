
using BepInEx;
using Genesis.UI;
using GenesisGUILibrary;
using GorillaNetworking;
using Photon.Pun;
using ShibaGTGenesis.Backend;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace ShibaGTGenesis.UI
{
    public class NewGUI : MonoBehaviour
    {
        // hey shiba welcome to the gui, looks a little messy (code wise) but as updates come i will fix alot and add a bunch for the the lib for ease of use.
        private bool ShowGUI = true;
        public static int _tabCurrent;
        public static int _windowID;
        public static List<Particle> _particles = new List<Particle>();
        public static Rect _windowRect = new Rect(100, 100, 730, 400);
        public static float Alpha = 240;
        public static Color windowBgColor = new Color32(10, 10, 10, 240);
        public static Color tabBarColor = new Color32(20, 20, 20, 255);
        public static Color tabHoverColor = new Color32(40, 40, 40, 255);
        public static Color tabActiveColor = new Color32(50, 50, 50, 255);
        public static Color tabActiveBarColor = new Color32(0, 0, 1, 255);
        public static Color cheatBoxColor = new Color32(30, 30, 30, 255);
        public static Color cheatBoxSeperatorColor = new Color32(0, 0, 205, 255);
        public static Color toggleBackgroundColor = new Color32(40, 40, 40, 255);
        public static Color toggleHoverColor = new Color32(70, 70, 70, 85);
        public static Color toggleActiveColor = new Color32(0, 0, 205, 255);
        public static Color buttonNormalColor = new Color32(40, 40, 40, 255);
        public static Color buttonHoverColor = new Color32(47, 47, 47, 255);
        public static Color buttonActiveColor = new Color32(85, 85, 85, 255);
        public static Color sliderBarColor = new Color32(0, 0, 205, 255);
        public static Color sliderGrabColor = Color.white;
        public static Color particlecolour = Color.blue;
        private Vector2 _scrollPosition;
        private bool IsLoading = true;
        private string _userInput = "";

        private string r = "0";
        private string g = "0";
        private string b = "0";

        private float LoadingPROGRESS = 0f;

        public class Particle
        {
            public Rect position = default;
            public float xDir = 0f;
            public float yDir = 0f;
        }


        public static void DrawWatermarkBox(Rect rect)
        {
            GUI.color = new Color(0, 0, 0, 0.8f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            DrawOutline(rect);
            GUIStyle watermarkStyle = new GUIStyle();
            watermarkStyle.normal.textColor = Color.white;
            watermarkStyle.fontSize = 18;
            watermarkStyle.fontStyle = FontStyle.Normal;
            watermarkStyle.alignment = TextAnchor.MiddleCenter;
            Rect textRect = new Rect(rect.x + 10, rect.y + 10, rect.width - 20, rect.height - 20);
        }
        public static void DrawOutline(Rect rect)
        {
            float lerpFactor = Mathf.PingPong(Time.time / 1f, 1f);
            Color lerpedColor = Color.Lerp(WristMenu.colorToFade1, WristMenu.colorToFade2, lerpFactor);
            Color rainbowColor = UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f);
            Color32 customColor = lerpedColor;
            if (Settings.rainbow)
                customColor = rainbowColor;

            GUI.color = customColor;
            float outlineThickness = 2f;
            Rect topBorder = new Rect(rect.xMin, rect.yMin, rect.width, outlineThickness);
            Rect bottomBorder = new Rect(rect.xMin, rect.yMax - outlineThickness, rect.width, outlineThickness);
            Rect leftBorder = new Rect(rect.xMin, rect.yMin, outlineThickness, rect.height);
            Rect rightBorder = new Rect(rect.xMax - outlineThickness, rect.yMin, outlineThickness, rect.height);
            GUI.DrawTexture(topBorder, Texture2D.whiteTexture);
            GUI.DrawTexture(bottomBorder, Texture2D.whiteTexture);
            GUI.DrawTexture(leftBorder, Texture2D.whiteTexture);
            GUI.DrawTexture(rightBorder, Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        private GUIStyle GetTextStyle()
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 14;
            style.fontStyle = FontStyle.Normal;
            return style;
        }

        public static int sec;
        public static int min;
        public static int hou;
        

        public static void SetTime()
        {
            sec++;
            if (sec == 60)
            {
                min++;
                sec = 0;
            }

            if (min == 60)
            {
                hou++;
                min = 0;
            }
        }

        public static float ez;
        static bool kickBool;
        static bool pastPref;

        public static bool kickSet;

        static bool playBool;
        public static bool playSet = false;

        public static string kickString = "Lobby Code Here";
        public static string playString = "Part of the sound name here!\nEx: 'get out'";
        public static string customString = "Emojis number here\nEx: '5'";

        private static List<string> enabled = new List<string>();

        public static string emoji;
        public static bool customBool;
        public static bool emojiSet;
        public static bool doGUIAnim;
        public static bool startAnimDecent;

        static Texture image;

        private static GameObject audiomgr = null;

        // Migrated to newer systems by crimson
        public static AudioClip LoadWav(byte[] wavFile)
        {
            int channels = BitConverter.ToInt16(wavFile, 22);
            int sampleRate = BitConverter.ToInt32(wavFile, 24);

            int dataChunkOffset = 12;
            int dataSize = 0;
            while (dataChunkOffset < wavFile.Length)
            {
                string chunkID = System.Text.Encoding.ASCII.GetString(wavFile, dataChunkOffset, 4);
                int chunkSize = BitConverter.ToInt32(wavFile, dataChunkOffset + 4);

                if (chunkID == "data")
                {
                    dataSize = chunkSize;
                    dataChunkOffset += 8;
                    break;
                }
                dataChunkOffset += 8 + chunkSize;
            }

            int sampleCount = dataSize / 2;
            float[] audioData = new float[sampleCount];

            int offset = 0;
            for (int i = 0; i < sampleCount; i++)
            {
                short sample = BitConverter.ToInt16(wavFile, dataChunkOffset + i * 2);
                audioData[offset++] = sample / 32768f;
            }

            AudioClip clip = AudioClip.Create("EmbeddedClip", sampleCount / channels, channels, sampleRate, false);
            clip.SetData(audioData, 0);
            return clip;
        }


        public static AudioClip LoadSoundFromResource(string resourceName)
        {
            AudioClip sound = null;

            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    Debug.LogError($"Failed to load resource stream for '{resourceName}'.");
                    return null;
                }

                byte[] wavData = new byte[stream.Length];
                stream.Read(wavData, 0, wavData.Length);
                sound = LoadWav(wavData);
            }


            return sound;
        }

        public static async void Anim()
        {
            image = LegacyGUI.DownloadImage("https://genesis.menu/designAttempts/dotell.png");
            await Task.Delay(1500);


            if (audiomgr == null)
            {
                audiomgr = new GameObject("2DAudioMgr");
                AudioSource temp = audiomgr.AddComponent<AudioSource>();
                temp.spatialBlend = 0f;
            }
            AudioSource ausrc = audiomgr.GetComponent<AudioSource>();
            ausrc.volume = 1f;
            ausrc.PlayOneShot(LoadSoundFromResource("ShibaGTGenesis.AssetBundles.startup.wav"));


            doGUIAnim = true;
            await Task.Delay(3000);
            startAnimDecent = true;
            await Task.Delay(3000);
            doGUIAnim = false;
        }

        static float animOpacity = 0f;

        public void OnGUI()
        {
            if (doGUIAnim)
            {
                if (animOpacity >= 100)
                {
                    animOpacity = 100;
                }
                else
                {
                    if (!startAnimDecent)
                        animOpacity += 0.3f;
                }

                if (startAnimDecent)
                {
                    if (animOpacity >= 0.5f) //if not 0
                        animOpacity -= 0.3f;
                    else
                        animOpacity = 0f;
                }

                Color guiColor = GUI.color;
                guiColor.a = Mathf.Clamp(animOpacity, 1f, 100f) / 100f;
                GUI.color = guiColor;

                float centerX = (Screen.width - 512) / 2;
                float centerY = (Screen.height - 512) / 2;
                GUI.DrawTexture(new Rect(centerX, centerY, 512, 512), image);

                GUI.color = Color.white;
                return;

            }

            if (Visual.visiblefps)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);
                style.fontSize = 25;
                GUI.Label(new Rect(0, Screen.height - 30, 200, 100), $" FPS: {Mathf.RoundToInt(1.0f / Time.deltaTime)}", style);
            }

            if (_toggle1)
            {
                LegacyGUI.Keyboarding();
            }
            //arraylist sys
            if (GorillaTagger.Instance.offlineVRRig.enabled == false)
            {
                GUIStyle style2 = new GUIStyle(GUI.skin.label);
                style2.fontSize = 25;
                GUI.Label(new Rect(0, Screen.height - 30, 200, 100), "In Ghost Mode", style2);
            }
            if (Back.GetButton("Arraylist").enabled == true)
            {
                try
                {
                    enabled.Clear();
                    try
                    {
                        foreach (ButtonInfo[] buttonList in Buttons.buttons)
                        {
                            if (buttonList != Buttons.buttons[13])
                            {
                                foreach (ButtonInfo buttonInfo in buttonList)
                                {
                                    if (buttonInfo.enabled)
                                        enabled.Add(buttonInfo.buttonText);
                                }
                            }
                        }
                    }
                    catch { }

                    GUIStyle originalLabelStyle = new GUIStyle(GUI.skin.label);
                    enabled.Sort((a, b) =>
                    {
                        return GUI.skin.label.CalcSize(new GUIContent(a)).x
                            .CompareTo(GUI.skin.label.CalcSize(new GUIContent(b)).x);
                    });
                    enabled.Reverse();

                    float yPos = 0f;
                    float maxYPos = 0f;
                    Texture2D gradientTexture = null;

                    for (int i = 0; i < enabled.Count; i++)
                    {
                        var textSize22 = GUI.skin.label.CalcSize(new GUIContent(enabled[i]));
                        float boxWidth22 = textSize22.x;
                        float boxHeight22 = 30f;
                        float gradientFactor = Mathf.Sin(Time.time * 2f + i * 0.5f) * 0.5f + 0.5f;
                        Color textColor22 = Color.Lerp(WristMenu.colorToFade1, WristMenu.colorToFade2, gradientFactor);
                        if (Settings.rainbow)
                            textColor22 = Color.Lerp(UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f), MakeDarker(UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f), 0.3f), gradientFactor);

                        Rect backgroundRect22 = new Rect(Screen.width - (boxWidth22 + 7f), yPos, boxWidth22 + 7f, boxHeight22);
                        GUI.DrawTexture(backgroundRect22, lib.ArrayListBG);

                        GUI.skin.label.fontSize = 18;
                        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                        GUIStyle textStyle22 = new GUIStyle(GUI.skin.label)
                        {
                            fontSize = 16,
                            fontStyle = FontStyle.Bold,
                            normal = { textColor = textColor22 },
                            alignment = TextAnchor.MiddleCenter
                        };
                        GUI.Label(new Rect(Screen.width - (textSize22.x + 15f), yPos, textSize22.x + 15f, boxHeight22), enabled[i], textStyle22);

                        yPos += boxHeight22;
                        maxYPos = Mathf.Max(maxYPos, yPos);
                    }

                    float lineGradientFactor = Mathf.Sin(Time.time * 2f) * 0.5f + 0.5f;
                    Color lineStartColor = Color.Lerp(WristMenu.colorToFade1, WristMenu.colorToFade2, lineGradientFactor);
                    Color lineEndColor = Color.Lerp(WristMenu.colorToFade1, WristMenu.colorToFade2, lineGradientFactor + 0.5f);
                    if (Settings.rainbow)
                    {
                        lineStartColor = Color.Lerp(UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f), MakeDarker(UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f), 0.3f), lineGradientFactor);
                        lineEndColor = Color.Lerp(UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f), MakeDarker(UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f), 0.3f), lineGradientFactor + 0.5f);
                    }
                    if (gradientTexture == null || gradientTexture.height != maxYPos)
                    {
                        gradientTexture = CreateGradientTexture(lineStartColor, lineEndColor, 5, (int)maxYPos);
                    }
                    Rect verticalLineRect = new Rect(Screen.width - 5f, 0f, 5f, maxYPos);
                    GUI.DrawTexture(verticalLineRect, gradientTexture);

                    GUI.skin.label = originalLabelStyle;

                    GUI.color = Color.white;
                }
                catch (Exception e) { }
            }


            if (ShibaGTGenesis.Backend.Room.kickOn)
            {
                if (!kickBool)
                {
                    pastPref = isopen1;
                    isopen1 = false;
                    kickBool = true;
                }
                lib.DrawWindowBG(new Rect(500, 500, 100, 100), 10);
                kickString = lib.DrawTextField(new Rect(500, 520, 100, 35), kickString, Color.white, 12, FontStyle.Bold, Color.black);
                if (lib.Button(new Rect(500, 570, 100, 25), "Submit", 5))
                {
                    kickString = kickString.ToUpper();
                    kickSet = true;
                }
            }
            else
            {
                if (kickBool)
                {
                    isopen1 = pastPref;
                    kickBool = false;
                }
            }

            if (ProjectileMods.playOn)
            {
                if (!playBool)
                {
                    pastPref = isopen1;
                    isopen1 = false;
                    playBool = true;
                }
                lib.DrawWindowBG(new Rect(500, 500, 100, 100), 10);
                playString = lib.DrawTextField(new Rect(500, 520, 100, 35), playString, Color.white, 12, FontStyle.Bold, Color.black);
                if (lib.Button(new Rect(500, 570, 100, 25), "Submit", 5))
                {
                    playString = playString.ToLower();
                    playSet = true;
                }
            }
            else
            {
                if (playBool)
                {
                    isopen1 = pastPref;
                    playBool = false;
                }
            }


            if (ez < Time.time)
            {
                ez = Time.time + 1f;
                SetTime();
            }
            if (LegacyGUI.watermark)
            {
                string text1 = $"{WristMenu.MenuTitle} | {hou}h:{min}m:{sec}s | FPS: {Mathf.RoundToInt(1.0f / Time.deltaTime)}";

                GUIStyle textStyle = GetTextStyle();
                Vector2 textSize = textStyle.CalcSize(new GUIContent(text1));
                float sidePadding = 10f;
                float verticalPadding = 20f;
                float boxWidth = textSize.x + sidePadding * 2;
                float boxHeight = textSize.y + verticalPadding;
                float xPosition = (Screen.width - boxWidth) / 2;
                float yPosition = 14f;
                if (GUI.Button(new Rect(xPosition, yPosition, boxWidth, boxHeight), ""))
                {
                    LegacyGUI.isopen3 = !LegacyGUI.isopen3;
                    StartCoroutine(LegacyGUI.MoveUI(LegacyGUI.isopen3));
                }

                DrawWatermarkBox(new Rect(xPosition, yPosition, boxWidth, boxHeight));
                GUI.Label(new Rect(xPosition + (boxWidth - textSize.x) / 2, yPosition + (boxHeight - textSize.y) / 2, textSize.x, textSize.y), text1, textStyle);
            }

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

            if (!isopen1)
                return;

            if (Settings.isLegacyUI)
                return;

            if (IsLoading)
            {
                LoadingPROGRESS += Time.deltaTime / 8f;
                if (LoadingPROGRESS > 1f)
                {
                    LoadingPROGRESS = 1f;
                    IsLoading = false;
                }
                int percentage = Mathf.RoundToInt(LoadingPROGRESS * 100f);

                lib.DrawText(new Rect((Screen.width - 200f) / 2f, (Screen.height - 25f) / 2f - 36f, 200f, 30f), "Loading Genesis GUI...", Color.white, 18, FontStyle.Bold);
                lib.DrawText(new Rect((Screen.width - 200f) / 2f + 70f, (Screen.height - 25f) / 2f - 55f, 200f, 30f), $"({percentage}%)", Color.white, 16, FontStyle.Bold);

                lib.DrawWindowBG(new Rect((Screen.width - 212f) / 2f, (Screen.height - 37f) / 2f, 210f, 35f), 10);
                lib.DrawLoadingBar(new Rect((Screen.width - 200f) / 2f, (Screen.height - 25f) / 2f, 200f * LoadingPROGRESS, 25f));
            }
            else if (ShowGUI)
            {
                _windowRect = GUI.Window(_windowID, _windowRect, GenesisGUI, "", new GUIStyle());
            }
        }

        public static Color MakeDarker(Color color, float darkenAmount = 0.1f)
        {
            darkenAmount = Mathf.Clamp01(darkenAmount);

            float r = Mathf.Clamp01(color.r * (1 - darkenAmount));
            float g = Mathf.Clamp01(color.g * (1 - darkenAmount));
            float b = Mathf.Clamp01(color.b * (1 - darkenAmount));

            return new Color(r, g, b, color.a);
        }

        public static string _searchText = "Search..";

        private void GenesisGUI(int windowID)
        {
            GUI.depth = -1;
            ApplyColorsToGUI();
            lib.DrawWindowBG(new Rect(0, 0, _windowRect.width, _windowRect.height), 8);
            lib.DrawTabBar(new Rect(0, 0, 120f, _windowRect.height), 8);
            //DrawRoundedTex2(new Rect(120f, 0f, 0.75f, _windowRect.height), Color.grey, 0);
            GUI.depth = 1;
            for (int i = 0; i < _particles.Count; i++)
            {
                var xPos = _particles[i].position.x + _particles[i].xDir;
                var yPos = _particles[i].position.y + _particles[i].yDir;
                _particles[i].position = new Rect(xPos, yPos, _particles[i].position.width, _particles[i].position.height);
                if (_particles[i].position.y > _windowRect.height)
                {
                    var size = new System.Random().Next(2, 5);
                    var xLol = Random.Range(-1f, 1f);
                    var yLol = Random.Range(0, 1f);
                    _particles[i] = new Particle()
                    {
                        position = new Rect(new Rect(Random.Range(0, (int)_windowRect.width), 0, size, size)),
                        xDir = xLol,
                        yDir = yLol
                    };
                }
                GUI.DrawTexture(_particles[i].position, Texture2D.whiteTexture, ScaleMode.StretchToFill, false, 0f, particlecolour, 0f, 50f);
            }

            Color textcolor;
            if (Settings.rainbow)
                textcolor = UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f);
            else
                textcolor = WristMenu.menuOnTextColor;
            lib.DrawText(new Rect(20, _windowRect.height - 48, 80, 30), "Genesis", textcolor, 20, FontStyle.Bold);
            lib.DrawText(new Rect(10, _windowRect.height - 62, 500, 30), "Insert To Close", textcolor, 14, FontStyle.Bold);
            lib.DrawText(new Rect(22, _windowRect.height - 28, 80, 30), $"FPS: {Mathf.RoundToInt(1.0f / Time.deltaTime)}", textcolor, 18, FontStyle.Bold);
            lib.Tab(new Rect(0, 30, 120, 35), 1, ref _tabCurrent, "Modules");
            lib.Tab(new Rect(0, 65, 120, 35), 2, ref _tabCurrent, "Computer");
            lib.Tab(new Rect(0, 100, 120, 35), 3, ref _tabCurrent, "Room Info");
            lib.Tab(new Rect(0, 135, 120, 35), 4, ref _tabCurrent, "Emulators");
            if (_tabCurrent == 1)
            {
                // lib.DrawText(new Rect((Screen.width - 200f) / 2f, 10f, 200f, 30f), "This Area Is Scrollable.", new Color32(100, 100, 100, 255), 18, FontStyle.Bold);
                Rect scrollViewRect = new Rect(2f, -3f, 720f + 20f, 600f - 40f); // Adjusted width to include the scrollbar
                _scrollPosition = GUI.BeginScrollView(scrollViewRect, _scrollPosition, new Rect(0f, 0f, scrollViewRect.width - 40f, 2000f), false, false); // Adjusted content width to exclude the scrollbar
                float boxWidth = 173f;
                float boxHeight = 60f;
                float spacing = 15f;
                float startX = 144f;
                float startY = 70f;
                int buttonNum = -1;

                _searchText = lib.DrawTextField(new Rect(144, 20, _windowRect.width - 180, 30), _searchText, Color.white, 19, FontStyle.Bold, GUI.color);

                

                if (_searchText != "Search.." && _searchText != "")
                {
                    foreach (ButtonInfo[] buttoninfo in Buttons.buttons)
                    {
                        for (int i = buttoninfo.Length - 1; i >= 0; i--)
                        {
                            if (buttoninfo.AsEnumerable().Reverse().ToList()[i].buttonText.ToLower().Contains(_searchText.ToLower()))
                            {
                                buttonNum++;
                                if (buttonNum == 3)
                                {
                                    startY += boxHeight + spacing;
                                    buttonNum = 0;
                                }

                                ButtonInfo info = buttoninfo.AsEnumerable().Reverse().ToList()[i];
                                if (buttonNum == 0)
                                    DrawCheatBox(new Rect(startX, startY, boxWidth, boxHeight), "<b>" + info.buttonText + "</b>", ref info.enabled, info.toolTip);
                                if (buttonNum == 1)
                                    DrawCheatBox(new Rect(startX + boxWidth + spacing, startY, boxWidth, boxHeight), "<b>" + info.buttonText + "</b>", ref info.enabled, info.toolTip);
                                if (buttonNum == 2)
                                    DrawCheatBox(new Rect(startX + 2 * (boxWidth + spacing), startY, boxWidth, boxHeight), "<b>" + info.buttonText + "</b>", ref info.enabled, info.toolTip);
                            }
                        }
                    }
                }
                else
                {
                    ButtonInfo[] buttoninfo = Buttons.buttons[Back.category];
                    for (int i = buttoninfo.Length - 1; i >= 0; i--)
                    {
                        buttonNum++;
                        if (buttonNum == 3)
                        {
                            startY += boxHeight + spacing;
                            buttonNum = 0;
                        }

                        ButtonInfo info = buttoninfo.AsEnumerable().Reverse().ToList()[i];
                        if (buttonNum == 0)
                            DrawCheatBox(new Rect(startX, startY, boxWidth, boxHeight), "<b>" + info.buttonText + "</b>", ref info.enabled, info.toolTip);
                        if (buttonNum == 1)
                            DrawCheatBox(new Rect(startX + boxWidth + spacing, startY, boxWidth, boxHeight), "<b>" + info.buttonText + "</b>", ref info.enabled, info.toolTip);
                        if (buttonNum == 2)
                            DrawCheatBox(new Rect(startX + 2 * (boxWidth + spacing), startY, boxWidth, boxHeight), "<b>" + info.buttonText + "</b>", ref info.enabled, info.toolTip);

                    }
                }

                GUI.EndScrollView();
            }


            if (_tabCurrent == 2)
            {
                _searchText = "Search..";
                //pc
                Rect scrollViewRect = new Rect(2f, -3f, 720f + 20f, 600f - 40f); // Adjusted width to include the scrollbar
                _scrollPosition = GUI.BeginScrollView(scrollViewRect, _scrollPosition, new Rect(0f, 0f, scrollViewRect.width - 40f, 2000f), false, false); // Adjusted content width to exclude the scrollbar
                float boxWidth = 173f;
                float boxHeight = 60f;
                float spacing = 15f;
                float startX = 144f;
                float startY = 20f;

                _userInput = lib.DrawTextField(new Rect(startX, startY, boxWidth, boxHeight), _userInput, Color.white, 19, FontStyle.BoldAndItalic, GUI.color);

                DrawCheatBox(new Rect(startX + boxWidth + spacing, startY, boxWidth, boxHeight), "<b>" + "WASD" + "</b>", ref _toggle1, "Lets you move on PC.");

                DrawCheatBox(new Rect(startX + 2 * (boxWidth + spacing), startY, boxWidth, boxHeight), "<b>" + "Disconnect" + "</b>", ref _toggle2, "Disconnects you.");
                if (_toggle2)
                {
                    PhotonNetwork.Disconnect();
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                    _toggle2 = false;
                }

                startY += boxHeight + spacing;


                DrawCheatBox(new Rect(startX, startY, boxWidth, boxHeight), "<b>" + "Set Name" + "</b>", ref _toggle4, "Sets ur name to the thing!");
                if (_toggle4)
                {
                    PhotonNetwork.LocalPlayer.NickName = _userInput;
                    PhotonNetwork.NickName = _userInput;
                    PlayerPrefs.SetString("playerName", _userInput);
                    GorillaComputer.instance.currentName = _userInput;
                    NetworkSystem.Instance.SetMyNickName(_userInput);
                    PlayerPrefs.Save();
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                    _toggle4 = false;
                }

                DrawCheatBox(new Rect(startX + boxWidth + spacing, startY, boxWidth, boxHeight), "<b>" + "Join Room" + "</b>", ref _toggle5, "Join Room");
                if (_toggle5)
                {
                    PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(_userInput, JoinType.Solo);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                    _toggle5 = false;
                }

                //colors

                startY += boxHeight + spacing;

                r = lib.DrawTextField(new Rect(startX, startY, boxWidth, boxHeight), r, Color.white, 19, FontStyle.BoldAndItalic, GUI.color);
                g = lib.DrawTextField(new Rect(startX + boxWidth + spacing, startY, boxWidth, boxHeight), g, Color.white, 19, FontStyle.BoldAndItalic, GUI.color);
                b = lib.DrawTextField(new Rect(startX + 2 * (boxWidth + spacing), startY, boxWidth, boxHeight), b, Color.white, 19, FontStyle.BoldAndItalic, GUI.color);

                float r2 = float.Parse(r);
                float g2 = float.Parse(g);
                float b2 = float.Parse(b);

                startY += boxHeight + spacing;

                DrawCheatBox(new Rect(startX, startY, boxWidth, boxHeight), "<b>" + "Set Color" + "</b>", ref _toggle222, "Change ur color!");
                if (_toggle222)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(-66.7989f, 12.5422f, -82.6815f);
                    if (GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(PhotonNetwork.LocalPlayer.UserId))
                    {
                        GorillaTagger.Instance.UpdateColor(r2, g2, b2);
                        Debug.Log("local");
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[]
                        {
                            r2,
                            g2,
                            b2,
                        });
                        GorillaTagger.Instance.offlineVRRig.enabled = true;
                        _toggle222 = false;
                    }
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                }

                GUI.EndScrollView();
            }
            if (_tabCurrent == 3)
            {
                _searchText = "Search..";
                float boxWidth = 173f;
                float boxHeight = 60f;
                float startX = 144f;
                float startY = 20f;
                if (PhotonNetwork.InRoom)
                {
                    lib.DrawText(new Rect(144, 20, 183, 70), "Room Code : " + PhotonNetwork.CurrentRoom.Name, Color.white);
                    lib.DrawText(new Rect(144, 40, 183, 70), "Server IP : " + PhotonNetwork.ServerAddress, Color.white);
                    lib.DrawText(new Rect(144, 60, 183, 70), "Players : " + PhotonNetwork.CurrentRoom.PlayerCount, Color.white);
                    lib.DrawText(new Rect(144, 80, 183, 70), "Region : " + PhotonNetwork.CloudRegion, Color.white);
                    lib.DrawText(new Rect(144, 100, 183, 70), "Master : " + PhotonNetwork.MasterClient.NickName, Color.white);
                    lib.DrawText(new Rect(144, 120, 183, 70), "Max Users : " + PhotonNetwork.CurrentRoom.MaxPlayers, Color.white);

                    DrawCheatBox(new Rect(144, 150, 183, 70), "<b>" + "Disconnect" + "</b>", ref _toggle5, "Makes you leave.");
                    if (_toggle5)
                    {
                        PhotonNetwork.Disconnect();
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                        _toggle5 = false;
                    }
                }
                else
                {
                    lib.DrawText(new Rect(startX, startY, boxWidth, boxHeight), "Not connected to a room", Color.white);
                }
                // pit your room shit here idk wtv yapyap

            }
            if (_tabCurrent == 4)
            {
                _searchText = "Search..";
                Rect scrollViewRect = new Rect(2f, -3f, 720f + 20f, 600f - 40f); // Adjusted width to include the scrollbar
                _scrollPosition = GUI.BeginScrollView(scrollViewRect, _scrollPosition, new Rect(0f, 0f, scrollViewRect.width - 40f, 2000f), false, false); // Adjusted content width to exclude the scrollbar
                float boxWidth = 173f;
                float boxHeight = 60f;
                float spacing = 15f;
                float startX = 144f;
                float startY = 50f;
                
                DrawCheatBox(new Rect(startX, startY, boxWidth, boxHeight), "<b>" + "Emulate Left Trigger" + "</b>", ref _emulator1, "Makes you press down the button.");

                DrawCheatBox(new Rect(startX + boxWidth + spacing, startY, boxWidth, boxHeight), "<b>" + "Emulate Right Trigger" + "</b>", ref _emulator2, "Makes you press down the button.");

                DrawCheatBox(new Rect(startX + 2 * (boxWidth + spacing), startY, boxWidth, boxHeight), "<b>" + "Emulate Left Grip" + "</b>", ref _emulator3, "Makes you press down the button.");
                startY += boxHeight + spacing;

                DrawCheatBox(new Rect(startX, startY, boxWidth, boxHeight), "<b>" + "Emulate Right Grip" + "</b>", ref _emulator4, "Makes you press down the button.");

                DrawCheatBox(new Rect(startX + boxWidth + spacing, startY, boxWidth, boxHeight), "<b>" + "Emulate Left Primary" + "</b>", ref _emulator5, "Makes you press down the button.");

                DrawCheatBox(new Rect(startX + 2 * (boxWidth + spacing), startY, boxWidth, boxHeight), "<b>" + "Emulate Right Primary" + "</b>", ref _emulator6, "Makes you press down the button.");
                startY += boxHeight + spacing;

                DrawCheatBox(new Rect(startX, startY, boxWidth, boxHeight), "<b>" + "Emulate Left Secondary" + "</b>", ref _emulator7, "Makes you press down the button.");

                DrawCheatBox(new Rect(startX + boxWidth + spacing, startY, boxWidth, boxHeight), "<b>" + "Emulate Right Secondary" + "</b>", ref _emulator8, "Makes you press down the button.");

                GUI.EndScrollView();
                //3rd tab emulatro stuff
            }
            if (_tabCurrent == 5)
            {
                _searchText = "Search..";
                Rect scrollViewRect = new Rect(2f, -3f, 720f + 20f, 600f - 40f); // Adjusted width to include the scrollbar
                _scrollPosition = GUI.BeginScrollView(scrollViewRect, _scrollPosition, new Rect(0f, 0f, scrollViewRect.width - 40f, 2000f), false, false); // Adjusted content width to exclude the scrollbar
                float boxWidth = 173f;
                float boxHeight = 60f;
                float spacing = 15f;
                float startX = 144f;
                float startY = 50f;
                int buttonNum = -1;


                float labelWidth = 100f;
                float sliderWidth = 132f;
                float sliderHeight = 20f;

                for (int i = Buttons.buttons[0].Length - 1; i >= 0; i--)
                {
                    if (Buttons.buttons[0].AsEnumerable().Reverse().ToList()[i].buttonText != "Settings" && Buttons.buttons[0].AsEnumerable().Reverse().ToList()[i].buttonText != "settings")
                    {

                        buttonNum++;
                        if (buttonNum == 3)
                        {
                            startY += boxHeight + spacing;
                            buttonNum = 0;
                        }

                        ButtonInfo info = Buttons.buttons[0].AsEnumerable().Reverse().ToList()[i];
                        if (buttonNum == 0)
                            DrawCheatBox(new Rect(startX, startY, boxWidth, boxHeight), "<b>" + info.buttonText + "</b>", ref info.enabled, info.toolTip);
                        if (buttonNum == 1)
                            DrawCheatBox(new Rect(startX + boxWidth + spacing, startY, boxWidth, boxHeight), "<b>" + info.buttonText + "</b>", ref info.enabled, info.toolTip);
                        if (buttonNum == 2)
                            DrawCheatBox(new Rect(startX + 2 * (boxWidth + spacing), startY, boxWidth, boxHeight), "<b>" + info.buttonText + "</b>", ref info.enabled, info.toolTip);

                    }
                }

                // settings stuff you can change wtv idc;

                lib.DrawText(new Rect(160, 20, labelWidth, sliderHeight), "Window Alpha %", Color.white, 13, FontStyle.Normal);
                lib.Slider(new Rect(160 + labelWidth, 20, sliderWidth, sliderHeight), ref Alpha, 0f, 1f);


                GUI.EndScrollView();
            }

            if (isEmulating())
            {
                WristMenu.triggerDownL = _emulator1;
                WristMenu.triggerDownR = _emulator2;
                WristMenu.gripDownL = _emulator3;
                WristMenu.gripDownR = _emulator4;
                WristMenu.xbuttonDown = _emulator5;
                WristMenu.abuttonDown = _emulator6;
                WristMenu.ybuttonDown = _emulator7;
                WristMenu.bbuttonDown = _emulator8;
            }


            
            GUI.DragWindow();

        }

        public static void Update()
        {
            if (lib.WindowBg == null) { lib.InitLib(); }
        }

        public static bool isEmulating()
        {
            if (_emulator1 || _emulator2 || _emulator3 || _emulator4 || _emulator5 || _emulator6 || _emulator7 || _emulator8)
                return true;

            return false;
        }

        private bool isopen1 = true;
        private bool isopen2 = false;

        void DrawCheatBox(Rect rect, string title, ref bool toggle, string tooltipText)
        {
            lib.DrawCheatBox(rect, title);
            float moveLeftOffset = -3f;

            float labelWidth = 50f;
            float labelHeight = 20f;
            float toggleSize = 20f;
            float buttonWidth = 30f;
            float buttonHeight = 20f;
            float spacing = 10f;
            float yOffset = 2f;

            float textX = rect.x + 10f;
            float textY = rect.y + 30f + yOffset;

            float toggleX = textX + labelWidth + spacing + 29f - moveLeftOffset;
            float toggleY = textY;
            float tooltipX = toggleX + toggleSize + spacing - moveLeftOffset;
            float tooltipY = textY;

            lib.DrawText(new Rect(textX, textY, labelWidth, labelHeight), "Enabled:", Color.white, 14);
            lib.Toggle(new Rect(toggleX - -5f, toggleY, toggleSize, toggleSize), ref toggle);
            Rect tooltipButtonRect = new Rect(tooltipX, tooltipY, buttonWidth, buttonHeight);

            if (lib.Button(tooltipButtonRect, "i", 12))
            {
                // nothing here just tooltip button
            }
            if (tooltipButtonRect.Contains(Event.current.mousePosition))
            {
                float tooltipWidth = GUI.skin.label.CalcSize(new GUIContent(tooltipText)).x + 10;
                float xOffset = -tooltipWidth - 10;

                lib.DrawToolTip(tooltipText, xOffset, yOffset);
            }
        }

        public static Texture2D CreateGradientTexture(Color startColor, Color endColor, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int y = 0; y < height; y++)
            {
                float gradientFactor = (float)y / height;
                Color color = Color.Lerp(startColor, endColor, gradientFactor);

                for (int x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
            return texture;
        }
        public static void ApplyColorsToGUI()
        {
            Color buttonColor = WristMenu.buttonColor;
            Color menuOnTextColor = WristMenu.menuOnTextColor;
            Color menuOffTextColor = WristMenu.menuOffTextColor;
            Color colorToFade1 = WristMenu.colorToFade1;
            Color colorToFade2 = WristMenu.colorToFade2;

            float lerpFactor = Mathf.PingPong(Time.time / 1f, 1f);
            Color lerpedColor = Color.Lerp(colorToFade1, colorToFade2, lerpFactor);
            Color rainbowColor = UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f);

            if (Settings.rainbow)
            {
                NewGUI.particlecolour = rainbowColor;
                Color newLerped = windowBgColor;
                newLerped.a = Alpha;
                lib.UpdateTex(lib.WindowBg, newLerped);
                lib.UpdateTex(lib.TabBar, tabBarColor);
                lib.UpdateTex(lib.TabHover, tabHoverColor);
                lib.UpdateTex(lib.TabActive, tabActiveColor);
                lib.UpdateTex(lib.TabActiveBar, tabActiveBarColor);
                lib.UpdateTex(lib.CheatBox, cheatBoxColor);
                lib.UpdateTex(lib.CheatBoxSeperator, rainbowColor);
                lib.UpdateTex(lib.ToggleBackground, toggleBackgroundColor);
                lib.UpdateTex(lib.ToggleHover, toggleHoverColor);
                lib.UpdateTex(lib.ToggleActive, rainbowColor);
                lib.UpdateTex(lib.ButtonNormal, buttonNormalColor);
                lib.UpdateTex(lib.ButtonHover, buttonHoverColor);
                lib.UpdateTex(lib.ButtonActive, buttonActiveColor);
                lib.UpdateTex(lib.SliderBar, rainbowColor);
                lib.UpdateTex(lib.SliderGrab, sliderGrabColor);
                lib.UpdateTex(lib.ArrayListBG, new Color32(10, 10, 10, 120));
            }
            else
            {
                NewGUI.particlecolour = lerpedColor;
                Color newLerped = lerpedColor;
                newLerped.a = Alpha;
                lib.UpdateTex(lib.WindowBg, newLerped);
                lib.UpdateTex(lib.TabBar, MakeDarker(lerpedColor, 0.5f));
                lib.UpdateTex(lib.TabHover, lerpedColor);
                lib.UpdateTex(lib.TabActive, lerpedColor);
                lib.UpdateTex(lib.TabActiveBar, MakeDarker(lerpedColor, 0.5f));
                if (Settings.buttonColorInt == 15)
                    lib.UpdateTex(lib.CheatBox, MakeDarker(lerpedColor));
                else
                    lib.UpdateTex(lib.CheatBox, buttonColor);
                lib.UpdateTex(lib.CheatBoxSeperator, lerpedColor);
                lib.UpdateTex(lib.ToggleBackground, lerpedColor);
                lib.UpdateTex(lib.ToggleHover, menuOnTextColor);
                lib.UpdateTex(lib.ToggleActive, menuOnTextColor);
                lib.UpdateTex(lib.ButtonNormal, lerpedColor);
                lib.UpdateTex(lib.ButtonHover, lerpedColor);
                lib.UpdateTex(lib.ButtonActive, lerpedColor);
                lib.UpdateTex(lib.SliderBar, lerpedColor);
                lib.UpdateTex(lib.SliderGrab, sliderGrabColor);
                lib.UpdateTex(lib.ArrayListBG, new Color32(10, 10, 10, 120));
            }
        }
        //idk your toggles here wtv shibshab skibidi
        // also name these toggles your cheats so you dont get confused obviously
        private static bool _toggle1;
        private static bool _toggle2;
        private static bool _toggle3;
        private static bool _toggle4;
        private static bool _toggle5;
        private static bool _toggle6;
        private static bool _toggle7;
        private static bool _toggle8;
        private static bool _toggle9;
        private static bool _toggle10;
        private static bool _toggle11;
        private static bool _toggle12;
        private static bool _toggle13;
        private static bool _toggle14;
        private static bool _toggle15;
        private static bool _toggle16;
        private static bool _toggle17;
        private static bool _toggle18;
        private static bool _toggle222;

        //
        private static bool _emulator1;
        private static bool _emulator2;
        private static bool _emulator3;
        private static bool _emulator4;
        private static bool _emulator5;
        private static bool _emulator6;
        private static bool _emulator7;
        private static bool _emulator8;

        // these toggles here are now for the templated
        private static bool _toggle19;
        private static bool _toggle20;

    }
}