using BBKBootcampSocial.Domains.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BBKBootcampSocial.Domains.Access;
using System.ComponentModel.DataAnnotations.Schema;
using BBKBootcampSocial.Domains.Canal;

namespace BBKBootcampSocial.Domains.User
{
    public class User : BaseEntity
    {
        #region Properties

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Introduce un Username unico")]
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
        [MinLength(9,ErrorMessage = "Ese campo no puede tener menos de 9 caracteres")]
        [MaxLength(150, ErrorMessage = "Ese campo no puede tener mas de 150 caracteres")]
        public string Password { get; set; }
        public string Facebook { get; set; }
        public string LinkdIn { get; set; }
        public string Instagram { get; set; }
        public string WhatsApp { get; set; }
        public bool IsActive { get; set; }
        public string ActiveCode { get; set; }

        public long? FriendId { get; set; }


        #endregion

        #region Relations

        public virtual ICollection<Post.Post> Posts { get; set; }
        public virtual ICollection<Post.Story> Stories { get; set; }
        public virtual ICollection<Image.Image> Images { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        [ForeignKey("FriendId")]
        public virtual ICollection<User> Users{ get; set; }

        public ICollection<CanalUser> Canals { get; set; }

        #endregion

    }
}
