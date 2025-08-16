using ClickCut.Application.Ports.In;
using ClickCut.Application.Ports.Out;
using ClickCut.Application.Services;
using ClickCut.Domain.Models;
using ClickCut.Shared.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Moq;

namespace ClickCut.Tests.Application.Services;

[TestClass]
public class AuthServiceTests
{
	private Mock<IJwtServicePort> _jwtService = null!;
	private Mock<IUsersRepositoryPort> _usersRepository = null!;
	private Mock<IPasswordServicePort> _passwordService = null!;
	private AuthServiceImplements _authService = null!;
	private Mock<IBusinessRepositoryPort> _businessReposity = null!;
	private User _validUserMock = null!;
	private Business _validBusinessMock = null!;

	private static User GenerateValidUser()
		=> User.Create(new Email("example@gmail.com"), new Password("VALIDPASS123"), "USERNAME");

	private static Business GenerateValidBusiness(User owner)
		=> Business.Create(owner, "VALID_NAME", null);

	[TestInitialize]
	public void Setup()
	{
		_jwtService = new Mock<IJwtServicePort>();
		_usersRepository = new Mock<IUsersRepositoryPort>();
		_passwordService = new Mock<IPasswordServicePort>();
		_businessReposity = new Mock<IBusinessRepositoryPort>();
		_authService = new AuthServiceImplements(_usersRepository.Object, _passwordService.Object, _jwtService.Object, _businessReposity.Object);
		_validUserMock = GenerateValidUser();
		_validBusinessMock = GenerateValidBusiness(_validUserMock);
	}

	[TestMethod]
	public async Task ItShouldThrowErrorWhenUserNotFoundInAuthBusiness()
	{
		_usersRepository.Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync(default(User));

		var result = await Assert.ThrowsExceptionAsync<BadRequestException>(() => _authService.BusinessAuth(Guid.NewGuid(), Guid.NewGuid()));

		_usersRepository.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
		_businessReposity.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>()), Times.Never);
		result.Message.Should().Be("usuário não encontrado!");
	}

	[TestMethod]
	public async Task ItShouldThrowErrorWhenBusinessNotFound()
	{
		_usersRepository.Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync(_validUserMock);

		var result = await Assert.ThrowsExceptionAsync<BadRequestException>(() => _authService.BusinessAuth(Guid.NewGuid(), Guid.NewGuid()));

		_usersRepository.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
		_businessReposity.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
		result.Message.Should().Be("Negócio não encontrado!");
	}


	[TestMethod]
	public async Task ItShouldThrowErrorWhenBusinessNotBelongsToUser()
	{
		_usersRepository.Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync(_validUserMock);

		_businessReposity.Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync(GenerateValidBusiness(GenerateValidUser()));

		var result = await Assert.ThrowsExceptionAsync<BadRequestException>
			(() => _authService.BusinessAuth(Guid.NewGuid(), Guid.NewGuid()));

		_usersRepository.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
		_businessReposity.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
		result.Message.Should().Be("Loja não pertence ao usuário!");
	}

	[TestMethod]
	public async Task ItShouldReturnAuthResponseOnBusinessAuth()
	{
		_usersRepository.Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync(_validUserMock);

		_businessReposity.Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync(_validBusinessMock);

		var result = await _authService.BusinessAuth(_validUserMock.Id, _validBusinessMock.Id);

		_usersRepository.Verify(repo => repo.FindByIdAsync(_validUserMock.Id), Times.Once);
		_businessReposity.Verify(repo => repo.FindByIdAsync(_validBusinessMock.Id), Times.Once);
	}
}