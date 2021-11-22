using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    /// <summary>
    /// partial class manages all get single instance related methods for BL
    /// </summary>
    public partial class BL :IBL.IBL
    {
        /// <summary>
        /// get a base station by its ID
        /// </summary>
        /// <param name="id"> base stations's ID </param>
        /// <returns> BL BaseStation instance matching the ID </returns>
        public BaseStation GetBaseStation(int id)
        {
            IDAL.DO.BaseStation st;
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

        /// <summary>
        /// get a drone by its ID
        /// </summary>
        /// <param name="id"> drone's ID </param>
        /// <returns> BL Drone instance matching the ID </returns>
        public Drone GetDrone(int id)
        {
            IDAL.DO.Drone dr;
            try
            {
                dr = myDal.GetDrone(id);
            }
            catch (Exception ex)
            {
                throw new GetInstanceException("", ex);
            }
            return convertToDrone(Drones.Find(drone => drone.Id == dr.Id));
        }

        /// <summary>
        /// get a representation of a drone in a parcel by its ID
        /// </summary>
        /// <param name="id"> drone's ID </param>
        /// <returns> BL DroneInParcel instance of drone matching the ID </returns>
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

        /// <summary>
        /// get a customer by his ID
        /// </summary>
        /// <param name="id"> customer's ID </param>
        /// <returns> BL Customer instance matching the ID </returns>
        public Customer GetCustomer(int id)
        {
            IDAL.DO.Customer cstmr;
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

        /// <summary>
        /// get a parcel by its ID
        /// </summary>
        /// <param name="id"> parcel's ID </param>
        /// <returns> Bl Parcel Instance matching the ID</returns>
        public Parcel GetParcel(int id)
        {
            IDAL.DO.Parcel prc;
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

        /// <summary>
        /// get customerInParcel representation of a customer by his ID 
        /// </summary>
        /// <param name="id"> customer's ID</param>
        /// <returns> BL CustomerInParcel instance  matching the ID</returns>
        public CustomerInParcel GetCustomerInParcel(int id)
        {
            IDAL.DO.Customer cstmr = myDal.GetCustomer(id);
            return new CustomerInParcel
            {
                Id = cstmr.Id,
                Name = cstmr.Name,
            };
        }

        /// <summary>
        ///  get ParcelInDelivery representation of a parcel by its  ID 
        /// </summary>
        /// <param name="id"> parcel's ID </param>
        /// <returns> Bl ParcelInDelivery instance matching the ID </returns>
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
