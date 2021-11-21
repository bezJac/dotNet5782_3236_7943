using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;
using IBL.BO;

namespace BL
{
    public partial class BL: IBL.IBL
    {
        public void ChargeDrone(int id)
        {
            // check if drone exists
            if (!Drones.Any(dr => dr.Id == id))
                throw new DroneException($" {id} dosen't exist already");

            int index = Drones.FindIndex(x => (x.Id == id));

            // check if drone is available to charge
            if (Drones[index].Status == DroneStatus.Maintenance)
                throw new DroneException($"drone - {id} is already charging");
            if (Drones[index].Status == DroneStatus.Delivery)
                throw new DroneException($"drone - {id} is en route , cannot be charged right now");

            // find nearest base station

            IDAL.DO.BaseStation st = getNearestBasestation(Drones[index].DroneLocation);
            Location stLocation = createLocation(st.Longitude, st.Lattitude);
            double distance = Distance.GetDistance(stLocation, Drones[index].DroneLocation);

            // check if drone has enough battery to cover the  distance
            if ((Drones[index].Battery - distance * DroneElecUseEmpty) <= 0)
                throw new DroneException($"drone - {id} does not have enough battery to reach nearest base station");

            // update drone's details 
            Drones[index].Battery -= (int)(distance * DroneElecUseEmpty);
            Drones[index].DroneLocation = stLocation;
            Drones[index].Status = DroneStatus.Maintenance;

            // update necessary details in datasource
            myDal.AddDroneCharge(new IDAL.DO.DroneCharge { DroneId = id, StationId = st.Id });
            st.NumOfSlots--;
            myDal.UpdateBaseStation(st);
        }
        public void DischargeDrone(int id, int time)
        {
            // check if drone exists
            if (!Drones.Any(dr => dr.Id == id))
                throw new DroneException($" {id} dosen't exist ");
            int index = Drones.FindIndex(x => (x.Id == id));
            if ((Drones[index].Status == DroneStatus.Available))
                throw new DroneException($"drone - {id} is currently  not at  charging dock");
            if (Drones[index].Status == DroneStatus.Delivery)
                throw new DroneException($"drone - {id} is en route , currently  not at  charging dock");

            Drones[index].Battery = Math.Max(Drones[index].Battery + (int)DroneHourlyChargeRate * time, 100);
            Drones[index].Status = DroneStatus.Available;

            IDAL.DO.DroneCharge tempCharge = myDal.GetDroneCharge(id);
            IDAL.DO.BaseStation tempSt = myDal.GetBaseStation(tempCharge.StationId);
            myDal.RemoveDroneCharge(tempCharge);
            tempSt.NumOfSlots++;
            myDal.UpdateBaseStation(tempSt);
        }
        public void LinkDroneToParcel(int id)
        {
            if (!Drones.Any(dr => dr.Id == id))
                throw new DroneException($"Drone - {id} doesn't exist");
            List<IDAL.DO.Parcel> unlinked;
            try
            {
                unlinked = myDal.GetAllParcels(pr => pr.DroneId == 0).ToList();
            }
            catch (Exception ex)
            {
                throw new ParcelException("BL - ", ex);
            }
            DroneInList dr = GetAllDronesInList().ToList().Find(dr => dr.Id == id);
            int index = GetAllDronesInList().ToList().FindIndex(dr => dr.Id == id);
            for (int i = (int)Priority.Emergency; i > 0; i--)
            {
                List<IDAL.DO.Parcel> temp = unlinked.FindAll(prc => (Priority)prc.Priority == (Priority)i);
                if (temp.Count > 0)
                {
                    for (int j = (int)dr.MaxWeight; j > 0; j--)
                    {
                        temp = temp.FindAll(prc => (WeightCategories)prc.Weight == (WeightCategories)j);
                        if (temp.Count() > 0)
                        {
                            IDAL.DO.Parcel prc = getNearestParcel(dr.DroneLocation, temp);
                            IDAL.DO.Customer sender = myDal.GetCustomer(prc.SenderId);
                            IDAL.DO.Customer target = myDal.GetCustomer(prc.TargetId);
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
            throw new DroneException($"drone - {id} has no compatible parcel to link");
        }
        public void DroneParcelPickUp(int id)
        {
            if (!Drones.Any(dr => dr.Id == id))
                throw new DroneException($"Drone - {id} doesn't exist");
            Drone dr = GetDrone(id);
            if (dr.Parcel == null)
                throw new DroneException($"Drone - {id} isen't linked yet to a parcel");
            if ((dr.Parcel.Status == true))
                throw new DroneException($"Drone - {id} already picked up parcel");

            int index = GetAllDronesInList().ToList().FindIndex(dr => dr.Id == id);
            Drones[index].Battery -= (int)(Distance.GetDistance(dr.Location, dr.Parcel.SenderLocation) * DroneElecUseEmpty);
            Drones[index].DroneLocation = dr.Parcel.SenderLocation;

            IDAL.DO.Parcel p = myDal.GetParcel(dr.Parcel.Id);
            p.PickedUp = DateTime.Now;
            myDal.UpdateParcel(p);
        }
        public void DroneParcelDelivery(int id)
        {
            if (!Drones.Any(dr => dr.Id == id))
                throw new DroneException($"Drone - {id} doesn't exist");
            Drone dr = GetDrone(id);
            if (dr.Parcel == null)
                throw new DroneException($"Drone - {id} isen't linked yet to a parcel");
            Parcel prc = GetParcel(dr.Parcel.Id);
            if (prc.Delivered != DateTime.MinValue)
                throw new DroneException($"Drone - {id} already delievered  parcel");

            int index = GetAllDronesInList().ToList().FindIndex(dr => dr.Id == id);
            Drones[index].Battery -= (int)(Distance.GetDistance(dr.Location, dr.Parcel.TargetLocation) * getElectricUseForDrone(prc.Weight));
            Drones[index].DroneLocation = dr.Parcel.TargetLocation;
            Drones[index].Status = DroneStatus.Available;
            Drones[index].ParcelId = 0;

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
