using Domain.Mail;
using Domain.Models;
using Infrastructure.IMailService;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;


namespace Application.MailService
{
    public class MailRepo : IMailRepo
    {
        private readonly MailSettings _mailSettings;
        public MailRepo(IOptions<MailSettings> IOptions)
        {
            _mailSettings = IOptions.Value;
        }

        public async Task<bool> SendMailAsync(MailData mailData)
        {
            mailData.EmailBody = await ConvertOrder(mailData);
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = mailData.EmailBody;
                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<String> ConvertOrder(MailData mailData)
        {
            var EmailObject = JsonConvert.DeserializeObject<Order>(mailData.EmailBody);
            string html = $"<div style=\"margin:30px;background-color:#f4fdff;padding:25px;border:1px solid;\"><h2>YoKart" +
                $"</h2><h4>{mailData.EmailToName}</h4><p>Your order has Comfirmed!</p>";
            html += "<table border=\"1\" cellpadding=\"7\" style=\"border-collapse:collapse;color:black;\"><thead><tr>" +
                "<th>Product Name</th><th>Unit Price</th><th>Quantity</th><th>Total</th>" +
                "</tr></thead>";

            foreach (var item in EmailObject.OrderItems)
            {
                html += $"<tbody><tr><td>{item.Products.ProductName}</td><td>{item.UnitPrice}</td><td>" +
                    $"{item.Quantity}</td><td>{item.Price}</td></tr></tbody>";
            }
            html += $"</table><h4>Total Price = <b style=\"color:red;\">{EmailObject.TotalPrice}/-</b></h4>" +
                $"<small>Thank you for placing your order!</small></div>";
            return html;
        }
    }
}
