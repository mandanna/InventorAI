namespace InventorAi_api.Models
{
    public class UserRegistrationRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Phonenumber { get; set; }
        public int StoreId { get; set; }

    }
}
