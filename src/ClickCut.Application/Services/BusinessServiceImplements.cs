using ClickCut.Application.Commands;
using ClickCut.Application.Ports.In;
using ClickCut.Application.Ports.Out;
using ClickCut.Application.Utils;
using ClickCut.Domain.Models;
using ClickCut.Shared.Exceptions;
using ClickCut.Shared.Extensions;
using ClickCut.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace ClickCut.Application.Services;

public class BusinessServiceImplements(IBusinessRepositoryPort businessRepositoryPort, ILogger<BusinessServiceImplements> logger, IUsersRepositoryPort usersRepositoryPort) : IBusinessServicePort
{
	private readonly IUsersRepositoryPort _usersRepository = usersRepositoryPort;
	private readonly IBusinessRepositoryPort _businessRepository = businessRepositoryPort;
	private readonly ILogger<BusinessServiceImplements> _logger = logger;

	public async Task<Business> CreateAsync(CreateBusinessCommand createBusinessCommand)
	{
		User owner = await _usersRepository.FindByIdAsync(createBusinessCommand.OwnerId) ??
			throw new BadRequestException("usuário não encontrado!");

		var password = createBusinessCommand.Password is not null ?
		 	SafeFactory.TryCreate(() => new Password(createBusinessCommand.Password))
				.ThrowIfHasError() : null;

		Result<Business> businessResult = SafeFactory.TryCreate(() => Business.Create(
			owner: owner,
			name: createBusinessCommand.Name,
			password: password
		));

		if (businessResult.IsFailure)
			throw new BadRequestException(businessResult.Error);

		try
		{
			Business business = businessResult.Value!;
			Business createdBusiness = await _businessRepository.SaveAsync(business);

			return createdBusiness;
		}
		catch (Exception err)
		{
			_logger.LogError(err, "Houve um erro interno no servidor!");
			throw new BadRequestException("Houve um erro interno, tente novamente mais tarde!");
		}

	}

	public Task<List<Business>> FindByUserAndMembersAsync(Guid ownerId)
	{
		throw new NotImplementedException();
	}
}