namespace AB.Ecommerce.Gros.Business;

public class CategoryImage
{
    public Guid CategoryId { get; set; } 
    public string MimeType { get; set; }
    public byte[] Content { get; set; }

    protected CategoryImage()
    {

    } 

    public CategoryImage(Guid categoryId, string mimeType, byte[] content)
    {
        CategoryId = categoryId; 
        MimeType = mimeType;
        Content = content;
    }

}
