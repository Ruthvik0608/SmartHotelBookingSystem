using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotelBookingSystem.Models
{
    public class CategoryModel
    {
        [Key]
        public int CatId { get; set; }
        public string Category { get; set; }
        public DateTime CatActivateDate { get; set; }
        public DateTime? CatDeactivateDate { get; set; }
        public string Remarks { get; set; }
    }
}
