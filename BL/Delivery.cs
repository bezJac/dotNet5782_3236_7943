using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Delivery
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority Priority { get; set; }
        public bool Status { get; set; }
        public Location SenderLocation { get; set; }
        public Location TargetLocation { get; set; }
        public double Distance { get; set; }
    }
}
