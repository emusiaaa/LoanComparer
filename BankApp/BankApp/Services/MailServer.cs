using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using BankApp.Repositories;
using BankApp.Models;

namespace BankApp.Services;

public class EmailSender : IEmailSender, IHostedService, IDisposable
{
    private readonly ILogger _logger;
    private Timer? _timer = null;
    private IServiceProvider? _serviceProvider;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<EmailSender> logger,
                       IServiceProvider serviceProvider = null)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(Options.SendGridKey))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(Options.SendGridKey, subject, message, toEmail);
    }
    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("getaloanfrombankgirls@gmail.com"),
            Subject = subject,
            PlainTextContent = "",
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        _logger.LogInformation(response.IsSuccessStatusCode
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(SendPeriodicalMail, null, TimeSpan.Zero,
            TimeSpan.FromHours(12));
            // (for demo)
           //TimeSpan.FromMinutes(2));

        return Task.CompletedTask;
    }

    private async void SendPeriodicalMail(object? state)
    {
        Random rnd = new Random();
        string happyReceiversEmail;
        using (var scope = _serviceProvider.CreateScope())
        {
            var help = scope.ServiceProvider.GetRequiredService<IClientRepository>();
              happyReceiversEmail = help.GetRandomClientsEmail(rnd.Next(1000));
        }

        await SendEmailAsync(happyReceiversEmail, "Ready for another loan?",
        MailCreator.ReminderEmail());
        // (for demo)
        //await SendEmailAsync("getaloanfrombankgirls2@gmail.com", "Ready for another loan?",
        //         MailCreator.ReminderEmail());
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
