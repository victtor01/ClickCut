using System.ComponentModel.DataAnnotations;

namespace ClickCut.Domain.Utils;

public static class ModelsConfig
{
	public static class User
	{
		public const int MinUsernameLength = 3;
		public const int MaxUsernameLength = 50;
	}

	public static class Booking
	{
		public const int MinTitleLength = 3;
		public const int MaxTitleLength = 100;
	}

	public static class Client
	{
		public const int MinNameLength = 3;
		public const int MaxNameLength = 150;
		public const int ExactPhoneNumberLength = 11;
	}

	public static bool IsLengthBetween(string? content, int min, int max)
	{
		if (string.IsNullOrWhiteSpace(content))
			return false;

		return content.Length >= min && content.Length <= max;
	}
}