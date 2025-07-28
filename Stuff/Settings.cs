using System.Windows.Forms;
using System.Xml;
using Rage;

namespace Adam69Callouts
{
    internal static class Settings
    {
        internal static bool VehicleBlockingSidewalk { get; set; }
        internal static bool BicyclePursuit { get; set; }
        internal static bool PersonCarryingAConcealedWeapon { get; set; }
        internal static bool Loitering { get; set; }
        internal static bool VehicleBlockingCrosswalk { get; set; }
        internal static bool BicycleBlockingRoadway { get; set; }
        internal static bool SuspiciousVehicle { get; set; }
        internal static bool AbandonedVehicle { get; set; }
        internal static bool DrugsFound { get; set; }
        internal static bool SuspiciousPerson { get; set; }
        internal static bool OfficerDown { get; set; }
        internal static bool DerangedDrunkenFeller { get; set; }
        internal static bool DeadBirdOnTheRoad { get; set; }
        internal static bool KnifeAttack { get; set; }
        internal static bool HelpMessages  { get; set; }
        internal static bool MissionMessages { get; set; }
        internal static Keys EndCall = Keys.End;
        internal static Keys Dialog { get; set; }
        internal static Keys PickUp { get; set; }
        internal static Keys CallAnimalControlKey { get; set; }
        internal static Keys CallAmbulanceKey { get; set; }
        internal static string Localization { get; set; }
        internal static bool DisableBluelineDispatch { get; set; }


        internal static void LoadSettings()
        {
            Game.Console.Print("[LOG]: Loading config file from Adam69 Callouts");
            InitializationFile initializationFile = new InitializationFile("Plugins/LSPDFR/Adam69Callouts/Adam69Callouts.ini");
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
            Settings.SuspiciousPerson = initializationFile.ReadBoolean("Callouts", "SuspiciousPerson", false);
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
            CallAmbulanceKey = initializationFile.ReadEnum<Keys>("Keys", "CallAmbulanceKey", Keys.K);
            Localization = initializationFile.ReadString("Settings", "Language", Localization);
            Settings.DisableBluelineDispatch = initializationFile.ReadBoolean("Settings", "DisableBluelineDispatch", true);
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
            ini.Write("Settings", "Language", Localization);
            ini.Write("Keys", "CallAnimalControlKey", CallAnimalControlKey);
            ini.Write("Keys", "CallAmbulanceKey", CallAmbulanceKey);
            ini.Write("Settings", "DisableBluelineDispatch", DisableBluelineDispatch);
        }

        public static readonly string PluginVersion = "0.4.2";
    }
}