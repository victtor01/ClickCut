namespace ClickCut.Domain.Models
{
	public class Password
	{
		public string Value { get; private set; }

		public override string ToString() => Value;

		public Password(string value)
		{
			Value = value;
			IsValidOrThrow();
		}

		public bool IsStrong()
		{
			if (string.IsNullOrWhiteSpace(Value) || Value.Length < 6)
				return false;

			bool hasLetter = false, hasDigit = false;

			foreach (char c in Value)
			{
				if (char.IsLetter(c)) hasLetter = true;
				if (char.IsDigit(c)) hasDigit = true;
			}

			return hasLetter && hasDigit;
		}

		public void IsValidOrThrow()
		{
			if (!IsStrong()) throw new Exception("Password is invalid");
		}
	}
}