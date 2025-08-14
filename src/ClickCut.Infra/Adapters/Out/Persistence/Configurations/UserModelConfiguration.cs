using ClickCut.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickCut.Infra.Adapters.Out.Persistence.Configurations;

public class UserModelConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(u => u.Id);
		builder.Property(u => u.Username).IsRequired().HasMaxLength(200);

		builder.Property(u => u.Email)
			  .HasConversion(
				  email => email.Address,
				  value => new Email(value))
			  .IsRequired()
			  .HasMaxLength(255)
			  .HasColumnName("Email");

		builder.Property(u => u.Password)
		   .HasConversion(
			   password => password.Value,
			   value => new Password(value))
		   .IsRequired()
		   .HasColumnName("PasswordHash");

		builder.Property(u => u.CreatedAt)
			.IsRequired()
			.HasDefaultValueSql("CURRENT_TIMESTAMP")
			.ValueGeneratedOnAdd();

		builder.Property(u => u.UpdatedAt)
			.IsRequired()
			.HasDefaultValueSql("CURRENT_TIMESTAMP")
			.ValueGeneratedOnAddOrUpdate();

		ConfigureRelations(builder);
	}

	public static void ConfigureRelations(EntityTypeBuilder<User> user)
	{
		user.HasMany(u => u.Businesses).WithOne(x => x.Owner).HasForeignKey("OwnerId")
			.IsRequired().OnDelete(DeleteBehavior.Restrict);

		user.HasMany(u => u.Clients).WithOne(x => x.Owner).HasForeignKey("OwnerId")
			.IsRequired().OnDelete(DeleteBehavior.Restrict);

		user.HasMany(u => u.Services).WithOne(x => x.Owner).HasForeignKey("OwnerId")
			.IsRequired().OnDelete(DeleteBehavior.Restrict);
	}
}