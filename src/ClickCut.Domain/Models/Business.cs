using ClickCut.Domain.Utils;

namespace ClickCut.Domain.Models;

public class Business
{
	private readonly List<TimeSlot> _operatingHours = [];
	private readonly List<User> _members = [];
	private readonly List<Booking> _bookings = [];

	public Guid Id { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public User Owner { get; private set; } = null!;
	public Password? Password { get; private set; }

	public IReadOnlyCollection<User> Members => _members.AsReadOnly();
	public IReadOnlyCollection<TimeSlot> OperatingHours => _operatingHours.AsReadOnly();
	public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

	private Business() { }

	private Business(User owner, string name, Password? password)
	{
		Owner = owner;
		Name = name;
		Password = password;
	}

	public static Business Create(User owner, string name, Password? password)
	{
		if (owner is null)
			throw new ArgumentNullException(nameof(owner), "O proprietário não pode ser nulo.");

		if (string.IsNullOrWhiteSpace(name) || name.Length < ModelsConfig.User.MinUsernameLength)
			throw new ArgumentException($"O nome deve ter no mínimo {ModelsConfig.User.MinUsernameLength} caracteres.", nameof(name));

		if (password is not null && string.IsNullOrEmpty(password.ToString()))
			throw new ArgumentException("Senha inválida!");

		return new Business(owner, name, password);
	}

	public void AddMember(User member)
	{
		if (member is null)
			throw new ArgumentNullException(nameof(member), "O membro a ser adicionado não pode ser nulo.");

		if (_members.Any(m => m.Id == member.Id))
			return;

		_members.Add(member);
	}

	public void RemoveMember(User member)
	{
		if (member is null)
			throw new ArgumentNullException(nameof(member), "O membro a ser removido não pode ser nulo.");

		if (member.Id == Owner.Id)
			throw new InvalidOperationException("O proprietário não pode ser removido do negócio.");

		var memberToRemove = _members.FirstOrDefault(m => m.Id == member.Id);

		if (memberToRemove != null)
			_members.Remove(memberToRemove);
	}

	public void AddOperatingHours(DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
	{
		var newSlot = new TimeSlot(day, startTime, endTime);
		bool hasOverlap = _operatingHours.Any(newSlot.OverlapsWith);

		if (hasOverlap)
		{
			throw new InvalidOperationException("O horário informado está em conflito com um horário já existente.");
		}

		_operatingHours.Add(newSlot);
	}
}

