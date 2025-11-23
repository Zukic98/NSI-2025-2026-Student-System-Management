using Identity.Core.Entities;
using Identity.Core.Repositories;
using Identity.Infrastructure.Db;
using Identity.Infrastructure.Entities;

namespace Identity.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUser(string email)
    {
        var newUser = new ApplicationUser { UserName = email };
        await _context.Users.AddAsync(newUser);
        
        // TODO: implement mappers (za sada direktno mapiramo na domen User)
        return new User(newUser.Id, newUser.UserName);
    }

    public async Task<User> GetByIdAsync(string id)
    {
        var appUser = await _context.Users.FindAsync(id);
        if (appUser is null) throw new InvalidOperationException($"User with id '{id}' not found.");
        return new User(appUser.Id, appUser.UserName);
    }

    public Task Save()
    {
        return _context.SaveChangesAsync();
    }
}
