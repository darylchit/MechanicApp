using MechanicApp.Data;
using MechanicApp.Models;
using Microsoft.AspNetCore.Identity;

namespace MechanicApp.Data;

public class Seed
{
    public static async Task SeedData(DataContext context, UserManager<ApplicationUser> userManager)
    {
        // Check if data already exists
        if (context.Clients.Any() || context.Mechanics.Any() || context.Shops.Any())
        {
            Console.WriteLine("Database already seeded. Skipping seed data.");
            return;
        }

        Console.WriteLine("Starting database seeding...");

        // 1. Create Users
        var clientUser1 = new ApplicationUser
        {
            UserName = "john.client@example.com",
            Email = "john.client@example.com",
            FirstName = "John",
            LastName = "Smith",
            PhoneNumber = "555-0101",
            EmailConfirmed = true
        };

        var clientUser2 = new ApplicationUser
        {
            UserName = "sarah.client@example.com",
            Email = "sarah.client@example.com",
            FirstName = "Sarah",
            LastName = "Johnson",
            PhoneNumber = "555-0102",
            EmailConfirmed = true
        };

        var mechanicUser1 = new ApplicationUser
        {
            UserName = "mike.mechanic@example.com",
            Email = "mike.mechanic@example.com",
            FirstName = "Mike",
            LastName = "Rodriguez",
            PhoneNumber = "555-0201",
            EmailConfirmed = true
        };

        var mechanicUser2 = new ApplicationUser
        {
            UserName = "lisa.mechanic@example.com",
            Email = "lisa.mechanic@example.com",
            FirstName = "Lisa",
            LastName = "Chen",
            PhoneNumber = "555-0202",
            EmailConfirmed = true
        };

        // Create users with password
        var password = "Test123!";
        await userManager.CreateAsync(clientUser1, password);
        await userManager.CreateAsync(clientUser2, password);
        await userManager.CreateAsync(mechanicUser1, password);
        await userManager.CreateAsync(mechanicUser2, password);

        Console.WriteLine("✓ Created 4 users");

        // 2. Create Shops
        var shop1 = new Shop
        {
            Name = "AutoCare Center",
            Address = "123 Main Street, Springfield, IL 62701",
            PhoneNumber = "555-1000",
            Email = "info@autocarecentre.com",
            CreatedAt = DateTime.UtcNow.AddMonths(-6)
        };

        var shop2 = new Shop
        {
            Name = "Quick Fix Auto Repair",
            Address = "456 Oak Avenue, Springfield, IL 62702",
            PhoneNumber = "555-2000",
            Email = "contact@quickfixauto.com",
            CreatedAt = DateTime.UtcNow.AddMonths(-12)
        };

        context.Shops.AddRange(shop1, shop2);
        await context.SaveChangesAsync();

        Console.WriteLine("✓ Created 2 shops");

        // 3. Create Clients
        var client1 = new Client
        {
            UserId = clientUser1.Id,
            Address = "789 Elm Street, Springfield, IL 62703",
            CreatedAt = DateTime.UtcNow.AddMonths(-3)
        };

        var client2 = new Client
        {
            UserId = clientUser2.Id,
            Address = "321 Pine Road, Springfield, IL 62704",
            CreatedAt = DateTime.UtcNow.AddMonths(-2)
        };

        context.Clients.AddRange(client1, client2);
        await context.SaveChangesAsync();

        Console.WriteLine("✓ Created 2 clients");

        // 4. Create Mechanics
        var mechanic1 = new Mechanic
        {
            UserId = mechanicUser1.Id,
            ShopId = shop1.Id,
            Specialization = "Engine Repair & Diagnostics",
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow.AddMonths(-5)
        };

        var mechanic2 = new Mechanic
        {
            UserId = mechanicUser2.Id,
            ShopId = shop2.Id,
            Specialization = "Brake Systems & Suspension",
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow.AddMonths(-4)
        };

        context.Mechanics.AddRange(mechanic1, mechanic2);
        await context.SaveChangesAsync();

        Console.WriteLine("✓ Created 2 mechanics");

        // 5. Create Vehicles
        var vehicle1 = new Vehicle
        {
            ClientId = client1.Id,
            Make = "Toyota",
            Model = "Camry",
            Year = 2020,
            LicensePlate = "ABC-1234",
            VIN = "1HGBH41JXMN109186",
            Color = "Silver",
            Mileage = 45000,
            CreatedAt = DateTime.UtcNow.AddMonths(-3)
        };

        var vehicle2 = new Vehicle
        {
            ClientId = client1.Id,
            Make = "Honda",
            Model = "Accord",
            Year = 2019,
            LicensePlate = "XYZ-5678",
            VIN = "2HGFC2F59KH123456",
            Color = "Blue",
            Mileage = 52000,
            CreatedAt = DateTime.UtcNow.AddMonths(-2)
        };

        var vehicle3 = new Vehicle
        {
            ClientId = client2.Id,
            Make = "Ford",
            Model = "F-150",
            Year = 2021,
            LicensePlate = "DEF-9012",
            VIN = "1FTFW1E84MKE12345",
            Color = "Black",
            Mileage = 28000,
            CreatedAt = DateTime.UtcNow.AddMonths(-2)
        };

        context.Vehicles.AddRange(vehicle1, vehicle2, vehicle3);
        await context.SaveChangesAsync();

        Console.WriteLine("✓ Created 3 vehicles");

        // 6. Create Service Requests
        var serviceRequest1 = new ServiceRequest
        {
            ClientId = client1.Id,
            VehicleId = vehicle1.Id,
            MechanicId = mechanic1.Id,
            Title = "Oil Change & Filter Replacement",
            Description = "Regular maintenance - 5,000 mile oil change",
            Status = ServiceStatus.Completed,
            CreatedAt = DateTime.UtcNow.AddDays(-30),
            ScheduledAt = DateTime.UtcNow.AddDays(-28),
            CompletedAt = DateTime.UtcNow.AddDays(-28),
            EstimatedCost = 75.00m,
            FinalCost = 72.50m
        };

        var serviceRequest2 = new ServiceRequest
        {
            ClientId = client1.Id,
            VehicleId = vehicle1.Id,
            MechanicId = mechanic1.Id,
            Title = "Brake Inspection",
            Description = "Customer reports squeaking noise when braking",
            Status = ServiceStatus.InProgress,
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            ScheduledAt = DateTime.UtcNow.AddDays(-2),
            EstimatedCost = 150.00m
        };

        var serviceRequest3 = new ServiceRequest
        {
            ClientId = client2.Id,
            VehicleId = vehicle3.Id,
            MechanicId = mechanic2.Id,
            Title = "Tire Rotation & Alignment",
            Description = "Scheduled maintenance",
            Status = ServiceStatus.Pending,
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            ScheduledAt = DateTime.UtcNow.AddDays(3),
            EstimatedCost = 120.00m
        };

        var serviceRequest4 = new ServiceRequest
        {
            ClientId = client1.Id,
            VehicleId = vehicle2.Id,
            MechanicId = null,
            Title = "Check Engine Light Diagnostic",
            Description = "Engine light came on yesterday",
            Status = ServiceStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            EstimatedCost = null
        };

        context.ServiceRequests.AddRange(serviceRequest1, serviceRequest2, serviceRequest3, serviceRequest4);
        await context.SaveChangesAsync();

        Console.WriteLine("✓ Created 4 service requests");

        // 7. Create Conversations
        var conversation1 = new Conversation
        {
            ClientId = client1.Id,
            MechanicId = mechanic1.Id,
            ServiceRequestId = serviceRequest2.Id,
            Subject = "Brake Service Follow-up",
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddDays(-4),
            LastMessageAt = DateTime.UtcNow.AddDays(-1)
        };

        var conversation2 = new Conversation
        {
            ClientId = client2.Id,
            MechanicId = mechanic2.Id,
            ServiceRequestId = serviceRequest3.Id,
            Subject = "Tire Service Appointment",
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            LastMessageAt = DateTime.UtcNow.AddHours(-2)
        };

        context.Conversations.AddRange(conversation1, conversation2);
        await context.SaveChangesAsync();

        Console.WriteLine("✓ Created 2 conversations");

        // 8. Create Messages
        var message1 = new Message
        {
            ConversationId = conversation1.Id,
            SenderId = clientUser1.Id,
            Content = "Hi, I wanted to check on the status of my brake inspection.",
            Status = MessageStatus.Read,
            SentAt = DateTime.UtcNow.AddDays(-4),
            ReadAt = DateTime.UtcNow.AddDays(-4).AddHours(2)
        };

        var message2 = new Message
        {
            ConversationId = conversation1.Id,
            SenderId = mechanicUser1.Id,
            Content = "We've completed the inspection. Your front brake pads need replacement. I'll send you a quote shortly.",
            Status = MessageStatus.Read,
            SentAt = DateTime.UtcNow.AddDays(-3),
            ReadAt = DateTime.UtcNow.AddDays(-3).AddHours(1)
        };

        var message3 = new Message
        {
            ConversationId = conversation1.Id,
            SenderId = clientUser1.Id,
            Content = "Great, please go ahead with the replacement.",
            Status = MessageStatus.Read,
            SentAt = DateTime.UtcNow.AddDays(-2),
            ReadAt = DateTime.UtcNow.AddDays(-2).AddMinutes(30)
        };

        var message4 = new Message
        {
            ConversationId = conversation2.Id,
            SenderId = clientUser2.Id,
            Content = "Can I reschedule my appointment to next Friday?",
            Status = MessageStatus.Sent,
            SentAt = DateTime.UtcNow.AddHours(-2)
        };

        context.Messages.AddRange(message1, message2, message3, message4);
        await context.SaveChangesAsync();

        Console.WriteLine("✓ Created 4 messages");
        Console.WriteLine("✅ Database seeding completed successfully!");
        Console.WriteLine();
        Console.WriteLine("Test Login Credentials:");
        Console.WriteLine("========================");
        Console.WriteLine("Clients:");
        Console.WriteLine("  Email: john.client@example.com | Password: Test123!");
        Console.WriteLine("  Email: sarah.client@example.com | Password: Test123!");
        Console.WriteLine();
        Console.WriteLine("Mechanics:");
        Console.WriteLine("  Email: mike.mechanic@example.com | Password: Test123!");
        Console.WriteLine("  Email: lisa.mechanic@example.com | Password: Test123!");
    }
}
