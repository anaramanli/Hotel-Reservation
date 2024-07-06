using Hotel.Models;
using Hotel.ViewModels.UserRole;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserRoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/UserRole/ManageRoles
        [HttpGet]
        public async Task<IActionResult> ManageRoles()
        {
            var users = _userManager.Users.ToList();
            var model = new List<UserRoleVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userRolesVM = new UserRoleVM
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = roles.ToList(),
                    SelectedRole = roles.FirstOrDefault(),
                    AvailableRoles = _roleManager.Roles.Select(r => new SelectListItem
                    {
                        Value = r.Name,
                        Text = r.Name
                    }).ToList()
                };

                model.Add(userRolesVM);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoles(Dictionary<string, string> userRoles)
        {
            foreach (var userRole in userRoles)
            {
                var userId = userRole.Key;
                var selectedRole = userRole.Value;

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    continue;
                }

                var existingRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, existingRoles);

                if (!string.IsNullOrEmpty(selectedRole))
                {
                    var roleExists = await _roleManager.RoleExistsAsync(selectedRole);
                    if (roleExists)
                    {
                        await _userManager.AddToRoleAsync(user, selectedRole);
                    }
                }
            }

            return RedirectToAction(nameof(ManageRoles));
        }
    }
}
