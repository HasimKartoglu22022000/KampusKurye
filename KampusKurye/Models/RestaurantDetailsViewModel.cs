namespace KampusKurye.Models
{
    public class RestaurantDetailsViewModel
    {
        public RestaurantModel Restaurant { get; set; }
        public IEnumerable<ProductModel> Products { get; set; }
        public IEnumerable<CategoriesModel> Categories { get; set; }
    }
}
