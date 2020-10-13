namespace BBKBootcampSocial.Core.AllServices.IServices
{
    public interface IMailSender
    {
        void Send(string to, string subject, string body);
    }
}