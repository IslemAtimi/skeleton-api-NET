using AB.Ecommerce.Gros.Shared;

namespace AB.Ecommerce.Gros.Application;

    public class CategoriesGetListInput
    {
        public string? Filter {get;set;}
        public bool? Visible { get; set; }

        public CategoryType? CategoryType { get; set; } = Shared.CategoryType.None;

}



