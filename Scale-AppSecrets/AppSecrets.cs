using System;
using System.Threading.Tasks;
using System.Security;

namespace Scale.AppSecrets
{
    /// <summary>
    /// Gets Secrets from a Key Vault.
    /// </summary>
    public class AppSecrets
    {
        private IKeyVault _keyVault;

        /// <summary>
        /// Instantiates an instance of <see cref="AppSecrets"/>.
        /// </summary>
        /// <param name="keyVault">Any Key Vault that implements <see cref="IKeyVault"/></param>
        public AppSecrets(IKeyVault keyVault)
        {
            if (keyVault == null) throw new ArgumentNullException("keyVault");
            _keyVault = keyVault;
        }

        /// <summary>
        /// Gets a secret value by name.
        /// </summary>
        /// <param name="name">The name of the Secret.</param>
        /// <returns>The Secret value as Task of string.</returns>
        public async Task<string> GetSecret(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            return await _keyVault.GetSecret(name);
        }

        /// <summary>
        /// Gets a secret value by name and returns as <see cref="SecureString"/>.
        /// </summary>
        /// <param name="name">The name of the Secret.</param>
        /// <returns>The Secret value as Task of <see cref="SecureString"/>.</returns>
        public async Task<SecureString> GetSecretSecure(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            return await _keyVault.GetSecretSecure(name);
        }
    }
}