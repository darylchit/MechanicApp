using MechanicApp.Data;
using MechanicApp.Interfaces;
using MechanicApp.Models;
using MechanicApp.Repositories;
using MechanicApp.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Database Context (Step 6 - Configure EF Core)
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity Services (Authentication & Authorization)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings (customize as needed)
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();

// Register AutoMapper
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<MappingProfiles>();
});

// Register Repositories (Dependency Injection)
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IMechanicRepository, MechanicRepository>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed the database in development
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DataContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            await Seed.SeedData(context, userManager);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred during seeding the database.");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Add this BEFORE UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
