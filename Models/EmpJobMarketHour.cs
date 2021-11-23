using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcePlanning.Models
{
    public class EmpJobMarketHour
    {

        // Experience hours by Market sector

        [DisplayName("Employee")]
        public string Employee { get; set; }

        public string EmployeeNumber { get; set; }       

        public string Title { get; set; }

        public string MartketSector { get; set; }

        public int Total_NWHR { get; set; }
        public int Total_NBHR { get; set; }
        public int Total_DTHR { get; set; }
        public int Total_OTHR { get; set; }
    }
}
