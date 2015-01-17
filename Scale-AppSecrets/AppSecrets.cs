using System;
using System.Threading.Tasks;
using System.Security;

namespace Scale.AppSecrets
{
    public class AppSecrets
    {
        private IKeyVault _keyVault;

        public AppSecrets(IKeyVault keyVault)
        {
            if (keyVault == null) throw new ArgumentNullException("keyVault");
            _keyVault = keyVault;
        }

        public async Task<string> GetSecret(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            return await _keyVault.GetSecret(name);
        }

        public async Task<SecureString> GetSecretSecure(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            return await _keyVault.GetSecretSecure(name);
        }
    }
}