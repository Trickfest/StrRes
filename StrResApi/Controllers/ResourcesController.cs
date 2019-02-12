using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ResourcesController : StrResControllerbase

    {
        private readonly IResourceService _resourceService;

        public ResourcesController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        // GET: api/Resources
        [HttpGet]
        public IEnumerable<Resource> GetResource()
        {
            if (User.IsInRole(ADMIN_ROLE))
            {
                return _resourceService.GetResources();
            }
            else
            {
                return _resourceService.GetResources(getTenantId());
            }
        }

        // GET: api/Resources/1/foo
        [HttpGet("{tenantId}/{key}")]
        public async Task<IActionResult> GetResource(
            [FromRoute] long tenantId,
            [FromRoute] string key)
        {
            if (!isOperationAuthorized(tenantId))
            {
                return Unauthorized();
            }

            var resource = await _resourceService.GetResource(tenantId, key);

            if (resource == null)
            {
                return NotFound();
            }

            return Ok(resource);
        }

        // PUT: api/Resources/1/1/foo
        [HttpPut("{tenantId}/{key}")]
        public async Task<IActionResult> PutResource(
            [FromRoute] long tenantId,
            [FromRoute] string key,
            [FromBody] Resource resource)
        {
            if ((tenantId != resource.TenantId) ||
                (key != resource.Key))
            {
                return BadRequest();
            }

            if (!isOperationAuthorized(tenantId))
            {
                return Unauthorized();
            }

            try
            {
                await _resourceService.UpdateResource(resource);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Resources
        [HttpPost]
        public async Task<IActionResult> PostResource([FromBody] Resource resource)
        {
            if (!isOperationAuthorized(resource.TenantId))
            {
                return Unauthorized();
            }

            try
            {
                await _resourceService.AddResource(ref resource);
            }
            catch (Exception)
            {
                return UnprocessableEntity();
            }

            return CreatedAtAction("GetResource",
                new { tenantId = resource.TenantId, key = resource.Key },
                resource);
        }

        // DELETE: api/Resources/5
        [HttpDelete("{tenantId}/{key}")]
        public async Task<IActionResult> DeleteResource(
            [FromRoute] long tenantId,
            [FromRoute] string key)
        {
            if (!isOperationAuthorized(tenantId))
            {
                return Unauthorized();
            }

            var resource = await _resourceService.GetResource(tenantId, key);

            if (resource == null)
            {
                return NotFound();
            }

            await _resourceService.DeleteResource(resource);

            return Ok(resource);
        }
    }
}