using GTAG_NotificationLib;
using UnityEngine;

namespace ShibaGTGenesis.Backend
{
    internal class ModPresets : MonoBehaviour
    {
        public static void GhostTrolling()
        {
            Back.GetButton("Ghost Monkey").enabled = true;
            Back.GetButton("Really Long Arms").enabled = true;
            Back.GetButton("Noclip").enabled = true;
            Back.GetButton("Fly").enabled = true;

            NotifiLib.SendNotification("Turned on Ghost Monkey, Really Long Arms, Noclip, and Fly!");
        }

        public static void Legit()
        {
            Back.GetButtonInCate("Speed Boost", 6).enabled = true;
            Back.GetButton("Steam Long Arms").enabled = true;
            Back.GetButton("Wall Walk [").enabled = true;
            Back.GetButton("Tag Aura").enabled = true;

            NotifiLib.SendNotification("Turned on Speed Boost, Steam Long Amrs, Wall Walk [lg], and Tag Aura!");
        }
    }
}
