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
        internal static string Language = "English";


        internal static void LoadSettings()
        {
            Game.Console.Print("[LOG]: Loading config file from Adam69 Callouts");
            InitializationFile initializationFile = new InitializationFile("Plugins/LSPDFR/Adam69Callouts.ini");
            initializationFile.Create();
            Game.LogTrivial("Initializing config for Adam69 Callouts....");
            Settings.VehicleBlockingSidewalk = initializationFile.ReadBoolean("Callouts", "VehicleBlockingSidewalk", true);
            Settings.BicyclePursuit = initializationFile.ReadBoolean("Callouts", "BicyclePursuit", true);
            Settings.PersonCarryingAConcealedWeapon = initializationFile.ReadBoolean("Callouts", "PersonCarryingAConcealedWeapon", true);
            Settings.Loitering = initializationFile.ReadBoolean("Callouts", "Loitering", true);
            Settings.VehicleBlockingCrosswalk = initializationFile.ReadBoolean("Callouts", "VehicleBlockingCrosswalk", true);
            Settings.BicycleBlockingRoadway = initializationFile.ReadBoolean("Callouts", "BicycleBlockingRoadway", true);
            Settings.SuspiciousVehicle = initializationFile.ReadBoolean("Callouts", "SuspiciousVehicle", true);
            Settings.AbandonedVehicle = initializationFile.ReadBoolean("Callouts", "AbandonedVehicle", true);
            Settings.DrugsFound = initializationFile.ReadBoolean("Callouts", "DrugsFound", true);
            Settings.SuspiciousPerson = initializationFile.ReadBoolean("Callouts", "SuspiciousPerson", true);
            Settings.OfficerDown = initializationFile.ReadBoolean("Callouts", "OfficerDown", true);
            Settings.DerangedDrunkenFeller = initializationFile.ReadBoolean("Callouts", "DerangedDrunkenFeller", true);
            HelpMessages = initializationFile.ReadBoolean("Settings", "HelpMessages", true);
            EndCall = initializationFile.ReadEnum<Keys>("Keys", "EndCall", Keys.End);
            Dialog = initializationFile.ReadEnum<Keys>("Keys", "Dialog", Keys.Y);
            PickUp = initializationFile.ReadEnum<Keys>("Keys", "PickUp", Keys.E);
            Language = initializationFile.ReadString("Settings", "Language", "English");
        }

        public static readonly string PluginVersion = "0.2.9.0";
    }
}