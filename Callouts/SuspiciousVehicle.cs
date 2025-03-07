using CalloutInterfaceAPI;
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
            List<Vector3> list = new()
            {
                new(-1104.28f, -1509.72f, 4.65f),
                new(138.39f, -1070.77f, 29.19f),
            };
            spawnpoint = LocationChooser.ChooseNearestLocation(list);
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_SuspicousVehicle_Audio");
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of a suspicious vehicle");
            CalloutMessage = "suspicious vehicle reported by a citizen.";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Suspicious Vehicle callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Suspicious Vehicle", "~b~Dispatch~w~: Vehicle and Suspect has been spotted. Respond ~y~Code 2~w~.");

            bool helpMessages = Settings.HelpMessages;
            if (helpMessages)
            {
                Game.DisplayHelp("Press ~y~" + Settings.EndCall + "~w~ at anytime to end the callout");
            }

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_2_Audio");

            motorVehicle = new Vehicle(spawnpoint);
            motorVehicle.IsPersistent = true;
            motorVehicle.IsValid();

            vehBlip = motorVehicle.AttachBlip();
            vehBlip.Color = System.Drawing.Color.Red;
            vehBlip.Alpha = 0.75f;
            vehBlip.IsRouteEnabled = true;


            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (motorVehicle) motorVehicle.Delete();
            if (vehBlip) vehBlip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if(MainPlayer.DistanceTo(motorVehicle) <= 10f)
            {
                Game.DisplayHelp("Deal with the situation to your liking.");
            }

            if (Game.IsKeyDown(Settings.EndCall)) End();

            base.Process();
        }

        public override void End()
        {
            if (motorVehicle) motorVehicle.Dismiss();
            if (vehBlip) vehBlip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Suspicious Vehicle", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Suspicious Vehicle callout is Code 4!");
        }
    }
}
