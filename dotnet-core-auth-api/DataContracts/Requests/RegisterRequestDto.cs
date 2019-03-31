using System.ComponentModel.DataAnnotations;

namespace dotnet_core_auth_api.DataContracts.Requests
{
    public class RegisterRequestDto
    {
        [Required, MinLength(3)]
        public string FirstName { get; set; }

        [Required, MinLength(3)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "Password does not match.")]
        public string PasswordConfirm{ get; set; }

        public int? RoleId { get; set; }
    }
}