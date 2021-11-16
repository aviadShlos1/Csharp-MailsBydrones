using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IDAL.DO
{
    [Serializable]
    public class InvalidIdException : Exception
    {
        public int ID;
        public InvalidIdException(int id) : base() => ID = id;
        public InvalidIdException(int id, string message) : base(message) => ID = id;
        public InvalidIdException(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", invalid id:{ID}";

    }
    [Serializable]
    public class NotExistException : Exception
    {
        public int ID;
        public NotExistException(int id) : base() => ID = id;
        public NotExistException(int id, string message) : base(message) => ID = id;
        public NotExistException(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $",id is not exist:{ID}";

    }
    [Serializable]
    public class AlreadyExistIdException : Exception
    {
        public int ID;
        public AlreadyExistIdException(int id) : base() => ID = id;
        public AlreadyExistIdException(int id, string message) : base(message) => ID = id;
        public AlreadyExistIdException(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", The id is already exist:{ID}";

    }
}

 