using ClickCut.Api.Dtos.Users;
using ClickCut.Domain.Models;

namespace ClickCut.Api.Mappers;

public static class UserMapper
{
	public static UserResponse ToResponse(this User user)
	{
		return new UserResponse()
		{
			Username = user.Username,
			Email = user.Email.Address,
			CreatedAt = user.CreatedAt,
		};
	}
}