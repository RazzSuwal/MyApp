using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;
using MyCommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.DbInitalizer
{
    public class DbInitializer : IDbInitalizer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initializer()
        {
            try
            {
                if(_context.Database.GetPendingMigrations().Count()>0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }
            if (!_roleManager.RoleExistsAsync(WebsiteRole.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebsiteRole.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRole.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRole.Role_User)).GetAwaiter().GetResult();
            }
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName ="admin123@gmail.com",
                Email="admin123@gmail.com",
                Name="Admin",
                PhoneNumber="123456789",
                Address="xyz",
                City="xyz",
                State="xyz",
                PinCode="333011"
            },"Admin@123").GetAwaiter().GetResult();
            ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(x => x.Email == "admin123@gmail.com");
            _userManager.AddToRoleAsync(user, WebsiteRole.Role_Admin).GetAwaiter().GetResult();
        }
        
    }
}
