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
            string dir = @"..\xml\";
            dronesRoot = new XElement("Drones");
            dronesRoot.Save(dir + dronePath);
        }

        private void LoadData()
        {
            string dir = @"..\xml\";
            try
            {
                dronesRoot = XElement.Load(dir + dronePath);
            }
            catch
            {
                Console.WriteLine("File upload problem");
            }
        }

        public void AddDrone(Drone dr)
        {
            LoadData();
            string dir = @"..\xml\";
            if (dronesRoot.Elements().Any(drone => (Convert.ToInt32(drone.Element("ID").Value) == dr.Id)))
                throw new ExsistException($"id number {dr.Id} already exists");

            dronesRoot.Add(new XElement("Drone",
                new XElement("ID", dr.Id),
                new XElement("Model", dr.Model),
                new XElement("MaxWeight", dr.MaxWeight)));
            dronesRoot.Save(dir+dronePath);
        }

        public Drone GetDrone(int id)
        {
            LoadData();

            Drone? temp = new Drone();

            temp = (from dr in dronesRoot.Elements()
                    where Convert.ToInt32(dr.Element("ID").Value) == id
                    select new Drone()
                    {
                        Id = Convert.ToInt32(dr.Element("ID").Value),
                        Model = dr.Element("Model").Value,
                        MaxWeight = (WeightCategories)(GetWeightCategories(dr.Element("MaxWeight").Value))
                    }).FirstOrDefault();
            if (temp.Value.Id == 0)
                throw new NonExistsException($"ID number {id} not found");

            return (Drone)temp;
        }

        public void RemoveDrone(Drone drone)
        {
            if (!dronesRoot.Elements().Any(dr => (Convert.ToInt32(dr.Element("ID").Value) == drone.Id)))
                throw new ExsistException($"id number {drone.Id} not found");
            string dir = @"..\xml\";
            XElement droneElement;

            droneElement = (from dr in dronesRoot.Elements()
                            where Convert.ToInt32(dr.Element("ID").Value) == drone.Id
                            select dr).FirstOrDefault();
            droneElement.Remove();
            droneElement.Save(dir+dronePath);

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
            droneElement.Element("MaxWeight").Value = drone.MaxWeight.ToString();
            dronesRoot.Save(dronePath);

        }

        public IEnumerable<Drone> GetAllDrones(Func<Drone, bool> predicate = null)
        {
            LoadData();
            IEnumerable<Drone> drones;
            drones = from dr in dronesRoot.Elements()
                     select new Drone()
                     {
                         Id = Convert.ToInt32(dr.Element("ID").Value),
                         Model = dr.Element("Model").Value,
                         MaxWeight = (WeightCategories)GetWeightCategories((dr.Element("MaxWeight").Value).ToString())
                     };
            if (predicate == null)
            {
                if (!drones.Any())
                    throw new EmptyListException("no drones in list");
                return drones;
            }
            IEnumerable<Drone> tmp = drones.Where(predicate);
            if (tmp.Any())
                return tmp;
            else
                throw new FilteredListException("No Drones in list match predicate");
        }
        private WeightCategories? GetWeightCategories(string val)
        {
            switch (val)
            {
                case "LIGHT":
                    return WeightCategories.LIGHT;
                case "MEDIUM":
                    return WeightCategories.MEDIUM;
                case "HEAVY":
                    return WeightCategories.HEAVY;
            }
            return null;
        }
    }
}
