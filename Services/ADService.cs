using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using ResourcePlanning.Models;
using ResourcePlanning.Utility;
using Microsoft.AspNetCore.Authentication;

namespace ResourcePlanning.Services
{
    public enum Locations
    {
        Barrington,
        Chicago,
        Indiana,
        Ohio
    }
    public class ADService : IADService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<RPUser> _userMan;

        private readonly string _company;
        private readonly string _ldapDomain;
        private readonly string _ldapUserName;
        private readonly string _ldapPassword;
        private readonly string _barrington;
        private readonly string _chicago;
        private readonly string _indiana;
        private readonly string _ohio;
        private readonly string _global;

        public ADService(IConfiguration configuration)
        {
            _configuration = configuration;
            //_userMan = userMan;
            _company = _configuration.GetSection("Company:Name").Value;
            _ldapDomain = _configuration.GetSection("LDAP:Domain").Value;
            _ldapUserName = _configuration.GetSection("LDAP:UserName").Value;
            _ldapPassword = _configuration.GetSection("LDAP:Password").Value;
            _barrington = _configuration.GetSection("LDAP:Barrington").Value;
            _chicago = _configuration.GetSection("LDAP:Chicago").Value;
            _indiana = _configuration.GetSection("LDAP:Indiana").Value;
            _ohio = _configuration.GetSection("LDAP:Ohio").Value;
            _global = _configuration.GetSection("LDAP:Global").Value;
        }
        public async Task<SignInResult> ADPasswordSignInAsync(string userName, string password)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            using var context = new PrincipalContext(ContextType.Domain, _ldapDomain, userName, password);
#pragma warning restore CA1416 // Validate platform compatibility

#pragma warning disable CA1416 // Validate platform compatibility
            if (context.ValidateCredentials(userName, password))
#pragma warning restore CA1416 // Validate platform compatibility
            {
                return await Task.FromResult(SignInResult.Success);
            }
            else
            {
                return await Task.FromResult(SignInResult.Failed);
            }
        }


    }
}
