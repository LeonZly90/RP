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

namespace ResourcePlanning.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class NoteController : Controller
    {
        BO busObj = new BO();
        private readonly ILogger<NoteController> _logger;
        //private readonly ProjectService _ps;
        private readonly IApplicationContext _appContext;

        private ISession _session;
        public ISession Session { get { return _session; } }

        public NoteController(ILogger<NoteController> logger, IHttpContextAccessor httpContextAccessor, IApplicationContext appContext)
        {
            _logger = logger;
            _session = httpContextAccessor.HttpContext.Session;
            _appContext = appContext;
            //_ps = ps;
        }

        public JsonResult ServerFiltering_GetEmployees(string text)
        {

            string sCompany = _session.GetString(SD.Session_Company_Key);

            if (String.IsNullOrEmpty(sCompany))
            {
                sCompany = "0100"; // Default 
            }

            var emps = busObj.GetEmployeeDropdown(sCompany);
            if (!String.IsNullOrEmpty(text))            {
                emps = emps.Where(t => t.EmployeeName.Contains(text)).ToList();
            }

            return Json(emps.ToList());

        }

        public JsonResult ServerFiltering_GetJobs(string text)
        {

            string sCompany = _session.GetString(SD.Session_Company_Key);

            if (String.IsNullOrEmpty(sCompany))
            {
                sCompany = "0100"; // Default 
            }

            var jobs = busObj.GetJobDropdown(sCompany);
            if (!String.IsNullOrEmpty(text))
            {
                jobs = jobs.Where(t => t.JobName.Contains(text)).ToList();
            }
            return Json(jobs.ToList());

        }

        public IActionResult NoteUpdate(string NoteId)
        {

            var param = new DynamicParameters();
            param.Add("@Id", NoteId);

            var note = _appContext.StoProc.OneRecord<ProjectNoteVM>(SD.Stocproc_GetNoteDetail, param);

            return View(note);
        }

        public IActionResult NoteCreate()
        {
            return View();
        }


        [HttpPost]
        public IActionResult NoteUpdate(string NoteId, string Note)
        {
            //
            if (!String.IsNullOrEmpty(NoteId))
            {
                //UpdateNote(Convert.ToInt32(NoteId), Note);
                var param = new DynamicParameters();
                param.Add("@id", Convert.ToInt32(NoteId));
                param.Add("@Note", Note);
                _appContext.StoProc.Execute(SD.Stoproc_UpdateProjectNote, param);
            }
            //
            var param2 = new DynamicParameters();
            param2.Add("@Id", NoteId);
            var note = _appContext.StoProc.OneRecord<ProjectNoteVM>(SD.Stocproc_GetNoteDetail, param2);


            return View(note);
        }

        [HttpPost]
        public IActionResult NoteCreate(string empName_Combo, string jobName_Combo, string txtNote)
        {
            var projNote = busObj.GetEmpJobForNote(empName_Combo, jobName_Combo, txtNote);
            InsertNote(projNote);         

            ViewData["_Note"] = txtNote;

            return View();
        }

        private void InsertNote(ProjectNoteVM note)
        {
            //bool bSuccess = false;

            var param = new DynamicParameters();
            //param.Add("@id", note.Id);
            param.Add("@CompCode", note.CompCode);
            param.Add("@JobCode", note.JobCode);
            param.Add("@JobName", note.JobName);
            param.Add("@EmpNo", note.EmpNo);
            param.Add("@EmpFirstName", note.EmpFirstName);
            param.Add("@EmpLastName", note.EmpLastName);
            param.Add("@Note", note.Note);

            _appContext.StoProc.Execute(SD.Stoproc_InsertProjectNote, param);

        }
        public JsonResult SearchEmployee(string text)
        {

            var emps =  busObj.GetEmployeeDropdown("0100");

            return Json(emps.ToList());

        }

        public ActionResult Search_Panel()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EmployeeDetail(string empNumber)
        {

            if (empNumber is not null)
            {
                _session.SetString("_SessionKey_EmpNumber", empNumber);
            }
            else
            {
                _session.SetString("_SessionKey_EmpNumber", "");
            }
            return View();
        }

        public IActionResult EmployeeDetail_Read([DataSourceRequest] DataSourceRequest request)
        {
            string empNumber = _session.GetString("_SessionKey_EmpNumber");
            DataSourceResult result = null;
            if (!string.IsNullOrEmpty(empNumber))
            {
                List<EmpJobHour> EmpDetailList = busObj.GetEmpJobHours(empNumber, "Open").ToList();
                result = EmpDetailList.ToDataSourceResult(request);

            }

            return Json(result);
        }


        private List<ProjectNoteVM> GetProjectNote()
        {
            //private readonly IApplicationContext _appContext;
            var notes = _appContext.StoProc.List<ProjectNoteVM>(SD.Stoproc_GetProjectNote).ToList();

            return notes;
        }

  
        // Get Note List
        public IActionResult Note_Read([DataSourceRequest] DataSourceRequest request)
        {
    
            List<ProjectNoteVM> PV = GetProjectNote();
            DataSourceResult result = PV.ToDataSourceResult(request);

            return Json(result);
        }

        [HttpPost]
        public IActionResult Note_Destroy([DataSourceRequest] DataSourceRequest request, ProjectNoteVM rec)
        {
            if (rec != null && ModelState.IsValid)
            {
                int noteId = rec.Id;
                var param = new DynamicParameters();
                param.Add("@id", noteId);

                _appContext.StoProc.Execute(SD.Stoproc_DeleteProjectNote, param);          
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
