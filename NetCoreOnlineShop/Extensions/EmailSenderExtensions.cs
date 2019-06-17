using NetcoreOnlineShop.Application.ViewModels;
using NetCoreOnlineShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace NetCoreOnlineShop.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Thank you for signing up at Shop Mart. " +
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a><br />" +
                $"Thank you!<br />" +
                $"Shop Mart IT team");
        }

        public static Task SendEmailResetPasswordAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Recovery Password",
                $"Hello,<br />" +
                $"A request to reset the password has been submitted. " +
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(link)}'>clicking here</a>.<br />" +
                $"Thank you!<br />" +
                $"Shop Mart IT team.");
        }

        public static Task SendFeedback(this IEmailSender emailSender, string email, FeedbackViewModel detail)
        {
            return emailSender.SendEmailAsync(email, "ShopMart - Customer feedback",
                $"Customer feedback detail,<br />" +
                $"Customer name: {detail.Name} <br />" +
                $"Customer address: {detail.Address} <br />" +
                $"Customer email: {detail.Email} <br />" +
                $"Message:<br/>{detail.Message}<br />");
        }

        public static Task SendTicket(this IEmailSender emailSender, string email, int billId)
        {
            return emailSender.SendEmailAsync(email, $"Ticket - Order number: #{billId}",
                $"We have received your ticket about bill number <strong>#{billId}</strong>," +
                $" we aim to reponse your ticket issue within 48 hours viayour e - mail address." +
                $"Thank you!<br /><br />" +
                $"Regards <br />" +
                $"Support team.");
        }
    }
}
