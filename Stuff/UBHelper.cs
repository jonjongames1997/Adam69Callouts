using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adam69Callouts.Stuff
{
    // Credit to Yobbin for his Open Source code as a reference. github.com/YobB1n/YobbinCallouts/blob/master/UltimateBackupHelper.cs

    internal class UltimateBackupFunctions
    {
        internal static void CallPursuitBackup()
        {
            try
            {
                UltimateBackup.API.Functions.callPursuitBackup();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callCode3BackUp()
        {
            try
            {
                UltimateBackup.API.Functions.callCode3Backup();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callCode3SwatBackup(bool radio = false, bool isNooseUnit = false)
        {
            try
            {
                UltimateBackup.API.Functions.callCode3SwatBackup(radio, isNooseUnit);
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callFireDepartment()
        {
            try
            {
                UltimateBackup.API.Functions.callFireDepartment();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callTrafficStopBackup(bool radio = false, bool isLocalPatrol = false)
        {
            try
            {
                UltimateBackup.API.Functions.callTrafficStopBackup(radio, isLocalPatrol);
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callPanicBackup(bool radio = false, bool isLocalPatrol = false)
        {
            try
            {
                UltimateBackup.API.Functions.callPanicButtonBackup(true);
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callGroupBackup(bool radio = false, bool isLocalPatrol = false)
        {
            try
            {
                UltimateBackup.API.Functions.callGroupBackup(true);
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callK9BackUp()
        {
            try
            {
                UltimateBackup.API.Functions.callK9Backup();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callFelonyBackUp()
        {
            try
            {
                UltimateBackup.API.Functions.callFelonyStopBackup();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callSpikeStripBackup()
        {
            try
            {
                UltimateBackup.API.Functions.callSpikeStripsBackup();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callFemaleBackup()
        {
            try
            {
                UltimateBackup.API.Functions.callFemaleBackup();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callCode2Backup(bool radio = false, bool isLocalPatrol = false)
        {
            try
            {
                UltimateBackup.API.Functions.callCode2Backup(radio, isLocalPatrol);
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callRoadBlockBackup()
        {
            try
            {
                UltimateBackup.API.Functions.callRoadBlockBackup();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }
    }
}
