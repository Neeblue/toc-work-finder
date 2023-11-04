using System.Net;
using System.Net.Mail;
using Backend.Configurations;

namespace Backend.Helpers;

public static class EmailToText
{
    private static readonly string _email = Secrets.EmailUsername;
    private static readonly string _phoneNumber = "@tmomail.net";

    public static bool SendTextMessage(string password, string subject, string message, string number)
    {

        try
        {
            using (MailMessage textMessage = new MailMessage(_email, number + _phoneNumber, subject, message))
            {
                using (SmtpClient textMessageClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    textMessageClient.UseDefaultCredentials = false;
                    textMessageClient.EnableSsl = true;
                    textMessageClient.Credentials = new NetworkCredential(_email, password);
                    textMessageClient.Send(textMessage);
                    return true;
                }
            }
        }
        catch { return false; }
    }
}