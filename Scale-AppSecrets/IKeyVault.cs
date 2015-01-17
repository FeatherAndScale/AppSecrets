using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace Scale.AppSecrets
{
    public interface IKeyVault
    {
        Task<string> GetSecret(string name);

        Task<SecureString> GetSecretSecure(string name);
    }
}
