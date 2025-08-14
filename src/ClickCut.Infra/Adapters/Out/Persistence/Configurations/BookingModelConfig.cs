namespace ClickCut.Infra.Adapters.Out.Persistence.Configurations;

using ClickCut.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BookingModelConfig : IEntityTypeConfiguration<Booking>
{
	public void Configure(EntityTypeBuilder<Booking> booking)
	{
		booking.ToTable("Bookings");

		booking.HasKey(b => b.Id);

		booking.Property(b => b.Title)
			.IsRequired()
			.HasMaxLength(100);

		booking.Property(b => b.StartsAt).IsRequired();
		booking.Property(b => b.EndsAt).IsRequired();

		booking.Property(b => b.Status)
			.IsRequired()
			.HasConversion<string>()
			.HasMaxLength(50);

		booking.HasOne(b => b.Business)
			.WithMany(b => b.Bookings)
			.HasForeignKey("BusinessId")
			.IsRequired()
			.OnDelete(DeleteBehavior.Restrict);

		booking.HasOne(b => b.Client)
			.WithMany(c => c.Bookings)
			.HasForeignKey("ClientId")
			.IsRequired(false)
			.OnDelete(DeleteBehavior.SetNull);

		booking.HasMany(b => b.Services)
		  .WithMany(service => service.Bookings)
		  .UsingEntity("BookingServices");

		booking.Navigation(b => b.Services).HasField("_services");
	}
}