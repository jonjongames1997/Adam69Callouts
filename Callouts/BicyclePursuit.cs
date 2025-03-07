using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] Bicycle Pursuit", CalloutProbability.Medium, "Bicyclist evading arrest", "CODE 3", "LSPD")]

    public class BicyclePursuit : Callout
    {
        private static readonly string[] bikeList = new string[] { "bmx", "cruiser", "fixter", "scorcher", "tribike", "tribike2", "tribike3" };
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
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Bicycle_Pursuit_Audio");
            CalloutMessage = "An officer reporting a civilian is evading arrest.";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Bicycle Pursuit callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Bicycle Pursuit", "~b~Dispatch~w~: The suspect has been spotted! Respond ~r~Code 3~w~.");

            bool helpMessages = Settings.HelpMessages;
            if (helpMessages)
            {
                Game.DisplayHelp("Press ~y~" + Settings.EndCall + "~w~ at anytime to end the callout");
            }

            bicycle = new Vehicle(bikeList[new Random().Next((int)bikeList.Length)], spawnpoint);
            bicycle.IsPersistent = true;
            bicycle.IsValid();
            bicycle.IsStolen = true;

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

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (bicycle) bicycle.Delete();
            if (blip) blip.Delete();
            if (suspect) suspect.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if (Settings.HelpMessages == true)
            {
                Game.DisplayHelp("Chase the bicycle and asrrest the suspect.");
            }
            else
            {
                Settings.HelpMessages = false;
            }

            if (MainPlayer.IsDead) End();
            if (Game.IsKeyDown(Settings.EndCall)) End();

            base.Process();
        }

        public override void End()
        {
            if (suspect) suspect.Dismiss();
            if (bicycle) bicycle.Delete();
            if (blip) blip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Bicycle Pursuit", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Bicycle Pursuit callout is Code 4!");
        }
    }
}
