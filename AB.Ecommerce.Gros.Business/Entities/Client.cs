namespace AB.Ecommerce.Gros.Business;

public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string ForgetPaasword { get; set; }

    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public bool IsActive { get; set; }
    public DateTime RegistredAt { get; set; }

    public virtual ICollection<ClientFile> Files { get; set; }

    protected Client()
    {

    }

    public Client(Guid id, string name, string firstname, string lastname, string forgetPassword, string phoneNumber, string email, string address, DateTime registeredAt)
    {
        Id = id;
        Name = name;
        Firstname = firstname;
        Lastname = lastname;
        ForgetPaasword= forgetPassword;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
        Files = new List<ClientFile>();
        RegistredAt = registeredAt;
        IsActive = false;
    }

    public void Validate()
    {
        IsActive = true;
    }

    public void Invalidate()
    {
        IsActive = false;
    }

}

