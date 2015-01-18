using System;
using System.Security;
using System.Threading.Tasks;
using Microsoft.KeyVault.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Scale.AppSecrets
{
    /// <summary>
    /// A wrapper for the Microsoft Key Vault Client.
    /// </summary>
    public class AzureKeyVault : IKeyVault
    {
        private KeyVaultClient _keyVaultClient;
        private string _clientId;
        private string _clientSecret;
        private string _vaultUrl;

        /// <summary>
        /// Instantiates an instance of <see cref="AzureKeyVault"/>
        /// </summary>
        /// <param name="vaultUrl">The fully qualified Vault URL.</param>
        /// <param name="clientId">The Application's Azure AD client ID.</param>
        /// <param name="clientSecret">A client key for the Application from Azure AD.</param>
        public AzureKeyVault(string vaultUrl, string clientId, string clientSecret)
        {
            if (string.IsNullOrEmpty(vaultUrl)) throw new ArgumentNullException("vaultUrl");
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException("clientId");
            if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException("clientSecret");
            _vaultUrl = vaultUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
            
            _keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
        }

        /// <summary>
        /// Gets a secret value by name.
        /// </summary>
        /// <param name="name">The name of the Secret.</param>
        /// <returns>The Secret value as Task of string.</returns>
        public async Task<string> GetSecret(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            var secret = await _keyVaultClient.GetSecretAsync(_vaultUrl, name);
            return secret.Value;
        }

        /// <summary>
        /// Get a secret value and return as <see cref="SecureString"/>
        /// </summary>
        /// <param name="name">The name of the Secret.</param>
        /// <returns>The secret value as <see cref="SecureString"/>.</returns>
        /// <remarks>WARNING: Key Vault client deserialises value as plaintext (in to RAM). Read the Readme before 
        /// using this method in Production.</remarks>
        public async Task<SecureString> GetSecretSecure(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            var secret = await _keyVaultClient.GetSecretAsync(_vaultUrl, name);
            return secret.SecureValue;
        }

        /// <summary>
        /// Authentication callback for Key Vault Client.
        /// </summary>
        private string GetAccessToken(string authority, string resource, string scope)
        {
            var clientCredential = new ClientCredential(_clientId, _clientSecret);
            var context = new AuthenticationContext(authority, null);
            var result = context.AcquireToken(resource, clientCredential);
            return result.AccessToken;
        }
    }
}

