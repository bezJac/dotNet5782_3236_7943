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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ChargeDrone(int id)
        {
            // check if drone exists
            if (!drones.Any(dr => dr.Id == id))
                throw new ActionException($" {id} dosen't exist already");
            // find index of drone in list
            int index = drones.FindIndex(x => (x.Id == id));

            // check if drone is available to charge
            if (drones[index].Status == DroneStatus.Maintenance)
                throw new ActionException($"drone - {id} is already charging");
            if (drones[index].Status == DroneStatus.Delivery)
                throw new ActionException($"drone - {id} is en route , cannot be charged right now");

            lock (myDal)
            {
                // find nearest base station
                DO.BaseStation st = getNearestAvailableBasestation(drones[index].DroneLocation);
                Location stLocation = createLocation(st.Longitude, st.Lattitude);
                double distance = Distance.GetDistance(stLocation, drones[index].DroneLocation);

                // check if drone has enough battery to cover flight  distance to the selected station
                if ((drones[index].Battery - distance * droneElecUseEmpty) <= 0)
                    throw new ActionException($"drone - {id} does not have enough battery to reach nearest base station");

                // update drone's details 
                drones[index].Battery -= (int)(distance * droneElecUseEmpty);
                drones[index].DroneLocation = stLocation;
                drones[index].Status = DroneStatus.Maintenance;

                // update base station in DAL , add a new drone charge entity to list
                myDal.AddDroneCharge(new DO.DroneCharge { DroneId = id, StationId = st.Id, EntranceTime = DateTime.Now, BatteryAtEntrance = drones[index].Battery });
                st.NumOfSlots--;
                myDal.UpdateBaseStation(st);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DischargeDrone(int id)
        {
            // check if drone exists
            if (!drones.Any(dr => dr.Id == id))
                throw new ActionException($" {id} dosen't exist ");
            // find index of drone in list
            int index = drones.FindIndex(x => (x.Id == id));

            // check if drone is currently charging
            if ((drones[index].Status == DroneStatus.Available))
                throw new ActionException($"drone - {id} is currently  not at  charging dock");
            if (drones[index].Status == DroneStatus.Delivery)
                throw new ActionException($"drone - {id} is en route , currently  not at  charging dock");
            lock (myDal)
            {
                DO.DroneCharge tempCharge = myDal.GetDroneCharge(id);
                TimeSpan duration = DateTime.Now.Subtract((DateTime)tempCharge.EntranceTime);
                double time = (duration.Hours * 3600) + ((double)duration.Minutes * 60) + duration.Seconds;
                // update drone 
                drones[index].Battery = Math.Min(tempCharge.BatteryAtEntrance + (int)(DroneChargeRatePerSecond * time), 100);
                drones[index].Status = DroneStatus.Available;

                // update base station available charging slots in DAL , remove drone charge entity from list

                DO.BaseStation tempSt = myDal.GetBaseStation(tempCharge.StationId);
                myDal.RemoveDroneCharge(tempCharge);
                tempSt.NumOfSlots++;
                myDal.UpdateBaseStation(tempSt);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void LinkDroneToParcel(int id)
        {
            lock (myDal)
            {
                // check if drones exists
                if (!drones.Any(dr => dr.Id == id))
                    throw new ActionException($"Drone - {id} doesn't exist");
                DroneInList dr = GetAllDronesInList().FirstOrDefault(dr => dr.Id == id);
                if (dr.Status == DroneStatus.Delivery)
                    throw new ActionException($"Drone - {id} is alredy in delivery");
                if (dr.Status == DroneStatus.Maintenance)
                    throw new ActionException($"Drone - {id} is charging, unavailable");
                int index = drones.FindIndex(dr => dr.Id == id);
                // get all parcels that sre both , unlinked to a drone yet, parcel wight is <= to drone's max weight capability
                IEnumerable<DO.Parcel> unlinked;
                try
                {
                    unlinked = from prc in myDal.GetAllParcels()
                               where getParcelStatus(prc) == ParcelStatus.Ordered && (WeightCategories)prc.Weight <= dr.MaxWeight
                               select prc;
                }
                catch (Exception ex)
                {
                    throw new ActionException("empty", ex);
                }
                // sort list by priority in descending order, then perform subsequent ordering by weight in descending order , the perform another subsequent ordering
                // by shortest distance from drone to parcel's sender location in ascending order 
                unlinked = unlinked.OrderByDescending(prc => prc.Priority).ThenByDescending(prc => prc.Weight).ThenBy
                    (prc => Distance.GetDistance(createLocation(myDal.GetCustomer(prc.SenderId).Longitude, myDal.GetCustomer(prc.SenderId).Lattitude), dr.DroneLocation));
                foreach (DO.Parcel prc in unlinked)
                {
                    // create copy of prc for update purposes
                    DO.Parcel tmp = prc;
                    // get parcel's sender and target customers
                    DO.Customer sender = myDal.GetCustomer(prc.SenderId);
                    DO.Customer target = myDal.GetCustomer(prc.TargetId);
                    // if drone can execute delivery for current parcel - link parcel to drone - return 
                    if (checkDroneDistanceCoverage(dr, createLocation(sender.Longitude, sender.Lattitude), createLocation(target.Longitude, target.Lattitude), (WeightCategories)dr.MaxWeight))
                    {
                        tmp.DroneId = id;
                        tmp.Scheduled = DateTime.Now;
                        myDal.UpdateParcel(tmp);
                        drones[index].Status = DroneStatus.Delivery;
                        drones[index].ParcelId = prc.Id;
                        return;
                    }
                }
                //no compatible parcels to link
                throw new ActionException($"drone - {id} has no compatible parcel to link");
            }


        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneParcelPickUp(int id)
        {
            lock (myDal)
            {
                // check if drones exsists
                Drone dr;
                try
                {
                    dr = GetDrone(id);
                }
                catch (Exception ex)
                {
                    throw new ActionException("", ex);
                }

                // check that drone and parcel are linked
                if (dr.Parcel == null)
                    throw new ActionException($"Drone - {id} isen't linked yet to a parcel");
                // check that parcel hasen't been picked up already
                if ((dr.Parcel.InTransit == true))
                    throw new ActionException($"Drone - {id} already picked up parcel");

                // deduct battery use for flight to sender  from drone battery and set drone's location to sender's location
                int index = GetAllDronesInList().ToList().FindIndex(dr => dr.Id == id);
                drones[index].Battery = drones[index].Battery - (int)(Distance.GetDistance(dr.Location, dr.Parcel.SenderLocation) * droneElecUseEmpty);
                drones[index].DroneLocation = dr.Parcel.SenderLocation;
                drones[index].ParcelId = dr.Parcel.Id;

                // update parcel in DAL 
                DO.Parcel p = myDal.GetParcel((int)dr.Parcel.Id);
                p.PickedUp = DateTime.Now;
                myDal.UpdateParcel(p);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneParcelDelivery(int id)
        {
            lock (myDal)
            {
                // check if drones exsists
                Drone dr;
                try
                {
                    dr = GetDrone(id);
                }
                catch (Exception ex)
                {
                    throw new ActionException("", ex);
                }
                // check that drone and parcel are linked
                if (dr.Parcel == null)
                    throw new ActionException($"Drone - {id} isen't linked yet to a parcel");
                // check that parcel wasen't delivered yet
                Parcel prc;
                try
                {
                    prc = GetParcel((int)dr.Parcel.Id);
                }
                catch (Exception ex)
                {
                    throw new ActionException("", ex);
                }

                if (prc.Delivered != null)
                    throw new ActionException($"Drone - {id} already delievered  parcel");
                if (prc.PickedUp == null)
                    throw new ActionException($"drone hasn't picked up parcel yet");
                // deduct battery use of  flight from current drone location to target location  from drone's battery
                // set location to target's location , and mark drone  as available
                int index = GetAllDronesInList().ToList().FindIndex(dr => dr.Id == id);
                drones[index].Battery = drones[index].Battery - (int)(Distance.GetDistance(dr.Location, dr.Parcel.TargetLocation) * getElectricUseForDrone((WeightCategories)(prc.Weight)));
                drones[index].DroneLocation = dr.Parcel.TargetLocation;
                drones[index].Status = DroneStatus.Available;
                drones[index].ParcelId = null;

                // update parcel in  DAL
                myDal.UpdateParcel(new DO.Parcel
                {
                    Id = prc.Id,
                    SenderId = prc.Sender.Id,
                    TargetId = prc.Target.Id,
                    Weight = (DO.WeightCategories)prc.Weight,
                    Priority = (DO.Priorities)prc.Priority,
                    Requested = prc.Ordered,
                    Scheduled = prc.Linked,
                    PickedUp = prc.PickedUp,
                    Delivered = DateTime.Now,
                    DroneId = 0,
                });
            }
        }
    }
}
