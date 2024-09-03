namespace notifier.Domain.Models;

[Table("Users",Schema = "dbo")]
public class Users : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName 
    {
        get
        {
            return $"{FirstName} {LastName}";
        }
    }
    public string PhoneNumber { get; set; } = null!;
    public Role Role { get; set;}
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;

}
