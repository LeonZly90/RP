using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResourcePlanning.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourcePlanning.Data;
using ResourcePlanning.Models;
using Microsoft.AspNetCore.Identity;

namespace ResourcePlanning.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<RPUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private BO busObject = new BO();
        public UserService(ApplicationDbContext db, UserManager<RPUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public List<GridUserViewModel> GetUsers()
        {
            var users = _db.RPUsers.Select(u => new GridUserViewModel {Id = u.Id, Email = u.Email, FirstName = u.FirstName, LastName = u.LastName}).ToList();
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in users)
            {
                var userRole = userRoles.FirstOrDefault(u => u.UserId == user.Id);

                if (userRole == null)
                {
                    var viewerRole = _roleManager.FindByNameAsync("Viewer").Result;
                    user.RoleId = viewerRole.Id;
                }
                else
                {
                    var existingUserRole = roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                    user.RoleId = existingUserRole.Id;
                }
            }

            return users;
        }

        public List<GridRoleViewModel> GetRoles()
        {
            var roles = _db.Roles.ToList();
            var roleViewModels = roles.Select(r => new GridRoleViewModel { RoleId = r.Id, RoleName = r.Name }).ToList();

            return roleViewModels;
        }

        public bool Register(GridUserViewModel model)
        {
            bool bOK = false;

            try
            {

                var u = busObject.GetCmiCUser(model.Email);

                var user = new RPUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Organization = "Pepper",
                    IsDomainUser = true
                };

                // Create user
                var result = _userManager.CreateAsync(user).GetAwaiter().GetResult();
                // Check record in database
                RPUser userNewRecord = _db.RPUsers.Where(u => u.Email == model.Email).FirstOrDefault();
                //
                if (userNewRecord != null)
                {
                    // Add Default role
                    _userManager.AddToRoleAsync(user, SD.Role_Viewer).GetAwaiter().GetResult();
                }

                bOK = true;

            }
            catch (Exception)
            {
                bOK = false;
                throw;
            }

            return bOK;
        }

        public bool UpdateUserRole(string userId, string roleId)
        {
            bool bOK = false;

            try
            {

                var userFromDb = _db.RPUsers.FirstOrDefault(u => u.Id == userId);

                if (userFromDb != null)
                {
                    var userRole = _db.UserRoles.FirstOrDefault(u => u.UserId == userId);
                    if (userRole != null)
                    {
                        var previousRole = _db.Roles.Where(r => r.Id == userRole.RoleId).FirstOrDefault();
                        //Remove old Role
                        _userManager.RemoveFromRoleAsync(userFromDb, previousRole.Name).GetAwaiter().GetResult();
                    }
                    // Verify new role
                    var newrole = _db.Roles.FirstOrDefault(r => r.Id == roleId);
                    if (newrole != null)
                    {
                        _userManager.AddToRoleAsync(userFromDb, newrole.Name).GetAwaiter().GetResult();
                        bOK = true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }

            return bOK;
        }


        public bool RemoveUser(string userId)
        {
            bool bOK = false;

            var userFromDb = _db.RPUsers.FirstOrDefault(u => u.Id == userId);

            if (userFromDb != null)
            {
                _db.RPUsers.Remove(userFromDb);
                _db.SaveChanges();

                bOK = true;
            }

            return bOK;

        }




    }
}
