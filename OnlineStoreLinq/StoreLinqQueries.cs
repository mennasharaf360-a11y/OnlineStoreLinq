using Microsoft.EntityFrameworkCore;
using OnlineStoreLinq.Models;

namespace OnlineStoreLinq.Queries;

/// <summary>
/// LINQ answers for the Online Store assignment. Methods return IQueryable where
/// practical so the query can be composed and translated by EF Core.
/// </summary>
public static class StoreLinqQueries
{
    // 1. Delivered order total, customer name, and order ID.
    public static IQueryable<DeliveredOrderTotalDto> Q1(DbContext db) =>
        db.Set<Order>()
            .Where(o => o.Status == OrderStatus.Delivered)
            .Select(o => new DeliveredOrderTotalDto
            {
                OrderId = o.Id,
                CustomerName = o.Customer.FullName,
                Total = o.Items.Sum(i => i.Quantity * i.UnitPrice * (1m - i.Discount))
            })
            .OrderByDescending(x => x.Total);

    // 2. Order count and percentage by status.
    public static IQueryable<OrderStatusCountDto> Q2(DbContext db)
    {
        var totalOrders = db.Set<Order>().Count();
        return db.Set<Order>()
            .GroupBy(o => o.Status)
            .Select(g => new OrderStatusCountDto
            {
                Status = g.Key,
                OrderCount = g.Count(),
                Percentage = totalOrders == 0 ? 0 : g.Count() * 100.0 / totalOrders
            })
            .OrderBy(x => x.Status);
    }

    // 3. Products priced above the average price in their own category.
    public static IQueryable<Product> Q3(DbContext db) =>
        db.Set<Product>()
            .Where(p => p.Price > db.Set<Product>()
                .Where(other => other.CategoryId == p.CategoryId)
                .Average(other => other.Price));

    // 4. Products that have never appeared in an order.
    public static IQueryable<Product> Q4(DbContext db) =>
        db.Set<Product>().Where(p => !p.OrderItems.Any());

    // 5. Customers registered within the last 30 days who have at least one order.
    public static IQueryable<Customer> Q5(DbContext db)
    {
        var since = DateTime.Now.AddDays(-30);
        return db.Set<Customer>()
            .Where(c => c.RegisteredAt >= since && c.Orders.Any());
    }

    // 6. Customers with more than two cancelled orders.
    public static IQueryable<CustomerCancelledOrdersDto> Q6(DbContext db) =>
        db.Set<Customer>()
            .Select(c => new CustomerCancelledOrdersDto
            {
                CustomerName = c.FullName,
                CancelledOrderCount = c.Orders.Count(o => o.Status == OrderStatus.Cancelled)
            })
            .Where(x => x.CancelledOrderCount > 2);

    // 7. Top five customers by spending on delivered orders.
    public static IQueryable<CustomerSpendingSummaryDto> Q7(DbContext db) =>
        db.Set<Customer>()
            .Select(c => new CustomerSpendingSummaryDto
            {
                CustomerName = c.FullName,
                OrderCount = c.Orders.Count(o => o.Status == OrderStatus.Delivered),
                TotalSpent = c.Orders
                    .Where(o => o.Status == OrderStatus.Delivered)
                    .SelectMany(o => o.Items)
                    .Sum(i => i.Quantity * i.UnitPrice * (1m - i.Discount)),
                AverageOrderValue = c.Orders
                    .Where(o => o.Status == OrderStatus.Delivered)
                    .Select(o => o.Items.Sum(i => i.Quantity * i.UnitPrice * (1m - i.Discount)))
                    .DefaultIfEmpty(0m)
                    .Average()
            })
            .Where(c => c.OrderCount > 0)
            .OrderByDescending(c => c.TotalSpent)
            .Take(5);

    // 8. Count all orders in each month of 2025; revenue includes delivered orders only.
    // ToQueryString() can be called on this query.
    public static IQueryable<MonthlyReportDto> Q8(DbContext db) =>
        db.Set<Order>()
            .Where(o => o.OrderDate >= new DateTime(2025, 1, 1) &&
                        o.OrderDate < new DateTime(2026, 1, 1))
            .GroupBy(o => new { Year = o.OrderDate.Year, Month = o.OrderDate.Month })
            .Select(g => new MonthlyReportDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                OrderCount = g.Count(),
                Revenue = g.Where(o => o.Status == OrderStatus.Delivered)
                    .SelectMany(o => o.Items)
                    .Sum(i => i.Quantity * i.UnitPrice * (1m - i.Discount))
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month);

    // 9. Revenue per category from delivered orders.
    public static IQueryable<CategoryRevenueDto> Q9(DbContext db) =>
        db.Set<OrderItem>()
            .Where(i => i.Order.Status == OrderStatus.Delivered)
            .GroupBy(i => new { i.Product.CategoryId, CategoryName = i.Product.Category.Name })
            .Select(g => new CategoryRevenueDto
            {
                CategoryName = g.Key.CategoryName,
                Revenue = g.Sum(i => i.Quantity * i.UnitPrice * (1m - i.Discount))
            })
            .OrderByDescending(x => x.Revenue);

    // 10. Customers without orders, grouped by city.
    public static IQueryable<CustomersByCityDto> Q10(DbContext db) =>
        db.Set<Customer>()
            .Where(c => !c.Orders.Any())
            .GroupBy(c => c.City)
            .Select(g => new CustomersByCityDto
            {
                City = g.Key,
                CustomerCount = g.Count()
            })
            .OrderByDescending(x => x.CustomerCount);

    // 11. Products with at least ten reviews and average rating of 4.5 or more.
    public static IQueryable<ProductReviewStatsDto> Q11(DbContext db) =>
        db.Set<Product>()
            .Where(p => p.Reviews.Count >= 10 && p.Reviews.Average(r => r.Rating) >= 4.5)
            .Select(p => new ProductReviewStatsDto
            {
                ProductName = p.Name,
                CategoryName = p.Category.Name,
                ReviewCount = p.Reviews.Count,
                AverageRating = p.Reviews.Average(r => r.Rating)
            });

    // 12. Two most expensive products per category.
    public static IQueryable<TopExpensiveProductDto> Q12(DbContext db) =>
        db.Set<Category>()
            .SelectMany(c => c.Products
                .OrderByDescending(p => p.Price)
                .ThenBy(p => p.Name)
                .Take(2)
                .Select(p => new TopExpensiveProductDto
                {
                    CategoryName = c.Name,
                    ProductName = p.Name,
                    Price = p.Price
                }));

    // 13. Products whose stock is below the quantity in pending orders.
    public static IQueryable<PendingProductQuantityDto> Q13(DbContext db)
    {
        var pendingQuantities = db.Set<OrderItem>()
            .Where(i => i.Order.Status == OrderStatus.Pending)
            .GroupBy(i => i.ProductId)
            .Select(g => new { ProductId = g.Key, Quantity = g.Sum(i => i.Quantity) });

        return from product in db.Set<Product>()
               join pending in pendingQuantities on product.Id equals pending.ProductId
               where product.Stock < pending.Quantity
               orderby pending.Quantity descending
               select new PendingProductQuantityDto
               {
                   ProductName = product.Name,
                   Stock = product.Stock,
                   PendingQuantity = pending.Quantity
               };
    }

    // 14. Customers whose delivered-order spending is above the average for all customers.
    public static IQueryable<CustomerSpendingDto> Q14(DbContext db)
    {
        var spending = db.Set<Customer>().Select(c => new CustomerSpendingDto
        {
            CustomerName = c.FullName,
            TotalSpent = db.Set<OrderItem>()
                .Where(i => i.Order.CustomerId == c.Id && i.Order.Status == OrderStatus.Delivered)
                .Sum(i => i.Quantity * i.UnitPrice * (1m - i.Discount))
        });

        var averageSpending = spending.Average(c => c.TotalSpent);
        return spending.Where(c => c.TotalSpent > averageSpending);
    }

    // 15. Best-selling product per category; categories without delivered sales remain in the result.
    public static IQueryable<CategoryBestSellerDto> Q15(DbContext db)
    {
        return from category in db.Set<Category>()
               let best = category.Products
                   .Select(p => new
                   {
                       ProductName = p.Name,
                       Quantity = p.OrderItems
                           .Where(i => i.Order.Status == OrderStatus.Delivered)
                           .Sum(i => i.Quantity)
                   })
                   .Where(p => p.Quantity > 0)
                   .OrderByDescending(p => p.Quantity)
                   .ThenBy(p => p.ProductName)
                   .FirstOrDefault()
               select new CategoryBestSellerDto
               {
                   CategoryName = category.Name,
                   ProductName = best == null ? null : best.ProductName,
                   QuantitySold = best == null ? 0 : best.Quantity
               };
    }

    // 16. Customers who bought every product in the Laptops category.
    public static IQueryable<Customer> Q16(DbContext db)
    {
        var laptopIds = db.Set<Product>()
            .Where(p => p.Category.Name == "Laptops")
            .Select(p => p.Id);

        return db.Set<Customer>().Where(c => laptopIds.All(productId =>
            db.Set<OrderItem>().Any(i => i.Order.CustomerId == c.Id && i.ProductId == productId)));
    }

    // 17. Reviews where the reviewer has no order containing that product.
    public static IQueryable<Review> Q17(DbContext db) =>
        db.Set<Review>().Where(r => !db.Set<OrderItem>().Any(i =>
            i.Order.CustomerId == r.CustomerId && i.ProductId == r.ProductId));

    // 18. All customers with order counts, including customers with zero orders (GroupJoin/left join).
    public static IQueryable<CustomerOrderCountDto> Q18(DbContext db) =>
        db.Set<Customer>()
            .GroupJoin(db.Set<Order>(),
                customer => customer.Id,
                order => order.CustomerId,
                (customer, orders) => new CustomerOrderCountDto
                {
                    CustomerName = customer.FullName,
                    OrderCount = orders.Count()
                });

    // 19. Five product pairs that co-occur in the largest number of orders.
    public static IQueryable<ProductPairCountDto> Q19(DbContext db) =>
        (from first in db.Set<OrderItem>()
         from second in db.Set<OrderItem>()
         where first.OrderId == second.OrderId && first.ProductId < second.ProductId
         group first by new
         {
             FirstId = first.ProductId,
             SecondId = second.ProductId,
             FirstName = first.Product.Name,
             SecondName = second.Product.Name
         }
            into pair
         select new ProductPairCountDto
         {
             FirstProduct = pair.Key.FirstName,
             SecondProduct = pair.Key.SecondName,
             OrderCount = pair.Select(i => i.OrderId).Distinct().Count()
         })
        .OrderByDescending(x => x.OrderCount)
        .ThenBy(x => x.FirstProduct)
        .ThenBy(x => x.SecondProduct)
        .Take(5);

    // 20. One flat query result row per item in each customer's latest order.
    // Customers with no order still appear with null order/item columns.
    public static IQueryable<CustomerLatestOrderItemDto> Q20(DbContext db) =>
        db.Set<Customer>().SelectMany(
            c => c.Orders
                .OrderByDescending(o => o.OrderDate)
                .ThenByDescending(o => o.Id)
                .Take(1)
                .SelectMany(o => o.Items.Select(i => new
                {
                    OrderId = (int?)o.Id,
                    OrderDate = (DateTime?)o.OrderDate,
                    ProductName = (string?)i.Product.Name,
                    Quantity = (int?)i.Quantity
                }))
                .DefaultIfEmpty(),
            (c, item) => new CustomerLatestOrderItemDto
            {
                CustomerName = c.FullName,
                OrderId = item == null ? null : item.OrderId,
                OrderDate = item == null ? null : item.OrderDate,
                ProductName = item == null ? null : item.ProductName,
                Quantity = item == null ? null : item.Quantity
            });

    // 21. Each customer's favorite category by delivered quantity.
    public static IQueryable<CustomerFavoriteCategoryDto> Q21(DbContext db)
    {
        var quantities = db.Set<OrderItem>()
            .Where(i => i.Order.Status == OrderStatus.Delivered)
            .GroupBy(i => new
            {
                i.Order.CustomerId,
                i.Product.CategoryId,
                CategoryName = i.Product.Category.Name
            })
            .Select(g => new CustomerFavoriteCategoryDto
            {
                CustomerId = g.Key.CustomerId,
                CategoryName = g.Key.CategoryName,
                Quantity = g.Sum(i => i.Quantity)
            });

        return db.Set<Customer>()
            .GroupJoin(quantities,
                c => c.Id,
                q => q.CustomerId,
                (customer, categories) => new { customer, categories })
            .SelectMany(x => x.categories
                    .OrderByDescending(q => q.Quantity)
                    .ThenBy(q => q.CategoryName)
                    .Take(1)
                    .DefaultIfEmpty(),
                (x, favorite) => new CustomerFavoriteCategoryDto
                {
                    CustomerId = x.customer.Id,
                    CustomerName = x.customer.FullName,
                    CategoryName = favorite == null ? null : favorite.CategoryName,
                    Quantity = favorite == null ? 0 : favorite.Quantity
                });
    }

    // 22. Revenue rank. SQL computes revenue; the small final ranking projection runs in memory.
    public static List<ProductRevenueRankDto> Q22(DbContext db)
    {
        var revenueRows = db.Set<Product>()
            .Select(p => new
            {
                ProductName = p.Name,
                Revenue = db.Set<OrderItem>()
                    .Where(i => i.ProductId == p.Id && i.Order.Status == OrderStatus.Delivered)
                    .Sum(i => i.Quantity * i.UnitPrice * (1m - i.Discount))
            })
            .OrderByDescending(x => x.Revenue)
            .ThenBy(x => x.ProductName)
            .ToList();

        return revenueRows
            .Select((row, index) => new ProductRevenueRankDto
            {
                Rank = index + 1,
                ProductName = row.ProductName,
                Revenue = row.Revenue
            })
            .ToList();
    }

    // 23. Optional product search filters, sorting, paging, and total count.
    public static ProductSearchPage SearchProducts(DbContext db, ProductFilter filter)
    {
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var query = db.Set<Product>().Select(p => new ProductSearchItemDto
        {
            Id = p.Id,
            Name = p.Name,
            Category = p.Category.Name,
            Price = p.Price,
            Stock = p.Stock,
            AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => (double)r.Rating) : 0
        });

        if (!string.IsNullOrWhiteSpace(filter.Text))
        {
            var text = filter.Text.Trim();
            query = query.Where(p => p.Name.Contains(text));
        }

        if (!string.IsNullOrWhiteSpace(filter.Category))
            query = query.Where(p => p.Category == filter.Category);

        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);

        if (filter.InStockOnly)
            query = query.Where(p => p.Stock > 0);

        var totalCount = query.Count();
        query = (filter.SortBy?.Trim().ToLowerInvariant(), filter.Descending) switch
        {
            ("price", false) => query.OrderBy(p => p.Price).ThenBy(p => p.Name),
            ("price", true) => query.OrderByDescending(p => p.Price).ThenBy(p => p.Name),
            ("rating", false) => query.OrderBy(p => p.AverageRating).ThenBy(p => p.Name),
            ("rating", true) => query.OrderByDescending(p => p.AverageRating).ThenBy(p => p.Name),
            ("name", true) => query.OrderByDescending(p => p.Name),
            _ => query.OrderBy(p => p.Name)
        };

        return new ProductSearchPage
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            Items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList()
        };
    }

    // 24. Set-based 10% price increase; does not load matching products into memory.
    public static int Q24(DbContext db) =>
        db.Set<Product>()
            .Where(p => p.Category.Name == "Accessories" && p.Stock < 5)
            .ExecuteUpdate(setters => setters
                .SetProperty(p => p.Price, p => p.Price * 1.10m));

    // 25. Set-based delete of cancelled orders older than one year.
    public static int Q25(DbContext db)
    {
        var cutoff = DateTime.Now.AddYears(-1);
        return db.Set<Order>()
            .Where(o => o.Status == OrderStatus.Cancelled && o.OrderDate < cutoff)
            .ExecuteDelete();
    }
}

public class DeliveredOrderTotalDto
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Total { get; set; }
}

public class OrderStatusCountDto
{
    public OrderStatus Status { get; set; }
    public int OrderCount { get; set; }
    public double Percentage { get; set; }
}

public class CustomerCancelledOrdersDto
{
    public string CustomerName { get; set; } = string.Empty;
    public int CancelledOrderCount { get; set; }
}

public class CustomerSpendingSummaryDto
{
    public string CustomerName { get; set; } = string.Empty;
    public int OrderCount { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal AverageOrderValue { get; set; }
}

public class MonthlyReportDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int OrderCount { get; set; }
    public decimal Revenue { get; set; }
}

public class CategoryRevenueDto
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}

public class CustomersByCityDto
{
    public string City { get; set; } = string.Empty;
    public int CustomerCount { get; set; }
}

public class ProductReviewStatsDto
{
    public string ProductName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int ReviewCount { get; set; }
    public double AverageRating { get; set; }
}

public class TopExpensiveProductDto
{
    public string CategoryName { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class PendingProductQuantityDto
{
    public string ProductName { get; set; } = string.Empty;
    public int Stock { get; set; }
    public int PendingQuantity { get; set; }
}

public class CustomerSpendingDto
{
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalSpent { get; set; }
}

public class CategoryBestSellerDto
{
    public string CategoryName { get; set; } = string.Empty;
    public string? ProductName { get; set; }
    public int QuantitySold { get; set; }
}

public class CustomerOrderCountDto
{
    public string CustomerName { get; set; } = string.Empty;
    public int OrderCount { get; set; }
}

public class ProductPairCountDto
{
    public string FirstProduct { get; set; } = string.Empty;
    public string SecondProduct { get; set; } = string.Empty;
    public int OrderCount { get; set; }
}

public class CustomerLatestOrderItemDto
{
    public string CustomerName { get; set; } = string.Empty;
    public int? OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? ProductName { get; set; }
    public int? Quantity { get; set; }
}

public class CustomerFavoriteCategoryDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public int Quantity { get; set; }
}

public class ProductRevenueRankDto
{
    public int Rank { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}

public class ProductFilter
{
    public string? Text { get; set; }
    public string? Category { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool InStockOnly { get; set; }
    public string? SortBy { get; set; } = "name";
    public bool Descending { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class ProductSearchItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public double AverageRating { get; set; }
}

public class ProductSearchPage
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<ProductSearchItemDto> Items { get; set; } = new();
}
