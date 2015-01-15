using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Scale.AppSecrets.Tests
{
    [TestClass]
    public class AppSecretsManagerTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public async Task GetSecret_KeyName_DoesNotThrow()
        {
            var secretValue = await AppSecretsManager.GetSecret("secret2");
            System.Diagnostics.Debug.WriteLine(secretValue.ToString());
        } 
    }
}
