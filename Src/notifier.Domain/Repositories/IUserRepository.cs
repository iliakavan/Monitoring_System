namespace notifier.Domain.Repositories;



public interface IUserRepository : IDisposable
{
    Task Register(Users user);

    Task<Users?> Authenticate(string usernameOremail,string password);

    void Update(Users user);

    Task<IEnumerable<Users>> GetAll();

    Task<IEnumerable<UserDto>> Search(DateTime? startDate,DateTime? endDate,string? FirstName,string? LastName,string? Email,string? PhoneNumber,Role? role,string? UserName);

    void DeactiveUser(Users user);
    Task<Users?> GetUserByID(int id);
}
