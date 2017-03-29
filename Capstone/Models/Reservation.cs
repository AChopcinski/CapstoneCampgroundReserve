using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{

    public class Reservation
    {
        public int ReservationID { get; set; }
        public int SiteID { get; set; }
        public string CustomerName { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public Reservation()
        {
            this.CustomerName = "";
            this.CreatedDate = new DateTime();
        }
    }
}
