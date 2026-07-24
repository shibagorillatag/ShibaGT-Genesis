using BepInEx;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mono;
using Photon.Pun;
using Genesis.Backend;
using System.Threading;
using ExitGames.Client.Photon;
using UnityEngine.UIElements;
using ShibaGTGenesis.UI;
using TMPro;
using Genesis.UI;
using ShibaGTGenesis.Backend;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GTAG_NotificationLib
{
    public class NotifiLib : MonoBehaviour
    {
        GameObject HUDObj;
        GameObject HUDObj2;
        GameObject MainCamera;
        Text Testtext;
        Text INDText;
        Material AlertText = new Material(Shader.Find("GUI/Text Shader"));
        int NotificationDecayTime = 100;
        int NotificationDecayTimeCounter = 100;
        public static int NoticationThreshold = 100; //Amount of notifications before they stop queuing up
        string[] Notifilines;
        string newtext;
        public static string PreviousNotifi;
        public static string PreviousInd;
        bool HasInit = false;
        public static Text IndiText;
        static Text NotifiText;
        public static bool IsEnabled = true;

        private void Start()
        {
            try
            {
                MainCamera = GameObject.Find("Main Camera");
                HUDObj = new GameObject();
                HUDObj2 = new GameObject();
                HUDObj2.name = "SHIBA_NOTIFICATIONLIB";
                HUDObj.name = "SHIBA_NOTIFICATIONLIB";
                HUDObj.AddComponent<Canvas>();
                HUDObj.AddComponent<CanvasScaler>();
                HUDObj.AddComponent<GraphicRaycaster>();
                HUDObj.GetComponent<Canvas>().enabled = true;
                HUDObj.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
                HUDObj.GetComponent<Canvas>().worldCamera = MainCamera.GetComponent<Camera>();
                HUDObj.GetComponent<RectTransform>().sizeDelta = new Vector2(5f, 5f);
                HUDObj.GetComponent<RectTransform>().position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z);
                HUDObj2.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z - 4.6f);
                HUDObj.transform.parent = HUDObj2.transform;
                HUDObj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 1.6f);
                Vector3 eulerAngles = HUDObj.GetComponent<RectTransform>().rotation.eulerAngles;
                eulerAngles.y = -270f;
                HUDObj.transform.localScale = new Vector3(1f, 1f, 1f);
                HUDObj.GetComponent<RectTransform>().rotation = Quaternion.Euler(eulerAngles);
                Testtext = new GameObject
                {
                    transform =
                {
                    parent = HUDObj.transform
                }
                }.AddComponent<Text>();
                Testtext.text = "";
                Testtext.fontSize = 5;
               
                Testtext.rectTransform.sizeDelta = new Vector2(260f, 70f);
                Testtext.alignment = TextAnchor.UpperLeft;
                Testtext.rectTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
                Testtext.rectTransform.localPosition = new Vector3(-1.5f, -0.9f, -0.6f);
                Testtext.material = AlertText;
                NotifiText = Testtext;

                // IND

                    INDText = new GameObject
                {
                    transform =
                {
                    parent = HUDObj.transform
                }
                }.AddComponent<Text>();
                INDText.text = "";
                INDText.fontSize = 5;
                INDText.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/ForestScoreboardAnchor/GorillaScoreBoard/LineParent/GorillaPlayerScoreboardLine/Mute Button/Mute Text").GetComponent<Text>().font;
                INDText.rectTransform.sizeDelta = new Vector2(250f, 700f);
                INDText.alignment = TextAnchor.UpperLeft;
                INDText.rectTransform.localScale = new Vector3(0.0033f, 0.0033f, 0.33333333f);
                INDText.rectTransform.localPosition = new Vector3(-1f, -0.7f, -0.5f);
                INDText.material = new Material(Shader.Find("GUI/Text Shader"));
                IndiText = INDText;
            }
            catch(Exception ex)
            {
                Debug.Log("Notif Issue : " + ex);
            }
        }

        private void Update()
        {
            HUDObj2.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z);
            HUDObj2.transform.rotation = MainCamera.transform.rotation;

            Color c = Color.Lerp(WristMenu.colorToFade1, WristMenu.colorToFade2, Mathf.PingPong(Time.time, 1f));
            INDText.material.color = c;
            if (Settings.rainbow)
                INDText.material.color = UnityEngine.Color.HSVToRGB((Time.frameCount / 180f) % 1f, 1f, 1f);

            //HUDObj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 1.6f);
            if (Testtext.text != "") //THIS CAUSES A MEMORY LEAK!!!!! -no longer causes a memory leak
            {
                NotificationDecayTimeCounter++;
                if (NotificationDecayTimeCounter > NotificationDecayTime)
                {
                    Notifilines = null;
                    newtext = "";
                    NotificationDecayTimeCounter = 0;
                    Notifilines = Testtext.text.Split(Environment.NewLine.ToCharArray()).Skip(1).ToArray();
                    foreach (string Line in Notifilines)
                    {
                        if (Line != "")
                        {
                            newtext = newtext + Line + "\n";
                        }
                    }
                    Testtext.text = newtext;
                }
            }
            else
            {
                NotificationDecayTimeCounter = 0;
            }
        }

        public static float ropedelay;

        public static float NotifFloat;

        public static void SendNotification(string NotificationText)
        {
            if (Back.GetButton("Turn Off Notification").enabled)
                return;

            if (NotifFloat < Time.time)
            {
                NotifFloat = Time.time + 0.1f;
                if (IsEnabled)
                {
                    if (!NotificationText.Contains(Environment.NewLine)) { NotificationText = NotificationText + Environment.NewLine; }
                    NotifiText.text = NotifiText.text + NotificationText;
                    PreviousNotifi = NotificationText;

                    if (NotificationText.Contains("<color=red>"))
                        NotifLibGUI.SendNotification("Notification", NotificationText, Color.red, 4f);
                    else
                        NotifLibGUI.SendNotification("Notification", NotificationText, Color.blue, 4f);
                }
            }
        }
        
        public static void SendIndictor(string NotificationText)
        {
            if (NotifFloat < Time.time)
            {
                NotifFloat = Time.time + 0.1f;
                if (IsEnabled)
                {
                    if (!NotificationText.Contains(Environment.NewLine)) { NotificationText = NotificationText + Environment.NewLine; }
                    IndiText.text = IndiText.text + NotificationText;
                    PreviousInd = NotificationText;

                    if (NotificationText.Contains("<color=red>"))
                        NotifLibGUI.SendNotification("Indicator", NotificationText, Color.red, 4f);
                    else
                        NotifLibGUI.SendNotification("Indicator", NotificationText, Color.blue, 4f);
                }
            }
        }

        public static void ClearAllNotifications()
        {
            NotifiText.text = "";
        }
        public static void ClearPastNotifications(int amount)
        {
            string[] Notifilines = null;
            string newtext = "";
            Notifilines = NotifiText.text.Split(Environment.NewLine.ToCharArray()).Skip(amount).ToArray();
            foreach (string Line in Notifilines)
            {
                if (Line != "")
                {
                    newtext = newtext + Line + "\n";
                }
            }

            NotifiText.text = newtext;
        }
    }
}