using System;

namespace ConsoleUI_BL
{
    class Program
    {
        #region Enums
        enum Options { Add = 1, Update, SingleDisplay, ListDisplay, Exit }

        /// <summary> enum for AddOption</summary>
        enum AddOptions { AddBaseStation = 1, AddDrone, AddCustomer, AddParcel }

        /// <summary> enum for UpdatesOption</summary>
        enum UpdatesOption { UpdateDroneName = 1, UpdateBaseStationData, UpdateCustomerData, DroneToCharge, ReleaseDroneCharge, AssignParcelToDrone, PickUpParcel, SupplyParcel }

        /// <summary> enum for SingleOptionDisplay </summary>
        enum SingleDisplayOptions { BaseStationDisplay = 1, DroneDisplay, CustomerDisplay, ParcelDisplay }

        /// <summary> enum for ListOptionDisplay </summary>
        enum ListDisplayOption { BaseStationsList = 1, DronesList, CustomersList, ParcelsList, ParcelsWithoutDrone, StationsWithFreeChargeSlots }
        #endregion Enums
        static void Main(string[] args)
        {
            int action;
            Console.ReadLine()
            int add;
            IBL.IBL bl = new IBL.BL();
            switch (action)
            {
                case Add:
                    {
                        switch (add)
                        {
                            case AddBaseStation:
                                break;
                            case AddDrone:
                                break;
                            case AddCustomer:
                                break;
                            case AddParcel:
                                break;
                            default:
                                break;
                        }
                    }
                default:
            }
            
        }
    }
}
