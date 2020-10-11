using System.ComponentModel.DataAnnotations;

namespace BBKBootcampSocial.Core.DTOs.Account
{
    public class RegisterUserDTO
    {
        [Display(Name = "Username")]
        [MinLength(4, ErrorMessage = "Ese campo no puede tener menos de 4 caracteres")]
        [MaxLength(100, ErrorMessage = "Ese campo no puede tener mas de 100 caracteres")]
        public string Username { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Introduce tu nombre")]
        [MaxLength(50, ErrorMessage = "Ese campo no puede tener mas de 50 caracteres")]
        public string FirstName { get; set; }
        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "Introduce tu apellido")]
        [MaxLength(50, ErrorMessage = "Ese campo no puede tener mas de 50 caracteres")]
        public string LastName { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Introduce tu email")]
        [MaxLength(150, ErrorMessage = "Ese campo no puede tener mas de 150 caracteres")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Introduce tu contraseña")]
        [MinLength(9, ErrorMessage = "Ese campo no puede tener menos de 9 caracteres")]
        [MaxLength(150, ErrorMessage = "Ese campo no puede tener mas de 150 caracteres")]
        public string Password { get; set; }
        [Display(Name = "Repetir la Contraseña")]
        [Required(ErrorMessage = "Introduce tu contraseña")]
        [Compare("Password")]
        public string RePassword { get; set; }
    }

    public enum RegisterUserResult
    {
        Success,
        EmailExists

    }
}
