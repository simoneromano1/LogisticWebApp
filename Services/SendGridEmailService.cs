using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using LogisticWebApp.Models;

namespace LogisticWebApp.Services;

public class SendGridEmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly SendGridClient _client;

    public SendGridEmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
        _client = new SendGridClient(_settings.SendGridApiKey);
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
        var toAddress = new EmailAddress(to);
        var msg = MailHelper.CreateSingleEmail(from, toAddress, subject, string.Empty, body);
        await _client.SendEmailAsync(msg);
    }
}