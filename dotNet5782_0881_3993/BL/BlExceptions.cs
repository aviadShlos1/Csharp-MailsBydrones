using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IBL.BO
{
    [Serializable]
    public class NotExistException : Exception
    {
        public int ID;
        public NotExistException() : base() { }
        public NotExistException(int id, string message, Exception innerException) : base(message, innerException) => ID =((IDAL.DO.NotExistException)innerException).ID;
        public override string ToString() => base.ToString() + $",The id does not exist:{ID}";

    }
    [Serializable]
    public class AlreadyExistException : Exception
    {
        public int ID;
        public AlreadyExistException() : base() { }
        public AlreadyExistException(int id, string message, Exception innerException) : base(message, innerException) => ID = ((IDAL.DO.AlreadyExistException)innerException).ID;
        public override string ToString() => base.ToString() + $", The id is already exists:{ID}";

    }
    [Serializable]
    public class CannotGoToChargeException:Exception
    {
        public int DroneID;       
        public CannotGoToChargeException(int id) : base() => DroneID = id;
        public CannotGoToChargeException(int id, string message) : base(message) => DroneID = id;
        public CannotGoToChargeException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", This drone cannot go to charge:{DroneID}";

    }
    [Serializable]
    public class CannotReleaseFromChargeException : Exception
    {
        public int DroneID;
        public CannotReleaseFromChargeException(int id) : base() => DroneID = id;
        public CannotReleaseFromChargeException(int id, string message) : base(message) => DroneID = id;
        public CannotReleaseFromChargeException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", This drone cannot release from charge:{DroneID}";

    }

    [Serializable]
    public class CannotAssignDroneToParcelException : Exception
    {
        public int DroneID;
        public CannotAssignDroneToParcelException(int id) : base() => DroneID = id;
        public CannotAssignDroneToParcelException(int id, string message) : base(message) => DroneID = id;
        public CannotAssignDroneToParcelException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", This drone cannot be assigned to a parcel:{DroneID}";
    }

    [Serializable]
    public class NotEnoughBatteryException : Exception
    {
        public int DroneID;
        public NotEnoughBatteryException(int id) : base() => DroneID = id;
        public NotEnoughBatteryException(int id, string message) : base(message) => DroneID = id;
        public NotEnoughBatteryException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", The drone doesn't have enough battery for this mission:{DroneID}";
    }

    [Serializable]
    public class CannotPickUpException : Exception
    {
        public int DroneID;
        public CannotPickUpException(int id) : base() => DroneID = id;
        public CannotPickUpException(string msg) { Console.WriteLine(msg); }
        public CannotPickUpException(int id, string message) : base(message) => DroneID = id;
        public CannotPickUpException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", The drone cannot pick up the parcel:{DroneID}";
    }
    [Serializable]
    public class CannotSupplyException : Exception
    {
        public int DroneID;
        public CannotSupplyException(int id) : base() => DroneID = id;
        public CannotSupplyException(string msg) { Console.WriteLine(msg); }
        public CannotSupplyException(int id, string message) : base(message) => DroneID = id;
        public CannotSupplyException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", The drone cannot supply the parcel:{DroneID}";
    }
    [Serializable]
    public class NoStationsWithFreeChargeException : Exception
    {
        public int firstChargeStation;
        public NoStationsWithFreeChargeException() : base() { }
        public NoStationsWithFreeChargeException(int id) : base() => firstChargeStation = id;
        public NoStationsWithFreeChargeException(int id, string message) : base(message) => firstChargeStation = id;
        public NoStationsWithFreeChargeException(int id, string message, Exception innerException) : base(message, innerException) => firstChargeStation = id;
        public override string ToString() => base.ToString() + $", This station doesn't have enough slots for charge:{firstChargeStation}";

    }
}
