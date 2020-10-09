namespace BBKBootcampSocial.Core.IServices
{
    public interface IMailSender
    {
        void Send(string to, string subject, string body);
    }
}