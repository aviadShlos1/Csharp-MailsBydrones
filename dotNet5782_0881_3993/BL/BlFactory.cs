using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;

namespace BlApi
{
    public static class BlFactory
    {
        public static BlApi.IBL GetBl()
        {
            BlApi.BL bl = BlApi.BL.Instance;
            return bl;
        }
            
    }
}
