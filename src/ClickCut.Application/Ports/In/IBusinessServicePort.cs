using ClickCut.Application.Commands;
using ClickCut.Domain.Models;

namespace ClickCut.Application.Ports.In;

public interface IBusinessServicePort
{
	public Task<Business> CreateAsync(CreateBusinessCommand createBusinessCommand);
	public Task<List<Business>> FindByUserAndMembersAsync(Guid ownerId);
}