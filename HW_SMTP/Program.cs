using MailKit.Net.Pop3;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace HW_SMTP
{
     class Program
    {
        private const string filepath = @"C:\pv_311\MyTest.txt";
        static async Task Main(string[] args)
        {

            await CreateFile(filepath);
            List<string> recipients = new List<string>()
            { "dorokhov.arteom@gmail.com"};

            MailAddress from = new MailAddress("ar.doroxoff@gmail.com","artem");
            MailMessage message = new MailMessage()
            {
                Subject = "hello friend",
                Body = "test message",
                IsBodyHtml = false,
                From = from

            };


            Attachment attachment = new Attachment(filepath);
            message.Attachments.Add(attachment);

            foreach (string recipient in recipients)
            {
                message.To.Add(recipient);
            }

            SmtpClient smtp = new SmtpClient("smtp.gmail.com",587);
            smtp.Credentials = new NetworkCredential("ar.doroxoff@gmail.com", "gyic ugxm ukac ylkt");
            smtp.EnableSsl = true;
            smtp.Send(message);
            await ReadEmail();



            static async Task CreateFile(string filePath)
            {
                 if (File.Exists(filePath))
                {
                   await using (FileStream fs = File.Create(filePath, 1024))
                    {
                        byte[] data = new UTF8Encoding(true).GetBytes("this is some text in file");
                        await fs.WriteAsync(data, 0, data.Length);
                    }
                }
                else
                {
                    throw new Exception("file not created or something error");
                }
            }


            static async Task ReadEmail()
            {
                using (var pop3 = new Pop3Client())
                {
                    try
                    {
                        await pop3.ConnectAsync("pop.gmail.com", 995, true);

                        await pop3.AuthenticateAsync("ar.doroxoff@gmail.com", "gyic ugxm ukac ylkt");

                        int messageCount = pop3.Count;
                        Console.WriteLine($"count message: {messageCount}");

                        await pop3.DisconnectAsync(true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
