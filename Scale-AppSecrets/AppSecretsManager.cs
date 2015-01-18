using System.Threading.Tasks;
using System.Security;
using System.Configuration;
using System;

namespace Scale.AppSecrets
{
    /// <summary>
    /// An opinionated and conventional helper class for getting Secrets from <see cref="AzureKeyVault"/>.
    /// </summary>
    /// <remarks>
    /// Expects three appSettings to exist in app.config or web.config: Scale.AppSecrets.AppSecretsManager.VaultUrl,
    /// Scale.AppSecrets.AppSecretsManager.ClientId and Scale.AppSecrets.AppSecretsManager.ClientSecret.
    /// </remarks>
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

        /// <summary>
        /// Gets a secret value by name.
        /// </summary>
        /// <param name="name">The name of the Secret.</param>
        /// <returns>The Secret value as Task of string.</returns>
        public static async Task<string> GetSecret(string name)
        {
            return await _appSecrets.GetSecret(name);
        }

        /// <summary>
        /// Gets a secret value by name and returns as <see cref="SecureString"/>.
        /// </summary>
        /// <param name="name">The name of the Secret.</param>
        /// <returns>The Secret value as Task of <see cref="SecureString"/>.</returns>
        /// <remarks>WARNING: Key Vault client deserialises value as plaintext (in to RAM). Read the Readme before 
        /// using this method in Production.</remarks>
        public static async Task<SecureString> GetSecretSecure(string name)
        {
            return await _appSecrets.GetSecretSecure(name);
        }
    }
}