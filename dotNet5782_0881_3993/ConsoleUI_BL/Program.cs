using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBL;
using IBL.BO;

namespace ConsoleUI_BL
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
    class Program
    {
        
        #region MainFunctions 

        #region Add options
        /// <summary>
        /// The function handles various addition options.
        /// </summary>
        /// <param name="dal"> DalObject object is a parameter which enables access to the DalObject class functions</param>
        static public void AddOptions(IBL.IBL bl)
        {
            Console.WriteLine(@"Add options:
1. BaseStationBl
2. DroneBl
3. CustomerBl
4. ParcelBl
Your choice:");

            int.TryParse(Console.ReadLine(), out int choice);

            switch ((AddOptions)choice)
            {
                // Adding a new station
                case ConsoleUI_BL.AddOptions.AddBaseStation:
                    int newStationID, newchargsSlots;
                    string newName;                   
                    double newLongitude= default, newLatitude=default;
                    Location newLocation = new() { Longitude = newLongitude, Latitude = newLatitude };
                    // User input for a new station
                    Console.WriteLine(@"
You selected to add a station.
Please enter an ID number for the station:(0-4)");
                    while (!int.TryParse(Console.ReadLine(), out newStationID)) ;
                    Console.WriteLine("Please enter the name of the station:");
                    newName = Console.ReadLine();
                    Console.WriteLine("Please enter the number of charging slots at the station:");
                    while (!int.TryParse(Console.ReadLine(), out newchargsSlots)) ;
                    Console.WriteLine("Please enter the longitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out newLongitude)) ;
                    Console.WriteLine("Please enter the latitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out newLatitude)) ;
                    Console.WriteLine();

                    BaseStationBl newBaseStation = new BaseStationBl
                    {
                        Id = newStationID,
                        BaseStationName = newName,
                        FreeChargeSlots = newchargsSlots,
                        Location = newLocation,
                        DronesInChargeList = new()
                    };
                    bl.AddBaseStation(newStationID, newName, newLocation, newchargsSlots);
                    break;

                // Adding a new drone
                case ConsoleUI_BL.AddOptions.AddDrone:
                    int newDroneID, newMaxWeight;
                    string newModel;

                    // User input for a new drone
                    Console.WriteLine(@"
You selected to add a DroneDal.
Please enter an ID number for the drone(1000-9999):");
                    while (!int.TryParse(Console.ReadLine(), out newDroneID)) ;
                    Console.WriteLine("Please enter the model of the drone:(model ***) ");
                    newModel = Console.ReadLine();
                    Console.WriteLine("Please enter the weight category of the drone: 0 for light, 1 for medium and 2 for heavy");
                    while (!int.TryParse(Console.ReadLine(), out newMaxWeight)) ;
                    Console.WriteLine();

                    DroneDal newdrone = new DroneDal
                    {
                        Id = newDroneID,
                        Model = newModel,
                        DroneWeight = (WeightCategoriesDal)newMaxWeight,
                    };
                    dal.AddDrone(newdrone);
                    break;

                // Adding a new customer
                case ConsoleUI_BL.AddOptions.AddCustomer:
                    int newCustomerID;
                    string newCustomerName, newPhoneNumber;
                    double newCustomerLongitude=default, newCustomerLatitude = default;
                    Location newCustomerLocation = new() { Longitude = newCustomerLongitude, Latitude = newCustomerLatitude };
                    // User input for a new customer
                    Console.WriteLine(@"
You selected to add a CustomerDal.
Please enter an ID number for the CustomerDal(9 digits):");
                    while (!int.TryParse(Console.ReadLine(), out newCustomerID)) ;
                    Console.WriteLine("Please enter the name of the customer:");
                    newCustomerName = Console.ReadLine();
                    Console.WriteLine("Please enter the phone number of the customer:");
                    newPhoneNumber = Console.ReadLine();
                    Console.WriteLine("Please enter the longitude of the customer city:");
                    while (!double.TryParse(Console.ReadLine(), out newCustomerLongitude)) ;
                    Console.WriteLine("Please enter the Latitude of the customer city:");
                    while (!double.TryParse(Console.ReadLine(), out newCustomerLatitude)) ;
                    Console.WriteLine();

                    CustomerBL newCustomer = new CustomerBL
                    {
                        CustomerId = newCustomerID,
                        CustomerName = newCustomerName,
                        CustomerPhone = newPhoneNumber,
                        CustomerLocation  = newCustomerLocation,               
                    };
                    bl.AddCustomer(newCustomer);
                    break;

                // Adding a new parcel
                case ConsoleUI_BL.AddOptions.AddParcel:
                    int newParcelId, newSenderId, newTargetId, newWeight, newPriorities;
                    // User input for a new parcel
                    Console.WriteLine(@"
You selected to add a ParcelDal.
Please enter the ParcelDal ID (0-1000):");
                    while (!int.TryParse(Console.ReadLine(), out newParcelId)) ;
                    Console.WriteLine("Please enter the sender ID number(9 digits):");
                    while (!int.TryParse(Console.ReadLine(), out newSenderId)) ;
                    Console.WriteLine("Please enter the target ID number(9 digits):");
                    while (!int.TryParse(Console.ReadLine(), out newTargetId)) ;
                    Console.WriteLine("Please enter the weight category of the parcel: 0 for free, 1 for maintenance and 2 for delievery");
                    while (!int.TryParse(Console.ReadLine(), out newWeight)) ;
                    Console.WriteLine("Please enter the priorities of the new parcel: 0 for normal, 1 for fast and 2 for urgent");
                    while (!int.TryParse(Console.ReadLine(), out newPriorities)) ;
                    Console.WriteLine();
                    //
                    ParcelBl newParcel = new ParcelBl
                    {
                        ParcelId = newParcelId,
                        Sender = newSenderId,
                        TargetId = newTargetId,
                        Weight = (WeightCategoriesDal)newWeight,
                        Priority = (Priorities)newPriorities,
                        DroneToParcelId = 0,
                        CreatingTime = DateTime.Now,
                        AssignningTime = DateTime.Now,
                        PickingUpTime = DateTime.Now,
                        SupplyingTime = DateTime.Now
                    };

                    int parcelCounter = dal.AddParcel(newParcel);
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
        /// <param name="dal"> DalObject object that enables access to the DalObject class functions </param>
        static public void UpdateOptions(IBL.IBL blObject)
        {
            Console.WriteLine(@"Update options:
1. Attributing a drone to a parcel
2. Picking up the parcel by the drone
3. Deliever the package to the customer
4. Sending a drone for battery charging 
5. Release drone from charging at base station
Your choice:");
            int.TryParse(Console.ReadLine(), out int choice);

            int ParcelId, DroneId, StationId;

            switch ((UpdatesOption)choice)
            {

                case UpdatesOption.ConnectDroneToParcel:
                    Console.WriteLine("please enter a parcel ID(0-1000):");
                    int.TryParse(Console.ReadLine(), out ParcelId);
                    Console.WriteLine("please enter a drone ID(4 digits):");
                    int.TryParse(Console.ReadLine(), out DroneId);
                    dal.ConnectDroneToParcel(ParcelId, DroneId);
                    break;

                case UpdatesOption.PickUpParcel:
                    Console.WriteLine("please enter a parcel ID(0-1000):");
                    int.TryParse(Console.ReadLine(), out ParcelId);
                    dal.PickUpParcel(ParcelId);
                    break;

                case UpdatesOption.DelieverParcel:
                    Console.WriteLine("please enter a parcel ID(0-1000):");
                    int.TryParse(Console.ReadLine(), out ParcelId);
                    dal.DelieverParcel(ParcelId);
                    break;

                case UpdatesOption.DroneToCharge:
                    Console.WriteLine("please enter a drone ID(4 digits):");
                    int.TryParse(Console.ReadLine(), out DroneId);
                    Console.WriteLine("please choose stationId ID from the List below:");
                    IEnumerable<BaseStationDal> FreeChargSlots = dal.GetStationsWithFreeCharge();
                    foreach (var item in FreeChargSlots)
                    {
                        Console.WriteLine(item);
                    }
                    int.TryParse(Console.ReadLine(), out StationId);
                    dal.DroneToCharge(DroneId, StationId);
                    break;

                case UpdatesOption.DroneRelease:
                    Console.WriteLine("please enter a drone ID(4 digits):");
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
        /// The function handles single display options.
        /// </summary>
        /// <param name="dal"> DalObject object is a parameter which enables access to the DalObject class functions</param>
        static public void DisplaySingleOptions(IBL.IBL blObject)
        {
            Console.WriteLine(@"Single display options:
1. Base station display
2. DroneDal display
3. CustomerDal display
4. ParcelDal display

Your choice:");
            int.TryParse(Console.ReadLine(), out int choice);

            int displayObjectId;

            switch ((SingleDisplayOptions)choice)
            {
                // Single station display
                case SingleDisplayOptions.StationDisplay:
                    Console.WriteLine("Add the requested station ID(0-4):");
                    int.TryParse(Console.ReadLine(), out displayObjectId);
                    Console.WriteLine(dal.GetSingleBaseStation(displayObjectId).ToString());
                    break;
                // Single drone display
                case SingleDisplayOptions.DroneDisplay:
                    Console.WriteLine("Add the requested drone ID(4 digits):");
                    int.TryParse(Console.ReadLine(), out displayObjectId);
                    Console.WriteLine(dal.GetSingleDrone(displayObjectId).ToString());
                    break;
                // Single customer display
                case SingleDisplayOptions.CustomerDisplay:
                    Console.WriteLine("Add the requested customer ID(9 digits):");
                    int.TryParse(Console.ReadLine(), out displayObjectId);
                    Console.WriteLine(dal.GetSingleCustomer(displayObjectId).ToString());
                    break;
                // Single parcel display
                case SingleDisplayOptions.ParcelDisplay:
                    Console.WriteLine("Add the requested parcel ID(0-1000):");
                    int.TryParse(Console.ReadLine(), out displayObjectId);
                    Console.WriteLine(dal.GetSingleParcel(displayObjectId).ToString());
                    break;

                default:
                    break;
            }
        }
        #endregion Display options

        #region List display options
        /// <summary>
        /// The function handles list display options.
        /// </summary>
        /// <param name="dal"> DalObject object is a parameter which enables access to the DalObject class functions</param>

        static public void DisplayListOptions(IBL.IBL blObject)
        {
            Console.WriteLine(@"List display options:
1. Base stations list 
2. Drones list 
3. Customers list 
4. Parcels list 
5. Parcels which haven't been assigned to a drone
6. Available charging stations
Your choice:");
            int.TryParse(Console.ReadLine(), out int choice);

            switch ((ListDisplayOption)choice)
            {
                // BaseStations list display
                case ListDisplayOption.StationsList:
                    IEnumerable<BaseStationDal> displayStationsList = dal.GetBaseStationsList();
                    foreach (var item in displayStationsList)
                    {
                        Console.WriteLine(item);
                    }
                    break;

                // Drones list display
                case ListDisplayOption.DronesList:
                    IEnumerable<DroneDal> displayDronesList = dal.GetDronesList();
                    foreach (var item in displayDronesList)
                    {
                        Console.WriteLine(item);
                    }
                    break;

                // Customers list display
                case ListDisplayOption.CustomersList:
                    IEnumerable<CustomerDal> displayCustomersList = dal.GetCustomersList();

                    foreach (var item in displayCustomersList)
                    {
                        Console.WriteLine(item);
                    }
                    break;

                // Parcels list display
                case ListDisplayOption.ParcelsList:
                    IEnumerable<ParcelDal> displayParcelsList = dal.GetParcelsList();

                    foreach (var item in displayParcelsList)
                    {
                        Console.WriteLine(item);
                    }
                    break;

                // GetParcelsWithoutDrone list display
                case ListDisplayOption.ParcelsWithoutDrone:
                    IEnumerable<ParcelDal> displayParcelsWithoutDrone = dal.GetParcelsWithoutDrone();

                    foreach (var item in displayParcelsWithoutDrone)
                    {
                        Console.WriteLine(item);
                    }
                    break;

                // FreeChargeSlots list display
                case ListDisplayOption.FreeChargeSlotsList:
                    IEnumerable<BaseStationDal> displayStationsWithFreeChargeSlots = dal.GetStationsWithFreeCharge();

                    foreach (var item in displayStationsWithFreeChargeSlots)
                    {
                        Console.WriteLine(item);
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
            
            IBL.IBL blObject = new IBL.BL();

            Options options;
            int choice = 0;
            do
            {
                Console.WriteLine(@"Hello guest, the program offers you the following options (select number): 
1. Adding 
2. Updating
3. Single display 
4. List display 
5. Exit

Your choice:");
                while (!int.TryParse(Console.ReadLine(), out choice)) ;
                options = (Options)choice;

                switch (options)
                {
                    case Options.Add:
                        AddOptions(blObject);
                        break;

                    case Options.Update:
                        UpdateOptions(blObject);
                        break;

                    case Options.SingleDisplay:
                        DisplaySingleOptions(blObject);
                        break;

                    case Options.ListDisplay:
                        DisplayListOptions(blObject);
                        break;
                    case Options.Exit:
                        Console.WriteLine("Thanks for using our system, see you soon");
                        break;

                    default:
                        break;
                }
            } while (!(choice == 5));
        }

    }
    
}
