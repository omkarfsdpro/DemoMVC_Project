using BestStoreMVC.Models;       //Models folder ke classes use karne ke liye    Product, Order, ApplicationUser yahin se aate hain
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;  //Login, Register, Roles, Users, Password – sab handle karta hai
using Microsoft.EntityFrameworkCore;      //Database se baat karne ka bridge

namespace BestStoreMVC.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>     //1]Ye class DbContext hai (Database context)    2]Aur saath me Identity system bhi enable hai
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }   //CRUD operations yahin se honge
        public DbSet<Order> Orders { get; set; }
    }
}


//ApplicationDbContext is the central class of Entity Framework Core
//that manages database connections, Identity tables, and DbSet mappings.