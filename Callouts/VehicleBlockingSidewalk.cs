using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts]: Vehicle Blocking Sidewalk", CalloutProbability.Medium, "Reports of a vehicle blocking the sidewalk", "CODE 1", "LSPD")]

    public class VehicleBlockingSidewalk : Callout
    {
        private static Vehicle motorVehicle;
        private static Vector3 spawnpoint;
        private static Blip vehBlip;

        public override bool OnBeforeCalloutDisplayed()
        {
            List<Vector3> list = new()
            {
                new(-835.34f, -1137.58f, 7.29f),
                new(-1273.35f, -639.09f, 26.55f),
            };
            spawnpoint = LocationChooser.ChooseNearestLocation(list);
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            AddMinimumDistanceCheck(50f, spawnpoint);
            CalloutInterfaceAPI.Functions.SendMessage(this, "A citizen reporting a vehicle blocking sidewalk.");
            CalloutMessage = "Vehicle blocking pedestrian's way.";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Vehicle Blocking Sidewalk callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Vehicle Blocking Sidewalk", "~b~Dispatch~w~: Vehicle has been located. Respond ~g~Code 1~w~.");

            if (Settings.HelpMessages == true)
            {
                Game.DisplayHelp("Press ~y~END~w~ to end the callout at anytime");
            }
            else
            {
                Settings.HelpMessages = false;
            }

            motorVehicle = new Vehicle(spawnpoint);
            motorVehicle.IsPersistent = true;

            vehBlip = motorVehicle.AttachBlip();
            vehBlip.Color = System.Drawing.Color.AliceBlue;
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
            base.Process();

            if(MainPlayer.DistanceTo(motorVehicle) <= 10f)
            {
                Game.DisplaySubtitle("Investigate the Vehicle, check vehicle record, then call tow truck");
            }

            if (Game.IsKeyDown(Settings.EndCall)) 
            { 
                End(); 
            }
        }

        public override void End()
        {
            if (motorVehicle) motorVehicle.Delete();
            if (vehBlip) vehBlip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Vehicle Blocking Sidewalk", "~b~You~w~: Dispatch, we are ~g~Code 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURHTER_UNITS_REQUIRED");

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Vehicle Blocking Sidewalk callout is Code 4!");
        }
    }
}
