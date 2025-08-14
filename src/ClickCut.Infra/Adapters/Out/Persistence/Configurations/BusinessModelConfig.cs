namespace ClickCut.Infra.Adapters.Out.Persistence.Configurations;

using ClickCut.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BusinessModelConfig : IEntityTypeConfiguration<Business>
{
	public void Configure(EntityTypeBuilder<Business> builder)
	{
		builder.ToTable("Businesses");

		builder.HasKey(b => b.Id);

		builder.Property(b => b.Name)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(b => b.Password)
			.HasConversion(
				password => password != null ? password.ToString() : null,
				value => value != null ? new Password(value) : null
			)
			.HasColumnName("HashedPassword")
			.HasMaxLength(255);

		builder.HasOne(b => b.Owner)
			   .WithMany(u => u.Businesses)
			   .IsRequired();

		builder.HasMany(b => b.Members)
			   .WithMany(u => u.MemberOfBusinesses)
			   .UsingEntity("BusinessMembers");

		builder.OwnsMany(b => b.OperatingHours, timeSlotBuilder =>
		{
			timeSlotBuilder.ToTable("OperatingHours");
			timeSlotBuilder.WithOwner().HasForeignKey("BusinessId");
			timeSlotBuilder.HasKey("Id");
			timeSlotBuilder.Property(ts => ts.Day).IsRequired();
			timeSlotBuilder.Property(ts => ts.StartTime).IsRequired();
			timeSlotBuilder.Property(ts => ts.EndTime).IsRequired();
		});

		builder.Navigation(b => b.Members)
			   .HasField("_members")
			   .UsePropertyAccessMode(PropertyAccessMode.Field);

		builder.Navigation(b => b.OperatingHours)
			   .HasField("_operatingHours")
			   .UsePropertyAccessMode(PropertyAccessMode.Field);
	}
}