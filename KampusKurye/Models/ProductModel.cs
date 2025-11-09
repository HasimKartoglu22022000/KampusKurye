using System.ComponentModel.DataAnnotations;

namespace KampusKurye.Models
{
    public class ProductModel
    {
        [Key]
        public int product_id { get; set; }
        public Guid product_guid { get; set; } = Guid.NewGuid();
        public int restaurant_id { get; set; }
        public string product_name { get; set; }
        public string product_contents { get; set; }
        public double product_price { get; set; }
        public string product_imgurl { get; set; }
        public int product_weight { get; set; }
        public int categories_id { get; set; }
    }
}
