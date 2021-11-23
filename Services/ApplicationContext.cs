using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResourcePlanning.Data;

namespace ResourcePlanning.Services
{
    public class ApplicationContext : IApplicationContext
    {

        private readonly ApplicationDbContext _db;

        public ApplicationContext(ApplicationDbContext db)
        {
            _db = db;
            StoProc = new StoProc(_db);
            EmpExperience = new EmpExperience(_db);
        }
        public IStoProc StoProc { get; private set; }

        public IEmpExperience EmpExperience { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }




    }
}
