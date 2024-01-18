using System.ComponentModel.DataAnnotations;

namespace Cial.ViewModels.AuthViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Fullname is required."), MaxLength(64)]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Email is required."), MaxLength(32)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required."), MaxLength(32)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required."), MinLength(6), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$")]
        public string Password { get; set; }
        [Required(ErrorMessage = "You need to re-type password."), MinLength(6), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$"), Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}
