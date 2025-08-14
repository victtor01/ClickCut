using System.Net;
using ClickCut.Domain.Enums;

namespace ClickCut.Domain.Models;

public class Booking
{
	private const int MaxTitleLength = 100;
	private const int MinTitleLength = 3;

	private readonly List<Service> _services = [];

	public Guid Id { get; private set; }
	public string Title { get; private set; } = null!;
	public Business Business { get; private set; } = null!;
	public Client? Client { get; private set; }
	public DateTime StartsAt { get; private set; }
	public DateTime EndsAt { get; private set; }
	public BookingStatus Status { get; private set; }

	public IReadOnlyCollection<Service> Services => _services;

	private Booking() { }

	private Booking(string title, DateTime startsAt, DateTime endsAt, Business business, Client? client)
	{
		Id = Guid.NewGuid();
		Title = title;
		StartsAt = startsAt;
		EndsAt = endsAt;
		Client = client;
		Business = business;
		Status = BookingStatus.CREATED;
	}

	public static Booking Create(string title, DateTime startsAt, DateTime endsAt, Business business, Client? client)
	{
		if (string.IsNullOrWhiteSpace(title) || title.Length < MinTitleLength || title.Length > MaxTitleLength)
			throw new ArgumentException(
				$"The title must be between {MinTitleLength} and {MaxTitleLength} characters.",
				nameof(title));

		if (startsAt >= endsAt)
			throw new InvalidOperationException("The booking's end time must be after the start time.");

		if (startsAt < DateTime.UtcNow)
			throw new InvalidOperationException("A booking cannot be scheduled in the past.");

		return new Booking(title, startsAt, endsAt, business, client);
	}

	public void Confirm()
	{
		if (Status != BookingStatus.CREATED && Status != BookingStatus.PENDING)
		{
			throw new InvalidOperationException($"Cannot confirm a booking with status '{Status}'.");
		}
		Status = BookingStatus.CONFIRMED;
	}

	public void Cancel()
	{
		if (Status == BookingStatus.COMPLETED || Status == BookingStatus.PAID || Status == BookingStatus.IN_PROGRESS)
			throw new InvalidOperationException($"Cannot cancel a booking with status '{Status}'.");

		Status = BookingStatus.CANCELLED;
	}

	public void Complete()
	{
		if (Status != BookingStatus.IN_PROGRESS)
			throw new InvalidOperationException($"Cannot complete a booking that is not in progress. Current status: '{Status}'.");

		Status = BookingStatus.COMPLETED;
	}

	public void MarkAsPaid()
	{
		if (Status != BookingStatus.COMPLETED)
			throw new InvalidOperationException($"Cannot mark as paid a booking that is not completed. Current status: '{Status}'.");

		Status = BookingStatus.PAID;
	}
}