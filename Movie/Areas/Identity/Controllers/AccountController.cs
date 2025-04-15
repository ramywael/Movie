using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Movie.Models;
using Movie.Models.ViewModels;

namespace Movie.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> _userManager,RoleManager<IdentityRole> roleManager,SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = _userManager;
            this._roleManager = roleManager;
            this._signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Cinema"));
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }
            return View(new RegisterVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = registerVm.UserName,
                    Email = registerVm.Email,
                };

                var result = await _userManager.CreateAsync(appUser, registerVm.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(appUser, false);
                    await _userManager.AddToRoleAsync(appUser,"Customer");
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }
            return View(registerVm);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByEmailAsync(loginVm.Email);

                if (appUser != null)
                {
                    var result = await _userManager.CheckPasswordAsync(appUser, loginVm.Password);
                    if (result)
                    {
                        await _signInManager.SignInAsync(appUser, loginVm.RememberMe);
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Cannot Found The Email");
                        ModelState.AddModelError("Password", "Donot Match The Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Cannot Found The Email");
                    ModelState.AddModelError("Password", "Donot Match The Password");

                }

            }
            return View(loginVm);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Sorry Something is Wrong");
            }

            var profileInfo = new ProfileVm
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email
            };
            return View(profileInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileVm profileVm)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.GetUserAsync(User);
                if (appUser == null)
                {
                    ModelState.AddModelError("", "Sorry Something is wrong");
                    return View(profileVm);
                }

                if (appUser.Email != profileVm.Email)
                {
                    var emailToken = await _userManager.GenerateChangeEmailTokenAsync(appUser, profileVm.Email);
                    var result = await _userManager.ChangeEmailAsync(appUser, profileVm.Email, emailToken);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(profileVm);
                    }

                }


                if (appUser.UserName != profileVm.UserName)
                {
                    appUser.UserName = profileVm.UserName;
                    var result = await _userManager.UpdateAsync(appUser);
                    if (!result.Succeeded)
                    {
                        foreach(var error in result.Errors)
                        {
                            ModelState.AddModelError("",error.Description);
                        }
                        return View(profileVm);
                    }



                }

                if (!string.IsNullOrEmpty(profileVm.CurrentPassword) && !string.IsNullOrEmpty(profileVm.NewPassword))
                {
                    var result = await _userManager.ChangePasswordAsync(appUser, profileVm.CurrentPassword, profileVm.NewPassword);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(profileVm);
                    }

                }
                await _signInManager.RefreshSignInAsync(appUser);
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            return View(profileVm);
        }
    }
}
