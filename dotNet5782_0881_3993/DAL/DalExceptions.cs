//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil1
//brief: In this program we built the data access layer

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IDAL.DO
{

    // An exceptions class for the data access layer

    /// <summary>
    ///This exception will be thrown when the object isn't exist
    /// </summary>
    [Serializable]
    public class NotExistException : Exception
    {
        public int ID;

        public NotExistException(int id) : base() => ID = id;
        public NotExistException(int id, string message) : base(message) => ID = id;
        public NotExistException(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $",The id does not exist:{ID}";

    }
    /// <summary>
    /// This exception will be thrown when the object is already exist
    /// </summary>
    [Serializable]
    public class AlreadyExistException : Exception
    {
        public int ID;
        public AlreadyExistException(int id) : base() => ID = id;
        public AlreadyExistException(int id, string message) : base(message) => ID = id;
        public AlreadyExistException(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", The id is already exists:{ID}";

    }
}

 