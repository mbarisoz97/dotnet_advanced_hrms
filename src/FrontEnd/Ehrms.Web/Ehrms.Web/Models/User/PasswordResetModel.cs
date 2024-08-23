using System.ComponentModel.DataAnnotations;

namespace Ehrms.Web.Models.User;

public sealed class PasswordResetModel
{
    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string CurrentPassword { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Password should have at least one lower case, one upper case, one number, one special character")]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(NewPassword))]
    public string NewPasswordConfirmation { get; set; } = string.Empty;
}