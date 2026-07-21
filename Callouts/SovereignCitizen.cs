using CalloutInterfaceAPI;
using Adam69Callouts.Common;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using PolicingRedefined.API;
using PolicingRedefined;

namespace Adam69Callouts.Callouts
{
    [CalloutInterface("[Adam69 Callouts] Sovereign Citizen", CalloutProbability.Medium, "Sovereign citizen causing a disturbance — traffic stop refusal, paperwork showdowns, or barricade scenarios", "Code 2", "LSPD")]
    public class SovereignCitizen : Callout
    {
        private enum ScenarioType { TrafficStop, CheckpointRefusal, ProtestBlocking, ArmedRefusal }

        // Instance fields (avoid shared state between callout instances)
        private ScenarioType _scenario;
        private Ped _suspect;
        private Ped _passenger;
        private Vehicle _suspectVehicle;
        private Blip _suspectBlip;
        private Vector3 _spawnPoint;
        private int _dialogStage;
        private bool _suspectArmed;
        private bool _suspectAggressive;

        // Pools and constants
        private static readonly Random s_random = new Random();
        private static readonly string[] VehiclePool = new[] { "sentinel", "regina", "felon", "cog55", "stafford" };
        private static readonly string[] PedPool = new[] { "s_m_m_prisoner_01", "g_m_m_chigoon_02", "a_m_m_farmer_01", "a_f_y_tourist_01" };
        private static readonly string[] WeaponPool = new[] { "weapon_pistol", "weapon_sawnoffshotgun", "weapon_microsmg" };

        // Reusable audio / UI constants
        private const string NotifyTexture = "web_adam69callouts";
        private const string CalloutTitle = "Sovereign Citizen";

        public override bool OnBeforeCalloutDisplayed()
        {
            // Choose a semi-urban street or intersection near player
            _spawnPoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around2D(250f, 700f));
            ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 120f);

            // Random scenario selection
            _scenario = (ScenarioType)s_random.Next(0, Enum.GetNames(typeof(ScenarioType)).Length);

            CalloutInterfaceAPI.Functions.SendMessage(this, "Sovereign Citizen report: refusal to comply with lawful orders or obstruction.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudioUsingPosition("SUSPICIOUS_PERSON_02", _spawnPoint);
            CalloutMessage = "Sovereign Citizen Disturbance";
            CalloutPosition = _spawnPoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            LogIfEnabled("[Adam69 Callouts LOG]: Sovereign Citizen callout accepted!");
            Game.DisplayNotification(NotifyTexture, NotifyTexture, "~w~Adam69 Callouts", CalloutTitle, "~b~Dispatch~w~: Reports of a Sovereign Citizen. Expect refusal/uncooperative behaviour. Respond ~r~Code 2~w~.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_2_Audio");

            // Randomize suspect attributes
            _suspectArmed = s_random.Next(0, 10) > 6; // ~30% armed
            _suspectAggressive = s_random.Next(0, 10) > 7; // ~20% aggressive

            try
            {
                SpawnScenarioEntities();
                SetupSuspectCommon();

                _dialogStage = 0;
            }
            catch (Exception ex)
            {
                LogException("OnCalloutAccepted", ex);
            }

            return base.OnCalloutAccepted();
        }

        private void SpawnScenarioEntities()
        {
            if (_scenario == ScenarioType.TrafficStop || _scenario == ScenarioType.CheckpointRefusal || _scenario == ScenarioType.ArmedRefusal)
            {
                string vehModel = VehiclePool[s_random.Next(VehiclePool.Length)];
                _suspectVehicle = new Vehicle(vehModel, _spawnPoint);
                if (_suspectVehicle.Exists())
                {
                    _suspectVehicle.IsPersistent = true;
                    _suspectVehicle.IsEngineOn = false;
                }

                _suspect = _suspectVehicle?.CreateRandomDriver();

                // Possibly spawn a passenger
                if (s_random.Next(0, 10) > 6 && _suspectVehicle != null && _suspectVehicle.Exists())
                {
                    int? seatIndex = _suspectVehicle.GetFreePassengerSeatIndex();
                    if (seatIndex.HasValue)
                    {
                        _passenger = new Ped(PedPool[s_random.Next(PedPool.Length)], _suspectVehicle.GetOffsetPositionFront(2f), 0f);
                        if (_passenger.Exists())
                        {
                            _passenger.WarpIntoVehicle(_suspectVehicle, seatIndex.Value);
                        }
                    }
                }
            }
            else // ProtestBlocking
            {
                _suspect = new Ped(PedPool[s_random.Next(PedPool.Length)], _spawnPoint.Around2D(1f, 6f), 0f);
                _passenger = new Ped(PedPool[s_random.Next(PedPool.Length)], _spawnPoint.Around2D(1f, 6f), 0f);
                if (_passenger.Exists())
                {
                    _passenger.IsPersistent = true;
                    _passenger.BlockPermanentEvents = true;
                    _passenger.Tasks.PlayAnimation(new AnimationDictionary("amb@world_human_protest_sign@male_a"), "base", -1f, AnimationFlags.Loop);
                }
            }
        }

        private void SetupSuspectCommon()
        {
            if (_suspect == null || !_suspect.Exists()) return;

            _suspect.IsPersistent = true;
            _suspect.BlockPermanentEvents = true;

            if (_suspectArmed)
            {
                SafeInventory.SafeGiveWeapon(_suspect, WeaponPool[s_random.Next(WeaponPool.Length)], 120, true);
            }

            _suspectBlip = _suspect.AttachBlip();
            if (_suspectBlip != null && _suspectBlip.Exists())
            {
                _suspectBlip.Color = System.Drawing.Color.Orange;
                _suspectBlip.IsRouteEnabled = true;
            }

            switch (_scenario)
            {
                case ScenarioType.TrafficStop:
                    if (_suspectVehicle != null && _suspectVehicle.Exists())
                    {
                        _suspect.Tasks.PlayAnimation(new AnimationDictionary("random@arrests"), "idle_a", -1f, AnimationFlags.Loop);
                    }
                    break;
                case ScenarioType.CheckpointRefusal:
                    if (_passenger != null && _passenger.Exists())
                    {
                        _passenger.Tasks.PlayAnimation(new AnimationDictionary("amb@world_human_drug_dealer_hard@male@base"), "base", -1f, AnimationFlags.Loop);
                    }
                    break;
                case ScenarioType.ProtestBlocking:
                    // animations already applied when created
                    break;
                case ScenarioType.ArmedRefusal:
                    if (_suspectArmed)
                    {
                        Game.DisplayNotification(NotifyTexture, NotifyTexture, "~r~ALERT", "Possible Armed Refusal", "~b~Use caution — suspect may be armed.");
                    }
                    break;
            }
        }

        public override void OnCalloutNotAccepted()
        {
            CleanupEntities();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            base.Process();

            try
            {
                if (_suspect == null || !_suspect.Exists())
                {
                    return;
                }

                HandleProximityAndDialog();
                HandlePlayerRequests();
                HandleVehicleFlee();
                HandleDetainedOrDeath();

                if (MainPlayer.IsDead)
                {
                    if (Settings.MissionMessages)
                    {
                        var bigMessage = new BigMessageThread();
                        bigMessage.MessageInstance.ShowColoredShard("MISSION FAILED!", "You have fallen in the line of duty.", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);
                    }

                    LogIfEnabled("Adam69 Callouts: Sovereign Citizen callout failed - Player died.");
                    End();
                    return;
                }

                if (Game.IsKeyDown(Settings.EndCall))
                {
                    End();
                }
            }
            catch (Exception ex)
            {
                LogException("Process", ex);
            }
        }

        private void HandleProximityAndDialog()
        {
            if (_dialogStage == 0 && MainPlayer.DistanceTo(_suspect) <= 12f)
            {
                if (Settings.HelpMessages)
                {
                    Game.DisplayHelp("Press ~y~" + Settings.Dialog.ToString() + "~w~ to approach and begin lawful contact. Press ~y~" + Settings.RequestVehicleInfo.ToString() + "~w~ to request vehicle info.");
                }

                if (Game.IsKeyDown(Settings.Dialog))
                {
                    GameFiber.Sleep(300);
                    NativeFunction.Natives.TASK_TURN_PED_TO_FACE_ENTITY(_suspect, MainPlayer, -1);
                    Game.DisplaySubtitle("~b~You~w~: Good afternoon. I'm stopping you to confirm identification and registration. Please step out and provide documents.");
                    GameFiber.Sleep(1800);

                    _dialogStage = 1;
                }
            }

            if (_dialogStage == 1 && MainPlayer.DistanceTo(_suspect) <= 10f && Game.IsKeyDown(Settings.Dialog))
            {
                GameFiber.Sleep(250);
                _dialogStage = 2;

                bool refuses = s_random.Next(0, 10) > 5;
                if (refuses)
                {
                    Game.DisplaySubtitle("~r~Suspect~w~: I am a sovereign citizen. You have no jurisdiction. I refuse to provide documents.");
                    GameFiber.Sleep(1800);

                    if (_suspectAggressive)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: I will not comply!");
                        GameFiber.Sleep(1000);
                        if (_suspectArmed)
                        {
                            _suspect.Tasks.FightAgainst(MainPlayer);
                            _suspect.Armor = 300;
                            PolicingRedefined.API.BackupDispatchAPI.RequestPanicBackup();
                            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("OFFICERS_UNDER_FIRE");
                        }
                        else
                        {
                            _suspect.Tasks.ReactAndFlee(MainPlayer);
                            if (_suspectBlip != null && _suspectBlip.Exists()) _suspectBlip.Color = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        Game.DisplaySubtitle("~b~You~w~: Sir/Ma'am, failure to comply may result in arrest. Please step out.");
                        GameFiber.Sleep(1500);

                        if (Settings.HelpMessages)
                        {
                            Game.DisplayHelp("Press ~y~" + Settings.RequestVehicleInfo.ToString() + "~w~ to request backup or ~y~" + Settings.RequestTowTruck.ToString() + "~w~ to request tow/k9 (if needed).");
                        }
                    }
                }
                else
                {
                    Game.DisplaySubtitle("~r~Suspect~w~: Fine. Here are my documents.");
                    GameFiber.Sleep(1300);
                    Game.DisplaySubtitle("~b~You~w~: Thank you. Everything seems in order. You are free to go.");
                }
            }
        }

        private void HandlePlayerRequests()
        {
            if (Game.IsKeyDown(Settings.RequestVehicleInfo))
            {
                GameFiber.Sleep(300);
                PolicingRedefined.API.InfoDispatchAPI.RunNearestVehicleThroughDispatch(true);
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Backup_Audio");
                Game.DisplaySubtitle("~b~You~w~: Dispatch, requesting backup.");
            }

            if (Game.IsKeyDown(Settings.RequestTowTruck))
            {
                GameFiber.Sleep(300);
                PolicingRedefined.API.BackupDispatchAPI.RequestTowServiceBackup();
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Tow_Truck_Audio");
                Game.DisplaySubtitle("~b~You~w~: Dispatch, request tow/K9 as needed.");
            }
        }

        private void HandleVehicleFlee()
        {
            if (_suspectVehicle != null && _suspectVehicle.Exists() && _suspect.IsInVehicle(_suspectVehicle, false))
            {
                if (_suspectVehicle.Speed > 5f && MainPlayer.DistanceTo(_suspectVehicle) > 25f)
                {
                    var pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                    LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, _suspect);
                    LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);
                    Game.DisplayNotification("~r~Suspect fleeing! Pursue and attempt to stop the vehicle!");
                    if (_suspectBlip != null && _suspectBlip.Exists()) _suspectBlip.Color = System.Drawing.Color.Red;
                }
            }
        }

        private void HandleDetainedOrDeath()
        {
            if (_suspect != null && _suspect.Exists())
            {
                if (LSPD_First_Response.Mod.API.Functions.IsPedArrested(_suspect) || LSPD_First_Response.Mod.API.Functions.IsPedStoppedByPlayer(_suspect))
                {
                    Game.DisplayHelp("Suspect detained. Press ~y~" + Settings.EndCall.ToString() + "~w~ to end the callout.");
                }

                if (_suspect.IsDead)
                {
                    Game.DisplayNotification(NotifyTexture, NotifyTexture, "~w~Adam69 Callouts", "~r~Sovereign Citizen", "~r~Suspect is deceased. Notify dispatch.");
                }
            }
        }

        public override void End()
        {
            try
            {
                CleanupEntities();

                Game.DisplayNotification(NotifyTexture, NotifyTexture, "~w~Adam69 Callouts", CalloutTitle, "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
                LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Code_4_Audio");

                if (Settings.MissionMessages)
                {
                    var bigMessage = new BigMessageThread();
                    bigMessage.MessageInstance.ShowColoredShard("Callout Completed!", "Situation resolved. You are now ~g~CODE 4~w~.", RAGENativeUI.HudColor.Green, RAGENativeUI.HudColor.Black, 5000);
                }
            }
            catch (Exception ex)
            {
                LogException("End", ex);
            }
            finally
            {
                base.End();
                LogIfEnabled("[Adam69 Callouts LOG]: Sovereign Citizen callout ended CODE 4.");
            }
        }

        private void CleanupEntities()
        {
            try
            {
                if (_suspect != null && _suspect.Exists())
                {
                    _suspect.Dismiss();
                }

                if (_passenger != null && _passenger.Exists())
                {
                    _passenger.Dismiss();
                }

                if (_suspectVehicle != null && _suspectVehicle.Exists())
                {
                    _suspectVehicle.Dismiss();
                }

                if (_suspectBlip != null && _suspectBlip.Exists())
                {
                    _suspectBlip.Delete();
                }
            }
            catch (Exception ex)
            {
                LogException("CleanupEntities", ex);
            }
            finally
            {
                _suspect = null;
                _passenger = null;
                _suspectVehicle = null;
                _suspectBlip = null;
            }
        }

        private void LogIfEnabled(string message)
        {
            if (Settings.EnableLogs)
            {
                Game.LogTrivial(message);
                LoggingManager.Log(message);
            }
        }

        private void LogException(string context, Exception ex)
        {
            if (Settings.EnableLogs)
            {
                Game.LogTrivial($"[Adam69 Callouts ERROR]: SovereignCitizen.{context} Exception: {ex.Message}\n{ex.StackTrace}");
                LoggingManager.Log($"Adam69 Callouts: SovereignCitizen.{context} Exception: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}