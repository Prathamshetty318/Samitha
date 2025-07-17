using DubaiChaRaja.Service;
using MailKit.Net.Smtp;
using MimeKit;

public class EmailService : IEmailService
{
    private readonly string senderEmail = "shettypratham73@gmail.com"; // your real Gmail
    private readonly string password = "mxdf soic zqrc ysyt"; // Gmail app password

    public async Task SendResetEmail(string to, string code)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(senderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = "Your Verification Code";

        email.Body = new TextPart("html")
        {
            Text = $"Your verification code is:{code}</p>"
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(senderEmail, password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}