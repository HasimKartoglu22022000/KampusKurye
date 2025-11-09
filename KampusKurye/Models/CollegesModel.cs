using System.ComponentModel.DataAnnotations;

namespace KampusKurye.Models
{
    public class CollegesModel
    {
        [Key]
        public int college_id { get; set; }
        public string college_name { get; set; }
        public ICollection<RestaurantModel> Restaurants { get; set; } = new List<RestaurantModel>();
    }
}
