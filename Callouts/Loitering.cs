﻿using CalloutInterfaceAPI;
using StopThePed.API;
using StopThePed;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] Loitering", CalloutProbability.Medium, "Reports of an individual loitering", "CODE 2", "LSPD")]

    public class Loitering : Callout
    {
        private static Ped suspect;
        private static Blip susBlip;
        private static Vector3 spawnpoint;
        private static int counter;
        private static string malefemale;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(100f));
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 WE_HAVE_01 CRIME_CITIZEN_REQUESTING_REMOVAL_OF_BEGGARS UNITS_RESPOND_CODE_02_02");
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of loitering");
            CalloutMessage = "An individual loitering";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Loitering callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Loitering", "~b~Dispatch~w~: Suspect has been spotted. Respond ~y~Code 2~w~.");

            if (Settings.HelpMessages == true)
            {
                Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout");
            }
            else
            {
                Settings.HelpMessages = false;
            }

            suspect = new Ped(spawnpoint);
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;
            suspect.IsValid();

            suspect.Tasks.PlayAnimation(new AnimationDictionary("timetable@tracy@ig_5@idle_a"), "idle_a", -1f, AnimationFlags.Loop);
            suspect.KeepTasks = true;

            susBlip = suspect.AttachBlip();
            susBlip.Color = System.Drawing.Color.Pink;
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
            if (susBlip) susBlip.Delete();
            if (suspect) suspect.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            base.Process();

            if (MainPlayer.DistanceTo(suspect) <= 10f)
            {
                if (Settings.HelpMessages == true)
                {
                    Game.DisplayHelp("Press ~y~Y~w~ to interact with suspect.");
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
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~b~You~w~: Pardon me, " + malefemale + ". What are you doing here loitering for?");
                    }
                    if (counter == 2)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~r~Suspect~w~: What you mean, officer? I'm not loitering. You are mistaken. I'm a street entertainer. You want to be entertained, Officer?");
                    }
                    if (counter == 3)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~b~You~w~: Oh, you're of those people and no, thank you " + malefemale + ". I just need to verify your information and we'll go from there.");
                    }
                    if (counter == 4)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("mini@strip_club@private_dance@idle"), "priv_dance_idle", 1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~r~Suspect~w~: Are you sure, Officer? It's FREE. Nothing says in the law that I can entertain you for free.");
                    }
                    if (counter == 5)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~b~You~w~: I'm positive.");
                    }
                    if (counter == 6)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("mini@strip_club@private_dance@idle"), "priv_dance_idle", 1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~r~Suspect~w~: Ok, Officer. My offer stands whenever you want to be entertained.");
                    }
                    if (counter == 7)
                    {
                        Game.DisplaySubtitle("~b~You~w~: Dispatch, request a 10-27.");
                        StopThePed.API.Functions.requestDispatchPedCheck(true);
                    }
                    if (counter == 8)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("Conversation Ended. Deal with the situation you may see fit.");
                    }
                }
            }


            if (Game.IsKeyDown(Settings.EndCall))
            {
                End();
            }
        }

        public override void End()
        {
            if (suspect) suspect.Dismiss();
            if (susBlip) susBlip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Loitering", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURHTER_UNITS_REQUIRED");

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Loitering callout is Code 4!");
        }
    }
}
