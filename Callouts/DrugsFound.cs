﻿using CalloutInterfaceAPI;
using LSPD_First_Response.Engine;
using System.Security.Cryptography.X509Certificates;

namespace Adam69Callouts.Callouts
{

    [CalloutInterface("[Adam69 Callouts] Drugs Found", CalloutProbability.Medium, "Drugs found reported by a citizen", "Code 2", "LEO")]

    public class DrugsFound : Callout
    {
        private static readonly string[] drugList = new string[] { "sf_prop_sf_bag_weed_01b", "bkr_prop_weed_bigbag_open_01a", "m24_1_prop_m41_weed_bigbag_01a", "sf_prop_sf_bag_weed_open_01a" };
        private static Vector3 spawnpoint;
        public Rage.Object theDrugs;
        private static Ped theCaller;
        private static Vector3 callerSpawn;
        private static Blip drugBlip;
        private static Blip callerBlip;
        private static int counter;
        private static string malefemale;
        private static readonly string[] backupList = new string[] { "s_m_y_cop_01", "s_f_y_cop_01", "csb_cop", "s_f_y_sheriff_01", "s_m_y_sheriff_01" };
        private static readonly string[] backupVehicle = new string[] { "police", "police2", "police3", "police4", "fbi", "fbi2", "sheriff", "sheriff2", "policeb" };
        private static Ped theCop;
        private static Vector3 copSpawn;
        private static Vector3 leoVehicleSpawn;
        private static Blip theCopBlip;
        private static Blip policeCarBlip;
        private static Vehicle policeVehicle;
        private static string copGender;
        private static bool isCollected = false;
        private static float callerHeading;
        private static float copHeading;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = new(979.11f, -1957.23f, 30.77f);
            callerSpawn = new(989.72f, -1945.02f, 30.99f);
            callerHeading = 199.09f;
            copSpawn = new(1000.36f, -1952.72f, 30.91f);
            copHeading = 74.75f;
            leoVehicleSpawn = new(1000.69f, -1958.26f, 30.86f);
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of illegal drugs found by a nearby citizen.");
            CalloutMessage = "Illegal drugs found";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("Adam69 Callouts [LOG]: Drugs Found callout has been accepted!");
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~y~Drugs Found", "~b~Dispatch~w~: The caller has been located. Respond ~r~Code 2~w~.");
            Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout", false);

            theCaller = new Ped(callerSpawn);
            theCaller.IsPersistent = true;
            theCaller.BlockPermanentEvents = true;
            theCaller.IsValid();

            theCaller.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);

            theDrugs = new Rage.Object("sf_prop_sf_bag_weed_open_01a", spawnpoint);
            theDrugs.IsPersistent = true;
            theDrugs.IsValid();

            callerBlip = theCaller.AttachBlip();
            callerBlip.Color = System.Drawing.Color.Orange;
            callerBlip.Alpha = 0.5f;

            drugBlip = theDrugs.AttachBlip();
            drugBlip.Color = System.Drawing.Color.Purple;
            drugBlip.IsRouteEnabled = true;

            theCop = new Ped(backupList[new Random().Next((int)backupList.Length)], copSpawn, 0f);
            theCop.IsPersistent = true;
            theCop.BlockPermanentEvents = true;
            theCop.IsValid();

            theCopBlip = theCop.AttachBlip();
            theCopBlip.Color = System.Drawing.Color.LightBlue;

            policeVehicle = new Vehicle(backupVehicle[new Random().Next((int)backupVehicle.Length)], leoVehicleSpawn, 0f);
            policeVehicle.IsPersistent = true;
            policeVehicle.IsValid();


            policeCarBlip = policeVehicle.AttachBlip();
            policeCarBlip.Color = System.Drawing.Color.DarkBlue;

            if (theCaller.IsMale)
                malefemale = "Sir";
            else
                malefemale = "Ma'am";

            if (MainPlayer.IsMale)
                copGender = "Sir";
            else
                copGender = "Ma'am";

            counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (theDrugs) theDrugs.Delete();
            if (theCaller) theCaller.Delete();
            if (drugBlip) drugBlip.Delete();
            if (callerBlip) callerBlip.Delete();
            if (theCop) theCop.Delete();
            if (theCopBlip) theCopBlip.Delete();
            if (policeVehicle) policeVehicle.Delete();
            if (policeCarBlip) policeCarBlip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if(MainPlayer.DistanceTo(theCaller) <= 5f)
            {
                Game.DisplayHelp("Press ~y~Y~w~ to interact with the caller", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                {
                    counter++;

                    if(counter == 1)
                    {
                        theCaller.Face(MainPlayer);
                        Game.DisplaySubtitle("~b~You~w~: Hello there, " + malefemale + ". Are you the caller?");
                    }
                    if(counter == 2)
                    {
                        theCaller.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~o~The Caller~w~: Yes I am, " + copGender + ".");
                    }
                    if(counter == 3)
                    {
                        theCaller.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~b~You~w~: Can you explain how did you find the drugs?");
                    }
                    if(counter == 4)
                    {
                        theCaller.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~o~The Caller~w~: I was going for a walk then I spotted this opened bag of weed from LD Organics. I didn't know who owns that bag of weed. That's why I called.");
                    }
                    if(counter == 5)
                    {
                        theCaller.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~b~You~w~: We really appreciate that you reported it. I'll investiagte this. I just need to see your ID so I know who I'm talking to.");
                    }
                    if(counter == 6)
                    {
                        theCaller.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@casino@brawl@fights@argue@"), "arguement_loop_mp_m_brawler_01", -1f, AnimationFlags.Loop);
                        Game.DisplaySubtitle("~o~The Caller~w~: Ok, no problem. I'm really in a hurry. I got to go watch Kansas City Chiefs vs Buffalo Bills game. Buffalo Bills is my team and I hope they give KC a good 'ol fashion spanking. Them cheating ass fuckers.");
                    }
                    if(counter == 7)
                    {
                        Game.DisplaySubtitle("Convo Ended. Deal with the situation you may see fit.");
                        theCaller.Tasks.PlayAnimation(new AnimationDictionary("rcmjosh1"), "idle", -1f, AnimationFlags.Loop);
                    }
                }

                if(MainPlayer.DistanceTo(theDrugs) <= 10f)
                {
                    Game.DisplayHelp("Press ~y~" + Settings.PickUp + "~w~ to pick up the drugs.", false);

                    if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                    {
                        isCollected = true;
                        MainPlayer.Tasks.PlayAnimation(new AnimationDictionary("missfbi_s4mop"), "pickup_bucket", -1f, AnimationFlags.UpperBodyOnly);
                        this.theDrugs.AttachTo(MainPlayer, 6286, Vector3.RelativeRight, Rotator.Zero);
                        GameFiber.Yield();
                        this.theDrugs.Delete();
                    }
                }
            }

            if (MainPlayer.IsDead) End();
            if (Game.IsKeyDown(System.Windows.Forms.Keys.End)) End();

            base.Process();
        }

        public override void End()
        {
            if (theDrugs) theDrugs.Delete();
            if (theCaller) theCaller.Dismiss();
            if (drugBlip) drugBlip.Delete();
            if (callerBlip) callerBlip.Delete();
            if (theCop) theCop.Dismiss();
            if (theCopBlip) theCopBlip.Delete();
            if (policeVehicle) policeVehicle.Delete();
            if (policeCarBlip) policeCarBlip.Delete();
            Game.DisplayNotification("web_adam69callouts", "web_adam69callouts", "~w~Adam69 Callouts", "~y~Drugs Found", "~b~You~w~: Dispatch, we are ~g~Code 4~w~. Show me back 10-8.");

            base.End();

            Game.LogTrivial("Adam69 Callouts [LOG]: Drugs Found callout is code 4!");
        }
    }
}