using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] Bicycle Blocking Roadway", CalloutProbability.Medium, "Bicycle obstructing roadway", "Code 2", "LSPD")]

    public class BicycleBlockingRoadway : Callout
    {
        private static readonly string[] BicycleList = new string[] { "bmx", "cruiser", "fixter", "scorcher", "tribike", "tribike2", "tribike3" };
        private static Vehicle thebike;
        private static Blip blip;
        private static Vector3 spawnpoint;

        public override bool OnBeforeCalloutDisplayed()
        {
            List<Vector3> list = new()
            {
                new(-1146.43f, -1496.17f, 4.37f),
                new(898.76f, -2458.87f, 28.61f),
                new(969.30f, -1436.41f, 31.41f),
                new(-113.87f, 6417.88f, 31.44f),
                new(1694.77f, 4643.75f, 43.62f),
                new(1704.11f, 3808.54f, 35.01f),
            };
            spawnpoint = LocationChooser.ChooseNearestLocation(list);
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of a bicycle blocking traffic");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("ILLEGALLY_PARKED_VEHICLE", spawnpoint);
            CalloutMessage = "Abandoned bicycle left in the street";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            if (Settings.EnableLogs)
            {
                Game.LogTrivial("[Adam69 Callouts LOG]: Bicycle Blocking Roadway callout accepted!");
            }

            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Bicycle Blocking Roadway", "~b~Dispatch~w~: The vehicle has been spotted! Respond ~r~Code 2~w~.");

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_2_Audio");

            thebike = new Vehicle(BicycleList[new Random().Next((int)BicycleList.Length)], spawnpoint, 0f);
            thebike.IsValid();
            thebike.IsPersistent = true;

            thebike.IsStolen = false;

            blip = thebike.AttachBlip();
            blip.Color = System.Drawing.Color.Yellow;
            blip.Alpha = 0.75f;
            blip.IsRouteEnabled = true;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {

            if (thebike) thebike.Delete();
            if (blip) blip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if (Game.IsKeyDown(System.Windows.Forms.Keys.L))
            {
                PolicingRedefined.API.BackupDispatchAPI.RequestTowServiceBackup();
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Tow_Truck_Audio");
            }

            if (MainPlayer.DistanceTo(thebike) <= 10f)
            {
                if (Settings.HelpMessages)
                {
                    Game.DisplayHelp("Deal with the situation as you see fit.", 5000);
                }
                else
                {
                    return;
                }
            }

            if (MainPlayer.IsDead)
            {
                if (Settings.MissionMessages)
                {
                    BigMessageThread bigMessage = new BigMessageThread();

                    bigMessage.MessageInstance.ShowColoredShard("Callout Failed!", "You'll get 'em next time.", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);
                }
                else
                {
                    return;
                }

                End();

                if (Game.IsKeyDown(Settings.EndCall))
                {
                    if (Settings.MissionMessages)
                    {
                        BigMessageThread bigMessage = new BigMessageThread();

                        bigMessage.MessageInstance.ShowColoredShard("Callout Complete!", "You are now ~g~CODE 4~w~.", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);
                    }

                    End();
                }
            }

            base.Process();
        }

        public override void End()
        {
            if (thebike) thebike.Dismiss();
            if (blip) blip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Vehicle Blocking Crosswalk", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

            if (Settings.MissionMessages)
            {
                BigMessageThread bigMessage = new BigMessageThread();

                bigMessage.MessageInstance.ShowColoredShard("Callout Completed!", "You are now ~g~CODE 4~w~.", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);
            }
            else
            {
                Settings.MissionMessages = false;
                return;
            }

            base.End();

            Game.LogTrivial("[Adam69 Callouts LOG]: Bicycle Blocking Roadway callout is CODE 4!");
        }

    }
}
