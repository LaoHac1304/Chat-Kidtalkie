using ChatKid.DataLayer.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.DataLayer.Entities
{
    public class KidtakieContextInitialiser
    {
        private readonly ILogger<KidtakieContextInitialiser> _logger;
        private readonly KidtalkieContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public KidtakieContextInitialiser(ILogger<KidtakieContextInitialiser> logger, KidtalkieContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            // Roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole(UserRoles.Parent),
                new IdentityRole(UserRoles.Children),
                new IdentityRole(UserRoles.Expert),
                new IdentityRole(UserRoles.Admin),
            };

            foreach (var role in roles)
            {

                if (_roleManager.Roles.All(r => r.Name != role.Name))
                {
                    await _roleManager.CreateAsync(role);
                }
            }

            // Manager
            var administrator = new ApplicationUser
            {
                UserName = "admin@kitalkie.com",
                Email = "admin@kitalkie.com",
            };

            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await _userManager.CreateAsync(administrator, "kidtalkie@2023");

                var administratorRole = roles.FirstOrDefault(r => r.Name == UserRoles.Admin);
                if (administratorRole != null)
                {
                    await _userManager.AddToRoleAsync(administrator, administratorRole.Name);
                }
            }

            // Expert
            var expert = new ApplicationUser
            {
                UserName = "expert01@kidtalkie.com",
                Email = "expert01@kidtalkie.com",
            };

            if (_userManager.Users.All(u => u.UserName != expert.UserName))
            {
                await _userManager.CreateAsync(expert, "kidtalkie@2023");

                var expertRole = roles.FirstOrDefault(r => r.Name == UserRoles.Expert);
                if (expertRole != null)
                {
                    await _userManager.AddToRoleAsync(expert, expertRole.Name);
                }
            }

            // Parent
            var parent = new ApplicationUser
            {
                UserName = "parent01@kidtalkie.com",
                Email = "parent01@kidtalkie.com",
            };

            if (_userManager.Users.All(u => u.UserName != parent.UserName))
            {
                await _userManager.CreateAsync(parent, "kidtalkie@2023");

                var parentRole = roles.FirstOrDefault(r => r.Name == UserRoles.Parent);
                if (parentRole != null)
                {
                    await _userManager.AddToRoleAsync(parent, parentRole.Name);
                }
            }

            /*if (!_context.TodoLists.Any())
            {
                _context.TodoLists.Add(new TodoList
                {
                    Title = "Todo List",
                    Items =
            {
                new TodoItem { Title = "Make a todo list 📃" },
                new TodoItem { Title = "Check off the first item ✅" },
                new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
            }
                });

                await _context.SaveChangesAsync();
            }*/
        }
    }
}
