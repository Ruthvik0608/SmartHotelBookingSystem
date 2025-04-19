using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotelBookingSystem.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int UserID { get; set; }
        public int HotelID { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsFlagged { get; internal set; }
    }
}
