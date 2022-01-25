using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
        // [MethodImpl(MethodImplOptions.Synchronized)] attribute is used to ensure that  only one thread at a time can executs function, uses instance of class object
        // calling method to lock, locks entire function that attribute is added to.

        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
            if (status!= ParcelStatus.Ordered)
                throw new RemoveException("delivery was proccessed, cannot remove parcel");
            myDal.RemoveParcel(prc);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDroneCharge(int id)
        {
            DO.DroneCharge dc;
            try
            {
                dc = myDal.GetDroneCharge(id);
            }
            catch (Exception ex)
            {

                throw new RemoveException("", ex);
            }
            myDal.RemoveDroneCharge(dc);
        }
    }
}
