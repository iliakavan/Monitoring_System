

namespace notifier.Domain.Dto;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime RecordDate { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;

}
