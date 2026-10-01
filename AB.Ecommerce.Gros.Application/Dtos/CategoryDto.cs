using AB.Ecommerce.Gros.Shared;

namespace AB.Ecommerce.Gros.Application;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Label { get; set; }
    public bool Visible { get; set; }
    public string Description { get; set; }
    public string Histoire { get; set; }
    public DateTime CreatedAt { get; set; }

    public CategoryType  CategoryType { get; set; }
    public int ProductCount { get; private set; }

    public void SetProductCount(int count)
    {
        ProductCount = count;
    }
}

