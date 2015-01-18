using System.Threading.Tasks;
using System.Security;

namespace Scale.AppSecrets
{
    /// <summary>
    /// Defines a Key Vault.
    /// </summary>
    public interface IKeyVault
    {
        /// <summary>
        /// Gets a secret value by name.
        /// </summary>
        /// <param name="name">The name of the Secret.</param>
        /// <returns>The Secret value as Task of string.</returns>
        Task<string> GetSecret(string name);

        /// <summary>
        /// Gets a secret value by name and returns as <see cref="SecureString"/>.
        /// </summary>
        /// <param name="name">The name of the Secret.</param>
        /// <returns>The Secret value as Task of <see cref="SecureString"/>.</returns>
        Task<SecureString> GetSecretSecure(string name);
    }
}
