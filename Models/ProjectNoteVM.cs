using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcePlanning.Models
{
    public class ProjectNoteVM
    {
        //[ScaffoldColumn(false)]
        public int Id { get; set; }

        //[HiddenInput(DisplayValue = false)]
        public string EmpNo { get; set; }

        public string CompCode { get; set; }

        public string JobCode { get; set; }

        public string JobName { get; set; }

        public string EmpFirstName { get; set; }

        public string EmpLastName { get; set; }

        public string EmpName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Note { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = @"{0:MM/dd/yyyy}")]
        public DateTime NoteDate { get; set; }



    }
}
