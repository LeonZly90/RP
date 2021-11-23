using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcePlanning.Services
{
    public interface IApplicationContext : IDisposable
    {
        IStoProc StoProc { get; }

        IEmpExperience EmpExperience { get; }
    }
}
