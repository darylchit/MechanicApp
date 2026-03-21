# Database Seed Data Reference

## 🌱 Seeding Overview
The database will be automatically seeded when you run the application in **Development** mode.

---

## 📊 Test Data Created

### **Users (4 total)**
All passwords: `Test123!`

| Email | First Name | Last Name | Phone | Type |
|-------|------------|-----------|-------|------|
| john.client@example.com | John | Smith | 555-0101 | Client |
| sarah.client@example.com | Sarah | Johnson | 555-0102 | Client |
| mike.mechanic@example.com | Mike | Rodriguez | 555-0201 | Mechanic |
| lisa.mechanic@example.com | Lisa | Chen | 555-0202 | Mechanic |

---

### **Shops (2 total)**

| ID | Name | Address | Phone |
|----|------|---------|-------|
| 1 | AutoCare Center | 123 Main Street, Springfield, IL 62701 | 555-1000 |
| 2 | Quick Fix Auto Repair | 456 Oak Avenue, Springfield, IL 62702 | 555-2000 |

---

### **Clients (2 total)**

| ID | User | Address |
|----|------|---------|
| 1 | John Smith | 789 Elm Street, Springfield, IL 62703 |
| 2 | Sarah Johnson | 321 Pine Road, Springfield, IL 62704 |

---

### **Mechanics (2 total)**

| ID | User | Shop | Specialization | Available |
|----|------|------|----------------|-----------|
| 1 | Mike Rodriguez | AutoCare Center | Engine Repair & Diagnostics | ✅ Yes |
| 2 | Lisa Chen | Quick Fix Auto Repair | Brake Systems & Suspension | ✅ Yes |

---

### **Vehicles (3 total)**

| ID | Client | Make | Model | Year | License Plate | Mileage |
|----|--------|------|-------|------|---------------|---------|
| 1 | John Smith | Toyota | Camry | 2020 | ABC-1234 | 45,000 |
| 2 | John Smith | Honda | Accord | 2019 | XYZ-5678 | 52,000 |
| 3 | Sarah Johnson | Ford | F-150 | 2021 | DEF-9012 | 28,000 |

---

### **Service Requests (4 total)**

| ID | Client | Vehicle | Mechanic | Title | Status | Estimated Cost |
|----|--------|---------|----------|-------|--------|----------------|
| 1 | John Smith | Toyota Camry | Mike Rodriguez | Oil Change & Filter Replacement | ✅ Completed | $72.50 |
| 2 | John Smith | Toyota Camry | Mike Rodriguez | Brake Inspection | 🔧 InProgress | $150.00 |
| 3 | Sarah Johnson | Ford F-150 | Lisa Chen | Tire Rotation & Alignment | ⏳ Pending | $120.00 |
| 4 | John Smith | Honda Accord | Unassigned | Check Engine Light Diagnostic | ⏳ Pending | TBD |

---

### **Conversations (2 total)**

| ID | Client | Mechanic | Service Request | Subject | Active |
|----|--------|----------|-----------------|---------|--------|
| 1 | John Smith | Mike Rodriguez | #2 (Brake Service) | Brake Service Follow-up | ✅ Yes |
| 2 | Sarah Johnson | Lisa Chen | #3 (Tire Service) | Tire Service Appointment | ✅ Yes |

---

### **Messages (4 total)**

| Conversation | Sender | Content | Status |
|--------------|--------|---------|--------|
| #1 | John Smith | "Hi, I wanted to check on the status..." | Read |
| #1 | Mike Rodriguez | "We've completed the inspection..." | Read |
| #1 | John Smith | "Great, please go ahead..." | Read |
| #2 | Sarah Johnson | "Can I reschedule my appointment..." | Sent |

---

## 🚀 How to Use

### **Option 1: Automatic Seeding (Recommended)**
1. **Stop your debugger** (Shift+F5)
2. **Restart the app** (F5)
3. The database will be seeded automatically on startup
4. Check the console for seeding confirmation messages

### **Option 2: Clear & Reseed**
If you want to clear existing data and reseed:

```powershell
# In Package Manager Console or Terminal
dotnet ef database drop
dotnet ef database update
# Then restart the app (F5)
```

---

## 📝 Testing with Seed Data

### Test these endpoints immediately:

```http
### Get all clients (should return 2)
GET https://localhost:7059/api/Clients

### Get all mechanics (should return 2)
GET https://localhost:7059/api/Mechanics

### Get all shops (should return 2)
GET https://localhost:7059/api/Shops

### Get all vehicles (should return 3)
GET https://localhost:7059/api/Vehicles

### Get all service requests (should return 4)
GET https://localhost:7059/api/ServiceRequests

### Get service requests by status
GET https://localhost:7059/api/ServiceRequests/status/Pending

### Get John Smith's vehicles (should return 2)
GET https://localhost:7059/api/Clients/1/vehicles

### Get Mike Rodriguez's service requests
GET https://localhost:7059/api/Mechanics/1/servicerequests
```

---

## 🎯 Key Features

- ✅ **Realistic relationships** between all entities
- ✅ **Various service request statuses** (Pending, InProgress, Completed)
- ✅ **Multiple vehicles per client** for testing
- ✅ **Active conversations with messages**
- ✅ **Completed and ongoing service requests**
- ✅ **Test login credentials** for both clients and mechanics

---

## ⚠️ Notes

- Seeding only runs if the database is **empty** (no existing clients/mechanics/shops)
- All test users use the password: **`Test123!`**
- Seed data is only created in **Development** environment
- Check the console output for seeding status and any errors
