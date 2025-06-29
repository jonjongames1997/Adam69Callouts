﻿using CalloutInterfaceAPI;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] Knife Attack", CalloutProbability.Medium, "Knife attack reported", "Code 3", "LSPD")]
    public class KnifeAttack : Callout
    {

        // General variables
        private static Ped suspect;
        private static Ped victim;
        private static float suspectHeading;
        private static float victimHeading;
        private static Vector3 spawnpoint;
        private static Vector3 victimSpawn;
        private static Blip suspectBlip;
        private static Blip victimBlip;
        private static int _scenario;
        private static bool hasBegunAttacking;
        private static bool isArmed;
        private static bool hasPursuitBegun;
        private static bool hasSpoke;
        private static bool ispursuitCreated = false;
        private static LHandle pursuit;


        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = new(-315.15f, 2786.42f, 59.56f);
            suspectHeading = 254.36f;
            victimSpawn = new(-311.28f, 2789.56f, 59.52f);
            victimHeading = 284.96f;
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of a knife attack in progress");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_KnifeAttack_Audio");
            CalloutMessage = "Knife attack in progress";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[Adam69 Callouts LOG]: Knife Attack callout accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~w~Knife Attack", "~b~Dispatch~w~: The suspect has been located. Respond ~r~Code 3~w~.");

            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("Adam69Callouts_Respond_Code_3_Audio");

            suspect = new Ped(spawnpoint)
            {
                IsPersistent = true,
                BlockPermanentEvents = true
            };
            suspect.IsValid();

            victim = new Ped(victimSpawn)
            {
                IsPersistent = true,
                BlockPermanentEvents = true
            };

            victim.IsValid();

            NativeFunction.Natives.APPLY_PED_DAMAGE_PACK(victim, "HitByVehicle", 1f, 1f);
            NativeFunction.Natives.IS_PED_INJURED(victim);
            NativeFunction.Natives.IS_PED_HURT(victim);

            suspectBlip = suspect.AttachBlip();
            suspectBlip.Color = System.Drawing.Color.Red;
            suspectBlip.IsRouteEnabled = true;
            suspectBlip.Alpha = 0.75f;

            victimBlip = victim.AttachBlip();
            victimBlip.Color = System.Drawing.Color.Blue;
            victimBlip.IsRouteEnabled = true;
            victimBlip.Alpha = 0.75f;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (suspect.Exists())
            {
                suspect.Delete();
            }
            if (victim.Exists())
            {
                victim.Delete();
            }
            if (suspectBlip.Exists())
            {
                suspectBlip.Delete();
            }
            if (victimBlip.Exists())
            {
                victimBlip.Delete();
            }
            base.OnCalloutNotAccepted();

        }

        public override void Process()
        {
            if (!suspect.Inventory.Weapons.Contains(WeaponHash.Knife) && suspect.DistanceTo(MainPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 20f)
            {
                suspect.Inventory.GiveNewWeapon(WeaponHash.Knife, 0, true);
                isArmed = true;
            }
            else if (!isArmed && suspect.Inventory.Weapons.Contains(WeaponHash.Knife) && suspect.DistanceTo(MainPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 20f)
            {
                suspect.Inventory.EquippedWeapon = WeaponHash.Knife;
                isArmed = true;
            }

            if (!hasBegunAttacking && suspect && suspect.DistanceTo(MainPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 20f)
            {
                hasBegunAttacking = true;
                GameFiber.StartNew(() =>
                {
                    switch (_scenario)
                    {
                        case > 50:
                            suspect.KeepTasks = true;
                            suspect.Tasks.FightAgainst(MainPlayer);
                            switch (Rndm.Next(1, 4))
                            {
                                case 1:
                                    Game.DisplaySubtitle("~r~Suspect~w~: I'm going to stab you!", 5000);
                                    hasSpoke = true;
                                    break;

                                case 2:
                                    Game.DisplaySubtitle("~r~Suspect~w~: You picked the wrong person to mess with!", 5000);
                                    hasSpoke = true;
                                    break;

                                case 3:
                                    Game.DisplaySubtitle("~r~Suspect~w~: I'll cut you!", 5000);
                                    hasSpoke = true;
                                    break;
                            }
                            GameFiber.Wait(5000);
                            break;

                        default:
                            if (!hasPursuitBegun)
                            {
                                ispursuitCreated = true;
                                pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                                LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, suspect);
                                LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);
                                hasPursuitBegun = true;
                            }

                            break;

                    }

                });

            }
            base.Process();

            if (MainPlayer.IsDead || suspect.IsDead || Game.IsKeyDown(Settings.EndCall))
            {
                bool missionMessages = Settings.MissionMessages;
                if (missionMessages == true)
                {
                    BigMessageThread bigMessage = new BigMessageThread();
                    bigMessage.MessageInstance.ShowColoredShard("Callout Failed!", "You are now ~r~CODE 4~w~.", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);
                }
                else
                {
                    missionMessages = false;
                    return;
                }

                this.End();
            }
        }

        public override void End()
        {
            if (suspect.Exists())
            {
                suspect.Delete();
            }
            if (victim.Exists())
            {
                victim.Delete();
            }
            if (suspectBlip.Exists())
            {
                suspectBlip.Delete();
            }
            if (victimBlip.Exists())
            {
                victimBlip.Delete();
            }

            bool missionMessages = Settings.MissionMessages;
            if (missionMessages == true)
            {
                BigMessageThread bigMessage = new BigMessageThread();
                bigMessage.MessageInstance.ShowColoredShard("Callout Complete!", "You are now ~r~CODE 4~w~.", RAGENativeUI.HudColor.Red, RAGENativeUI.HudColor.Black, 5000);
            }
            else
            {
                missionMessages = false;
                return;
            }
            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Knife Attack callout is CODE 4!");
        }
    }
}
