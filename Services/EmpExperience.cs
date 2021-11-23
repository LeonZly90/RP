using ResourcePlanning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ResourcePlanning.Data;
using ResourcePlanning.Utility;

namespace ResourcePlanning.Services
{
    public class EmpExperience : IEmpExperience
    {

        private readonly ApplicationDbContext _db;
        private readonly IApplicationContext _appContext;
        

        public EmpExperience(ApplicationDbContext db)
        {
            _db = db;
            
        }


        public List<PersonnelCertificationVM> GetEmpCertification(string empNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);

            var e = _appContext.StoProc.List<PersonnelCertificationVM>(SD.Stoproc_GetEmpCertification, parameter);

            return e.ToList();
        }

        public List<PersonnelEducationVM> GetEmpEducation(string empNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);

            var e = _appContext.StoProc.List<PersonnelEducationVM>(SD.Stoproc_GetEmpEducation, parameter);

            return e.ToList();
        }

        public List<PersonnelExperienceVM> GetEmpExperience(string empNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);

            var e = _appContext.StoProc.List<PersonnelExperienceVM>(SD.Stoproc_GetEmpExperience, parameter);

            return e.ToList();
        }

        public List<PersonnelMarketExperienceVM> GetEmpMarketExperience(string empNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@empNo", empNo);

            var e = _appContext.StoProc.List<PersonnelMarketExperienceVM>(SD.Stoproc_GetEmpMarketExperience, parameter);

            return e.ToList();
        }
    }
}
