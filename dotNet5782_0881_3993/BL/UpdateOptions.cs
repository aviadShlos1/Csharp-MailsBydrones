using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    partial class BL
    {


        public void UpdateDroneName( int droneId, string newModel)
        {
            bool flag = false;
            foreach (var item in DalAccess.DronesListDisplay() )
            {
                if (item.Id==droneId)
                {
                    string m = item.Model;
                    m = newModel;
                    flag = true;
                }
            }
            if (!flag)
            {
                throw new IDAL.DO.NotExistException(droneId);
            }
        }
        public void UpdateBaseStationData(int baseStationId, string newName,int totalChargeSlots)
        {
            bool flag = false;
            foreach (var item in DalAccess.BaseStationsListDisplay())
            {
                if (item.Id==baseStationId)
                {
                    if (newName!= "")
                    {
                        string temp = item.Name;
                        temp = newName;
                        flag = true;
                    }
                    
                    if (totalChargeSlots!=0)
                    {
                        item.FreeChargeSlots = totalChargeSlots - DalAccess.                    }
                }
            }
            if (!flag)
            {
                throw new IDAL.DO.NotExistException(baseStationId);
            }
        }
        //    public void UpdateCustomerData();
        //    public void DroneToCharge();
        //    public void ReleaseDroneCharge();
        //    public void AssignParcelToDrone();
        //    public void PickUpParcel();
        //    public void SupplyParcel();
        //}
    }
}
