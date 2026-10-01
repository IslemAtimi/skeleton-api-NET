namespace AB.Ecommerce.Gros.Business;

public class ClientFile
{
    public int Id { get; set; }
    public Guid ClientId { get; set; }
    public MediaType FileType { get; set; }
    public string Name { get; set; }
    public string MimeType { get; set; }
    public byte[] Content { get; set; }

    protected ClientFile()
    {

    }

    public ClientFile(Guid clientId, MediaType fileType, string name, string mimeType, byte[] content)
    {
        ClientId = clientId;
        FileType = fileType;
        Name = name;
        MimeType = mimeType;
        Content = content; 
    }

    public ClientFile(int id, Guid clientId, MediaType fileType, string name, string mimeType, byte[] content)
    {
        Id = id;
        ClientId = clientId;
        FileType = fileType;
        Name = name;
        MimeType = mimeType;
        Content = content;
    }

}



 