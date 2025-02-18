using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlashCards.Core.Domain.Constants;

namespace FlashCards.Core.Domain.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(10)]
    public string Role { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(64)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [NotMapped]
    public bool IsAdmin => string.Equals(Role, RolesType.Admin);
}
