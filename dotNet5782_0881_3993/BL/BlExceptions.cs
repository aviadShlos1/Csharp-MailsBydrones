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
        public NotExistException(int id, string message, Exception innerException) : base(message, innerException) => ID =((IDAL.DO.NotExistException)innerException).ID;
        public override string ToString() => base.ToString() + $",The id does not exist:{ID}";

    }
    [Serializable]
    public class AlreadyExistException : Exception
    {
        public int ID;
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
}
