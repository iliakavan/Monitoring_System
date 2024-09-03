namespace notifier.Application.User.Query.Search;



public class SearchUserQueryRequest : IRequest<ResultResponse<IEnumerable<UserDto>>>
{
        public DateTime? startDate {  get; set; } 
        public DateTime? endDate {  get; set; }
        public string? FullName {  get; set; }
        public string? FirstName {  get; set; }
        public string? LastName {  get; set; }
        public string? Email {  get; set; }
        public string? PhoneNumber {  get; set; }
        public Role? Role { get; set; }
        public string? UserName {  get; set; }

}
