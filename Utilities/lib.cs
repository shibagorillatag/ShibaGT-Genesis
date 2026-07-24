
using Genesis.UI;
using ShibaGTGenesis.Backend;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenesisGUILibrary
{
    public class lib
    {
        public static Texture2D WindowBg;
        public static Texture2D TabBar;
        public static Texture2D TabHover;
        public static Texture2D TabActive;
        public static Texture2D TabActiveBar;
        public static Texture2D CheatBox;
        public static Texture2D CheatBoxSeperator;
        public static Texture2D ToggleBackground;
        public static Texture2D ToggleHover;
        public static Texture2D ToggleActive;
        public static Texture2D ButtonNormal;
        public static Texture2D ButtonHover;
        public static Texture2D ButtonActive;
        public static Texture2D SliderBar;
        public static Texture2D WatermarkBg;
        public static Texture2D SliderGrab;
        public static Texture2D ArrayListBG;
        public static void InitLib()
        {
            Color colorToFade1 = WristMenu.colorToFade1;
            Color colorToFade2 = WristMenu.colorToFade2;

            float lerpFactor = Mathf.PingPong(Time.time / 5f, 1f);
            Color lerpedColor = Color.Lerp(WristMenu.colorToFade1, WristMenu.colorToFade2, Mathf.PingPong(Time.time, 1f));

            WatermarkBg = ColorTex(new Color32(10, 10, 10, 180));
            WindowBg = ColorTex(new Color32(10, 10, 10, 240));
            TabBar = ColorTex(new Color32(20, 20, 20, 255));
            TabHover = ColorTex(new Color32(40, 40, 40, 255));
            TabActive = ColorTex(new Color32(50, 50, 50, 255));
            TabActiveBar = ColorTex(lerpedColor);
            CheatBox = ColorTex(new Color32(30, 30, 30, 255));
            CheatBoxSeperator = ColorTex(new Color32(0, 0, 205, 255));
            ToggleBackground = ColorTex(new Color32(40, 40, 40, 255));
            ToggleHover = ColorTex(new Color32(70, 70, 70, 85));
            ToggleActive = ColorTex(new Color32(0, 0, 205, 255));
            ButtonNormal = ColorTex(new Color32(40, 40, 40, 255));
            ButtonHover = ColorTex(new Color32(47, 47, 47, 255));
            ButtonActive = ColorTex(new Color32(85, 85, 85, 255));
            SliderBar = ColorTex(new Color32(0, 0, 205, 255));
            SliderGrab = ColorTex(Color.white);
            ArrayListBG = ColorTex(new Color32(10, 10, 10, 120));
        }

        public static void DrawWatermarkBG(Rect rect, int borderRadius)
        {
            DrawRoundedTex(rect, WatermarkBg, borderRadius);
        }

       
        public static void DrawText(Rect rect, string text, Color lerp, int fontSize = 12, FontStyle fontStyle = FontStyle.Normal)
        {
            GUIStyle _style = new GUIStyle(GUI.skin.label);
            _style.fontSize = fontSize;
            _style.normal.textColor = lerp;
            _style.fontStyle = fontStyle;
            GUI.Label(rect, new GUIContent(text), _style);
        }
        public static void DrawText2(Rect rect, string text, int fontSize = 12, Color textColor = default(Color), FontStyle fontStyle = FontStyle.Normal, bool centerX = false, bool centerY = false)
        {
            GUIStyle _style = new GUIStyle(GUI.skin.label);
            _style.fontSize = fontSize;

            // Set additional style properties
            _style.fontStyle = fontStyle;

            float X = centerX ? rect.x + (rect.width / 2f) - (_style.CalcSize(new GUIContent(text)).x / 2f) : rect.x;
            float Y = centerY ? rect.y + (rect.height / 2f) - (_style.CalcSize(new GUIContent(text)).y / 2f) : rect.y;

            GUI.Label(new Rect(X, Y, rect.width, rect.height), new GUIContent(text), _style);
            var width = _style.CalcSize(new GUIContent(text));
            GUI.DrawTexture(new Rect(X, Y, width.x + 15f, 20f + width.y + 5f), ArrayListBG);
        }

        public static void Slider(Rect rect, ref float value, float min, float max)
        {
            value = GUI.HorizontalSlider(new Rect(rect.x, rect.y, rect.width, rect.height), value, min, max, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb);
            DrawRoundedTex(new Rect(rect.x, rect.y, rect.width, rect.height), CheatBox, 0);
            DrawRoundedTex(new Rect(rect.x, rect.y + (rect.height / 2f), rect.width, 2f), SliderBar, 4);
            Rect xrect = new Rect(rect.x, rect.y + (rect.height / 2f), rect.width, 2f);
            float X = xrect.x + xrect.width * ((value - min) / (max - min));
            DrawRoundedTex(new Rect(X - 5f, xrect.y - 5f, 15f, 15f), SliderGrab, 20);
        }

        public static void Toggle(Rect rect, ref bool toggle)
        {
            if (toggle)
            {
                DrawRoundedTex(new Rect(rect.x, rect.y, rect.height, rect.height), ToggleActive, 3);
            }
            else { DrawRoundedTex(new Rect(rect.x, rect.y, rect.height, rect.height), ToggleBackground, 3); }
            if (new Rect(rect.x, rect.y, rect.height, rect.height).Contains(Event.current.mousePosition))
            {
                if (toggle == false)
                    DrawRoundedTex(new Rect(rect.x, rect.y, rect.height, rect.height), ToggleHover, 0);
                if (Event.current.type == EventType.MouseDown)
                {
                    toggle = !toggle;
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                }
            }
        }
        public static void DrawCheatBox(Rect rect, string text)
        {
            DrawRoundedTex(rect, CheatBox, 4);
            DrawRoundedTex(new Rect(rect.x, rect.y + 25.4f, rect.width, 3f), CheatBoxSeperator, 0);

            int fontSize = 14;
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.white;
            style.wordWrap = false;

            Vector2 textSize = style.CalcSize(new GUIContent(text));
            while (textSize.x > rect.width - 20f && fontSize > 8)
            {
                fontSize--;
                style.fontSize = fontSize;
                textSize = style.CalcSize(new GUIContent(text));
            }

            DrawText(new Rect(rect.x + 10f, rect.y + 2.5f, rect.width - 20f, rect.height - 30f), text, Color.white, fontSize);
        }

        public static void DrawCheatBox2(Rect rect, string text)
        {
            DrawRoundedTex(rect, CheatBox, 7);
            DrawText(new Rect(rect.x + 10f, rect.y + 5f, rect.width - 10f, 30f), text, Color.white, 17);
        }

        public static string DrawTextField(Rect rect, string text, Color textColor, int fontSize = 12, FontStyle fontStyle = FontStyle.Normal, Color boxColor = default(Color))
        {
            Color originalBackgroundColor = GUI.backgroundColor;
            if (boxColor != default(Color))
            {
                GUI.backgroundColor = boxColor;
            }

            GUIStyle style = new GUIStyle(GUI.skin.textField)
            {
                fontSize = fontSize,
                fontStyle = fontStyle,
                normal = { textColor = textColor }
            };
            string newText = GUI.TextField(rect, text, style);
            GUI.backgroundColor = originalBackgroundColor;

            return newText;
        }

        public static bool Button(Rect rect, string text, int borderRadius)
        {
            if (rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                return true;
            if (rect.Contains(Event.current.mousePosition))
            {
                DrawRoundedTex(rect, ButtonHover, 4);
            }
            else { DrawRoundedTex(rect, ButtonNormal, 4); }
            if (Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed)
            {
                if (rect.Contains(Event.current.mousePosition))
                    DrawRoundedTex(rect, ButtonActive, 4);
            }
            GUIStyle labelstyle = new GUIStyle(GUI.skin.label);
            labelstyle.fontSize = 12;
            labelstyle.fontStyle = FontStyle.Bold;
            DrawText(new Rect(rect.x + (rect.width / 2f) - (labelstyle.CalcSize(new GUIContent(text)).x / 2f), rect.y + (rect.height / 2f) - (labelstyle.CalcSize(new GUIContent(text)).y / 2f), 200f, 50f), text, Color.white, 12, FontStyle.Bold);
            return false;
        }
        private static Rect TabActiveRect;
        private static float TabActiveYPos;
        private static float TabActiveYTimer;

        public static Vector2 DrawNotification(Rect rect, string title, string message, Color32 color, float alpha, float timerProgress)
        {
            GUIStyle titlStyle = new GUIStyle(UnityEngine.GUI.skin.label) { fontSize = 14, fontStyle = FontStyle.Bold };
            GUIStyle messagestyle = new GUIStyle(UnityEngine.GUI.skin.label) { fontSize = 12, fontStyle = FontStyle.Normal };

            Vector2 titleSize = titlStyle.CalcSize(new GUIContent(title));
            Vector2 messagesize = messagestyle.CalcSize(new GUIContent(message));

            rect.width = Mathf.Max(rect.width, Mathf.Max(titleSize.x, messagesize.x) + 10f);
            rect.height = Mathf.Max(rect.height, titleSize.y + messagesize.y + 10f);

            color.a = (byte)(alpha * 255);
            DrawRoundedTex(rect, WindowBg, 4);

            var oldColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = color;

            UnityEngine.GUI.color = new Color(1, 1, 1, alpha);
            DrawText(new Rect(rect.x + 15f, rect.y + 3f, rect.width, titleSize.y), title, Color.white, 14, FontStyle.Bold);
            DrawText(new Rect(rect.x + 15f, rect.y + titleSize.y + 3f, rect.width, messagesize.y), message, Color.white, 12, FontStyle.Normal);

            UnityEngine.GUI.color = oldColor;
            return rect.size;
        }

        public static void DrawTabActive()
        {
            DrawRoundedTex(TabActiveRect, TabActive, 0);
            DrawRoundedTex(new Rect(TabActiveRect.x, TabActiveRect.y, 3f, TabActiveRect.height), CheatBoxSeperator, 0);
        }

        public static void MoveTabActive(Rect rect)
        {
            TabActiveYTimer += Time.deltaTime * 2;
            TabActiveYPos = Mathf.Lerp(TabActiveYPos, rect.y, TabActiveYTimer);
            TabActiveRect = new Rect(rect.x, TabActiveYPos, rect.width, rect.height);
            TabActiveYTimer = 0;
        }

        public static void DrawToolTip(string tooltipText, float xOffset = 0, float yOffset = -15)
        {
            if (!string.IsNullOrEmpty(tooltipText))
            {
                Vector2 tooltipSize = GUI.skin.label.CalcSize(new GUIContent(tooltipText));
                Rect tooltipRect = new Rect(Event.current.mousePosition.x + xOffset, Event.current.mousePosition.y + yOffset - tooltipSize.y, tooltipSize.x + 10, tooltipSize.y + 5);

                float windowLeft = 100f;
                float windowTop = 100f;
                float windowRight = windowLeft + 10000f;
                float windowBottom = windowTop + 10000f;

                if (tooltipRect.x < windowLeft)
                {
                    tooltipRect.x = windowLeft;
                }
                if (tooltipRect.y < windowTop)
                {
                    tooltipRect.y = windowTop;
                }
                if (tooltipRect.xMax > windowRight)
                {
                    tooltipRect.x = windowRight - tooltipRect.width;
                }
                if (tooltipRect.yMax > windowBottom)
                {
                    tooltipRect.y = windowBottom - tooltipRect.height;
                }
                lib.DrawCheatBox2(tooltipRect, "");
                lib.DrawText(new Rect(tooltipRect.x + 5, tooltipRect.y + 2, tooltipSize.x, tooltipSize.y), tooltipText, Color.white, 13, FontStyle.Italic);
            }
        }




        public static void Tab(Rect rect, int index, ref int current, string text)
        {
            if (index == 1)
                DrawTabActive();
            GUIStyle labelstyle = new GUIStyle(GUI.skin.label);
            labelstyle.fontSize = 20;
            labelstyle.fontStyle = FontStyle.Bold;
            Color textcolor = WristMenu.menuOffTextColor;
            if (rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                current = index;
            if (current == index)
            {
                MoveTabActive(rect);
                if (Settings.rainbow)
                    textcolor = UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f);
                else
                    textcolor = WristMenu.menuOnTextColor;
            }
            else
            {
                if (rect.Contains(Event.current.mousePosition))
                {
                    DrawRoundedTex(rect, TabHover, 0);
                }
            }
            DrawText(new Rect(rect.x + (rect.width / 8f), rect.y + (rect.height / 2f) - (labelstyle.CalcSize(new GUIContent(text)).y / 2f), 200f, 50f), text, textcolor, 14, FontStyle.Bold);
        }
        public static void DrawLoadingBar(Rect rect)
        {
            GUI.color = UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f);
            GUI.Box(rect, GUIContent.none, new GUIStyle { normal = { background = Texture2D.grayTexture } });
            GUI.color = GUI.color;
        }



        public static void DrawTabBar(Rect rect, int borderRadius)
        {
            DrawRoundedTex(rect, TabBar, borderRadius);
        }
        public static void DrawWindowBG(Rect rect, int borderRadius)
        {
            DrawRoundedTex(rect, WindowBg, borderRadius);
        }
        public static Texture2D ColorTex(Color color)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, color);
            tex.Apply();
            return tex;
        }

        public static void UpdateTex(Texture2D tex, Color color)
        {
            tex.SetPixel(0, 0, color);
            tex.Apply();
        }
        public static Texture2D GradTex(Color startColor, Color endColor)
        {
            Texture2D tex = new Texture2D(1024, 1024);
            for (int y = 0; y < 1024; y++)
            {
                for (int x = 0; x < 1024; x++)
                {
                    tex.SetPixel(y, x, Color.Lerp(endColor, startColor, Mathf.InverseLerp(0, 1024, x)));
                }
            }
            tex.Apply();
            return tex;
        }
        public static void DrawRoundedTex(Rect rect, Texture2D tex, int radius)
        {
            GUI.DrawTexture(rect, tex, ScaleMode.StretchToFill, true, 0f, GUI.color, 0f, radius);
        }

        public static void DrawArrayListText(Rect rect, string text, int fontSize = 12, Color textColor = default(Color), FontStyle fontStyle = FontStyle.Normal, bool centerX = false, bool centerY = false)
        {
            GUIStyle _style = new GUIStyle(GUI.skin.label);
            _style.fontSize = fontSize;
            _style.normal.textColor = textColor;
            _style.fontStyle = fontStyle;
            float X = centerX ? rect.x + (rect.width / 2f) - (_style.CalcSize(new GUIContent(text)).x / 2f) : rect.x;
            float Y = centerY ? rect.y + (rect.height / 2f) - (_style.CalcSize(new GUIContent(text)).y / 2f) : rect.y;
            GUI.Label(new Rect(X, Y, rect.width, rect.height), new GUIContent(text), _style);
        }
    }
}