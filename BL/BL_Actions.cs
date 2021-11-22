using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    /// <summary>
    /// partial class manages all action related methods for BL
    /// </summary>
    public partial class BL: IBL.IBL
    {
        /// <summary>
        /// send a drone to Base Station for charging
        /// </summary>
        /// <param name="id"> drone's ID </param>
        public void ChargeDrone(int id)
        {
            // check if drone exists

            if (!Drones.Any(dr => dr.Id == id))
                throw new ActionException($" {id} dosen't exist already");

            int index = Drones.FindIndex(x => (x.Id == id));

            // check if drone is available to charge
            if (Drones[index].Status == DroneStatus.Maintenance)
                throw new ActionException($"drone - {id} is already charging");
            if (Drones[index].Status == DroneStatus.Delivery)
                throw new ActionException($"drone - {id} is en route , cannot be charged right now");

            // find nearest base station
            IDAL.DO.BaseStation st = getNearestBasestation(Drones[index].DroneLocation);
            Location stLocation = createLocation(st.Longitude, st.Lattitude);
            double distance = Distance.GetDistance(stLocation, Drones[index].DroneLocation);

            // check if drone has enough battery to cover the  distance
            if ((Drones[index].Battery - distance * DroneElecUseEmpty) <= 0)
                throw new ActionException($"drone - {id} does not have enough battery to reach nearest base station");

            // update drone's details 
            Drones[index].Battery -= (int)(distance * DroneElecUseEmpty);
            Drones[index].DroneLocation = stLocation;
            Drones[index].Status = DroneStatus.Maintenance;

            // update necessary details in datasource
            myDal.AddDroneCharge(new IDAL.DO.DroneCharge { DroneId = id, StationId = st.Id });
            st.NumOfSlots--;
            myDal.UpdateBaseStation(st);
        }

        /// <summary>
        /// discharge a drone from charging slot
        /// </summary>
        /// <param name="id"> drone's ID</param>
        /// <param name="time"> time of charge in hours </param>
        public void DischargeDrone(int id, int time)
        {
            // check if drone exists
            if (!Drones.Any(dr => dr.Id == id))
                throw new ActionException($" {id} dosen't exist ");
            int index = Drones.FindIndex(x => (x.Id == id));

            // check if drone is currently charging
            if ((Drones[index].Status == DroneStatus.Available))
                throw new ActionException($"drone - {id} is currently  not at  charging dock");
            if (Drones[index].Status == DroneStatus.Delivery)
                throw new ActionException($"drone - {id} is en route , currently  not at  charging dock");

            // update drone 
            Drones[index].Battery = Math.Max(Drones[index].Battery + (int)DroneHourlyChargeRate * time, 100);
            Drones[index].Status = DroneStatus.Available;

            // update DAL
            IDAL.DO.DroneCharge tempCharge = myDal.GetDroneCharge(id);
            IDAL.DO.BaseStation tempSt = myDal.GetBaseStation(tempCharge.StationId);
            myDal.RemoveDroneCharge(tempCharge);
            tempSt.NumOfSlots++;
            myDal.UpdateBaseStation(tempSt);
        }

        /// <summary>
        /// link a drone to a  compatible parcel for initial process of a delivery
        /// </summary>
        /// <param name="id"> drone's ID</param>
        public void LinkDroneToParcel(int id)
        {
            // check if drones exists
            if (!Drones.Any(dr => dr.Id == id))
                throw new ActionException($"Drone - {id} doesn't exist");

            // get all unlinked parcels
            List<IDAL.DO.Parcel> unlinked;
            try
            {
                unlinked = myDal.GetAllParcels(pr => pr.DroneId == 0).ToList();
            }
            catch (Exception ex)
            {
                throw new ActionException("", ex);
            }
            DroneInList dr = GetAllDronesInList().FirstOrDefault(dr => dr.Id == id);
            int index = GetAllDronesInList().ToList().FindIndex(dr => dr.Id == id);

            // loop from highest parcel delivery priority down
            // for each level check all weight categories compatible for drone  if a parcel can be
            // delivered by the drone.  
            for (int i = (int)Priority.Emergency; i > 0; i--)
            {
                // filter unlinked parcels list to parcels matching current priority level
                List<IDAL.DO.Parcel> temp = unlinked.FindAll(prc => (Priority)prc.Priority == (Priority)i);
                if (temp.Count > 0)
                {
                    // check all weight categories for current priority level
                    for (int j = (int)dr.MaxWeight; j > 0; j--)
                    {
                        // filter parcel list down to parcels also matching curreny weight category
                        temp = temp.FindAll(prc => (WeightCategories)prc.Weight == (WeightCategories)j);
                        if (temp.Count() > 0)
                        {
                            // find nearest parcel
                            IDAL.DO.Parcel prc = getNearestParcel(dr.DroneLocation, temp);
                            IDAL.DO.Customer sender = myDal.GetCustomer(prc.SenderId);
                            IDAL.DO.Customer target = myDal.GetCustomer(prc.TargetId);

                            // if checkDroneDistanceCoverage() return true - drone can execute delivery - link drone to parcel 
                            // and end function
                            if (checkDroneDistanceCoverage(dr, createLocation(sender.Longitude, sender.Lattitude), createLocation(target.Longitude, target.Longitude), dr.MaxWeight))
                            {
                                prc.DroneId = id;
                                prc.Scheduled = DateTime.Now;
                                myDal.UpdateParcel(prc);
                                Drones[index].Status = DroneStatus.Delivery;
                                Drones[index].ParcelId = prc.Id;
                                return;
                            }
                        }
                    }
                }
            }
            // no compatible parcels to link
            throw new ActionException($"drone - {id} has no compatible parcel to link");
        }

        /// <summary>
        /// pick up a parcel by its linked drone
        /// </summary>
        /// <param name="id"> drone ID </param>
        public void DroneParcelPickUp(int id)
        {
            Drone dr = GetDrone(id);

            // check that drone and parcel are linked
            if (dr.Parcel == null)
                throw new ActionException($"Drone - {id} isen't linked yet to a parcel");
            if ((dr.Parcel.Status == true))
                throw new ActionException($"Drone - {id} already picked up parcel");

            // deduct drone battery and set drone's location to sender's location
            int index = GetAllDronesInList().ToList().FindIndex(dr => dr.Id == id);
            Drones[index].Battery -= (int)(Distance.GetDistance(dr.Location, dr.Parcel.SenderLocation) * DroneElecUseEmpty);
            Drones[index].DroneLocation = dr.Parcel.SenderLocation;

            // update DAL
            IDAL.DO.Parcel p = myDal.GetParcel(dr.Parcel.Id);
            p.PickedUp = DateTime.Now;
            myDal.UpdateParcel(p);
        }

        /// <summary>
        /// deliver a parcel by its linked drone
        /// </summary>
        /// <param name="id"> drone ID </param>
        public void DroneParcelDelivery(int id)
        {
            Drone dr = GetDrone(id);
            // check that drone and parcel are linked
            if (dr.Parcel == null)
                throw new ActionException($"Drone - {id} isen't linked yet to a parcel");
            // check that parcel wasen't delivered yet
            Parcel prc = GetParcel(dr.Parcel.Id);
            if (prc.Delivered != DateTime.MinValue)
                throw new ActionException($"Drone - {id} already delievered  parcel");
            
            // deduct drone battery , set location to target's location , and mark drone  as available
            int index = GetAllDronesInList().ToList().FindIndex(dr => dr.Id == id);
            Drones[index].Battery -= (int)(Distance.GetDistance(dr.Location, dr.Parcel.TargetLocation) * getElectricUseForDrone(prc.Weight));
            Drones[index].DroneLocation = dr.Parcel.TargetLocation;
            Drones[index].Status = DroneStatus.Available;
            Drones[index].ParcelId = 0;

            // update DAL
            myDal.UpdateParcel(new IDAL.DO.Parcel
            {
                Id = prc.Id,
                SenderId = prc.Sender.Id,
                TargetId = prc.Target.Id,
                Weight = (IDAL.DO.WeightCategories)prc.Weight,
                Priority = (IDAL.DO.Priorities)prc.Priority,
                Requested = prc.Ordered,
                Scheduled = prc.Linked,
                PickedUp = prc.PickedUp,
                Delivered = DateTime.Now,
                DroneId = 0,
            });
        }

    }
}
