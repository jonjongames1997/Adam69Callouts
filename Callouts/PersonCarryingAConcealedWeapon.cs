using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] Person Carrying A Concealed Weapon", CalloutProbability.Medium, "Reports of an individual carrying a concealed weapon", "CODE 2", "LSPD")]

    public class PersonCarryingAConcealedWeapon : Callout
    {
        private static readonly string[] wepList = new string[] { "WEAPON_PISTOL", "WEAPON_COMBATPISTOL", "WEAPON_GUSENBURG", "WEAPON_STUNGUN", "WEAPON_MARKSMANPISTOL", "WEAPON_PUMPSHOTGUN", "WEAPON_CARBINERIFLE" };
        private static Ped suspect;
        private static Vector3 spawnpoint;
        private static Blip susBlip;
        private static int counter;
        private static string malefemale;
        private static bool HelpMessages = true;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(1000f));
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            AddMinimumDistanceCheck(100f, spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS_01 WE_HAVE_01 CRIME_PERSON_WITH_A_FIREARM UNITS_RESPOND_CODE_03_02", spawnpoint);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Citizen's reporting an individual with a gun.");
            CalloutMessage = "Person carrying a firearm";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Person Carrying A Concealed Weapon callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Person Carrying A Concealed Weapon", "~b~Dispatch~w~: Suspect has been spotted. Respond ~y~Code 2~w~.");

            if (Settings.HelpMessages == true)
            {
                Game.DisplayHelp("Press ~y~" + Settings.EndCall + "~w~ at anytime to end the callout");
            }
            else
            {
                Settings.HelpMessages = false;
            }

            suspect = new Ped(spawnpoint);
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;

            suspect.Tasks.Wander();

            suspect.Armor = 500;

            susBlip = suspect.AttachBlip();
            susBlip.Color = System.Drawing.Color.Aqua;
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
            base.Process();

            if (MainPlayer.DistanceTo(suspect) <= 10f)
            {
                if(Settings.HelpMessages == true)
                {
                    Game.DisplayHelp("Press ~y~ " + Settings.Dialog + " to interact with suspect.");
                }
                else
                {
                     Settings.HelpMessages = false;
                }

                if (Game.IsKeyDown(Settings.Dialog))
                {
                    counter++;

                    if (counter == 1)
                    {
                        Game.DisplaySubtitle("~b~You~w~: LSPD, freeze, mothertrucker!");
                    }
                    if (counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: What's going on, Officer?");
                    }
                    if (counter == 3)
                    {
                        Game.DisplaySubtitle("~b~You~w~: Don't reach for anything, " + malefemale + ".");
                    }
                    if (counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: What the f are you talking about? I don't have a weapon on me!");
                    }
                    if (counter == 5)
                    {
                        Game.DisplaySubtitle("~b~You~w~: I said, 'Don't reach for anything'. keep your hands where I can see them.");
                    }
                    if (counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: And I said, 'I don't have a weapon on me', motherfucker! Are you deaf?");
                    }
                    if (counter == 7)
                    {
                        Game.DisplaySubtitle("~b~You~w~: I will shoot if you reach for anything!");
                    }
                    if (counter == 8)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: Screw this! Die, motherfucker, you!");
                        suspect.Tasks.FightAgainst(MainPlayer);
                        suspect.Inventory.GiveNewWeapon("WEAPON_COMBATPISTOL", 500, true);
                    }
                }
            }

            if (MainPlayer.IsDead)
            {
                End();
            }

            if (Game.IsKeyDown(Settings.EndCall))
            {
                End();
            }
        }

        public override void End()
        {
            if (susBlip) susBlip.Delete();
            if (suspect) suspect.Dismiss();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Person Carrying A Concealed Weapon", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURHTER_UNITS_REQUIRED");

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Person Carrying A Concealed Weapon callout is Code 4!");
        }
    }
}
