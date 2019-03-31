using System.ComponentModel.DataAnnotations;

namespace dotnet_core_auth_api.DataContracts.Requests
{
    public class GetUserInfoRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
