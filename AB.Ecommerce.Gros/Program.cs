using System.Net.NetworkInformation;
using System.Text;
using AB.Ecommerce.Gros;
using AB.Ecommerce.Gros.Application;
using AB.Ecommerce.Gros.Data;
using AB.Ecommerce.Gros.Shared;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Stripe;
using Application = AB.Ecommerce.Gros.Application.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 200;
    options.Limits.MaxRequestBufferSize = 1024 * 1024 * 200;
});


builder.Services.Configure<FormOptions>(options =>
{
    // Adjust the limit as needed
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.BufferBodyLengthLimit = long.MaxValue;
    options.MultipartBoundaryLengthLimit = int.MaxValue;
    options.ValueLengthLimit = int.MaxValue;
});

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
string version = builder.Configuration["ConnectionStrings:Version"];

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(
    connectionString,
    new MySqlServerVersion(version),
    mySqlOptions => mySqlOptions.EnableRetryOnFailure())
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


var stripeSecretKey = builder.Configuration["Stripe:SecretKey"];
StripeConfiguration.ApiKey = stripeSecretKey;



var secret = builder.Configuration["Jwt:SecretKey"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };
            });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

Data.Configure(builder.Services, builder.Configuration);

Application.Configure(builder.Services, builder.Configuration);





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var useHttps = bool.Parse(builder.Configuration["UseHttps"]);

var app = builder.Build();


ServiceLocator.SetServiceProvider(app.Services);

await DataSeeder.SeedDataAsync(app.Services);

// Configure the HTTP request pipeline.
 
    app.UseSwagger();
    app.UseSwaggerUI();


if (useHttps)
{
    app.UseHttpsRedirection();
} 

app.UseAuthentication();

app.UseAuthorization();

app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                                                        //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
                    .AllowCredentials());

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();

