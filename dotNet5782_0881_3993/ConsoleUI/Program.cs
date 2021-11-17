//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil1


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

    /// <summary> enum for SingleOptionDisplay </summary>
    enum SingleDisplayOptions { StationDisplay = 1, DroneDisplay, CustomerDisplay, ParcelDisplay }

    /// <summary> enum for ListOptionDisplay </summary>
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
        /// <param name="dal"> DalObject object is a parameter which enables access to the DalObject class functions</param>
        static public void AddOptions(DalObject.DalObject dal)
        {
            Console.WriteLine(@"Add options:
1. BaseStation
2. Drone
3. Customer
4. Parcel
Your choice:");
    
            int.TryParse(Console.ReadLine(), out int choice);
            
            switch ((AddOptions)choice)
            {
                    // Adding a new station
                case ConsoleUI.AddOptions.AddStation:
                    int newStationID, newchargsSlots;
                    string newName;
                    double newLongitude, newLattitude;
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
                    Console.WriteLine("Please enter the lattitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out newLattitude)) ;
                    Console.WriteLine();

                    BaseStation newStation = new BaseStation
                    {
                        Id = newStationID,
                        Name = newName,
                        ChargeSlots = newchargsSlots,
                        Longitude = newLongitude,
                        Latitude = newLattitude
                    };
                    dal.AddStation(newStation);
                    break;

                    // Adding a new drone
                case ConsoleUI.AddOptions.AddDrone:
                    int newDroneID, newMaxWeight;
                    string newModel;
                  
                    // User input for a new drone
                    Console.WriteLine(@"
You selected to add a Drone.
Please enter an ID number for the drone(1000-9999):");
                    while (!int.TryParse(Console.ReadLine(), out newDroneID)) ;
                    Console.WriteLine("Please enter the model of the drone:(model ***) ");
                    newModel = Console.ReadLine();
                    Console.WriteLine("Please enter the weight category of the drone: 0 for light, 1 for medium and 2 for heavy");
                    while (!int.TryParse(Console.ReadLine(), out newMaxWeight)) ;
                    Console.WriteLine();

                    Drone newdrone = new Drone
                    {
                        Id = newDroneID,
                        Model = newModel,
                        MaxWeight = (WeightCategoriesDal)newMaxWeight,
                    };
                    dal.AddDrone(newdrone);
                    break;

                    // Adding a new customer
                case ConsoleUI.AddOptions.AddCustomer:
                    int newCustomerID;
                    string newCustomerName, newPhoneNumber;
                    double newCustomerLongitude, newCustomerLattitude;
                    // User input for a new customer
                    Console.WriteLine(@"
You selected to add a Customer.
Please enter an ID number for the Customer(9 digits):");
                    while (!int.TryParse(Console.ReadLine(), out newCustomerID)) ;
                    Console.WriteLine("Please enter the name of the customer:");
                    newCustomerName = Console.ReadLine();
                    Console.WriteLine("Please enter the phone number of the customer:");
                    newPhoneNumber = Console.ReadLine();
                    Console.WriteLine("Please enter the longitude of the customer city:");
                    while (!double.TryParse(Console.ReadLine(), out newCustomerLongitude)) ;
                    Console.WriteLine("Please enter the lattitude of the customer city:");
                    while (!double.TryParse(Console.ReadLine(), out newCustomerLattitude)) ;
                    Console.WriteLine();

                    Customer newCustomer = new Customer
                    {
                        Id = newCustomerID,
                        Name = newCustomerName,
                        Phone = newPhoneNumber,
                        CustomerLongitude = newCustomerLongitude,
                        CustomerLatitude = newCustomerLattitude
                    };
                    dal.AddCustomer(newCustomer);
                    break;

                    // Adding a new parcel
                case ConsoleUI.AddOptions.AddParcel:
                    int newParcelId, newSenderId, newTargetId, newWeight, newPriorities;
                    // User input for a new parcel
                    Console.WriteLine(@"
You selected to add a Parcel.
Please enter the Parcel ID (0-1000):");
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
                    Parcel newParcel = new Parcel
                    {
                        Id = newParcelId,
                        SenderId = newSenderId,
                        TargetId = newTargetId,
                        Weight = (WeightCategoriesDal)newWeight,
                        Priority = (Priorities)newPriorities,
                        DroneToParcel_Id = 0,
                        Created = DateTime.Now,
                        Assigned = DateTime.Now,
                        PickedUp = DateTime.Now,
                        Supplied = DateTime.Now
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
        static public void UpdateOptions(DalObject.DalObject dal)
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
                    IEnumerable<BaseStation> FreeChargSlots = dal.FreeChargeSlotsList();
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
        static public void DisplaySingleOptions(DalObject.DalObject dal)
        {
            Console.WriteLine(@"Single display options:
1. Base station display
2. Drone display
3. Customer display
4. Parcel display

Your choice:");
            int.TryParse(Console.ReadLine(), out int choice);

            int displayObjectId;

            switch ((SingleDisplayOptions)choice)
            {
                // Single station display
                case SingleDisplayOptions.StationDisplay:
                    Console.WriteLine("Add the requested station ID(0-4):");
                    int.TryParse(Console.ReadLine(), out displayObjectId);
                    Console.WriteLine(dal.BaseStationDisplay(displayObjectId).ToString());
                    break;
                // Single drone display
                case SingleDisplayOptions.DroneDisplay:
                    Console.WriteLine("Add the requested drone ID(4 digits):");
                    int.TryParse(Console.ReadLine(), out displayObjectId);
                    Console.WriteLine(dal.DroneDisplay(displayObjectId).ToString());
                    break;
                // Single customer display
                case SingleDisplayOptions.CustomerDisplay:
                    Console.WriteLine("Add the requested customer ID(9 digits):");
                    int.TryParse(Console.ReadLine(), out displayObjectId);
                    Console.WriteLine(dal.CustomerDisplay(displayObjectId).ToString());
                    break;
                // Single parcel display
                case SingleDisplayOptions.ParcelDisplay:
                    Console.WriteLine("Add the requested parcel ID(0-1000):");
                    int.TryParse(Console.ReadLine(), out displayObjectId);
                    Console.WriteLine(dal.ParcelDisplay(displayObjectId).ToString());
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

        static public void DisplayListOptions(DalObject.DalObject dal)
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
                    IEnumerable <BaseStation> displayStationsList = dal.BaseStationsListDisplay();
                    foreach (var item in displayStationsList)
                    {
                        Console.WriteLine(item);
                    }
                    break;

                // Drones list display
                case ListDisplayOption.DronesList:
                    IEnumerable<Drone> displayDronesList = dal.DronesListDisplay();
                    foreach (var item in displayDronesList)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                   
                // Customers list display
                case ListDisplayOption.CustomersList:
                    IEnumerable<Customer> displayCustomersList = dal.CustomersListDisplay();

                    foreach (var item in displayCustomersList)
                    {
                        Console.WriteLine(item);
                    }
                    break;

                // Parcels list display
                case ListDisplayOption.ParcelsList:
                    IEnumerable<Parcel> displayParcelsList = dal.ParcelsListDisplay();

                    foreach (var item in displayParcelsList)
                    {
                        Console.WriteLine(item);
                    }
                    break;

                // ParcelsWithoutDrone list display
                case ListDisplayOption.ParcelsWithoutDrone:
                    IEnumerable<Parcel> displayParcelsWithoutDrone = dal.ParcelsWithoutDrone();

                    foreach (var item in displayParcelsWithoutDrone)
                    {
                        Console.WriteLine(item);
                    }
                    break;

                // FreeChargeSlots list display
                case ListDisplayOption.FreeChargeSlotsList:
                    IEnumerable<BaseStation> displayStationsWithFreeChargeSlots = dal.FreeChargeSlotsList();

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
            DalObject.DalObject dalObject = new DalObject.DalObject();
            IDAL.IDal idal = new DalObject.DalObject();
           
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
                        AddOptions(dalObject);
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
