using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrResData.Entities;
using StrResServices.Interfaces;
using static StrResApi.Auth.Constants;

namespace StrResApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "StrResAuthScheme")]
    public class TenantsController : StrResControllerbase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        // GET: api/Tenants
        [HttpGet]
        public IEnumerable<Tenant> GetTenant()
        {
            if (User.IsInRole(ADMIN_ROLE))
            {
                return _tenantService.GetTenants();
            }
            else
            {
                return _tenantService.GetTenants(getTenantId());
            }
        }

        // GET: api/Tenants/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenant([FromRoute] long id)
        {
            if (!isOperationAuthorized(id))
            {
                return Unauthorized();
            }

            var tenant = await _tenantService.GetTenant(id);

            if (tenant == null)
            {
                return NotFound();
            }

            return Ok(tenant);
        }

        // PUT: api/Tenants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenant([FromRoute] long id, [FromBody] Tenant tenant)
        {
            if (id != tenant.TenantId)
            {
                return BadRequest();
            }

            if (!isOperationAuthorized(id))
            {
                return Unauthorized();
            }

            try
            {
                await _tenantService.UpdateTenant(tenant);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Tenants
        [HttpPost]
        public async Task<IActionResult> PostTenant([FromBody] Tenant tenant)
        {
            // only admins can create new tenants
            if (!User.IsInRole(ADMIN_ROLE))
            {
                return Unauthorized();
            }

            try
            {
                await _tenantService.AddTenant(ref tenant);
            }
            catch (Exception)
            {
                return UnprocessableEntity();
            }
            
            return CreatedAtAction("GetTenant", new { id = tenant.TenantId }, tenant);
        }

        // DELETE: api/Tenants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenant([FromRoute] long id)
        {
            // only admins can delete tenants
            if (!User.IsInRole(ADMIN_ROLE))
            {
                return Unauthorized();
            }

            var tenant = await _tenantService.GetTenant(id);

            if (tenant == null)
            {
                return NotFound();
            }

            await _tenantService.DeleteTenant(tenant);

            return Ok(tenant);
        }
    }
}