using ClickCut.Domain.Models;

namespace ClickCut.Application.Commands;

public class CreateUserCommand
{
	public required string Email { get; set; }
	public required string Username { get; set; }
	public required string Password { get; set; }
}