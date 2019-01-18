
namespace CloudPlus.Models.Category
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Ord { get; set; }
        public CategoryModel Parent { get; set; }
		public string ImgUrl { get; set; }
		public string Vendor { get; set; }
    }
}
