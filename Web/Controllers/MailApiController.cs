using Domain.Mail;
using Infrastructure.IMailService;
using Microsoft.AspNetCore.Mvc;

namespace YoKart.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailApiController : ControllerBase
    {
        private readonly IMailRepo _mailRepo;

        public MailApiController(IMailRepo mailRepo)
        {
            _mailRepo = mailRepo;
        }

        [HttpPost]
        [Route("SendMail")]
        public async Task<bool> SendMail(MailData mailData)
        {
            return await _mailRepo.SendMailAsync(mailData);
        }
    }
}
