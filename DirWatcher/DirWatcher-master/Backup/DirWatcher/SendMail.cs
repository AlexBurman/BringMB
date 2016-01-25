using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.IO;
using System.Linq;

namespace DirWatcher
{
    class SendMail
    {
        /// <summary>
        /// Send a warning to the users with a list of the files that have the problem. 
        /// </summary>
        /// <param name="fileList"></param>
        /// <returns></returns>
        public bool SendWarning(Dictionary<string, bool> fileList)
        {
            var sendMail = new SmtpClient();

            try
            {
                // Mail settings
                sendMail.Host = "scan.ergogroup.no";

                // Message Settings
                var theMessage = new MailMessage
                                     {
                                         Sender = new MailAddress("NoReply@Bring.com", "DirWatcher"),
                                         From = new MailAddress("NoReply@Bring.com", "DirWatcher")
                                     };

                // Set the message properties (header information)
                theMessage.To.Add(new MailAddress("BringMB.Support@bring.com", "BringMB Support"));
                theMessage.To.Add(new MailAddress("monitoring.notification@tcs.com", "TCS Monitoring"));
                theMessage.Subject = "DirWatcher Warning";
                
                // Generate the body of the mail
                theMessage.Body = "The following files have not been picked up:\n\n";

                fileList = fileList.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

                string lastFolder = null;
                foreach (string fileName in fileList.Keys)
                {
                    if (!fileList[fileName])
                    {
                        var fileInformation = new FileInfo(fileName);

                        if(lastFolder != fileInformation.DirectoryName)
                            theMessage.Body += "\n\nFolder link: " + fileInformation.DirectoryName + "\n\n";

                        theMessage.Body += fileInformation.Name + "\n";
                        lastFolder = fileInformation.DirectoryName;
                    }
                }

                // This is a plain text mail...
                theMessage.IsBodyHtml = false;

                // Send the mail
                sendMail.Send(theMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return true;
        }
    }
}
