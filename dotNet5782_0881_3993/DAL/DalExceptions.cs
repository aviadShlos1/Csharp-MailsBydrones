using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IDAL.DO
{
    
    [Serializable]
    public class NotExistException : Exception
    {
        public int ID;
        public NotExistException(int id) : base() => ID = id;
        public NotExistException(string msg) : base() { }
        public NotExistException(int id, string message) : base(message) => ID = id;
        public NotExistException(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $",The id does not exist:{ID}";

    }
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

 