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


        public void AddBookToOrder(int bookId, int quantity)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == bookId);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found");
            }

            // Create a new Order for demonstration. In practice, you might be adding to an existing order.
            var order = new Order
            {
                // Set necessary Order properties like OrderDate, CustomerID, etc.
            };

            _context.Orders.Add(order);
            _context.SaveChanges(); // Save to get the generated OrderID

            var orderItem = new OrderItem
            {
                OrderID = order.OrderID, // Use the newly created order's ID
                BookID = book.BookID,
                Quantity = quantity
            };

            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();
        }
        


    }
}
