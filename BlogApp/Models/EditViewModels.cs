using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class EditViewModels
    {
        public string? Name { get; set; }
        public string? UserId { get; set; }
        [EmailAddress]
        public string? Email { get; set; } 

        [DataType(DataType.Password)]
        public string? Password { get; set; } 

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parolanız Eşlesmiyor.")]
        public string? ConfirmPassword { get; set; } 
        public IList<string>? SelectedRoles { get; set; }
    }
}