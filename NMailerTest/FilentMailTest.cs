using NMailer;
using NMailer.Mails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NMailerTest
{
    public class FilentMailTest
    {
        [Fact]
        public void IWantAddSenderToMail()
        {
            var message1 = new FluentMail(new SmtpClient())
                .From("mailTest@email.com");
            var message2 = new FluentMail(new SmtpClient())
                .From(new MailAddress("mailTest2@email.com"));
            Assert.Equal("mailTest@email.com", message1.Mail.From.Address);
            Assert.Equal("mailTest2@email.com", message2.Mail.From.Address);
        }

        [Fact]
        public void IWantAddNewMailReciver()
        {
            var message1 = new FluentMail(new SmtpClient())
                .To("reciver@mail.com");
            var message2 = new FluentMail(new SmtpClient())
                .To(new MailAddress("reciver2@mail.com"));
            Assert.Equal("reciver@mail.com", message1.Mail.To.First(x => x.Address == "reciver@mail.com").Address);
            Assert.Equal(1, message1.Mail.To.Count);
            Assert.Equal("reciver2@mail.com", message2.Mail.To.First(x => x.Address == "reciver2@mail.com").Address);
            Assert.Equal(1, message2.Mail.To.Count);
        }

        [Fact]
        public void IWantAddTitleToMail()
        {
            var message = new FluentMail(new SmtpClient())
                .Subject("Hi you are awsome");
            Assert.Equal("Hi you are awsome", message.Mail.Subject);
        }

        [Fact]
        public void IWantToAddCCToMyMail()
        {
            var message1 = new FluentMail(new SmtpClient())
                .CC("ccmail@mail.com");
            var message2 = new FluentMail(new SmtpClient())
                .CC(new MailAddress("cc2mail@mail.com"));
            Assert.Equal("ccmail@mail.com", message1.Mail.CC.First(x => x.Address == "ccmail@mail.com").Address);
            Assert.Equal(1, message1.Mail.CC.Count);
            Assert.Equal("cc2mail@mail.com", message2.Mail.CC.First(x => x.Address == "cc2mail@mail.com").Address);
            Assert.Equal(1, message2.Mail.CC.Count);
        }

        [Fact]
        public void IWantToAddMessageFromTemplate()
        {
            MailerConfiguration.TemplateDirectory = "TemplateTest";
            var message = new FluentMail(new SmtpClient())
                .BodyFromTemplate("test", new { Message = "Hello world from template", Sign = "Somebody" });
            Assert.Contains("Hello world from template", message.Mail.Body);
            Assert.Contains("<html>", message.Mail.Body);
            Assert.Contains("<head>", message.Mail.Body);
            Assert.Contains("<body>", message.Mail.Body);
            Assert.Contains("Somebody", message.Mail.Body);
        }

        [Fact]
        public void IWantAddMessageBodyToMail()
        {
            var message = new FluentMail(new SmtpClient())
                .Body("Hi there", null);
            Assert.Equal("Hi there", message.Mail.Body);
        }

        [Fact]
        public void IWantToAddAttachmentToEamil()
        {
            var message = new FluentMail(new SmtpClient())
                .AddAttachment(new Attachment("file.txt"));
            Assert.NotNull(message.Mail.Attachments[0]);
        }

        [Fact]
        public void IWantToAddBcc()
        {
            var message = new FluentMail(new SmtpClient())
                .Bcc(new MailAddress("asdfg@mail.com"));
            var message2 = new FluentMail(new SmtpClient())
                .Bcc("asdfg@mail.com");
            Assert.Equal(1, message.Mail.Bcc.Count);
            Assert.True(message.Mail.Bcc.Any(x => x.Address == "asdfg@mail.com"));
            Assert.Equal(1, message2.Mail.Bcc.Count);
            Assert.True(message2.Mail.Bcc.Any(x => x.Address == "asdfg@mail.com"));
        }

        [Fact]
        public void IWantToSendMail()
        {
            string path = string.Format(@"{0}\Mails", AppDomain.CurrentDomain.BaseDirectory);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (Directory.EnumerateFiles(path).Any())
            {
                var files = Directory.EnumerateFiles(path);
                foreach (string file in files)
                {
                    File.Delete(string.Format( file));
                }
            }
            MailerConfiguration.TemplateDirectory = "TemplateTest";
            var client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            client.PickupDirectoryLocation = path;
            new FluentMail(client)
                .From("Sender@mail.com")
                .To("Reciver@mail.com")
                .Subject("Test")
                .BodyFromTemplate("test", new { Message = "Im test message", Sign = "Sender" })
                .Send();

            Assert.True(Directory.EnumerateFiles(path).Any());
        }
    }
}
