using AB.Ecommerce.Gros.Business;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AB.Ecommerce.Gros.Data;


public class AppDbContext: IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientFile> ClientFiles { get; set; }
   
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryImage> CategoryImages { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>(b =>
        {
            b.ToTable("Clients");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).IsRequired();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Firstname).IsRequired();
            b.Property(x => x.Lastname).IsRequired();
            b.Property(x => x.Email).IsRequired();
            b.Property(x => x.PhoneNumber).IsRequired();
            b.Property(x => x.Address).IsRequired(false);
            b.Property(x => x.RegistredAt).IsRequired(true);//.HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);

            b.HasMany(x => x.Files).WithOne().HasForeignKey(f => f.ClientId);


        });

        modelBuilder.Entity<ClientFile>(b =>
        {
            b.ToTable("ClientFiles");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).IsRequired();
            b.Property(x => x.ClientId).IsRequired();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.MimeType).IsRequired();
            b.Property(x => x.Content).IsRequired();
            b.Property(x => x.FileType).IsRequired();
        });

        modelBuilder.Entity<Category>(b =>
        {
            b.ToTable("Categories");
            b.HasKey(x => x.Id);
            b.Property(x => x.Label).IsRequired();
            b.Property(x => x.Visible).IsRequired();
            b.Property(x => x.Description).IsRequired(false);
            b.Property(x => x.Histoire).IsRequired(false);
            b.Property(x => x.CreatedAt).IsRequired();

            b.HasOne(x => x.Image).WithOne().HasForeignKey<CategoryImage>(f => f.CategoryId);

        });

        modelBuilder.Entity<CategoryImage>(b =>
        {
            b.ToTable("CategoryImages");
            b.HasKey(x => x.CategoryId);
            b.Property(x => x.MimeType).IsRequired();
            b.Property(x => x.Content).IsRequired();

        });
    }
     
}

