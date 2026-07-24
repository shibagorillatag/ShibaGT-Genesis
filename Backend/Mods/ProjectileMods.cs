using Genesis.UI;
using GTAG_NotificationLib;
using Photon.Voice.Unity;
using ShibaGTGenesis.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static Photon.Voice.Unity.Recorder;

namespace ShibaGTGenesis.Backend
{
    internal class ProjectileMods : MonoBehaviour
    {
        public static bool loop;

        public static void LoopSounds()
        {
            loop = true;
        }

        public static void DontLoopSound()
        {
            loop = false;
        }

        public static void AddOwn()
        {
            File.WriteAllText("howtoaddownsounds.txt", "Go to your gorilla tag files\nopen the 'genesisprefs' folder\nthen open the 'genesissounds' folder\nand then put in whatever mp3s you want");
            Process.Start("howtoaddownsounds.txt");
            //File.Delete("ez.txt");
        }
        public static void LoadSounds()
        {
            string fullPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "genesisPrefs\\genesisSounds");

            if (Directory.Exists(fullPath))
            {
                string[] mp3Files = Directory.GetFiles(fullPath, "*.mp3");

                List<ButtonInfo> mods = new List<ButtonInfo> { new ButtonInfo { buttonText = "Soundboard", method = () => ShibaGTGenesis.Backend.Back.ProjectileMods(), isClickable = true, enabled = false, toolTip = "Go back!" },
                new ButtonInfo { buttonText = "How To Add Your Own Sounds", method = () => ShibaGTGenesis.Backend.ProjectileMods.AddOwn(), isClickable = true, enabled = false, toolTip = "adds sounds!" },
                new ButtonInfo { buttonText = "Loop Sound", method = () => ShibaGTGenesis.Backend.ProjectileMods.LoopSounds(), disableMethod = () => ProjectileMods.DontLoopSound(), isClickable = false, enabled = false, toolTip = "loops ur sounds!" },
                new ButtonInfo { buttonText = "Load Sounds", method = () => ShibaGTGenesis.Backend.ProjectileMods.LoadSounds(), isClickable = true, enabled = false, toolTip = "loads ur sounds!" },
                new ButtonInfo { buttonText = "Play Sound With Trigger", method = () => ShibaGTGenesis.Backend.ProjectileMods.PlaySoundTrigger(), disableMethod=()=> ProjectileMods.OffPlaySound(), isClickable = false, enabled = false, toolTip = "play ur sounds!" }};

                foreach (string file in mp3Files)
                {
                    string fileName = Path.GetFileName(file);
                    UnityEngine.Debug.Log("Found MP3 file: " + fileName);
                    mods.Add(new ButtonInfo { buttonText = "Play " + fileName, oneMethod = () => ProjectileMods.PlaySound(fileName), oneDisableMethod =()=> StopSound(fileName), isClickable = false, enabled = false });
                }

                Buttons.buttons[8] = mods.ToArray();
            }
            else
            {
                UnityEngine.Debug.LogError("Directory does not exist: " + fullPath);
            }
        }

        public static bool soundToggle;
        public static bool soundToggle2;

        public static void StopSound(string from)
        {
            Back.GetButton(from).enabled = false;
            GorillaTagger.Instance.myRecorder.SourceType = (InputSourceType)0;
            GorillaTagger.Instance.myRecorder.AudioClip = null;
            GorillaTagger.Instance.myRecorder.RestartRecording(true);
            GorillaTagger.Instance.myRecorder.DebugEchoMode = false;
        }

        public static float PlayFloat;
        public static bool playOn;

        public static void PlaySoundTrigger()
        {
            if (!NewGUI.playSet)
            {
                playOn = true;
                if (Time.time > PlayFloat)
                {
                    PlayFloat = Time.time + 3.5f;

                    NotifiLib.SendNotification("<color=blue>[genesis]</color> Type in the sounds name on your pc!");
                }
            }
            if (NewGUI.playSet)
            {
                playOn = false;
                if (WristMenu.triggerDownL)
                {
                    if (soundToggle)
                    {
                        Back.GetButton(NewGUI.playString).enabled = true;
                        soundToggle = false;
                    }
                }   
                else
                {
                    soundToggle = true;
                }
            }
            if (WristMenu.triggerDownR)
            {
                if (soundToggle2)
                {
                    Back.GetButton("Stop Soun").enabled = true;
                    NotifiLib.SendNotification("<color=blue>[genesis]</color> Stopped all sounds!");
                    soundToggle2 = false;
                }
            }
            else
            {
                soundToggle2 = true;
            }
        }

        public static void OffPlaySound()
        {
            NewGUI.playSet = false;
            playOn = false;
        }

        public static void PlaySound(string file)
        {
            try
            {
                AudioClip sound = LoadSoundFromFile(file);
                GorillaTagger.Instance.myRecorder.SourceType = Recorder.InputSourceType.AudioClip;
                GorillaTagger.Instance.myRecorder.AudioClip = sound;
                GorillaTagger.Instance.myRecorder.RestartRecording(true);
                GorillaTagger.Instance.myRecorder.LoopAudioClip = loop;
                GorillaTagger.Instance.myRecorder.DebugEchoMode = true;
            }
            catch { }
        }

        public static AudioClip LoadSoundFromFile(string fileName)
        {
            AudioClip sound = null;
            string filePath = System.IO.Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, "GenesisPrefs/genesisSounds/" + fileName);
            filePath = filePath.Split(new string[] { "BepInEx\\" }, StringSplitOptions.None)[0] + "GenesisPrefs/genesisSounds/" + fileName;
            filePath = filePath.Replace("\\", "/");

            UnityWebRequest actualrequest = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG);
            UnityWebRequestAsyncOperation newvar = actualrequest.SendWebRequest();
            while (!newvar.isDone) { }

            AudioClip actualclip = DownloadHandlerAudioClip.GetContent(actualrequest);
            sound = Task.FromResult(actualclip).Result;

            return sound;
        }
    }
}
