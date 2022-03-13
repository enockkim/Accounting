using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using Accounting.Models.AccountingDb;

namespace Accounting.Data
{
  public partial class AccountingDbContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public AccountingDbContext(DbContextOptions<AccountingDbContext> options):base(options)
    {
    }

    public AccountingDbContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Accounting.Models.AccountingDb.TblAccount>()
              .HasOne(i => i.AccountType)
              .WithMany(i => i.TblAccounts)
              .HasForeignKey(i => i.account_type_id)
              .HasPrincipalKey(i => i.account_type_id);
        builder.Entity<Accounting.Models.AccountingDb.TblTransaction>()
              .HasOne(i => i.TblAccount)
              .WithMany(i => i.TblTransactions)
              .HasForeignKey(i => i.account_id)
              .HasPrincipalKey(i => i.gl_account_no);
        builder.Entity<Accounting.Models.AccountingDb.TblTransaction>()
              .HasOne(i => i.AccountType)
              .WithMany(i => i.TblTransactions)
              .HasForeignKey(i => i.account_type)
              .HasPrincipalKey(i => i.account_type_id);

        builder.Entity<Accounting.Models.AccountingDb.Customer>()
              .Property(p => p.DateAdded)
              .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Entity<Accounting.Models.AccountingDb.Supplier>()
              .Property(p => p.DateAdded)
              .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Entity<Accounting.Models.AccountingDb.TblAccount>()
              .Property(p => p.status)
              .HasDefaultValueSql("true");

        this.OnModelBuilding(builder);
    }


    public DbSet<Accounting.Models.AccountingDb.AccountType> AccountTypes
    {
      get;
      set;
    }

    public DbSet<Accounting.Models.AccountingDb.Customer> Customers
    {
      get;
      set;
    }

    public DbSet<Accounting.Models.AccountingDb.Supplier> Suppliers
    {
      get;
      set;
    }

    public DbSet<Accounting.Models.AccountingDb.TblAccount> TblAccounts
    {
      get;
      set;
    }

    public DbSet<Accounting.Models.AccountingDb.TblTransaction> TblTransactions
    {
      get;
      set;
    }
  }
}
