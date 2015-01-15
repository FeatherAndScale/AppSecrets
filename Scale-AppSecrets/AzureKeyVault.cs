using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.KeyVault.Client;
using System.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Scale.AppSecrets
{
    public class AzureKeyVault : IKeyVault
    {
        private KeyVaultClient _keyVaultClient;

        public AzureKeyVault()
        {
            _keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
        }

        public async Task<SecureString> GetSecret(string name)
        {
            var vaultUrl = ConfigurationManager.AppSettings["VaultUrl"]; //TODO
            var secret = await _keyVaultClient.GetSecretAsync(vaultUrl, name);
            return secret.SecureValue;
        }

        public static string GetAccessToken(string authority, string resource, string scope)
        {
            var client_id = ConfigurationManager.AppSettings["AuthClientId"]; //TODO
            var client_secret = ConfigurationManager.AppSettings["AuthClientSecret"]; //TODO

            var clientCredential = new ClientCredential(client_id, client_secret);
            var context = new AuthenticationContext(authority, null);
            var result = context.AcquireToken(resource, clientCredential);

            return result.AccessToken;
        }

    }
}
