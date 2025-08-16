using ClickCut.Application.Commands;
using ClickCut.Application.Ports.Out;
using ClickCut.Application.Services;
using ClickCut.Domain.Models;
using ClickCut.Domain.Utils;
using ClickCut.Shared.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClickCut.Tests.Application.Services;

[TestClass]
public class BusinessServiceTests
{
	protected Mock<IBusinessRepositoryPort> _businessRepositoryMock = null!;
	protected Mock<IUsersRepositoryPort> _usersRepositoryMock = null!;
	protected BusinessServiceImplements _businessService = null!;
	protected Mock<ILogger<BusinessServiceImplements>> _loggerMock = null!;

	[TestInitialize]
	public void Setup()
	{
		_businessRepositoryMock = new Mock<IBusinessRepositoryPort>();
		_loggerMock = new Mock<ILogger<BusinessServiceImplements>>();
		_usersRepositoryMock = new Mock<IUsersRepositoryPort>();

		_businessService = new BusinessServiceImplements(
			businessRepositoryPort: _businessRepositoryMock.Object,
			usersRepositoryPort: _usersRepositoryMock.Object,
			logger: _loggerMock.Object
		);
	}

}

[TestClass]
public class CreateAsyncMethodTests : BusinessServiceTests
{
	[TestMethod]
	public async Task ItShouldThrowErrorWhenUserNotFound()
	{
		_usersRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((User)null!);

		CreateBusinessCommand createBusinessCommand = new CreateBusinessCommand(
			Name: "Any name",
			OwnerId: Guid.NewGuid(),
			Password: null
		);


		var result = await Assert.ThrowsExceptionAsync<BadRequestException>(
			() => _businessService.CreateAsync(createBusinessCommand)
		);

		_usersRepositoryMock.Verify(u => u.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
		result.Message.Should().Be("usuário não encontrado!");
	}

	[TestMethod]
	public async Task ItShouldThrowErroInValidationDomain()
	{
		User user = User.Create(new Email("example@gmail.com"), new Password("anypassword1"), "_USERNAME_");

		_usersRepositoryMock.Setup(x => x.FindByIdAsync(user.Id))
			.ReturnsAsync(user);

		CreateBusinessCommand createBusinessCommand = new CreateBusinessCommand(
			Name: "_",
			OwnerId: user.Id,
			Password: null
		);

		var result = await Assert.ThrowsExceptionAsync<BadRequestException>(
			() => _businessService.CreateAsync(createBusinessCommand)
		);

		_usersRepositoryMock.Verify(u => u.FindByIdAsync(It.IsAny<Guid>()), Times.Once);
		result.Message.Should().Contain($"O nome deve ter no mínimo {ModelsConfig.User.MinUsernameLength} caracteres.");
	}

	[TestMethod]
	public async Task ItShouldThrowErrorWhenBusinnesExistsInDatabase()
	{
		User user = User.Create(new Email("example@gmail.com"), new Password("anypassword1"), "_USERNAME_");
		Business business = Business.Create(user, "VALID_NAME", null);

		_usersRepositoryMock.Setup(x => x.FindByIdAsync(user.Id))
			.ReturnsAsync(user);

		CreateBusinessCommand createBusinessCommand = new CreateBusinessCommand(
			Name: "VALID_NAME",
			OwnerId: user.Id,
			Password: null
		);

		_businessRepositoryMock.Setup(b => b.FindByName(business.Name))
			.ReturnsAsync(business);

		var result = await Assert.ThrowsExceptionAsync<BadRequestException>(() => _businessService.CreateAsync(createBusinessCommand));

		_businessRepositoryMock.Verify(b => b.FindByName(business.Name), Times.Once);

		result.Message.Should().Be("Já existe um negócio com esse nome");
	}
}