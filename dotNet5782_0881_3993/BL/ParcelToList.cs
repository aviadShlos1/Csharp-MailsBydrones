﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelToList // חבילה לרשימה
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string RecieverName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatus ParState{ get; set; }
    }
}
