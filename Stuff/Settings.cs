using System.Windows.Forms;
using System.Xml;

namespace Adam69Callouts
{
    internal static class Settings
    {
        internal static bool LostDog = true;
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
        internal static bool HelpMessages = true;
        internal static bool WarningMessages = true;
        internal static Keys EndCall = Keys.End;
        internal static Keys Dialog = Keys.Y;
        internal static Keys PickUp = Keys.E;
        internal static InitializationFile ini;
        internal static string inipath = "Plugins/LSPDFR/Adam69Callouts.ini";


        internal static void LoadSettings()
        {
            Game.Console.Print("[LOG]: Loading config file from Adam69 Callouts");
            var path = "Plugins/LSPDFR/Adam69Callouts.ini";
            var ini = new InitializationFile(path);
            ini.Create();
            Game.LogTrivial("Initializing config for Adam69 Callouts....");
            Settings.LostDog = ini.ReadBoolean("Callouts", "LostDog", true);
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
            Settings.HelpMessages = ini.ReadBoolean("Help Messages", "HelpMessages", true);
            Settings.WarningMessages = ini.ReadBoolean("Misc", "WarningMessages", true);
            EndCall = ini.ReadEnum<Keys>("Keys", "EndCall", Keys.End);
            Dialog = ini.ReadEnum<Keys>("Keys", "Dialog", Keys.Y);
            PickUp = ini.ReadEnum<Keys>("Keys", "PickUp", Keys.E);
        }

        public static readonly string PluginVersion = "0.1.2";
    }
}
