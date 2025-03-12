using System.Reflection;
using Adam69Callouts.Callouts;
using Adam69Callouts.VersionChecker;
using LSPD_First_Response.Mod.Utils;

[assembly: Rage.Attributes.Plugin("Adam69Callouts", Description = "LSPDFR Callout Pack", Author = "OfficerMorrison")]
namespace Adam69Callouts
{
    public class Main : Plugin
    {
        public static bool CalloutInterface;
        public static bool StopThePed;
        public static bool UltimateBackup;

        public override void Initialize()
        {
            try
            {
                Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
                Settings.LoadSettings();
            }
            catch(Exception ex)
            {
                Game.LogTrivial("Adam69Callouts [ERROR]: Failed to initialize the plugin: " + ex.Message);
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
                    Game.Console.Print("=============================================== Adam69 Callouts by OfficerMorrison ================================================");
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
                    Game.Console.Print("=============================================== Adam69 Callouts by OfficerMorrison ================================================");
                    Game.Console.Print();


                    Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "Adam69 Callouts", "~g~v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ~g~by ~o~OfficerMorrison", "~b~successfully loaded!");
                    Game.Console.Print();
                    Game.Console.Print();
                    Game.Console.Print("============================================== Adam69 Callouts by OfficerMorrison ===================================================");
                    Game.Console.Print();
                    Game.Console.Print();
                    Game.Console.Print();
                    Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Mission Started", "Survive your shft.");
                    Game.Console.Print();
                    Game.Console.Print();
                    Game.Console.Print("[LOG] Adam69Callouts: Mission notification enabled");
                    Game.Console.Print();
                    Game.Console.Print("============================================== Adam69 Callouts by OfficerMorrison ===================================================");
                    Game.Console.Print();
                    Game.Console.Print();

                    bool helpMessages = Settings.HelpMessages;
                    if (helpMessages)
                    {
                        Game.DisplayHelp("You can change all ~y~keys~w~ in the ~o~Adam69Callouts.ini~w~. Press ~p~" + Settings.EndCall.ToString() + "~w~ to end a callout.", 5000);
                    }

                    VersionChecker.PluginCheck.IsUpdateAvailable();

                    GameFiber.Wait(300);
                });
            }
        }

        private static void RegisterCallouts()
        {
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
            if (Functions.GetAllUserPlugins().ToList().Any(a => a != null && a.FullName.Contains("StopThePed")) == true)
            {
                Game.LogTrivial("User has Stop The Ped 4.9.5.2 by Bejoijo INSTALLED. starting integration.......");
                StopThePed = true;
            }
            else
            {
                Game.LogTrivial("User does NOT have Stop The Ped installed. Stopping integration....");
                StopThePed = false;
            }
            if (Functions.GetAllUserPlugins().ToList().Any(a => a != null && a.FullName.Contains("UltimateBackup")) == true)
            {
                Game.LogTrivial("User has Ultimate Backup1.8.7.0 by Bejoijo INSTALLED. starting integration.......");
                UltimateBackup = true;
            }
            else
            {
                Game.LogTrivial("User does NOT have Ultimate Backup installed. Stopping integration....");
                UltimateBackup = false;
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
            if (Settings.DrugsFound) { Functions.RegisterCallout(typeof(DrugsFound)); }
            if (Settings.SuspiciousPerson) { Functions.RegisterCallout(typeof(SuspiciousPerson)); }
            if (Settings.OfficerDown) { Functions.RegisterCallout(typeof(OfficerDown)); }
            if (Settings.DerangedDrunkenFeller) { Functions.RegisterCallout(typeof(DerangedDrunkenFeller)); }
            Game.Console.Print("[LOG]: All callouts of the Adam69Callouts.ini were loaded successfully.");
            Game.Console.Print();
            Game.Console.Print("================================================== Adam69 Callouts ===================================================");
            Game.Console.Print();
        }

        public override void Finally() 
        {
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "Adam69 Callouts", "~w~Mission Passed", "Survive your shift.");
            Game.Console.Print("[LOG] Adam69 Callouts: Mission Complete!");
            Game.Console.Print();
        }
    }
}
