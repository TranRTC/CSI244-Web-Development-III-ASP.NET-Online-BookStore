using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {


        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Action Method to View User Roles
        public IActionResult ViewUserRoles()
        {
            var users = _userManager.Users.ToList();
            var userRolesList = new List<UserRole>();

            foreach (var user in users)
            {
                var roles = _userManager.GetRolesAsync(user).Result;

                var userRole = new UserRole
                {
                    UserId = user.Id,
                    Email = user.Email,
                    AssignedRoles = roles.ToList(),
                    AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList()
                };

                userRolesList.Add(userRole);
            }

            return View(userRolesList);
        }

        // Action Method to Assign New Role to User
        [HttpPost]
        public IActionResult AssignRole(string userId, string roleName)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user != null && _roleManager.RoleExistsAsync(roleName).Result)
            {
                var result = _userManager.AddToRoleAsync(user, roleName).Result;
                if (result.Succeeded)
                {
                    // Role assigned successfully
                }
                else
                {
                    // Handle errors
                }
            }

            return RedirectToAction("ViewUserRoles");
        }



       
    }
}
