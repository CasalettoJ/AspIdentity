using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace AngularandCSS.Service
{
    public static class EmailFactory
    {
        private static readonly string EmailUsername = "jctestemailer@gmail.com";//"Boorueon@gmail.com";
        private static readonly string EmailPassword = "0fficial1!";
        private static SmtpClient server = new SmtpClient("smtp.gmail.com"){Port = 587, Credentials = new System.Net.NetworkCredential(EmailUsername, EmailPassword), EnableSsl = true};
        public static void SendConfirmationEmail(string To, string userID, string confirmationToken)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.To.Add(To);
            mail.Subject = "Account Activation";//"Boorueon Account Activation";
            mail.Body = /*"Welcome to Boorueon!*/"  To activate your account, please click this link: <a href=\"http://localhost:54801/Home/Activate?userID=" + userID + "&confirmationToken=" + confirmationToken + "\">Activate My Account!</a><br /><br />This email was auto-generated and is not monitored.  Please do not reply to this email.";
            mail.From = new MailAddress(EmailUsername);
            server.Send(mail);
        }

    }
}
