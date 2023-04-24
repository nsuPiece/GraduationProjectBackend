using Host.Base.Result;
using Host.Dto.Login;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using IResult = Host.Base.Result.IResult;
using Host.Dto.Email;
using Microsoft.AspNetCore.Authorization;

namespace Host.Controllers;

[ApiController]
[Route("email")]
public class EmailController
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IResult> SendEmail(EmailInputDto input)
    {
        var result = new ResultDto();

        string senderEmail = "1802975153@qq.com";
        string senderPassword = "jalcnfkowqsrdfcf";
        string recipientEmail = "nsupiece@qq.com";
        string subject = input.subject;
        string body = input.body;
        string error = "";

        var message = new MailMessage(senderEmail, recipientEmail)
        {
            Subject = subject,
            Body = body
        };

        var client = new SmtpClient("smtp.qq.com", 587);

        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(senderEmail, senderPassword);
        client.EnableSsl = true;

        client.SendCompleted += (sender, e) =>
        {
            if (e.Error != null)
            {
                error = "Error sending email: " + e.Error.Message;
                Console.WriteLine(error);
            }
            else if (e.Cancelled)
            {
                error = "Email sending was cancelled.";
                Console.WriteLine(error);
            }
            else
            {
                Console.WriteLine("Email sent successfully.");
            }
        };

        client.SendAsync(message, null);
        Console.WriteLine("Sending email...");


        if (error == "") return result.Success();
        else return result.Error(error);

    }
}
