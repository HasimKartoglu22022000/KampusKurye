using System.ComponentModel.DataAnnotations;

namespace KampusKurye.Models
{
    public class OrderModel
    {
        [Key]
        public int order_id { get; set; }
        public Guid order_guid { get; set; } = Guid.NewGuid();
        public string? order_number { get; set; }
        public int? restaurant_id { get; set; }
        public int? user_id { get; set; }
        public string? order_customer_note { get; set; }
        public string? order_company_note { get; set; }
        public string? order_delivery_address { get; set; }
        public int? order_status { get; set; }= 0;
        public int? courier_id { get; set; }
        public DateTime order_created_at { get; set; }= DateTime.Now;
        public double order_total_price { get; set; }

        public ICollection<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();

    }
}
