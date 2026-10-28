using OnlineStoreLinq.DataAccess;
using OnlineStoreLinq.Models;
using OnlineStoreLinq.Queries;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

using var db = new StoreDbContext();

Order CreateOrder(
    string customerName,
    DateTime orderDate,
    OrderStatus status,
    params (string ProductName, int Quantity, decimal Discount)[] lines)
{
    var order = new Order
    {
        Customer = db.Customers.Single(c => c.FullName == customerName),
        OrderDate = orderDate,
        Status = status
    };

    foreach (var line in lines)
    {
        var product = db.Products.Single(p => p.Name == line.ProductName);

        order.Items.Add(new OrderItem
        {
            Product = product,
            Quantity = line.Quantity,
            UnitPrice = product.Price,
            Discount = line.Discount
        });
    }

    return order;
}

if (!db.Orders.Any())
{
    db.Orders.AddRange(
        CreateOrder("Amina Hassan", new DateTime(2025, 1, 15),
            OrderStatus.Delivered,
            ("Lenovo ThinkPad", 1, 0.05m),
            ("Wireless Mouse", 2, 0m)),

        CreateOrder("Amina Hassan", new DateTime(2025, 3, 10),
            OrderStatus.Delivered,
            ("MacBook Air", 1, 0.10m),
            ("USB-C Hub", 1, 0m)),

        CreateOrder("Omar Ali", new DateTime(2025, 2, 12),
            OrderStatus.Delivered,
            ("Dell XPS", 1, 0m),
            ("USB-C Hub", 2, 0.05m)),

        CreateOrder("Nour Adel", new DateTime(2025, 4, 3),
            OrderStatus.Delivered,
            ("iPhone", 1, 0m),
            ("Galaxy S24", 1, 0.05m)),

        CreateOrder("Youssef Samy", new DateTime(2025, 5, 10),
            OrderStatus.Delivered,
            ("Lenovo ThinkPad", 1, 0m),
            ("Galaxy S24", 1, 0m)),

        CreateOrder("Salma Nabil", new DateTime(2025, 6, 17),
            OrderStatus.Delivered,
            ("iPhone", 1, 0.05m),
            ("Wireless Mouse", 1, 0m)),

        CreateOrder("Omar Ali", new DateTime(2026, 10, 18),
            OrderStatus.Pending,
            ("Lenovo ThinkPad", 5, 0m),
            ("Wireless Mouse", 4, 0m)),

        CreateOrder("Nour Adel", new DateTime(2026, 1, 15),
            OrderStatus.Pending,
            ("MacBook Air", 2, 0m),
            ("Galaxy S24", 3, 0m)),

        CreateOrder("Nour Adel", new DateTime(2026, 2, 2),
            OrderStatus.Shipped,
            ("iPhone", 1, 0m)),

        CreateOrder("Amina Hassan", new DateTime(2024, 8, 1),
            OrderStatus.Cancelled,
            ("Lenovo ThinkPad", 2, 0m)),

        CreateOrder("Amina Hassan", new DateTime(2025, 4, 20),
            OrderStatus.Cancelled,
            ("MacBook Air", 1, 0m)),

        CreateOrder("Amina Hassan", new DateTime(2026, 9, 1),
            OrderStatus.Cancelled,
            ("Galaxy S24", 1, 0m)),

        CreateOrder("Salma Nabil", new DateTime(2026, 10, 2),
            OrderStatus.Cancelled,
            ("iPhone", 1, 0m))
    );

    db.SaveChanges();
}

Review CreateReview(
    string customerName,
    string productName,
    int rating,
    string comment,
    int daysAgo)
{
    return new Review
    {
        Customer = db.Customers.Single(c => c.FullName == customerName),
        Product = db.Products.Single(p => p.Name == productName),
        Rating = rating,
        Comment = comment,
        CreatedAt = DateTime.Now.AddDays(-daysAgo)
    };
}

if (!db.Reviews.Any())
{
    db.Reviews.AddRange(
        CreateReview("Amina Hassan", "Lenovo ThinkPad", 5, "Excellent laptop", 10),
        CreateReview("Omar Ali", "Lenovo ThinkPad", 5, "Very reliable", 15),
        CreateReview("Nour Adel", "Lenovo ThinkPad", 5, "Fast and useful", 20),
        CreateReview("Youssef Samy", "Lenovo ThinkPad", 5, "Great quality", 25),
        CreateReview("Salma Nabil", "Lenovo ThinkPad", 5, "Good value", 30),
        CreateReview("Karim Hany", "Lenovo ThinkPad", 5, "Looks great", 35),
        CreateReview("Amina Hassan", "Lenovo ThinkPad", 5, "Would recommend", 40),
        CreateReview("Omar Ali", "Lenovo ThinkPad", 4, "Good, but pricey", 45),
        CreateReview("Nour Adel", "Lenovo ThinkPad", 5, "Works perfectly", 50),
        CreateReview("Youssef Samy", "Lenovo ThinkPad", 5, "Very happy with it", 55)
    );

    db.SaveChanges();
}

Console.WriteLine($"Orders: {db.Orders.Count()}");
Console.WriteLine($"Order items: {db.OrderItems.Count()}");

if (!db.Categories.Any())
{
    var laptops = new Category { Name = "Laptops" };
    var accessories = new Category { Name = "Accessories" };
    var phones = new Category { Name = "Phones" };
    var monitors = new Category { Name = "Monitors" };

    db.Categories.AddRange(laptops, accessories, phones, monitors);
    db.SaveChanges();

    db.Products.AddRange(
        new Product { Name = "Lenovo ThinkPad", Price = 1250m, Stock = 4, Category = laptops },
        new Product { Name = "MacBook Air", Price = 1100m, Stock = 3, Category = laptops },
        new Product { Name = "Dell XPS", Price = 1700m, Stock = 1, Category = laptops },
        new Product { Name = "USB-C Hub", Price = 45m, Stock = 12, Category = accessories },
        new Product { Name = "Wireless Mouse", Price = 25m, Stock = 3, Category = accessories },
        new Product { Name = "iPhone", Price = 900m, Stock = 8, Category = phones },
        new Product { Name = "Galaxy S24", Price = 800m, Stock = 6, Category = phones },
        new Product { Name = "Dell Monitor", Price = 350m, Stock = 5, Category = monitors }
    );

    db.SaveChanges();
}
if (!db.Customers.Any())
{
    db.Customers.AddRange(
        new Customer
        {
            FullName = "Amina Hassan",
            Email = "amina@example.com",
            City = "Cairo",
            RegisteredAt = DateTime.Now.AddDays(-10)
        },
        new Customer
        {
            FullName = "Omar Ali",
            Email = "omar@example.com",
            City = "Alexandria",
            RegisteredAt = DateTime.Now.AddDays(-40)
        },
        new Customer
        {
            FullName = "Nour Adel",
            Email = "nour@example.com",
            City = "Cairo",
            RegisteredAt = DateTime.Now.AddDays(-20)
        },
        new Customer
        {
            FullName = "Youssef Samy",
            Email = "youssef@example.com",
            City = "Giza",
            RegisteredAt = DateTime.Now.AddDays(-300)
        },
        new Customer
        {
            FullName = "Salma Nabil",
            Email = "salma@example.com",
            City = "Alexandria",
            RegisteredAt = DateTime.Now.AddDays(-60)
        },
        new Customer
        {
            FullName = "Karim Hany",
            Email = "karim@example.com",
            City = "Cairo",
            RegisteredAt = DateTime.Now.AddDays(-5)
        }
    );

    db.SaveChanges();
}

Console.WriteLine($"Categories: {db.Categories.Count()}");
Console.WriteLine($"Products: {db.Products.Count()}");
Console.WriteLine($"Customers: {db.Customers.Count()}");
Console.WriteLine($"Reviews: {db.Reviews.Count()}");

foreach (var order in StoreLinqQueries.Q1(db))
{
    Console.WriteLine(
        $"Order {order.OrderId} | {order.CustomerName} | {order.Total:C}");
}

Console.WriteLine("\nOrders by status:");

foreach (var item in StoreLinqQueries.Q2(db))
{
    Console.WriteLine(
        $"{item.Status}: {item.OrderCount} ({item.Percentage:F1}%)");
}

Console.WriteLine("\nProducts above their category average:");

foreach (var product in StoreLinqQueries.Q3(db))
{
    Console.WriteLine($"{product.Name}: {product.Price:C}");
}

void Show<T>(string title, Func<T> getResult)
{
    Console.WriteLine($"\n--- {title} ---");

    try
    {
        var result = getResult();
        Console.WriteLine(JsonSerializer.Serialize(
            result,
            new JsonSerializerOptions { WriteIndented = true }));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"تعذر تشغيل {title}: {ex.Message}");
    }
}

Show("Q1", () => StoreLinqQueries.Q1(db).ToList());
Show("Q2", () => StoreLinqQueries.Q2(db).ToList());
Show("Q3", () => StoreLinqQueries.Q3(db).ToList());
Show("Q4", () => StoreLinqQueries.Q4(db).ToList());
Show("Q5", () => StoreLinqQueries.Q5(db).ToList());
Show("Q6", () => StoreLinqQueries.Q6(db).ToList());
Show("Q7", () => StoreLinqQueries.Q7(db).ToList());
Show("Q8", () => StoreLinqQueries.Q8(db).ToList());
Show("Q8 SQL", () => StoreLinqQueries.Q8(db).ToQueryString());
Show("Q9", () => StoreLinqQueries.Q9(db).ToList());
Show("Q10", () => StoreLinqQueries.Q10(db).ToList());
Show("Q11", () => StoreLinqQueries.Q11(db).ToList());
Show("Q12", () => StoreLinqQueries.Q12(db).ToList());
Show("Q13", () => StoreLinqQueries.Q13(db).ToList());
Show("Q14", () => StoreLinqQueries.Q14(db).ToList());
Show("Q15", () => StoreLinqQueries.Q15(db).ToList());
Show("Q16", () => StoreLinqQueries.Q16(db).ToList());
Show("Q17", () => StoreLinqQueries.Q17(db).ToList());
Show("Q18", () => StoreLinqQueries.Q18(db).ToList());
Show("Q19", () => StoreLinqQueries.Q19(db).ToList());
Show("Q20", () => StoreLinqQueries.Q20(db).ToList());
Show("Q21", () => StoreLinqQueries.Q21(db).ToList());
Show("Q22", () => StoreLinqQueries.Q22(db));
Show("Q23", () => StoreLinqQueries.SearchProducts(db, new ProductFilter

{
    Text = "Think",
    SortBy = "rating",
    Descending = true,
    Page = 1,
    PageSize = 5
}));

Console.WriteLine("\n--- Q24 ---");
Console.WriteLine(StoreLinqQueries.Q24(db));

Console.WriteLine("\n--- Q25 ---");
Console.WriteLine(StoreLinqQueries.Q25(db));