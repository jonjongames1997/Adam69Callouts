﻿using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{
    [CalloutInterface("[Adam69 Callouts] Bicycle Pursuit", CalloutProbability.Medium, "Bicyclist evading arrest", "CODE 3", "LSPD")]
    public class BicyclePursuit : Callout
    {
        private static readonly string[] bikeList = { "bmx", "cruiser", "fixter", "scorcher", "tribike", "tribike2", "tribike3" };
        private static Vehicle bicycle;
        private static Blip blip;
        private static Ped suspect;
        private static Vector3 spawnpoint;
        private static LHandle pursuit;
        private static bool pursuitCreated = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(1000f));
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "A civilian is evading arrest");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("Adam69Callouts_Bicycle_Pursuit", spawnpoint);
            CalloutMessage = "Bicycle Pursuit Reported";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            if (Settings.EnableLogs)
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Bicycle Pursuit callout has been accepted!");
            }
            else
            {
                Settings.EnableLogs = false;
            }

            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Bicycle Pursuit", "~b~Dispatch~w~: The suspect has been spotted! Respond ~r~Code 3~w~.");

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_3_Audio");

            bicycle = new Vehicle(bikeList[new Random().Next((int)bikeList.Length)], spawnpoint);
            bicycle.IsPersistent = true;
            bicycle.IsStolen = true;
            bicycle.IsValid();

            blip = bicycle.AttachBlip();
            blip.Color = System.Drawing.Color.Yellow;
            blip.Alpha = 1.0f;

            suspect = new Ped(spawnpoint);
            suspect.WarpIntoVehicle(bicycle, -1);
            suspect.Inventory.GiveNewWeapon("WEAPON_COMBATPISTOL", 500, true);
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;
            suspect.IsValid();

            blip = suspect.AttachBlip();
            blip.Color = System.Drawing.Color.Red;
            blip.IsRouteEnabled = true;

            pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, suspect);
            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);
            pursuitCreated = true;
            PolicingRedefined.API.BackupDispatchAPI.RequestPursuitBackup();

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (bicycle) bicycle.Delete();
            if (suspect) suspect.Delete();
            if (blip) blip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if (Settings.HelpMessages)
            {
                Game.DisplayHelp("Chase the bicycle and arrest the suspect.");
            }
            else
            {
                if (Settings.EnableLogs)
                {
                    Game.LogTrivial("Adam69 Callouts [LOG]: Help messages are disabled in the config file.");
                }
                else
                {
                    Settings.EnableLogs = false;
                }

                Settings.HelpMessages = false;
            }

            if (MainPlayer.IsDead)
            {
                if (Settings.MissionMessages)
                {
                    BigMessageThread bigMessage = new BigMessageThread();

                    bigMessage.MessageInstance.ShowColoredShard("Callout Failed!", "~r~You have failed the callout.", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);
                }
                else
                {
                    Settings.MissionMessages = false;
                }

                End();
            }

            if (Game.IsKeyDown(Settings.EndCall))
            {
                if (Settings.MissionMessages)
                {
                    BigMessageThread bigMessage = new BigMessageThread();
                    bigMessage.MessageInstance.ShowColoredShard("Callout Complete!", "Return back to patrol", RAGENativeUI.HudColor.Yellow, RAGENativeUI.HudColor.Black, 5000);
                }
                else
                {
                    Settings.MissionMessages = false;
                }

                End();
            }

            base.Process();
        }

        public override void End()
        {
            if (suspect) suspect.Dismiss();
            if (bicycle) bicycle.Delete();
            if (blip) blip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Bicycle Pursuit", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

            if (Settings.MissionMessages)
            {

                BigMessageThread bigMessage = new BigMessageThread();

                bigMessage.MessageInstance.ShowColoredShard("Callout Completed!", "You are now ~g~CODE 4~w~.", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);
            }
            else
            {
                Settings.MissionMessages = false;
            }

            base.End();

            if (Settings.EnableLogs)
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Bicycle Pursuit callout is Code 4!");
                LoggingManager.Log("Adam69 Callouts [LOG]: Bicycle Pursuit callout has ended.");
            }
            else
            {
                Settings.EnableLogs = false;
            }
        }
    }
}