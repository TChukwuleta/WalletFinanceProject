using Dot.Core.Entities;
using Dot.Core.Entities.MerchantSide;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<RequestModel> RequestModels { get; set; }
        public DbSet<Saving> Savings { get; set; }
        public DbSet<RequestBuddy> RequestBuddies { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Barcode> Barcodes { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}
