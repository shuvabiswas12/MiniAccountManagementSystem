﻿using Microsoft.AspNetCore.Identity.UI.Services;

namespace MiniAccountManagementSystem.App.Utils
{
    public class EmailSender : IEmailSender
    {
        Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
