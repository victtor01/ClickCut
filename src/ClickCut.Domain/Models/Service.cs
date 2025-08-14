namespace ClickCut.Domain.Models;

public class Service
{
	private const int MaxNameLength = 80;
	private const int MinNameLength = 5;
	private const int MaxDescriptionLength = 300;

	private readonly List<Booking> _bookings = [];

	public Guid Id { get; private set; }
	public string Name { get; private set; } = null!;
	public string? Description { get; private set; }
	public int DurationInMinutes { get; private set; }
	public int Price { get; private set; }
	public bool IsActive { get; private set; } = true;
	public string? PhotoUrl { get; private set; }

	public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();
	public User Owner { get; private set; } = null!;

	private Service() { }

	private Service(string name, int durationInMinutes, User owner, int price, string? description)
	{
		Id = Guid.NewGuid();
		IsActive = true;
		Name = name;
		Description = description;
		DurationInMinutes = durationInMinutes;
		Price = price;
		Owner = owner;
	}

	public static Service Create(string name, int price, int durationInMinutes, User owner, string? description = null)
	{
		if (string.IsNullOrWhiteSpace(name) || name.Length < MinNameLength || name.Length > MaxNameLength)
			throw new ArgumentException($"O nome do serviço deve ter entre {MinNameLength} e {MaxNameLength} caracteres.", nameof(name));

		if (price <= 0)
			throw new ArgumentOutOfRangeException(nameof(price), "O preço deve ser maior que zero.");

		if (durationInMinutes <= 0)
			throw new ArgumentOutOfRangeException(nameof(durationInMinutes), "A duração deve ser maior que zero.");

		if (description is not null && description.Length > MaxDescriptionLength)
			throw new ArgumentException($"A descrição não pode exceder {MaxDescriptionLength} caracteres.", nameof(description));

		return new Service(name, durationInMinutes, owner, price, description);
	}
}