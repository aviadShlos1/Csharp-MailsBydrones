using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IDAL.DO
{
[Serializable]
    public class DroneIdException:Exception
    {
        public int ID;
        public DroneIdException(int id) : base() => ID = id;
        public DroneIdException(int id,string message) : base(message) => ID = id;
        public DroneIdException(int id, string message,Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", invalid drone id:{ID}";
        
    }
 [Serializable]
    public class ParcelIdException : Exception
    {
        public int ID;
        public ParcelIdException(int id) : base() => ID = id;
        public ParcelIdException(int id, string message) : base(message) => ID = id;
        public ParcelIdException(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", invalid parcel id:{ID}";

    }
 [Serializable]
    public class BaseStationIdException : Exception
    {
        public int ID;
        public BaseStationIdException(int id) : base() => ID = id;
        public BaseStationIdException(int id, string message) : base(message) => ID = id;
        public BaseStationIdException(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", invalid base station id:{ID}";

    }
 [Serializable]
    public class CustomerIdException : Exception
    {
        public int ID;
        public CustomerIdException(int id) : base() => ID = id;
        public CustomerIdException(int id, string message) : base(message) => ID = id;
        public CustomerIdException(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", invalid customer id:{ID}";

    }
    [Serializable]
    public class CustomerPhoneException : Exception
    {
        public int Phone;
        public CustomerPhoneException(int phone) : base() => Phone = phone;
        public CustomerPhoneException(int phone, string message) : base(message) => Phone = phone;
        public CustomerPhoneException(int phone, string message, Exception innerException) : base(message, innerException) => Phone = phone;
        public override string ToString() => base.ToString() + $", invalid customer phone:{Phone}";

    }
}
