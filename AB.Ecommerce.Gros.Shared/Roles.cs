using System;
using System.Reflection;

namespace AB.Ecommerce.Gros;

public static class Roles
{

    public const string Customer = "Customer";

    public static string[] GetAll()
    {
        return new[] { Customer };
    }

    public static string GetDefault()
    {
        return Customer;
    }
}


public static class SystemRoles
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";


    public static string[] GetAll()
    {
        return new[] { Admin,Manager };
    }

     
}

public static class CustomClaims
{
    public const string ConstraintsEnabled = "ConstraintsEnabled";

}

