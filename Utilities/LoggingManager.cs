using Rage;
using Adam69Callouts.Utilities;

namespace Adam69Callouts.Utilities
{
    internal static class LoggingManager
    {
        private const string loggingPrefix = "Adam69 Callouts";

        internal static void Logging(string message)
        {
            if (GlobalsManager.TheApplication.DebugLogging)
            {
                Game.LogTrivial($"{loggingPrefix}: {message}");
            }

            Game.LogTrivial($"{loggingPrefix}: {message}");
        }
    }
}
