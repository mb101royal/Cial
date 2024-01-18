using System.ComponentModel.DataAnnotations;

namespace Cial.ViewModels.AuthViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username or Email is required."), MaxLength(32)]
        public string UsernameOrEmail { get; set; }
        [Required(ErrorMessage = "Password is required."), MinLength(6), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$")]
        public string Password { get; set; }
    }
}
