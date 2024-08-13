using System.ComponentModel.DataAnnotations;

namespace Ehrms.Web.Models;

public sealed class RegisterUserModel
{
    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email {  set; get; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;   

    [Required]
    [Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; }
}