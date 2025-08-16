using ClickCut.Application.Ports.Out;
using ClickCut.Domain.Models;
using ClickCut.Infra.Adapters.Out.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ClickCut.Infra.Adapters.Out.Persistence.Repositories;

public class BusinessRepositoryImplements(AppDatabaseContext context) : IBusinessRepositoryPort
{
	public AppDatabaseContext _context = context;

	public async Task<List<Business>> FindAllByUserOrMember(User user)
	{
		return await _context.Businesses
			.Where(b => b.Owner.Id == user.Id || b.Members.Any(m => m.Id == user.Id))
			.AsNoTracking()
			.ToListAsync();
	}

	public async Task<Business?> FindByIdAsync(Guid businessId)
	{
		return await _context.Businesses.FirstOrDefaultAsync(b => b.Id == businessId);
	}

	public async Task<Business?> FindByName(string name)
	{
		return await _context.Businesses.FirstOrDefaultAsync(b => b.Name == name);
	}

	public async Task<Business> SaveAsync(Business business)
	{
		var created = await _context.Businesses.AddAsync(business);
		await _context.SaveChangesAsync();
		return created.Entity;
	}
}