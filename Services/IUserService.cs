using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ResourcePlanning.Models;

namespace ResourcePlanning.Services
{
    public interface IUserService
    {

        List<GridUserViewModel> GetUsers();

        List<GridRoleViewModel> GetRoles();

        bool Register(GridUserViewModel model);

        bool UpdateUserRole(string userId, string roleId);

        bool RemoveUser(string userId);


    }
}
