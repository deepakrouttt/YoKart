using Domain.Mail;
using Domain.Models;

namespace Infrastructure.IMailService
{
    public interface IMailRepo
    {
        Task<bool> SendMailAsync(MailData mailData);
    }
}