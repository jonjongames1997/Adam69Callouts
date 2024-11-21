using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] Abandoned Vehicle", CalloutProbability.Medium, "Abandoned vehicle reported", "Code 1", "")]

    public class AbandonedVehicle : Callout
    {
        private static Vehicle _Vehicle;
        private static Vector3 spawnpoint;
        private static Blip _vehicleBlip;

        public override bool OnBeforeCalloutDisplayed()
        {
            List<Vector3> list = new()
            {
                new(1377.12f, 3598.41f, 34.88f),
                new(621.12f, 2723.96f, 41.84f),
            };
            spawnpoint = LocationChooser.ChooseNearestLocation(list);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of an Abandoned Vehicle");
            CalloutMessage = "Vehicle left abandoned several weeks ago. No owner to be found on scene.";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[Adam69 Callouts LOG]: Bicycle Blocking Roadway callout accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Abandoned Vehicle", "~b~Dispatch~w~: The vehicle has been spotted! Respond ~r~Code 2~w~.");
            Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout", false);

            _Vehicle = new Vehicle(spawnpoint);
            _Vehicle.IsValid();
            _Vehicle.IsPersistent = true;

            _vehicleBlip = _Vehicle.AttachBlip();
            _vehicleBlip.Color = System.Drawing.Color.Yellow;
            _vehicleBlip.Alpha = 0.5f;
            _vehicleBlip.IsRouteEnabled = true;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (_Vehicle) _Vehicle.Delete();
            if (_vehicleBlip) _vehicleBlip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if (MainPlayer.DistanceTo(_Vehicle) <= 10f)
            {
                Game.DisplayHelp("Deal with the situation as you see fit.");
            }

            if (Game.IsKeyDown(Settings.EndCall)) End();

            base.Process();
        }

        public override void End()
        {
            if (_Vehicle) _Vehicle.Dismiss();
            if (_vehicleBlip) _vehicleBlip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Abandoned Vehicle", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");

            base.End();

            Game.LogTrivial("[Adam69 Callouts LOG]: Abandoned Vehicle callout is CODE 4!");
        }
    }
}
