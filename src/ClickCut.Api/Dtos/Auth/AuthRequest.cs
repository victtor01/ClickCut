using System.ComponentModel.DataAnnotations;

namespace ClickCut.Api.Dtos.Auth;

public record AuthRequest(
	[Required(ErrorMessage = "O email é obrigatório!")]
	[EmailAddress(ErrorMessage = "O formato do email é inválido.")]
	string Email,
	[Required(ErrorMessage = "A senha é obrigatória!")]
	string Password
);
