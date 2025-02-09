namespace Adam69Callouts.VersionChecker
{
    public class PluginCheck
    {

        public static bool IsUpdateAvailable()
        {
            string curVersion = Settings.PluginVersion;
            Uri latestVersionUri = new("https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=49465&textOnly=1");
            WebClient webClient = new();
            string recieveData;
            try
            {
                recieveData = webClient.DownloadString(latestVersionUri).Trim();
            }
            catch (WebException)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~Adam69 Callouts Warning", "~r~Failed to check for an update", "Please make sure you're ~y~connected~w~ to your WiFi Network or try to reload the plugin");
                Game.Console.Print();
                Game.Console.Print("===================================================== Adam69 Callouts ===========================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING!]: Failed to check for an update!");
                Game.Console.Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
                Game.Console.Print();
                Game.Console.Print("==================================================== Adam69 Callouts ============================================");
                Game.Console.Print();
                return false;
            }
            if (recieveData != Settings.PluginVersion)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~Adam69 Callouts Warning", "~y~A new update is available!", "Current Version: ~r~" + curVersion + "~w~<br>New Version: ~y~" + recieveData + "<br>~w~Please Update to the latest build for ~y~new callouts~w~ and ~g~improvments! :-)");
                Game.Console.Print();
                Game.Console.Print("===================================================== Adam69 Callouts ===========================================");
                Game.Console.Print();
                Game.Console.Print("[LOG]: Current Version:" + curVersion);
                Game.Console.Print("[LOG]: New Version:" + recieveData);
                Game.Console.Print();
                Game.Console.Print("===================================================== Adam69 Callouts ===========================================");
                Game.Console.Print();
                return true;
            }
            else
            {
                Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "", "Detected the ~g~latest~w~ build of ~o~Adam69 Callouts! Thank you for downloading!");
                return false;
            }
        }

    }

}
