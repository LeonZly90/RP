using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ResourcePlanning.Models;

namespace ResourcePlanning.Services
{
    public interface IEmpExperience
    {
        List<PersonnelExperienceVM> GetEmpExperience(string empNo);

        List<PersonnelEducationVM> GetEmpEducation(string empNo);

        List<PersonnelCertificationVM> GetEmpCertification(string empNo);

        List<PersonnelMarketExperienceVM> GetEmpMarketExperience(string empNo);

    }
}


