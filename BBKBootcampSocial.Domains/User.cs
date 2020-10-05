
namespace BBKBootcampSocial.Domains
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Facebook { get; set; }
        public string LinkdIn { get; set; }
        public string Instagram { get; set; }
        public string WhatsApp { get; set; }
        public bool IsActive { get; set; }



    }
}
