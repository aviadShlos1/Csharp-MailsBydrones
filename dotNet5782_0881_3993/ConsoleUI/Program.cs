//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;

namespace ConsoleUI
{
    #region Enums
    /// <summary>
    enum Options { Add = 1, Update, SingleDisplay, ListDisplay, Distance, EXIT }
    /// <summary> enum for AddOption</summary>
    enum AddOptions { AddStation = 1, AddDrone, AddCustomer, AddParcel }
    /// <summary> enum for UpdatesOption</summary>
    enum UpdatesOption { ConnectDroneToParcel = 1, PickUpParcel, DelieverParcel, DroneToCharge, DroneRelease }
    /// <summary>enum for DisplaySingleOption </summary>
    enum SingleDisplayOption { StationDisplay = 1, DroneDisplay, CustomerDisplay, ParcelDisplay }
    /// <summary>enum for DisplayListOption </summary>
    enum ListDisplayOption
    {
        StationsList = 1, DronesList, CustomersList, ParcelsList, ParcelsWithoutDrone, FreeChargeSlotsList
    }
    #endregion Enums

    ///<summary> main class </summary> 
    class Program
    {
        #region MainFunctions 

        #region Add options
        /// <summary>
        /// The function handles various addition options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void InsertOptions(DalObject.DalObject dal)
        {
            Console.WriteLine(@"
Insert options:

1. Add a base station to the list of stations.
2. Add a drone to the list of existing drones.
3. Admission of a new customer to the customer list.
4. Admission of package for shipment.

Your choice:");
            int.TryParse(Console.ReadLine(), out int choice);

            switch ((AddOptions)choice)
            {
                case AddOptions.AddStation:
                    int newBaseStationID, newchargsSlots;
                    string newname;
                    double newlongitude, newlatitude;

                    Console.WriteLine(@"
You have selected to add a new station.
Please enter an ID number for the new station:");
                    while (!int.TryParse(Console.ReadLine(), out newBaseStationID)) ;
                    Console.WriteLine("Next Please enter the name of the station:");
                    newname = Console.ReadLine();
                    Console.WriteLine("Next Please enter the number of charging slots at the station:");
                    while (!int.TryParse(Console.ReadLine(), out newchargsSlots)) ;
                    Console.WriteLine("Next Please enter the longitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out newlongitude)) ;
                    Console.WriteLine("Next Please enter the latitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out newlatitude)) ;

                    Station newbaseStation = new Station
                    {
                        Id = newBaseStationID,
                        Name = newname,
                        ChargeSlots = newchargsSlots,
                        Longitude = newlongitude,
                        Lattitude = newlatitude
                    };
                    dal.AddStation(newbaseStation);
                    break;

                case AddOptions.AddDrone:
                    int newDroneID, newMaxWeight, newStatus;
                    string newmodel;
                    double newBatteryLevel;

                    Console.WriteLine(@"
You have selected to add a new Drone.
Please enter an ID number for the new drone:");
                    while (!int.TryParse(Console.ReadLine(), out newDroneID)) ;
                    Console.WriteLine("Next Please enter the model of the Drone:");
                    newmodel = Console.ReadLine();
                    Console.WriteLine("Next enter the weight category of the new Drone: 0 for light, 1 for medium and 2 for heavy");
                    while (!int.TryParse(Console.ReadLine(), out newMaxWeight)) ;
                    Console.WriteLine("Next enter the charge level of the battery:");
                    while (!double.TryParse(Console.ReadLine(), out newBatteryLevel)) ;
                    Console.WriteLine("Next enter the status of the new Drone: 0 for free, 1 for inMaintenance and 2 for busy");
                    while (!int.TryParse(Console.ReadLine(), out newStatus)) ;

                    Drone newdrone = new Drone
                    {
                        Id = newDroneID,
                        Model = newmodel,
                        MaxWeight = (WeightCategories)newMaxWeight,
                        Battery = newBatteryLevel,
                        Status = (DroneStatuses)newStatus
                    };
                    dal.AddDrone(newdrone);
                    break;

                case AddOptions.AddCustomer:
                    int newCustomerID;
                    string newCustomerName, newPhoneNumber;
                    double newCustomerLongitude, newCustomerLatitude;

                    Console.WriteLine(@"
You have selected to add a new Customer.
Please enter an ID number for the new Customer:");
                    while (!int.TryParse(Console.ReadLine(), out newCustomerID)) ;
                    Console.WriteLine("Next Please enter the name of the customer:");
                    newCustomerName = Console.ReadLine();
                    Console.WriteLine("Next enter the phone number of the new customer:");
                    newPhoneNumber = Console.ReadLine();
                    Console.WriteLine("Next Please enter the longitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out newCustomerLongitude)) ;
                    Console.WriteLine("Next Please enter the latitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out newCustomerLatitude)) ;

                    Customer newCustomer = new Customer
                    {
                        Id = newCustomerID,
                        Name = newCustomerName,
                        Phone = newPhoneNumber,
                        Longitude = newCustomerLongitude,
                        Lattitude = newCustomerLatitude
                    };
                    dal.AddCustomer(newCustomer);
                    break;

                case AddOptions.AddParcel:
                    int newSenderId, newTargetId, newWeight, newPriorities;

                    Console.WriteLine(@"
You have selected to add a new Parcel.
Next Please enter the sender ID number:");
                    while (!int.TryParse(Console.ReadLine(), out newSenderId)) ;
                    Console.WriteLine("Next Please enter the target ID number:");
                    while (!int.TryParse(Console.ReadLine(), out newTargetId)) ;
                    Console.WriteLine("Next enter the weight category of the new Parcel: 0 for free, 1 for inMaintenance and 2 for busy");
                    while (!int.TryParse(Console.ReadLine(), out newWeight)) ;
                    Console.WriteLine("Next enter the priorities of the new Parcel: 0 for regular, 1 for fast and 2 for urgent");
                    while (!int.TryParse(Console.ReadLine(), out newPriorities)) ;

                    Parcel newParcel = new Parcel
                    {
                        SenderId = newSenderId,
                        TargetId = newTargetId,
                        Weight = (WeightCategories)newWeight,
                        Priority = (Priorities)newPriorities
                    };

                    int counterParcelSerialNumber = dal.AddParcel(newParcel);
                    break;

                default:
                    break;
            }
        }
        #endregion Add options

        #region Update options
        /// <summary>
        /// The function handles various update options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void UpdateOptions(DalObject.DalObject dal)
        {
            Console.WriteLine(@"Update options:
            1. Attributing a drone to a parcel
            2. picking up the parcel by the drone
            3. Deliever the package to the customer
            4. Sending a drone for battery charging 
            5. Release drone from charging at base station
            Your choice:");
            int.TryParse(Console.ReadLine(), out int choice);

            int ParcelId, DroneId, StationId;

            switch ((UpdatesOption)choice)
            {

                case UpdatesOption.ConnectDroneToParcel:
                    Console.WriteLine("please enter the parcel ID:");
                    int.TryParse(Console.ReadLine(), out ParcelId);
                    Console.WriteLine("please enter the drone ID:");
                    int.TryParse(Console.ReadLine(), out DroneId);
                    dal.ConnectDroneToParcel(ParcelId, DroneId);
                    break;

                case UpdatesOption.PickUpParcel:
                    Console.WriteLine("please enter the parcel ID:");
                    int.TryParse(Console.ReadLine(), out ParcelId);
                    dal.PickUpParcel(ParcelId);
                    break;

                case UpdatesOption.DelieverParcel:
                    Console.WriteLine("please enter the parcel ID:");
                    int.TryParse(Console.ReadLine(), out ParcelId);

                    dal.DelieverParcel(ParcelId);
                    break;

                case UpdatesOption.DroneToCharge:
                    Console.WriteLine("please enter the drone ID:");
                    int.TryParse(Console.ReadLine(), out DroneId);
                    Console.WriteLine("please choose baseStationId ID from the List below:");
                    List<Station> FreeChargSlots = dal.FreeChargeSlotsList();
                    for (int i = 0; i < FreeChargSlots.Count; i++)
                    {
                        Console.WriteLine(FreeChargSlots[i].ToString());
                    }
                    int.TryParse(Console.ReadLine(), out StationId);
                    dal.DroneToCharge(StationId, DroneId);
                    break;

                case UpdatesOption.DroneRelease:
                    Console.WriteLine("please enter the Drone ID:");
                    int.TryParse(Console.ReadLine(), out DroneId);
                    dal.DroneRelease(DroneId);
                    break;

                default:
                    break;
            }
        }
        #endregion  Update options

        #region Display options
        /// <summary>
        /// The function handles display options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void DisplaySingleOptions(DalObject.DalObject dal)
        {
            Console.WriteLine(@"
Display options(single):

1. Base station view.
2. Drone display.
3. Customer view.
4. Package view.

Your choice:");
            int.TryParse(Console.ReadLine(), out int choice);

            int idForViewObject;

            switch ((SingleDisplayOption)choice)
            {
                case SingleDisplayOption.StationDisplay:
                    Console.WriteLine("Insert ID number of base station:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);

                    Console.WriteLine(dal.StationDisplay(idForViewObject).ToString());
                    break;

                case SingleDisplayOption.DroneDisplay:
                    Console.WriteLine("Insert ID number of requsted drone:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);

                    Console.WriteLine(dal.DroneDisplay(idForViewObject).ToString());
                    break;

                case SingleDisplayOption.CustomerDisplay:
                    Console.WriteLine("Insert ID number of requsted Customer:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);

                    Console.WriteLine(dal.CustomerDisplay(idForViewObject).ToString());
                    break;

                case SingleDisplayOption.ParcelDisplay:
                    Console.WriteLine("Insert ID number of reqused parcel:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);

                    Console.WriteLine(dal.ParcelDisplay(idForViewObject).ToString());
                    break;

                default:
                    break;
            }
        }
        #endregion Display options

        #region List display options
        /// <summary>
        /// The function handles list view options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void DisplayListOptions(DalObject.DalObject dal)
        {
            Console.WriteLine(@"
            List display options:
            1. Base stations list 
            2. Drones list 
            3. Customers list 
            4. Parcels list 
            5. Parcels which haven't been assigned to a drone.
            6. Available charging stations.
            Your choice:");
            int.TryParse(Console.ReadLine(), out int choice);

            switch ((ListDisplayOption)choice)
            {
                case ListDisplayOption.StationsList:
                    List<Station> displayBaseList = dal.StationsList();

                    for (int i = 0; i < displayBaseList.Count; i++)
                    {
                        Console.WriteLine(displayBaseList[i].ToString());
                    }
                    break;

                case ListDisplayOption.DronesList:
                    List<Drone> displayDroneList = dal.DronesList();

                    for (int i = 0; i < displayDroneList.Count; i++)
                    {
                        Console.WriteLine(displayDroneList[i].ToString());
                    }
                    break;

                case ListDisplayOption.CustomersList:
                    List<Customer> displayCustomerList = dal.CustomersList();

                    for (int i = 0; i < displayCustomerList.Count; i++)
                    {
                        Console.WriteLine(displayCustomerList[i].ToString());
                    }
                    break;

                case ListDisplayOption.ParcelsList:
                    List<Parcel> displayPackageList = dal.ParcelsList();

                    for (int i = 0; i < displayPackageList.Count(); i++)
                    {
                        Console.WriteLine(displayPackageList[i].ToString());
                    }
                    break;

                case ListDisplayOption.ParcelsWithoutDrone:
                    List<Parcel> displayParcelWithoutDrone = dal.ParcelsWithoutDrone();

                    for (int i = 0; i < displayParcelWithoutDrone.Count(); i++)
                    {
                        Console.WriteLine(displayParcelWithoutDrone[i].ToString());
                    }
                    break;

                case ListDisplayOption.FreeChargeSlotsList:
                    List<Station> displayBaseStationWithFreeChargSlots = dal.FreeChargeSlotsList();

                    for (int i = 0; i < displayBaseStationWithFreeChargSlots.Count(); i++)
                    {
                        Console.WriteLine(displayBaseStationWithFreeChargSlots[i].ToString());
                    }
                    break;

                default:
                    break;
            }

        }
        #endregion  List display options

        #endregion MainFunctions

        static void Main(string[] args)
        {
            DalObject.DalObject dalObject = new DalObject.DalObject();

            Options options;
            int choice = 0;
            do
            {
                Console.WriteLine(@"Hello guest, the program offers you the following options (select number): 
                1. Adding 
                2. Updating
                3. Single displaying 
                4. List displaying 
                5. Exit.
                Your choice:");
                while (!int.TryParse(Console.ReadLine(), out choice)) ;
                options = (Options)choice;

                switch (options)
                {
                    case Options.Add:
                        InsertOptions(dalObject);
                        break;

                    case Options.Update:
                        UpdateOptions(dalObject);
                        break;

                    case Options.SingleDisplay:
                        DisplaySingleOptions(dalObject);
                        break;

                    case Options.ListDisplay:
                        DisplayListOptions(dalObject);
                        break;
                    case Options.EXIT:
                        Console.WriteLine("Thanks for using our system, see you soon");
                        break;

                    default:
                        break;
                }
            } while (!(choice == 5));
        }
    }

}
