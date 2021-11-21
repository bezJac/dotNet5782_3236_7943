using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using BL.BO;

namespace BL
{
    public partial class BL :IBL.IBL
    {
        public BaseStation GetBaseStation(int id)
        {
            IDAL.DO.BaseStation st;
            try
            {
                st = myDal.GetBaseStation(id);
            }
            catch (Exception ex)
            {
                throw new BaseStationException("Base Station Exception: ", ex);
            }
            return convertToBaseStation(st);
        }
        public Drone GetDrone(int id)
        {
            IDAL.DO.Drone dr;
            try
            {
                dr = myDal.GetDrone(id);
            }
            catch (Exception ex)
            {
                throw new DroneException("Drone Exception: ", ex);
            }
            return convertToDrone(Drones.Find(drone => drone.Id == dr.Id));
        }
        public DroneInParcel GetDroneInParcel(int id)
        {
            DroneInList dr = Drones.Find(dr => dr.Id == id);
            return new DroneInParcel
            {
                Id = dr.Id,
                Battery = dr.Battery,
                DroneLocation = dr.DroneLocation,
            };
        }
        public Customer GetCustomer(int id)
        {
            IDAL.DO.Customer cstmr;
            try
            {
                cstmr = myDal.GetCustomer(id);
            }
            catch (Exception ex)
            {
                throw new CustomerException("Customer Exception: ", ex);
            }
            return convertToCustomer(cstmr);
        }
        public Parcel GetParcel(int id)
        {
            IDAL.DO.Parcel prc;
            try
            {
                prc = myDal.GetParcel(id);
            }
            catch (Exception ex)
            {
                throw new ParcelException("Parcel Exception: ", ex);
            }
            return convertToParcel(prc);
        }
        public CustomerInParcel GetCustomerInParcel(int id)
        {
            IDAL.DO.Customer cstmr = myDal.GetCustomer(id);
            return new CustomerInParcel
            {
                Id = cstmr.Id,
                Name = cstmr.Name,
            };
        }
        public ParcelInDelivery GetParcelInDelivery(int id)
        {
            IDAL.DO.Parcel parcel;
            IDAL.DO.Customer sender;
            IDAL.DO.Customer target;
            try
            {
                parcel = myDal.GetParcel(id);
            }
            catch (Exception ex)
            {
                throw new ParcelException("BL - Parcel Exception: ", ex);
            }
            try
            {
                sender = myDal.GetCustomer(parcel.SenderId);
                target = myDal.GetCustomer(parcel.TargetId);
            }
            catch (Exception ex)
            {

                throw new CustomerException("BL - CustomerException: ", ex);
            }
            bool flag = false;
            if (getParcelStatus(parcel) == ParcelStatus.PickedUp)
                flag = true;
            Location senderLocation = createLocation(sender.Longitude, sender.Lattitude);
            Location targetLocation = createLocation(target.Longitude, target.Lattitude);
            return new ParcelInDelivery
            {
                Id = parcel.Id,
                Status = flag,
                Priority = (Priority)parcel.Priority,
                Weight = (WeightCategories)parcel.Weight,
                Sender = GetCustomerInParcel(parcel.SenderId),
                Target = GetCustomerInParcel(parcel.TargetId),
                SenderLocation = senderLocation,
                TargetLocation = targetLocation,
                DeliveryDistance = Distance.GetDistance(senderLocation, targetLocation),
            };

        }
    }
}
