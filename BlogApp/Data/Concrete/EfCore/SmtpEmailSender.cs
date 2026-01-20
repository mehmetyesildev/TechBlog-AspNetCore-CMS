using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using BlogApp.Data.Abstract;

namespace BlogApp.Data.Concrete.EfCore
{
    public class SmtpEmailSender : IEmailSender
    {
        private string? _host;
        private int _port;
        private bool _enableSSL;
        private string? _Username;
        private string? _password;
        private string? _sender;
        public SmtpEmailSender(string? host,int port,bool enableSSL,string? Username,string? password,string? sender)
        {
            _host=host;
            _port=port;
            _enableSSL=enableSSL;
            _Username=Username;
            _password=password;
            _sender=sender;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client=new SmtpClient(_host,_port)
            {
                Credentials=new NetworkCredential(_Username,_password),
                EnableSsl=_enableSSL
            };
            return client.SendMailAsync(new MailMessage(_sender ?? "",email,subject,message)
                {IsBodyHtml=true});
        }
    }
}