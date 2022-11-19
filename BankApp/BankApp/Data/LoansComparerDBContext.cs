using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using BankApp.Models;
using Microsoft.AspNetCore.Identity;

namespace BankApp.Data
{
    public class LoansComparerDBContext : DbContext
    {
        public LoansComparerDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<IdentityUser> Users { get; set; }
    }
}
