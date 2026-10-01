using System.ComponentModel.DataAnnotations;
using AB.Ecommerce.Gros.Business;

namespace AB.Ecommerce.Gros.Application;

public class RegisterInput
{


    public string? Name { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public FileInput[]? Files  { get; set; }

    public void Validate()
    {
        if (string.IsNullOrEmpty(Name))
        {
            throw new BusinessException(BusinessErrors.RequiredField, "Nom de magasin");
        }
        if (string.IsNullOrEmpty(Username))
        {
            throw new BusinessException(BusinessErrors.RequiredField, "Nom d'utilisateur");
        }
        if (string.IsNullOrEmpty(Password))
        {
            throw new BusinessException(BusinessErrors.RequiredField, "Mot de passe");
        }
        if (Password.Length<8)
        {
            throw new BusinessException(BusinessErrors.MinPasswordLength, "8");
        }
        if (Password.Length > 20)
        {
            throw new BusinessException(BusinessErrors.MaxPasswordLength, "20");
        }
        if (string.IsNullOrEmpty(Lastname))
        {
            throw new BusinessException(BusinessErrors.RequiredField, "Nom");
        }
        if (string.IsNullOrEmpty(Firstname))
        {
            throw new BusinessException(BusinessErrors.RequiredField, "Prénom");
        }
        if (string.IsNullOrEmpty(Email))
        {
            throw new BusinessException(BusinessErrors.RequiredField, "Email");
        }
        if (string.IsNullOrEmpty(PhoneNumber))
        {
            throw new BusinessException(BusinessErrors.RequiredField, "Numéro de téléphone");
        }
    }


}

