using System.ComponentModel.DataAnnotations;

namespace Profile_app_backend.Models;
public class UserProfile
{
    public int Id { get; set; } // Internal auto-increment primary key for SQL relations

    [Required]
    public string AzureB2COid { get; set; } // The immutable Object ID from B2C token

    [Required]
    public string Email { get; set; }

    public string FullName { get; set; }

    public string Role { get; set; } = "Member"; // Default app role

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}