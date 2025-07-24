using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomConfigurationManager
{
    public class AWSSecretsManagerKeyReadService
    {
        public async Task<SecretResult> RetrieveSecretsManagerSecretKey(string accessKey, string secretKey, string secretName)
        {
            var client = new AmazonSecretsManagerClient(new BasicAWSCredentials(accessKey, secretKey), RegionEndpoint.USWest2);
            return await GetSecretValueAsync(client, secretName);
        }

        private async Task<SecretResult> GetSecretValueAsync(AmazonSecretsManagerClient client, string secretName)
        {
            var response = await client.GetSecretValueAsync(new GetSecretValueRequest { SecretId = secretName });
            var secretData = response.SecretString ?? Encoding.UTF8.GetString(Convert.FromBase64String(response.SecretBinary.ToString()));
            return ParseSecret(secretData);
        }

        private SecretResult ParseSecret(string secretData)
        {
            try
            {
                using var doc = JsonDocument.Parse(secretData);
                var root = doc.RootElement;

                var result = new SecretResult { IsJson = true };

                if (root.TryGetProperty("apiSettings", out JsonElement apiSettingsElement))
                {
                    result.AppSettings = new Dictionary<string, string>();
                    foreach (var setting in apiSettingsElement.EnumerateArray())
                    {
                        var key = setting.GetProperty("key").GetString();
                        var value = setting.GetProperty("value").GetString();
                        if (!string.IsNullOrWhiteSpace(key))
                            result.AppSettings[key] = value ?? string.Empty;
                    }
                }

                if (root.TryGetProperty("connectionStrings", out JsonElement connStringsElement))
                {
                    result.ConnectionStringPairs = new Dictionary<string, (string, string)>();
                    foreach (var conn in connStringsElement.EnumerateArray())
                    {
                        var name = conn.GetProperty("name").GetString();
                        var connStr = conn.GetProperty("connectionString").GetString();
                        var provider = conn.GetProperty("providerName").GetString();
                        if (!string.IsNullOrWhiteSpace(name))
                            result.ConnectionStringPairs[name] = (connStr ?? string.Empty, provider ?? string.Empty);
                    }
                }

                return result;
            }
            catch
            {
                return new SecretResult { IsJson = false, PlainText = secretData };
            }
        }
    }
}
