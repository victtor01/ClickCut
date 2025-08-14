
using ClickCut.Domain.Utils;

namespace ClickCut.Domain.Models;

public class User
{
	private readonly List<Business> _businesses = [];
	private readonly List<Client> _clients = [];
	private readonly List<Service> _services = [];
	private readonly List<Business> _memberOfBusinesses = [];

	public Guid Id { get; private set; } = Guid.NewGuid();
	public Email Email { get; private set; } = null!;
	public Password Password { get; private set; } = null!;
	public string Username { get; private set; } = null!;
	public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
	public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
	public IReadOnlyCollection<Business> Businesses => _businesses.AsReadOnly();
	public IReadOnlyCollection<Client> Clients => _clients.AsReadOnly();
	public IReadOnlyCollection<Service> Services => _services.AsReadOnly();
	public IReadOnlyCollection<Business> MemberOfBusinesses => _memberOfBusinesses.AsReadOnly();

	private User() { }

	private User(Email email, Password password, string username)
	{
		Email = email;
		Password = password;
		Username = username;
	}

	public static User Create(Email email, Password password, string username)
	{
		if (!ModelsConfig.IsLengthBetween(username, ModelsConfig.User.MinUsernameLength, ModelsConfig.User.MaxUsernameLength))
			throw new ArgumentException(
				$"Username must be between {ModelsConfig.User.MinUsernameLength} and {ModelsConfig.User.MaxUsernameLength} characters.",
				nameof(username));

		return new User(email, password, username);
	}
}
