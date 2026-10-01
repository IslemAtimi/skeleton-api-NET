using System;
using AB.Ecommerce.Gros.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AB.Ecommerce.Gros.Data;


public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseMySql("Server=localhost;Database=guste;User=root;Password=P@$$w0rd;Port=3306;", new MySqlServerVersion("8.0.28"));

        return new AppDbContext(optionsBuilder.Options);
    }
}
