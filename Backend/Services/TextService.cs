using Backend.Interfaces;
using Backend.Configurations;

namespace Backend.Services;

public class TextService : INotifyService
{
    public void Notify(string message)
    {
        Helpers.EmailToText.SendTextMessage(Secrets.EmailPassword, "", message, Secrets.PhoneNumber);        
    }
}