using System.Threading.Tasks;
using System.Security;
using System.Configuration;
using System;

namespace Scale.AppSecrets
{
    public static class AppSecretsManager
    {
        private static AppSecrets _appSecrets;

        static AppSecretsManager()
        {
            string vaultUrl = ConfigurationManager.AppSettings["Scale.AppSecrets.AppSecretsManager.VaultUrl"];
            string clientId = ConfigurationManager.AppSettings["Scale.AppSecrets.AppSecretsManager.ClientId"];
            string clientSecret = ConfigurationManager.AppSettings["Scale.AppSecrets.AppSecretsManager.ClientSecret"];

            if (string.IsNullOrEmpty(vaultUrl)) throw new InvalidOperationException("AppSetting Scale.AppSecrets.AppSecretsManager.VaultUrl is missing or empty.");
            if (string.IsNullOrEmpty(clientId)) throw new InvalidOperationException("AppSetting Scale.AppSecrets.AppSecretsManager.ClientId is missing or empty.");
            if (string.IsNullOrEmpty(clientSecret)) throw new InvalidOperationException("AppSetting Scale.AppSecrets.AppSecretsManager.ClientSecret is missing or empty.");

            _appSecrets = new AppSecrets(new AzureKeyVault(vaultUrl, clientId, clientSecret));
        }

        public static async Task<string> GetSecret(string name)
        {
            return await _appSecrets.GetSecret(name);
        }
        public static async Task<SecureString> GetSecretSecure(string name)
        {
            // WARNING: Not Actually Secure - Read the Readme before using in Production.
            return await _appSecrets.GetSecretSecure(name);
        }
    }
}