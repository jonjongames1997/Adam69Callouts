using CalloutInterfaceAPI;
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

        private const string CalloutMessageText = "An individual loitering";
        private const string DispatchMessage = "~b~Dispatch~w~: Suspect has been spotted. Respond ~y~Code 2~w~.";
        private const string ScannerAudio = "Adam69Callouts_Loitering_Audio";
        private const string RespondAudio = "Adam69Callouts_Respond_Code_2_Audio";
        private const string Code4Audio = "Adam69Callouts_Code_4_Audio";

        public override bool OnBeforeCalloutDisplayed()
        {
            try
            {
                spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(100f));
                ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio(ScannerAudio);
                CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of loitering");
                CalloutMessage = CalloutMessageText;
                CalloutPosition = spawnpoint;

                return base.OnBeforeCalloutDisplayed();
            }
            catch (Exception ex)
            {
                Game.LogTrivial($"Error in {nameof(OnBeforeCalloutDisplayed)}: {ex.Message}");
                return false;
            }
        }

        public override bool OnCalloutAccepted()
        {
            try
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Loitering callout has been accepted!");
                Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Loitering", DispatchMessage);

                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio(RespondAudio);

                suspect = new Ped(spawnpoint)
                {
                    IsPersistent = true,
                    BlockPermanentEvents = true
                };

                if (!suspect.IsValid())
                {
                    End();
                    return false;
                }

                suspect.Tasks.PlayAnimation(new AnimationDictionary("timetable@tracy@ig_5@idle_a"), "idle_a", -1f, AnimationFlags.Loop);
                suspect.KeepTasks = true;

                susBlip = suspect.AttachBlip();
                susBlip.Color = System.Drawing.Color.Pink;
                susBlip.IsRouteEnabled = true;

                malefemale = suspect.IsMale ? "Sir" : "Ma'am";
                counter = 0;

                return base.OnCalloutAccepted();
            }
            catch (Exception ex)
            {
                Game.LogTrivial($"Error in {nameof(OnCalloutAccepted)}: {ex.Message}");
                End();
                return false;
            }
        }

        public override void OnCalloutNotAccepted()
        {
            susBlip?.Delete();
            suspect?.Delete();

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
                        missionMessages = false;
                        Game.LogTrivial("Adam69 Callouts [LOG]: Mission messages are disabled in the config file.");
                        return;
                    }
                    
                    End();
                }
            }
        }


        private void PlayAnimation(string dictionary, string animation)
        {
            suspect.Tasks.PlayAnimation(new AnimationDictionary(dictionary), animation, -1f, AnimationFlags.Loop);
        }

        public override void End()
        {
            suspect?.Dismiss();
            susBlip?.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Loitering", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio(Code4Audio);

            bool missionMessages = Settings.MissionMessages;
            if (missionMessages == true)
            {
                BigMessageThread bigMessage = new BigMessageThread();

                bigMessage.MessageInstance.ShowColoredShard("Callout Completed!", "You are now ~g~CODE 4~w~.", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);
            }
            else
            {
                missionMessages = false;
                Game.LogTrivial("Adam69 Callouts [LOG]: Mission messages are disabled in the config file.");
                return;
            }

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Loitering callout is Code 4!");
        }
    }
}