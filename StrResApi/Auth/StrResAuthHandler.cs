using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StrResData.Entities;
using StrResServices.Interfaces;
using static StrResApi.Auth.Constants;

namespace StrResApi.Auth
{
    public class StrResAuthHandler : AuthenticationHandler<StrResAuthOptions>
    {
        private readonly IAdminService _adminService;
        private readonly ITenantService _tenantService;

        public StrResAuthHandler(IOptionsMonitor<StrResAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAdminService adminService,
            ITenantService tenantService)
            : base(options, logger, encoder, clock)
        {
            _adminService = adminService;
            _tenantService = tenantService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string adminName = Request.Headers[ADMIN_NAME_HEADER];
            string tenantIdString = Request.Headers[TENANT_ID_HEADER];
            string accessToken = Request.Headers[ACCESS_TOKEN_HEADER];

            // must specify either admin name or tenant id (not both)
            // must always specify an access token
            if ((string.IsNullOrWhiteSpace(adminName) && string.IsNullOrWhiteSpace(tenantIdString)) ||
                (!string.IsNullOrWhiteSpace(adminName) && !string.IsNullOrWhiteSpace(tenantIdString)) ||
                (string.IsNullOrEmpty(accessToken)))
            {
                return Task.FromResult(AuthenticateResult.Fail(INVALID_REQUEST_MESSAGE));
            }

            // process admin authentication
            if (!string.IsNullOrEmpty(adminName))
            {
                var admin = _adminService.GetAdmin(adminName).Result;

                if ((admin == null) || (accessToken != admin.AccessToken))
                {
                    return Task.FromResult(AuthenticateResult.Fail(UNAUTHORIZED_ACCESS_MESSAGE));
                }
                else
                {
                    var adminClaims = new List<Claim> {
                        new Claim(ClaimTypes.Role, ADMIN_ROLE, ISSUER),
                        new Claim(ADMIN_NAME_CLAIM_TYPE, adminName, ISSUER)
                    };

                    var adminIdentity = new ClaimsIdentity(adminClaims, STR_RES_AUTH_SCHEME);

                    Options.Identity = adminIdentity;

                    return Task.FromResult(
                        AuthenticateResult.Success(
                            new AuthenticationTicket(
                                new ClaimsPrincipal(Options.Identity),
                                new AuthenticationProperties(),
                                this.Scheme.Name)));
                }
            }

            // tenant ids are always positive longs
            long tenantId;
            if (!long.TryParse(tenantIdString, out tenantId) || (tenantId < 1))
            {
                return Task.FromResult(AuthenticateResult.Fail(UNAUTHORIZED_ACCESS_MESSAGE));
            }

            // process tenant authentication
            var tenant = _tenantService.GetTenant(tenantId).Result;

            if ((tenant == null) || (accessToken != tenant.AccessToken))
            {
                return Task.FromResult(AuthenticateResult.Fail(UNAUTHORIZED_ACCESS_MESSAGE));
            }

            var tenantClaims = new List<Claim> {
                new Claim(ClaimTypes.Role, TENANT_ROLE, ISSUER),
                new Claim(TENANT_ID_CLAIM_TYPE, tenantIdString, ISSUER)
            };

            var tenantIdentity = new ClaimsIdentity(tenantClaims, STR_RES_AUTH_SCHEME);

            Options.Identity = tenantIdentity;

            return Task.FromResult(
                 AuthenticateResult.Success(
                    new AuthenticationTicket(
                        new ClaimsPrincipal(Options.Identity),
                        new AuthenticationProperties(),
                        this.Scheme.Name)));
        }
    }

    public static class StrResAuthExtensions
    {
        public static AuthenticationBuilder AddStrResAuth(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<StrResAuthOptions> configureOptions)
        {
            return builder.AddScheme<StrResAuthOptions, StrResAuthHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}