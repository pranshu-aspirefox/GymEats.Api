using GymEats.Data;
using GymEats.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymEats.Services.Seed
{
    public class SeedData
    {
        private readonly GymEatsDbContext _dataContext;
        private readonly UserManager<User> _userManager;
        public SeedData(GymEatsDbContext dataContext, UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _dataContext.Database.EnsureCreated();
            await SeedRoles();
            await SeedUsers();
        }

        public async Task SeedRoles()
        {
            var roles = await _dataContext.Roles.CountAsync();
            if (roles == 0)
            {
                _dataContext.Roles.Add(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                _dataContext.Roles.Add(new IdentityRole { Name = "User", NormalizedName = "USER" });
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task SeedUsers()
        {
            try
            {
                var user = await _userManager.FindByEmailAsync("admin@gmail.com");
                if (user == null)
                {
                    user = new User
                    {
                        Email = "admin@gmail.com",
                        UserName = "admin@gmail.com",
                        FirstName = "Admin",
                        LastName = "Admin",

                        EmailConfirmed = true,
                        CreatedOn = DateTime.UtcNow
                    };

                    var result = await _userManager.CreateAsync(user, "Admin@123");
                    if (result == IdentityResult.Success)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    await _dataContext.SaveChangesAsync();
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
