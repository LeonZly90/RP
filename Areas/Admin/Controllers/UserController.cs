using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ResourcePlanning.Models;
using ResourcePlanning.Utility;
using ResourcePlanning.Data;
using ResourcePlanning.Services;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Identity;

namespace ResourcePlanning.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IUserService _userservice;


        public UserController(ApplicationDbContext db, IUserService userservice)
        {
            _db = db;
            _userservice = userservice;
        }
        public IActionResult Index()
        {

            ViewData["roles"] = _userservice.GetRoles(); 
            return View();
        }

        public IActionResult Users_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<GridUserViewModel> u = _userservice.GetUsers();
            DataSourceResult result = u.ToDataSourceResult(request);
            return Json(result);
        }

        [AcceptVerbs("Post")]
        public IActionResult Users_Update([DataSourceRequest] DataSourceRequest request, GridUserViewModel user)
        {
            if(user != null && ModelState.IsValid)
            {
                _userservice.UpdateUserRole(user.Id, user.RoleId);
            }
            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public IActionResult Users_Create([DataSourceRequest] DataSourceRequest request, GridUserViewModel user)
        {
            if(user != null && ModelState.IsValid)
            {
                _userservice.Register(user);
            }
            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public IActionResult Users_Destroy([DataSourceRequest] DataSourceRequest request, GridUserViewModel user)
        {
            if (user != null)
            {
                _userservice.RemoveUser(user.Id);
            }
            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }


    }
}
