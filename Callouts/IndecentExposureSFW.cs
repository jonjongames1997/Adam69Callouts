using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] Attempted Indecent Exposure (SFW)", CalloutProbability.Medium, "Reports of an individual trying exposing themself", "CODE 2", "LSPD")]

    public class IndecentExposureSFW : Callout
    {
        private static Ped suspect;
        private static Vector3 spawnpoint;
        private static Blip susBlip;
        private static int counter;
        private static string malefemale;


        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(1000f));
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            AddMinimumDistanceCheck(500f, spawnpoint);
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS_01 WE_HAVE_01 CRIME_DISTURBING_THE_PEACE_01 UNITS_RESPOND_CODE_02_02", spawnpoint);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Multiple reports of an individual trying to expose themself");
            CalloutMessage = "An individual attempting to expose themself to the public.";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Indecent Exposure SFW callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Indecent Exposure SFW", "~b~Dispatch~w~: Suspect has been spotted. Respond ~y~Code 2~w~.");
            Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout", false);

            Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~Adam69 Callouts", "~r~WARNING!", "This callout MAY contain adult humor/jokes. Player Discretion is advised! If you're under 18, ~y~END~w~ this callout immediately.");

            suspect = new Ped(spawnpoint);
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;
            suspect.IsValid();

            suspect.Tasks.PlayAnimation(new AnimationDictionary("anim@mp_player_intcelebrationfemale@air_shagging"), "air_shagging", -1f, AnimationFlags.Loop);
            
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
            if (susBlip) susBlip.Delete();
            if (suspect) suspect.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            base.Process();


            if(MainPlayer.DistanceTo(suspect) <= 10f)
            {
                Game.DisplayHelp("Press ~y~Y~w~ to interact with suspect.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                {
                    counter++;

                    if(counter == 1)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~b~You~w~: Excuse me, " + malefemale + ". LSPD, come talk to me for a minute.");
                    }
                    if(counter == 2)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~r~Suspect~w~: What did I do, officer?");
                    }
                    if(counter == 3)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~b~You~w~: I've gotten reports of you trying to expose yourself to the public. That's against the law.");
                    }
                    if(counter == 4)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~r~Suspect~w~: What? I thought it was National Nude Day. Did I get the date wrong?");
                    }
                    if(counter == 5)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~b~You~w~: What you mean 'Did I get the date wrong'? The state of San Andreas does not recognize 'National Nude Day'.");
                    }
                    if(counter == 6)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~r~Suspect~w~: Bruh! Why?! Being nude on '~y~National Nude Day~w~' improves skin, heart health, and confidence. That's just pure bullshit. They should recognize that."); ;
                    }
                    if(counter == 7)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~b~You~w~: Sorry, it's the state law.");
                    }
                    if(counter == 8)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~r~Suspect~w~: Why are y'all being a Karen/Darren?");
                    }
                    if(counter == 9)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplayNotification("You got called a Karen/Darren. 😂");
                    }
                    if(counter == 10)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~b~You~w~: I'm not gonna argue with you on this. I'm gonna have to give you a citation.");
                    }
                    if(counter == 11)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~r~Suspect~w~: FOR WHAT?!");
                    }
                    if(counter == 12)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~b~You~w~: For trying to take your clothes off in public, which is considered indecent exposure. A citation is better then going to jail.");
                    }
                    if(counter == 13)
                    {
                        suspect.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~r~Suspect~w~: Whatever, Karen/Darren.");
                    }
                    if(counter == 14)
                    {
                        Game.DisplaySubtitle("Conversation Ended! Deal the situation you may see fit.");
                        suspect.Tasks.ReactAndFlee(suspect);
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
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Indecent Exposure (SFW)", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURHTER_UNITS_REQUIRED");

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Indecent Exposure (SFW) callout is Code 4!");
        }
    }
}