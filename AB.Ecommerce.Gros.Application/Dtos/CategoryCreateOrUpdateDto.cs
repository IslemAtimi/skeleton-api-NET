using AB.Ecommerce.Gros.Shared;

namespace AB.Ecommerce.Gros.Application;


public class CategoryCreateOrUpdateDto
{
    public string Label { get; set; }
    public bool Visible { get; set; }
    public string Description { get; set; }
    public string Histoire { get; set; }
    public CategoryType? Type { get; set; }= Shared.CategoryType.None;

    public FileInput Image { get; set; }
}
