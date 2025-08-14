namespace ClickCut.Api.Dtos.Users;

public class UserResponse
{
	public required string Email { get; init; }
	public required string Username { get; init; }
	public required DateTime CreatedAt { get; init; }
}