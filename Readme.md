# AppSecrets

Easy library for retrieving secrets from [Microsoft Azure Key Vault](http://azure.microsoft.com/en-us/services/key-vault/) (Preview).
Only two methods are offered; `Task<string> GetSecret(string name)` and `Task<SecureString> GetSecretSecure(string name)`. There are no plans to add calls to create or modify secrets. If you want to do that, use the PowerShell commands (see below).

**Be aware that although secrets can be retrieved as `SecureString`, values are still deserialised by the Key Vault client in plain text (in RAM).** Hopefully this will be addressed in future versions. For secrets like Bearer Tokens and Connection Strings this is not usually a big deal - but use your own judgment.


## Quick Start

Library is .NET 4.5 Framework and Async by default.

Once your Key Vault has been set up and some secrets added to it, AppSecrets provides a simple API for retrieving them inside your Application. A conventional implementation is provided via `AppSecretsManager`. For more options, use the `AppSecrets` class instead.

```csharp
using Scale.AppSecrets;
public async Task<string> GetStorageConnectionString()
{
    return await AppSecretsManager.GetSecret("AZURE-STORAGE-CONNECTION-STRING");
}
```

`AppSecretsManager` looks for three AppSettings in your config file: 

```xml
<appSettings>
    <add key="Scale.AppSecrets.AppSecretsManager.VaultUrl" value="{Fully qualified Vault URL}"/>
    <add key="Scale.AppSecrets.AppSecretsManager.ClientId" value="{Azure AD Client ID}"/>
    <add key="Scale.AppSecrets.AppSecretsManager.ClientSecret" value="{Azure AD Client Secret}"/>
  </appSettings>
```

If you prefer to manage your own config use the `AppSecrets` class instead.


## Setting up Azure Key Vault

There are a couple of really good blogs on this:

* [Azure Key Vault - Step by Step](http://blogs.technet.com/b/kv/archive/2015/01/09/azure-key-vault-step-by-step.aspx)
* [Securing Azure Web Job Secrets with Azure Key Vault](http://wp.sjkp.dk/securing-azure-web-job-secrets-with-azure-key-vault/)

This project currently requires you to create a "Client Key" (Client Secret) for your App in Azure AD. Hoping to support other Client Credentials in a future version. Here are the high level steps *(will fill in more detail later)*:

**In the Full Azure Portal**

1. Create an Azure AD Application for your App. Use the Web app type so that you can create a Client Key (Client Secret).
1. Copy the Client ID and the Client Key (secret).

**In PowerShell**

1. Install [Azure PowerShell](http://azure.microsoft.com/en-us/documentation/articles/install-configure-powershell/) version 0.8.13 or higher.
1. Download [Key Vault Manager Cmdlets](http://go.microsoft.com/fwlink/?LinkID=521539).
1. `Add-AzureAccount`
1. `Import-Module .\KeyVaultManager`
1. `Switch-AzureMode AzureResourceManager`
1. `New-AzureKeyVault -VaultName 'MyDevVault' -Location 'West Europe' -ResourceGroupName 'MyDevRG'`    *- Creates a Vault and Resource Group for it. Only some Locations work in preview.*
1. `Set-AzureKeyVaultAccessPolicy -vaultname 'MyDevVault' -ServicePrincipalName {{Copy from output of previous}} -PermissionsToSecrets get -PermissionsToKeys wrapkey,unwrapkey,decrypt,encrypt`
1. `Add-AzureKeyVaultKey -Name 'MyRSAKey1' -VaultName 'MyDevVault' -Destination 'Software'`    *- Creates an encryption key in Software (in Azure).* 
1. `$Secret = ConvertTo-SecureString -String 'This is secret text' -AsPlainText -Force`    *- Creates a SecureString*
1. `Set-AzureKeyVaultSecret -VaultName 'MyDevVault' -Name 'Secret1' -SecretValue $Secret1`    *- Sets a Secret name and value.*

## Questions an support
@DanielLarsenNZ or create an Issue. Contributions welcome.
