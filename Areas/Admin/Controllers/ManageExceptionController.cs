using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ResourcePlanning.Models;
using ResourcePlanning.Services;
using Microsoft.AspNetCore.Authorization;

namespace ResourcePlanning.Areas.Admin.Controllers
{

    
    [Area("Admin")]
    [Authorize]
    public class ManageExceptionController : Controller
    {

        private BO _dbObj = new BO();


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult PR_Exception_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<PepperResourceException> exceptionList = _dbObj.GetExceptionList().ToList();
            DataSourceResult result = exceptionList.ToDataSourceResult(request);
            return Json(result);
        }

        [AcceptVerbs("Post")]
        public IActionResult PR_Exception_Update([DataSourceRequest] DataSourceRequest request, PepperResourceException rec)
        {
            if (rec != null && ModelState.IsValid)
            {
                _dbObj.SaveException(rec);
            }
            return Json(new[] { rec }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public IActionResult PR_Exception_Delete([DataSourceRequest] DataSourceRequest request, PepperResourceException rec)
        {
            if (rec != null && ModelState.IsValid)
            {
                _dbObj.RemoveException(rec);
            }
            return Json(new[] { rec }.ToDataSourceResult(request, ModelState));
        }

    }
}
