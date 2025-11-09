namespace KampusKurye.Models
{
    public class AdminOrderListItemViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string RestaurantName { get; set; }
        public string CustomerName { get; set; }
        public double TotalPrice { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? CustomerNote { get; set; }
    }
}
