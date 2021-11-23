using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ResourcePlanning.Services;
using ResourcePlanning.Utility;
using ResourcePlanning.Models;
using System.Data;
using Kendo.Mvc.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Reflection;
using System;
using Microsoft.AspNetCore.Authorization;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace ResourcePlanning.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        BO busObj = new BO();
        private readonly IMemoryCache memoryCache;
        private readonly IApplicationContext _appContext;
        private ISession _session;
        public ISession Session { get { return _session; } }

        public HomeController(IMemoryCache memoryCache, IApplicationContext appContext, IHttpContextAccessor httpContextAccessor)
        {
            this.memoryCache = memoryCache;
            _appContext = appContext;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test(string jobNumber, string empNumber)
        {
            ViewData["_JobNumber"] = jobNumber;
            ViewData["_EmpNumber"] = empNumber;
            return View();
        }

        [AllowAnonymous]
        public IActionResult EmployeeExperience(string empNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);

            var e = _appContext.StoProc.List<PersonnelMarketExperienceVM>(SD.Stoproc_GetEmpMarketExperience, parameter).ToList();

            return View(e);
        }


        [HttpPost]
        public JsonResult GetResources(ResourceFilter filters)
        {
            string SortFields = "GroupLeader ASC";
            //
            var projUserVM = GetProjectUsers(true, filters);
            return Json(projUserVM);
        }

        [HttpGet]
        public JsonResult GetEmpDetail(string empNo)
        {
            var empDetails = busObj.GetEmpJobHours(empNo, "Open");
            return Json(empDetails);
        }

        [HttpGet]
        public JsonResult GetEmpMarketExperience(string empNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);

            var experience = _appContext.StoProc.List<PersonnelMarketExperienceVM>(SD.Stoproc_GetEmpMarketExperience, parameter).ToList();

            return Json(experience);
        }

        [HttpGet]
        public JsonResult GetEmpCertification(string empNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);

            var certs = _appContext.StoProc.List<PersonnelCertificationVM>(SD.Stoproc_GetEmpCertification, parameter).ToList();

            return Json(certs);
        }

        [HttpGet]
        public JsonResult GetEmpEducation(string empNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);

            var edu = _appContext.StoProc.List<PersonnelEducationVM>(SD.Stoproc_GetEmpEducation, parameter).ToList();

            return Json(edu);
        }

        [HttpGet]
        public JsonResult GetEmpExperience(string empNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);

            var exp = _appContext.StoProc.List<PersonnelExperienceVM>(SD.Stoproc_GetEmpExperience, parameter).ToList();

            return Json(exp);
        }

        [HttpPost]
        public JsonResult SaveException(PepperResourceException exception)
        {
            int? status = busObj.SaveException(exception);
            string msg = status >= 1 ? "Exception saved successfully" : "Unable to save the exception at this time.";
            return Json(msg);
        }

        private ProjectUserVM GetProjectUsers(bool RefreshCache, ResourceFilter filters = null)
        {
            string sCompany = _session.GetString("_SessionKey_Company");
            //
            bool anyFilters = filters.GetType().GetProperties()
                .Where(pi => pi.PropertyType == typeof(List<string>))
                .Select(pi => (List<string>)pi.GetValue(filters))
                .Any(val => val != null);

            if (RefreshCache == true || !memoryCache.TryGetValue(SD.Cache_ProjectUser, out ProjectUserVM projectUserVM))
            {
                projectUserVM = busObj.GetProjectUsers(sCompany);
                memoryCache.Set(SD.Cache_ProjectUser, projectUserVM);
            }

            if (anyFilters)
            {
                projectUserVM.ProjectUserList = FilterProjectUsers(filters, projectUserVM.ProjectUserList);
            }

            return projectUserVM;
        }

        private IEnumerable<ProjectUser> FilterProjectUsers(ResourceFilter filters, IEnumerable<ProjectUser> projectUsers)
        {
            if (filters.GroupLeader?.Count > 0)
            {
                projectUsers = projectUsers.Where(pu => filters.GroupLeader.Contains(pu.GroupLeader));
            }
            if (filters.ProjectDirector?.Count > 0)
            {
                projectUsers = projectUsers.Where(pu => filters.ProjectDirector.Contains(pu.ProjectDirector));
            }
            if (filters.JobNumber?.Count > 0)
            {
                projectUsers = projectUsers.Where(pu => filters.JobNumber.Contains(pu.JobNumber));
            }
            if (filters.JobName?.Count > 0)
            {
                projectUsers = projectUsers.Where(pu => filters.JobName.Contains(pu.JobName));
            }
            if (filters.JobStatus?.Count > 0)
            {
                projectUsers = projectUsers.Where(pu => filters.JobStatus.Contains(pu.JobStatus));
            }
            if (filters.Employee?.Count > 0)
            {
                projectUsers = projectUsers.Where(pu => filters.Employee.Contains(pu.Employee));
            }
            if (filters.Title?.Count > 0)
            {
                projectUsers = projectUsers.Where(pu => filters.Title.Contains(pu.Title));
            }

            return projectUsers;
        }


        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            Response.Headers.Add("Content-Disposition", "inline; filename=" + fileName);
            return File(fileContents, contentType);
        }

    }
}
