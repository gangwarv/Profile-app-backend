using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile_app_backend.Models;
public class UserProfile
{
    public int Id { get; set; } // Internal auto-increment primary key for SQL relations

    [Required]
    public string AzureOid { get; set; } // Neutral property name for the identity object id (will create AzureOid column when DB is created)

    [Required]
    public string Email { get; set; }

    public string FullName { get; set; }

    public string Role { get; set; } = "Member"; // Default app role

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}