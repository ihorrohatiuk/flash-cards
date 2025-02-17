using System.ComponentModel.DataAnnotations;

namespace FlashCards.Core.Application.Dtos;

public class UserRegistrationDto
{
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public required string FirstName { get; init; }

    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public required string LastName { get; init; }
    
    [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; init; }
    
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters and no more than 100 characters.")]
    public required string Password { get; init; }
}