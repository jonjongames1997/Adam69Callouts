using Adam69Callouts.Utilities;
using CalloutInterfaceAPI;
using LSPD_First_Response.Engine;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] Drugs Found [Beta]", CalloutProbability.Medium, "Drugs found reported by a citizen", "Code 2", "LEO")]

    public class DrugsFound : Callout
    {
        private const bool V = false;
        private static readonly string[] drugList = new string[] { "sf_prop_sf_bag_weed_01b", "bkr_prop_weed_bigbag_open_01a", "m24_1_prop_m41_weed_bigbag_01a", "sf_prop_sf_bag_weed_open_01a", "m25_1_prop_m51_box_weed_01a", "m25_1_prop_m51_box_weed_02a", "m25_1_prop_m51_bag_weed_01a" };
        private static Vector3 spawnpoint;
        public Rage.Object theDrugs;
        private static Ped theCaller;
        private static Vector3 callerSpawn;
        private static Blip drugBlip;
        private static Blip callerBlip;
        private static int counter;
        private static string malefemale;
        private static readonly string[] backupList = new string[] { "s_m_y_cop_01", "s_f_y_cop_01", "csb_cop", "s_f_y_sheriff_01", "s_m_y_sheriff_01" };
        private static readonly string[] backupVehicle = new string[] { "police", "police2", "police3", "police4", "fbi", "fbi2", "sheriff", "sheriff2", "policeb", "policeb2" };
        private static Ped theCop;
        private static Vector3 copSpawn;
        private static Vector3 leoVehicleSpawn;
        private static Blip theCopBlip;
        private static Blip policeCarBlip;
        private static Vehicle policeVehicle;
        private static string copGender;
        private static bool isCollected;
        private static float callerHeading;
        private static float copHeading;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = new(979.11f, -1957.23f, 30.77f);
            callerSpawn = new(989.72f, -1945.02f, 30.99f);
            callerHeading = 199.09f;
            copSpawn = new(1000.36f, -1952.72f, 30.91f);
            copHeading = 74.75f;
            leoVehicleSpawn = new(1000.69f, -1958.26f, 30.86f);
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of illegal drugs found by a nearby citizen.");
            if (Settings.BluelineDispatchIntegration == true)
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("A_CIVILIAN_REQUIRING_ASSISTANCE_01", spawnpoint);
            }
            else
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Drugs_Found_Audio");
            }
            CalloutMessage = "Illegal drugs found";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Drugs Found callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~y~Drugs Found", "~b~Dispatch~w~: The caller has been located. Respond ~r~Code 2~w~.");
            LoggingManager.Log("Adam69 Callouts [LOG]: Drugs Found callout has been accepted!");

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_2_Audio");

            theCaller = new Ped(callerSpawn);
            theCaller.IsPersistent = true;
            theCaller.BlockPermanentEvents = true;
            theCaller.IsValid();

            theCaller.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);


            theDrugs = new Rage.Object("m25_1_prop_m51_bag_weed_01a", spawnpoint)
            {
                IsPersistent = true
            };
            theDrugs.IsValid();

            callerBlip = theCaller.AttachBlip();
            callerBlip.Color = System.Drawing.Color.Orange;
            callerBlip.Alpha = 0.5f;

            drugBlip = theDrugs.AttachBlip();
            drugBlip.Color = System.Drawing.Color.Purple;
            drugBlip.IsRouteEnabled = true;

            theCop = new Ped(backupList[new Random().Next((int)backupList.Length)], copSpawn, 0f);
            theCop.IsPersistent = true;
            theCop.BlockPermanentEvents = true;
            theCop.IsValid();

            theCopBlip = theCop.AttachBlip();
            theCopBlip.Color = System.Drawing.Color.LightBlue;

            policeVehicle = new Vehicle(backupVehicle[new Random().Next((int)backupVehicle.Length)], leoVehicleSpawn, 0f);
            policeVehicle.IsPersistent = true;

            // Ensure the cop vehicle exists and is valid
            try
            {
                if (policeVehicle != null && policeVehicle.IsValid())
                {
                    // Turn on emergency lights
                    policeVehicle.IsSirenOn = true; // Activates the siren and emergency lights
                    policeVehicle.IsSirenSilent = true; // Keeps the siren silent while lights are active (optional)
                    LoggingManager.Log("Adam69 Callouts [LOG]: " + LogLevel.Info);
                }
                else
                {
                    Game.LogTrivial("ERR: copVehicle is null or invalid. Cannot enable emergency lights.");
                    LoggingManager.Log("Adam69 Callouts [LOG]: copVehicle is null or invalid. Cannot enable emergency lights.");
                }
            }
            catch (Exception ex)
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Exception while setting up police vehicle emergency lights: " + ex.Message);
                LoggingManager.Log("Adam69 Callouts [LOG]: " + ex.StackTrace);
                LoggingManager.Log("Adam69 Callouts [LOG]: " + ex.Message);
            }

            policeCarBlip = policeVehicle.AttachBlip();
            policeCarBlip.Color = System.Drawing.Color.DarkBlue;

            if (theCaller.IsMale)
                malefemale = "Sir";
            else
                malefemale = "Ma'am";

            if (MainPlayer.IsMale)
                copGender = "Sir";
            else
                copGender = "Ma'am";

            counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (theDrugs) theDrugs.Delete();
            if (theCaller) theCaller.Delete();
            if (drugBlip) drugBlip.Delete();
            if (callerBlip) callerBlip.Delete();
            if (theCop) theCop.Delete();
            if (theCopBlip) theCopBlip.Delete();
            if (policeVehicle) policeVehicle.Delete();
            if (policeCarBlip) policeCarBlip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if (MainPlayer.DistanceTo(theCaller) <= 5f)
            {
                if (Game.IsKeyDown(Settings.Dialog))
                {
                    counter++;

                    try
                    {
                        if (counter == 1)
                        {
                            theCop.Tasks.PlayAnimation(new AnimationDictionary("amb@world_human_cop_idles@male@idle_b"), "idle_e", -1f, AnimationFlags.Loop);
                            Game.DisplaySubtitle("~b~You~w~: Hello there, " + malefemale + ". Are you the caller?");
                        }
                        if (counter == 2)
                        {
                            theCaller.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                            Game.DisplaySubtitle("~o~The Caller~w~: Yes I am, " + copGender + ".");
                        }
                        if (counter == 3)
                        {
                            theCaller.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                            Game.DisplaySubtitle("~b~You~w~: Can you explain how did you find the drugs?");
                        }
                        if (counter == 4)
                        {
                            theCaller.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                            Game.DisplaySubtitle("~o~The Caller~w~: I was going for a walk then I spotted this opened bag of weed from LD Organics. I didn't know who owns that bag of weed. That's why I called.");
                        }
                        if (counter == 5)
                        {
                            theCaller.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                            Game.DisplaySubtitle("~b~You~w~: We really appreciate that you reported it. I'll investiagte this. I just need to see your ID so I know who I'm talking to.");
                        }
                        if (counter == 6)
                        {
                            theCaller.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                            Game.DisplaySubtitle("~o~The Caller~w~: Ok, no problem. I'm really in a hurry. I got to go watch Kansas City Chiefs vs Buffalo Bills game. Buffalo Bills is my team and I hope they give KC a good 'ol fashion spanking. Them cheating ass fuckers.");
                        }
                        if (counter == 7)
                        {
                            Game.DisplaySubtitle("Convo Ended. Deal with the situation you may see fit.");
                            theCaller.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        }
                    }
                    catch (Exception ex)
                    {
                        Game.LogTrivial("Adam69 Callouts [LOG]: Error in Drugs Found callout. Error: " + ex.Message);
                        LoggingManager.Log("Adam69 Callouts [LOG]: ERROR:" + ex.StackTrace);
                        LoggingManager.Log("Adam69 Callouts [LOG]: ERROR:" + ex.Message);
                        LoggingManager.Log("Adam69 Callouts [LOG]: " + LogLevel.Error);
                        LoggingManager.Log("Adam69 Callouts [LOG]: " + LogLevel.Warning);
                    }
                }
            }

            if (MainPlayer.DistanceTo(theDrugs) <= 10f)
            {
                bool helpMessages = Settings.HelpMessages;
                if (helpMessages)
                {
                    Game.DisplayHelp("Press ~y~" + Settings.PickUp.ToString() + "~w~ to pick up the drugs.", 5000);
                }
                else
                {
                    Game.LogTrivial("Adam69 Callouts [LOG]: Help messages are disabled in the config file.");
                    return;
                }

                if (Game.IsKeyDown(Settings.PickUp))
                {
                    isCollected = true;
                    MainPlayer.Tasks.PlayAnimation(new AnimationDictionary("anim@move_m@trash"), "pickup", -1f, AnimationFlags.UpperBodyOnly);
                    theDrugs.AttachTo(MainPlayer, 6286, Vector3.RelativeRight, Rotator.Zero);
                    GameFiber.Yield();
                    theDrugs.Delete();
                }
            }

            if (MainPlayer.IsDead || Game.IsKeyDown(Settings.EndCall))
            {
                bool missionMessages = Settings.MissionMessages;
                if (missionMessages == true)
                {
                    BigMessageThread bigMessage = new BigMessageThread();
                    bigMessage.MessageInstance.ShowColoredShard("Callout Failed!", "You are now ~r~CODE 4~w~.", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);
                }
                else
                {
                    Game.LogTrivial("Adam69 Callouts [LOG]: Mission messages are disabled in the config file.");
                    return;
                }

                End();
            }

            base.Process();
        }

        public override void End()
        {
            if (theDrugs) theDrugs.Delete();
            if (theCaller) theCaller.Dismiss();
            if (drugBlip) drugBlip.Delete();
            if (callerBlip) callerBlip.Delete();
            if (theCop) theCop.Dismiss();
            if (theCopBlip) theCopBlip.Delete();
            if (policeVehicle) policeVehicle.Delete();
            if (policeCarBlip) policeCarBlip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~y~Drugs Found", "~b~You~w~: Dispatch, we are ~g~Code 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

            bool missionMessages = Settings.MissionMessages;
            if (missionMessages == true)
            {
                BigMessageThread bigMessage = new BigMessageThread();

                bigMessage.MessageInstance.ShowColoredShard("Callout Complete!", "Return to patrol!", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);
            }
            else
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Mission messages are disabled in the config file.");
                return;
            }

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Drugs Found callout is code 4!");
            LoggingManager.Log("Adam69 Callouts [LOG]: Drugs Found callout is code 4!");
        }
    }
}