using MailKit.Net.Smtp;
using MimeKit;
using NotificationService.Models;
using MailKit.Security;
using Microsoft.Extensions.Logging;

namespace NotificationService.Services;
public class EmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(Appointment appointment)
    {
        if (string.IsNullOrEmpty(appointment.OwnerEmail))
        {
            _logger.LogError("Owner email cannot be null or empty.");
            throw new ArgumentNullException(nameof(appointment.OwnerEmail), "Owner email cannot be null or empty.");
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Pet Care", "no-reply@petserviceapi.com"));
        message.To.Add(new MailboxAddress("", appointment.OwnerEmail));
        message.Subject = "Confirmação de Agendamento";
        message.Body = new TextPart("plain")
        {
            Text = $"Olá,\n\nO agendamento para o seu pet está confirmado para {DateTime.Parse(appointment.Date).ToString("dd/MM/yyyy")} às {appointment.Time}.\n\nServiço: {appointment.ServiceType}\n\nObrigado!"
        };

        try
        {
            using (var client = new SmtpClient())
            {
                _logger.LogInformation("Connecting to SMTP server...");
                await client.ConnectAsync(_configuration["Email:SmtpServer"], int.Parse(_configuration["Email:Port"]), SecureSocketOptions.StartTls);
                _logger.LogInformation("Authenticating...");
                await client.AuthenticateAsync(_configuration["Email:Username"], _configuration["Email:Password"]);
                _logger.LogInformation("Sending email to {OwnerEmail}", appointment.OwnerEmail);
                await client.SendAsync(message);
                _logger.LogInformation("Email sent successfully to {OwnerEmail}", appointment.OwnerEmail);
                await client.DisconnectAsync(true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {OwnerEmail}", appointment.OwnerEmail);
            // Implement retry logic here if needed
        }
    }
}