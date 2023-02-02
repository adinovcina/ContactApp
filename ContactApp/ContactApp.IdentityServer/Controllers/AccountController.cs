using ContactApp.Data.EF.Identity;
using ContactApp.IdentityServer.Interfaces;
using ContactApp.IdentityServer.Requests;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.IdentityServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
        private static readonly string InvalidCredentialsErrorMessage = "Invalid username or password";

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IEventService events,
            IConfiguration configuration, IAccountService accountService)
        {
            _signInManager = signInManager;
            _interaction = interaction;
            _events = events;
            _configuration = configuration;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _accountService.CreateUserAsync(model.Email.Trim(), model.Password);

                    // Redirect to login
                    return Redirect(_configuration.GetValue<string>("ContactAppUI"));
                }

                return View(model);
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var vm = new LoginRequest()
            {
                ReturnUrl = returnUrl
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            // check if we are in the context of an authorization request
             var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // user might have clicked on a malicious link - should be logged
            if (context == null)
                throw new Exception("Invalid return URL");

            if (User.Identity?.IsAuthenticated == true)
            {
                return Redirect(model.ReturnUrl);
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email.Trim(), model.Password.Trim(), true, lockoutOnFailure: true);

                if (result.Succeeded)
                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Redirect(model.ReturnUrl);

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Email, "invalid credentials", clientId: context?.Client?.ClientId));
                ModelState.AddModelError("InvalidCredentials", InvalidCredentialsErrorMessage);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await DeleteAuthenticationCookie();

            var logout = await _interaction.GetLogoutContextAsync(logoutId);
            if (logout?.PostLogoutRedirectUri != null)
            {
                return Redirect(logout.PostLogoutRedirectUri);
            }

            // fallback to use redirect url from configuration
            var postLogoutUris = _configuration.GetSection("PostLogoutRedirectUris").Get<List<string>>();
            return Redirect(postLogoutUris.FirstOrDefault() ?? string.Empty);
        }

        private async Task DeleteAuthenticationCookie()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();
            }
        }

        private IActionResult ExceptionResult(Exception e)
        {
            if (e is ArgumentException || e is InvalidOperationException)
            {
                return BadRequest(e.Message);
            }
            return Problem("Something went wrong");
        }
    }
}