﻿using CalloutInterfaceAPI;
using System.Net.Configuration;

namespace Adam69Callouts.Callouts
{
    [CalloutInterface("[Adam69 Callouts] Suspicious Vehicle", CalloutProbability.Medium, "Suspicious vehicle reported", "Code 2", "LSPD")]
    public class SuspiciousVehicle : Callout
    {
        private static Vehicle motorVehicle;
        private static Vector3 spawnpoint;
        private static Blip vehBlip;


        public override bool OnBeforeCalloutDisplayed()
        {
            var list = new List<Vector3>
            {
                new(-1104.28f, -1509.72f, 4.65f),
                new(138.39f, -1070.77f, 29.19f),
                new(-1206.27f, -1444.60f, 4.34f),
                new(-1154.73f, -1986.52f, 13.16f),
                new(1075.43f, -791.60f, 58.26f),
                new(-2289.43f, 409.98f, 174.47f),
                new(-99.34f, 6374.28f, 31.47f),
            };
            spawnpoint = LocationChooser.ChooseNearestLocation(list);
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            if (Settings.BluelineDispatchIntegration == true)
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CRIME_SUSPICIOUS_ACTIVITY_01", spawnpoint);
            }
            else
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_SuspicousVehicle_Audio");
            }
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of a suspicious vehicle");
            CalloutMessage = "suspicious vehicle reported by a citizen.";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Suspicious Vehicle callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Suspicious Vehicle", "~b~Dispatch~w~: Vehicle and Suspect has been spotted. Respond ~y~Code 2~w~.");

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_2_Audio");

            motorVehicle = new Vehicle(spawnpoint)
            {
                IsPersistent = true,
                IsStolen = false
            };

            if (motorVehicle.IsValid())
            {
                vehBlip = motorVehicle.AttachBlip();
                vehBlip.Color = System.Drawing.Color.Red;
                vehBlip.Alpha = 0.75f;
                vehBlip.IsRouteEnabled = true;
            }

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            motorVehicle?.Delete();
            vehBlip?.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if (MainPlayer.DistanceTo(motorVehicle) <= 10f)
            {
                Game.DisplayHelp("Deal with the situation to your liking.", 5000);
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
                    return;
                }

                End();
            }

            base.Process();
        }

        public override void End()
        {
            motorVehicle?.Dismiss();
            vehBlip?.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Suspicious Vehicle", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

            base.End();

            bool missionMessages = Settings.MissionMessages;
            if (missionMessages == true)
            {
                BigMessageThread bigMessage = new BigMessageThread();

                bigMessage.MessageInstance.ShowColoredShard("Callout Completed!", "You are now ~g~CODE 4~w~.", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);
            }
            else
            {
                return;
            }

            Game.LogTrivial("Adam69 Callouts [LOG]: Suspicious Vehicle callout is Code 4!");
        }
    }
}