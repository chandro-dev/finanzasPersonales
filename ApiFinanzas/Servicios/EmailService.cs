using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task EnviarAsync(string destinatario, string asunto, string mensajeHtml)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config["Email:Remitente"]));
        email.To.Add(MailboxAddress.Parse(destinatario));
        email.Subject = asunto;

        var builder = new BodyBuilder { HtmlBody = mensajeHtml };
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_config["Email:Servidor"], int.Parse(_config["Email:Puerto"]), true);
        await smtp.AuthenticateAsync(_config["Email:Usuario"], _config["Email:Clave"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
