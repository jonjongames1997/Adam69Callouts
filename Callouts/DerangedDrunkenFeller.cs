using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] - Deranged Drunken Feller", CalloutProbability.Medium, "Reports of a drunken feller", "Code 2", "LSPD")]


    public class DerangedDrunkenFeller : Callout
    {
        private static Ped suspect;
        private static Vector3 spawnpoint;
        private static Blip susBlip;
        private static int counter;
        private static string malefemale;
        private static int copgender;
        private static readonly string[] wepList = new string[] { "weapon_pistol", "weapon_combatmg", "weapon_combatpistol" };

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(500f));
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "A Deranged Drunken Feller has been reported in the area. Respond Code 2.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Deranged_Drunken_Feller_Audio");
            CalloutMessage = "A Deranged Drunken Feller Reported";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Deranged Drunken Feller callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "Deranged Drunken Feller", "~b~Dispatch~w~: Suspect has been located. Respond ~r~Code 2~w~.");

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_2_Audio");

            susBlip = suspect.AttachBlip();
            susBlip.Color = System.Drawing.Color.Red;
            susBlip.IsRouteEnabled = true;

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
            if (susBlip) susBlip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if (MainPlayer.DistanceTo(suspect) <= 10f)
            {
                bool helpMessages = Settings.HelpMessages;
                if (helpMessages)
                {
                    Game.DisplayHelp("Press ~y~" + Settings.Dialog.ToString() + "~w~ to interact with the suspect.", 5000);
                }

                if (Game.IsKeyDown(Settings.Dialog))
                {
                    counter++;

                    if (counter == 1)
                    {
                        Game.DisplaySubtitle("~b~You~w~: What goin' on, feller? Have anything to drink today?");
                    }
                    if (counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: *slurring* What you want, officer pigfucker?");
                    }
                    if (counter == 3)
                    {
                        Game.DisplaySubtitle("~b~You~w~: Ok, we'll do a few tests to see if you're drunk.");
                    }
                    if (counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: *slurring* You got to catch me first, donut eater.");
                    }
                    if (counter == 5)
                    {
                        Game.DisplaySubtitle("Convo ended. Chase and arrest the suspect.");
                        suspect.Tasks.FightAgainst(MainPlayer);
                        suspect.Armor = 500;
                        suspect.Inventory.GiveNewWeapon(wepList[new Random().Next((int)wepList.Length)], 500, true);
                    }
                }
            }

            if (MainPlayer.IsDead)
            {
                BigMessageThread bigMessage = new BigMessageThread();

                bigMessage.MessageInstance.ShowColoredShard("~r~MISSION FAILED!", "You'll get em next time", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);

                End();
            }

            if (Game.IsKeyDown(Settings.EndCall)) End();

            base.Process();
        }

        public override void End()
        {
            if (suspect.Exists()) suspect.Dismiss();
            if (susBlip) susBlip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Deranged Drunken Feller", "~b~You~w~: Dispatch, we are ~g~Code 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

            BigMessageThread bigMessage = new BigMessageThread();

            bigMessage.MessageInstance.ShowColoredShard("~g~Code 4", "Suspect Neutralized!", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Deranged Drunken Feller callout is CODE 4!");
        }
    }
}