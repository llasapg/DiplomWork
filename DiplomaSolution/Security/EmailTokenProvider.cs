using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiplomaSolution.Security
{
    public class EmailTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public EmailTokenProvider(IDataProtectionProvider provider, IOptions<EmailTokenOptions> opt, ILogger<EmailTokenProvider<TUser>> logger) : base(provider, opt, logger)
        {
        }
    }
}
