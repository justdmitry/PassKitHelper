namespace PassKitHelper
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    public class Program
    {
        /// <summary>
        /// Identifiers from your Apple developer account.
        /// </summary>
        private const string PassTypeIdentifier = "pass.com.apple.devpubs.example";
        private const string TeamIdentifier = "A93A5CM278";

        /// <summary>
        /// Filename and password for your PFX file.
        /// </summary>
        /// <remarks>See `hot_to_create_pfx.md` file for instructions how to obtain your PFX file.</remarks>
        private const string PassPfxFile = "pass.pfx";
        private const string PassPfxPassword = "";

        /// <summary>
        /// Valud `pushToken` value from pass registration (<see cref="IPassKitService.RegisterDeviceAsync"/>).
        /// </summary>
        private const string RegisterationPushToken = "1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef";

        public static async Task Main()
        {
            await GenerateSamplePass();

            //// uncomment to send push notification
            //// attention! your pass (you push update for) must already include WebService fields (auth.token and endpoint), and your web service/app must be up and running
            // await SendEmptyPushNotification(RegisterationPushToken);
        }

        public static async Task GenerateSamplePass()
        {
            var libraryVersion = typeof(PassInfoBuilder).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            var pass = new PassInfoBuilder()
                .Standard
                    .PassTypeIdentifier(PassTypeIdentifier)
                    .SerialNumber("PassKitHelper")
                    .TeamIdentifier(TeamIdentifier)
                    .OrganizationName("PassKit")
                    .Description("PassKitHelper demo pass")
                //.WebService
                //    .AuthenticationToken("some-nonce")
                //    .WebServiceURL("https://www.example.com/")
                .VisualAppearance
                    .Barcodes("1234567890128", BarcodeFormat.Code128)
                    .LogoText("PassKit Helper demo pass")
                    .ForegroundColor("rgb(44, 62, 80)")
                    .BackgroundColor("rgb(149, 165, 166)")
                    .LabelColor("rgb(236, 240, 241)")
                .StoreCard
                    .PrimaryFields
                        .Add("version")
                            .Label("Library version")
                            .Value(libraryVersion)
                    .AuxiliaryFields
                        .Add("github")
                            .Label("GitHub link")
                            .Value("https://github.com/justdmitry/PassKitHelper")
                .Build();

            var appleBytes = await File.ReadAllBytesAsync("AppleWWDRCA.cer");
            var passBytes = await File.ReadAllBytesAsync(PassPfxFile);

            var package = await new PassPackageBuilder(pass)
                .Icon(await File.ReadAllBytesAsync("images/icon.png"))
                .Icon2X(await File.ReadAllBytesAsync("images/icon@2x.png"))
                .Logo(await File.ReadAllBytesAsync("images/logo.jpg"))
                .Strip(await File.ReadAllBytesAsync("images/strip.jpg"))
                .Strip2X(await File.ReadAllBytesAsync("images/strip@2x.jpg"))
                .SignAndBuildAsync(appleBytes, passBytes, PassPfxPassword);

            await File.WriteAllBytesAsync("Sample.pkpass", package.ToArray());
        }

        public static async Task SendEmptyPushNotification(string pushToken)
        {
            var cert = string.IsNullOrEmpty(PassPfxFile)
                ? new X509Certificate2(await File.ReadAllBytesAsync(PassPfxFile))
                : new X509Certificate2(await File.ReadAllBytesAsync(PassPfxFile), PassPfxPassword);

            using var clientHandler = new HttpClientHandler();
            clientHandler.ClientCertificates.Add(cert);
            clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;

            using var content = new StringContent("{\"aps\":\"\"}");
            using var client = new HttpClient(clientHandler);

            var req = new HttpRequestMessage(HttpMethod.Post, $"https://api.push.apple.com/3/device/{pushToken}")
            {
                Version = new Version(2, 0),
                Content = content,
            };

            using var response = await client.SendAsync(req);

            var responseText = await response.Content.ReadAsStringAsync();

            Console.WriteLine(response.IsSuccessStatusCode ? "SUCCESS. Response:" : "FAILED. Response:");
            Console.WriteLine(responseText);
        }
    }
}
