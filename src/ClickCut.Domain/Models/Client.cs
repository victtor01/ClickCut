namespace ClickCut.Domain.Models;

public class Client
{
	private const int MinNameLength = 3;
	private const int MaxNameLength = 150;
	private const int ExactPhoneNumberLength = 11;

	public Guid Id { get; private set; }
	public string FullName { get; private set; }
	public string PhoneNumber { get; private set; }
	public Business Business { get; private set; }
	public User Owner { get; private set; }

	private readonly List<Booking> _bookings = [];
	public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

	private Client()
	{
		FullName = null!;
		PhoneNumber = null!;
		Business = null!;
		Owner = null!;
	}

	private Client(Business store, User owner, string fullName, string phoneNumber)
	{
		Id = Guid.NewGuid();
		FullName = fullName;
		PhoneNumber = phoneNumber;
		Business = store;
		Owner = owner;
	}

	public static Client Create(Business store, User owner, string fullName, string phoneNumber)
	{
		ArgumentNullException.ThrowIfNull("A valid store ID is required.", nameof(store));
		ArgumentNullException.ThrowIfNull("Owner is required to create new client!", nameof(owner));

		if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < MinNameLength || fullName.Length > MaxNameLength)
			throw new ArgumentException($"Full name must be between {MinNameLength} and {MaxNameLength} characters.", nameof(fullName));

		var sanitizedPhone = new string(phoneNumber?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());

		if (sanitizedPhone.Length != ExactPhoneNumberLength)
			throw new ArgumentException($"Phone number must have exactly {ExactPhoneNumberLength} digits.", nameof(phoneNumber));

		return new Client(store, owner, fullName, sanitizedPhone);
	}

	public void AddBooking(Booking booking)
	{
		ArgumentNullException.ThrowIfNull(booking);

		if (_bookings.Any(b => b.Id == booking.Id))
			return;

		_bookings.Add(booking);
	}
}