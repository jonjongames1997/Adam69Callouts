using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] - Clown Car Pursuit", CalloutProbability.Medium, "A pursuit with armed and dangerous clowns", "Code 3", "SWAT")]

    public class ClownCarPursuit : Callout
    {
        private static readonly string[] clownCar = new string[] { "issi6", "speedo2", "burrito3" };
        private static readonly Random random = new Random();

        private Vehicle TheClownCar;
        private Ped _suspect1;
        private Ped _suspect2;
        private Vector3 _spawnpoint;
        private Blip _blip;
        private Blip _blip2;
        private Blip _VehicleBlip;
        private LHandle _pursuit;
        private bool _pursuitCreated = false;

        private Vector3 lastTrackedPosition;
        private const float GPS_UPDATE_THRESHOLD = 10f;

        public override bool OnBeforeCalloutDisplayed()
        {
            _spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(1000f));
            ShowCalloutAreaBlipBeforeAccepting(_spawnpoint, 100f);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("Adam69Callouts_ClownCarPursuit_Crimes_01", _spawnpoint);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of a clown car pursuit.");
            CalloutMessage = "Reports of a armed clown car pursuit. Approach with caution";
            CalloutPosition = _spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            if (Settings.EnableLogs)
            {
                Game.LogTrivial("[Adam69 Callouts] LOG: Clown Car Pursuit Callout accepted!");
                LoggingManager.Log("[Adam69 Callouts] LOG: Clown Car Pursuit Callout accepted!");
            }

            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Clown Car Pursuit", "~b~Dispatch~w~: The suspect has been spotted! Respond ~r~Code 3~w~.");

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_3_Audio");

            TheClownCar = new Vehicle(clownCar[random.Next(clownCar.Length)], _spawnpoint);
            if (!TheClownCar.IsValid())
            {
                if (Settings.EnableLogs)
                {
                    Game.LogTrivial("Adam669 Callouts [LOG]: Failed to create a clown car!");
                    LoggingManager.Log("Adam69 Callouts [LOG]: Failed to create a clown car!");
                }

                return false;
            }

            TheClownCar.IsPersistent = true;
            TheClownCar.FuelTankHealth = 500;
            TheClownCar.FuelLevel = 100;
            TheClownCar.DirtLevel = 0;

            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Dispatch", "Loading ~g~Information~w~ of the ~o~LSPD Database~w~...");

            LSPD_First_Response.Mod.API.Functions.DisplayVehicleRecord(TheClownCar, true);

            _suspect1 = new Ped("s_m_y_clown_01", _spawnpoint, 0f);
            if (!_suspect1.IsValid())
            {
                if (Settings.EnableLogs)
                {
                    Game.LogTrivial("Adam69 Callouts [LOG]: Failed to create the clown ped!");
                    LoggingManager.Log("Adam69 Callouts [LOG]: Failed to create the clown ped!");
                }
                CleanupEntities();
                return false;
            }

            _suspect1.WarpIntoVehicle(TheClownCar, -1);
            _suspect1.Inventory.GiveNewWeapon("weapon_microsmg", 900, true);
            _suspect1.IsPersistent = true;
            _suspect1.BlockPermanentEvents = true;
            _suspect1.Tasks.CruiseWithVehicle(TheClownCar, 60f, VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.DriveAroundPeds | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.DriveAroundVehicles | VehicleDrivingFlags.AllowWrongWay | VehicleDrivingFlags.Emergency | VehicleDrivingFlags.AvoidHighways);

            _suspect2 = new Ped("s_m_y_clown_01", _spawnpoint, 0f);
            if (!_suspect2.IsValid())
            {
                if (Settings.EnableLogs)
                {
                    Game.LogTrivial("Adam69 Callouts [LOG]: Failed to create the clown ped!");
                    LoggingManager.Log("Adam69 Callouts [LOG]: Failed to create the clown ped!");
                }
                CleanupEntities();
                return false;
            }

            _suspect2.WarpIntoVehicle(TheClownCar, 0);
            _suspect2.Inventory.GiveNewWeapon("weapon_minismg", 900, true);
            _suspect2.IsPersistent = true;
            _suspect2.BlockPermanentEvents = true;

            _suspect2.Tasks.FightAgainst(MainPlayer);

            _blip = _suspect1.AttachBlip();
            _blip.Color = System.Drawing.Color.Red;

            _blip2 = _suspect2.AttachBlip();
            _blip2.Color = System.Drawing.Color.Red;

            _VehicleBlip = TheClownCar.AttachBlip();
            _VehicleBlip.Color = System.Drawing.Color.DarkRed;
            _VehicleBlip.IsRouteEnabled = true;

            _VehicleBlip.RouteColor = System.Drawing.Color.Yellow;

            lastTrackedPosition = TheClownCar.Position;

            _pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(_pursuit, _suspect1);
            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(_pursuit, _suspect2);
            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
            _pursuitCreated = true;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            CleanupEntities();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            // GPS Tracking the vehicle during the pursuit

            if (_VehicleBlip != null && _VehicleBlip.Exists() && TheClownCar != null && TheClownCar.Exists())
            {
                float distanceMoved = Vector3.Distance(lastTrackedPosition, TheClownCar.Position);

                if (distanceMoved > GPS_UPDATE_THRESHOLD)
                {
                    _VehicleBlip.Position = TheClownCar.Position;
                    lastTrackedPosition = TheClownCar.Position;
                }
            }

            if (MainPlayer.IsDead)
            {
                BigMessageThread bigMessage = new BigMessageThread();
                bigMessage.MessageInstance.ShowColoredShard("MISSION FAILED!", "You'll get 'em next time!", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);
                End();
            }

            if (Game.IsKeyDown(Settings.EndCall))
            {
                BigMessageThread bigMessage = new BigMessageThread();
                bigMessage.MessageInstance.ShowColoredShard("Callout Ended", "Return back to the skreets, Officer!", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);

                End();
            }

            base.Process();
        }

        public override void End()
        {
            CleanupEntities();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "Clown Car Pursuit", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

            if (Settings.MissionMessages)
            {
                BigMessageThread bigMessage = new BigMessageThread();
                bigMessage.MessageInstance.ShowColoredShard("MISSION COMPLETE!", "Good Job, Officer. Get back on the Skreets!", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", true);
            }

            if (Settings.EnableLogs)
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Clown Car Pursuit callout is code 4! Return to the skreets, Officer!");
                LoggingManager.Log("Adam69 Callouts [LOG]: Clown Car Pursuit callout is code 4! Return to the skreets, Officer!");
            }

            base.End();
        }

        private void CleanupEntities()
        {
            if (_suspect1 != null && _suspect1.Exists()) _suspect1.Dismiss();
            if (_suspect2 != null && _suspect2.Exists()) _suspect2.Dismiss();
            if (TheClownCar != null && TheClownCar.Exists()) TheClownCar.Dismiss();
            if (_VehicleBlip != null && _VehicleBlip.Exists()) _VehicleBlip.Delete();
            if (_blip != null && _blip.Exists()) _blip.Delete();
            if (_blip2 != null && _blip2.Exists()) _blip2.Delete();
        }
    }
}
