using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace StrResApi.Auth
{
    public class StrResAuthOptions : AuthenticationSchemeOptions
    {
        public ClaimsIdentity Identity { get; set; }
    }
}