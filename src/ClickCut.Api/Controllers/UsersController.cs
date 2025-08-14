using ClickCut.Api.Dtos.Users;
using ClickCut.Api.Mappers;
using ClickCut.Application.Commands;
using ClickCut.Application.Ports.In;
using ClickCut.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickCut.Api.Controllers;

[ApiController]
[Route("/users")]
public class UsersController(IUsersServicePort usersServicePort) : ControllerBase
{
	private readonly IUsersServicePort _usersServicePort = usersServicePort;

	[HttpPost]
	public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest createUserRequest)
	{
		User user = await _usersServicePort.CreateUserAsync(new CreateUserCommand()
		{
			Email = createUserRequest.Email,
			Password = createUserRequest.Password,
			Username = createUserRequest.Username
		});

		return Ok(user.ToResponse());
	}

	[Authorize]
	[HttpGet("mine")]
	public IActionResult Mine()
	{
		return Ok("Its OK!");
	}
}