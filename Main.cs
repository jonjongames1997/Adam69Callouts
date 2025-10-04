﻿[assembly: Rage.Attributes.Plugin("Adam69Callouts", Description = "LSPDFR Callout Pack", Author = "JM Modifications")]
namespace Adam69Callouts
{
    public class Main : Plugin
    {
        public static bool CalloutInterface;
        public static bool PolicingRedefined;

        public static bool IsDlcInstalled(string dlcName)
        {
            uint dlcHash = Game.GetHashKey(dlcName);
            return NativeFunction.CallByName<bool>("IS_DLC_PRESENT", dlcHash);
        }

        public override void Initialize()
        {

                try
                {
                    Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
                    Game.AddConsoleCommands();
                    Settings.LoadSettings();
                    if (Settings.EnableLogs)
                    {
                        LoggingManager.Log("Adam69 Callouts: Plugin initialized successfully.");
                    }
                    else
                    {
                        Settings.EnableLogs = false;
                    }
                }
                catch (Exception ex)
                {
                    if (Settings.EnableLogs)
                    {
                        Game.LogTrivial("Adam69Callouts [ERROR]: Failed to initialize the plugin: " + ex.Message);
                        LoggingManager.Log("Adam69 Callouts: Failed to initialize the plugin: " + ex.Message);
                    }
                    else
                    {
                        Settings.EnableLogs = false;
                    }
                }
        }

        private static void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            bool flag = !onDuty;
            if (!flag)
            {
                GameFiber.StartNew(delegate
                {
                    RegisterCallouts();
                    Game.Console.Print();
                    Game.Console.Print("=============================================== Adam69 Callouts by JM Modifications ================================================");
                    Game.Console.Print();
                    Game.Console.Print("[LOG]: Callouts and settings were loaded successfully.");
                    Game.Console.Print("[LOG]: The config file was loaded successfully.");
                    Game.Console.Print("[VERSION]: Detected Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    Game.Console.Print("[LOG]: Checking for a new Adam69 Callouts version...");
                    Game.Console.Print();
                    Game.Console.Print();
                    Game.Console.Print("For support, Join the official Jon Jon Games Discord: https://discord.gg/N9KgZx4KUn");
                    Game.Console.Print();
                    Game.Console.Print();
                    Game.Console.Print("If you're banned from the Discord and want to appeal, file an appeal here: https://appeal.gg/N9KgZx4KUn7");
                    Game.Console.Print();
                    Game.Console.Print("=============================================== Adam69 Callouts by JM Modificationsn ================================================");
                    Game.Console.Print();


                    Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "Adam69 Callouts", "~g~v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ~g~by ~o~JM Modifications", "~b~successfully loaded!");
                    Game.Console.Print();
                    Game.Console.Print();
                    Game.Console.Print("============================================== Adam69 Callouts by JM Modifications ===================================================");
                    Game.Console.Print();
                    Game.Console.Print();
                    Game.Console.Print();
                    Game.Console.Print();
                    Game.Console.Print();
                    Game.Console.Print("");
                    Game.Console.Print();
                    Game.Console.Print("============================================== Adam69 Callouts by JM Modifications ===================================================");
                    Game.Console.Print();
                    Game.Console.Print();

                    if (Settings.HelpMessages)
                    {
                        Game.DisplayHelp("You can change all ~y~keys~w~ in the ~o~Adam69Callouts.ini~w~. Press ~p~" + Settings.EndCall.ToString() + "~w~ to end a callout or use console command 'endcurrentadam69calloutscallout' to end the callout." +
                           "Press ~y~" + Settings.Dialog.ToString() + "~w~ to talk to the suspect/person of interest.", 5000);
                    }
                    else
                    {
                        Settings.HelpMessages = false;
                        if (Settings.EnableLogs)
                        {
                            Game.LogTrivial("[LOG]: Help messages are disabled in the config file.");
                            LoggingManager.Log("Adam69 Callouts: Help messages are disabled in the config file.");
                        }
                        else
                        {
                            Settings.EnableLogs = false;
                        }
                        return;
                    }

                    if (Settings.MissionMessages)
                    {
                        BigMessageThread bigMessage = new BigMessageThread();

                        bigMessage.MessageInstance.ShowColoredShard("Mission Started!", "Survive Your Shift!", RAGENativeUI.HudColor.Yellow, RAGENativeUI.HudColor.Black, 5000);
                    }
                    else
                    {
                        Settings.MissionMessages = false;
                        return;
                    }

                    VersionChecker.PluginCheck.IsUpdateAvailableAsync().GetAwaiter().GetResult();

                    GameFiber.Wait(300);
                });
            }
        }

        private static void RegisterCallouts()
        {
            int registered = 0;

            if (DLCManager.IsDLCInstalled("mp2025_01"))
            {
                if (Settings.DrugsFound) { Functions.RegisterCallout(typeof(DrugsFound)); }
                registered++;
            }

            if (Functions.GetAllUserPlugins().ToList().Any(a => a != null && a.FullName.Contains("CalloutInterface")) == true)
            {
                Game.LogTrivial("User has Callout Interface 1.4.1 by Opus INSTALLED. starting integration.......");
                CalloutInterface = true;
            }
            else
            {
                Game.LogTrivial("User does NOT have CalloutInterface installed. Stopping integration....");
                CalloutInterface = false;
            }
            if (Functions.GetAllUserPlugins().ToList().Any(a => a != null && a.FullName.Contains("PolicingRedefined")) == true)
            {
                Game.LogTrivial("User has Policing Redefined 1.0.0.0 by MarcelWRLD INSTALLED. starting integration.......");
                PolicingRedefined = true;
            }
            else
            {
                Game.LogTrivial("User does NOT have Policing Redefined 1.0.0.0 by MarcelWRLD installed. Stopping integration....");
                PolicingRedefined = false;
            }

            Game.Console.Print();
            Game.Console.Print();
            Game.Console.Print("================================================== Adam69 Callouts ===================================================");
            Game.Console.Print();
            if (Settings.VehicleBlockingSidewalk) { Functions.RegisterCallout(typeof(VehicleBlockingSidewalk)); }
            if (Settings.BicyclePursuit) { Functions.RegisterCallout(typeof(BicyclePursuit)); }
            if (Settings.PersonCarryingAConcealedWeapon) { Functions.RegisterCallout(typeof(PersonCarryingAConcealedWeapon)); }
            if (Settings.Loitering) { Functions.RegisterCallout(typeof(Loitering)); }
            if (Settings.VehicleBlockingCrosswalk) { Functions.RegisterCallout(typeof(VehicleBlockingCrosswalk)); }
            if (Settings.BicycleBlockingRoadway) { Functions.RegisterCallout(typeof(BicycleBlockingRoadway)); }
            if (Settings.SuspiciousVehicle) { Functions.RegisterCallout(typeof(SuspiciousVehicle)); }
            if (Settings.AbandonedVehicle) { Functions.RegisterCallout(typeof(AbandonedVehicle)); }
            if (Settings.SuspiciousPerson) { Functions.RegisterCallout(typeof(SuspiciousPerson)); }
            if (Settings.OfficerDown) { Functions.RegisterCallout(typeof(OfficerDown)); }
            if (Settings.DerangedDrunkenFeller) { Functions.RegisterCallout(typeof(DerangedDrunkenFeller)); }
            if (Settings.DeadBirdOnTheRoad) { Functions.RegisterCallout(typeof(DeadBirdOnTheRoad)); }
            if (Settings.KnifeAttack) { Functions.RegisterCallout(typeof(KnifeAttack)); }
            Game.Console.Print("[LOG]: All callouts of the Adam69Callouts.ini were loaded successfully.");
            Game.Console.Print();
            Game.Console.Print("================================================== Adam69 Callouts ===================================================");
            Game.Console.Print();
        }

        public override void Finally()
        {
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "Adam69 Callouts", "~w~by JM Modifications", "Enjoy your night, Officer.");
            Game.Console.Print("[LOG] Adam69 Callouts: Mission Complete!");
            Game.Console.Print();
            if (Settings.EnableLogs)
            {
                LoggingManager.Log("Adam69 Callouts: Plugin unloaded successfully.");
            }
            else
            {
                Settings.EnableLogs = false;
            }

            if (Settings.MissionMessages)
            {
                BigMessageThread bigMessage = new BigMessageThread();

                bigMessage.MessageInstance.ShowColoredShard("Mission Passed!", "Survive Your Shift", RAGENativeUI.HudColor.Yellow, RAGENativeUI.HudColor.Black, 5000);
            }
            else
            {
                if (Settings.EnableLogs)
                {
                    Game.LogTrivial("[LOG]: Mission messages are disabled in the config file.");
                    LoggingManager.Log("Adam69 Callouts: Mission messages are disabled in the config file.");
                }
                else
                {
                    Settings.EnableLogs = false;
                }
                Settings.MissionMessages = false;
                return;
            }
        }

        private static void LoadConfig()
        {
            Settings.LoadSettings();
            if (Settings.EnableLogs)
            {
                LoggingManager.Log("Adam69 Callouts: Config file loaded successfully.");
            }
            else
            {
                Settings.EnableLogs = false;
            }
        }
    }
}
