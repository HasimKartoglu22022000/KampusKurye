using System.ComponentModel.DataAnnotations;

namespace KampusKurye.Models
{
    public class CategoriesModel
    {
        [Key]
        public int categories_id { get; set; }
        public string categories_name { get; set; }
    }
}
