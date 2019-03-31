namespace dotnet_core_auth_api.DataContracts.Responses
{
    public class UserInfoResponseDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public string Role { get; set; }
    }
}
