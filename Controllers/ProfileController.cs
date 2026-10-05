using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profile_app_backend.Data;
using Profile_app_backend.Models;
using System.Security.Claims;

namespace Profile_app_backend.Controllers;

[Authorize] // Forces the request to present a valid Bearer token from B2C
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProfileController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyProfile()
    {
        // 1. Extract the unique Object ID (oid or sub) and email from the B2C token claims
        var azureOid = User.FindFirstValue("oid");

        var email =
            User.FindFirstValue("preferred_username")
                       ?? User.FindFirstValue(ClaimTypes.Email)
                       ?? "Unknown";

        var name =
            User.FindFirstValue("name")
            ?? "User";

        if (string.IsNullOrWhiteSpace(azureOid))
        {
            return BadRequest(new { message = "Invalid token claims: OID missing." });
        }

        // 2. Check if user already exists in our local SQL database
        var userProfile = await _context.UserProfiles
            .FirstOrDefaultAsync(u => u.AzureOid == azureOid);

        // 3. Just-In-Time (JIT) Provisioning if they are logging in for the first time
        if (userProfile == null)
        {
            userProfile = new UserProfile
            {
                AzureOid = azureOid,
                Email = email,
                FullName = name,
                Role = "Member",
                CreatedAt = DateTime.UtcNow
            };

            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();
        }

        // 4. Return combined profile info back to the React frontend
        return Ok(new
        {
            message = "Profile retrieved successfully",
            profile = userProfile
        });
    }
}