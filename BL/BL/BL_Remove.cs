using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
namespace BL
{
    /// <summary>
    /// partial class, manages all remove related methods for BL
    /// </summary>
    public partial class BL : BlApi.IBL
    {
        public   void removeDrone(int id)
        {
            DO.Drone dr;
            try
            {
                dr = myDal.GetDrone(id);
            }
            catch (Exception ex)
            {

                throw new RemoveException("", ex);
            }
            DroneInList drone = drones.FirstOrDefault(d => d.Id == dr.Id);
            if (drone.ParcelId != null)
                throw new RemoveException("drone is currently busy with delivery and cannot be removed at the momment");
            myDal.RemoveDrone(dr);
        }

        public void removeCustomer(int id)
        {
            DO.Customer cstmr;
            try
            {
                cstmr = myDal.GetCustomer(id);
            }
            catch (Exception ex)
            {

                throw new RemoveException("", ex);
            }
            CustomerInList cs = convertToCustomerInList(convertToCustomer(cstmr));
            if (cs.SentCount-cs.DeliveredCount > 0 || cs.ExpectedCount-cs.RecievedCount > 0)
                throw new RemoveException("customer waiting on delivery proccesses,\ncannot be removed at the momment");
            if(cs.DeliveredCount>0||cs.RecievedCount>0)
                throw new RemoveException("customer details appear in other entities,\ncannot be removed at the momment");
            myDal.RemoveCustomer(cstmr);
        }

        public void RemoveBaseStation(int id)
        {
            DO.BaseStation st;
            try
            {
                st = myDal.GetBaseStation(id);
            }
            catch (Exception ex)
            {

                throw new RemoveException("", ex);
            }
            BaseStation station = convertToBaseStation(st);
            if (station.DronesCharging.Any())
                throw new RemoveException("drones currently charging at station, station cannot be removed");
            myDal.RemoveBaseStation(st);
        }
        public void RemoveParcel(int id)
        {
            DO.Parcel prc;
            try
            {
                prc = myDal.GetParcel(id);
            }
            catch (Exception ex)
            {

                throw new RemoveException("", ex);
            }
            ParcelStatus status = getParcelStatus(prc);
            if (status==ParcelStatus.Linked || status == ParcelStatus.PickedUp)
                throw new RemoveException("delivery proccess started, cannot remove parcel");
            myDal.RemoveParcel(prc);
        }
    }
}
