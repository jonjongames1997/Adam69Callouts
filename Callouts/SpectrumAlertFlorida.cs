using CalloutInterfaceAPI;
using Adam69Callouts.Common;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace Adam69Callouts.Callouts
{
    [CalloutInterface("[Adam69 Callouts] Spectrum Alert (Florida)", CalloutProbability.Medium, "Statewide Spectrum Alert: Missing person possibly abducted. Vehicle last seen in the area.", "Code 2", "FDLE")]
    public class SpectrumAlertFlorida : Callout
    {
        private Ped missingPerson;
        private Ped suspect;
        private Vehicle suspectVehicle;
        private Ped reportingOfficer;
        private Blip missingBlip;
        private Blip suspectBlip;
        private Blip officerBlip;
        private Vector3 spawnPoint;
        private Vector3 lastSeenPosition;
        private int dialogStage;
        private bool suspectArmed;
        private bool pursuitStarted;
        private static readonly string[] suspectCars = new string[] { "jackal", "emperor", "fusilade", "cogcabrio", "intruder" };
        private static readonly string[] suspectModels = new string[] { "s_m_m_security_01", "g_m_y_mexgoon_02", "s_f_y_cop_01" };

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around2D(400f, 900f));
            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 120f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Spectrum Alert: Missing person reported; possible abduction vehicle last seen nearby.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("SUSPICIOUS_VEHICLE_LOITERING_02", spawnPoint);
            CalloutMessage = "Spectrum Alert: Missing Person";
            CalloutPosition = spawnPoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            if (Settings.EnableLogs)
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Spectrum Alert (Florida) callout accepted.");
                LoggingManager.Log("Adam69 Callouts: Spectrum Alert (Florida) callout accepted.");
            }

            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "Spectrum Alert (Florida)", "~b~Dispatch~w~: Missing person reported. Possible abductor vehicle. Respond ~r~Code 2~w~.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_2_Audio");

            var rng = new Random();
            suspectArmed = rng.Next(0, 10) > 5;
            lastSeenPosition = spawnPoint.Around2D(15f, 60f);

            // Spawn reporting officer
            reportingOfficer = new Ped("s_m_y_hwaycop_01", spawnPoint.Around2D(5f, 12f), 0f);
            if (reportingOfficer.Exists())
            {
                reportingOfficer.IsPersistent = true;
                reportingOfficer.BlockPermanentEvents = true;
                reportingOfficer.Tasks.PlayAnimation(new AnimationDictionary("misscommon@response"), "idle_c", -1f, AnimationFlags.Loop);
                officerBlip = reportingOfficer.AttachBlip();
                officerBlip.Color = System.Drawing.Color.Green;
                officerBlip.Scale = 0.8f;
            }

            // Spawn missing person — always create the Ped first
            missingPerson = new Ped("a_m_m_prolhost_01", lastSeenPosition, 0f);
            if (missingPerson.Exists())
            {
                missingPerson.IsPersistent = true;
                missingPerson.BlockPermanentEvents = true;

                if (rng.Next(0, 10) > 3) // ~60% on foot
                {
                    missingPerson.Tasks.PlayAnimation(new AnimationDictionary("random@homelandsecurity"), "idle_a", -1f, AnimationFlags.Loop);
                }
                else // in vehicle — spawn a separate vehicle for the missing person
                {
                    var mpVehicleModel = suspectCars[rng.Next(suspectCars.Length)];
                    var mpVehicle = new Vehicle(mpVehicleModel, lastSeenPosition);
                    if (mpVehicle.Exists())
                    {
                        mpVehicle.IsPersistent = true;
                        mpVehicle.IsEngineOn = false;
                        if (mpVehicle.IsSeatFree((int)VehicleSeat.Passenger))
                        {
                            missingPerson.WarpIntoVehicle(mpVehicle, (int)VehicleSeat.Passenger);
                        }
                    }
                }

                missingBlip = missingPerson.AttachBlip();
                missingBlip.Color = System.Drawing.Color.Yellow;
                missingBlip.IsRouteEnabled = false;
            }

            // Spawn suspect vehicle and driver
            var suspectModel = suspectCars[rng.Next(suspectCars.Length)];
            suspectVehicle = new Vehicle(suspectModel, spawnPoint.Around2D(20f, 60f));
            if (suspectVehicle.Exists())
            {
                suspectVehicle.IsPersistent = true;
                suspectVehicle.IsEngineOn = false;

                suspect = suspectVehicle.CreateRandomDriver();
                if (suspect.Exists())
                {
                    suspect.IsPersistent = true;
                    suspect.BlockPermanentEvents = true;
                    if (suspectArmed)
                    {
                        SafeInventory.SafeGiveWeapon(suspect, "weapon_pistol", 60, true);
                    }

                    suspectBlip = suspect.AttachBlip();
                    suspectBlip.Color = System.Drawing.Color.Red;
                    suspectBlip.IsRouteEnabled = true;
                }
            }

            dialogStage = 0;
            pursuitStarted = false;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (missingPerson != null && missingPerson.Exists()) missingPerson.Delete();
            if (suspect != null && suspect.Exists()) suspect.Delete();
            if (suspectVehicle != null && suspectVehicle.Exists()) suspectVehicle.Delete();
            if (reportingOfficer != null && reportingOfficer.Exists()) reportingOfficer.Delete();
            if (missingBlip != null && missingBlip.Exists()) missingBlip.Delete();
            if (suspectBlip != null && suspectBlip.Exists()) suspectBlip.Delete();
            if (officerBlip != null && officerBlip.Exists()) officerBlip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            base.Process();

            // Interact with reporting officer
            if (reportingOfficer != null && reportingOfficer.Exists() && MainPlayer.DistanceTo(reportingOfficer) <= 12f && dialogStage == 0)
            {
                if (Settings.HelpMessages)
                {
                    Game.DisplayHelp("Press ~y~" + Settings.Dialog.ToString() + "~w~ to speak with the reporting officer and get details.");
                }

                if (Game.IsKeyDown(Settings.Dialog))
                {
                    GameFiber.Sleep(400);
                    NativeFunction.Natives.TASK_TURN_PED_TO_FACE_ENTITY(reportingOfficer, MainPlayer, -1);
                    Game.DisplaySubtitle("~g~Officer~w~: Officer, the missing person was last seen getting into a vehicle. License plate partial: 'FL' and a blue sedan.");
                    GameFiber.Sleep(4000);
                    Game.DisplaySubtitle("~b~You~w~: Copy. Check cameras and keep the area secure. I'll look for the vehicle.");
                    dialogStage++;
                }
            }

            // Interact with suspect
            if (suspect != null && suspect.Exists() && MainPlayer.DistanceTo(suspect) <= 18f && dialogStage >= 1)
            {
                if (Settings.HelpMessages)
                {
                    Game.DisplayHelp("Press ~y~" + Settings.Dialog.ToString() + "~w~ to attempt to stop and question the suspect.");
                }

                if (Game.IsKeyDown(Settings.Dialog))
                {
                    GameFiber.Sleep(400);
                    dialogStage++;

                    if (dialogStage == 2)
                    {
                        NativeFunction.Natives.TASK_TURN_PED_TO_FACE_ENTITY(suspect, MainPlayer, -1);
                        Game.DisplaySubtitle("~b~You~w~: Citizen, pull over, step out of the vehicle. You're being investigated in a Spectrum Alert.");
                        GameFiber.Sleep(3000);

                        if (suspectArmed)
                        {
                            Game.DisplaySubtitle("~r~Suspect~w~: I ain't giving myself up! *reaches for a weapon*");
                            GameFiber.Sleep(1500);
                            suspect.Tasks.FightAgainst(MainPlayer);
                            suspect.Armor = 200;
                        }
                        else
                        {
                            Game.DisplaySubtitle("~r~Suspect~w~: I didn't do anything. I was just giving them a ride.");
                            GameFiber.Sleep(1500);
                            suspect.Tasks.PutHandsUp(-1, MainPlayer);
                        }
                    }
                }
            }

            // Start pursuit only once when suspect flees
            if (!pursuitStarted && suspect != null && suspect.Exists() &&
                suspectVehicle != null && suspectVehicle.Exists() &&
                suspect.IsInVehicle(suspectVehicle, false) &&
                suspectVehicle.IsDriveable && !suspect.IsInCombat &&
                suspectVehicle.Speed > 5f && MainPlayer.DistanceTo(suspectVehicle) > 20f)
            {
                pursuitStarted = true;
                var pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, suspect);
                LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);
                Game.DisplayNotification("~r~Suspect fleeing! Engage and attempt to stop the vehicle!");
                if (suspectBlip != null && suspectBlip.Exists()) suspectBlip.Color = System.Drawing.Color.Red;
            }

            // Request backup
            if (Game.IsKeyDown(Settings.RequestVehicleInfo))
            {
                GameFiber.Sleep(200);
                PolicingRedefined.API.InfoDispatchAPI.RunNearestVehicleThroughDispatch(true);
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Backup_Audio");
            }

            // Player death
            if (MainPlayer.IsDead)
            {
                if (Settings.MissionMessages)
                {
                    BigMessageThread bigMessage = new BigMessageThread();
                    bigMessage.MessageInstance.ShowColoredShard("MISSION FAILED!", "You have fallen in the line of duty.", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);
                }

                if (Settings.EnableLogs)
                {
                    LoggingManager.Log("Adam69 Callouts: Spectrum Alert (Florida) callout failed - Player died.");
                }

                End();
            }

            if (Game.IsKeyDown(Settings.EndCall))
            {
                End();
            }
        }

        public override void End()
        {
            if (missingPerson != null && missingPerson.Exists()) missingPerson.Dismiss();
            if (suspect != null && suspect.Exists()) suspect.Dismiss();
            if (suspectVehicle != null && suspectVehicle.Exists()) suspectVehicle.Dismiss();
            if (reportingOfficer != null && reportingOfficer.Exists()) reportingOfficer.Dismiss();
            if (missingBlip != null && missingBlip.Exists()) missingBlip.Delete();
            if (suspectBlip != null && suspectBlip.Exists()) suspectBlip.Delete();
            if (officerBlip != null && officerBlip.Exists()) officerBlip.Delete();

            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "Spectrum Alert (Florida)", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

            if (Settings.MissionMessages)
            {
                BigMessageThread bigMessage = new BigMessageThread();
                bigMessage.MessageInstance.ShowColoredShard("Callout Completed!", "Spectrum Alert handled. You are now ~g~CODE 4~w~.", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);
            }

            base.End();

            if (Settings.EnableLogs)
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Spectrum Alert (Florida) callout ended CODE 4.");
                LoggingManager.Log("Adam69 Callouts: Spectrum Alert (Florida) callout ended CODE 4.");
            }
        }
    }
}