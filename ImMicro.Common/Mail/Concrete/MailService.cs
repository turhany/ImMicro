using System;
using System.IO;
using System.Threading.Tasks;
using ImMicro.Common.BaseModels.Mail;
using ImMicro.Common.Constans;
using ImMicro.Common.Mail.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RazorLight;
using SendGrid;
using SendGrid.Helpers.Mail; 

namespace ImMicro.Common.Mail.Concrete
{
    public class MailService :  IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MailService> _logger;

        public MailService(IConfiguration configuration, ILogger<MailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(MailRequest request)
        {
            try
            {
                var client = new SendGridClient(_configuration.GetValue<string>(MailConstants.SendGridApiKey));
                var from = new EmailAddress(MailConstants.FromEmail, MailConstants.FromName);
                var subject = $"{MailConstants.SubjectPrefix}{request.Subject}";
                var to = new EmailAddress(request.To);
                var plainTextContent = request.PlainTextContent;
                var htmlContent = request.HtmlContent;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> SendEmailWithTemplateAsync<T>(MailRequest request, string templatePath, T model)
        {
            try
            {
                var engine = new RazorLightEngineBuilder()
                    .UseFileSystemProject(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mail", "MailTemplates"))
                    .UseMemoryCachingProvider()
                    .Build();

                string razorResult = await engine.CompileRenderAsync(templatePath, model);

                var client = new SendGridClient(_configuration.GetValue<string>(MailConstants.SendGridApiKey));
                var from = new EmailAddress(MailConstants.FromEmail, MailConstants.FromName);
                var subject = $"{MailConstants.SubjectPrefix}{request.Subject}";
                var to = new EmailAddress(request.To);
                var plainTextContent = request.PlainTextContent;
                var htmlContent = razorResult;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                return response.StatusCode == System.Net.HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            } 
        }
    } 
}