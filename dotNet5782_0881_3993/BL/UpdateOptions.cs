using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    partial class BL
    {


        public void UpdateDroneName(int droneId, string newModel)
        {

            foreach (var item in DalAccess.DronesListDisplay())
            {
                if (item.Id == droneId)
                {
                    string m = item.Model;
                    m = newModel;

                }
            }

        }
        public void UpdateBaseStationData(int baseStationId, string newName, int totalChargeSlots)
        {

            foreach (var item in DalAccess.BaseStationsListDisplay())
            {
                if (item.Id == baseStationId)
                {
                    if (newName != "")
                    {
                        string temp = item.Name;
                        temp = newName;

                    }
                    if (totalChargeSlots != 0)
                    {
                        int dronesInCharge = 0;
                        foreach (var item2 in DronesListBL)
                        {
                            if (item2.DroneStatus == BO.DroneStatus.Maintaince)
                            {
                                dronesInCharge++;
                            }
                        }
                        int free = item.FreeChargeSlots;
                        free = totalChargeSlots - dronesInCharge;
                    }
                }
            }

        }
        public void UpdateCustomerData(int myId, string newName, string newPhone)
        {
            foreach (var item in DalAccess.CustomersListDisplay())
            {
                if (item.Id == myId)
                {
                    if (newName != "")
                    {
                        string m = item.Name;
                        m = newName;
                    }
                    if (newPhone != "")
                    {
                        string p = item.Phone;
                        p = newPhone;
                    }
                }
            }
        }
        public void DroneToCharge(int myDroneId)
        {
            var element = DronesListBL.Find(x => x.DroneId == myDroneId);

            if (element != default && element.DroneStatus != BO.DroneStatus.Free)
            {
                throw new BO.CannotGoToChargeException(myDroneId);
            }
            else
            {
                element.DroneStatus = BO.DroneStatus.Maintaince;
            }

        }
        //    public void ReleaseDroneCharge();
        //    public void AssignParcelToDrone();
        //    public void PickUpParcel();
        //    public void SupplyParcel();
        //}
    }
}
