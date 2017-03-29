using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Site
    {
        public int SiteID { get; set; }
        public int CampgroundID { get; set; }
        public int SiteNumberInCampground { get; set; }
        public int MaxOccupancy { get; set; }
        public bool IsWheelChairAccessible { get; set; }
        public int MaxRVLength { get; set; }
        public bool HasRVHookup { get; set; }
    }
}
