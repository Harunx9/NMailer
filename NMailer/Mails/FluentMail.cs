using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NMailer.Mails
{
    public class FluentMail
    {
        private MailMessage _mail;
        private SmtpClient _client;

        public MailMessage Mail { get { return _mail; } }

        public FluentMail(SmtpClient client)
        {
            _client = client;
            _mail = new MailMessage();
        }

        public FluentMail From(MailAddress from)
        {
            _mail.From = from;
            return this;
        }

        public FluentMail From(string from)
        {
            var addressFrom = new MailAddress(from, from, System.Text.Encoding.UTF8);
            _mail.From = addressFrom;
            return this;
        }

        public FluentMail Subject(string subject)
        {
            _mail.Subject = subject;
            return this;
        }


        public FluentMail To(string to)
        {
            var address = new MailAddress(to);
            _mail.To.Add(address);
            return this;
        }

        public FluentMail To(MailAddress to)
        {
            _mail.To.Add(to);
            return this;
        }

        public FluentMail CC(string cc)
        {
            var address = new MailAddress(cc);
            _mail.CC.Add(address);
            return this;
        }

        public FluentMail CC(MailAddress cc)
        {
            _mail.CC.Add(cc);
            return this;
        }

        public FluentMail Body(string message, System.Text.Encoding encoding)
        {
            if (encoding != null)
                _mail.BodyEncoding = encoding;

            _mail.Body = message;
            return this;
        }

        public FluentMail BodyFromTemplate(string templateName, object model)
        {
            string templateSource = File.ReadAllText(string.Format("{0}/{1}/{2}.cshtml", AppDomain.CurrentDomain.BaseDirectory,MailerConfiguration.TemplateDirectory, templateName));
            string bodyResult = Engine.Razor.RunCompile(templateSource, "key", null, model);
            _mail.Body = bodyResult;
            _mail.IsBodyHtml = true;
            return this;
        }

        public FluentMail AddAttachment(Attachment attachment)
        {
            _mail.Attachments.Add(attachment);
            return this;
        }

        public void Send()
        {
            if (_mail.From.Address == string.Empty) throw new ArgumentException("There is no reciver set");
            _client.Send(_mail);
        }

        public FluentMail Bcc(MailAddress address)
        {
            _mail.Bcc.Add(address);
            return this;
        }

        public FluentMail Bcc(string address)
        {
            var addr = new MailAddress(address);
            _mail.Bcc.Add(addr);
            return this;
        }
    }
}
