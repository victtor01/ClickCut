using System.Reflection;
using ClickCut.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ClickCut.Infra.Adapters.Out.Persistence.DatabaseContext;

public class AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Business> Businesses { get; set; } = null!;
	public DbSet<Booking> Bookings { get; set; } = null!;
	public DbSet<Service> Services { get; set; } = null!;
	public DbSet<Client> Clients { get; set; } = null!;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}