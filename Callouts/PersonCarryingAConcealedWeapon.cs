﻿using CalloutInterfaceAPI;

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

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(100f));
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            if (Settings.BluelineDispatch)
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("Adam69Callouts_PersonCarryingAConcealedWeapon_Audio", spawnpoint);
            }
            else
            {
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("CRIME_BRANDISHING_WEAPON_03", spawnpoint);
            }
            CalloutInterfaceAPI.Functions.SendMessage(this, "Citizen's reporting an individual with a gun.");
            CalloutMessage = "Person carrying a firearm";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Person Carrying A Concealed Weapon callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Person Carrying A Concealed Weapon", "~b~Dispatch~w~: Suspect has been spotted. Respond ~y~Code 2~w~.");


            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_2_Audio");

            suspect = new Ped(spawnpoint);
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;

            suspect.Tasks.StandStill(6000);

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
            base.Process();

            if (MainPlayer.DistanceTo(suspect) <= 10f)
            {

                if (Game.IsKeyDown(Settings.Dialog))
                {
                    counter++;
                    try
                    {
                        if (counter == 1)
                        {
                            NativeFunction.Natives.TASK_TURN_PED_TO_FACE_ENTITY(suspect, MainPlayer, -1);
                            Game.DisplaySubtitle("~b~You~w~: LSPD, freeze, mothertrucker!");
                        }
                        if (counter == 2)
                        {
                            suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                            Game.DisplaySubtitle("~r~Suspect~w~: What's going on, Officer?");
                        }
                        if (counter == 3)
                        {
                            Game.DisplaySubtitle("~b~You~w~: Don't reach for anything, " + malefemale + ".");
                        }
                        if (counter == 4)
                        {
                            suspect.Tasks.PlayAnimation(new AnimationDictionary("josh_1_int-11"), "cs_josh_dual-11", -1f, AnimationFlags.UpperBodyOnly);
                            Game.DisplaySubtitle("~r~Suspect~w~: What the f are you talking about? I don't have a weapon on me!");
                        }
                        if (counter == 5)
                        {
                            suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                            Game.DisplaySubtitle("~b~You~w~: I said, 'Don't reach for anything'. keep your hands where I can see them.");
                        }
                        if (counter == 6)
                        {
                            suspect.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_02", -1f, AnimationFlags.Loop);
                            Game.DisplaySubtitle("~r~Suspect~w~: And I said, 'I don't have a weapon on me', motherfucker! Are you deaf?");
                        }
                        if (counter == 7)
                        {
                            suspect.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                            Game.DisplaySubtitle("~b~You~w~: I will shoot if you reach for anything!");
                        }
                        if (counter == 8)
                        {
                            Game.DisplaySubtitle("~r~Suspect~w~: Screw this! Die, motherfucker, you!");
                            suspect.Tasks.FightAgainst(MainPlayer);
                            suspect.Inventory.GiveNewWeapon(wepList[new Random().Next((int)wepList.Length)], 500, true);
                            suspect.Armor = 2500;
                        }
                    }
                    catch (Exception ex)
                    {
                        Game.LogTrivial("Adam69 Callouts [LOG]: Error found in Person Carrying An Explosive Weapon: " + ex.Message);
                        Game.LogTrivial("Adam69 Callouts [LOG]: Error found in Person Carrying An Explosive Weapon: " + ex.StackTrace);
                        LoggingManager.Log("Adam69 Callouts [LOG] " + LogLevel.Error);
                        LoggingManager.Log("Adam69 Callouts [LOG] " + LogLevel.Info);
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

        public override void End()
        {
            if (susBlip) susBlip.Delete();
            if (suspect) suspect.Dismiss();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Person Carrying A Concealed Weapon", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
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

            Game.LogTrivial("Adam69 Callouts [LOG]: Person Carrying A Concealed Weapon callout is Code 4!");
        }
    }
}
