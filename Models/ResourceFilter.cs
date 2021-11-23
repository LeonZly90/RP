using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcePlanning.Models
{
    public class ResourceFilter
    {
        public List<string> GroupLeader { get; set; }
        public List<string> ProjectDirector { get; set; }
        public List<string> Employee { get; set; }
        public List<string> Title { get; set; }
        public List<string> JobNumber { get; set; }
        public List<string> JobName { get; set; }
        public List<string> JobStatus { get; set; }
    }
}
