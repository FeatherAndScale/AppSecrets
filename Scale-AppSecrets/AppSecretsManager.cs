using System.Threading.Tasks;
using System.Security;
namespace Scale.AppSecrets
{
    public static class AppSecretsManager
    {
        private static AppSecrets _appSecrets;

        static AppSecretsManager()
        {
            _appSecrets = new AppSecrets(new AzureKeyVault());
        }

        public static async Task<SecureString> GetSecret(string name)
        {
            return await _appSecrets.GetSecret(name);
        }
    }
}