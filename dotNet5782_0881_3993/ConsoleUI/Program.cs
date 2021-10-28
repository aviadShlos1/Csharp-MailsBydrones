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
            Console.WriteLine(@"Add options:
1. Station
2. Drone
3. Customer
4. Parcel
Your choice:");
    
            int.TryParse(Console.ReadLine(), out int choice);
            
            switch ((AddOptions)choice)
            {
                case AddOptions.AddStation:
                    int newStationID, newchargsSlots;
                    string newName;
                    double newLongitude, newLattitude;

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

                    Station newStation = new Station
                    {
                        Id = newStationID,
                        Name = newName,
                        ChargeSlots = newchargsSlots,
                        Longitude = newLongitude,
                        Lattitude = newLattitude
                    };
                    dal.AddStation(newStation);
                    break;

                case AddOptions.AddDrone:
                    int newDroneID, newMaxWeight, newStatus;
                    string newModel;
                    double newBatteryLevel;

                    Console.WriteLine(@"
You selected to add a Drone.
Please enter an ID number for the drone(4 digits):");
                    while (!int.TryParse(Console.ReadLine(), out newDroneID)) ;
                    Console.WriteLine("Please enter the model of the Drone:");
                    newModel = Console.ReadLine();
                    Console.WriteLine("Please enter the weight category of the Drone: 0 for light, 1 for medium and 2 for heavy");
                    while (!int.TryParse(Console.ReadLine(), out newMaxWeight)) ;
                    Console.WriteLine("Please enter the charge level of the battery:");
                    while (!double.TryParse(Console.ReadLine(), out newBatteryLevel)) ;
                    Console.WriteLine("Please enter the status of the Drone: 0 for free, 1 for maintenance and 2 for delievery");
                    while (!int.TryParse(Console.ReadLine(), out newStatus)) ;
                    Console.WriteLine();

                    Drone newdrone = new Drone
                    {
                        Id = newDroneID,
                        Model = newModel,
                        MaxWeight = (WeightCategories)newMaxWeight,
                        Battery = newBatteryLevel,
                        Status = (DroneStatuses)newStatus
                    };
                    dal.AddDrone(newdrone);
                    break;

                case AddOptions.AddCustomer:
                    int newCustomerID;
                    string newCustomerName, newPhoneNumber;
                    double newCustomerLongitude, newCustomerLattitude;

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
                        CustomerLattitude = newCustomerLattitude
                    };
                    dal.AddCustomer(newCustomer);
                    break;

                case AddOptions.AddParcel:
                    int newParcelId, newSenderId, newTargetId, newWeight, newPriorities;

                    Console.WriteLine(@"
You selected to add a Parcel.
Please enter the Parcel ID (0-1000):");
                    while (!int.TryParse(Console.ReadLine(), out newParcelId)) ;
                    Console.WriteLine("Please enter the sender ID number(9 digits):");
                    while (!int.TryParse(Console.ReadLine(), out newSenderId)) ;
                    Console.WriteLine("Please enter the target ID number(9 digits):");
                    while (!int.TryParse(Console.ReadLine(), out newTargetId)) ;
                    Console.WriteLine("Please enter the weight category of the Parcel: 0 for free, 1 for maintenance and 2 for delievery");
                    while (!int.TryParse(Console.ReadLine(), out newWeight)) ;
                    Console.WriteLine("Please enter the priorities of the new Parcel: 0 for normal, 1 for fast and 2 for urgent");
                    while (!int.TryParse(Console.ReadLine(), out newPriorities)) ;
                    Console.WriteLine();

                    Parcel newParcel = new Parcel
                    {
                        Id = newParcelId,
                        SenderId = newSenderId,
                        TargetId = newTargetId,
                        Weight = (WeightCategories)newWeight,
                        Priority = (Priorities)newPriorities
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
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
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
                    Console.WriteLine("please enter a parcel ID:");
                    int.TryParse(Console.ReadLine(), out ParcelId);
                    Console.WriteLine("please enter a drone ID:");
                    int.TryParse(Console.ReadLine(), out DroneId);
                    dal.ConnectDroneToParcel(ParcelId, DroneId);
                    break;

                case UpdatesOption.PickUpParcel:
                    Console.WriteLine("please enter a parcel ID:");
                    int.TryParse(Console.ReadLine(), out ParcelId);
                    dal.PickUpParcel(ParcelId);
                    break;

                case UpdatesOption.DelieverParcel:
                    Console.WriteLine("please enter a parcel ID:");
                    int.TryParse(Console.ReadLine(), out ParcelId);

                    dal.DelieverParcel(ParcelId);
                    break;

                case UpdatesOption.DroneToCharge:
                    Console.WriteLine("please enter a drone ID:");
                    int.TryParse(Console.ReadLine(), out DroneId);
                    Console.WriteLine("please choose StationId ID from the List below:");
                    List<Station> FreeChargSlots = dal.FreeChargeSlotsList();
                    for (int i = 0; i < FreeChargSlots.Count; i++)
                    {
                        Console.WriteLine(FreeChargSlots[i].ToString());
                    }
                    int.TryParse(Console.ReadLine(), out StationId);
                    dal.DroneToCharge(DroneId, StationId);
                    break;

                case UpdatesOption.DroneRelease:
                    Console.WriteLine("please enter a Drone ID:");
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
            Console.WriteLine(@"Single display options:
1. Base station display
2. Drone display
3. Customer display
4. Parcel display

Your choice:");
            int.TryParse(Console.ReadLine(), out int choice);

            int idForViewObject;

            switch ((SingleDisplayOption)choice)
            {
                case SingleDisplayOption.StationDisplay:
                    Console.WriteLine("Add the requested station ID:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);

                    Console.WriteLine(dal.StationDisplay(idForViewObject).ToString());
                    break;

                case SingleDisplayOption.DroneDisplay:
                    Console.WriteLine("Add the requested drone ID:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);

                    Console.WriteLine(dal.DroneDisplay(idForViewObject).ToString());
                    break;

                case SingleDisplayOption.CustomerDisplay:
                    Console.WriteLine("Add the requested customer IDr:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);

                    Console.WriteLine(dal.CustomerDisplay(idForViewObject).ToString());
                    break;

                case SingleDisplayOption.ParcelDisplay:
                    Console.WriteLine("Add the requested parcel ID:");
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
                case ListDisplayOption.StationsList:
                    List<Station> displayStationsList = dal.StationsList();

                    for (int i = 0; i < displayStationsList.Count; i++)
                    {
                        Console.WriteLine(displayStationsList[i].ToString());
                    }
                    break;

                case ListDisplayOption.DronesList:
                    List<Drone> displayDronesList = dal.DronesList();

                    for (int i = 0; i < displayDronesList.Count; i++)
                    {
                        Console.WriteLine(displayDronesList[i].ToString());
                    }
                    break;

                case ListDisplayOption.CustomersList:
                    List<Customer> displayCustomersList = dal.CustomersList();

                    for (int i = 0; i < displayCustomersList.Count; i++)
                    {
                        Console.WriteLine(displayCustomersList[i].ToString());
                    }
                    break;

                case ListDisplayOption.ParcelsList:
                    List<Parcel> displayParcelsList = dal.ParcelsList();

                    for (int i = 0; i < displayParcelsList.Count(); i++)
                    {
                        Console.WriteLine(displayParcelsList[i].ToString());
                    }
                    break;

                case ListDisplayOption.ParcelsWithoutDrone:
                    List<Parcel> displayParcelsWithoutDrone = dal.ParcelsWithoutDrone();

                    for (int i = 0; i < displayParcelsWithoutDrone.Count(); i++)
                    {
                        Console.WriteLine(displayParcelsWithoutDrone[i].ToString());
                    }
                    break;

                case ListDisplayOption.FreeChargeSlotsList:
                    List<Station> displayStationsWithFreeChargeSlots = dal.FreeChargeSlotsList();

                    for (int i = 0; i < displayStationsWithFreeChargeSlots.Count(); i++)
                    {
                        Console.WriteLine(displayStationsWithFreeChargeSlots[i].ToString());
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
5. Exit

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
