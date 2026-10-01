namespace AB.Ecommerce.Gros.Application;

public class FileInput {
    public string Name { get; set; }
    public MediaType MediaType { get; set; }
    public string? MimeType { get; set; }
    public byte[]? Content { get; set; }

    public string? Url { get; set; }
}

