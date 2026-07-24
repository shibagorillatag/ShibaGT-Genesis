using GenesisGUILibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ShibaGTGenesis.UI
{
    internal class NotifLibGUI : MonoBehaviour
    {
        public class Notification
        {
            public string title, message;
            public Color32 color;
            public float duration;
            public Vector2 position;
            public float alpha;
            public Notification(string title, string message, Color color, float duration)
            {
                this.title = title;
                this.message = message;
                this.color = color;
                this.duration = duration;
                this.position = new Vector2(-100f, 0f);
                this.alpha = 1f;
            }
        }

        static List<Notification> Notifications = new List<Notification>();

        public static void SendNotification(string title, string message, Color color, float duration)
        {
            Notifications.Add(new Notification(title, message, color, duration));
        }

        void Update()
        {
            for (int i = Notifications.Count - 1; i >= 0; i--)
            {
                Notification notification = Notifications[i];
                notification.duration -= Time.deltaTime;

                float totalDuration = 4f;
                float slideInDuration = Mathf.Min(0.5f, totalDuration / 5f);
                float fadeOutDuration = Mathf.Min(0.5f, totalDuration / 5f);
                float displayDuration = totalDuration - slideInDuration - fadeOutDuration;

                if (notification.duration > displayDuration + fadeOutDuration)
                {
                    float t = (totalDuration - notification.duration) / slideInDuration;
                    notification.position.x = Mathf.Lerp(-100f, 20f, t);
                }
                else if (notification.duration < fadeOutDuration)
                {
                    float t = notification.duration / fadeOutDuration;
                    notification.position.x = Mathf.Lerp(20f, -100f, 1 - t);
                    notification.alpha = t;
                }
                else
                {
                    notification.position.x = 20f;
                }

                if (notification.duration < 0)
                {
                    Notifications.RemoveAt(i);
                }
            }
        }
        void Start()
        {
            //string text = new WebClient().DownloadString("urlhere");
            //SendNotification("Welcome!", "Welcome to GENESIS!\nCurrent Status:\n<color=yellow>[ " + text + " ]", Color.blue, 5f);
        }

        void OnGUI()
        {
            float yPos = 20f;
            float size = 55f;
            foreach (Notification notification in Notifications)
            {
                Rect notificationRect = new Rect(notification.position.x, yPos, 250f, size);
                yPos += lib.DrawNotification(notificationRect, notification.title, notification.message, notification.color, notification.alpha, notification.duration).y;
            }
        }
    }
}
