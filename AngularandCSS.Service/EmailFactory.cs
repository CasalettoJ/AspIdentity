using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using AngularandCSS.Data;

namespace AngularandCSS.Service
{
    public class EmailFactory
    {
        private static readonly string EmailUsername = "jctestemailer@gmail.com";//"Boorueon@gmail.com";
        private static readonly string EmailPassword = "0fficial1!";
        private SmtpClient server = new SmtpClient("smtp.gmail.com"){Port = 587, Credentials = new System.Net.NetworkCredential(EmailUsername, EmailPassword), EnableSsl = true};
        public void SendConfirmationEmail(User user, string confirmationToken)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.To.Add(user.Email);
            mail.Subject = "Account Activation";//"Boorueon Account Activation";
            mail.Body = /*"Welcome to Boorueon!*/"Welcome " + user.UserName + ", <br />To activate your account, please click this link: <a href=\"http://localhost:54801/User/Activate?userID=" + user.Id + "&confirmationToken=" + confirmationToken + "\">Activate My Account!</a><br /><br />This email was auto-generated and is not monitored.  Please do not reply to this email.";
            mail.From = new MailAddress(EmailUsername);
            server.Send(mail);
        }

        public void SendPasswordRecoveryEmail(User user, string recoveryToken)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.To.Add(user.Email);
            mail.Subject = "Password Recovery";//"Boorueon Account Activation";
            mail.Body = /*"Welcome to Boorueon!*/"Dear " + user.UserName + ", <br />You are receiving this email because you requested a password recovery.  To reset your password, please click this link: <a href=\"http://localhost:54801/User/Recover?userID=" + user.Id + "&recoveryToken=" + recoveryToken + "\">Reset My Password!</a><br />This request will expire in 24 hours.<br /><br />This email was auto-generated and is not monitored.  Please do not reply to this email.";
            mail.From = new MailAddress(EmailUsername);
            server.Send(mail);
        }

    }
}
