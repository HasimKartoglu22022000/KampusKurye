using KampusKurye.Controllers;
using System.ComponentModel.DataAnnotations;

namespace KampusKurye.Models
{
    public class RestaurantModel
    {
        [Key]
        public int restaurant_id { get; set; }
        public string restaurant_name { get; set; }
        public double restaurant_point { get; set; }
        public string restaurant_working_hours { get; set; }
        public string restaurant_phone { get; set; }
        public string restaurant_address { get; set; }
        public string restaurant_description { get; set; }
        public string restaurant_img_url { get; set; }
        public int college_id { get; set; }

        public CollegesModel? College { get; set; }
    }
}
