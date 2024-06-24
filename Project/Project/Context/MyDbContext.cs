using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Context;

public class MyDbContext : DbContext
{
    public MyDbContext() { }
    
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<IndividualClient> IndividualClients { get; set; }
    public DbSet<CompanyClient> CompanyClients { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
            .HasDiscriminator<ClientType>("ClientType")
            .HasValue<IndividualClient>(ClientType.Individual)
            .HasValue<CompanyClient>(ClientType.Company);
        
        modelBuilder.Entity<IndividualClient>()
            .Property(i => i.IsDeleted)
            .HasDefaultValue(false);

        modelBuilder.Entity<Contract>()
            .Property(p => p.IsSigned)
            .HasDefaultValue(false);
    }
}