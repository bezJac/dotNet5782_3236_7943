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

            // find nearest base station
            IDAL.DO.BaseStation st = getNearestAvailableBasestation(drones[index].DroneLocation);
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
            myDal.AddDroneCharge(new IDAL.DO.DroneCharge { DroneId = id, StationId = st.Id,EntranceTime=DateTime.Now });
            st.NumOfSlots--;
            myDal.UpdateBaseStation(st);
        }
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

            IDAL.DO.DroneCharge tempCharge = myDal.GetDroneCharge(id);
            TimeSpan duration = DateTime.Now.Subtract((DateTime)tempCharge.EntranceTime);
            double time = duration.Hours + (int)(duration.Minutes / 60)+ (int)(duration.Seconds/3600);
            // update drone 
            drones[index].Battery = Math.Max(drones[index].Battery + (int)(droneHourlyChargeRate * time), 100);
            drones[index].Status = DroneStatus.Available;

            // update base station available charging slots in DAL , remove drone charge entity from list
            
            IDAL.DO.BaseStation tempSt = myDal.GetBaseStation(tempCharge.StationId);
            myDal.RemoveDroneCharge(tempCharge);
            tempSt.NumOfSlots++;
            myDal.UpdateBaseStation(tempSt);
        }
        public void LinkDroneToParcel(int id)
        {
            // check if drones exists
            if (!drones.Any(dr => dr.Id == id))
                throw new ActionException($"Drone - {id} doesn't exist");
            DroneInList dr = GetAllDronesInList().FirstOrDefault(dr => dr.Id == id);
            if(dr.Status==DroneStatus.Delivery)
                throw new ActionException($"Drone - {id} is alredy in delivery");
            if(dr.Status==DroneStatus.Maintenance)
                throw new ActionException($"Drone - {id} is charging, unavailable");
            int index = GetAllDronesInList().ToList().FindIndex(dr => dr.Id == id);
            // get all parcels that sre both , unlinked to a drone yet, parcel wight is <= to drone's max weight capability
            IEnumerable<IDAL.DO.Parcel> unlinked;
            try
            {
                unlinked = myDal.GetAllParcels(pr => pr.DroneId == 0 && (WeightCategories)pr.Weight <= dr.MaxWeight);
            }
            catch (Exception ex)
            {
                throw new ActionException("", ex);
            }
            // sort list by priority in descending order, then perform subsequent ordering by weight in descending order , the perform another subsequent ordering
            // by shortest distance from drone to parcel's sender location in ascending order 
            unlinked = unlinked.OrderByDescending(prc => prc.Priority).ThenByDescending(prc => prc.Weight).ThenBy
                (prc => Distance.GetDistance(createLocation(myDal.GetCustomer(prc.SenderId).Longitude, myDal.GetCustomer(prc.SenderId).Lattitude), dr.DroneLocation));
            foreach (IDAL.DO.Parcel prc in unlinked)
            {
                // create copy of prc for update purposes
                IDAL.DO.Parcel tmp = prc;
                // get parcel's sender and target customers
                IDAL.DO.Customer sender = myDal.GetCustomer(prc.SenderId);
                IDAL.DO.Customer target = myDal.GetCustomer(prc.TargetId);
                // if drone can execute delivery for current parcel - link parcel to drone - return 
                if (checkDroneDistanceCoverage(dr, createLocation(sender.Longitude, sender.Lattitude), createLocation(target.Longitude, target.Lattitude), dr.MaxWeight))
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
        public void DroneParcelPickUp(int id)
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
            drones[index].Battery -= (int)(Distance.GetDistance(dr.Location, dr.Parcel.SenderLocation) * droneElecUseEmpty);
            drones[index].DroneLocation = dr.Parcel.SenderLocation;

            // update parcel in DAL 
            IDAL.DO.Parcel p = myDal.GetParcel(dr.Parcel.Id);
            p.PickedUp = DateTime.Now;
            myDal.UpdateParcel(p);
        }
        public void DroneParcelDelivery(int id)
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
                prc = GetParcel(dr.Parcel.Id);
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
            drones[index].Battery -= (int)(Distance.GetDistance(dr.Location, dr.Parcel.TargetLocation) * getElectricUseForDrone(prc.Weight));
            drones[index].DroneLocation = dr.Parcel.TargetLocation;
            drones[index].Status = DroneStatus.Available;
            drones[index].ParcelId = 0;

            // update parcel in  DAL
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
                DroneId = dr.Id,
            });
        }
    }
}
