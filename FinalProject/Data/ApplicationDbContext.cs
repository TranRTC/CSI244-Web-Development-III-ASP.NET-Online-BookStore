using FinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data
{
    public class ApplicationDbContext : IdentityDbContext

    {

        //// DbSet properties represent database tables
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }


        // code add to override SaveChange to update Total Price before saving
        //public override int SaveChanges()
        //{
        //    foreach (var entry in ChangeTracker.Entries<Order>())
        //    {
        //        if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
        //        {
        //            var order = entry.Entity;
        //            order.TotalPrice = order.CalculateTotal();
        //        }
        //    }
        //    return base.SaveChanges();
        //}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}