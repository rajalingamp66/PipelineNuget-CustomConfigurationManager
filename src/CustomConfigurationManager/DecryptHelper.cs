using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text;

namespace CustomConfigurationManager
{
    public static class DecryptHelper
    {
        private static readonly IDataProtectionProvider _dataProtectionProvider = DataProtectionProvider.Create("AppSettings");

        public static bool TryDecrypt(string value, out string result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                var encryptedBytes = Convert.FromBase64String(value);
                var protector = _dataProtectionProvider.CreateProtector("AppSettingsProtector");
                var decryptedBytes = protector.Unprotect(encryptedBytes);

                result = Encoding.UTF8.GetString(decryptedBytes);
                return !string.IsNullOrEmpty(result);
            }
            catch
            {
                return false; // fallback to original
            }
        }
    }
}
