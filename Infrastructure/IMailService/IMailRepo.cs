using Domain.Mail;

namespace Infrastructure.IMailService
{
    public interface IMailRepo
    {
        Task<bool> SendMailAsync(MailData mailData);
        Task<string> ConvertOrder(MailData mailData);
    }
}