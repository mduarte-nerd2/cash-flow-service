using System;
using Microsoft.EntityFrameworkCore;
using CashFlowService.Core.DomainEntities;

namespace CashFlowService.Infra.Data.Contexts;

public class InMemoryContext : DbContext
{
    public InMemoryContext(DbContextOptions<InMemoryContext> options)
        : base(options) { }

    public DbSet<CashBook> CashBooks { get; set; }

    public DbSet<CashBookTransaction> CashBookGTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CashBookTransaction>()
            .HasOne(cbt => cbt.CashBook)
            .WithMany()
            .HasForeignKey(cbt => cbt.CashBookId);
    }
}


