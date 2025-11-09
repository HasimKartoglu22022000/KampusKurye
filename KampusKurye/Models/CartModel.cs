namespace KampusKurye.Models
{
    public class CartModel
    {
        public List<CartItemModel> Items { get; set; } = new();

        public int TotalQuantity => Items.Sum(i => i.order_quantity);
        public double TotalPrice => Items.Sum(i => i.order_price * i.order_quantity);

        public void AddItem(int productId, string name, double price, string? imageUrl = null, int quantity = 1)
        {
            var existing = Items.FirstOrDefault(i => i.order_id == productId);

            if (existing == null)
            {
                Items.Add(new CartItemModel
                {
                    order_id = productId,
                    order_name = name,
                    order_price = price,
                    order_quantity = quantity,
                    order_img_url = imageUrl
                });
            }
            else
            {
                existing.order_quantity += quantity;
            }
        }

        public void DecreaseItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.order_id == productId);
            if (item == null) return;

            item.order_quantity--;
            if (item.order_quantity <= 0)
                Items.Remove(item);
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.order_id == productId);
            if (item != null)
                Items.Remove(item);
        }

        public void Clear() => Items.Clear();
    }
}
