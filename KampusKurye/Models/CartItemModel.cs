namespace KampusKurye.Models
{
    public class CartItemModel
    {
        public int order_id { get; set; }
        public string order_name { get; set; }
        public double order_price { get; set; }
        public int order_quantity { get; set; }
        public string? order_img_url { get; set; }
    }
}
