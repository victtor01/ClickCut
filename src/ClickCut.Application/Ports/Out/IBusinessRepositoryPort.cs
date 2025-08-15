using ClickCut.Domain.Models;

namespace ClickCut.Application.Ports.Out;

public interface IBusinessRepositoryPort
{
	public Task<Business> SaveAsync(Business business);
}