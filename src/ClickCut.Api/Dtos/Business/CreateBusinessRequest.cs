using System.ComponentModel.DataAnnotations;

namespace ClickCut.Api.Dtos.Business;

public record CreateBusinessRequest(
	[Required(ErrorMessage = "O nome é obrigatorio")]
	string Name,

	[MinLength(4, ErrorMessage = "A senha deve ter no mínimo 4 caracteres.")]
	string? Password
);