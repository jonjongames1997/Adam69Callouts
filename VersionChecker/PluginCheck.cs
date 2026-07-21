using System.Net;

namespace Adam69Callouts.VersionChecker
{
    public class PluginCheck
    {

        public static bool IsUpdateAvailable()
        {
            string curVersion = Settings.PluginVersion;
            Uri latestVersionUri = new("https://api-prod.lcpdfr.com/api/downloadsng/files/49465/version");
            WebClient webClient = new();
            string recievedData;

            try
            {
                recievedData = webClient.DownloadString(latestVersionUri).Trim();
            }
            catch (WebException ex)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~Adam69 Callouts Warning", "~r~Failed to check for an update", "Please make sure you're ~y~connected~w~ to your WiFi Network or try to reload the plugin");
                Game.Console.Print();
                Game.Console.Print("===================================================== Adam69 Callouts ===========================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING!]: Failed to check for an update!");
                Game.Console.Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
                Game.Console.Print("[ERROR]: " + ex.Message);
                Game.Console.Print();
                Game.Console.Print("==================================================== Adam69 Callouts ============================================");
                Game.Console.Print();
                LoggingManager.Log("[Adam69 Callouts] LOG: Error: " + ex.Message);
                return false;
            }

            if (recievedData != Settings.PluginVersion)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~Adam69 Callouts Warning", "~y~A new update is available!", $"Current Version: ~r~{curVersion}~w~<br>New Version: ~y~{recievedData}<br>~w~Please Update to the latest build for new ~p~callouts~w~ and ~g~improvements~w~!:-)");
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~Adam69 Callouts Deprecation Warning", "~y~Adam69Callouts Deprecation Info:", "v0.3.3.3 or lower is ~r~NO LONGER supported~w~ and ~r~deprecated~w~. Update to latest build for guaranteed support.");
                Game.Console.Print();
                Game.Console.Print("===================================================== Adam69 Callouts ===========================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING!]: A new version of Adam69 Callouts is NOW AVAILABLE to download! Update to latest build!");
                Game.Console.Print("[WARNING!]: v0.4.6.0 or lower is NO LONGER Supported! Update to latest build for guaranteed suppport");
                Game.Console.Print($"[LOG]: Current Version: {curVersion}");
                Game.Console.Print($"[LOG]: New Version: {recievedData}");
                Game.Console.Print();
                Game.Console.Print("===================================================== Adam69 Callouts ===========================================");
                LoggingManager.Log("[Adam69 Callouts] LOG: This version is OUTDATED. Update to the latest version.");
                Game.Console.Print();
                return true;
            }
            else
            {
                Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "", "Detected the ~g~latest~w~ build of ~o~Adam69 Callouts~w~! Thank you for downloading! :-)");
                LoggingManager.Log("[Adam69 Callouts] LOG: You are on the LATEST version of the callout pack. Good Boy!");
                return false;
            }
        }
    }
}
