using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotelBookingSystem.Models
{
    public class LoyaltyAccount
    {
        [Key]
        public int LoyaltyID { get; set; }
        public int UserID { get; set; }
        public int PointsBalance { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsActive { get; set; }

    }
}
