using System.Text.RegularExpressions;

namespace ClickCut.Domain.Models
{
	public record Email
	{
		public string Address { get; }

		private static readonly Regex _emailRegex = new(
			@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
			RegexOptions.Compiled);

		public Email(string address)
		{
			if (string.IsNullOrWhiteSpace(address))
			{
				throw new ArgumentNullException(nameof(address), "O endereço de e-mail não pode ser nulo ou vazio.");
			}

			if (!_emailRegex.IsMatch(address))
			{
				throw new ArgumentException("O formato do e-mail é inválido.", nameof(address));
			}

			Address = address;
		}

		public override string ToString() => Address;
	}
}
