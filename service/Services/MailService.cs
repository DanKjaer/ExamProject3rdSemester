using infrastructure.datamodels;
using MailKit.Net.Smtp;
using MimeKit;

namespace service.Services;

public class MailService
{
    public static void SendEmail(string clientName, AnimalSpecies animal, string toEmail)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Møøhn Zoo" , Environment.GetEnvironmentVariable("FromEmail")));
        message.To.Add(new MailboxAddress(clientName, toEmail));
        message.Subject = "Information about " + animal.SpeciesName;
        
        message.Body = new TextPart("plain")
        {
            Text = @$"Animal Species: {animal.SpeciesName}

 Description:
 {animal.SpeciesDescription}

Best Regards, Møønh Zoo"
        };

        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(Environment.GetEnvironmentVariable("FromEmail"), Environment.GetEnvironmentVariable("FromPass"));
            client.Send(message);
            client.Disconnect(true);
        }
    }
}