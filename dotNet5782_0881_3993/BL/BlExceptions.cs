//Names: Aviad Shlosberg       314960881      
///       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IBL.BO
{
    /// <summary>
    ///This exception will be thrown when the object isn't exist
    /// </summary>
    [Serializable]
    public class NotExistException : Exception
    {
        public int ID;
        public NotExistException() : base() { }
        public NotExistException(string msg) { Console.WriteLine(msg); }
        public NotExistException(int id, string message, Exception innerException) : base(message, innerException) => ID =((IDAL.DO.NotExistException)innerException).ID;
        public override string ToString() => base.ToString() + $",The id does not exist";

    }

    /// <summary>
    /// This exception will be thrown when the object is already exist
    /// </summary>
    [Serializable]
    public class AlreadyExistException : Exception
    {
        public int ID;
        public AlreadyExistException() : base() { }
        public AlreadyExistException(int id, string message, Exception innerException) : base(message, innerException) => ID = ((IDAL.DO.AlreadyExistException)innerException).ID;
        public override string ToString() => base.ToString() + $", The id is already exists";

    }

    /// <summary>
    ///This Exception will be thrown when the drone cannot go to charge
    /// </summary>
    [Serializable]
    public class CannotGoToChargeException:Exception
    {
        public int DroneID;       
        public CannotGoToChargeException(int id) : base() => DroneID = id;
        public CannotGoToChargeException(int id, string message) : base(message) => DroneID = id;
        public CannotGoToChargeException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", This drone cannot go to charge";

    }

    /// <summary>
    /// This Exception will be thrown when the drone cannot release from charging
    /// </summary>
    [Serializable]
    public class CannotReleaseFromChargeException : Exception
    {
        public int DroneID;
        public CannotReleaseFromChargeException(int id) : base() => DroneID = id;
        public CannotReleaseFromChargeException(int id, string message) : base(message) => DroneID = id;
        public CannotReleaseFromChargeException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", This drone cannot release from charge";

    }

    /// <summary>
    /// This Exception will be thrown when the drone cannot assign to parcel
    /// </summary>
    [Serializable]
    public class CannotAssignDroneToParcelException : Exception
    {
        public int DroneID;
        public CannotAssignDroneToParcelException(int id) : base() => DroneID = id;
        public CannotAssignDroneToParcelException(int id, string message) : base(message) => DroneID = id;
        public CannotAssignDroneToParcelException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", This drone cannot be assigned to this parcel:";
    }

    /// <summary>
    /// This Exception will be thrown when the drone is not available now from some reasons
    /// </summary>
    [Serializable]
    public class DroneIsNotAvailable : Exception
    {
        public int DroneID;
        public DroneIsNotAvailable(int id) : base() => DroneID = id;
        public DroneIsNotAvailable(int id, string message) : base(message) => DroneID = id;
        public DroneIsNotAvailable(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", The drone is not available now:";
    }

    /// <summary>
    /// This Exception will be thrown when the station haven't enough charge slots 
    /// </summary>
    [Serializable]
    public class NotEnoughChargeSlotsInThisStation : Exception
    {
        public int stationId;
        public NotEnoughChargeSlotsInThisStation(int id) : base() => stationId = id;
        public NotEnoughChargeSlotsInThisStation(int id, string message) : base(message) => stationId = id;
        public NotEnoughChargeSlotsInThisStation(int id, string message, Exception innerException) : base(message, innerException) => stationId = id;
        public override string ToString() => base.ToString() + $", There are'nt enough charge slots in this station";
    }

    /// <summary>
    /// This Exception will be thrown when the drone cannot pick up a parcel from some reasons 
    /// </summary>
    [Serializable]
    public class CannotPickUpException : Exception
    {
        public int DroneID;
        public CannotPickUpException(int id) : base() => DroneID = id;
        public CannotPickUpException(string msg) { Console.WriteLine(msg); }
        public CannotPickUpException(int id, string message) : base(message) => DroneID = id;
        public CannotPickUpException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", The drone cannot pick up the parcel";
    }

    /// <summary>
    /// This Exception will be thrown when the drone cannot supply a parcel from some reasons 
    /// </summary>
    [Serializable]
    public class CannotSupplyException : Exception
    {
        public int DroneID;
        public CannotSupplyException(int id) : base() => DroneID = id;
        public CannotSupplyException(string msg) { Console.WriteLine(msg); }
        public CannotSupplyException(int id, string message) : base(message) => DroneID = id;
        public CannotSupplyException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", The drone cannot supply the parcel";
    }

    /// <summary>
    /// This Exception will be thrown when there don't have stations with free charge slots 
    /// </summary>
    [Serializable]
    public class NoStationsWithFreeChargeException : Exception
    {
        public int firstChargeStation;
        public NoStationsWithFreeChargeException() : base() { }
        public NoStationsWithFreeChargeException(int id) : base() => firstChargeStation = id;
        public NoStationsWithFreeChargeException(int id, string message) : base(message) => firstChargeStation = id;
        public NoStationsWithFreeChargeException(int id, string message, Exception innerException) : base(message, innerException) => firstChargeStation = id;
        public override string ToString() => base.ToString() + $", This station doesn't have enough slots for charge";

    }
}
