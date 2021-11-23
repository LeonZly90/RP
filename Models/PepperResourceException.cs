using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcePlanning.Models
{
    public class PepperResourceException
    {

        [Required]
        public string EmployeeNumber { get; set; }


        [Required]
        public string JobNumber { get; set; }

        [DisplayName("FirstName")]
        public string EmpFirstName { get; set; }

        [DisplayName("LastName")]
        public string EmpLastName { get; set; }

        [DisplayName("Name")]
        public string Employee { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime ExceptionStart { get; set; }

        //public string ExceptionStart { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime ExceptionEnd { get; set; }

        //public string ExceptionEnd { get; set; }


        //public string GL_EmpNo { get; set; }

        public string GL_FirstName { get; set; }

        public string GL_LastName { get; set; }

        [DisplayName("GroupLeader")]
        public string GL_Name { get; set; }

        public int Commitment { get; set; }

    }
}
