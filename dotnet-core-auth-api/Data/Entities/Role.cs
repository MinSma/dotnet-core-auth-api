using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace dotnet_core_auth_api.Data.Entities
{
    public class Role : IdentityRole<int>
    {
        public List<User> Users { get; set; }
    }
}
