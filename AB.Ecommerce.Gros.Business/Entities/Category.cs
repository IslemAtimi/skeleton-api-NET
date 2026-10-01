using AB.Ecommerce.Gros.Shared;

namespace AB.Ecommerce.Gros.Business;

public class Category
{
    public Guid Id { get; set; }
    public string Label { get; set; } 
    public bool Visible { get; set; }
    public string Description { get; set; }
    public string Histoire { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual CategoryImage Image { get; set; }

    public CategoryType? CategoryType { get; set; }


    protected Category()
    {

    }

    public Category(Guid id,  string label, bool visible,string description,string histoire, CategoryType? type, DateTime createdAt, CategoryImage image)
    {
        Id = id;
        Label = label;
        Description= description;
        Histoire = histoire;
        CreatedAt = createdAt; 
        Visible = visible;
        Image = image;
        CategoryType = type;
    }

}
