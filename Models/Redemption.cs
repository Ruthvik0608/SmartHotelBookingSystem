using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotelBookingSystem.Models
{
    public class Redemption
    {
        public int RedemptionID { get; set; }
        public int UserID { get; set; }
        public int BookingID { get; set; }
        public int PointsUsed { get; set; }
        public decimal DiscountAmount { get; set; }

    }
}
