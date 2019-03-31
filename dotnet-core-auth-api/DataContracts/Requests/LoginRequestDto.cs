using System.ComponentModel.DataAnnotations;

namespace dotnet_core_auth_api.DataContracts.Requests
{
    public class LoginRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
