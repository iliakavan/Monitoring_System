
namespace notifier.Infrastructure.Repositories;




public class UserRepository : IUserRepository
{
    private readonly AppDbcontext _context;

    public UserRepository(AppDbcontext context)
    {
        _context = context;
    }
    public async Task<Users?> Authenticate(string? usernameOrEmail, string password)
    {
        var user = await _context.Users.AsQueryable()
            .Where(u => u.UserName.ToLower() == usernameOrEmail ||
                        u.Email == usernameOrEmail).FirstOrDefaultAsync();
        
        if(BC.EnhancedVerify(text: password,hash: user?.Password))
        {
            return user;
        }
        return null;
    }

    public void DeactiveUser(Users user)
    {
        user.IsActive = false;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<IEnumerable<Users>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<Users?> GetUserByID(int id)
    {
        return await _context.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
    }

    public async Task Register(Users user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<IEnumerable<UserDto>> Search(DateTime? startDate, DateTime? endDate, string? FirstName, string? LastName, string? Email, string? PhoneNumber, Role? role, string? UserName)
    {
        DateTime start = startDate ?? DateTime.MinValue;

        DateTime end = endDate ?? DateTime.MaxValue;

        var query = _context.Users.AsQueryable().Where(x => x.RecordDate.Date >= start && x.RecordDate.Date <= end);

        if(!string.IsNullOrEmpty(FirstName))
        {
            query = query.Where(x => x.FirstName.ToLower() == FirstName.ToLower());

        }

        if (!string.IsNullOrEmpty(LastName)) 
        {
            query = query.Where(x => x.LastName.ToLower() == LastName.ToLower());
        }

        if(!string.IsNullOrEmpty(Email)) 
        {
            query = query.Where(x => x.Email == Email);
        }
        if (!string.IsNullOrEmpty(PhoneNumber)) 
        {
            query = query.Where(x => x.PhoneNumber == PhoneNumber);
        }
        if(role is not null) 
        {
            query = query.Where(x => x.Role == role);
        }
        if (!string.IsNullOrEmpty(UserName)) 
        {
            query = query.Where(x => x.UserName == UserName);
        }

        return await query.Select(user => new UserDto()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            UserName = user.UserName,
            RecordDate = user.RecordDate,
            Role = user.Role.ToString(),
        }).ToListAsync();
    }

    public void Update(Users user)
    {
        _context.Users.Update(user);
    }
}
