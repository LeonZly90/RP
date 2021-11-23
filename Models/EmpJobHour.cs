using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcePlanning.Models
{
    public class EmpJobHour
    {
        // Total job hours  by Employee

        [DisplayName("Employee")]
        public string Employee { get; set; }

        public string RoleName { get; set; }

        public string EmpFirstName { get; set; }

        public string EmpLastName { get; set; }

        public string EmployeeNumber { get; set; }

        public string JobNumber { get; set; }

        [DisplayName("Job Name")]
        public string JobName { get; set; }

        public string Title { get; set; }

        public string MarketSector { get; set; }

        public string ResumeLink { get; set; }


        public double Total_NWHR { get; set; }
        public double Total_NBHR { get; set; }
        public double Total_DTHR { get; set; }
        public double Total_OTHR { get; set; }

        public double AvgProjectRangeHours { get; set; }

        public double AvgLastMonthHours { get; set; }

        public double AvgLastWeekHours { get; set; }

        public DateTime? ProjectStart { get; set; }

        public DateTime? ProjectEnd { get; set; }

        public string ExceptionDate { get; set; }



    }
}
