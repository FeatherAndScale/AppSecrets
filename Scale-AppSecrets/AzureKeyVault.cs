using System;
using System.Security;
using System.Threading.Tasks;
using Microsoft.KeyVault.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Scale.AppSecrets
{
    public class AzureKeyVault : IKeyVault
    {
        private KeyVaultClient _keyVaultClient;
        private string _clientId;
        private string _clientSecret;
        private string _vaultUrl;

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

        public async Task<string> GetSecret(string name)
        {
            var secret = await _keyVaultClient.GetSecretAsync(_vaultUrl, name);
            return secret.Value;
        }

        /// <summary>
        /// Get a secret value and return as <see cref="SecureString"/>
        /// </summary>
        /// <param name="name">The name of the Secret.</param>
        /// <returns>The secret value as <see cref="SecureString"/>.</returns>
        public async Task<SecureString> GetSecretSecure(string name)
        {
            // WARNING: Not actually secure. See Readme for details.
            var secret = await _keyVaultClient.GetSecretAsync(_vaultUrl, name);
            return secret.SecureValue;
        }

        private string GetAccessToken(string authority, string resource, string scope)
        {
            var clientCredential = new ClientCredential(_clientId, _clientSecret);
            var context = new AuthenticationContext(authority, null);
            var result = context.AcquireToken(resource, clientCredential);
            return result.AccessToken;
        }
    }
}

