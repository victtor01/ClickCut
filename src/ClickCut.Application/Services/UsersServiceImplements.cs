using System.Runtime.CompilerServices;
using ClickCut.Application.Commands;
using ClickCut.Application.Ports.In;
using ClickCut.Application.Ports.Out;
using ClickCut.Application.Utils;
using ClickCut.Domain.Models;
using ClickCut.Shared.Exceptions;
using ClickCut.Shared.Utils;

namespace ClickCut.Application.Services;

public class UsersServiceImplements(IUsersRepositoryPort usersRepositoryPort, IPasswordServicePort passwordServicePort) : IUsersServicePort
{
	private readonly IUsersRepositoryPort _usersRepositoryPort = usersRepositoryPort;
	private readonly IPasswordServicePort _passwordServicePort = passwordServicePort;

	public async Task<User> CreateUserAsync(CreateUserCommand createUserCommand)
	{

		Email email = new(createUserCommand.Email);

		User? userInDb = await _usersRepositoryPort.FindByEmailAsync(email);

		if (userInDb != null)
			throw new BadRequestException("Usuário já existe");

		string passwordHashed = _passwordServicePort.Encoder(createUserCommand.Password);
		Password password = new(passwordHashed);

		Result<User> result = SafeFactory.TryCreate(() => User.Create(
				email: new Email(createUserCommand.Email),
				username: createUserCommand.Username,
				password: password
			)
		);

		if (result.IsFailure)
			throw new BadRequestException(result.Error);

		User user = result.Value!;

		User created = await _usersRepositoryPort.CreateUserAsync(user);

		return created;
	}
}