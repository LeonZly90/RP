using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcePlanning.Models
{
    public class ProjectUserVM
    {
        public IEnumerable<ProjectUser> ProjectUserList { get; set; }

        public IEnumerable<SelectListItem> GroupLeaderList { get; set; }

        public IEnumerable<SelectListItem> ProjectDirectorList { get; set; }

        public IEnumerable<SelectListItem> EmployeeList { get; set; }

        public IEnumerable<SelectListItem> JobList { get; set; }

        public IEnumerable<SelectListItem> JobNameList { get; set; }

        public IEnumerable<SelectListItem> JobStatusList { get; set; }

        public IEnumerable<SelectListItem> TitleList { get; set; }

        // Added to build months grid in view (default 24 months
        public int NumberOfMonths { get; set; } = 24;
        public List<string> GridMonths
        {
            get
            {
                List<string> monthsRange = new();

                DateTime rangeEnd = DateTime.Now.AddMonths(NumberOfMonths);

                for (DateTime d = DateTime.Now; d < rangeEnd; d = d.AddMonths(1))
                {
                    monthsRange.Add(d.ToString("MM-yy"));
                }

                return monthsRange;
            }
        }
    }
}
