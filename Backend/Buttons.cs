using Genesis.Backend;
using Genesis.UI;
using Photon.Pun;
using UnityEngine;

namespace ShibaGTGenesis.Backend
{
    internal class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {

            new ButtonInfo[] // 0
            {
                //new ButtonInfo { buttonText = "test", method =() => Back.debugging(), isClickable = true, enabled = false, toolTip = "Automatically enables all enabled mods on next boot!"},
                //new ButtonInfo { buttonText = "test 2", method =() => NotifiLib.SendNotification("rizzzwefwefw wefwe fewfwe"), isClickable = true, enabled = false, toolTip = "Automatically enables all enabled mods on next boot!"},
                new ButtonInfo { buttonText = "Save Enabled Buttons", method =() => ShibaGTGenesis.Backend.Back.SaveOnButtons(), isClickable = true, enabled = false, toolTip = "Automatically enables all enabled mods on next boot!"},
                //new ButtonInfo { buttonText = "Load Enabled Buttons", method =() => ShibaGTGenesis.Backend.Back.LoadOnButtons(), enabled = false, toolTip = "Automatically enables all enabled mods on next boot!"},
                new ButtonInfo { buttonText = "Enabled Mods", method =() => ShibaGTGenesis.Backend.Back.EnabledMods(), enabled = false, isClickable = true, toolTip = "Go to enabled!"},
                new ButtonInfo { buttonText = "Favorite Mods", method =() => ShibaGTGenesis.Backend.Back.FavoriteMods(), enabled = false, isClickable = true, toolTip = "Go to favorites!"},
                new ButtonInfo { buttonText = "Custom Plugins", method =() => ShibaGTGenesis.Backend.Back.Plugins(), enabled = false, isClickable = true, toolTip = "Go to plugins!"},
                new ButtonInfo { buttonText = "Settings", method =() => ShibaGTGenesis.Backend.Back.Settings(), enabled = false, isClickable = true, toolTip = "Go to settings!"},
                new ButtonInfo { buttonText = "Mod Presets", method =() => ShibaGTGenesis.Backend.Back.ModPresets(), enabled = false, isClickable = true, toolTip = "Go to mod presets!"},
                new ButtonInfo { buttonText = "OP Mods", method =() => ShibaGTGenesis.Backend.Back.OPMods(), enabled = false, isClickable = true, toolTip = "OP Mods!"},
                new ButtonInfo { buttonText = "Master Mods", method =() => ShibaGTGenesis.Backend.Back.MasterMods(), enabled = false, isClickable = true, toolTip = "OP Mods!"},
                //new ButtonInfo { buttonText = "Projectile Mods", method =() => ShibaGTGenesis.Backend.Back.ProjectileModsFR(), enabled = false, isClickable = true, toolTip = "Projectile Mods!"},
                new ButtonInfo { buttonText = "Room Mods", method =() => ShibaGTGenesis.Backend.Back.RoomMods(), enabled = false, isClickable = true, toolTip = "Room Mods!"},
                new ButtonInfo { buttonText = "World Mods", method =() => ShibaGTGenesis.Backend.Back.WorldMods(), enabled = false, isClickable = true, toolTip = "World Mods!"},
                new ButtonInfo { buttonText = "Visual Mods", method =() => ShibaGTGenesis.Backend.Back.VisualMods(), enabled = false, isClickable = true, toolTip = "Visual Mods!"},
                new ButtonInfo { buttonText = "Player / Movement Mods", method =() => ShibaGTGenesis.Backend.Back.PlayerMods(), enabled = false, isClickable = true, toolTip = "Player Mods!"},
                new ButtonInfo { buttonText = "Soundboard", method =() => ShibaGTGenesis.Backend.Back.ProjectileMods(), enabled = false, isClickable = true, toolTip = "Soundboard!"},
                new ButtonInfo { buttonText = "Legit Mods", method =() => ShibaGTGenesis.Backend.Back.LegitMods(), enabled = false, isClickable = true, toolTip = "Legit Mods!"},
                new ButtonInfo { buttonText = "Rig Mods", method =() => ShibaGTGenesis.Backend.Back.RigMods(), enabled = false, isClickable = true, toolTip = "Rig Mods!"},
                new ButtonInfo { buttonText = "Advantage Mods", method =() => ShibaGTGenesis.Backend.Back.AdvantageMods(), enabled = false, isClickable = true, toolTip = "Adv Mods!"},
            },

            new ButtonInfo[] // 1
            {
                new ButtonInfo { buttonText = "Go Back", method =() => Back.category = 0, isClickable = true, enabled = false, toolTip = "Go back!"},

                new ButtonInfo { buttonText = "Save Main Preferences", method =() => ShibaGTGenesis.Backend.Back.Save(), isClickable = true, enabled = false, toolTip = "Save your settings!"},
                new ButtonInfo { buttonText = "Load Main Preferences", method =() => ShibaGTGenesis.Backend.Back.Load(), isClickable = true, enabled = false, toolTip = "Load your settings!"},

                new ButtonInfo { buttonText = "Save First Preset", method =() => ShibaGTGenesis.Backend.Back.Save1(), isClickable = true, enabled = false, toolTip = "Save your settings!"},
                new ButtonInfo { buttonText = "Load First Preset", method =() => ShibaGTGenesis.Backend.Back.Load1(), isClickable = true, enabled = false, toolTip = "Load your settings!"},

                new ButtonInfo { buttonText = "Save Second Preset", method =() => ShibaGTGenesis.Backend.Back.Save2(), isClickable = true, enabled = false, toolTip = "Save your settings!"},
                new ButtonInfo { buttonText = "Load Second Preset", method =() => ShibaGTGenesis.Backend.Back.Load2(), isClickable = true, enabled = false, toolTip = "Load your settings!"},

                new ButtonInfo { buttonText = "Save Third Preset", method =() => ShibaGTGenesis.Backend.Back.Save3(), isClickable = true, enabled = false, toolTip = "Save your settings!"},
                new ButtonInfo { buttonText = "Load Third Preset", method =() => ShibaGTGenesis.Backend.Back.Load3(), isClickable = true, enabled = false, toolTip = "Load your settings!"},

                //new ButtonInfo { buttonText = "Load Preferences", method =() => ShibaGTGenesis.Backend.Back.Load(), enabled = false, isClickable = true, toolTip = "Load your settings!"},
                new ButtonInfo { buttonText = "Right Hand Menu", oneMethod =() => ShibaGTGenesis.Backend.Settings.RightHand(), oneDisableMethod =() => Settings.offRightHand(), enabled = false, toolTip = "righthand!"},
                new ButtonInfo { buttonText = "Ingame GUI Menu", oneMethod =() => Settings.IngameGUI(), oneDisableMethod =() => Settings.IngameMenu(), enabled = false, toolTip = "righthand!"},
                new ButtonInfo { buttonText = "Legacy GUI", oneMethod =() => ShibaGTGenesis.Backend.Settings.GoldUI(), oneDisableMethod =() => Settings.OFFGoldUI(), enabled = false, toolTip = "chnges gui!"},
                new ButtonInfo { buttonText = "No Categories", method =() => ShibaGTGenesis.Backend.Settings.NoCate(), oneDisableMethod =() => Settings.OFFNoCate(), enabled = false, toolTip = "removes all the categories!"},
                new ButtonInfo { buttonText = "Make Favorite Buttons Into Grip On Click", method =() => ShibaGTGenesis.Backend.Settings.gripfavs(), oneDisableMethod =() => Settings.normfav(), enabled = false, toolTip = "turns off the tooltip buttons!"},
                new ButtonInfo { buttonText = "Turn Favorite Buttons Off", method =() => ShibaGTGenesis.Backend.Settings.turntooltipbuttonsoff(), oneDisableMethod =() => Settings.turntooltipbuttonson(), enabled = false, toolTip = "turns off the tooltip buttons!"},
                new ButtonInfo { buttonText = "See Yourself In Ghost", method =() => ShibaGTGenesis.Backend.Settings.SeeInGhost(), oneDisableMethod =() => Settings.OFFSeeInGhost(), enabled = true, toolTip = "poverseer!"},
                new ButtonInfo { buttonText = "Turn Off OP Mods After Disconnect", method =() => ShibaGTGenesis.Backend.Settings.OPOff(), oneDisableMethod =() => Settings.OPOn(), enabled = false, toolTip = "turn off!"},
                new ButtonInfo { buttonText = "Autosave Preferences", method =() => ShibaGTGenesis.Backend.Settings.Autosave(), oneDisableMethod =() => Settings.OFFAutosave(), enabled = false, toolTip = "turn off!"},
                new ButtonInfo { buttonText = "Lowercase Menu", method =() => ShibaGTGenesis.Backend.Settings.LowercaseMenu(), oneDisableMethod =() => Settings.OFFLowercaseMenu(), enabled = false, toolTip = "makes the menu lowercase!"},
                new ButtonInfo { buttonText = "Invert Menu Opener Button", method =() => ShibaGTGenesis.Backend.Settings.Invert(), oneDisableMethod =() => Settings.Revert(), enabled = false, toolTip = "makes the menu lowercase!"},
                new ButtonInfo { buttonText = "Freeze When In Menu", method =() => ShibaGTGenesis.Backend.Settings.FreezeIn(), oneDisableMethod =() => Settings.AllowMovement(), enabled = false, toolTip = "makes the menu lowercase!"},
                //new ButtonInfo { buttonText = "Lag Power: 2", method =() => Settings.ChangeCrashPower(false), enabled = false, toolTip = "the more crash power you have, the faster youll get kicked!"},
                new ButtonInfo { buttonText = "Move Menu Status Gun", method =() => Settings.MoveStatusGun(), enabled = false, toolTip = "Change Menu X!"},
                //new ButtonInfo { buttonText = "Remove Custom Boards", oneMethod =() => Settings.RemoveBoards(), oneDisableMethod =()=> Settings.AddBoards(), enabled = false, toolTip = "Change Menu X!"},
                //new ButtonInfo { buttonText = "Menu Status Y Offset: 0", method =() => Settings.ChangeY(false), enabled = false, toolTip = "Change Menu Y!"},
                //new ButtonInfo { buttonText = "Menu Status Z Offset: 0", method =() => Settings.ChangeZ(false), enabled = false, toolTip = "Change Menu Z!"},

                new ButtonInfo { buttonText = "Change Menu Layout: ShibaGT", method =() => Settings.ChangeLayout(false), enabled = false, toolTip = "Change layout!"},
                new ButtonInfo { buttonText = "Change Menu First Color: Black", method =() => Settings.ChangeFirstColor(false), enabled = false, toolTip = "Change theme!"},
                new ButtonInfo { buttonText = "Change Menu Second Color: Black", method =() => Settings.ChangeSecondColor(false), enabled = false, toolTip = "Change theme!"},
                new ButtonInfo { buttonText = "Change Menu Button Color: Dark", method =() => Settings.ChangeButtonColor(false), enabled = false, toolTip = "Change theme!"},
                new ButtonInfo { buttonText = "Change Menu Text Off Color: White", method =() => Settings.ChangeTextOffColor(false), enabled = false, toolTip = "Change theme!"},
                new ButtonInfo { buttonText = "Change Menu Text On Color: Magenta", method =() => Settings.ChangeTextOnColor(false), enabled = false, toolTip = "Change theme!"},
                new ButtonInfo { buttonText = "Change Menu Outline Color: Magenta", method =() => Settings.ChangeOutlineColor(false), enabled = false, toolTip = "Change theme!"},
                new ButtonInfo { buttonText = "Outline Menu", method =() => Settings.Outline(), disableMethod =() => Settings.DontOutline(), enabled = false, toolTip = "Rainbow!"},

                //new ButtonInfo { buttonText = "Change Menu Transparency: 0", method =() => Settings.ChangeMenuTransparency(false), enabled = false, toolTip = "Change theme!"},
                new ButtonInfo { buttonText = "Activate Gradient Menu", oneMethod =() => Settings.ActivateGradient(), oneDisableMethod =() => Settings.DisableGradient(), isClickable = false, enabled = false, toolTip = "Change preset!"},
                new ButtonInfo { buttonText = "Change Gradient: Black And Purple", method =() => Settings.ChangeGradient(false), isClickable = true, enabled = false, toolTip = "Change preset!"},

                new ButtonInfo { buttonText = "Activate Custom Image Background", oneMethod =() => Settings.ActivateCustom(), oneDisableMethod =() => Settings.DisableCustom(), isClickable = false, enabled = false, toolTip = "Change preset!"},
                new ButtonInfo { buttonText = "Menu Trail", oneMethod =() => Settings.Trail(), oneDisableMethod =() => Settings.DisableTrail(), isClickable = false, enabled = false, toolTip = "Change preset!"},

                new ButtonInfo { buttonText = "Theme Preset: Dark", method =() => Settings.Preset("dark", "theme preset: dar"), isClickable = true, enabled = false, toolTip = "Change preset!"},
                new ButtonInfo { buttonText = "Theme Preset: Christmas", method =() => Settings.Preset("christmas", "christmas"), isClickable = true, enabled = false, toolTip = "Change preset!"},
                new ButtonInfo { buttonText = "Theme Preset: Halloween", method =() => Settings.Preset("halloween", "halloween"), isClickable = true, enabled = false, toolTip = "Change preset!"},
                new ButtonInfo { buttonText = "Theme Preset: Genesis", method =() => Settings.Preset("genesis", "Theme Preset: genesi"), isClickable = true, enabled = false, toolTip = "Change preset!"},
                new ButtonInfo { buttonText = "Theme Preset: Bubblegum", method =() => Settings.Preset("bubblegum", "bubblegum"), isClickable = true, enabled = false, toolTip = "Change preset!"},
                new ButtonInfo { buttonText = "Theme Preset: Mint", method =() => Settings.Preset("mint", "mint"), isClickable = true, enabled = false, toolTip = "Change preset!"},
                new ButtonInfo { buttonText = "Theme Preset: Yin & Yang", method =() => Settings.Preset("yinyang", "yin & yan"), isClickable = true, enabled = false, toolTip = "Change preset!"},

                new ButtonInfo { buttonText = "Discord RPC", oneMethod =() => WristMenu.Awake(), oneDisableMethod =() => WristMenu.OnDestroy(), enabled = true, toolTip = "Rainbow!"},
                new ButtonInfo { buttonText = "Rainbow Menu", method =() => Settings.RainbowMenu(), disableMethod =() => Settings.OFFRainbowMenu(), enabled = false, toolTip = "Rainbow!"},
                new ButtonInfo { buttonText = "Change Speed: Mosa / Legit", method =() => Settings.ChangeSpeed(false), enabled = false, toolTip = "Change speed!"},
                new ButtonInfo { buttonText = "ESP: Color Code", method =() => Settings.ChangeESP(false), enabled = false, toolTip = "Change esp!"},
                new ButtonInfo { buttonText = "Change Time Of Day: Untouched", method =() => Settings.ChangeTime(false), enabled = false, toolTip = "Change time!"},
                new ButtonInfo { buttonText = "Platforms Type: Normal", method =() => Settings.ChangePlatforms(false), enabled = false, toolTip = "Change platforms!"},
                new ButtonInfo { buttonText = "Debug Own RPCs", method =() => Settings.DebugRPCS(), oneDisableMethod =() => Settings.OFFDebugRPCS(), enabled = false, toolTip = "Rainbow!"},
                new ButtonInfo { buttonText = "Sticky Platforms", method =() => Settings.StickyPlatforms(), oneDisableMethod =() => Settings.OFFStickyPlatforms(), enabled = false, toolTip = "Rainbow!"},
                new ButtonInfo { buttonText = "Disable Menu Status In Stump", method =() => Settings.DisableStatus(), oneDisableMethod =() => Settings.OFFDisableStatus(), enabled = false, toolTip = "Rainbow!"},
                new ButtonInfo { buttonText = "Disable Voice Commands (say 'genesis')", method =() => Settings.DisableVoice(), oneDisableMethod =() => Settings.EnableVoice(), enabled = false, toolTip = "Rainbow!"},
                new ButtonInfo { buttonText = "Let Others Hear Voice Commands", method =() => Settings.OthersHear(), oneDisableMethod =() => Settings.OFFOthersHear(), enabled = false, toolTip = "Rainbow!"},
               // new ButtonInfo { buttonText = "Projectile Mods Projectile: Snowball", method =() => Settings.ChangeProjectile(false), enabled = false, toolTip = "Change prpojecitles!"},
                new ButtonInfo { buttonText = "Risky Mods", method =() => ShibaGTGenesis.Backend.Back.RiskyMods(), isClickable = true, enabled = false, toolTip = "turn off!"},
            },

            new ButtonInfo[] // 2
            {
                new ButtonInfo { buttonText = "Go Back", method = () => Back.category = 0, isClickable = true, enabled = false, toolTip = "Go back!" },
                //new ButtonInfo { buttonText = "Particle Gun", method = () => OP.ParticleGun(), isClickable = false, enabled = false, toolTip = "particle!" },
                //new ButtonInfo { buttonText = "Particle Around Self", method = () => OP.ParticleAroundSelf(), isClickable = false, enabled = false, toolTip = "particle!" },
                //new ButtonInfo { buttonText = "Particle Around Player Gun", method = () => OP.ParticleAroundPlayerGun(), isClickable = false, enabled = false, toolTip = "particle!" },
                //new ButtonInfo { buttonText = "Wear Cosmetics Out Of Tryon SS", method = () => OP.newthing(), isClickable = false, disableMethod = () => OP.newthingoff(), enabled = false, toolTip = "HOW TO USE: Go to the tryon room, put on ur cosmetics, click this button, walk out and everyone can see ur cosmetics (reuse every city lobby, and only in city)!" },
                //new ButtonInfo { buttonText = "Save Tryon Cosmetics", method = () => OP.saveTryon(), isClickable = true, enabled = false, toolTip = "" },
                //new ButtonInfo { buttonText = "Load Tryon Cosmetics", method = () => OP.loadTryon(), isClickable = true, enabled = false, toolTip = "" },
               // new ButtonInfo { buttonText = "UnLoad Tryon Cosmetics", method = () => OP.unloadTryon(), isClickable = true, enabled = false, toolTip = "" },

                //new ButtonInfo { buttonText = "rass", method = () => OP.CrashTest(), isClickable = false, enabled = false, toolTip = "" },
                //new ButtonInfo { buttonText = "trigger comseotxs", method = () => OP.AllCosmeticsTest(), isClickable = false, enabled = false, toolTip = "H" },
                //new ButtonInfo { buttonText = "Super Infection Kick Master", method =() => OP.SuperInfectionKickMasterClient(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Super Infection Kick All", method =() => OP.SuperInfectionKickAll(), isClickable = false, enabled = false, toolTip = "crashes everyone"},

                new ButtonInfo { buttonText = "File Get Info On Player Gun", method =() => OP.InfoGun(true), enabled = false, toolTip = "info"},
                new ButtonInfo { buttonText = "Notif Get Info On Player Gun", method =() => OP.InfoGun(false), enabled = false, toolTip = "info"},
                //new ButtonInfo { buttonText = "activate all cosmetixcs", method = () => OP.AllCosmeticsTest(), isClickable = false, enabled = false, toolTip = "H" },
                //new ButtonInfo { buttonText = "Stack Cosmetics", method = () => OP.StackCosmetics(), isClickable = false, enabled = false, toolTip = "H" },
                //new ButtonInfo { buttonText = "Antiban", method = () => ShibaGTGenesis.Backend.OP.Antiban(), isClickable = true, enabled = false, toolTip = "Antiban!" },
                //new ButtonInfo { buttonText = "Auto Antiban <color=yellow>[USEFUL]</color>", method = () => ShibaGTGenesis.Backend.OP.AutoAntiban(), isClickable = false, enabled = false, toolTip = "Antiban!" },
                //new ButtonInfo { buttonText = "Antiban Status", method = () => ShibaGTGenesis.Backend.OP.AntibanStatus(), enabled = false, isClickable = true, toolTip = "Antiban!" },
                //new ButtonInfo { buttonText = "Set Master", method = () => ShibaGTGenesis.Backend.OP.SetMaster(), enabled = false, isClickable = true, toolTip = "set maste4r!" },
                //new ButtonInfo { buttonText = "Fling All From Point Gun", method =() => OP.FlingAllFromPoint(), isClickable = false, enabled = false, toolTip = "crashes everyone"},

                //new ButtonInfo { buttonText = "SS Size Changer [t] [a to reset]", method =() => OP.SizeChanger(), disableMethod =()=> OP.DSizeChanger(), isClickable = false, enabled = false, toolTip = "crashes everyone"},

                //new ButtonInfo { buttonText = "Paper Plane Spam [rg]", method = () => OP.PaperplaneSpam(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Paper Plane Gun", method = () => OP.PaperplaneGun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
               // new ButtonInfo { buttonText = "Paper Plane Minigun [rg]", method = () => OP.PaperplaneMinigun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Paper Plane Still [rg]", method =() => OP.PlaneStill(), isClickable = false, enabled = false, toolTip = "crashes everyone"});

               // new ButtonInfo { buttonText = "Fat Paper Plane Spam [rg]", method = () => OP.FatPlaneSpam(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Fat Paper Plane Gun", method = () => OP.FatPlaneGun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Fat Paper Plane Minigun [rg]", method = () => OP.FatPlaneMinigun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Fat Paper Plane Still [rg]", method =() => OP.FatStill(), isClickable = false, enabled = false, toolTip = "crashes everyone"});

                //new ButtonInfo { buttonText = "Heart Star Spam [rg]", method =() => OP.HeartStarSpam(), isClickable = false, enabled = false, toolTip = "crashes everyone"});
                //new ButtonInfo { buttonText = "Heart Star Gun", method = () => OP.HeartStarGun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Heart Star Minigun [rg]", method = () => OP.HeartStarMinigun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Heart Star Still [rg]", method =() => OP.HeartStarStill(), isClickable = false, enabled = false, toolTip = "crashes everyone"});

                //new ButtonInfo { buttonText = "Plane Bomb [rg]", method = () => OP.SpazPlanes(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Plane Wind [rg]", method = () => OP.PlaneWind(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Plane Shotgun [rg]", method = () => OP.PaperplaneShotgun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },

                //new ButtonInfo { buttonText = "Plane AK47 [gs]", method = () => OP.PlaneSpray(25) ,isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Plane Spray [gs]", method = () => OP.PlaneSpray(300), isClickable = false, enabled = false, toolTip = "crashes everyone" },

               // new ButtonInfo { buttonText = "Plane Swastika Spam [rg]", method = () => OP.PaperPlaneSwastikaSpam(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Plane Swastika Gun [rg]", method = () => OP.PaperPlaneSwastikaGun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Plane Text Name [rg]", method = () => OP.PaperPlaneText(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Plane Text Name Gun", method = () => OP.PaperPlaneTextGun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },

                //new ButtonInfo { buttonText = "Both Planes Minigun [grips]", method = () => OP.BothMinigun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },

                //new ButtonInfo { buttonText = "Beta Grab All Hoverboards [rg]", method = () => OP.DumpSoundData(), isClickable = true, enabled = false, toolTip = "crashes everyone" },

               // new ButtonInfo { buttonText = "Firecracker Spam [rg]", method = () => OP.FirecrackerSpam(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
               // new ButtonInfo { buttonText = "Firecracker Gun", method = () => OP.FirecrackerGun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Firecracker Minigun [rg]", method = () => OP.FirecrackerMinigun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },

                //new ButtonInfo { buttonText = "Bomb Spam [rg]", method = () => OP.BombSpam(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Bomb Gun", method = () => OP.BombGun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Bomb Minigun [rg]", method = () => OP.BombMinigun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },

                //new ButtonInfo { buttonText = "Plane Lag All [lt]", method = () => OP.PlaneCrashAll(), isClickable = false, enabled = false, toolTip = "crashes everyone" },
                //new ButtonInfo { buttonText = "Beta Kick Gun", method = () => OP.BetaKickGun(), isClickable = false, enabled = false, toolTip = "crashes everyone" },

                //new ButtonInfo { buttonText = "Paper Plane Minigun Gun", method =() => OP.PaperPlaneMiniGunGun(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Paper Plane Lag All [rg]", method =() => OP.PaperPlaneCrashAll(), isClickable = false, enabled = false, toolTip = "crashes everyone"},

               // new ButtonInfo { buttonText = "Lag Gun", method =() => OP.CrashGun(0.1f, 50), isClickable = false, enabled = false, toolTip = "crashes everyone"},
               // new ButtonInfo { buttonText = "Lag All", method =() => OP.CrashAll(0.1f, 50), isClickable = false, enabled = false, toolTip = "crashes everyone"},

               // new ButtonInfo { buttonText = "Crash All v1", method =() => OP.CrashAll(1f, 450), isClickable = false, enabled = false, toolTip = "crashes everyone"},
               // new ButtonInfo { buttonText = "Crash Gun v2", method =() => OP.CrashGun(1f, 450), isClickable = false, enabled = false, toolTip = "crashes everyone"},

               // new ButtonInfo { buttonText = "Crash All v2", method =() => OP.CrashAll(4.5f, 1900), isClickable = false, enabled = false, toolTip = "crashes everyone"},
               // new ButtonInfo { buttonText = "Crash Gun v2", method =() => OP.CrashGun(4.5f, 1900), isClickable = false, enabled = false, toolTip = "crashes everyone"},

                new ButtonInfo { buttonText = "Fling Gun", method =() => OP.FlingGun(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Fling Gun 2", method =() => OP.FlingGun2(), isClickable = false, enabled = false, toolTip = "crashes everyone"},

                //new ButtonInfo { buttonText = "Huge Void Above Stump [rt] [ss]", method =() => OP.HugeVoid(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Huge Laughing Emoji Above Stump [rt] [ss]", method =() => OP.HugeLaughingEmoji(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Huge Tongue Emoji Above Stump [rt] [ss]", method =() => OP.HugeTongueEmoji(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Huge Heart Eyes Above Stump [rt] [ss]", method =() => OP.HugeHeartEyesEmoji(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Huge Custom Emoji Above Stump [rt] [ss]", method =() => OP.HugeCustomEmoji(), oneDisableMethod =()=> OP.EnableCustom(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Huge SUB Above Stump [rt] [ss]", method =() => OP.HugeText(), isClickable = false, enabled = false, toolTip = "crashes everyone"},

                //new ButtonInfo { buttonText = "Firecracker Spam [g]", method =() => OP.FirecrackerSpam(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Firecracker Gun", method =() => OP.FirecrackerGun(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Firecracker Minigun [g]", method =() => OP.FirecrackerMinigun(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Firecracker Minigun Fast [g]", method =() => OP.FirecrackerMinigunFast(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Firecracker Rain [rg]", method =() => OP.FirecrackerRain(), isClickable = false, enabled = false, toolTip = "crashes everyone"},

                new ButtonInfo { buttonText = "Stump VFX Spammer [rt]", method =() => OP.ArcadeTeleSpam(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                new ButtonInfo { buttonText = "Arcade VFX Spammer [rt]", method =() => OP.StumpTeleSpam(), isClickable = false, enabled = false, toolTip = "crashes everyone"},

                //new ButtonInfo { buttonText = "Lucy Sounds", method =() => OP.EnableLucy(), isClickable = true, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Noclip Gun [m]", method =() => OP.NoClipGun(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Move Lucy Gun [m]", method =() => OP.MoveSkeletonGun(), isClickable = false, enabled = false, toolTip = "crashes everyone"},

                new ButtonInfo { buttonText = "Snowball Launcher [rg]", method =() => OP.SnowballLauncher(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                new ButtonInfo { buttonText = "Snowball Gun", method =() => OP.SnowballGun(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                new ButtonInfo { buttonText = "Snowball Rain [rg]", method =() => OP.SnowballRain(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Snowball Speedboost Giver Gun", method =() => OP.SnowballSpeedboostGiverGun(), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Spawn Massive Snowball [rg]", method =() => OP.RollingProj(), isClickable = false, enabled = false, toolTip = "crashes everyone"},

                /*
                new ButtonInfo { buttonText = "Barrier Block Gun [m]", method =() => OP.BlockBestGun(false), enabled = false, toolTip = "crashes everyone"},
                new ButtonInfo { buttonText = "Barrier Sphere [m]", method =() => OP.SpawnSphere(), isClickable = true, enabled = false, toolTip = "crashes everyone"},
                new ButtonInfo { buttonText = "SS Fan Sphere [m]", method =() => OP.SpawnFANSphere(), isClickable = true, enabled = false, toolTip = "crashes everyone"},
                new ButtonInfo { buttonText = "SS Fan Gun [m]", method =() => OP.FanGun(), isClickable = false, enabled = false, toolTip = "crashes everyone"},


                new ButtonInfo { buttonText = "Barrier Block Hand [rg] [m]", method =() => OP.BlockBestHand(false), enabled = false, toolTip = "crashes everyone"},
                new ButtonInfo { buttonText = "Trap Gun [m]", method =() => OP.TrapGun(), enabled = false, toolTip = "crashes everyone"},
                new ButtonInfo { buttonText = "Barrier Block Antireport [m] [rg]", method =() => OP.BlockAntireport(), enabled = false, toolTip = "crashes everyone"},
                new ButtonInfo { buttonText = "Master Crash All [m]", method =() => OP.MasterCrashAll(), enabled = false, toolTip = "crashes everyone"},
                */

               // new ButtonInfo { buttonText = "Kick All", method = () => OP.KickAll(), isClickable = true, enabled = false, toolTip = "a" },
               // new ButtonInfo { buttonText = "Kick Gun", method = () => OP.KickGun(), isClickable = false, enabled = false, toolTip = "a" },
                //new ButtonInfo { buttonText = "Kick On Touch", method = () => OP.KickTouch(), isClickable = false, enabled = false, toolTip = "a" },
               // new ButtonInfo { buttonText = "Kick On Your Touch", method = () => OP.KickOnYouTouch(), isClickable = false, enabled = false, toolTip = "a" },
                //new ButtonInfo { buttonText = "Kick On Your Report", method = () => OP.KickOnReport(), isClickable = false, enabled = false, toolTip = "a" },
               // new ButtonInfo { buttonText = "Anti Kick", oneMethod = () => AntiKick.enabled = true, oneDisableMethod =()=> AntiKick.enabled = false, enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },

                //new ButtonInfo { buttonText = "TP To Virtual Stump", method =() => OP.TPToVirtual(), isClickable = true, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Unload Custom Map For All [rg]", method =() => OP.DespawnMapAll(), enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Unload Custom Map For Gun", method =() => OP.DespawnMapGun(), enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Lag All In Custom Map", method =() => OP.LagCustom(), enabled = false, toolTip = "crashes everyone"},


                

                //new ButtonInfo { buttonText = "Instant Crash All", method = () => ShibaGTGenesis.Backend.OP.KillIRLAll(), enabled = false, isClickable = false, toolTip = "grabs everyone!" },
                //new ButtonInfo { buttonText = "Instant Crash Gun", method = () => ShibaGTGenesis.Backend.OP.KillIRLGun(), enabled = false, isClickable = false, toolTip = "grabs everyone!" },

                //new ButtonInfo { buttonText = "lucy thing", oneMethod = () => ShibaGTGenesis.Backend.OP.EnableLucy(), enabled = false, isClickable = false, toolTip = "grabs everyone!" },
                //new ButtonInfo { buttonText = "fat2", method = () => ShibaGTGenesis.Backend.OP.NoClipGun2(), enabled = false, isClickable = false, toolTip = "grabs everyone!" },

                //new ButtonInfo { buttonText = "Black Screen All", method = () => ShibaGTGenesis.Backend.OP.BlackAll(), enabled = false, isClickable = false, toolTip = "grabs everyone!" },
                //new ButtonInfo { buttonText = "Black Screen Gun", method = () => ShibaGTGenesis.Backend.OP.BlackGun(), enabled = false, isClickable = false, toolTip = "grabs everyone!" },
                //new ButtonInfo { buttonText = "Black Screen On Your Touch", method = () => ShibaGTGenesis.Backend.OP.KillIRLOnYouTouch(), enabled = false, isClickable = false, toolTip = "grabs everyone!" },
               // new ButtonInfo { buttonText = "Black Screen On Your Report", method = () => ShibaGTGenesis.Backend.OP.KillOnReport(), enabled = false, isClickable = false, toolTip = "grabs everyone!" },

                //new ButtonInfo { buttonText = "Anti Black Screen / Crash", oneMethod = () => ShibaGTGenesis.Backend.World.AntiCrash(), oneDisableMethod =()=> World.AllowCrash(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },

                //new ButtonInfo { buttonText = "Instant Crash All [lt]", method = () => OP.ElfCrash(WristMenu.triggerDownL), isClickable = false, enabled = false, toolTip = "ban" },
                //new ButtonInfo { buttonText = "Instant Crash Gun", method = () => OP.ElfCrashGun(), isClickable = false, enabled = false, toolTip = "ban" },
                //new ButtonInfo { buttonText = "Anti Instant Crash", oneMethod = () => OP.AntiCrash(), oneDisableMethod =()=> OP.CrashAllow(), isClickable = false, enabled = false, toolTip = "ban" },

               
               // new ButtonInfo { buttonText = "Elf Gun", method = () => OP.ElfSpamGun(2), isClickable = false, enabled = false, toolTip = "ban" },
                //new ButtonInfo { buttonText = "Elf Gun Fast", method = () => OP.ElfSpamGun(5), isClickable = false, enabled = false, toolTip = "ban" },
                //new ButtonInfo { buttonText = "Elf Gun Super", method = () => OP.ElfSpamGun(12), isClickable = false, enabled = false, toolTip = "ban" },

               // new ButtonInfo { buttonText = "Elf Rain Close [rg]", method = () => OP.ElfSpam(OP.RainPosGen(), Vector3.zero, WristMenu.gripDownR), isClickable = false, enabled = false, toolTip = "ban" },
                //new ButtonInfo { buttonText = "Elf Rain Forest [rg]", method = () => OP.ElfSpam(OP.RainForGen(), Vector3.zero, WristMenu.gripDownR), isClickable = false, enabled = false, toolTip = "ban" },

                //new ButtonInfo { buttonText = "Elf Fountain [rg]", method = () => OP.ElfSpam(GorillaLocomotion.GTPlayer.Instance.transform.position + new Vector3(0, 1, 0), OP.FountainGen(), WristMenu.gripDownR), isClickable = false, enabled = false, toolTip = "ban" },



                //new ButtonInfo { buttonText = "Instant Crash All [lt]", method = () => OP.ElfCrash(WristMenu.triggerDownL), isClickable = false, enabled = false, toolTip = "ban" },
                //new ButtonInfo { buttonText = "Instant Crash Gun", method = () => OP.ElfCrashGun(), isClickable = false, enabled = false, toolTip = "ban" },

                //new ButtonInfo { buttonText = "Break Movement All [LT]", method = () => ShibaGTGenesis.Backend.OP.fuckingkilleveryone(), enabled = false, isClickable = false, toolTip = "gives u platforms too!" },
                //new ButtonInfo { buttonText = "Freeze Gun", method = () => ShibaGTGenesis.Backend.OP.sillyGun(), enabled = false, isClickable = false, toolTip = "gives u platforms too!" },
                //new ButtonInfo { buttonText = "Crash Gun", method = () => ShibaGTGenesis.Backend.OP.ermmkk(), enabled = false, isClickable = false, toolTip = "Master!" },
                //new ButtonInfo { buttonText = "Remove Self From Leaderboard", method = () => ShibaGTGenesis.Backend.OP.RemoveSelfFromBoard(), disableMethod=()=> OP.OffRemoveSelf(), enabled = false, isClickable = false, toolTip = "hhhhh!" },
                //new ButtonInfo { buttonText = "Break Infection [NO MASTER]", method = () => ShibaGTGenesis.Backend.OP.BreakLobby(), enabled = false, isClickable = true, toolTip = "no way1!!" },
                //new ButtonInfo { buttonText = "Fix Infection [NO MASTER]", method = () => ShibaGTGenesis.Backend.OP.FixLobby(), enabled = false, isClickable = true, toolTip = "no way1!!" },
                //new ButtonInfo { buttonText = "Make Infection Rock [NO MASTER]", method = () => ShibaGTGenesis.Backend.OP.BreakLobby(), enabled = false, isClickable = true, toolTip = "no way1!!" },
                //new ButtonInfo { buttonText = "Make Rock Infection [NO MASTER]", method = () => ShibaGTGenesis.Backend.OP.FixLobby(), enabled = false, isClickable = true, toolTip = "no way1!!" },
                //new ButtonInfo { buttonText = "Freeze Gun", method = () => ShibaGTGenesis.Backend.OP.FloatGun2(), enabled = false, isClickable = false, toolTip = "Master!" },
                //new ButtonInfo { buttonText = "Float Gun", method = () => ShibaGTGenesis.Backend.OP.FloatGun(), enabled = false, isClickable = false, toolTip = "Master!" },
                //new ButtonInfo { buttonText = "tp test", method = () => ShibaGTGenesis.Backend.OP.testingg(), enabled = false, isClickable = false, toolTip = "Master!" },
                //new ButtonInfo { buttonText = "lock room", method =() => OP.emableskib(), isClickable = false, enabled = false, toolTip = "ban"},
                //new ButtonInfo { buttonText = "Kick Gun", method =() => OP.emableskib2(), isClickable = false, enabled = false, toolTip = "ban"},
                //new ButtonInfo { buttonText = "lag all test [lt]", method =() => OP.emableskib3(), isClickable = false, enabled = false, toolTip = "ban"},
                //new ButtonInfo { buttonText = "false", method =() => OP.servertimestampPatch = false, isClickable = false, enabled = false, toolTip = "ban"},
                //new ButtonInfo { buttonText = "true", method =() => OP.servertimestampPatch = true, isClickable = false, enabled = false, toolTip = "ban"},
                //new ButtonInfo { buttonText = "Delay Ban Gun", method =() => OP.BanGun(), enabled = false, toolTip = "bans the guy"},
                //new ButtonInfo { buttonText = "Kick Gun", method =() => OP.KickGun(), enabled = false, toolTip = "crashes the player"},
               // new ButtonInfo { buttonText = "Crash Gun", method =() => OP.QuitGun(), enabled = false, toolTip = "crashes"},
                //new ButtonInfo { buttonText = "Ultra Fling Gun", method =() => OP.UltraFlingGun(), enabled = false, toolTip = "crashes"},
                //new ButtonInfo { buttonText = "Crash All", method =() => OP.CrashAll(), enabled = false, toolTip = "crashes"},
               // new ButtonInfo { buttonText = "Crash Gun", method =() => OP.CrashGun(), enabled = false, toolTip = "crashes"},
                //new ButtonInfo { buttonText = "Crash On Touch", method =() => OP.CrashGun(), enabled = false, toolTip = "crashes"},
               // new ButtonInfo { buttonText = "Crash On Your Touch", method =() => OP.CrashOnYouTouch(), enabled = false, toolTip = "crashes"},
                //new ButtonInfo { buttonText = "Lag Gun", method =() => OP.CrashGun(), enabled = false, toolTip = "crashes"},

               // new ButtonInfo { buttonText = "Crash All", method = () => ShibaGTGenesis.Backend.OP.c204(2.5f, 990), enabled = false, isClickable = false, toolTip = "grabs everyone!" },
               // new ButtonInfo { buttonText = "Crash Gun", method = () => ShibaGTGenesis.Backend.OP.c204G(2.5f, 990), enabled = false, isClickable = false, toolTip = "grabs everyone!" },

                new ButtonInfo { buttonText = "Lag All", method = () => ShibaGTGenesis.Backend.OP.FaggotLag(445, 1f), enabled = false, isClickable = false, toolTip = "grabs everyone!" },
                new ButtonInfo { buttonText = "Lag Gun", method = () => ShibaGTGenesis.Backend.OP.FaggotLagGun(445, 1f), enabled = false, isClickable = false, toolTip = "grabs everyone!" },

                new ButtonInfo { buttonText = "Guardian Crash All", method = () => ShibaGTGenesis.Backend.OP.CrashAllGuard(), enabled = false, isClickable = true, toolTip = "grabs everyone!" },
                new ButtonInfo { buttonText = "Guardian Crash Gun", method = () => ShibaGTGenesis.Backend.OP.CrashGunGuard(), enabled = false, isClickable = false, toolTip = "grabs everyone!" },

                //new ButtonInfo { buttonText = "Instant Crash All", method = () => ShibaGTGenesis.Backend.OP.CamCrashAll(0), enabled = false, isClickable = false, toolTip = "grabs everyone!" },
                //new ButtonInfo { buttonText = "Instant Crash Gun", method = () => ShibaGTGenesis.Backend.OP.CamCrashGun(0), enabled = false, isClickable = false, toolTip = "grabs everyone!" },

                //new ButtonInfo { buttonText = "Kick/Domain All", method = () => ShibaGTGenesis.Backend.OP.KickAll(), enabled = false, isClickable = true, toolTip = "grabs everyone!" },
                //new ButtonInfo { buttonText = "Kick/Domain Gun", method = () => ShibaGTGenesis.Backend.OP.KickGun(), enabled = false, isClickable = false, toolTip = "grabs everyone!" },


                //new ButtonInfo { buttonText = "Intense Lag Master [lt]", method =() => OP.CrashMaster(), enabled = false, toolTip = "crashes the master"},
                //new ButtonInfo { buttonText = "Block Intense Lag All", method =() => OP.BlockLagAll(), enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Block Intense Lag Gun", method =() => OP.BlockLagGun(), enabled = false, toolTip = "crashes everyone"},
                


                //new ButtonInfo { buttonText = "BAN Gun [lt]", method =() => OP.BanGun(), enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Lag All 2 [lt]", method =() => OP.LagAll2(), enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Lag Gun 2", method =() => OP.LagGun2(), isClickable = false, enabled = false, toolTip = "crashes the person u shoot"},
                //new ButtonInfo { buttonText = "Lag Gun 2 On Your Touch", method =() => OP.Lag2OnYouTouch(), isClickable = false, enabled = false, toolTip = "crashes the person u shoot"},
                //new ButtonInfo { buttonText = "Lag Gun 2 On Touch", method =() => OP.Lag2Touch(), isClickable = false, enabled = false, toolTip = "crashes the person u shoot"},

                //new ButtonInfo { buttonText = "SS Mute All [lt]", method =() => OP.MuteAllSS(), enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "SS Mute Gun", method =() => OP.MuteGunSS(), enabled = false, toolTip = "crashes everyone"},

                //new ButtonInfo { buttonText = "PRIV Lag All", method =() => OP.LagView2(null, true), isClickable = false, enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Rope Lag All [lt]", method =() => World.sillyropefortest(), enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Rope Lag Gun", method =() => World.sillyropefortestgun(), enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Lag On Touch", method =() => OP.LagTouch(), enabled = false, toolTip = "crashes when someone touches u"},
                //new ButtonInfo { buttonText = "Lag On YOUR Touch", method =() => OP.LagOnYouTouch(), enabled = false, toolTip = "crashes someone that u touch"},
                //new ButtonInfo { buttonText = "Lag Aura", method =() => OP.LagAura(), enabled = false, toolTip = "crash"},
                //new ButtonInfo { buttonText = "Drain Battery All [lt]", method =() => OP.LagAll(), enabled = false, toolTip = "literally just the crasher :skull:"},
                //new ButtonInfo { buttonText = "Drain Battery Gun", method =() => OP.LagGun(), enabled = false, toolTip = "literally just the crasher :skull:"},
                new ButtonInfo { buttonText = "Kick All Modders", method =() => OP.KickModders(), isClickable = true, enabled = false, toolTip = "kicks them if they have antireport on!"},

                new ButtonInfo { buttonText = "Freeze All", method = () => ShibaGTGenesis.Backend.OP.silly2(), enabled = false, isClickable = false, toolTip = "gives u platforms too!" },
                //new ButtonInfo { buttonText = "PRIVATE Freeze All", method = () => ShibaGTGenesis.Backend.OP.silly2(), enabled = false, isClickable = false, toolTip = "gives u platforms too!" },
                //new ButtonInfo { buttonText = "Kick All (gotta wait a bit)", method = () => ShibaGTGenesis.Backend.OP.silly3(), enabled = false, isClickable = false, toolTip = "gives u platforms too!" },

                //new ButtonInfo { buttonText = "Activate Live Event [ss if m]", method =() => OP.LiveEvent(true), isClickable = true, enabled = false, toolTip = "act!"},
                //new ButtonInfo { buttonText = "Deactivate Live Event [ss if m]", method =() => OP.LiveEvent(false), isClickable = true, enabled = false, toolTip = "act!"},

                //new ButtonInfo { buttonText = "Break Lobby", method = () => ShibaGTGenesis.Backend.OP.BreakLobby(), enabled = false, isClickable = false, toolTip = "fwefewfefwfewfwefefewf!" },
                //new ButtonInfo { buttonText = "Gamemode Spam <color=red>[DELAY BAN]</color>", method =() => OP.GMSpam(), enabled = false, toolTip = "Spams gamemodes"},
                //new ButtonInfo { buttonText = "Mat Spam All", method =() => OP.MatAll(), enabled = false, toolTip = "Spams materials"},
                //new ButtonInfo { buttonText = "Mat Spam Gun", method =() => OP.MatGun(), enabled = false, toolTip = "Spams materials on a player"},
                //new ButtonInfo { buttonText = "Break Gamemode", method =() => OP.BreakGamemode(), enabled = false, toolTip = "Breaks the game / tag!"},
                //new ButtonInfo { buttonText = "Change Gamemode To Infection <color=red>[DELAY BAN]</color>", method =() => OP.ChangeGamemode("Infection"), isClickable = true, enabled = false, toolTip = "Change gamemode!"},
                //new ButtonInfo { buttonText = "Change Gamemode To Casual <color=red>[DELAY BAN]</color>", method =() => OP.ChangeGamemode("Casual"), isClickable = true, enabled = false, toolTip = "Change gamemode!"},
                //new ButtonInfo { buttonText = "Change Gamemode To Hunt <color=red>[DELAY BAN]</color>", method =() => OP.ChangeGamemode("Hunt"), isClickable = true, enabled = false, toolTip = "Change gamemode!"},
                //new ButtonInfo { buttonText = "Change Gamemode To Battle <color=red>[DELAY BAN]</color>", method =() => OP.ChangeGamemode("Battle"), isClickable = true, enabled = false, toolTip = "Change gamemode!"},
                //new ButtonInfo { buttonText = "set mastero", method =() => OP.SetMaster(), isClickable = true, enabled = false, toolTip = "Change gamemode!"},
                new ButtonInfo { buttonText = "Destroy All", method =() => AdvMods.DestroyAll(), isClickable = true, enabled = false, toolTip = "tells you what it does when u enable it!"},
                new ButtonInfo { buttonText = "Destroy Gun", method =() => AdvMods.DestroyGun(), isClickable = false, enabled = false, toolTip = "tells you what it does when u enable it!"},
                //new ButtonInfo { buttonText = "Slow All", method =() => AdvMods.SlowAll(), isClickable = false, enabled = false, toolTip = "Slow all!"},
                //new ButtonInfo { buttonText = "Slow Gun", method =() => AdvMods.SlowGun(), isClickable = false, enabled = false, toolTip = "Slow all!"},
                //new ButtonInfo { buttonText = "Vibrate All", method =() => AdvMods.VibrateAll(), isClickable = false, enabled = false, toolTip = "Slow all!"},
                //new ButtonInfo { buttonText = "Vibrate Gun", method =() => AdvMods.VibrateGun(), isClickable = false, enabled = false, toolTip = "Slow all!"},
                  //new ButtonInfo { buttonText = "Name Change All", method = () => ShibaGTGenesis.Backend.OP.NameAll(), enabled = false, toolTip = "Name changer!" },
                //new ButtonInfo { buttonText = "Name Change Gun", method = () => ShibaGTGenesis.Backend.OP.NameGun(), enabled = false, toolTip = "Name changer!" },


                //new ButtonInfo { buttonText = "ban all", method =() => OP.CrashAll(), enabled = false, toolTip = "sigamrizza"},
            },

            new ButtonInfo[] // 3
            {
                new ButtonInfo { buttonText = "Go Back", method = () => ShibaGTGenesis.Backend.Back.RoomMods(), isClickable = true, enabled = false, toolTip = "Go back!" },
                new ButtonInfo { buttonText = "Join Code GENESIS", method = () => ShibaGTGenesis.Backend.Room.JoinGenesis(), isClickable = true, enabled = false, toolTip = "Join the menus code!" },
                new ButtonInfo { buttonText = "B to Disconnect", method = () => ShibaGTGenesis.Backend.Room.BDisconnect(), isClickable = false, enabled = false, toolTip = "B to leave!" },
                //new ButtonInfo { buttonText = "Create Public Room", method = () => ShibaGTGenesis.Backend.Room.CreatePublic(), isClickable = true, enabled = false, toolTip = "Create Public!" },
                new ButtonInfo { buttonText = "Rejoin Last Joined Lobby", method = () => ShibaGTGenesis.Backend.Room.RejoinLast(), isClickable = true, enabled = false, toolTip = "Rejoin!" },
                new ButtonInfo { buttonText = "Serverhop", method = () => ShibaGTGenesis.Backend.Room.Serverhop(), enabled = false, isClickable = false, toolTip = "Serverhop!" },
                new ButtonInfo { buttonText = "Reconnect", method = () => ShibaGTGenesis.Backend.Room.Rejoin(), isClickable = true, enabled = false, toolTip = "rejoin!" },
                new ButtonInfo { buttonText = "Cancel Reconnect", method = () => ShibaGTGenesis.Backend.Room.CancelRejoin(), isClickable = true, enabled = false, toolTip = "rejoin!" },
                new ButtonInfo { buttonText = "Disable Network Triggers [cs]", method = () => ShibaGTGenesis.Backend.Room.DisableTriggers(), oneDisableMethod = () => Room.OffDisableTriggers(), enabled = false, toolTip = "Go into other maps in pubs!" },
                new ButtonInfo { buttonText = "Visible Network Triggers [cs]", oneMethod = () => ShibaGTGenesis.Backend.Room.VisibleTrigs(), oneDisableMethod = () => Room.NonVisTrigs(), enabled = false, toolTip = "Go into other maps in pubs!" },
                new ButtonInfo { buttonText = "AntiReport", method = () => ShibaGTGenesis.Backend.Room.AntiReport(), enabled = true, toolTip = "Antireport!" },
                new ButtonInfo { buttonText = "Oculus AntiReport", method = () => ShibaGTGenesis.Backend.Room.oculisantireport(), enabled = false, toolTip = "Antireport!" },
                new ButtonInfo { buttonText = "Rejoin AntiReport", method = () => ShibaGTGenesis.Backend.Room.AntiReportRejoin(), oneDisableMethod =()=> Room.OffAntiRejoin(), enabled = false, toolTip = "Antireport!" },
                new ButtonInfo { buttonText = "Serverhop AntiReport", method = () => ShibaGTGenesis.Backend.Room.AntiReportServerhop(), oneDisableMethod =()=> Room.OffAntiServer(), enabled = false, toolTip = "Antireport!" },
                new ButtonInfo { buttonText = "Visualize Antireport", oneMethod =()=> Room.AntiReportVis(), method = () => ShibaGTGenesis.Backend.Room.AntiReportVis(), oneDisableMethod =()=> Room.OFFAntireportVis(), enabled = false, toolTip = "Antireport!" },
                new ButtonInfo { buttonText = "Mute Gun", method = () => ShibaGTGenesis.Backend.Room.MuteGun(), enabled = false, toolTip = "ez!" },
                new ButtonInfo { buttonText = "Mute All", method = () => ShibaGTGenesis.Backend.Room.MuteAll(), isClickable = true, enabled = false, toolTip = "ez!" },
                new ButtonInfo { buttonText = "Hide Name On Leaderboard", method = () => ShibaGTGenesis.Backend.Room.HideName(), enabled = false, toolTip = "Hide!" },
                new ButtonInfo { buttonText = "Anti Moderator", method = () => ShibaGTGenesis.Backend.Room.AntiModerator(), enabled = true, toolTip = "ez!" },
                //new ButtonInfo { buttonText = "Add To Party", method = () => ShibaGTGenesis.Backend.Room.ADDParty(), enabled = false, toolTip = "ez!" },
                new ButtonInfo { buttonText = "Kick Party Members To Code", method = () => ShibaGTGenesis.Backend.Room.KickPartyMembers(), enabled = false, toolTip = "ez!" },
                new ButtonInfo { buttonText = "Kick Party Members To Saved Code", method = () => ShibaGTGenesis.Backend.Room.KickPartyMembersSaved(), enabled = false, toolTip = "ez!" },
            },

            new ButtonInfo[] // 4 World
            {
                new ButtonInfo { buttonText = "Go Back", method = () => ShibaGTGenesis.Backend.Back.WorldMods(), isClickable = true, enabled = false, toolTip = "Go back!" },
                new ButtonInfo { buttonText = "Disguise", method =() => World.Disguise(), isClickable = false, enabled = false, toolTip = "disguise!"},
                new ButtonInfo { buttonText = "No Gravity", method =() => World.NoGravity(), enabled = false, toolTip = "No gravity!"},
                new ButtonInfo { buttonText = "Low Gravity", method =() => World.LowGravity(), enabled = false, toolTip = "Low gravity, like the moon!"},
                new ButtonInfo { buttonText = "High Gravity", method =() => World.HighGravity(), enabled = false, toolTip = "High gravity!"},
                new ButtonInfo { buttonText = "Reverse Gravity", method =() => World.ReverseGravity(), enabled = false, toolTip = "only vr!"},
                new ButtonInfo { buttonText = "Gravity Wind", method =() => World.GravityWind(), enabled = false, toolTip = "Random!"},
                new ButtonInfo { buttonText = "Gravity Switcher [g]", method =() => World.GravitySwitcher(), enabled = false, toolTip = "Switcher!"},

                new ButtonInfo { buttonText = "Full Invis Self", oneMethod =() => World.FullInvis(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Stop Gun [M]", method =() => World.StopGun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Stop All [M]", method =() => World.StopAll(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Barrel Spam Gun [M]", method =() => World.BarrelSpamGun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Crate Spam Gun [M]", method =() => World.CrateSpamGun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Wood Spam Gun [M]", method =() => World.WoodSpamGun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Flower Spam Gun [M]", method =() => World.FlowerSpamGun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Light Bulb Spam Gun [M]", method =() => World.LightBulbSpamGun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Skeleton Spam Gun [M]", method =() => World.SkeletonSpamGun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Monster Spam Gun [M]", method =() => World.MonsterSpamGun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Baby Skeleton Spam Gun [M]", method =() => World.BabySkeletonSpamGun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Gate Spam Gun [M]", method =() => World.GateSpamGun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Storm Spam Gun [M]", method =() => World.StormSpamGun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Dupe Cores [M]", method =() => World.DupeCores(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Dupe Flashers [M]", method =() => World.DupeFlashers(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Dupe Lanterns [M]", method =() => World.DupeLanterns(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Dupe Reviver [M]", method =() => World.DupeReviver(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Dupe Collectors [M]", method =() => World.DupeCollectors(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Dupe Lanterns [M]", method =() => World.DupeLanterns(), enabled = false, toolTip = "Switcher!"},

                new ButtonInfo { buttonText = "Max Out Quest Points", method = () => ShibaGTGenesis.Backend.OP.MaxQuestPoints(), enabled = false, isClickable = true, toolTip = "Sets your quest score to 99999." },
                new ButtonInfo { buttonText = "Max Rank", method = () => ShibaGTGenesis.Backend.OP.MaxRank(), enabled = false, isClickable = true, toolTip = "Gives you the max banana rank." },

                new ButtonInfo { buttonText = "Critter Spammer [rg]", method =() => World.CritterSpammer(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Critter Minigun [rg]", method =() => World.CritterMinigun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Critter Gun", method =() => World.CritterGun(), enabled = false, toolTip = "Switcher!"},

                new ButtonInfo { buttonText = "Stunbomb Spammer [rg]", method =() => World.StunBombSpammer(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Stunbomb Minigun [rg]", method =() => World.StunBombMinigun(), enabled = false, toolTip = "Switcher!"},
                new ButtonInfo { buttonText = "Stunbomb Gun", method =() => World.StunBombGun(), enabled = false, toolTip = "Switcher!"},

                //new ButtonInfo { buttonText = "Destroy All Blocks", method = () => ShibaGTGenesis.Backend.OP.DestroyBlocks(), enabled = false, isClickable = false, toolTip = "moves all nearby blocks to the guns pos" },

                //new ButtonInfo { buttonText = "Copy Grabbed Block ID", method = () => ShibaGTGenesis.Backend.OP.CopyPieceID(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                //new ButtonInfo { buttonText = "Copy Grabbed Block", method = () => ShibaGTGenesis.Backend.OP.CopyPiece(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                //new ButtonInfo { buttonText = "Copy Block Gun", method = () => ShibaGTGenesis.Backend.OP.CopyPieceGun(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },

                //new ButtonInfo { buttonText = "Paste Block Gun", method = () => ShibaGTGenesis.Backend.OP.GrabAllPieces(true), enabled = false, isClickable = false, toolTip = "moves all nearby blocks to the guns pos" },
                //new ButtonInfo { buttonText = "Paste Block Gun", method = () => ShibaGTGenesis.Backend.OP.GrabAllPieces(true), enabled = false, isClickable = false, toolTip = "moves all nearby blocks to the guns pos" },
                //new ButtonInfo { buttonText = "Paste Block Halo", method = () => ShibaGTGenesis.Backend.OP.PiecesHalo(true), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                //new ButtonInfo { buttonText = "Paste Block Vomit", method = () => ShibaGTGenesis.Backend.OP.PiecesVomit(true), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },

                //new ButtonInfo { buttonText = "Random Building Block Gun", method = () => ShibaGTGenesis.Backend.OP.GrabAllPieces(false), enabled = false, isClickable = false, toolTip = "moves all nearby blocks to the guns pos" },
                //new ButtonInfo { buttonText = "Random Building Block Halo", method = () => ShibaGTGenesis.Backend.OP.PiecesHalo(false), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                //new ButtonInfo { buttonText = "Building Block Rain", method = () => ShibaGTGenesis.Backend.OP.BlockRain(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                //new ButtonInfo { buttonText = "Random Building Block Vomit", method = () => ShibaGTGenesis.Backend.OP.PiecesVomit(false), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },

                //new ButtonInfo { buttonText = "Block Push Gun", method = () => ShibaGTGenesis.Backend.OP.FloatGun(false), enabled = false, isClickable = false, toolTip = "moves all nearby blocks to the guns pos" },

                new ButtonInfo { buttonText = "Guardian All", method = () => ShibaGTGenesis.Backend.OP.GuardianAll(), enabled = false, isClickable = true, toolTip = "attempts to become guardian!" },

                //new ButtonInfo { buttonText = "Eject Guardian", method = () => ShibaGTGenesis.Backend.OP.EjectGuardian(), enabled = false, isClickable = true, toolTip = "attempts to become guardian!" },

                new ButtonInfo { buttonText = "Grab Gun", method = () => ShibaGTGenesis.Backend.OP.GrabGun(), enabled = false, isClickable = false, toolTip = "grabs the player!" },
                new ButtonInfo { buttonText = "Grab All [rg]", method = () => ShibaGTGenesis.Backend.OP.GrabAll(), enabled = false, isClickable = false, toolTip = "grabs everyone!" },
                new ButtonInfo { buttonText = "Drop All [rt]", method = () => ShibaGTGenesis.Backend.OP.ReleaseAll(), enabled = false, isClickable = false, toolTip = "grabs everyone!" },
                new ButtonInfo { buttonText = "Fling All", method = () => ShibaGTGenesis.Backend.OP.FlingAll2(), enabled = false, isClickable = true, toolTip = "grabs everyone!" },
                new ButtonInfo { buttonText = "Fling All Held", method = () => ShibaGTGenesis.Backend.OP.FlingAll(), enabled = false, isClickable = true, toolTip = "grabs everyone!" },


                                new ButtonInfo { buttonText = "Both Effects Gun", method = () => ShibaGTGenesis.Backend.OP.BothEffectsGun(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                new ButtonInfo { buttonText = "Slam Effect Gun", method = () => ShibaGTGenesis.Backend.OP.SlamEffectsGun(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                new ButtonInfo { buttonText = "Slap Effect Gun", method = () => ShibaGTGenesis.Backend.OP.SlapEffectsGun(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },

                new ButtonInfo { buttonText = "Both Effects [rg]", method = () => ShibaGTGenesis.Backend.OP.BothEffects(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                new ButtonInfo { buttonText = "Slam Effect [rg]", method = () => ShibaGTGenesis.Backend.OP.SlamEffect(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                new ButtonInfo { buttonText = "Slap Effect [rg]", method = () => ShibaGTGenesis.Backend.OP.SlapEffect(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },

                new ButtonInfo { buttonText = "Both Effects Halo", method = () => ShibaGTGenesis.Backend.OP.BothEffectsHalo(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                new ButtonInfo { buttonText = "Slam Effect Halo", method = () => ShibaGTGenesis.Backend.OP.SlamEffectsHalo(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                new ButtonInfo { buttonText = "Slap Effect Halo", method = () => ShibaGTGenesis.Backend.OP.SlapEffectsHalo(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },

                //new ButtonInfo { buttonText = "Spawn Blue Lucy [m]", method =() => World.SpawnLucy(Color.blue), isClickable = true, enabled = false, toolTip = "lucy!"},
                //new ButtonInfo { buttonText = "Spawn Red Lucy [m]", method =() => World.SpawnLucy(Color.red), isClickable = true, enabled = false, toolTip = "lucy!"},
                //new ButtonInfo { buttonText = "Spawn Black Lucy [m]", method =() => World.SpawnLucy(Color.black), isClickable = true, enabled = false, toolTip = "lucy!"},
               // new ButtonInfo { buttonText = "Despawn Lucy [m]", method =() => World.DespawnLucy(), isClickable = true, enabled = false, toolTip = "lucy!"},
                //new ButtonInfo { buttonText = "Lucy Grab Player Gun [m]", method =() => World.LucyGrabGun(), enabled = false, toolTip = "lucy!"},
                //new ButtonInfo { buttonText = "Lucy Gun [m]", method =() => World.LucyGun(), enabled = false, toolTip = "lucy!"},
                //new ButtonInfo { buttonText = "Become Lucy [m]", method =() => World.BecomeLucy(), enabled = false, toolTip = "lucy!"},
                //new ButtonInfo { buttonText = "Slow Lucy [m]", method =() => World.SlowLucy(), enabled = false, toolTip = "lucy!"},
                //new ButtonInfo { buttonText = "Fast Lucy [m]", method =() => World.FastLucy(), enabled = false, toolTip = "lucy!"},
               // new ButtonInfo { buttonText = "Fix Lucy [m]", method =() => World.FixLucy(), isClickable = true, enabled = false, toolTip = "lucy!"},

                //new ButtonInfo { buttonText = "Fast Brooms [m]", method =() => World.FastBrooms(), enabled = false, toolTip = "lucy!"},
                //new ButtonInfo { buttonText = "Slow Brooms [m]", method =() => World.SlowBrooms(), enabled = false, toolTip = "lucy!"},
                //new ButtonInfo { buttonText = "Freeze Brooms [m]", method =() => World.FreezeBrooms(), enabled = false, toolTip = "lucy!"},
                //new ButtonInfo { buttonText = "Spaz Brooms [m]", method =() => World.SpazBrooms(), enabled = false, toolTip = "lucy!"},
                //new ButtonInfo { buttonText = "Fix Brooms [m]", method =() => World.FixBroomSpeed(), isClickable = true, enabled = false, toolTip = "lucy!"},

                new ButtonInfo { buttonText = "Make Player Crazy Gun", method =() => World.CrazyGun(), enabled = false, toolTip = "crazy!"},
                new ButtonInfo { buttonText = "Disable QuitBox", method =() => World.DisableQuitbox(), oneDisableMethod =() => World.OnQuitbox(), enabled = false, toolTip = "crazy!"},
                new ButtonInfo { buttonText = "Teleport To Stump When Out Of Map", method =() => World.QuitboxMod(), oneDisableMethod =() => World.OffQuitboxMod(), enabled = true, toolTip = "crazy!"},

                //new ButtonInfo { buttonText = "Grab Ball [rg]", method =() => World.GrabBall(), enabled = false, toolTip = "spazes!"},
               // new ButtonInfo { buttonText = "Ball Gun", method =() => World.BallGun(), enabled = false, toolTip = "spazes!"},
                //new ButtonInfo { buttonText = "Despawn Ball", method =() => World.BreakBall(), isClickable = true, enabled = false, toolTip = "spazes!"},

                //new ButtonInfo { buttonText = "Spaz Ropes [lt]", method =() => World.SpazRopes(), enabled = false, toolTip = "spazes!"},
                //new ButtonInfo { buttonText = "Anti Grav Ropes [lt]", method =() => World.AntiGravRopes(), enabled = false, toolTip = "spazes!"},
                //new ButtonInfo { buttonText = "Ropes Up [lt]", method =() => World.UpRopes(), enabled = false, toolTip = "spazes!"},
                //new ButtonInfo { buttonText = "Ropes Down [lt]", method =() => World.DownRopes(), enabled = false, toolTip = "spazes!"},
                //new ButtonInfo { buttonText = "Ropes Toward You [lt]", method =() => World.RopesToward(), enabled = false, toolTip = "spazes!"},
                //new ButtonInfo { buttonText = "Silly Ropes", method =() => World.SillyRopes(), enabled = false, toolTip = "whenever someone grabs a rope, the rope they grab spazes out only for them!"},
                //new ButtonInfo { buttonText = "Silly Ropes Gun", method =() => World.SillyRopesGun(), enabled = false, toolTip = "whenever someone grabs a rope, the rope they grab spazes out only for them!"},
                //new ButtonInfo { buttonText = "Ropes Gun", method =() => World.RopesGun(), enabled = false, toolTip = "spazes!"},

               // new ButtonInfo { buttonText = "Select Ropes Gun <color=green>[NEW]</color>", method =() => World.SelectRopeGun(), enabled = false, toolTip = "spazes!"},
               // new ButtonInfo { buttonText = "Spaz Selected Ropes <color=green>[NEW]</color>", method =() => World.SpazSelectedRopes(), enabled = false, toolTip = "spazes!"},
               // new ButtonInfo { buttonText = "Unselect Ropes <color=green>[NEW]</color>", method =() => World.UnselectRopes(), isClickable = true, enabled = false, toolTip = "spazes!"},
                new ButtonInfo { buttonText = "Blind Gun", method =() => World.GliderBlindGun(), enabled = false, toolTip = "glider!"},
                new ButtonInfo { buttonText = "Blind All", method =() => World.GliderBlindAll(), enabled = false, toolTip = "glider!"},
                new ButtonInfo { buttonText = "Glider Gun", method =() => World.GliderGun(), enabled = false, toolTip = "glider!"},
                new ButtonInfo { buttonText = "Glider Spaz", method =() => World.GliderSpaz(), enabled = false, toolTip = "glider!"},
                new ButtonInfo { buttonText = "Glider Halo", method =() => World.GliderHalo(), enabled = false, toolTip = "glider!"},
                new ButtonInfo { buttonText = "Glider Dick", method =() => World.GliderDick(), enabled = false, toolTip = "glider!"},
                new ButtonInfo { buttonText = "L Waterbending [lg]", method =() => World.LBend(), enabled = false, toolTip = "grip bend!"},
                new ButtonInfo { buttonText = "R Waterbending [rg]", method =() => World.RBend(), enabled = false, toolTip = "grip bend!"},
                new ButtonInfo { buttonText = "Water Player Aura [lt]", method =() => World.WaterPlayerAura(), enabled = false, toolTip = "aura!"},
                new ButtonInfo { buttonText = "Water Aura [lt]", method =() => World.WaterAura(), enabled = false, toolTip = "aura!"},
                new ButtonInfo { buttonText = "Tap All Cave Crystals", method =() => World.TapCrystals(), isClickable = true, enabled = false, toolTip = "tap!"},
                new ButtonInfo { buttonText = "Break All Arcade Machines", method =() => World.breakallmachines(), enabled = false, toolTip = "crazy!"},

                //new ButtonInfo { buttonText = "Grab Bug", method =() => World.GrabBug(), isClickable = false, enabled = false, toolTip = "grabs the bug!"},
                //new ButtonInfo { buttonText = "Bug Gun", method =() => World.BugGun(), isClickable = false, enabled = false, toolTip = "moves the bug!"},
                //new ButtonInfo { buttonText = "Bug Halo", method =() => World.BugHalo(), isClickable = false, enabled = false, toolTip = "halos the bug!"},
                ////new ButtonInfo { buttonText = "Give Bug Gun", method =() => World.GiveBugGun(), isClickable = false, enabled = false, toolTip = "give the bug to whoever!"},
                //new ButtonInfo { buttonText = "Break Bug", method =() => World.BreakBug(), isClickable = false, enabled = false, toolTip = "breaks the bug!"},
                //new ButtonInfo { buttonText = "Fast Bug", method =() => World.FastBug(), isClickable = false, enabled = false, toolTip = "make the bug fast!"},
                //new ButtonInfo { buttonText = "Slow Bug", method =() => World.SlowBug(), isClickable = false, enabled = false, toolTip = "make the bug slow!"},
                //new ButtonInfo { buttonText = "Fix Bug Speed", method =() => World.FixBug(), isClickable = false, enabled = false, toolTip = "make the bug slow!"},
                //new ButtonInfo { buttonText = "Do Not Respawn Bug", method =() => World.DoNotRespawnBug(), isClickable = false, enabled = false, toolTip = "doesnt respawn the bug!"},
                //new ButtonInfo { buttonText = "Ride Bug", method =() => World.RideBug(), isClickable = false, enabled = false, toolTip = "makes u ride the bug!"},
                //new ButtonInfo { buttonText = "Spaz Bug", method =() => World.SpazBug(), isClickable = false, enabled = false, toolTip = "spazs the bug!"},
                //new ButtonInfo { buttonText = "Revert The LGBTQ Update", method =() => World.ezrevert(), isClickable = true, enabled = false, toolTip = "because the pillows are intrusive!"},
               // new ButtonInfo { buttonText = "Spam Gates", method =() => World.GateSpam(), isClickable = false, enabled = false, toolTip = "gates!"},
            },



            new ButtonInfo[] // 5
            {
                new ButtonInfo { buttonText = "Go Back", method = () => ShibaGTGenesis.Backend.Back.VisualMods(), isClickable = true, enabled = false, toolTip = "Go back!" },
                new ButtonInfo { buttonText = "Clear Notifications", method =() => GTAG_NotificationLib.NotifiLib.ClearAllNotifications(), enabled = false, toolTip = "clear notifs!"},
                new ButtonInfo { buttonText = "Turn Off Notifications", enabled = false, toolTip = "clear notifs!"},
                new ButtonInfo { buttonText = "Always BepInEx Debug", method =() =>  Debug.unityLogger.logEnabled = true, enabled = false, toolTip = "clear notifs!"},
               //new ButtonInfo { buttonText = "FPC", method =() => Visual.FPC(), oneDisableMethod =() => Visual.OFFFPC(), enabled = false, toolTip = "Only pc, first person camera!"},
                new ButtonInfo { buttonText = "Arraylist", enabled = true, toolTip = "Only pc!"},
                new ButtonInfo { buttonText = "VR Arraylist", method =() => Visual.VRArraylist(), oneDisableMethod=() => Visual.OFFVRArraylist(), enabled = false, toolTip = "Only vr!"},
                new ButtonInfo { buttonText = "Visible FPS On GUI", oneMethod =() => Visual.fpsvisible(), oneDisableMethod=() => Visual.fpsinvisible(), enabled = false, toolTip = "Only vr!"},
                new ButtonInfo { buttonText = "ESP", method = () => ShibaGTGenesis.Backend.Visual.ESP(), oneDisableMethod = () => Visual.OFFESP(), enabled = false, toolTip = "ESP!" },
                new ButtonInfo { buttonText = "Guardian ESP", method = () => ShibaGTGenesis.Backend.Visual.GuardianESP(), oneDisableMethod = () => Visual.OFFGuardianESP(), enabled = false, toolTip = "ESP!" },
                //new ButtonInfo { buttonText = "Meteor Tracers", method = () => ShibaGTGenesis.Backend.Visual.MeteorTRACERS(), disableMethod = () => Visual.OFFMeteorTRACERS(), enabled = false, toolTip = "ESP!" },
                new ButtonInfo { buttonText = "Breadcrumbs", method = () => ShibaGTGenesis.Backend.Visual.BreadCrumbs(), oneDisableMethod=()=> Visual.offBreadcrumbs(), enabled = false, toolTip = "trail!" },
                new ButtonInfo { buttonText = "Voice ESP", method = () => ShibaGTGenesis.Backend.Visual.VoiceESP(), oneDisableMethod = () => Visual.OFFVoiceESP(), enabled = false, toolTip = "Voice ESP!" },
                new ButtonInfo { buttonText = "Bone ESP", method =() => Visual.StartSkeleEsp(), oneDisableMethod =() => Visual.EndSkeleEsp(), enabled = false, toolTip = "Shibas favorite."},
                new ButtonInfo { buttonText = "Nametags All", method =() => Visual.NameTags(false), oneDisableMethod =() => Visual.DeleteTags(false), enabled = false, toolTip = "Shibas favorite."},
                new ButtonInfo { buttonText = "Nametags Others", method =() => Visual.NameTags(true), oneDisableMethod =() => Visual.DeleteTags(true), enabled = false, toolTip = "Shibas favorite."},
                new ButtonInfo { buttonText = "Tracers", method =() => Visual.Tracers(), oneDisableMethod =() => Visual.OFFTracers(), enabled = false, toolTip = "Very epic!"},
                new ButtonInfo { buttonText = "Box ESP", method =() => Visual.BoxEsp(), oneDisableMethod =() => Visual.OFFBoxEsp(), enabled = false, toolTip = "Box esp!"},
                new ButtonInfo { buttonText = "Bottom ESP", method =() => Visual.bottomhandler(), oneDisableMethod =() => Visual.OFFBottomHandler(), enabled = false, toolTip = "Bottom esp!"},
                new ButtonInfo { buttonText = "Head ESP", method =() => Visual.HeadESP(), oneDisableMethod =()=> Visual.OffHeadESP(), enabled = false, toolTip = "Head esp!"},
                new ButtonInfo { buttonText = "Shiba Gun [cs]", method =() => Visual.ShibaGun(), enabled = false, toolTip = "Head esp!"},
                new ButtonInfo { buttonText = "Custom Image Gun [cs]", method =() => Visual.CustomImageGun(), enabled = false, toolTip = "Head esp!"},
                new ButtonInfo { buttonText = "Custom Image Ball [rg] [cs]", method =() => Visual.CustomImageBall(), enabled = false, toolTip = "Head esp!"},
                new ButtonInfo { buttonText = "Accept TOS", method =() => Visual.AcceptTOS(), oneDisableMethod =()=> Visual.UnacceptTOS(), enabled = false, toolTip = "Head esp!"},
                new ButtonInfo { buttonText = "Stress Ball [cs]", method =() => Visual.StressBall(), oneDisableMethod =() => Visual.OffStress(), enabled = false, toolTip = "Stress ball!"},
                new ButtonInfo { buttonText = "Anti Lag", method =() => Visual.AntiLag(), isClickable = true, enabled = false, toolTip = "disables some things tjhat might lag!"},
                //new ButtonInfo { buttonText = "Material Gun", method =() => Visual.MaterialGun(), isClickable = false, enabled = false, toolTip = "disables  rain!"},
                new ButtonInfo { buttonText = "Disable Rain", method =() => Visual.DisableRain(), isClickable = true, enabled = false, toolTip = "disables  rain!"},
                new ButtonInfo { buttonText = "Disable Leaves", method =() => Visual.DisableLeaves(), oneDisableMethod =()=> Visual.EnableLeaves(), isClickable = false, enabled = false, toolTip = "disables leaves!"},
            },

            new ButtonInfo[] // 6
            {
                new ButtonInfo { buttonText = "Go Back", method = () => ShibaGTGenesis.Backend.Back.PlayerMods(), isClickable = true, enabled = false, toolTip = "Go back!" },
                new ButtonInfo { buttonText = "Speed Boost", oneMethod = () => ShibaGTGenesis.Backend.PlayerMovement.SpeedBoost(), oneDisableMethod = () => PlayerMovement.DisableSpeedBoost(), enabled = false, toolTip = "Speed boost!" },
                new ButtonInfo { buttonText = "Pull Mod [rg]", oneMethod = () => ShibaGTGenesis.Backend.PlayerMovement.PullMod(), enabled = false, toolTip = "Speed boost!" },
                new ButtonInfo { buttonText = "Really Long Arms", method = () => ShibaGTGenesis.Backend.PlayerMovement.ReallyArms(), enabled = false, toolTip = "Really long arms!" },
                new ButtonInfo { buttonText = "Punch Mod", method = () => ShibaGTGenesis.Backend.PlayerMovement.PunchMod(), enabled = false, toolTip = "punch!" },
                //new ButtonInfo { buttonText = "Snowball Punch Mod Others", method = () => ShibaGTGenesis.Backend.PlayerMovement.PunchModSnowball(), enabled = false, toolTip = "punch!" },
                //new ButtonInfo { buttonText = "Tubski", method = () => ShibaGTGenesis.Backend.PlayerMovement.BecomePunchi(), oneDisableMethod =()=> PlayerMovement.PunchiDisable(), enabled = false, toolTip = "punch!" },
                new ButtonInfo { buttonText = "RGB [stump]", method = () => ShibaGTGenesis.Backend.PlayerMovement.RGB(false), enabled = false, toolTip = "punch!" },
                //new ButtonInfo { buttonText = "RGB [out of stump]", method = () => ShibaGTGenesis.Backend.PlayerMovement.OutOfStumpRGB(false), enabled = false, toolTip = "punch!" },
                new ButtonInfo { buttonText = "Strobe [stump]", method = () => ShibaGTGenesis.Backend.PlayerMovement.RGB(true), enabled = false, toolTip = "punch!" },
                new ButtonInfo { buttonText = "Hoverboard Launcher [rg] [ss]", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardLauncher(), isClickable = false, enabled = false, toolTip = "Platforms!" },
                new ButtonInfo { buttonText = "Fast Hoverboard [rg] [ss]", method = () => ShibaGTGenesis.Backend.PlayerMovement.FastHoverboard(), isClickable = false, enabled = false, toolTip = "Platforms!" },
                new ButtonInfo { buttonText = "Spawn Black Hoverboard [ss]", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardSigma(Color.black), isClickable = true, enabled = false, toolTip = "Platforms!" },
                new ButtonInfo { buttonText = "Spawn Color Code Hoverboard [ss]", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardSigma(GorillaTagger.Instance.offlineVRRig.playerColor), isClickable = true, enabled = false, toolTip = "Platforms!" },
                new ButtonInfo { buttonText = "Give Hoverboard [ss]", method = () => ShibaGTGenesis.Backend.PlayerMovement.GrabHoverboardFR(), isClickable = true, enabled = false, toolTip = "Platforms!" },
                new ButtonInfo { buttonText = "Destroy Own Hoverboards [ss]", method = () => ShibaGTGenesis.Backend.PlayerMovement.GrabHoverboard(), isClickable = true, enabled = false, toolTip = "Platforms!" },
                new ButtonInfo { buttonText = "Destroy Dropped Nearby Hoverboards [ss]", method = () => ShibaGTGenesis.Backend.PlayerMovement.GrabHoverboards(), isClickable = false, enabled = false, toolTip = "Platforms!" },
                new ButtonInfo { buttonText = "Give Gay Hoverboard [ss]", method = () => ShibaGTGenesis.Backend.PlayerMovement.GayHoverboard(), isClickable = false, enabled = false, toolTip = "Platforms!" },
                new ButtonInfo { buttonText = "Rainbow Hoverboards", oneMethod = () => ShibaGTGenesis.Backend.PlayerMovement.RainbowHoverboardsOn(), oneDisableMethod = () => ShibaGTGenesis.Backend.PlayerMovement.RainbowHoverboardsOff(), enabled = false, toolTip = "Hoverboards spawn rainbow."},
                new ButtonInfo { buttonText = "Hoverboard Aura", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardAura(), enabled = false, toolTip = "Two hoverboards orbit around you."},
                new ButtonInfo { buttonText = "Hoverboard Tornado", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardTornado(), enabled = false, toolTip = "Hoverboards spiral up around you."},
                new ButtonInfo { buttonText = "Hoverboard Trail", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardTrail(), enabled = false, toolTip = "Drops hoverboards behind you as you walk."},
                new ButtonInfo { buttonText = "Hoverboard Rain", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardRain(), enabled = false, toolTip = "Drops hoverboards from the sky onto you."},
                new ButtonInfo { buttonText = "Hoverboard Snipe Gun", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardSnipe(), isClickable = false, enabled = false, toolTip = "Shoots a hoverboard at the point."},
                new ButtonInfo { buttonText = "Hoverboard Up Launcher", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardUpLauncher(), enabled = false, toolTip = "Launches hoverboards straight up."},
                new ButtonInfo { buttonText = "Hoverboard Hand Track", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardHandTrack(), enabled = false, toolTip = "Glues a hoverboard to your left hand."},
                new ButtonInfo { buttonText = "Hoverboard Annoy Gun", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardAnnoyGun(), isClickable = false, enabled = false, toolTip = "Drops a hoverboard on the target's head."},
                new ButtonInfo { buttonText = "Hoverboard Cannon", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardCannon(), enabled = false, toolTip = "Continuously launches hoverboards forward from your right hand."},
                new ButtonInfo { buttonText = "Hoverboard Spam", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardSpam(), enabled = false, toolTip = "Continuously drops hoverboards at your right hand."},
                new ButtonInfo { buttonText = "Hoverboard Stack", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardStack(), enabled = false, toolTip = "Stacks rising hoverboards above you."},
                new ButtonInfo { buttonText = "Hoverboard Sides", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardPulse(), enabled = false, toolTip = "Pulses hoverboards out from your sides."},
                new ButtonInfo { buttonText = "Hoverboard Sides 2", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardTagAlong(), enabled = false, toolTip = "Two hoverboards follow at your sides."},
                new ButtonInfo { buttonText = "Hoverboard Follow Gun", method = () => ShibaGTGenesis.Backend.PlayerMovement.HoverboardFollowGun(), isClickable = false, enabled = false, toolTip = "Drops a hoverboard wherever your gun is pointing."},
                new ButtonInfo { buttonText = "Platforms", method = () => ShibaGTGenesis.Backend.PlayerMovement.PlatformsThing(PlayerMovement.invisplat, Settings.sticky), isClickable = false, enabled = false, toolTip = "Platforms!" },
                new ButtonInfo { buttonText = "Noclip [t]", method =() => PlayerMovement.NoClip(false), enabled = false, toolTip = "Press trigger for noclip!"},
                new ButtonInfo { buttonText = "Loud Handtaps", oneMethod =() => PlayerMovement.LoudHandtaps(), oneDisableMethod =() => PlayerMovement.OFFLoudHandtaps(), enabled = false, toolTip = "Makes your handtaps louder!"},
                new ButtonInfo { buttonText = "No Handtaps", oneMethod =() => PlayerMovement.NoHandtaps(), oneDisableMethod =() => PlayerMovement.OFFNoHandtaps(), enabled = false, toolTip = "Removes ur handtap sounds!"},
                new ButtonInfo { buttonText = "Fly", method =() => PlayerMovement.Fly(), enabled = false, toolTip = "Press secondary to fly!"},
                new ButtonInfo { buttonText = "Helicoptor Fly", method =() => PlayerMovement.HelicopterFly(), enabled = false, toolTip = "Press secondary to fly!"},
                new ButtonInfo { buttonText = "Trigger Fly", method =() => PlayerMovement.TriggerFly(), enabled = false, toolTip = "Press trigger to fly!"},
                new ButtonInfo { buttonText = "Noclip Trigger Fly", method =() => PlayerMovement.NoClipTriggerFly(), enabled = false, toolTip = "Press trigger to fly!"},
                new ButtonInfo { buttonText = "Bark Fly", method =() => PlayerMovement.BarkFly(), oneDisableMethod =() => PlayerMovement.OffBarkFly(), enabled = false, toolTip = "Just like bark!"},
                new ButtonInfo { buttonText = "Iron Monkey", method =() => PlayerMovement.IronMonke(), enabled = false, toolTip = "ironman!"},
                new ButtonInfo { buttonText = "Slingshot Fly", method =() => PlayerMovement.SlingshotFly(), enabled = false, toolTip = "slingshot, from the OG menu!"},
                new ButtonInfo { buttonText = "Fast Slingshot Fly", method =() => PlayerMovement.FastSlingshotFly(), enabled = false, toolTip = "slingshot, from the OG menu!"},
                new ButtonInfo { buttonText = "Talk Through Siren [t]", method = () => ShibaGTGenesis.Backend.PlayerMovement.SirenTalk(), enabled = false, toolTip = "siren!" },
                new ButtonInfo { buttonText = "Banana Car", method =() => PlayerMovement.BananaCar(), enabled = false, toolTip = "Car (joystick)!"},
                new ButtonInfo { buttonText = "TP Gun", method =() => PlayerMovement.TeleportGun(), enabled = false, toolTip = "Teleport to the pointer!"},
                new ButtonInfo { buttonText = "TP To Random", method =() => PlayerMovement.TeleportRandom(), isClickable = true, enabled = false, toolTip = "TP!"},
                new ButtonInfo { buttonText = "Spider Monkey", method =() => PlayerMovement.SpiderMonke(), disableMethod =() => PlayerMovement.DisableSpiderMonke(), enabled = false, toolTip = "Spiderman!"},
                new ButtonInfo { buttonText = "Auto Run [lg]", method =() => PlayerMovement.AutoRun(), enabled = false, toolTip = "Run silly!"},
                new ButtonInfo { buttonText = "Swim Everywhere", method =() => PlayerMovement.SwimEverywhere(), disableMethod =() => PlayerMovement.OFFSwimEverywhere(), enabled = false, toolTip = "Swim!"},
                new ButtonInfo { buttonText = "Walk On Water", method =() => PlayerMovement.WalkOnWater(), disableMethod =() => PlayerMovement.OFFWalkOnWater(), enabled = false, toolTip = "Walk on water!"},
                new ButtonInfo { buttonText = "Bhop [kingofnetflix]", method =() => PlayerMovement.Bhop(), enabled = false, toolTip = "Minecraft!"},
                new ButtonInfo { buttonText = "Fling", method =() => PlayerMovement.Fling(), isClickable = true, enabled = false, toolTip = "Minecraft!"},
            },

            new ButtonInfo[] // 7
            {
                new ButtonInfo { buttonText = "Go Back", method = () => ShibaGTGenesis.Backend.Back.LegitMods(), isClickable = true, enabled = false, toolTip = "Go back!" },
                new ButtonInfo { buttonText = "Steam Long Arms", method = () => ShibaGTGenesis.Backend.Legit.SteamArms(), oneDisableMethod = () => Legit.DisableSteamArms(), enabled = false, toolTip = "Steam long arms!" },
                new ButtonInfo { buttonText = "No Fingers", method = () => ShibaGTGenesis.Backend.Legit.NoFingers(), enabled = false, toolTip = "fingeer!" },
                new ButtonInfo { buttonText = "60 HZ", method =() => Legit.HZ(), enabled = false, toolTip = "Slide!"},
                new ButtonInfo { buttonText = "Wall Walk [lg]", method = () => ShibaGTGenesis.Backend.Legit.WallWalk(), enabled = false, toolTip = "Walk on walls!" },
                new ButtonInfo { buttonText = "Fake Oculus Menu [b]", method =() => Legit.FakeOculus(), enabled = false, toolTip = "Put your hands on your chest first!"},
                new ButtonInfo { buttonText = "Remove Wind Barriers", method =() => Legit.RemoveWind(), oneDisableMethod =() => Legit.OFFRemoveWind(), enabled = false, toolTip = "No more barriers!"},
                new ButtonInfo { buttonText = "Fake Lag", method =() => Legit.FakeLag(), oneDisableMethod =() => Legit.OFFFakeLag(), enabled = false, toolTip = "Lag!"},
               // new ButtonInfo { buttonText = "Rasidi Settings", method =() => Legit.Ras(), disableMethod =() => Legit.OFFRas(), enabled = false, toolTip = "Lag!"},
                new ButtonInfo { buttonText = "Desync", method =() => Legit.Desync(), oneDisableMethod =() => Legit.OFFDesync(), enabled = false, toolTip = "Lag!"},
                new ButtonInfo { buttonText = "Macro [lt to rec, rt to play]", method =() => Legit.Macro(false), enabled = false, toolTip = "Lag!"},
                new ButtonInfo { buttonText = "Out Of Body Macro [lt to rec, rt to play]", method =() => Legit.Macro(true), enabled = false, toolTip = "Lag!"},
                new ButtonInfo { buttonText = "Clear Macro", method =() => Legit.ClearMacro(), isClickable = true, enabled = false, toolTip = "Lag!"},
                new ButtonInfo { buttonText = "Slide Control", method =() => Legit.SlideControl(), oneDisableMethod =() => Legit.OFFSlideControl(), enabled = false, toolTip = "Slide!"},
            },

            new ButtonInfo[] // 8
            {
                new ButtonInfo { buttonText = "Go Back", method = () => ShibaGTGenesis.Backend.Back.ProjectileMods(), isClickable = true, enabled = false, toolTip = "Go back!" },
                new ButtonInfo { buttonText = "How To Add Your Own Sounds", method = () => ShibaGTGenesis.Backend.ProjectileMods.AddOwn(), isClickable = true, enabled = false, toolTip = "adds sounds!" },
                new ButtonInfo { buttonText = "Loop Sound", method = () => ShibaGTGenesis.Backend.ProjectileMods.LoopSounds(), disableMethod =()=> ProjectileMods.DontLoopSound(), isClickable = false, enabled = false, toolTip = "stops ur sounds!" },
                new ButtonInfo { buttonText = "Load Sounds", method = () => ShibaGTGenesis.Backend.ProjectileMods.LoadSounds(), isClickable = true, enabled = false, toolTip = "loads ur sounds!" },
                new ButtonInfo { buttonText = "Play Sound With Trigger", method = () => ShibaGTGenesis.Backend.ProjectileMods.PlaySoundTrigger(), isClickable = false, enabled = false, toolTip = "play ur sounds!" },
            },

            new ButtonInfo[] // 9
            {
                new ButtonInfo { buttonText = "Go Back", method = () => ShibaGTGenesis.Backend.Back.RigMods(), isClickable = true, enabled = false, toolTip = "Go back!" },
                new ButtonInfo { buttonText = "Ghost Monkey", method = () => ShibaGTGenesis.Backend.RigMods.Ghost(), enabled = false, toolTip = "Primary or smth to use!" },
                new ButtonInfo { buttonText = "Update Rig Position [rg]", method = () => ShibaGTGenesis.Backend.RigMods.UpdateRig(), enabled = false, toolTip = "Primary or smth to use!" },
                new ButtonInfo { buttonText = "Invis Monkey", method = () => ShibaGTGenesis.Backend.RigMods.Invis(), enabled = false, toolTip = "Trigger to invis!" },
                //new ButtonInfo { buttonText = "Ghost+Invis Monke", method = () => ShibaGTGenesis.Backend.RigMods.GhostPlusInvis(), enabled = false, toolTip = "Both!" },
                new ButtonInfo { buttonText = "Hold Rig", method = () => ShibaGTGenesis.Backend.RigMods.HoldRig(), enabled = false, toolTip = "Grips!" },
                new ButtonInfo { buttonText = "Rig Gun", method = () => ShibaGTGenesis.Backend.RigMods.RigGun(), oneDisableMethod =()=> Back.EnableRig(), enabled = false, toolTip = "Gun!" },
                new ButtonInfo { buttonText = "Follow Player Gun", method = () => ShibaGTGenesis.Backend.RigMods.FollowPlayerGun(), enabled = false, toolTip = "Gun!" },
                new ButtonInfo { buttonText = "Lucy Random [rg]", method = () => ShibaGTGenesis.Backend.RigMods.LucyRandom(), enabled = false, toolTip = "Become Lucy!" },
                new ButtonInfo { buttonText = "Lucy Gun", method = () => ShibaGTGenesis.Backend.RigMods.LucyGun(), enabled = false, toolTip = "Gun!" },
                new ButtonInfo { buttonText = "Copy Gun", method = () => ShibaGTGenesis.Backend.RigMods.CopyGun(), enabled = false, toolTip = "Copy gun!" },
                new ButtonInfo { buttonText = "Sex Gun", method = () => ShibaGTGenesis.Backend.RigMods.SexGun(), enabled = false, toolTip = "Gun!" },
                //new ButtonInfo { buttonText = "High Five Gun", method = () => ShibaGTGenesis.Backend.RigMods.HighFive(), enabled = false, toolTip = "Gun!" },
                new ButtonInfo { buttonText = "Look At Gun", method = () => ShibaGTGenesis.Backend.RigMods.LookAtGun(), enabled = false, toolTip = "Closest!" },
                new ButtonInfo { buttonText = "Look At Closest", method = () => ShibaGTGenesis.Backend.RigMods.LookAtClosest(), enabled = false, toolTip = "Closest!" },
                new ButtonInfo { buttonText = "Spin Bot [lg]", method = () => ShibaGTGenesis.Backend.RigMods.SpinBot(), enabled = false, toolTip = "Spin!" },
                new ButtonInfo { buttonText = "T Pose [rg]", method = () => ShibaGTGenesis.Backend.RigMods.Tpose(), enabled = false, toolTip = "T!" },
                new ButtonInfo { buttonText = "Helicoptor [lg]", method = () => ShibaGTGenesis.Backend.RigMods.Helicopter(), enabled = false, toolTip = "vroom vroom!" },
                new ButtonInfo { buttonText = "Annoy Player Gun", method = () => ShibaGTGenesis.Backend.RigMods.AnnoyGun(), enabled = false, toolTip = "annoy!" },
                new ButtonInfo { buttonText = "Halo Around Player Gun", method = () => ShibaGTGenesis.Backend.RigMods.HaloGun(), enabled = false, toolTip = "annoy!" },
                new ButtonInfo { buttonText = "Jumpscare Gun", method = () => ShibaGTGenesis.Backend.RigMods.JumpscareGun(),  enabled = false, toolTip = "annoy!" },
                new ButtonInfo { buttonText = "Head Spin", method = () => ShibaGTGenesis.Backend.RigMods.HeadSpin(), oneDisableMethod = () => RigMods.OffHeadSpin(), enabled = false, toolTip = "Spin!" },
                new ButtonInfo { buttonText = "Head Roll", method = () => ShibaGTGenesis.Backend.RigMods.HeadRoll(), oneDisableMethod = () => RigMods.OffHeadRoll(), enabled = false, toolTip = "Roll!" },
                new ButtonInfo { buttonText = "Spaz Monkey", method = () => ShibaGTGenesis.Backend.RigMods.SpazMonk(), enabled = false, toolTip = "Spaz!" },
                new ButtonInfo { buttonText = "Head Spaz", method = () => ShibaGTGenesis.Backend.RigMods.HeadSpaz(), enabled = false, toolTip = "Spaz!" },
                new ButtonInfo { buttonText = "Head Upsidedown", method = () => ShibaGTGenesis.Backend.RigMods.HeadUpsidedown(), oneDisableMethod =() => RigMods.OFFHeadUpsidedown(), enabled = false, toolTip = "Upsidedown!" },
                new ButtonInfo { buttonText = "Head Backwards", method = () => ShibaGTGenesis.Backend.RigMods.HeadBackwards(), oneDisableMethod =() => RigMods.OFFHeadBackwards(), enabled = false, toolTip = "Backwords!" },
            },

            new ButtonInfo[] // 10
            {
                new ButtonInfo { buttonText = "Go Back", method = () => ShibaGTGenesis.Backend.Back.AdvantageMods(), isClickable = true, enabled = false, toolTip = "Go back!" },
                new ButtonInfo { buttonText = "No Tag Freeze", method = () => ShibaGTGenesis.Backend.AdvMods.NoTagFreeze(), enabled = false, toolTip = "no bom!" },
                new ButtonInfo { buttonText = "Uninfect Self", method = () => ShibaGTGenesis.Backend.AdvMods.Untagself(), isClickable = true, enabled = false, toolTip = "Untag Self!" },
                new ButtonInfo { buttonText = "Uninfect Gun", method = () => ShibaGTGenesis.Backend.AdvMods.UntagGun(), enabled = false, toolTip = "Untag Gun!" },
                new ButtonInfo { buttonText = "Uninfect All", method = () => ShibaGTGenesis.Backend.AdvMods.UntagAll(), isClickable = true, enabled = false, toolTip = "Untag All!" },
                new ButtonInfo { buttonText = "Tag Gun", method = () => ShibaGTGenesis.Backend.AdvMods.TagGun(), enabled = false, toolTip = "Tag Gun!" },
                new ButtonInfo { buttonText = "Tag All", method = () => ShibaGTGenesis.Backend.AdvMods.TagAll(), enabled = false, toolTip = "Tag All!" },
                new ButtonInfo { buttonText = "Auto Tag All", method = () => ShibaGTGenesis.Backend.AdvMods.AutoTagAll(), enabled = false, toolTip = "Tag All!" },
                new ButtonInfo { buttonText = "Tag Aura", method = () => ShibaGTGenesis.Backend.AdvMods.TagAura(), enabled = false, toolTip = "Tag Aura!" },
                new ButtonInfo { buttonText = "Grip Tag Aura [g]", method = () => ShibaGTGenesis.Backend.AdvMods.GripTagAura(), enabled = false, toolTip = "Tag Aura!" },

                new ButtonInfo { buttonText = "Always Guardian", method = () => ShibaGTGenesis.Backend.OP.AlwaysGuardian(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                new ButtonInfo { buttonText = "Anti Grab", oneMethod = () => ShibaGTGenesis.Backend.World.AntiGrab(), oneDisableMethod =()=> World.AllowGrab(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },
                new ButtonInfo { buttonText = "Small Guardian", method = () => ShibaGTGenesis.Backend.OP.SmallGuard(), enabled = false, isClickable = false, toolTip = "makes you guardian, but small!" },

                new ButtonInfo { buttonText = "Push Others Away From Meteor", method = () => ShibaGTGenesis.Backend.OP.AntiGuardian(), enabled = false, isClickable = false, toolTip = "attempts to become guardian!" },

                new ButtonInfo { buttonText = "Tag Self", method = () => ShibaGTGenesis.Backend.AdvMods.TagSelf(), enabled = false, toolTip = "Tag Self!" },
                new ButtonInfo { buttonText = "Auto Tag Self", method = () => ShibaGTGenesis.Backend.AdvMods.AutoTagSelf(), enabled = false, toolTip = "Tag Self!" },
                new ButtonInfo { buttonText = "Anti Tag", method = () => ShibaGTGenesis.Backend.AdvMods.AntiTag(), enabled = false, toolTip = "*weave*" },

                //new ButtonInfo { buttonText = "Ambush Gun", method = () => ShibaGTGenesis.Backend.AdvMods.AmbushGun(), enabled = false, toolTip = "Ambush Gun!" },
                //new ButtonInfo { buttonText = "Ambush All", method = () => ShibaGTGenesis.Backend.AdvMods.AmbushAll(), enabled = false, toolTip = "Ambush All!" },
                //new ButtonInfo { buttonText = "Ambush Aura", method = () => ShibaGTGenesis.Backend.AdvMods.AmbushAura(), enabled = false, toolTip = "Ambush Aura!" },
                //new ButtonInfo { buttonText = "Ambush Self", method = () => ShibaGTGenesis.Backend.AdvMods.AmbushSelf(), enabled = false, toolTip = "Ambush Self!" },
                //new ButtonInfo { buttonText = "Anti Ambush", method = () => ShibaGTGenesis.Backend.AdvMods.AntiAmbush(), enabled = false, toolTip = "*weave*" },

                new ButtonInfo { buttonText = "Hunt Tag Gun", method = () => ShibaGTGenesis.Backend.AdvMods.HuntTagGun(), enabled = false, toolTip = "Hunt Tag Gun!" },
                new ButtonInfo { buttonText = "Hunt Tag All", method = () => ShibaGTGenesis.Backend.AdvMods.HuntTagAll(), enabled = false, toolTip = "Hunt Tag All!" },
                new ButtonInfo { buttonText = "Hunt Tag Aura", method = () => ShibaGTGenesis.Backend.AdvMods.HuntTagAura(), enabled = false, toolTip = "Hunt Tag Aura!" },
                new ButtonInfo { buttonText = "Battle Aimbot", method = () => ShibaGTGenesis.Backend.AdvMods.Aimbot(), enabled = false, toolTip = "kill" },
                new ButtonInfo { buttonText = "Kill All", method = () => ShibaGTGenesis.Backend.AdvMods.KillAll(), enabled = false, toolTip = "kill" },
                new ButtonInfo { buttonText = "Kill Gun", method = () => ShibaGTGenesis.Backend.AdvMods.KillGun(), enabled = false, toolTip = "*weave*" },
                new ButtonInfo { buttonText = "Kill Aura", method = () => ShibaGTGenesis.Backend.AdvMods.KillAura(), enabled = false, toolTip = "*weave*" },
            },


            new ButtonInfo[] // 11
            {
                new ButtonInfo { buttonText = "Go Back", method =() => ShibaGTGenesis.Backend.Back.FavoriteMods(), isClickable = true, enabled = false, toolTip = "Go to favorites!"},
            },


            new ButtonInfo[] // 12
            {
                new ButtonInfo { buttonText = "Go Back", method =() => ShibaGTGenesis.Backend.Back.ModPresets(), isClickable = true, enabled = false, toolTip = "Go to mod presets!"},
                new ButtonInfo { buttonText = "Ghost Trolling Preset", method =() => ShibaGTGenesis.Backend.ModPresets.GhostTrolling(), isClickable = true, enabled = false, toolTip = "Go to mod presets!"},
                new ButtonInfo { buttonText = "Comp Preset", method =() => ShibaGTGenesis.Backend.ModPresets.Legit(), isClickable = true, enabled = false, toolTip = "Go to mod presets!"},
            },

            new ButtonInfo[] // 13
            {
                new ButtonInfo { buttonText = "Go Back", method =() => ShibaGTGenesis.Backend.Back.EnabledMods(), isClickable = true, enabled = false, toolTip = "Go to enabled mods!"},

            },

            new ButtonInfo[] // 14
            {
                new ButtonInfo { buttonText = "Go Back", method =() => ShibaGTGenesis.Backend.Back.RiskyMods(), isClickable = true, enabled = false, toolTip = "Go to enabled mods!"},
                //new ButtonInfo { buttonText = "Lag All 2 [lt]", method =() => OP.LagAll2(), enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "Lag Gun 2", method =() => OP.LagGun2(), isClickable = false, enabled = false, toolTip = "crashes the person u shoot"},
                //new ButtonInfo { buttonText = "Lag Gun 2 On Your Touch", method =() => OP.Lag2OnYouTouch(), isClickable = false, enabled = false, toolTip = "crashes the person u shoot"},
                //new ButtonInfo { buttonText = "Lag Gun 2 On Touch", method =() => OP.Lag2Touch(), isClickable = false, enabled = false, toolTip = "crashes the person u shoot"},
                //new ButtonInfo { buttonText = "SS Mute All [lt]", method =() => OP.MuteAllSS(), enabled = false, toolTip = "crashes everyone"},
                //new ButtonInfo { buttonText = "SS Mute Gun", method =() => OP.MuteGunSS(), enabled = false, toolTip = "crashes everyone"},
            },

            new ButtonInfo[] // 15
            {
                new ButtonInfo { buttonText = "Go Back", method =() => ShibaGTGenesis.Backend.Back.Plugins(), isClickable = true, enabled = false, toolTip = "Go to enabled mods!"},
                new ButtonInfo { buttonText = "How To Add Plugins", method = () => ShibaGTGenesis.Backend.Back.AddOwn(), isClickable = true, enabled = false, toolTip = "explains how!" },
                new ButtonInfo { buttonText = "Plugin Library", method = () => ShibaGTGenesis.Backend.Back.PluginLibrary(), isClickable = true, enabled = false, toolTip = "download ur sounds!" },
                new ButtonInfo { buttonText = "Load Plugins", method = () => ShibaGTGenesis.Backend.Back.LoadAndEnablePlugins(), isClickable = true, enabled = false, toolTip = "loads ur sounds!" },



            },

            new ButtonInfo[] // 16
            {
                new ButtonInfo { buttonText = "Go Back", method =() => ShibaGTGenesis.Backend.Back.MasterMods(), isClickable = true, enabled = false, toolTip = "Go to enabled mods!"},
               // new ButtonInfo { buttonText = "debug custom", method =() => Master.DESPAWN(), isClickable = false, enabled = false, toolTip = "Go to enabled mods!"},
                new ButtonInfo { buttonText = "Gamemode Spam [ban]", method =() => Master.GMSpam(), isClickable = false, enabled = false, toolTip = "Go to enabled mods!"},
                new ButtonInfo { buttonText = "Invis All [ban]", method =() => Master.InvisAll(), isClickable = false, enabled = false, toolTip = "Go to enabled mods!"},
                new ButtonInfo { buttonText = "Skeleton All [u cant see] [ban]", method =() => Master.SkeletonAll(), isClickable = false, enabled = false, toolTip = "Go to enabled mods!"},
                new ButtonInfo { buttonText = "Sound Spam [g to change t to play]", method =() => Master.SoundSpammer(), isClickable = false, enabled = false, toolTip = "Go to enabled mods!"},
                new ButtonInfo { buttonText = "Mat Spam All", method =() => ShibaGTGenesis.Backend.Master.MatSpamAll(), isClickable = false, enabled = false, toolTip = "Go to enabled mods!"},
                new ButtonInfo { buttonText = "Mat Spam Gun", method =() => ShibaGTGenesis.Backend.Master.MatSpamGun(), isClickable = false, enabled = false, toolTip = "Go to enabled mods!"},
                new ButtonInfo { buttonText = "Untag Aura [M]", method =() => ShibaGTGenesis.Backend.Master.UntagAura(), isClickable = false, enabled = false, toolTip = "Untags everyone in proximity!"},
                new ButtonInfo { buttonText = "Activate Grey Zone [M]", method =() => ShibaGTGenesis.Backend.Master.ActivateGreyZone(), isClickable = true, enabled = false, toolTip = "Activates the grey zone."},
                new ButtonInfo { buttonText = "Deactivate Grey Zone [M]", method =() => ShibaGTGenesis.Backend.Master.DeactivateGreyZone(), isClickable = true, enabled = false, toolTip = "Deactivates the grey zone."},
                new ButtonInfo { buttonText = "Flash Screen All [M]", method =() => ShibaGTGenesis.Backend.Master.FlashScreenAll(), isClickable = false, enabled = false, toolTip = "Strobes the grey zone for everyone."},
                new ButtonInfo { buttonText = "Lava Spaz [M]", method =() => ShibaGTGenesis.Backend.Master.SpazLava(), isClickable = false, enabled = false, toolTip = "Toggles forest lava state."},
                new ButtonInfo { buttonText = "Lava Swim Gun [M]", method =() => ShibaGTGenesis.Backend.Master.LavaSwimGun(), isClickable = false, enabled = false, toolTip = "Forces target to swim in lava."},
                new ButtonInfo { buttonText = "Lava Swim All [M]", method =() => ShibaGTGenesis.Backend.Master.LavaSwimAll(), isClickable = true, enabled = false, toolTip = "Forces everyone to swim in lava."},
            },

            new ButtonInfo[] // 17
            {
                new ButtonInfo { buttonText = "Go Back", method =() => ShibaGTGenesis.Backend.Back.ProjectileModsFR(), isClickable = true, enabled = false, toolTip = "Go to enabled mods!"},
                new ButtonInfo { buttonText = "Projectile Type : Deadshot", method = () => ProjectileModsFR.ChangeProjectile(false), isClickable = true, enabled = false, toolTip = "j" },
                new ButtonInfo { buttonText = "Projectile Color : Default", method = () => ProjectileModsFR.ChangeColor(false), isClickable = true, enabled = false, toolTip = "j" },
                new ButtonInfo { buttonText = "Save Projectile Preferences", method =() => ShibaGTGenesis.Backend.Back.Save(), isClickable = true, enabled = false, toolTip = "Save your settings!"},
                new ButtonInfo { buttonText = "Projectile Spam [rg]", method = () => ProjectileModsFR.SlingshotCall(ProjectileModsFR.projectileToCall, GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position, GorillaLocomotion.GTPlayer.Instance.rightHand.velocityTracker.GetAverageVelocity(), ProjectileModsFR.colorToCall), isClickable = false, enabled = false, toolTip = "j" },
                new ButtonInfo { buttonText = "Projectile Red Laser [rg]", method = () => ProjectileModsFR.SlingshotCall(214, GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position, GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.forward *  15000, ProjectileModsFR.colorToCall), isClickable = false, enabled = false, toolTip = "j" },
                new ButtonInfo { buttonText = "Projectile Blue Laser [rg]", method = () => ProjectileModsFR.SlingshotCall(217, GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position, GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.forward *  15000, ProjectileModsFR.colorToCall), isClickable = false, enabled = false, toolTip = "j" },
            },
        };
    }
}
