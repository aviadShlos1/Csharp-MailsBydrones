using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    partial class BL
    {

        public void GetBaseStation(int baseStationId)
        {

            return DalAccess.GetBaseStation();
        }
        //public void GetDrone();
        //public void GetCustomer();
        //public void GetParcel();
        //}

    }
}
