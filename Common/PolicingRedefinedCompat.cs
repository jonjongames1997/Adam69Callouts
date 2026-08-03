using System;
using System.Linq;
using System.Reflection;
using Rage;

namespace Adam69Callouts.Common
{
    // Reflection-based runtime wrapper. Safe no-op when PolicingRedefined isn't present.
    public static class PolicingRedefinedCompat
    {
        private static Type GetTypeByName(string fullName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetType(fullName, false))
                .FirstOrDefault(t => t != null);
        }

        private static object InvokeStatic(string typeFullName, string methodName, params object[] args)
        {
            try
            {
                var type = GetTypeByName(typeFullName);
                if (type == null) return null;
                var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
                if (method == null) return null;
                return method.Invoke(null, args);
            }
            catch (Exception ex)
            {
                Game.LogTrivial($"[Adam69Callouts] Reflection call failed: {ex.Message}");
                return null;
            }
        }

        // Try multiple candidate types/methods and match method by parameter count before invoking.
        private static bool TryInvokeCandidates(string[] typeFullNames, string[] methodNames, params object[] args)
        {
            try
            {
                foreach (var typeName in typeFullNames)
                {
                    var type = GetTypeByName(typeName);
                    if (type == null) continue;

                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                                      .Where(m => methodNames.Contains(m.Name, StringComparer.OrdinalIgnoreCase));

                    foreach (var method in methods)
                    {
                        var parameters = method.GetParameters();
                        // crude match by parameter count; many PolicingRedefined API methods accept either (Vector3) or (Entity) or (Vector3,int)
                        if (parameters.Length != args.Length) continue;

                        try
                        {
                            method.Invoke(null, args);
                            return true;
                        }
                        catch (TargetInvocationException tie)
                        {
                            Game.LogTrivial($"[Adam69Callouts] PolicingRedefined invocation error: {tie.InnerException?.Message ?? tie.Message}");
                        }
                        catch (Exception ex)
                        {
                            Game.LogTrivial($"[Adam69Callouts] PolicingRedefined invocation failed: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Game.LogTrivial($"[Adam69Callouts] TryInvokeCandidates exception: {ex.Message}");
            }

            return false;
        }

        // Try multiple candidate types/methods and return the first non-null result.
        private static object TryInvokeCandidatesReturn(string[] typeFullNames, string[] methodNames, params object[] args)
        {
            try
            {
                foreach (var typeName in typeFullNames)
                {
                    var type = GetTypeByName(typeName);
                    if (type == null) continue;

                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                                      .Where(m => methodNames.Contains(m.Name, StringComparer.OrdinalIgnoreCase));

                    foreach (var method in methods)
                    {
                        var parameters = method.GetParameters();
                        if (parameters.Length != args.Length) continue;

                        try
                        {
                            var result = method.Invoke(null, args);
                            if (result != null) return result;
                            // If a method explicitly returns a value type default (e.g., false/0), still return it
                            if (method.ReturnType.IsValueType)
                            {
                                return result;
                            }
                        }
                        catch (TargetInvocationException tie)
                        {
                            Game.LogTrivial($"[Adam69Callouts] PolicingRedefined invocation error: {tie.InnerException?.Message ?? tie.Message}");
                        }
                        catch (Exception ex)
                        {
                            Game.LogTrivial($"[Adam69Callouts] PolicingRedefined invocation failed: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Game.LogTrivial($"[Adam69Callouts] TryInvokeCandidatesReturn exception: {ex.Message}");
            }

            return null;
        }

        public static void Slow(bool enable) => InvokeStatic("PolicingRedefined.API.TrafficControlAPI", "Slow", enable);
        public static bool IsActive()
        {
            var r = InvokeStatic("PolicingRedefined.API.TrafficControlAPI", "IsActive");
            return r is bool b && b;
        }

        public static void RunVehicleThroughDispatch(Vehicle vehicle, bool a, bool b, bool c)
        {
            try
            {
                var type = GetTypeByName("PolicingRedefined.API.VehicleAPI");
                if (type == null) return;
                var method = type.GetMethod("RunVehicleThroughDispatch", BindingFlags.Public | BindingFlags.Static);
                method?.Invoke(null, new object[] { vehicle, a, b, c });
            }
            catch (Exception ex)
            {
                Game.LogTrivial($"[Adam69Callouts] Reflection call failed: {ex.Message}");
            }
        }

        public static void Clear() => InvokeStatic("PolicingRedefined.API.TrafficControlAPI", "Clear");

        // New helpers: request services / code responses / panic backup via PolicingRedefined if available.
        // These methods attempt multiple plausible PolicingRedefined API types and method names and will silently no-op if none match.

        public static void RequestTowService(Vehicle vehicle)
        {
            if (vehicle == null || !vehicle.Exists()) return;

            // candidate types and method names commonly used in policing-type mods
            var types = new[]
            {
                "PolicingRedefined.API.TowServiceAPI",
                "PolicingRedefined.API.DispatchAPI",
                "PolicingRedefined.API.VehicleAPI"
            };

            var methods = new[]
            {
                "RequestTow",
                "RequestTowService",
                "CallTow",
                "DispatchTow",
                "RequestTowingService"
            };

            // try invoke with the vehicle instance first
            if (TryInvokeCandidates(types, methods, vehicle)) return;

            // fallback: try by position
            TryInvokeCandidates(types, methods, vehicle.Position);
        }

        public static void RequestAnimalControl(Ped animal)
        {
            if (animal == null || !animal.Exists()) return;

            var types = new[]
            {
                "PolicingRedefined.API.AnimalControlAPI",
                "PolicingRedefined.API.WildlifeAPI",
                "PolicingRedefined.API.AnimalsAPI",
                "PolicingRedefined.API.DispatchAPI"
            };

            var methods = new[]
            {
                "RequestAnimalControl",
                "RequestAnimalControlUnit",
                "RequestAnimalServices",
                "CallAnimalControl",
                "RequestWildlifeControl",
                "RequestAnimalControlService",
                "RequestAnimal",
                "RequestAnimalControlUnits"
            };

            // try invoke with the entity first
            if (TryInvokeCandidates(types, methods, animal)) return;

            // fallback: try by position
            TryInvokeCandidates(types, methods, animal.Position);
        }

        public static void RequestAnimalControl(Vector3 location)
        {
            var types = new[]
            {
                "PolicingRedefined.API.AnimalControlAPI",
                "PolicingRedefined.API.WildlifeAPI",
                "PolicingRedefined.API.AnimalsAPI",
                "PolicingRedefined.API.DispatchAPI"
            };

            var methods = new[]
            {
                "RequestAnimalControl",
                "RequestAnimalControlUnit",
                "RequestAnimalServices",
                "CallAnimalControl",
                "RequestWildlifeControl",
                "RequestAnimalControlService",
                "RequestAnimal",
                "RequestAnimalControlUnits"
            };

            // try (Vector3) first
            if (TryInvokeCandidates(types, methods, location)) return;

            // try common variants
            TryInvokeCandidates(types, methods, location, true);
            TryInvokeCandidates(types, methods, location, "AnimalControl");
        }

        public static void RequestCode1(Vector3 location)
        {
            var types = new[] { "PolicingRedefined.API.DispatchAPI", "PolicingRedefined.API.ResponseAPI" };
            var methods = new[] { "RequestCode1", "RequestCode", "RequestUnits", "RequestBackup" };

            // try (Vector3), (Vector3,int) variants
            if (TryInvokeCandidates(types, methods, location)) return;
            TryInvokeCandidates(types, methods, location, 1);
            TryInvokeCandidates(types, methods, location, "Code1");
        }

        public static void RequestCode2(Vector3 location)
        {
            var types = new[] { "PolicingRedefined.API.DispatchAPI", "PolicingRedefined.API.ResponseAPI" };
            var methods = new[] { "RequestCode2", "RequestCode", "RequestUnits", "RequestBackup" };

            if (TryInvokeCandidates(types, methods, location)) return;
            TryInvokeCandidates(types, methods, location, 2);
            TryInvokeCandidates(types, methods, location, "Code2");
        }

        public static void RequestCode3(Vector3 location)
        {
            var types = new[] { "PolicingRedefined.API.DispatchAPI", "PolicingRedefined.API.ResponseAPI" };
            var methods = new[] { "RequestCode3", "RequestCode", "RequestUnits", "RequestBackup" };

            if (TryInvokeCandidates(types, methods, location)) return;
            TryInvokeCandidates(types, methods, location, 3);
            TryInvokeCandidates(types, methods, location, "Code3");
        }

        public static void RequestPanicBackup(Ped ped)
        {
            if (ped == null || !ped.Exists()) return;

            var types = new[] { "PolicingRedefined.API.DispatchAPI", "PolicingRedefined.API.ResponseAPI" };
            var methods = new[] { "RequestPanicBackup", "PanicBackup", "RequestPanic", "RequestEmergencyBackup", "RequestImmediateBackup" };

            // try entity-specific variants first
            if (TryInvokeCandidates(types, methods, ped)) return;

            // fallback to position-based request
            RequestPanicBackup(ped.Position);
        }

        public static void RequestPanicBackup(Vehicle vehicle)
        {
            if (vehicle == null || !vehicle.Exists()) return;

            var types = new[] { "PolicingRedefined.API.DispatchAPI", "PolicingRedefined.API.ResponseAPI" };
            var methods = new[] { "RequestPanicBackup", "PanicBackup", "RequestPanic", "RequestEmergencyBackup", "RequestImmediateBackup" };

            // try entity-specific variants first
            if (TryInvokeCandidates(types, methods, vehicle)) return;

            // fallback to position-based request
            RequestPanicBackup(vehicle.Position);
        }

        public static void RequestPanicBackup(Vector3 location)
        {
            var types = new[] { "PolicingRedefined.API.DispatchAPI", "PolicingRedefined.API.ResponseAPI" };
            var methods = new[] { "RequestPanicBackup", "PanicBackup", "RequestPanic", "RequestEmergencyBackup", "RequestImmediateBackup" };

            if (TryInvokeCandidates(types, methods, location)) return;
            TryInvokeCandidates(types, methods, location, true);
        }

        public static void RequestPursuitBackup(Vector3 location)
        {
            var types = new[] { "PolicingRedefined.API.DispatchAPI", "PolicingRedefined.API.ResponseAPI", "PolicingRedefined.API.PursuitAPI" };
            var methods = new[] { "RequestPursuitBackup", "RequestPursuit", "RequestPursuitUnits", "RequestBackupForPursuit", "RequestBackup" };

            // try (Vector3) first
            if (TryInvokeCandidates(types, methods, location)) return;

            // try (Vector3,bool) or (Vector3,string) variants commonly used by other APIs
            TryInvokeCandidates(types, methods, location, true);
            TryInvokeCandidates(types, methods, location, "Pursuit");
        }

        public static void RequestCode3Ambulance(Vector3 location)
        {
            var types = new[] { "PolicingRedefined.API.AmbulanceAPI", "PolicingRedefined.API.DispatchAPI", "PolicingRedefined.API.MedicalAPI" };
            var methods = new[] { "RequestAmbulance", "RequestAmbulanceCode3", "RequestMedical", "RequestEMS" };

            if (TryInvokeCandidates(types, methods, location)) return;
            TryInvokeCandidates(types, methods, location, 3);
            TryInvokeCandidates(types, methods, location, "Code3");
        }

        // Policing Redefined: Ped resistance helpers

        public static object GetPedResistanceAction(Ped ped)
        {
            if (ped == null || !ped.Exists()) return null;

            var types = new[]
            {
                "PolicingRedefined.API.PedAPI",
                "PolicingRedefined.API.PedResistanceAPI",
                "PolicingRedefined.API.CombatAPI",
                "PolicingRedefined.API.PedBehaviorAPI"
            };

            var methods = new[]
            {
                "GetPedResistanceAction",
                "GetResistanceAction",
                "GetPedResistance",
                "GetResistance",
                "GetCurrentResistanceAction"
            };

            // try with the ped instance first
            var result = TryInvokeCandidatesReturn(types, methods, ped);
            if (result != null) return result;

            // fallback: try by ped position
            return TryInvokeCandidatesReturn(types, methods, ped.Position);
        }

        public static bool IsPedResistingRightNow(Ped ped)
        {
            if (ped == null || !ped.Exists()) return false;

            var types = new[]
            {
                "PolicingRedefined.API.PedAPI",
                "PolicingRedefined.API.PedResistanceAPI",
                "PolicingRedefined.API.CombatAPI",
                "PolicingRedefined.API.PedBehaviorAPI"
            };

            var methods = new[]
            {
                "IsPedResistingRightNow",
                "IsResistingRightNow",
                "IsResisting",
                "IsPedResisting",
                "IsCurrentlyResisting",
                "PedIsResisting"
            };

            // try with the ped instance first
            var r = TryInvokeCandidatesReturn(types, methods, ped);

            if (r is bool b) return b;
            if (r is int i) return i != 0;
            if (r is string s && bool.TryParse(s, out var parsed)) return parsed;

            // fallback: try by ped position
            r = TryInvokeCandidatesReturn(types, methods, ped.Position);
            if (r is bool b2) return b2;
            if (r is int i2) return i2 != 0;
            if (r is string s2 && bool.TryParse(s2, out var parsed2)) return parsed2;

            return false;
        }

        // New: Set a ped as "stopped" via PolicingRedefined if available.
        // Tries common candidate types and method names and matches multiple plausible signatures.
        public static void SetPedAsStopped(Ped ped, bool stopped)
        {
            if (ped == null || !ped.Exists()) return;

            var types = new[]
            {
                "PolicingRedefined.API.PedAPI",
                "PolicingRedefined.API.PedBehaviorAPI",
                "PolicingRedefined.API.PedControlAPI",
                "PolicingRedefined.API.CombatAPI"
            };

            var methods = new[]
            {
                "SetPedAsStopped",
                "SetPedStopped",
                "SetAsStopped",
                "SetStopped",
                "MarkPedStopped",
                "SetPedAsStationary"
            };

            // try (Ped,bool) signature first
            if (TryInvokeCandidates(types, methods, ped, stopped)) return;

            // try (Ped) signature (treat boolean as implicit)
            if (TryInvokeCandidates(types, methods, ped)) return;

            // fallback: try by position variants
            if (TryInvokeCandidates(types, methods, ped.Position, stopped)) return;
            TryInvokeCandidates(types, methods, ped.Position);
        }

        // New: Query whether a ped can be run through dispatch by PolicingRedefined.
        // Returns true when a matching PolicingRedefined API method indicates permissable; otherwise false.
        public static bool CanRunPedThroughDispatch(Ped ped)
        {
            if (ped == null || !ped.Exists()) return false;

            var types = new[]
            {
                "PolicingRedefined.API.PedAPI",
                "PolicingRedefined.API.DispatchAPI",
                "PolicingRedefined.API.PedDispatchAPI",
                "PolicingRedefined.API.PedBehaviorAPI"
            };

            var methods = new[]
            {
                "CanRunPedThroughDispatch",
                "CanRunThroughDispatch",
                "CanRunPedThroughDispatchForPed",
                "CanRunPedThroughDispatchForEntity",
                "CanDispatchPed",
                "CanDispatchEntity",
                "CanRunDispatchForPed"
            };

            // try (Ped) signature first
            var r = TryInvokeCandidatesReturn(types, methods, ped);
            if (r is bool b) return b;
            if (r is int i) return i != 0;
            if (r is string s && bool.TryParse(s, out var parsed)) return parsed;

            // fallback: try by position
            r = TryInvokeCandidatesReturn(types, methods, ped.Position);
            if (r is bool b2) return b2;
            if (r is int i2) return i2 != 0;
            if (r is string s2 && bool.TryParse(s2, out var parsed2)) return parsed2;

            return false;
        }
    }
}