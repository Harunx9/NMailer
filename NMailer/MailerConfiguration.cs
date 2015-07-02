using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMailer
{


    public class MailerConfiguration 
    {
        private static string _teplateDir = "~/Mails";

        public static string TemplateDirectory
        {
            get
            {
                return _teplateDir;
            }
            set
            {
                _teplateDir = value;
            }
        }
    }
}
