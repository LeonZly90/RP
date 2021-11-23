using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResourcePlanning.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourcePlanning.Models
{
    public class ProjectUser
    {
        public string OMKeyPlayerOraseq { get; set; }

        [DisplayName("Business Unit Leader")]
        public string GroupLeader { get; set; }

        public string GLFirstName { get; set; }
        public string GLLastName { get; set; }

        [DisplayName("PD/VP")]
        public string ProjectDirector { get; set; }

        [DisplayName("Employee")]
        public string Employee { get; set; }

        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }

        //[Key]
        public string EmployeeNumber { get; set; }

        public string Company { get; set; }

        public string RoleCode { get; set; }

        public string RoleName { get; set; }

        public string Title { get; set; }

        public string MarketSector { get; set; }

        public string JobNumber { get; set; }

        [DisplayName("Job Name")]
        public string JobName { get; set; }

        [DisplayName("Status")]
        public string JobStatus { get; set; }

        [DisplayName("Close %")]
        public string ClosePercent { get; set; }

        [DisplayName("Job Start")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime JobStart { get; set; }

        [DisplayName("Job End")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime JobEnd { get; set; }

        public double Revenue { get; set; }

        public string HasException { get; set; }

        // Added to color view grid blocks
        public List<string> MonthsBooked
        {
            get
            {
                List<string> monthsBooked = new();
                for (DateTime date = JobStart; date < JobEnd; date = date.AddMonths(1))
                {
                    monthsBooked.Add(date.ToString("yyyy-M"));
                }
                return monthsBooked;
            }
        }

        // Links... Added a second or two to the view's load time.
        public string KeyPlayerLink
        {
            get { return SD.Link_KeyPlayer + OMKeyPlayerOraseq; }
        }

        public string ResumeLink { get; set; }

        public int Commitment { get; set; }









    }
}
