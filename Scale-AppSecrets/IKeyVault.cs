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
        Task<SecureString> GetSecret(string name);
    }
}
