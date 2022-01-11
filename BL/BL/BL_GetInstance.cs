using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation GetBaseStation(int id)
        {
            DO.BaseStation st;
            lock (myDal)
            {
                try
                {
                    st = myDal.GetBaseStation(id);
                }
                catch (Exception ex)
                {
                    throw new GetInstanceException("", ex);
                }
                return convertToBaseStation(st);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int id)
        {
            DO.Drone dr;
            lock (myDal)
            {
                try
                {
                    dr = myDal.GetDrone(id);
                }
                catch (Exception ex)
                {
                    throw new GetInstanceException("", ex);
                }
                return convertToDrone(drones.Find(drone => drone.Id == dr.Id));
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneInParcel GetDroneInParcel(int id)
        {
            DroneInList dr = drones.Find(dr => dr.Id == id);
            return new DroneInParcel
            {
                Id = dr.Id,
                Battery = dr.Battery,
                DroneLocation = dr.DroneLocation,
            };
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int id)
        {
            DO.Customer cstmr;
            lock (myDal)
            {
                try
                {
                    cstmr = myDal.GetCustomer(id);
                }
                catch (Exception ex)
                {
                    throw new GetInstanceException("", ex);
                }
                return convertToCustomer(cstmr);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int id)
        {
            DO.Parcel prc;
            lock (myDal)
            {
                try
                {
                    prc = myDal.GetParcel(id);
                }
                catch (Exception ex)
                {
                    throw new GetInstanceException("", ex);
                }
                return convertToParcel(prc);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public CustomerInParcel GetCustomerInParcel(int id)
        {
            lock (myDal)
            {
                DO.Customer cstmr = myDal.GetCustomer(id);
                return new CustomerInParcel
                {
                    Id = cstmr.Id,
                    Name = cstmr.Name,
                };
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ParcelInDelivery GetParcelInDelivery(int id)
        {
            DO.Parcel parcel;
            DO.Customer sender;
            DO.Customer target;
            DroneInList drone;
            lock (myDal)
            {
                try
                {
                    parcel = myDal.GetParcel(id);
                }
                catch (Exception ex)
                {
                    throw new GetInstanceException("", ex);
                }
                try
                {
                    sender = myDal.GetCustomer(parcel.SenderId);
                    target = myDal.GetCustomer(parcel.TargetId);
                }
                catch (Exception ex)
                {

                    throw new GetInstanceException("", ex);
                }
                try
                {
                    drone  = drones.Find(dr=> dr.ParcelId== parcel.Id);
                }
                catch (Exception ex)
                {

                    throw new GetInstanceException("", ex);
                }
                // parcel's in transit status initialized by flag 
                bool flag = false;
                // if parcel was already picked up set flag  to true 
                if (getParcelStatus(parcel) == ParcelStatus.PickedUp)
                    flag = true;

                Location senderLocation = createLocation(sender.Longitude, sender.Lattitude);
                Location targetLocation = createLocation(target.Longitude, target.Lattitude);
                return flag switch
                {
                    true => new ParcelInDelivery
                    {
                        Id = parcel.Id,
                        InTransit = flag,
                        Priority = (Priority)parcel.Priority,
                        Weight = (WeightCategories)parcel.Weight,
                        Sender = GetCustomerInParcel(parcel.SenderId),
                        Target = GetCustomerInParcel(parcel.TargetId),
                        SenderLocation = senderLocation,
                        TargetLocation = targetLocation,
                        DeliveryDistance = Distance.GetDistance(senderLocation, targetLocation),
                    },

                    false => new ParcelInDelivery
                    {
                        Id = parcel.Id,
                        InTransit = flag,
                        Priority = (Priority)parcel.Priority,
                        Weight = (WeightCategories)parcel.Weight,
                        Sender = GetCustomerInParcel(parcel.SenderId),
                        Target = GetCustomerInParcel(parcel.TargetId),
                        SenderLocation = senderLocation,
                        TargetLocation = targetLocation,
                        DeliveryDistance = Distance.GetDistance(drone.DroneLocation, senderLocation),
                    }

                };
            }


        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneInList GetDroneFromList(int? id)
        {
            return (from dr in drones
                    where dr.Id == id
                    select dr).FirstOrDefault();
        }
    }
}
