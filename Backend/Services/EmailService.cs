using System.Net;
using System.Net.Mail;
using Backend.Interfaces;
using Backend.Configurations;

namespace Backend.Services;

public class EmailService : INotifyService
{
    public void Notify(string message)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(Secrets.EmailUsername, Secrets.EmailPassword),
            EnableSsl = true
        };
        
        int count = message.Split("Id:").Length - 1;

        client.Send(Secrets.EmailUsername, Secrets.EmailForNotifications, $"{count} job(s) found", message);

    }
}