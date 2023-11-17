using FinalProject.Data;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services
{
    public class OrderService
    {
                
        
            private readonly ApplicationDbContext _context;

            public OrderService(ApplicationDbContext context)
            {
                _context = context;
            }

            public decimal CalculateOrderTotal(int orderId)
            {
                var order = _context.Orders.Include(o => o.OrderItems)
                                          .ThenInclude(oi => oi.Book)
                                          .FirstOrDefault(o => o.OrderID == orderId);

                if (order == null) throw new InvalidOperationException("Order not found");

                decimal total = 0;
                foreach (var item in order.OrderItems)
                {
                    total += item.Quantity * item.Book.Price; // Basic total calculation
                }

                total += CalculateTax(total); // Add tax
                                              // You can also apply discounts or other calculations here

                return total;
            }
            
            // calculate tax
            private decimal CalculateTax(decimal total)
            {
                decimal taxRate = 0.10m; // 10% tax rate
                decimal taxAmount = total * taxRate;
                return taxAmount;
            }

            public Order GetOrderById(int orderId)
            {
                return _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Book)
                    .Include(o => o.Customer)
                    .FirstOrDefault(o => o.OrderID == orderId);
            }



    }
}
