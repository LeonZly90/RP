using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResourcePlanning.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using ResourcePlanning.Utility;
using ResourcePlanning.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Kendo.Mvc.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Dapper;

namespace ResourcePlanning.Services
{
    public class BO
    {
        
        //Employee Available
        public IEnumerable<EmpAvailable> GetEmpAvailable()
        {

            // Order By
            string sql = "select Emp_no EmpNumber, Emp_First_Name EmpFirstName, Emp_Last_Name EmpLastName, Emp_First_Name ||' '|| Emp_Last_Name EmpFullName, Emp_Comp_code EmpCompany, Emp_Dept_Code EmpDepartment, emp_job_title EmpTitle " +
            "FROM VWPEPPERRESPLANINGKEYPLAY_V3 " +
            " WHERE emp_comp_code = '0100'";

            var empAvailables = DataUtil<EmpAvailable>.GetList(new EmpAvailable(), sql);
            return empAvailables;
        }



        // JobStatusCode = Open : Open Jobs
        // JobStatusCode = Closed : Closed Jobs
        public IEnumerable<EmpJobHour> GetEmpJobHours(string empNumber, string JobStatusCode)
        {

            // Order By
            string sql = "SELECT " +
                            "EMPNO EmployeeNumber, " +
                            "EMP_FIRST_NAME EmpFirstName, " +
                            "EMP_LAST_NAME EmpLastName, " +
                            "EMP_FIRST_NAME || ' ' || EMP_LAST_NAME Employee, " +
                            "Role_name RoleName, " +
                            "EMP_JOB_TITLE Title, " +
                            "PMP_PROJ_CODE JobNumber, " +
                            "JOBNAME, " +
                            "MARKET_SECTOR MarketSector, " +
                            "NWHR Total_NWHR, " +
                            "OT Total_OTHR, " +
                            "DT Total_DTHR, " +
                            "NBHR Total_NBHR, " +
                            "Proj_Start_Date ProjectStart, " +
                            "Proj_End_Date ProjectEnd, " +
                            "avgNormalHoursProjDateRange AvgProjectRangeHours, " +
                            "avgNormalHoursLast30Days AvgLastMonthHours, " +
                            "avgNormalHoursLast7Days AvgLastWeekHours, " +
                            "Exception_date ExceptionDate, " +
                            "'/illinois/marketingresumes/IL/' || EMP_LAST_NAME || ', ' || EMP_FIRST_NAME ||'.pdf' as ResumeLink " +
                            "FROM VWPEPPEREMPTMSHTJOBHRS " +
                            "WHERE Proj_Start_Date is not null AND EMPNO ='" + empNumber + "' ";
            if (JobStatusCode == "Open")
            {
                sql += " AND Job_Status_Code <> 'C' ";
            }
            if (JobStatusCode == "Closed")
            {
                sql += " AND Job_Status_Code = 'C' ";
            }

            sql += "order by avgnormalhourslast30days desc";

            var jobHours = DataUtil<EmpJobHour>.GetList(new EmpJobHour(), sql);

            return jobHours;
        }

        public IEnumerable<EmpJobMarketHour> GetEmployeeByExperienceHours(string roleCode, string marketSector)
        {

            // Need Project Start and End Date
            string sql = "SELECT " +
                            "EMPNO EmployeeNumber, " +
                            "EMP_FIRST_NAME || ' ' || EMP_LAST_NAME Employee, " +
                            "EMP_JOB_TITLE title, " +
                            "MARKET_SECTOR MarketSector, " +
                            "sum(NWHR) Total_NWHR, " +
                            "sum(OT) Total_OTHR, " +
                            "sum(DT) Total_DTHR, " +
                            "sum(NBHR) Total_NBHR " +
                            "FROM VWPEPPEREMPTMSHTJOBHRS " +
                            "WHERE Market_Sector ='" + marketSector + "' " +
                            "AND Emp_Job_title = '" + roleCode + "' " + // CHANGED this to RoleCode
                            "group by EMPNO, " +
                            "EMP_FIRST_NAME || ' ' || EMP_LAST_NAME , " +
                            "EMP_JOB_TITLE, " +
                            "MARKET_SECTOR " +
                            "order by SUM(NWHR) desc ";



            var empList = DataUtil<EmpJobMarketHour>.GetList(new EmpJobMarketHour(), sql);

            return empList;
        }


        public List<EmployeeListVM> GetEmployeeDropdown(string Company)
        {
            string sql = "select distinct empno as EmpNumber, emp_first_name || ' ' || emp_last_name as EmployeeName  from VWPEPPERRESPLANING_test where PMP_Comp_Code = '" + Company + "'";

            var emplist = DataUtil<EmployeeListVM>.GetList(new EmployeeListVM(), sql).ToList();

            return emplist;
        }

        public List<JobListVM> GetJobDropdown(string Company)
        {
            string sql = "select distinct Job_code JobNumber, JobName  from VWPEPPERRESPLANING_test where PMP_Comp_Code = '" + Company + "'";

            var joblist = DataUtil<JobListVM>.GetList(new JobListVM(), sql).ToList();

            return joblist;
        }

        public EmployeeListVM GetEmployeeByEmpCode(string empNumber)
        {
            string sql = "select distinct EmpNo EmpNumber, Emp_First_Name EmpFirstName, Emp_Last_Name EmpLastName, Emp_First_Name || ' ' || Emp_Last_Name as EmployeeName " +
                            " from VWPEPPERRESPLANING_test where EmpNO='" + empNumber + "'";

            var emp = DataUtil<EmployeeListVM>.GetList(new EmployeeListVM(), sql).FirstOrDefault();

            return emp;

        }

        public JobListVM GetJobByJobCode(string jobNumber)
        {
            string sql = "select distinct Job_code JobNumber, Pmp_comp_code JobCompany,JobName from VWPEPPERRESPLANING_test where Job_Code='" + jobNumber + "'";

            var job = DataUtil<JobListVM>.GetList(new JobListVM(), sql).FirstOrDefault();

            return job;

        }

        public ProjectNoteVM GetEmpJobForNote(string empNumber, string jobNumber, string note)
        {
            //string sql = "select pmp_comp_code CompCode, empno, Emp_First_Name EmpFirstName, Emp_Last_Name EmpLastname, JobName, Job_code JobCode from VWPEPPERRESPLANING_test where EmpNo ='" + empNumber + "' AND Job_code='" + jobNumber + "'";
            //var projectnote = DataUtil<ProjectNoteVM>.GetList(new ProjectNoteVM(), sql).FirstOrDefault();
            string Company = "";
            string JobName = "";
            string EmpFirstName = "";
            string EmpLastName = "";

            var emp = GetEmployeeByEmpCode(empNumber);
            var job = GetJobByJobCode(jobNumber);

            if (emp != null)
            {
                EmpFirstName = emp.EmpFirstName;
                EmpLastName = emp.EmpLastName;
            }
            else if (empNumber != null)
            {
                string[] words = empNumber.Split(' ');
                if (words.Length == 1)
                    EmpFirstName = empNumber;
                else
                {
                    EmpFirstName = words[0];
                    EmpLastName = words[words.Length - 1];
                }
            }

            
            if (job != null)
            {
                Company = job.JobCompany;
                JobName = job.JobName;
            }
            else
            {
                Company = "";
                if (jobNumber == null)
                    JobName = "";
                else
                {
                    JobName = jobNumber;
                }
            }

            ProjectNoteVM n = new ProjectNoteVM();
            n.CompCode = Company;
            n.EmpFirstName = EmpFirstName;
            n.EmpLastName = EmpLastName;
            n.EmpNo = empNumber;
            n.JobCode = jobNumber;
            n.Note = note;
            n.JobName = JobName;
            //

            return n;


        }

        public ProjectUserVM GetProjectUsers(string company)
        {

            string sql = "SELECT proj_bumgl_name GroupLeader, proj_dr_name ProjectDirector,emp_first_name EmpFirstName, emp_last_name EmpLastName, emp_first_name || ' ' || emp_last_name Employee, EmpNo EmployeeNumber,PMP_Comp_code Company, Role_code RoleCode, Role_Name RoleName, " +
                    "Market_Sector MarketSector, Exception_Date HasException, emp_title Title,job_code JobNumber, Stagecode JobStatus,chance_closing_pct ClosePercent, jobname JobName, Revenue, OMOP_OPPO_ORASEQ OMKeyPlayerOraseq, " +
                    "proj_start_date JobStart, proj_end_date JobEnd, Proj_bumgl_firstname GLFirstName, Proj_bumgl_LastName GLLastName, Commitment_Flag Has_Commitment, COMMITMENT " +
                    "FROM VWPEPPERRESPLANING_test " +
                    "WHERE (NVL(ROLE_CODE,'SUPPERNEEDED') IN ('MNGR','PM','SUPT','SUPER','PAST','PBM','SUPPERNEEDED')) AND PMP_Comp_code='" + company + "'";

            var projUsers = DataUtil<ProjectUser>.GetList(new ProjectUser(), sql);

            // Groupleader
            var GLList = projUsers
                .Select(u => new { u.GroupLeader })
                .Where(u => u.GroupLeader.HasValue())
                .Distinct()
                .OrderBy(u => u.GroupLeader);
            // PD
            var PDList = projUsers
                .Select(u => new { u.ProjectDirector })
                .Where(u => u.ProjectDirector.HasValue())
                .Distinct()
                .OrderBy(u => u.ProjectDirector);
            // Employee
            var EmpList = projUsers
                .Select(u => new { u.Employee })
                .Where(u => u.Employee.HasValue())
                .Distinct()
                .OrderBy(u => u.Employee);
            // Job
            var JobList = projUsers
                .Select(u => new { u.JobNumber })
                .Where(u => u.JobNumber.HasValue())
                .Distinct()
                .OrderBy(u => u.JobNumber);
            // Job Names
            var JobNameList = projUsers
                .Select(u => new { u.JobName })
                .Where(u => u.JobName.HasValue())
                .Distinct()
                .OrderBy(u => u.JobName);

            // JobStatus
            var JobStatusList = projUsers
                .Select(u => new { u.JobStatus })
                .Where(u => u.JobStatus.HasValue())
                .Distinct()
                .OrderBy(u => u.JobStatus);
            // Title
            var TitleList = projUsers
                .Select(u => new { u.Title })
                .Where(u => u.Title.HasValue())
                .Distinct()
                .OrderBy(u => u.Title);

            ProjectUserVM projuserVM = new ProjectUserVM()
            {
                ProjectUserList = projUsers,
                GroupLeaderList = GLList.Select(i => new SelectListItem
                {
                    Text = i.GroupLeader,
                    Value = i.GroupLeader
                }),
                ProjectDirectorList = PDList.Select(i => new SelectListItem
                {
                    Text = i.ProjectDirector,
                    Value = i.ProjectDirector
                }),
                EmployeeList = EmpList.Select(i => new SelectListItem
                {
                    Text = i.Employee,
                    Value = i.Employee
                }),
                JobList = JobList.Select(i => new SelectListItem
                {
                    Text = i.JobNumber,
                    Value = i.JobNumber
                }),
                JobNameList = JobNameList.Select(i => new SelectListItem
                {
                    Text = i.JobName,
                    Value = i.JobName
                }),
                JobStatusList = JobStatusList.Select(i => new SelectListItem
                {
                    Text = i.JobStatus,
                    Value = i.JobStatus
                }),
                TitleList = TitleList.Select(i => new SelectListItem
                {
                    Text = i.Title,
                    Value = i.Title
                })
            };
            //
            return projuserVM;
        }

        public int? SaveCommitment(EmployeeCommitmentException c)
        {
            int? i = 0;
            string sql = "";
            string sqlCheck = "SELECT EMP_NO, JOB_CODE, COMMITMENT FROM da.PEPPER_RES_PLANNING_COMMITMENT WHERE EMP_NO='" + c.EmpNo + "' AND JOB_CODE='" + c.JobCode + "'";

            var res = DataUtil<PepperResourceException>.GetList(new PepperResourceException(), sqlCheck);

            if (!res.Any())
            {
                // Insert new
                sql = "INSERT INTO PEPPER_RES_PLANNING_COMMITMENT(Emp_no, Job_Code, Commitment) VALUES('" + c.EmpNo + "','" + c.JobCode + "'," + c.Commitment + ")";
            } 
            else
            {
                // Update

                if (c.Commitment != 100)
                {
                    // Update
                    sql = "UPDATE PEPPER_RES_PLANNING_COMMITMENT SET Commitment=" + c.Commitment + " WHERE Emp_no='" + c.EmpNo + "' AND Job_Code='" + c.JobCode + "'";
                } 
                else
                {
                    // Delete
                    sql = "DELETE PEPPER_RES_PLANNING_COMMITMENT  WHERE Emp_no='" + c.EmpNo + "' AND Job_Code='" + c.JobCode + "'";
                }

            }


            i = DataUtil<EmployeeCommitmentException>.ExecuteAction(sql);



            return i;
        }

        public int? SaveException(PepperResourceException p)
        {
            int? i = 0;

            // Removing single quotes from name props to avoid SQL Exception
            PepperResourceException cleanP = new()
            {
                EmployeeNumber = p.EmployeeNumber,
                EmpFirstName = p.EmpFirstName.Replace("'", "''"),
                EmpLastName = p.EmpLastName.Replace("'", "''"),
                JobNumber = p.JobNumber,
                ExceptionStart = p.ExceptionStart,
                ExceptionEnd = p.ExceptionEnd,
                GL_FirstName = p.GL_FirstName == null ? "" : p.GL_FirstName.Replace("'", "''"),
                GL_LastName = p.GL_LastName == null ? "" : p.GL_LastName.Replace("'", "''")
            };

            string sqlCheck = "SELECT EMP_NO EmployeeNumber, job_code JobNumber, EMP_First_Name EmpFirstname, Emp_Last_Name EmpLastName, Exception_Start_date ExceptionStart, Exception_End_Date ExceptionEnd, " +
                            "Grp_leader_Emp_No GL_EmpNo, Grp_Leader_First_Name GL_FirstName, Grp_Leader_Last_Name GL_LastName " +
                            "FROM DA.PEPPER_RES_PLANNING_EXP_TEST " +
                            "WHERE EMP_NO='" + cleanP.EmployeeNumber + "' AND job_code='" + cleanP.JobNumber + "'";

            string sqlUpdateInsert = "";

            var res = DataUtil<PepperResourceException>.GetList(new PepperResourceException(), sqlCheck);

            if (!res.Any())
            {
                // Insert
                sqlUpdateInsert = "INSERT INTO DA.PEPPER_RES_PLANNING_EXP_TEST(emp_no, EMP_First_Name, Emp_Last_Name, job_code, exception_start_date, exception_end_date, grp_leader_first_Name, grp_leader_last_name) " +
                            "VALUES('" + cleanP.EmployeeNumber + "','" + cleanP.EmpFirstName + "','" + cleanP.EmpLastName + "','" + cleanP.JobNumber + "'," + "TO_DATE('" + cleanP.ExceptionStart.ToShortDateString() + "','MM/DD/YYYY')" + "," + "TO_DATE('" + cleanP.ExceptionEnd.ToShortDateString() + "','MM/DD/YYYY')" + ",'" + cleanP.GL_FirstName + "','" + cleanP.GL_LastName + "')";

            }
            else
            {
                // Update
                sqlUpdateInsert = "UPDATE DA.PEPPER_RES_PLANNING_EXP_TEST SET exception_start_date=" + "TO_DATE('" + cleanP.ExceptionStart.ToShortDateString() + "','MM/DD/YYYY')" + ", exception_end_date=" + "TO_DATE('" + cleanP.ExceptionEnd.ToShortDateString() + "','MM/DD/YYYY')" +
                            " WHERE Emp_no='" + cleanP.EmployeeNumber + "' AND job_code='" + cleanP.JobNumber + "'";
            }

            i = DataUtil<PepperResourceException>.ExecuteAction(sqlUpdateInsert);

            return i;
        }

        public int? RemoveException(PepperResourceException p)
        {
            int? i = 0;

            string sql = "DELETE Pepper_res_Planning_Exception where emp_no = '" + p.EmployeeNumber + "' AND job_code='" + p.JobNumber + "'";

            i = DataUtil<PepperResourceException>.ExecuteAction(sql);

            return i;
        }



        public IEnumerable<PepperResourceException> GetExceptionList()
        {

            // Need Project Start and End Date
            string sql = "SELECT emp_no EmployeeNumber,job_code JobNumber, EMP_First_Name EmpFirstName, Emp_Last_Name EmpLastName, EMP_FIRST_NAME || ' ' || EMP_LAST_NAME Employee, " +
                        "exception_start_date ExceptionStart, exception_end_date ExceptionEnd, grp_leader_emp_no GL_EmpNo, grp_leader_first_Name GL_FirstName, grp_leader_last_name GL_LastName, " +
                        "grp_leader_first_Name || ' ' || grp_leader_last_name GL_Name " +
                        "FROM Pepper_res_Planning_Exception";

            var exceptionList = DataUtil<PepperResourceException>.GetList(new PepperResourceException(), sql);

            return exceptionList;
        }



        public RPUser GetCmiCUser(string email)
        {
            RPUser u = new RPUser();

            string sql = "SELECT e.emp_comp_code UserCompany,c.comp_name, e.emp_first_name FirstName, e.emp_last_name LastName, e.emp_first_name || ' ' || e.emp_last_name DisplayName, e.emp_email_address Email FROM da.pyemployee e " +
                        " JOIN da.Company c on e.emp_comp_code = c.comp_code " +
                        " WHERE Lower(e.emp_email_address) = '" + email.ToLower() + "'";

            var cmicuser = DataUtil<RPUser>.SingleOrDefault(new RPUser(), sql);


            return cmicuser;


        }

        


    }
}
