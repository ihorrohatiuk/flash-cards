using System.ComponentModel.DataAnnotations;

namespace FlashCards.Core.Application.Dtos;

public class RegistrationRequestDto
{
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name cannot exceed 50 characters.")]
    public required string FirstName { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public required string LastName { get; set; }
    
    [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
    public required string Email { get; set; }
    
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters and no more than 100 characters.")]
    public required string Password { get; set; }
}