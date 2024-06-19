using Hotel.Interfaces;
using Hotel.Models;
using Hotel.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Studio.ViewModels.Account;

namespace Hotel.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, IEmailService _emailService) : Controller
    {
        public IActionResult Register()
        {
            var model = new RegisterVM();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (registerVM == null) return BadRequest();
            AppUser user = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
            };
            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            string body = @$"<a href=""www.instagram.com""DSSDA </a>";
            await _emailService.SendMailAsync(user.Email, "Verify", body, true);


            if (!result.Succeeded)
            {
                foreach (var errors in result.Errors)
                {
                    foreach (var item in ModelState)
                    {
                        ModelState.AddModelError("", errors.Description);
                    }
                }
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, Email = user.Email }, Request.Scheme);

            await _emailService.SendMailAsync(user.Email, "Email Confirmation", confirmationLink);


            return RedirectToAction(nameof(SuccesfullyRegistered), "Account");

        }

        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return View("Error");
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return View();
        }
        public IActionResult SuccesfullyRegistered()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            {
                var user = await _userManager.FindByNameAsync(vm.UsernameOrEmail) ??
                        await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not Found");
                    return View(vm);
                }
                var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(String.Empty, "Login is not enable please try again later");
                    return View(vm);
                }
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError("", "Please verify your Email");
                    return View(vm);

                }
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Wrong name or Password");
                    return View(vm);
                }
                return RedirectToAction(nameof(HomeController.Index), "Home");

            }
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");

        }
    }
}
