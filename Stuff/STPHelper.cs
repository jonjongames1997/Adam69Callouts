using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adam69Callouts.Stuff
{
    // Credit to Yobbin for his Open Source code as a reference. github.com/YobB1n/YobbinCallouts/blob/master/StopThePedHelper.cs

    internal class StopThePedFunctions
    {
        internal static void SetPedUnderTheInfluence(Ped ped, bool underInfluence)
        {
            try
            {
                StopThePed.API.Functions.setPedAlcoholOverLimit(ped, underInfluence);
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }
        internal static void SetPedUnderDrugInfluence(Ped ped, bool overDrugInfluence)
        {
            try
            {
                StopThePed.API.Functions.setPedUnderDrugsInfluence(ped, overDrugInfluence);
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }
        internal static void injectPedItems(Ped ped)
        {
            try
            {
                StopThePed.API.Functions.injectPedSearchItems(ped);
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }
        internal static void InjectVehicleItems(Vehicle vehicle)
        {
            try
            {
                StopThePed.API.Functions.injectVehicleSearchItems(vehicle);
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }
        internal static void CallTowService()
        {
            try
            {
                StopThePed.API.Functions.callTowService();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }
        internal static void callAnimalControl()
        {
            try
            {
                StopThePed.API.Functions.callAnimalControl();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callCoronerService()
        {
            try
            {
                StopThePed.API.Functions.callCoroner();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callInsurance()
        {
            try
            {
                StopThePed.API.Functions.callInsuranceService();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void callPoliceTransport()
        {
            try
            {
                StopThePed.API.Functions.callPoliceTransport();
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
        }

        internal static void requestPIT()
        {
            try
            {
                StopThePed.API.Functions.requestPIT();
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
