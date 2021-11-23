//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ResourcePlanning.Utility;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ResourcePlanning.Data;
//using ResourcePlanning.Models;
//using Microsoft.AspNetCore.Identity;


//namespace ResourcePlanning.Services
//{
//    public class ProjectService
//    {
//        private readonly ApplicationDbContext _db;

//        public ProjectService(ApplicationDbContext db)
//        {
//            _db = db;
//        }


//        //public List<Certification> GetCertifications()
//        //{
//        //    return _db.Certifications.OrderBy(c => c.CertificationName).ToList();
//        //}


//        public List<ProjectUser> GetProject()
//        {            
//            return _db.ProjectUser.ToList();
//        }

//        public void CreateProject(ProjectUser project)
//        {
//            _db.Add(project);
//            _db.SaveChanges();
//        }

//        public void UpdateProject(ProjectUser project)
//        {
//            _db.ProjectUser.Attach(project);
//            _db.Entry(project).State = EntityState.Modified;
//            _db.SaveChanges();
//        }

//        public void DestroyProject(ProjectUser project)
//        {
//            _db.ProjectUser.Remove(project);
//            _db.SaveChanges();
//        }








//    }
//}
