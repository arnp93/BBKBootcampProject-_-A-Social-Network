using System.ComponentModel.DataAnnotations;

namespace BBKBootcampSocial.Core.DTOs.Account
{
    public class LoginUserDTO
    {
        public string Email { get; set; }
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Introduce tu contraseña")]
        [MinLength(9, ErrorMessage = "Ese campo no puede tener menos de 9 caracteres")]
        [MaxLength(150, ErrorMessage = "Ese campo no puede tener mas de 150 caracteres")]
        public string Password { get; set; }
    }

    public enum LoginUserResult{
        Success,
        UserNotExist,
        NotActivated,
        UnknownError
    }
}
