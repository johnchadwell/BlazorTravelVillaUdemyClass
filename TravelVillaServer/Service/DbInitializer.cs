using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelVillaServer.Service.IService;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Common;
using Microsoft.EntityFrameworkCore;

namespace TravelVillaServer.Service
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<IdentityUser> _userMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            _db = db;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }

            } catch (Exception)
            {

            }


            if (_db.Roles.Any(x => x.Name == SD.Role_Admin)) return;

            _roleMgr.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            _roleMgr.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
            _roleMgr.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();

            _userMgr.CreateAsync(new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            }, "Admin123*").GetAwaiter().GetResult();

            IdentityUser user = _db.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
            _userMgr.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

        }
    }
}
