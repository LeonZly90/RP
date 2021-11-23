using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ResourcePlanning.Services
{
    public interface IADService
    {
        public Task<SignInResult> ADPasswordSignInAsync(string userName, string password);
    }
}
