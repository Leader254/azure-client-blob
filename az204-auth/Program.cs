using Microsoft.Identity.Client;
using System.Threading.Tasks;
using Azure.Identity;

namespace az204_auth
{
    class Program
    {
        private const string _clientId = "a9d5dcef-e32d-454b-8595-6d101a361b1b";
        private const string _tenantId = "4f0e79a2-3835-405a-aec9-8c1c4fc7ef53";

        public static async Task Main(string[] args)
        {
            var client = new SecretClient(new Uri("https://myvault.vault.azure.net/"), new DefaultAzureCredential)
            var app = PublicClientApplicationBuilder
                .Create(_clientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
                .WithRedirectUri("http://localhost")
                .Build();
            string[] scopes = { "user.read" };
            AuthenticationResult result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();

            Console.WriteLine($"Token:\t{result}");
        }
    }

    internal class SecretClient
    {
        private Uri uri;
        private DefaultAzureCredential defaultAzureCredential;

        public SecretClient(Uri uri, DefaultAzureCredential defaultAzureCredential)
        {
            this.uri = uri;
            this.defaultAzureCredential = defaultAzureCredential;
        }
    }
}