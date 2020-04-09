using System;
using System.Net.Mail;
using System.Collections.Generic;

//TODO Add option to use the Gmail API so that more sophicated security methods can be used
namespace Email
{
    class SMTPServerAddress{
        public static string Outlook {get{ return "smtp-mail.outlook.com"; } }
        public static string Gmail {get{ return "smtp.gmail.com"; } }
    }

    public class EmailCredentials{
        public string Username{get;set;}
        public string Password{get;set;}
        public bool EnableSSL{get;set;}

        private string smtpAddress;
        public string SmtpAddress{
        get{
            return smtpAddress;
        }
        set{
            switch(value){
                case "gmail":
                    smtpAddress = SMTPServerAddress.Gmail;
                    break;
                case "outlook":
                    smtpAddress = SMTPServerAddress.Outlook;
                    break;
            }
        }
        }
    }

    public class Email{
        public List<string> recipients;
        private string emailAddress;
        SmtpClient smtp;
        public Email(EmailCredentials credentials){
            //Google Account needs to allow for less secure log in for this to work
            smtp = new SmtpClient(credentials.SmtpAddress){
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(credentials.Username,credentials.Password),
                EnableSsl = credentials.EnableSSL,
            };
            emailAddress = credentials.Username;
        }

        public void SendEmail(string emailSubject, string emailBody)
        {
            MailMessage mail = new MailMessage(){
                From = new MailAddress(emailAddress),
                Subject = emailSubject,
                Body = emailBody,
            };

            foreach(var recipient in recipients){
                mail.To.Add(recipient);
            }       
            try{
                smtp.Send(mail);
                Console.WriteLine("Sent new video email");
            }
            catch{
                Console.WriteLine("Email Sending Failed");
            }
        }
    }
    
}