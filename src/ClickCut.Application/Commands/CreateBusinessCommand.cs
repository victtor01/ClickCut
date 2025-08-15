using ClickCut.Domain.Models;

namespace ClickCut.Application.Commands;

public record CreateBusinessCommand(
	string Name,
	Guid OwnerId,
	string? Password
);