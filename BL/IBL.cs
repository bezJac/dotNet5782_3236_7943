using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
     public interface IBL
    {
        void AddBaseStation(BaseStation station);
        void AddDrone(Drone drone);
        void AddParcel(Parcel parcel);
        void AddCustomer(Customer customer);
    }
}
