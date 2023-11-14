using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //// DbSet properties represent database tables
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {

            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18, 2)"); // Example: Precision of 18 and 2 decimal places




            base.OnModelCreating(modelBuilder); // This call is important

            // Configure the primary key for IdentityUserLogin
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(login => new { login.ProviderKey, login.LoginProvider });



            // No need this code

            // Define relationships using Fluent API

            //// Many-to-One: Books -> Authors
            //modelBuilder.Entity<Book>()
            //    .HasOne(b => b.Author)
            //    .WithMany(a => a.Books)
            //    .HasForeignKey(b => b.AuthorID);

            // // Configure the 'Price' property of the 'Book' entity
            //modelBuilder.Entity<Book>()
            //    .Property(b => b.Price)
            //    .HasColumnType("decimal(18, 2)"); // Example: Precision of 18 and 2 decimal places

            //// // Configure the 'TotalPrice' property of the 'Order' entity
           

            //// Many-to-One: OrderItems -> Books
            //modelBuilder.Entity<OrderItem>()
            //    .HasOne(oi => oi.Book)
            //    .WithMany(b => b.OrderItems)
            //    .HasForeignKey(oi => oi.BookID);

            //// Many-to-One OrderItem -> Order
            //modelBuilder.Entity<OrderItem>()
            //    .HasOne(oi => oi.Order)
            //    .WithMany(o => o.OrderItems)
            //    .HasForeignKey(oi => oi.OrderID);

            //// Many-to-One: Orders -> Customers
            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.Customer)
            //    .WithMany(c => c.Orders)
            //    .HasForeignKey(o => o.CustomerID);



            //modelBuilder.Entity<Author>().HasData(
            //    new Author
            //    {
            //        AuthorID = 1,
            //        Name = "John Doe",
            //        Biography = "John Doe is a prolific author with a passion for storytelling."
            //    },
            //    new Author
            //    {
            //        AuthorID = 2,
            //        Name = "Jane Smith",
            //        Biography = "Jane Smith is an award-winning novelist known for her vivid writing style."
            //    }
            // Add more authors with complete data as needed
            //);




            //modelBuilder.Entity<Book>().HasData(
            //    new Book
            //    {
            //        BookID = 1,
            //        Title = "Book 1",
            //        ISBN = "978-1234567890",
            //        Description = "This is the description of Book 1.",
            //        Price = 29.99m,
            //        AuthorID = 1 // Reference to the author with ID 1
            //    },
            //    new Book
            //    {
            //        BookID = 2,
            //        Title = "Book 2",
            //        ISBN = "978-9876543210",
            //        Description = "This is the description of Book 2.",
            //        Price = 24.99m,
            //        AuthorID = 2 // Reference to the author with ID 2
            //    }
            //// Add more books with complete data as needed
            //);

            //    modelBuilder.Entity<Book>()
            //.Property(b => b.Price)
            //.HasColumnType("decimal(18, 2)");

            //modelBuilder.Entity<Order>()
            //.Property(o => o.TotalPrice)
            //.HasColumnType("decimal(18, 2)");


            // modelBuilder.Entity<OrderItem>().HasData(
            //    new OrderItem
            //    {
            //        OrderItemID = 1,
            //        OrderID = 1, // Reference to Order with ID 1
            //        BookID = 1,  // Reference to Book with ID 1
            //        Quantity = 3
            //        // Other order item properties...
            //    },
            //    new OrderItem
            //    {
            //        OrderItemID = 2,
            //        OrderID = 1, // Reference to Order with ID 1
            //        BookID = 2,  // Reference to Book with ID 2
            //        Quantity = 2
            //        // Other order item properties...
            //    }
            //         // Add more order items with complete data as needed
            // );

            // modelBuilder.Entity<Customer>().HasData(
            //     new Customer
            //     {
            //         CustomerID = 1,
            //         Name = "Alice Johnson",
            //         Email = "alice@example.com",
            //         Address = "123 Main St, City, Country",
            //         Phone = "+1 (123) 456-7890"
            //     },
            //     new Customer
            //     {
            //         CustomerID = 2,
            //         Name = "Bob Smith",
            //         Email = "bob@example.com",
            //         Address = "456 Elm St, Town, Country",
            //         Phone = "+1 (987) 654-3210"
            //     }
            //     // Add more customers with complete data as needed
            // );


            // modelBuilder.Entity<Order>().HasData(
            //    new Order
            //    {
            //        OrderID = 1,
            //        CustomerID = 1, // Reference to Customer with ID 1
            //        OrderDate = new DateTime(2023, 3, 10),
            //        TotalPrice = 89.97m,
            //        // Other order properties...
            //    },
            //    new Order
            //    {
            //        OrderID = 2,
            //        CustomerID = 2, // Reference to Customer with ID 2
            //        OrderDate = new DateTime(2023, 3, 11),
            //        TotalPrice = 124.95m,
            //        // Other order properties...
            //    }
            //// Add more orders with complete data as needed
            //);
        }


    }
}