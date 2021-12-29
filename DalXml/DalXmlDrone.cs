using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        private void CreateFiles()
        {
            dronesRoot = new XElement("Drones");
           dronesRoot.Save(dronePath);
        }

        private void LoadData()
        {
            try
            {
                dronesRoot = XElement.Load(dronePath);
            }
            catch
            {
                Console.WriteLine("File upload problem");
            }
        }

        public void AddDrone(Drone dr)
        {
            LoadData();
            if (dronesRoot.Elements().Any(drone => (Convert.ToInt32(drone.Element("ID").Value) == dr.Id)))
                throw new ExsistException($"id number {dr.Id} already exists");
           
            dronesRoot.Add(new XElement("Drone",
                new XElement("ID", dr.Id),
                new XElement("Model", dr.Model),
                new XElement("MaxWeight", dr.MaxWeight)));
            dronesRoot.Save(dronePath);
        }

        public Drone GetDrone(int id)
        {
            LoadData();
            Drone? drone;
            try
            {
                drone = (from dr in dronesRoot.Elements()
                           where Convert.ToInt32(dr.Element("ID").Value) == id
                           select new Drone()
                           {
                               Id = Convert.ToInt32(dr.Element("id").Value),
                               Model = dr.Element("Model").Value,
                               MaxWeight = (WeightCategories)Convert.ToInt32(dr.Element("MaxWeight").Value)
                           }).First();
            }
            catch
            {
                 throw new NonExistsException($"id number {id} not found");
            }
            return (Drone) drone;
        }

        public void RemoveDrone(Drone drone)
        {
            if (!dronesRoot.Elements().Any(dr => (Convert.ToInt32(dr.Element("ID").Value) == drone.Id)))
                throw new ExsistException($"id number {drone.Id} not found");
           
            XElement droneElement;
            
                droneElement = (from dr in dronesRoot.Elements()
                                  where Convert.ToInt32(dr.Element("ID").Value) == drone.Id
                                  select dr).FirstOrDefault();
                droneElement.Remove();
                droneElement.Save(dronePath);
            
                throw new NonExistsException($"id number {drone.Id} not found");
            

        }

        public void UpdateDrone(Drone drone)
        {
            LoadData();
            if (!dronesRoot.Elements().Any(dr => (Convert.ToInt32(dr.Element("ID").Value) == drone.Id)))
                throw new ExsistException($"id number {drone.Id} not found");
            
            XElement droneElement = (from dr in dronesRoot.Elements()
                                       where Convert.ToInt32(dr.Element("ID").Value) == drone.Id
                                       select dr).FirstOrDefault();
            droneElement.Element("Model").Value = drone.Model;
            droneElement.Element("MaxWeight").Value =drone.MaxWeight.ToString();
            dronesRoot.Save(dronePath);

        }

        public IEnumerable<Drone> GetAllDrones(Func<Drone, bool> predicate = null)
        {
            LoadData();
            IEnumerable<Drone> drones;
            
                    drones = (from dr in dronesRoot.Elements()
                              select new Drone()
                              {
                                  Id = Convert.ToInt32(dr.Element("id").Value),
                                  Model = dr.Element("Model").Value,
                                  MaxWeight = (WeightCategories)Convert.ToInt32(dr.Element("MaxWeight").Value)
                              });
            if (predicate == null)
            {
                if (!drones.Any())
                    throw new EmptyListException("no drones in list");
                return drones;
            }
            IEnumerable<Drone> tmp =drones.Where(predicate);
            if (tmp.Any())
                return tmp;
            else
                throw new FilteredListException("No Drones in list match predicate");
        }
    }
}
