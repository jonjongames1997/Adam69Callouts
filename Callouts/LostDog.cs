using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] Lost Dog", CalloutProbability.Medium, "Citizen;s reporting a lost dog", "Code 2", "LSPD")]

    public class LostDog : Callout
    {
        private static readonly string[] petList = new string[] { "a_c_poodle", "a_c_pug", "a_c_retriever", "a_c_rottweiler", "a_c_shepard", "a_c_westy", "a_c_husky" };
        private static Ped petDog;
        private static Blip petBlip;
        private static Vector3 spawnpoint;


        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(1000f));
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            AddMinimumDistanceCheck(50f, spawnpoint);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Lost dog reported.");
            CalloutMessage = "A civilian reporting a lost dog.";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Lost Dog callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Lost Dog", "~b~Dispatch~w~: The dog has been spotted! Respond ~g~Code 2~w~.");
            Game.DisplayHelp("Press ~y~End~w~ to end the callout at anytime", false);

            petDog = new Ped(petList[new Random().Next((int)petList.Length)], spawnpoint, 0f);
            petDog.IsPersistent = true;
            petDog.BlockPermanentEvents = true;
            petDog.Tasks.PlayAnimation(new AnimationDictionary("creatures@rottweiler@amb@world_dog_sitting@base"), "base", -1, AnimationFlags.Loop);
            petDog.IsValid();

            petBlip = petDog.AttachBlip();
            petBlip.Color = System.Drawing.Color.Blue;
            petBlip.IsRouteEnabled = true;


            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (petDog) petDog.Delete();
            if (petBlip) petBlip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            base.Process();

            if(MainPlayer.DistanceTo(petDog) <= 10f)
            {
                Game.DisplaySubtitle("Call Animal Control to take the dog to the impound or Find it's owner.");
            }

            if (Game.IsKeyDown(Settings.EndCall)) End();
        }

        public override void End()
        {
            if (petDog) petDog.Dismiss();
            if (petBlip) petBlip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Lost Dog", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURHTER_UNITS_REQUIRED");

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Lost Dog callout is code 4!");
        }
    }
}
