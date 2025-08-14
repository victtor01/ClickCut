using System.Reflection;
using ClickCut.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ClickCut.Infra.Adapters.Out.Persistence.DatabaseContext;

public class AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; } = null!;

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