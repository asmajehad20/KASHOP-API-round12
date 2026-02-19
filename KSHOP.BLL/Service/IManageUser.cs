using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.BLL.Service
{
    public interface IManageUser
    {
        Task<List<UserListResponse>> GetUsersAsync();
        Task<UserDetailsResponse> GetUserDetailsAsync();

        Task<BaseResponse> BlockedUserAsync(string userId);
        Task<BaseResponse> UnBlockedUserAsync(string userId);
        Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request);
    }
}
