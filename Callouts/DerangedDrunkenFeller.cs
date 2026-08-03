using CalloutInterfaceAPI;
using Adam69Callouts.Common;
using RAGENativeUI;
using Rage;

namespace Adam69Callouts.Callouts
{
    [CalloutInterface("[Adam69 Callouts] - Deranged Drunken Feller", CalloutProbability.Medium, "Reports of a drunken feller", "Code 2", "LSPD")]
    public class DerangedDrunkenFeller : Callout
    {
        private Ped suspect;
        private Vector3 spawnpoint;
        private Blip blip;
        private int counter;
        private string malefemale;
        private static readonly string[] wepList = new string[] { "weapon_pistol", "weapon_combatmg", "weapon_combatpistol" };
        private static readonly Random random = new Random();

        private enum DrunkScenario
        {
            Aggressive,
            Compliant,
            Flees,
            PassesOut
        }

        private DrunkScenario scenario;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(500f));
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "A Deranged Drunken Feller has been reported in the area. Respond Code 2.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("SUSPICIOUS_PERSON_02", spawnpoint);
            CalloutMessage = "Deranged Drunken Feller Reported";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            if (Settings.EnableLogs)
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Deranged Drunken Feller callout has been accepted!");
            }

            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "Deranged Drunken Feller", "~b~Dispatch~w~: Suspect has been located. Respond ~r~Code 2~w~.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_2_Audio");

            scenario = (DrunkScenario)random.Next(Enum.GetValues(typeof(DrunkScenario)).Length);

            suspect = new Ped(spawnpoint)
            {
                IsPersistent = true,
                BlockPermanentEvents = true
            };

            suspect.Tasks.PlayAnimation(new AnimationDictionary("random@drunk_driver_1"), "drunk_driver_stand_loop_dd2", -1f, AnimationFlags.Loop);

            blip = suspect.AttachBlip();
            blip.Color = System.Drawing.Color.Red;
            blip.IsRouteEnabled = true;

            if (suspect.IsMale)
                malefemale = "Sir";
            else
                malefemale = "Ma'am";

            counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (suspect != null && suspect.Exists()) suspect.Delete();
            if (blip != null && blip.Exists()) blip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if (MainPlayer.DistanceTo(suspect) <= 10f)
            {
                if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                {
                    counter++;
                    try
                    {
                        HandleInteraction();
                    }
                    catch (Exception ex)
                    {
                        if (Settings.EnableLogs)
                        {
                            Game.LogTrivial("Adam69 Callouts [LOG]: Exception in Deranged Drunken Feller callout: " + ex.Message);
                            Game.LogTrivial("Adam69 Callouts [LOG]: Exception in Deranged Drunken Feller callout: " + ex.StackTrace);
                            LoggingManager.Log("Adam69 Callouts [LOG]: Exception in Deranged Drunken Feller callout: " + ex.Message);
                            LoggingManager.Log("Adam69 Callouts [LOG]: Exception in Deranged Drunken Feller callout: " + ex.StackTrace);
                        }
                        else
                        {
                            Settings.EnableLogs = false;
                        }
                    }
                }
            }

            if (MainPlayer.IsDead)
            {
                if (Settings.MissionMessages)
                {
                    BigMessageThread bigMessage = new BigMessageThread();
                    bigMessage.MessageInstance.ShowColoredShard("MISSION FAILED!", "You'll get 'em next time!", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);
                }
                else
                {
                    Settings.MissionMessages = false;
                    Game.LogTrivial("[LOG]: Mission messages are disabled in the config file.");
                }

                End();
            }

            if (Game.IsKeyDown(System.Windows.Forms.Keys.End))
            {
                if (Settings.MissionMessages)
                {
                    BigMessageThread bigMessage = new BigMessageThread();
                    bigMessage.MessageInstance.ShowColoredShard("Callout Complete!", "Get back out there and protect the citizens, officer", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);
                }
                else
                {
                    Settings.MissionMessages = false;
                    Game.LogTrivial("[LOG]: Mission messages are disabled in the config file.");
                    LoggingManager.Log("Adam69 Callouts [LOG]: Mission messages are disabled in the config file.");
                }

                End();
            }

            base.Process();
        }

        private void HandleInteraction()
        {
            switch (scenario)
            {
                case DrunkScenario.Aggressive:
                    HandleAggressiveScenario();
                    break;
                case DrunkScenario.Compliant:
                    HandleCompliantScenario();
                    break;
                case DrunkScenario.Flees:
                    HandleFleesScenario();
                    break;
                case DrunkScenario.PassesOut:
                    HandlePassesOutScenario();
                    break;
            }
        }

        // Scenario 1: Suspect argues and then turns violent
        private void HandleAggressiveScenario()
        {
            switch (counter)
            {
                case 1:
                    NativeFunction.Natives.TASK_TURN_PED_TO_FACE_ENTITY(suspect, MainPlayer, -1);
                    Game.DisplaySubtitle("~b~You~w~: What goin' on, feller? Have anything to drink today?");
                    break;
                case 2:
                    suspect.Tasks.PlayAnimation(new AnimationDictionary("random@drunk_driver_1"), "drunk_driver_stand_loop_dd2", -1f, AnimationFlags.Loop);
                    Game.DisplaySubtitle("~r~Suspect~w~: *slurring* What you want, officer pigfucker?");
                    break;
                case 3:
                    suspect.Tasks.PlayAnimation(new AnimationDictionary("amb@world_human_stand_impatient@female@no_sign@base"), "base", -1f, AnimationFlags.Loop);
                    Game.DisplaySubtitle($"~b~You~w~: {malefemale}, I'm going to need you to calm down.");
                    break;
                case 4:
                    suspect.Tasks.PlayAnimation(new AnimationDictionary("random@drunk_driver_1"), "drunk_driver_stand_loop_dd2", -1f, AnimationFlags.Loop);
                    Game.DisplaySubtitle("~r~Suspect~w~: *slurring* You got to catch me first, donut eater!");
                    break;
                case 5:
                    Game.DisplaySubtitle("~y~Suspect is hostile! Chase and arrest!");
                    suspect.Tasks.FightAgainst(MainPlayer);
                    suspect.Armor = 500;
                    if (suspect != null && suspect.Exists() && suspect.IsValid())
                        SafeInventory.SafeGiveWeapon(suspect, wepList[random.Next(wepList.Length)], 500, true);
                    break;
            }
        }

        // Scenario 2: Suspect is too drunk to resist and cooperates
        private void HandleCompliantScenario()
        {
            switch (counter)
            {
                case 1:
                    NativeFunction.Natives.TASK_TURN_PED_TO_FACE_ENTITY(suspect, MainPlayer, -1);
                    Game.DisplaySubtitle("~b~You~w~: Hey there, have you been drinking today?");
                    break;
                case 2:
                    suspect.Tasks.PlayAnimation(new AnimationDictionary("random@drunk_driver_1"), "drunk_driver_stand_loop_dd2", -1f, AnimationFlags.Loop);
                    Game.DisplaySubtitle("~r~Suspect~w~: *slurring* ...maaaybe jus' a lil' bit, ossifer...");
                    break;
                case 3:
                    Game.DisplaySubtitle($"~b~You~w~: {malefemale}, I'm going to need you to put your hands behind your back.");
                    break;
                case 4:
                    suspect.Tasks.PlayAnimation(new AnimationDictionary("amb@world_human_stand_impatient@female@no_sign@base"), "base", -1f, AnimationFlags.Loop);
                    Game.DisplaySubtitle("~r~Suspect~w~: *slurring* Okay, okay... don't hurt me...");
                    break;
                case 5:
                    Game.DisplaySubtitle("~g~Suspect is compliant. Detain and transport.");
                    LSPD_First_Response.Mod.API.Functions.IsPedArrested(suspect);
                    break;
            }
        }

        // Scenario 3: Suspect panics and flees on contact
        private void HandleFleesScenario()
        {
            switch (counter)
            {
                case 1:
                    NativeFunction.Natives.TASK_TURN_PED_TO_FACE_ENTITY(suspect, MainPlayer, -1);
                    Game.DisplaySubtitle("~b~You~w~: Hey! Stop right there!");
                    break;
                case 2:
                    suspect.Tasks.PlayAnimation(new AnimationDictionary("random@drunk_driver_1"), "drunk_driver_stand_loop_dd2", -1f, AnimationFlags.Loop);
                    Game.DisplaySubtitle("~r~Suspect~w~: *slurring* Oh no no no no no...");
                    GameFiber.Wait(800);
                    suspect.Tasks.ReactAndFlee(MainPlayer);
                    blip.Color = System.Drawing.Color.Yellow;
                    break;
                case 3:
                    Game.DisplaySubtitle("~b~You~w~: Stop! Police! Get on the ground!");
                    suspect.Tasks.ReactAndFlee(MainPlayer);
                    break;
                case 4:
                    Game.DisplaySubtitle("~y~Suspect is fleeing! Pursue and apprehend!");
                    break;
            }
        }

        // Scenario 4: Suspect is too far gone and passes out
        private void HandlePassesOutScenario()
        {
            switch (counter)
            {
                case 1:
                    NativeFunction.Natives.TASK_TURN_PED_TO_FACE_ENTITY(suspect, MainPlayer, -1);
                    Game.DisplaySubtitle("~b~You~w~: Sir, are you alright? Have you been drinking?");
                    break;
                case 2:
                    suspect.Tasks.PlayAnimation(new AnimationDictionary("random@drunk_driver_1"), "drunk_driver_stand_loop_dd2", -1f, AnimationFlags.Loop);
                    Game.DisplaySubtitle("~r~Suspect~w~: *incoherent mumbling* ...tequilaaa...");
                    break;
                case 3:
                    Game.DisplaySubtitle("~b~You~w~: Dispatch, I've got a heavily intoxicated individual. Requesting EMS.");
                    LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("REQUESTING_ASSISTANCE_02");
                    break;
                case 4:
                    suspect.Tasks.PlayAnimation(new AnimationDictionary("missheistdockssetup1ig_14"), "base_idle_a", -1f, AnimationFlags.Loop);
                    Game.DisplaySubtitle("~r~Suspect~w~: *collapses onto the ground*");
                    NativeFunction.Natives.SET_PED_TO_RAGDOLL(suspect, 5000, 5000, 0, false, false, false);
                    break;
                case 5:
                    Game.DisplaySubtitle("~g~Suspect is down. Secure the scene and wait for EMS.");
                    LSPD_First_Response.Mod.API.Functions.IsPedArrested(suspect);
                    break;
            }
        }

        public override void End()
        {
            if (suspect != null && suspect.Exists()) suspect.Dismiss();
            if (blip != null && blip.Exists()) blip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Deranged Drunken Feller", "~b~You~w~: Dispatch, we are ~g~Code 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

            if (Settings.MissionMessages)
            {
                BigMessageThread bigMessage = new BigMessageThread();
                bigMessage.MessageInstance.ShowColoredShard("~g~Code 4", "Suspect Neutralized!", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);
            }
            else
            {
                Settings.MissionMessages = false;
                Game.LogTrivial("[LOG]: Mission messages are disabled in the config file.");
            }

            base.End();

            if (Settings.EnableLogs)
            {
                Game.LogTrivial("Adam69 Callouts [LOG]: Deranged Drunken Feller callout is CODE 4!");
                LoggingManager.Log("Adam69 Callouts [LOG]: Deranged Drunken Feller callout is CODE 4!");
            }
            else
            {
                Settings.EnableLogs = false;
            }
        }
    }
}