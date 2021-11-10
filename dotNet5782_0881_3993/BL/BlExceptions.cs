using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL.BO
{
    [Serializable]
    public class BaseStationIdException:Exception
    {
        public int ID;
        
    }
}
