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
        internal static bool DeadBirdOnTheRoad = true;
        internal static bool KnifeAttack = true;
        internal static bool HelpMessages = true;
        internal static bool MissionMessages = true;
        internal static Keys EndCall = Keys.End;
        internal static Keys Dialog = Keys.Y;
        internal static Keys PickUp = Keys.E;
        internal static Keys CallAnimalControlKey = Keys.NumPad1;
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
            Settings.DeadBirdOnTheRoad = initializationFile.ReadBoolean("Callouts", "DeadBirdOnTheRoad", true);
            Settings.KnifeAttack = initializationFile.ReadBoolean("Callouts", "KnifeAttack", true);
            HelpMessages = initializationFile.ReadBoolean("Settings", "HelpMessages", true);
            MissionMessages = initializationFile.ReadBoolean("Settings", "MissionMessages", true);
            EndCall = initializationFile.ReadEnum<Keys>("Keys", "EndCall", Keys.End);
            Dialog = initializationFile.ReadEnum<Keys>("Keys", "Dialog", Keys.Y);
            PickUp = initializationFile.ReadEnum<Keys>("Keys", "PickUp", Keys.E);
            CallAnimalControlKey = initializationFile.ReadEnum<Keys>("Keys", "CallAnimalControlKey", Keys.NumPad1);
            Language = initializationFile.ReadString("Settings", "Language", Language);
        }

        internal static void SaveConfigSettings()
        {
            var ini = new InitializationFile("Plugins/LSPDFR/Adam69Callouts.ini");
            ini.Create();
            ini.Write("Callouts", "VehicleBlockingSidewalk", VehicleBlockingSidewalk);
            ini.Write("Callouts", "BicyclePursuit", BicyclePursuit);
            ini.Write("Callouts", "PersonCarryingAConcealedWeapon", PersonCarryingAConcealedWeapon);
            ini.Write("Callouts", "Loitering", Loitering);
            ini.Write("Callouts", "VehicleBlockingCrosswalk", VehicleBlockingCrosswalk);
            ini.Write("Callouts", "BicycleBlockingRoadway", BicycleBlockingRoadway);
            ini.Write("Callouts", "SuspiciousVehicle", SuspiciousVehicle);
            ini.Write("Callouts", "AbandonedVehicle", AbandonedVehicle);
            ini.Write("Callouts", "DrugsFound", DrugsFound);
            ini.Write("Callouts", "SuspiciousPerson", SuspiciousPerson);
            ini.Write("Callouts", "OfficerDown", OfficerDown);
            ini.Write("Callouts", "DerangedDrunkenFeller", DerangedDrunkenFeller);
            ini.Write("Callouts", "DeadBirdOnTheRoad", DeadBirdOnTheRoad);
            ini.Write("Callouts", "KnifeAttack", KnifeAttack);
            ini.Write("Settings", "HelpMessages", HelpMessages);
            ini.Write("Settings", "MissionMessages", MissionMessages);
            ini.Write("Keys", "EndCall", EndCall);
            ini.Write("Keys", "Dialog", Dialog);
            ini.Write("Keys", "PickUp", PickUp);
            ini.Write("Settings", "Language", Language);
            ini.Write("Keys", "CallAnimalControlKey", CallAnimalControlKey);
        }

        public static readonly string PluginVersion = "0.4.0";
    }
}