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
        public override string ToString() => base.ToString() + $", This drone {DroneID} cannot be assigned to this parcel:";
    }

    [Serializable]
    public class DroneIsNotAvailable : Exception
    {
        public int DroneID;
        public DroneIsNotAvailable(int id) : base() => DroneID = id;
        public DroneIsNotAvailable(int id, string message) : base(message) => DroneID = id;
        public DroneIsNotAvailable(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", The drone {DroneID} is not available now:";
    }

    [Serializable]
    public class CannotPickUpException : Exception
    {
        public int DroneID;
        public CannotPickUpException(int id) : base() => DroneID = id;
        public CannotPickUpException(int id, string message) : base(message) => DroneID = id;
        public CannotPickUpException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", The drone cannot pick up the parcel:{DroneID}";
    }
    [Serializable]
    public class CannotSupplyException : Exception
    {
        public int DroneID;
        public CannotSupplyException(int id) : base() => DroneID = id;
        public CannotSupplyException(int id, string message) : base(message) => DroneID = id;
        public CannotSupplyException(int id, string message, Exception innerException) : base(message, innerException) => DroneID = id;
        public override string ToString() => base.ToString() + $", The drone cannot supply the parcel:{DroneID}";
    }

    [Serializable]
    public class NotEnoughChargeSlotsInThisStation : Exception
    {
        public int stationId;
        public NotEnoughChargeSlotsInThisStation() : base() { }
        public NotEnoughChargeSlotsInThisStation(int id, string message, Exception innerException) : base(message, innerException) => stationId = ((IDAL.DO.NotExistException)innerException).ID;
        public override string ToString() => base.ToString() + $",There are few charge slots in this station :{stationId}";

    }
}
