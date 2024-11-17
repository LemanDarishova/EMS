using Ems.ExternalServices.Interfaces;
using Ems.ExternalServices.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Ems.ExternalServices.Services;

public class EmailService : IEmailService
{
    public readonly SmtpSetting _smtpSetting;

    public EmailService(IOptions<SmtpSetting> smtpSetting)
    {
        _smtpSetting = smtpSetting.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSetting.SenderEmail, _smtpSetting.SenderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);
            var smtpClient = new SmtpClient(_smtpSetting.Server, _smtpSetting.Port)
            {
                EnableSsl = _smtpSetting.EnableSsl,
                Credentials = new NetworkCredential(_smtpSetting.SenderEmail, _smtpSetting.Password)
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (SmtpException smtpEx)
        {
            
            throw new InvalidOperationException($"SMTP səhvi: {smtpEx.Message}", smtpEx);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Could not send email", ex);
        }
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetLink)
    {
        var subject = "Parol Sıfırlama";
        var message = $"Parolunuzu sıfırlamaq üçün aşağıdakı linkə klikləyin: <a href='{resetLink}'>Sıfırla</a>";

        await SendEmailAsync(email, subject, message); // E-poçt göndərmək üçün mövcud metodu çağırın

    }
}