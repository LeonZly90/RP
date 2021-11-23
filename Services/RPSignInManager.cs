using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ResourcePlanning.Models;
using ResourcePlanning.Utility;

namespace ResourcePlanning.Services
{
    public class RPSignInManager : SignInManager<RPUser>
    {
        private readonly IADService _adService;

        public RPSignInManager(
            UserManager<RPUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<RPUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<RPUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<RPUser> confirmation,
            IADService adService)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            _adService = adService;
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {

            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInResult.Failed;
            }
            else if (user.IsDomainUser)
            {
                var attempt = await _adService.ADPasswordSignInAsync(userName, password); // Using custom IADService
                return attempt.Succeeded
                    ? await SignInOrTwoFactorAsync(user, isPersistent)
                    : attempt;
            }
            else
            {
                return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
            }
        }

        public override async Task<SignInResult> PasswordSignInAsync(RPUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var attempt = await CheckPasswordSignInAsync(user, password, lockoutOnFailure);
            return attempt.Succeeded
                ? await SignInOrTwoFactorAsync(user, isPersistent)
                : attempt;
        }

        public override async Task SignOutAsync()
        {
            await Context.SignOutAsync(IdentityConstants.ApplicationScheme);
            await Context.SignOutAsync(IdentityConstants.ExternalScheme);
            await Context.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
        }

    }
}
