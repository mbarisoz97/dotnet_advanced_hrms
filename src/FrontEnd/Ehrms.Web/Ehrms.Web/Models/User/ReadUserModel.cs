using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Ehrms.Web.Models.User;

public sealed class ReadUserModel
{
    public Guid Id { get; set; }

    [Required]
    [Length(4, 50)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    
    [Required]
    [EnsureOneItem]
    public IEnumerable<string> Roles { get; set; } = [];
}

public class EnsureOneItemAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is IList list)
        {
            return list.Count > 0;
        }

        if (value is ICollection collection)
        {
            return collection.Count > 0;
        }
        
        return false;
    }     
}