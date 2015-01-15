using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Security;
using Moq;

namespace Scale.AppSecrets.Tests
{
    [TestClass]
    public class AppSecretsTests
    {
        /// <summary>
        /// When GetSecret is called for the first time of an Application's lifetime, the API should be called.
        /// </summary>
        [TestMethod]
        public async Task GetSecret_FirstTime_KeyVaultGetSecretIsCalled()
        {
            // Arrange
            const string keyName = "secret1";

            var mockKeyVault = new Mock<IKeyVault>();
            var appSecrets = new AppSecrets(mockKeyVault.Object);

            // Act
            SecureString secretValue = await appSecrets.GetSecret(keyName);

            // Assert
            mockKeyVault.Verify(k => k.GetSecret(keyName), Times.Once);
        }

        /// <summary>
        /// We actually want the API to be called every time because we don't want the secret being cached by this
        /// library. If the Application decides it's OK to cache, it should do so there.
        /// </summary>
        [TestMethod]
        public async Task GetSecret_SecondTime_ApiCalled()
        {
            // Arrange
            const string keyName = "secret1";

            var mockKeyVault = new Mock<IKeyVault>();
            var appSecrets = new AppSecrets(mockKeyVault.Object);

            // Act
            SecureString secretValue = await appSecrets.GetSecret(keyName);
            secretValue = await appSecrets.GetSecret(keyName);

            // Assert
            mockKeyVault.Verify(k => k.GetSecret(keyName), Times.Exactly(2));
        }
    }
}

