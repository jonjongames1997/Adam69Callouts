using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{
    [CalloutInterface("[Adam69 Callouts] Bicycle Pursuit", CalloutProbability.Medium, "Bicyclist evading arrest", "CODE 3", "LSPD")]
    public class BicyclePursuit : Callout
    {
        private static readonly string[] bikeList = { "bmx", "cruiser", "fixter", "scorcher", "tribike", "tribike2", "tribike3" };
        private static readonly Random random = new();
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
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CRIME_PERSON_IN_A_STOLEN_VEHICLE_01", spawnpoint);
            CalloutMessage = "An officer reporting a civilian is evading arrest.";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Bicycle Pursuit callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Bicycle Pursuit", "~b~Dispatch~w~: The suspect has been spotted! Respond ~r~Code 3~w~.");

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_3_Audio");

            bicycle = new Vehicle(bikeList[new Random().Next((int)bikeList.Length)], spawnpoint)
            {
                IsPersistent = true,
                IsStolen = true
            };

            bicycle.IsValid();

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
            LSPD_First_Response.Mod.API.Functions.RequestBackup(suspect.Position, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.StateUnit);
            LSPD_First_Response.Mod.API.Functions.RequestBackup(suspect.Position, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
            LSPD_First_Response.Mod.API.Functions.RequestBackup(suspect.Position, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            bicycle?.Delete();
            blip?.Delete();
            suspect?.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            bool helpMessages = Settings.HelpMessages;
            if (helpMessages == true)
            {
                Game.DisplayHelp("Chase the bicycle and arrest the suspect.", 5000);
            }
            else
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Help messages are disabled in the config file.");
                return;
            }

            if (MainPlayer.IsDead || Game.IsKeyDown(Settings.EndCall))
            {
                bool missionMessages = Settings.MissionMessages;
                if (missionMessages == true)
                {
                    BigMessageThread bigMessage = new BigMessageThread();

                    bigMessage.MessageInstance.ShowColoredShard("Callout Failed!", "~r~You have failed the callout.", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);
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
            suspect?.Dismiss();
            bicycle?.Delete();
            blip?.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Bicycle Pursuit", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
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

            Game.LogTrivial("Adam69 Callouts [LOG]: Bicycle Pursuit callout is Code 4!");
        }
    }
}