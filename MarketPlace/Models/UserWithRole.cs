namespace MarketPlace.Models
{
    public class UserWithRole
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string? Email { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
