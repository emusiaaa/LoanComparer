using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using BankApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BankApp.Data
{
    public class LoansComparerDBContext : IdentityDbContext<ClientModel>
    {
        public LoansComparerDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<NotRegisteredInquiryModel> NotRegisteredInquiries { get; set; }
    }
}
