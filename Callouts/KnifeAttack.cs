using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] Knife Attack", CalloutProbability.Medium, "Knife attack reported", "Code 3", "LSPD")]
    public class KnifeAttack : Callout
    {

        // General variables
        private static Ped suspect;
        private static Ped victim;
        private static float suspectHeading;
        private static float victimHeading;
        private static Vector3 spawnpoint;
        private static Vector3 victimSpawn;
        private static Blip suspectBlip;
        private static Blip victimBlip;
        private static bool isVictimDead = false;
        private static int _scenario;
        private static bool hasBegunAttacking;
        private static bool isArmed;
        private static bool hasPursuitBegun;
        private static bool hasSpoke;
        private static bool ispursuitCreated = false;
        private static LHandle pursuit;


        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = new();
            suspectHeading = 0f;
            victimSpawn = new();
            victimHeading = 0f;
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of a knife attack in progress");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_KnifeAttack_Audio");
            CalloutMessage = "Knife attack in progress";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[Adam69 Callouts LOG]: Knife Attack callout accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Knife Attack", "~b~Dispatch~w~: The suspect has been located. Respond ~r~Code 3~w~.");

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_3_Audio");

            suspect = new Ped(spawnpoint)
            {
                IsPersistent = true,
                IsPositionFrozen = true,
                BlockPermanentEvents = true
            };
            suspect.IsValid();

            victim = new Ped(victimSpawn)
            {
                IsPersistent = true,
                IsPositionFrozen = true,
                BlockPermanentEvents = true
            };

            victim.IsValid();

            suspectBlip = suspect.AttachBlip();
            suspectBlip.Color = System.Drawing.Color.Red;
            suspectBlip.IsRouteEnabled = true;
            suspectBlip.Alpha = 0.75f;

            victimBlip = victim.AttachBlip();
            victimBlip.Color = System.Drawing.Color.Blue;
            victimBlip.IsRouteEnabled = true;
            victimBlip.Alpha = 0.75f;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (suspect.Exists())
            {
                suspect.Delete();
            }
            if (victim.Exists())
            {
                victim.Delete();
            }
            if (suspectBlip.Exists())
            {
                suspectBlip.Delete();
            }
            if (victimBlip.Exists())
            {
                victimBlip.Delete();
            }
            base.OnCalloutNotAccepted();

        }

        public override void Process()
        {


            base.Process();
        }
    }
}
