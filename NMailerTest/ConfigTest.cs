using NMailer;
using System;
using System.Linq;
using Xunit;

namespace NMailerTest
{
    public class ConfigTest
    {
        [Fact]
        public void IWantSetNewTemplatesDir()
        {
            MailerConfiguration.TemplateDirectory = "~/MyMails";
            Assert.Equal("~/MyMails", MailerConfiguration.TemplateDirectory);
        }
    }
}
