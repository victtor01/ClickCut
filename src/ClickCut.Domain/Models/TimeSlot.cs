namespace ClickCut.Domain.Models;

public record TimeSlot
{
	public DayOfWeek Day { get; }
	public TimeOnly StartTime { get; }
	public TimeOnly EndTime { get; }

	public TimeSlot(DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
	{
		if (endTime <= startTime)
			throw new ArgumentException("O horário de término deve ser posterior ao horário de início.");

		Day = day;
		StartTime = startTime;
		EndTime = endTime;
	}

	public bool OverlapsWith(TimeSlot other)
	{
		if (this.Day != other.Day)
		{
			return false;
		}

		return this.StartTime < other.EndTime && other.StartTime < this.EndTime;
	}
}
