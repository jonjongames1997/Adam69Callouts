using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{


    [CalloutInterface("[Adam69 Callouts] Officer Down", CalloutProbability.Medium, "Reports of an officer down", "Code 3", "LEO")]

    public class OfficerDown : Callout
    {

        private static readonly string[] pedsList = new string[] { "s_f_y_cop_01", "s_m_y_cop_01", "csb_cop", "s_f_y_sheriff_01", "s_m_y_sheriff_01", "s_m_y_hwaycop_01", "s_m_m_security_01", "s_f_y_ranger_01", "s_m_y_ranger_01" };
        private static Ped suspect;
        private static Ped officer;
        private static Blip copBlip;
        private static Vehicle emergencyVehicle;
        private static readonly string[] officerVehicle = new string[] { "police", "police2", "police3", "police4", "police5", "polgauntlet", "poldominator10", "poldorado", "polgreenwood", "polimpaler5", "polimpaler6", "polcaracara", "polcoquette4", "polfaction2", "polterminus", "dilettante2", "fbi", "pbus", "policeb", "pranger", "riot", "riot2", "sheriff", "sheriff2" };
        private static Blip officerVehicleBlip;
        private static Vector3 spawnpoint;
        private static Vector3 vehicleSpawn;
        private static Vector3 susSpawn;
        private static float vehicleHeading;
        private static float officerheading;
        private static float susHeading;
        private static Blip suspectBlip;
        private static int counter;
        private static string malefemale;
        private static readonly int armorCount = 1500; // Set the armor value for the officer and suspect

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = new(132.69f, -1308.34f, 29.03f);
            officerheading = 318.26f;
            susSpawn = new(116.04f, -1291.59f, 28.26f);
            susHeading = 246.21f;
            vehicleSpawn = new(140.00f, -1308.37f, 29.00f);
            vehicleHeading = 46.70f;
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            if (Settings.DisableBluelineDispatch == true)
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_OfficerDown_Audio");
            }
            else
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CRIME_OFFICER_DOWN_02", spawnpoint);
            }
            CalloutInterfaceAPI.Functions.SendMessage(this, "Officer Down Reported by an unkown civilian");
            CalloutMessage = "Officer Down Reported";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Officer Down callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Officer Down", "~b~Dispatch~w~: The suspect has been spotted! Respond ~r~Code 3~w~.");


            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_3_Audio");

            officer = new Ped(pedsList[new Random().Next((int)pedsList.Length)], spawnpoint, 0f);
            officer.IsPersistent = true;
            officer.BlockPermanentEvents = true;
            officer.Kill();
            officer.IsValid();

            NativeFunction.Natives.APPLY_PED_DAMAGE_PACK(officer, "TD_PISTOL_FRONT", 1f, 1f);

            emergencyVehicle = new Vehicle(officerVehicle[new Random().Next((int)officerVehicle.Length)], vehicleSpawn, 0f);
            emergencyVehicle.IsPersistent = true;
            emergencyVehicle.IsValid();

            suspect = new Ped(susSpawn);
            suspect.IsPersistent = true;
            suspect.IsValid();
            suspect.BlockPermanentEvents = true;

            copBlip = officer.AttachBlip();
            copBlip.Color = System.Drawing.Color.Blue;
            copBlip.IsRouteEnabled = true;

            suspectBlip = suspect.AttachBlip();
            suspectBlip.Color = System.Drawing.Color.Red;

            try
            {
                // Ensure the cop vehicle exists and is valid
                if (emergencyVehicle != null && emergencyVehicle.IsValid())
                {
                    // Turn on emergency lights
                    emergencyVehicle.IsSirenOn = true; // Activates the siren and emergency lights
                    emergencyVehicle.IsSirenSilent = true; // Keeps the siren silent while lights are active (optional)
                    emergencyVehicle.LockStatus = VehicleLockStatus.Locked; // Locks the vehicle
                }
                else
                {
                    Game.LogTrivial("Emergency Vehicle is null or invalid. Cannot enable emergency lights.");
                    LoggingManager.Log("Adam69 Callouts [LOG]: " + LogLevel.Error);
                    LoggingManager.Log("Adam69 Callouts [LOG]: " + LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Error enabling emergency lights: " + ex.Message);
                LoggingManager.Log("Adam69 Callouts [LOG]: Error enabling emergency lights: " + ex.Message);
                LoggingManager.Log("Adam69 Callouts [LOG]: Error enabling emergency lights: " + ex.StackTrace);
                LoggingManager.Log("Adam69 Callouts [LOG]: Please report this issue on the Adam69 Callouts Discord server: https://discord.gg/N9KgZx4KUn");
            }

            officerVehicleBlip = emergencyVehicle.AttachBlip();
            officerVehicleBlip.Color = System.Drawing.Color.LightBlue;

            if (suspect.IsMale)
                malefemale = "Sir";
            else
                malefemale = "Ma'am";

            counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (suspect) suspect.Delete();
            if (suspectBlip) suspectBlip.Delete();
            if (copBlip) copBlip.Delete();
            if (officer) officer.Delete();
            if (officerVehicleBlip) officerVehicleBlip.Delete();
            if (copBlip) copBlip.Delete();
            if (emergencyVehicle) emergencyVehicle.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if (MainPlayer.DistanceTo(officer) <= 10f)
            {
                Game.DisplayHelp("Press ~y~" + Settings.Dialog.ToString() + "~w~ to Call in a officer down to ~b~dispatch~w~.", 5000);

                if (Game.IsKeyDown(Settings.Dialog))
                {
                    counter++;

                    try
                    {
                        if (counter == 1)
                        {
                            Game.DisplaySubtitle("~b~You~w~: Dispatch, we got an officer down, requesting medic but have them stage a few blocks away from the scene until the scene is secured.");
                        }
                        if (counter == 2)
                        {
                            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_OfficerDown_Audio_2");
                            UltimateBackup.API.Functions.callAmbulance();
                            UltimateBackup.API.Functions.callCode3Backup();

                        }
                        if (counter == 3)
                        {
                            suspect.Tasks.FightAgainst(MainPlayer);
                            suspect.Inventory.GiveNewWeapon("WEAPON_COMBATPISTOL", 500, true);
                            suspect.Armor = armorCount;
                            MainPlayer.Armor = armorCount;
                        }
                        if (counter == 4)
                        {
                            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_ShotsFired_Audio_Remastered_01");
                            UltimateBackup.API.Functions.callPanicButtonBackup(true);
                        }

                        LoggingManager.Log("Adam69 Callouts [LOG]: " + LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        Game.LogTrivial("Adam69 Callouts [LOG]: " + ex.Message);
                        LoggingManager.Log("Adam69 Callouts [LOG]: " + ex.Message);
                        LoggingManager.Log("Adam69 Callouts [LOG]: " + ex.StackTrace);
                    }
                }

                if (MainPlayer.IsDead || Game.IsKeyDown(Settings.EndCall))
                {
                    bool missionMessages = Settings.MissionMessages;
                    if (missionMessages == true)
                    {
                        BigMessageThread bigMessage = new BigMessageThread();
                        bigMessage.MessageInstance.ShowColoredShard("MISSION FAILED!", "You'll get 'em next time!", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);
                    }
                    else
                    {
                        return;
                    }
                    
                    End();
                }
            }

            base.Process();
        }

        public override void End()
        {
            if (officer) officer.Dismiss();
            if (copBlip) copBlip.Delete();
            if (suspect) suspect.Dismiss();
            if (suspectBlip) suspectBlip.Delete();
            if (emergencyVehicle) emergencyVehicle.Delete();
            if (officerVehicleBlip) officerVehicleBlip.Delete();
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Officer Down", "~b~You~w~: We are Code 4. Show me back 10-8!");
            base.End();

            bool missionMessages = Settings.MissionMessages;
            if (missionMessages == true)
            {
                BigMessageThread bigMessage = new BigMessageThread();

                bigMessage.MessageInstance.ShowColoredShard("CODE 4", "The scene is now secure.", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);
            }
            else
            {
                return;
            }

            Game.LogTrivial("Adam69 Callouts [LOG]: Officer Down callout is code 4!");

        }

    }
}