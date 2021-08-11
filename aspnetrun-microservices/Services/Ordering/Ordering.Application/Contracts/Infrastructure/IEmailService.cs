using Ordering.Application.Models;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendMail(Email email);
    }
}
