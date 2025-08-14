namespace ClickCut.Infra.Adapters.Out.Persistence.Configurations;

using ClickCut.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ServicesModelConfig : IEntityTypeConfiguration<Service>
{
	public void Configure(EntityTypeBuilder<Service> service)
	{
		service.ToTable("Services");

		// 1. Chave Primária
		service.HasKey(s => s.Id);

		// 2. Propriedades Simples
		service.Property(s => s.Name)
			.IsRequired()
			.HasMaxLength(80);

		service.Property(s => s.Description)
			.HasMaxLength(300);

		service.Property(s => s.DurationInMinutes)
			.IsRequired();

		// É uma boa prática definir a precisão para colunas decimais
		service.Property(s => s.Price)
			.IsRequired()
			.HasColumnType("decimal(18,2)");

		service.Property(s => s.IsActive)
			.IsRequired();

		service.Property(s => s.PhotoUrl)
			.HasMaxLength(2048); // Um tamanho razoável para URLs

		// Relacionamento com o Proprietário (User)
		service.HasOne(s => s.Owner)
			.WithMany(u => u.Services) // Aponta para a coleção 'Services' na classe User
			.IsRequired()
			.OnDelete(DeleteBehavior.Restrict); // Impede que um usuário seja deletado se ele possuir serviços

		// Relacionamento com Agendamentos (Booking)
		// A configuração principal do muitos-para-muitos já está no BookingModelConfig.
		// Aqui, apenas garantimos que o EF Core saiba como acessar o campo privado.
		service.Navigation(s => s.Bookings)
			   .HasField("_bookings");
	}
}