using Common.Core.Tenant;
using Identity.Core.Enums;
using Identity.Infrastructure.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.Infrastructure.Entities;

public class IdentityDbContextSeed
{
    private readonly IScopedTenantContext _tenantContext;

    public IdentityDbContextSeed(IScopedTenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

    public async Task SeedAsync(
        AuthDbContext context,
        UserManager<ApplicationUser> userManager,
        Guid superAdminId,
        Guid adminId,
        Guid teacherId,
        Guid studentId)
    {
        // Define our test users
        var usersToSeed = new List<(string Email, string UserName, UserRole Role, string Id, string? Index)>
        {
            ("superadmin@unsa.ba", "superadmin", UserRole.Superadmin, superAdminId.ToString(), null),
            ("admin@unsa.ba", "admin", UserRole.Admin, adminId.ToString(), null),
            ("emir.buza@unsa.ba", "teacher", UserRole.Teacher, teacherId.ToString(), null),
            ("niko.nikic@unsa.ba", "student", UserRole.Student, studentId.ToString(), "IB20001")
        };

        foreach (var (email, username, role, userId, index) in usersToSeed)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Id = userId,
                    UserName = username,
                    Email = email,
                    FirstName = username == "student" ? "Niko" : username == "teacher" ? "Emir" : "System",
                    LastName = username == "student" ? "Nikic" : username == "teacher" ? "Buza" : "Admin",
                    EmailConfirmed = true,
                    Role = role,
                    FacultyId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    IndexNumber = index
                };
                await userManager.CreateAsync(user, "Test123!");
            }
            else
            {
                // Force update role and facultyId for existing test users if they are 0/empty
                bool changed = false;
                if (user.Role == 0) { user.Role = role; changed = true; }
                if (user.FacultyId == Guid.Empty) { user.FacultyId = Guid.Parse("11111111-1111-1111-1111-111111111111"); changed = true; }
                if (string.IsNullOrEmpty(user.IndexNumber) && !string.IsNullOrEmpty(index)) { user.IndexNumber = index; changed = true; }
                
                if (changed)
                {
                    await userManager.UpdateAsync(user);
                }
            }
        }
    }
}