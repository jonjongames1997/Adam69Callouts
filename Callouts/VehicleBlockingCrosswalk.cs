using CalloutInterfaceAPI;
using StopThePed.API;

namespace Adam69Callouts.Callouts
{
    [CalloutInterface("[Adam69 Callouts] Vehicle Blocking Crosswalk", CalloutProbability.Medium, "Citizen's reporting a vehicle blocking crosswalk", "CODE 2", "LSCSO")]
    public class VehicleBlockingCrosswalk : Callout
    {
        private Vehicle motorVehicle;
        private Vector3 spawnpoint;
        private Blip vehBlip;

        /// <summary>
        /// Called before the callout is displayed to the player.
        /// </summary>
        /// <returns>True if the callout should be displayed, otherwise false.</returns>
        public override bool OnBeforeCalloutDisplayed()
        {
            var list = new List<Vector3>
            {
                new(103.17f, -1344.18f, 29.04f),
                new(-752.53f, -1118.11f, 10.27f),
                new(-657.13f, 280.97f, 80.86f),
                new(-103.97f, 239.40f, 97.87f),
                new(-454.79f, -260.03f, 3596f),
                new(-1310.03f, -70.61f, 47.95f),
                new(-2155.47f, -341.27f, 13.21f),
            };
            spawnpoint = LocationChooser.ChooseNearestLocation(list);
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            if (Settings.BluelineDispatch == true)
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_VehicleBlockingCrosswalk_Audio");
            }
            else
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CRIME_ROAD_BLOCKADE_01", spawnpoint);
            }
            CalloutInterfaceAPI.Functions.SendMessage(this, "A vehicle blocking crosswalk");
            CalloutMessage = "Multiple reports of a vehicle blocking crosswalk";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        /// <summary>
        /// Called when the callout is accepted by the player.
        /// </summary>
        /// <returns>True if the callout was successfully accepted, otherwise false.</returns>
        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Vehicle Blocking Crosswalk callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Vehicle Blocking Crosswalk", "~b~Dispatch~w~: The vehicle has been located. Respond ~y~Code 2~w~.");

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_2_Audio");

            motorVehicle = new Vehicle(spawnpoint)
            {
                IsPersistent = true,
                IsStolen = false
            };

            vehBlip = motorVehicle.AttachBlip();
            vehBlip.Color = System.Drawing.Color.BurlyWood;
            vehBlip.IsRouteEnabled = true;

            return base.OnCalloutAccepted();
        }

        /// <summary>
        /// Called when the callout is not accepted by the player.
        /// </summary>
        public override void OnCalloutNotAccepted()
        {
            motorVehicle?.Delete();
            vehBlip?.Delete();

            base.OnCalloutNotAccepted();
        }

        /// <summary>
        /// Called every frame while the callout is active.
        /// </summary>
        public override void Process()
        {
            if (MainPlayer.DistanceTo(motorVehicle) <= 10f)
            {
                Game.DisplaySubtitle("Check the vehicle record, search the vehicle (If you have probable cause), then tow the vehicle.", 5000);
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

        /// <summary>
        /// Called when the callout ends.
        /// </summary>
        public override void End()
        {
            motorVehicle?.Delete();
            vehBlip?.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Vehicle Blocking Crosswalk", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

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

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Vehicle Blocking Crosswalk callout is Code 4!");
        }
    }
}