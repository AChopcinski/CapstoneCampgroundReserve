﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campground
    {
        public int CampgroundID { get; set; }
        public int ParkID { get; set; }
        public string Name { get; set; }
        public int OpenMonth { get; set; }
        public int CloseMonth { get; set; }
        public double DailyFee { get; set; }
    }  
}
