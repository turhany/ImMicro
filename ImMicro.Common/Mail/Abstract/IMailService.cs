using System.Threading.Tasks;
using ImMicro.Common.BaseModels.Mail;

namespace ImMicro.Common.Mail.Abstract
{
    public interface IMailService
    {
        Task<bool> SendEmailAsync(MailRequest request);
        Task<bool> SendEmailWithTemplateAsync<T>(MailRequest request, string templatePath, T model);
    }
}