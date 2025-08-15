using ClickCut.Application.Ports.Out;
using ClickCut.Domain.Models;
using ClickCut.Infra.Adapters.Out.Persistence.DatabaseContext;

namespace ClickCut.Infra.Adapters.Out.Persistence.Repositories;

public class BusinessRepositoryImplements(AppDatabaseContext context) : IBusinessRepositoryPort
{
	public AppDatabaseContext _context = context;

	public async Task<Business> SaveAsync(Business business)
	{
		var created = await _context.Businesses.AddAsync(business);
		await _context.SaveChangesAsync();
		return created.Entity;
	}
}