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
            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
        }

        static void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
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
                    Game.Console.Print();
                    Game.Console.Print("=============================================== Adam69 Callouts by OfficerMorrison ================================================");
                    Game.Console.Print();


                    Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "Adam69 Callouts", "~g~v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ~g~by ~o~OfficerMorrison", "~b~successfully loaded!");


                    if (Settings.HelpMessages == true)
                    {
                        Game.DisplayHelp("You can enable/disable Help Messages in Adam69Callouts.ini at anytime.");
                    }
                    else
                    {
                        Settings.HelpMessages = false;
                    }

                    VersionChecker.PluginCheck.IsUpdateAvailable();

                    GameFiber.Wait(300);
                });
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
                Game.LogTrivial("User do NOT have CalloutInterface installed. Stopping integration....");
                CalloutInterface = false;
            }
            if (Functions.GetAllUserPlugins().ToList().Any(a => a != null && a.FullName.Contains("StopThePed")) == true)
            {
                Game.LogTrivial("User has StopThePed by Bejoijo Plugins installed. Starting integration.....");
                StopThePed = true;
            }
            else
            {
                Game.LogTrivial("User doe not have Stop The Ped by Bejoijo Plugins installed. Stopping integration....");
                StopThePed = false;
            }
            if (Functions.GetAllUserPlugins().ToList().Any(a => a != null && a.FullName.Contains("UltimateBackup")) == true)
            {
                Game.LogTrivial("User has Ultimate Backup by Bejoijo Plugins installed. Starting integration....");
                UltimateBackup = true;
            }
            else
            {
                Game.LogTrivial("User does not have Ultimate Backup by Bejoijo Plugins installed. Stopping integration....");
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
            Game.Console.Print("[LOG]: All callouts of the Adam69Callouts.ini were loaded successfully.");
            Game.Console.Print();
            Game.Console.Print("================================================== Adam69 Callouts ===================================================");
            Game.Console.Print();
        }

        public override void Finally() 
        {
            Game.Console.Print("[Adam69Callouts LOG]: Adam69Callouts Sucessfully unloaded and player has gone off duty for the day.");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "Adam69Callouts", "by ~o~OfficerMorrison", "Successfully unloaded.");
        }

        // Credits to SuperPyroManiac: https://www.lcpdfr.com/forums/topic/147923-psa-to-devs-you-do-not-need-ini-parser-with-rph/#comment-797340
        private static void Run()
        {
            Settings.LoadSettings();
        }
    }
}
