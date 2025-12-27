using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name= "EPosta")]        
        public string? Email { get; set; }

        [Required]
        [StringLength(10,ErrorMessage ="{0} alanı en az {2} karekter uznuluğunda olmalıdır." ,MinimumLength =6)]
        [DataType(DataType.Password)]
        [Display(Name ="Parola")]
        public string? Password { get; set; }
    }
}