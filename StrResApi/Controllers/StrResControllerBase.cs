using System.Linq;
using Microsoft.AspNetCore.Mvc;
using static StrResApi.Auth.Constants;

namespace StrResApi.Controllers
{
    public class StrResControllerbase : ControllerBase
    {
        public bool isOperationAuthorized(long tenantId)
        {
            if (User.IsInRole(ADMIN_ROLE))
            {
                return true;
            }

            return tenantId == getTenantId();
        }

        public long getTenantId()
        {
            if (User.IsInRole(TENANT_ROLE) && User.HasClaim(c => c.Type == TENANT_ID_CLAIM_TYPE))
            {
                string tenantIdClaimString = User.Claims.FirstOrDefault(c => c.Type == TENANT_ID_CLAIM_TYPE).Value;
                long tenantIdClaim = 0;

                if (!long.TryParse(tenantIdClaimString, out tenantIdClaim))
                {
                    return INVALID_TENANT_ID;
                }

                return tenantIdClaim;
            }

            return INVALID_TENANT_ID;
        }
    }
}