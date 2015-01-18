using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Security;

namespace Scale.AppSecrets.Tests
{
    // To get these tests to work, add your KeyVault details to app.config.
    [TestClass]
    public class AppSecretsManagerTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public async Task GetSecret_KeyName_DoesNotThrow()
        {
            string secretValue = await AppSecretsManager.GetSecret("secret1");
            System.Diagnostics.Debug.WriteLine(secretValue);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task GetSecretSecure_KeyName_DoesNotThrow()
        {
            SecureString secretValue = await AppSecretsManager.GetSecretSecure("secret2");
            System.Diagnostics.Debug.WriteLine(secretValue);

        }
    }
}
