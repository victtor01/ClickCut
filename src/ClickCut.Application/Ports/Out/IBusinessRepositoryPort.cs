using ClickCut.Domain.Models;

namespace ClickCut.Application.Ports.Out;

public interface IBusinessRepositoryPort
{
	public Task<Business> SaveAsync(Business business);
	public Task<List<Business>> FindAllByUserOrMember(User user);
	public Task<Business?> FindByIdAsync(Guid businessId);
	public Task<Business?> FindByName(string name);
}