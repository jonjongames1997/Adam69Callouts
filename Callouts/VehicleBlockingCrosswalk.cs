﻿using CalloutInterfaceAPI;
using StopThePed.API;

namespace Adam69Callouts.Callouts
{


    [CalloutInterface("[Adam69 Callouts] Vehicle Blocking Crosswalk", CalloutProbability.Medium, "Citizen's reporting a vehicle blocking crosswalk", "CODE 2", "LSCSO")]

    public class VehicleBlockingCrosswalk : Callout
    {
        private static Vehicle motorVehicle;
        private static Vector3 spawnpoint;
        private static Blip vehBlip;


        public override bool OnBeforeCalloutDisplayed()
        {
            List<Vector3> list = new()
            {
                new(103.17f, -1344.18f, 29.04f),
                new(-752.53f, -1118.11f, 10.27f),
            };
            spawnpoint = LocationChooser.ChooseNearestLocation(list);
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "A vehicle blocking crosswalk");
            CalloutMessage = "Multiple reports of a vehicle blocking crosswalk";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Vehicle Blocking Crosswalk callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Vehicle Blocking Crosswalk", "~b~Dispatch~w~: The vehicle has been located. Respond ~y~Code 2~w~.");

            if (Settings.HelpMessages == true)
            {
                Game.DisplayHelp("Press ~y~" + Settings.EndCall + "~w~ at anytime to end the callout");
            }
            else
            {
                Settings.HelpMessages = false;
            }

            motorVehicle = new Vehicle(spawnpoint);
            motorVehicle.IsPersistent = true;
            motorVehicle.IsStolen = false;

            vehBlip = motorVehicle.AttachBlip();
            vehBlip.Color = System.Drawing.Color.BurlyWood;
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
                Game.DisplaySubtitle("Check the vehicle record, search the vehicle (If you have probable cause), then tow the vehicle.");
            }

            if (Game.IsKeyDown(Settings.EndCall))
            {
                End();
            }

            base.Process();
        }

        public override void End()
        {
            if (motorVehicle) motorVehicle.Delete();
            if (vehBlip) vehBlip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Vehicle Blocking Crosswalk", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURHTER_UNITS_REQUIRED");

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Vehicle Blocking Crosswalk callout is Code 4!");
        }
    }
}
