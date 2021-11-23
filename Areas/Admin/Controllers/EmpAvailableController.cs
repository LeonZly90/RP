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
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ResourcePlanning.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class EmpAvailableController : Controller
    {
        BO busObj = new BO();
        private readonly ILogger<EmpAvailableController> _logger;
        //private readonly ProjectService _ps;

        //private ISession _session;
        //public ISession Session { get { return _session; } }

        public EmpAvailableController(ILogger<EmpAvailableController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            //_session = httpContextAccessor.HttpContext.Session;

            //_ps = ps;
        }

        public ActionResult Search_Panel()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult EmployeeDetail(string empNumber)
        //{
        //    //ViewData["_JobNumber"] = jobNumber;
        //    //ViewData["_EmpNumber"] = empNumber;
        //    if (empNumber is not null)
        //    {
        //        _session.SetString("_SessionKey_EmpNumber", empNumber);
        //    }
        //    else
        //    {
        //        _session.SetString("_SessionKey_EmpNumber", "");
        //    }
        //    return View();
        //}


        //public IActionResult EmployeeDetail(string jobNumber, string empNumber)
        //{



        //    // Set Session Variable
        //    _session.SetString("_SessionKey_JobNumber", jobNumber);
        //    _session.SetString("_SessionKey_EmpNumber", empNumber);
        //    return View();
        //}



        //public IActionResult EmployeeDetail_Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    string empNumber = _session.GetString("_SessionKey_EmpNumber");
        //    DataSourceResult result = null;
        //    if (!string.IsNullOrEmpty(empNumber))
        //    {
        //        List<EmpJobHour> EmpDetailList = busObj.GetEmpJobHours(empNumber, "Open").ToList();
        //        result = EmpDetailList.ToDataSourceResult(request);

        //    }

        //    return Json(result);
        //}

        public IActionResult empAvlList_Read([DataSourceRequest] DataSourceRequest request)
        {
            //ProjectUserVM PV = busObj.GetProjectUsers();

            ////List<ProjectUser> u = _ps.GetProject();
            //DataSourceResult result = PV.ProjectUserList.ToDataSourceResult(request);


            //ProjectUserVM PV = busObj.GetProjectUsers();
            //IEnumerable<ProjectUser> projectUserList = PV.ProjectUserList;
            List<EmpAvailable> empAvlList = busObj.GetEmpAvailable().ToList();

            DataSourceResult result = empAvlList.ToDataSourceResult(request);

            return Json(result);
        }

        //[AcceptVerbs("Post")]
        //public IActionResult Project_Update([DataSourceRequest] DataSourceRequest request, PepperResourceException rec)
        //{
        //    if (rec != null && ModelState.IsValid)
        //    {
        //        busObj.SaveException(rec);
        //    }
        //    return Json(new[] { rec }.ToDataSourceResult(request, ModelState));
        //}

        //[AcceptVerbs("Post")]
        //public IActionResult Project_Update([DataSourceRequest] DataSourceRequest request, ProjectUserVM project)
        //{
        //    if (project != null && ModelState.IsValid)
        //    {
        //        PepperResourceException exceptionObj = new PepperResourceException();
        //        exceptionObj.EmpFirstName = project.ProjectUserList.FirstOrDefault().EmpFirstName;

        //        //busObj.SaveException(project);




        //    }
        //    return Json(new[] { project }.ToDataSourceResult(request, ModelState));
        //}

        //[AcceptVerbs("Post")]
        //public IActionResult Project_Create([DataSourceRequest] DataSourceRequest request, ProjectUserVM project)
        //{
        //    if (project != null && ModelState.IsValid)
        //    {
        //        busObj.CreateProject(project);
        //    }
        //    return Json(new[] { project }.ToDataSourceResult(request, ModelState));
        //}

        //[AcceptVerbs("Post")]
        //public IActionResult Project_Destroy([DataSourceRequest] DataSourceRequest request, ProjectUser project)
        //{
        //    if (project != null)
        //    {
        //        _ps.DestroyProject(project);
        //    }
        //    return Json(new[] { project }.ToDataSourceResult(request, ModelState));
        //}


        public ActionResult BasicUsage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
