using ClickCut.Application.Ports.Out;
using ClickCut.Domain.Models;
using ClickCut.Infra.Adapters.Out.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ClickCut.Infra.Adapters.Out.Persistence.Repositories;

public class UsersRepositoryImplements(AppDatabaseContext appDatabaseContext) : IUsersRepositoryPort
{
	private readonly AppDatabaseContext _context = appDatabaseContext;

	public async Task<User> CreateUserAsync(User user)
	{
		var created = await _context.AddAsync(user);
		await _context.SaveChangesAsync();
		return created.Entity;
	}

	public async Task<User?> FindByEmailAsync(Email email)
	{
		return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
	}

	public async Task<User?> FindByIdAsync(Guid Id)
	{
		return await _context.Users.FirstOrDefaultAsync(user => user.Id == Id);
	}
}