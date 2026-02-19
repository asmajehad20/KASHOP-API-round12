using KSHOP.DAL.Data;
using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Dtos.Response;
using KSHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.BLL.Service
{
    public class ManageUser : IManageUser
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageUser(UserManager<ApplicationUser> userManager) 
        { 
            _userManager = userManager;
        }

        public async Task<List<UserListResponse>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var result = users.Adapt<List<UserListResponse>>();

            for (int i=0; i<users.Count; i++)
            {
                var roles = await _userManager.GetRolesAsync(users[i]);
                result[i].Roles = roles.ToList();
            }
            
            return result;
        }

        public Task<UserDetailsResponse> GetUserDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> BlockedUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await _userManager.UpdateAsync(user);

            return new BaseResponse
            {
                Success = true,
                Message = "user blocked"
            };
        }

        
        public async Task<BaseResponse> UnBlockedUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.SetLockoutEndDateAsync(user, null);
            await _userManager.UpdateAsync(user);

            return new BaseResponse
            {
                Success = true,
                Message = "user unblocked"
            };
        }

        public async Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            var curruentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, curruentRoles);
            await _userManager.AddToRoleAsync(user, request.Role);
            return new BaseResponse
            {
                Success = true,
                Message = "role updated"
            };
        }
    }
}
