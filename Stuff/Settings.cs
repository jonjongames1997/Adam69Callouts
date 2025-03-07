using System.Windows.Forms;
using System.Xml;
using Rage;

namespace Adam69Callouts
{
    internal static class Settings
    {
        internal static bool VehicleBlockingSidewalk = true;
        internal static bool BicyclePursuit = true;
        internal static bool PersonCarryingAConcealedWeapon = true;
        internal static bool Loitering = true;
        internal static bool VehicleBlockingCrosswalk = true;
        internal static bool BicycleBlockingRoadway = true;
        internal static bool SuspiciousVehicle = true;
        internal static bool AbandonedVehicle = true;
        internal static bool DrugsFound = true;
        internal static bool SuspiciousPerson = true;
        internal static bool OfficerDown = true;
        internal static bool DerangedDrunkenFeller = true;
        internal static bool HelpMessages = true;
        internal static Keys EndCall = Keys.End;
        internal static Keys Dialog = Keys.Y;
        internal static Keys PickUp = Keys.E;


        internal static void LoadSettings()
        {
            Game.Console.Print("[LOG]: Loading config file from Adam69 Callouts");
            var ini = new InitializationFile(@"Plugins/LSPDFR/Adam69Callouts.ini");
            ini.Create();
            Game.LogTrivial("Initializing config for Adam69 Callouts....");
            Settings.VehicleBlockingSidewalk = ini.ReadBoolean("Callouts", "VehicleBlockingSidewalk", true);
            Settings.BicyclePursuit = ini.ReadBoolean("Callouts", "BicyclePursuit", true);
            Settings.PersonCarryingAConcealedWeapon = ini.ReadBoolean("Callouts", "PersonCarryingAConcealedWeapon", true);
            Settings.Loitering = ini.ReadBoolean("Callouts", "Loitering", true);
            Settings.VehicleBlockingCrosswalk = ini.ReadBoolean("Callouts", "VehicleBlockingCrosswalk", true);
            Settings.BicycleBlockingRoadway = ini.ReadBoolean("Callouts", "BicycleBlockingRoadway", true);
            Settings.SuspiciousVehicle = ini.ReadBoolean("Callouts", "SuspiciousVehicle", true);
            Settings.AbandonedVehicle = ini.ReadBoolean("Callouts", "AbandonedVehicle", true);
            Settings.DrugsFound = ini.ReadBoolean("Callouts", "DrugsFound", true);
            Settings.SuspiciousPerson = ini.ReadBoolean("Callouts", "SuspiciousPerson", true);
            Settings.OfficerDown = ini.ReadBoolean("Callouts", "OfficerDown", true);
            Settings.DerangedDrunkenFeller = ini.ReadBoolean("Callouts", "DerangedDrunkenFeller", true);
            Settings.HelpMessages = ini.ReadBoolean("Settings", "HelpMessages", true);
            EndCall = ini.ReadEnum<Keys>("Keys", "EndCall", Keys.End);
            Dialog = ini.ReadEnum<Keys>("Keys", "Dialog", Keys.Y);
            PickUp = ini.ReadEnum<Keys>("Keys", "PickUp", Keys.E);
        }

        // Credits to SuperPyroManiac: https://www.lcpdfr.com/forums/topic/147923-psa-to-devs-you-do-not-need-ini-parser-with-rph/#comment-797340
        internal static void SaveSettings()
        {
            var ini = new InitializationFile(@"Plugins/LSPDFR/Adam69Callouts.ini");
            ini.Write("Callouts", "VehicleBlockingSidewalk", true);
            ini.Write("Callouts", "BicyclePursuit", true);
            ini.Write("Callouts", "PersonCarryingAConcealedWeapon", true);
            ini.Write("Callouts", "Loitering", true);
            ini.Write("Callouts", "VehicleBlockingCrosswalk", true);
            ini.Write("Callouts", "SuspiciousVehicle", true);
            ini.Write("Callouts", "AbandonedVehicle", true);
            ini.Write("Callouts", "DrugsFound", true);
            ini.Write("Callouts", "SuspiciousPerson", true);
            ini.Write("Callouts", "OfficerDown", true);
            ini.Write("Callouts", "DerangedDrunkenFeller", true);
            ini.Write("Settings", "HelpMessages", true);
            ini.Write("Keys", "EndCall", Keys.End);
            ini.Write("Keys", "Dialog", Keys.Y);
            ini.Write("Keys", "PickUp", Keys.E);
        }

        public static readonly string PluginVersion = "0.2.8.1";
    }
}