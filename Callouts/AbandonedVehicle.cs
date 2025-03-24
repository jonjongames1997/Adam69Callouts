using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{
    [CalloutInterface("[Adam69 Callouts] Abandoned Vehicle", CalloutProbability.Medium, "Abandoned vehicle reported", "Code 1", "LEO")]
    public class AbandonedVehicle : Callout
    {
        private static Vehicle _vehicle;
        private static Vector3 _spawnPoint;
        private static Blip _vehicleBlip;

        public override bool OnBeforeCalloutDisplayed()
        {
            _spawnPoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(500f));
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of an Abandoned Vehicle");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Abandoned_Vehicle_Callout_Audio");
            CalloutMessage = "Vehicle left abandoned several weeks ago. No owner to be found on scene.";
            CalloutPosition = _spawnPoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[Adam69 Callouts LOG]: Abandoned Vehicle callout accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Abandoned Vehicle", "~b~Dispatch~w~: The vehicle has been spotted! Respond ~r~Code 2~w~.");

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_1_Audio");

            _vehicle = new Vehicle(_spawnPoint);
            if (_vehicle.IsValid())
            {
                _vehicle.IsPersistent = true;

                _vehicleBlip = _vehicle.AttachBlip();
                _vehicleBlip.Color = System.Drawing.Color.Yellow;
                _vehicleBlip.Alpha = 0.5f;
                _vehicleBlip.IsRouteEnabled = true;
            }
            else
            {
                Game.LogTrivial("[Adam69 Callouts LOG]: Failed to create vehicle.");
            }

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            _vehicle?.Delete();
            _vehicleBlip?.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if (MainPlayer.DistanceTo(_vehicle) <= 10f)
            {
                if (Settings.HelpMessages)
                {
                    Game.DisplayHelp("Deal with the situation as you see fit.", 5000);
                }
            }

            if (Game.IsKeyDown(Settings.EndCall)) End();

            base.Process();
        }

        public override void End()
        {
            _vehicle?.Dismiss();
            _vehicleBlip?.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Abandoned Vehicle", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

            BigMessageThread bigMessage = new BigMessageThread();

            bigMessage.MessageInstance.ShowColoredShard("Callout Completed!", "You are now ~g~CODE 4~w~.", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);

            base.End();

            Game.LogTrivial("[Adam69 Callouts LOG]: Abandoned Vehicle callout is CODE 4!");
        }
    }
}