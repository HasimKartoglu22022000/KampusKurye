using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KampusKurye.Models
{
    public class OrderItemModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int order_item_id { get; set; }

        public int order_id { get; set; }
        public int product_id { get; set; }

        public string product_name { get; set; }
        public double product_price { get; set; }
        public int product_quantity { get; set; }
        public string? product_imgurl { get; set; }

        [ForeignKey(nameof(order_id))]
        public OrderModel order { get; set; }
    }
}
