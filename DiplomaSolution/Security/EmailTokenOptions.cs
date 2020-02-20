using System;
using Microsoft.AspNetCore.Identity;

namespace DiplomaSolution.Security
{
    /// <summary>
    /// Custom token provider options to specify token life-span
    /// </summary>
    public class EmailTokenOptions : DataProtectionTokenProviderOptions
    {
    }
}
