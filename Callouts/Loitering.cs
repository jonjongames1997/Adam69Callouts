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
            {
                spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(100f));
                ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
                if (BluelineDispatch == true)
                {
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Loitering_Audio");
                }
                else
                {
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("SUSPICIOUS_PERSON", spawnpoint);
                }
                CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of loitering");
                CalloutMessage = "Person Loitering Reported";
                CalloutPosition = spawnpoint;

                return base.OnBeforeCalloutDisplayed();
            }
        }

        public override bool OnCalloutAccepted()
        {
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Loitering callout has been accepted!");
                Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Loitering", "~b~Dispatch~w~: Suspect located. Respond code 2.");

                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_2_Audio");

                suspect = new Ped(spawnpoint)
                {
                    IsPersistent = true,
                    BlockPermanentEvents = true
                };

                suspect.IsValid();

                suspect.Tasks.PlayAnimation(new AnimationDictionary("timetable@tracy@ig_5@idle_a"), "idle_a", -1f, AnimationFlags.Loop);
                suspect.KeepTasks = true;

                susBlip = suspect.AttachBlip();
                susBlip.Color = System.Drawing.Color.Pink;
                susBlip.IsRouteEnabled = true;

                if (suspect.IsMale)
                    malefemale = "sir";
                else
                    malefemale = "ma'am";

                counter = 0;

                return base.OnCalloutAccepted();
            }
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

                if (Game.IsKeyDown(Settings.Dialog))
                {
                    if (MainPlayer.DistanceTo(suspect) <= 10f)
                    {
                        Game.DisplayHelp("Press ~y~" + Settings.Dialog.ToString() + "~w~ to interact with suspect.");


                        if (Game.IsKeyDown(Settings.Dialog))
                        {
                            counter++;

                            try
                            {
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
                            catch (Exception ex)
                            {
                                Game.LogTrivial($"Error in {nameof(Process)}: {ex.Message}");
                                LoggingManager.Log("Adam69 Callouts [ERROR]: " + LogLevel.Error);
                            }
                        }
                    }
                }

                if (MainPlayer.IsDead || Game.IsKeyDown(Settings.EndCall))
                {
                    bool missionMessages = Settings.MissionMessages;
                    if (missionMessages == true)
                    {
                        BigMessageThread bigMessage = new BigMessageThread();
                        bigMessage.MessageInstance.ShowColoredShard("MISSION FAILED!", "You'll get 'em next time!", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);
                    }
                    else
                    {
                        return;
                    }
                    
                    End();
                }
            }
        }

        public override void End()
        {
            if (suspect) suspect.Dismiss();
            if (susBlip) susBlip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Loitering", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
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

            Game.LogTrivial("Adam69 Callouts [LOG]: Loitering callout is Code 4!");
        }
    }
}