using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{


    [CalloutInterface("[Adam69 Callouts] - Officer Down", CalloutProbability.Medium, "Reports of an officer down", "Code 3", "LEO")]

    public class OfficerDown : Callout
    {

        private static readonly string[] pedsList = new string[] { "s_f_y_cop_01", "s_m_y_cop_01", "csb_cop", "s_f_y_sheriff_01", "s_m_y_sheriff_01", "s_m_y_hwaycop_01", "s_m_m_security_01", "s_f_y_ranger_01", "s_m_y_ranger_01" };
        private static Ped suspect;
        private static Ped officer;
        private static Blip copBlip;
        private static Vehicle emergencyVehicle;
        private static readonly string[] officerVehicle = new string[] { "police", "police2", "police3", "police4", "police5", "polgauntlet", "poldominator10", "poldorado", "polgreenwood", "polimpaler5", "polimpaler6", "polcaracara", "polcoquette4", "polfaction2", "polterminus", "dilettante2", "fbi", "pbus", "policeb", "pranger", "riot", "riot2", "sheriff", "sheriff2" };
        private static Blip officerVehicleBlip;
        private static Vector3 spawnpoint;
        private static Vector3 vehicleSpawn;
        private static Vector3 susSpawn;
        private static float officerheading;
        private static float susHeading;
        private static Blip suspectBlip;
        private static int counter;
        private static string malefemale;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = new();
            officerheading = 125.69f;
            susSpawn = new();
            susHeading = 199.65f;
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_OfficerDown_Callout_Audio");
            CalloutInterfaceAPI.Functions.SendMessage(this, "Officer panic button pressed");
            CalloutMessage = "Officer Down reported by an unknown civilian";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Officer Down callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Officer Down", "~b~Dispatch~w~: The suspect has been spotted! Respond ~r~Code 3~w~.");

            if (Settings.HelpMessages == true)
            {
                Game.DisplayHelp("Press ~y~" + Settings.EndCall + "~w~ at anytime to end the callout");
            }
            else
            {
                Settings.HelpMessages = false;
            }

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_2_Audio");

            officer = new Ped(pedsList[new Random().Next((int)pedsList.Length)], spawnpoint, 0f);
            officer.IsPersistent = true;
            officer.BlockPermanentEvents = true;
            officer.Kill();
            officer.IsValid();

            NativeFunction.Natives.APPLY_PED_DAMAGE_PACK(officer, "TD_PISTOL_FRONT", 1f, 1f);

            emergencyVehicle = new Vehicle(officerVehicle[new Random().Next((int)officerVehicle.Length)], vehicleSpawn, 0f);
            emergencyVehicle.IsPersistent = true;
            emergencyVehicle.IsValid();

            suspect = new Ped(susSpawn);
            suspect.IsPersistent = true;
            suspect.IsValid();
            suspect.BlockPermanentEvents = true;

            copBlip = officer.AttachBlip();
            copBlip.Color = System.Drawing.Color.Blue;
            copBlip.IsRouteEnabled = true;

            suspectBlip = suspect.AttachBlip();
            suspectBlip.Color = System.Drawing.Color.Red;
            suspectBlip.IsFriendly = false;

            officerVehicleBlip = emergencyVehicle.AttachBlip();
            officerVehicleBlip.Color = System.Drawing.Color.LightBlue;
            officerVehicleBlip.IsFriendly = true;

            if (suspect.IsMale)
                malefemale = "Sir";
            else
                malefemale = "Ma'am";

            counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (suspect) suspect.Delete();
            if (officer) officer.Delete();
            if (emergencyVehicle) emergencyVehicle.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Officer Down callout accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Officer Down", "~b~You~w~: Suspect has been spotted! Respond Code 2.");

            if(Settings.HelpMessages == true)
            {
                Game.DisplayHelp("Press ~y~" + Settings.EndCall + "~w~ to end the callout at anytime.");
            }
            else
            {
                Settings.HelpMessages = false;
            }



            base.Process();
        }

    }
}
