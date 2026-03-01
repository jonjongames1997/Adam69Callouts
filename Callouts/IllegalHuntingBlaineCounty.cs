using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] Illegal Hunting", CalloutProbability.Medium, "Reports of Illegal Hunting of Endangered Species", "Code 2", "SASPR")]

    public class IllegalHuntingBlaineCounty
    {

        private static Ped suspect;
        private static Ped theAnimal;
        private static Vehicle susVehicle;
        private static Blip susBlip;
        private static Vector3 spawnpoint;
        private static Vector3 animalSpawn;
        private static Ped ParkOfficer;
        private static Vehicle rangerVehicle;

    }
}
