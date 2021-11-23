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
using Dapper;
using Microsoft.Extensions.Caching.Memory;

namespace ResourcePlanning.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class ProjectController : Controller
    {
        BO busObj = new BO();
        private readonly ILogger<ProjectController> _logger;
        //private readonly ProjectService _ps;

        private readonly IMemoryCache memoryCache;
        private readonly IApplicationContext _appContext;

        private ISession _session;
        public ISession Session { get { return _session; } }

        public ProjectController(IMemoryCache memoryCache, IApplicationContext appContext, IHttpContextAccessor httpContextAccessor)
        {
            //_logger = logger;
            _session = httpContextAccessor.HttpContext.Session;
            _appContext = appContext;
            //_ps = ps;
        }

        public ActionResult Search_Panel()
        {
            return View();
        }

        public IActionResult Index()
        {
            // Default : PCC
            //_session.SetString("_SessionKey_Company", "0100");        

            return View();
        }

        public string GetConstructionExperience(string empNo)
        {
            string s = "";
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);
            //
            var exp = _appContext.StoProc.List<PersonnelExperienceVM>(SD.Stoproc_GetEmpExperience, parameter).FirstOrDefault();

            if (exp != null)
            {
                s += "Years with Pepper: <b>" + 13 + "</b>." + " Years in industry: <b>" + exp.TimeInIndustry + "</b>";
            }
            


            return s;
        }

        public string GetEmpMarketExperience(string empNo)
        {
            string s = "";
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);
            var experience = _appContext.StoProc.List<PersonnelMarketExperienceVM>(SD.Stoproc_GetEmpMarketExperience, parameter).ToList();

            if (experience != null)
            {
                foreach (var item in experience)
                {
                    s += "<li class='list-group-item'>" + item.MarketExperience + "</li>";
                }
            }
            


            return s;
        }

        public string GetEmpCertifications(string empNo)
        {
            string s = "";
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);
            //
            var certs = _appContext.StoProc.List<PersonnelCertificationVM>(SD.Stoproc_GetEmpCertification, parameter).ToList();

            if (certs != null)
            {
                foreach (var item in certs)
                {
                    s += "<li class='list-group-item'>" + item.CerType + ": " + item.Certified + "</li>";
                }
            }        


            return s;
        }

        public string GetEmpEducation(string empNo)
        {
            string s = "";
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);

            var edu = _appContext.StoProc.List<PersonnelEducationVM>(SD.Stoproc_GetEmpEducation, parameter).ToList();

            if (edu != null)
            {
                foreach (var item in edu)
                {
                    s += "<li class='list-group-item'>" + item.EducationDegree + ": " + item.EducationMajor + "</li>";
                }
            }
            

            return s;
        }

        public IActionResult EmployeeDetail(string empNumber)
        {

            _session.SetString("_SessionKey_EmpNumber", empNumber);

            ViewData["_EmpMarketExp"] = GetEmpMarketExperience(empNumber);

            ViewData["_EmpEducation"] = GetEmpEducation(empNumber);

            ViewData["_EmpCertification"] = GetEmpCertifications(empNumber);

            ViewData["_EmpConstructionExp"] = GetConstructionExperience(empNumber);

            return View();
        }


        //[HttpPost]
        //public IActionResult PostCompany(string CompanyLocation)
        //{
        //    _session.SetString("_SessionKey_EmpNumber", CompanyLocation);

        //    return View();
        //}



        public IActionResult EmployeeDetail_Read([DataSourceRequest] DataSourceRequest request)
        {
            string empNumber = _session.GetString("_SessionKey_EmpNumber");
            DataSourceResult result=null;
            if (!string.IsNullOrEmpty(empNumber))
            {
                List<EmpJobHour> EmpDetailList = busObj.GetEmpJobHours(empNumber, "Open").ToList();
                result = EmpDetailList.ToDataSourceResult(request);

            }

            return Json(result);
        }

        public IActionResult Project_Read([DataSourceRequest] DataSourceRequest request)
        {
   
            string sCompany = _session.GetString(SD.Session_Company_Key);

            if (String.IsNullOrEmpty(sCompany))
            {
                sCompany = "0100"; // Default 
                _session.SetString(SD.Session_Company_Key, sCompany);
            }


            ProjectUserVM PV = busObj.GetProjectUsers(sCompany);
            IEnumerable<ProjectUser> projectUserList = PV.ProjectUserList;
            DataSourceResult result = projectUserList.ToDataSourceResult(request);           //

            
            


            return Json(result);
        }

        [AcceptVerbs("Post")]
        public IActionResult Project_Update([DataSourceRequest] DataSourceRequest request, ProjectUser rec)
        {
            if (rec != null && ModelState.IsValid)
            {
                //busObj.SaveException(rec);

                PepperResourceException ResException = new PepperResourceException();
                ResException.EmployeeNumber = rec.EmployeeNumber;
                ResException.JobNumber = rec.JobNumber;
                ResException.EmpFirstName = rec.EmpFirstName;
                ResException.EmpLastName = rec.EmpLastName;
                ResException.Employee = rec.Employee;
                ResException.ExceptionStart = rec.JobStart;
                ResException.ExceptionEnd = rec.JobEnd;
                ResException.GL_FirstName = rec.GLFirstName;
                ResException.GL_LastName = rec.GLLastName;
                ResException.GL_Name = rec.GroupLeader;

                busObj.SaveException(ResException);

                if (rec.Commitment != 100)
                {
                    EmployeeCommitmentException commitmentException = new EmployeeCommitmentException();
                    commitmentException.Commitment = rec.Commitment;
                    commitmentException.EmpNo = rec.EmployeeNumber;
                    commitmentException.JobCode = rec.JobNumber;
                    // Save Commitment
                    busObj.SaveCommitment(commitmentException);
                }

            }
            return Json(new[] { rec }.ToDataSourceResult(request, ModelState));
        }

       


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
