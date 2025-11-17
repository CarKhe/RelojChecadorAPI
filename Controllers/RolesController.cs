using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using relojChecadorAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _service;

        public RolesController(IRolesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async  Task<ActionResult<IEnumerable<RolesDto>>> GetRoles()
        {
            var roles = await _service.GetRoles();
            if(!roles.Any()) return NotFound();
            return Ok(roles);
        }
    }
}
