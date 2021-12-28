using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlApi
{
    /// <summary>
    /// Factory Design for BL
    /// </summary>
    public static class BlFactory
    {
        public static IBL GetBL()
        {
            return BL.BL.Instance;
        }
    }
}
