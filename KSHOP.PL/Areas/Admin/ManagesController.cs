using KSHOP.BLL.Service;
using KSHOP.DAL.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KSHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ManagesController : ControllerBase
    {
        private readonly IManageUser _manageUser;

        public ManagesController(IManageUser manageUser) 
        { 
            _manageUser = manageUser;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _manageUser.GetUsersAsync();
            return Ok(result);
        }

        [HttpPatch("block/{id}")]
        public async Task<IActionResult> BlockUser([FromRoute] string id)
        {
            var result = await _manageUser.BlockedUserAsync(id);
            return Ok(result);
        }

        [HttpPatch("unblock/{id}")]
        public async Task<IActionResult> UnBlockUser([FromRoute] string id)
        {
            var result = await _manageUser.UnBlockedUserAsync(id);
            return Ok(result);
        }

        [HttpPatch("change-role")]
        public async Task<IActionResult> ChangeRole([FromBody] ChangeUserRoleRequest request)
        {
            var result = await _manageUser.ChangeUserRoleAsync(request);
            return Ok(result);
        }

    }
}
