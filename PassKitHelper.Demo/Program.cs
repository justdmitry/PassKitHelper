namespace PassKitHelper
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main()
        {
            var options = new PassKitOptions()
            {
                // See `how_to_create_pfx.md` to create your onw pfx file
                PassCertificate = new X509Certificate2(File.ReadAllBytes("pass.pfx")),
                ConfigureNewPass =
                    p =>
                        p.Standard
                            .PassTypeIdentifier("pass.com.apple.devpubs.example")
                            .TeamIdentifier("A93A5CM278")
                            .OrganizationName("PassKit")
                        .VisualAppearance
                            .LogoText("PassKit Helper demo")
                        .StoreCard
                            .AuxiliaryFields
                                .Add("github")
                                    .Label("GitHub link")
                                    .Value("https://github.com/justdmitry/PassKitHelper"),
            };

            var passKitHelper = new PassKitHelper(options);

            await GenerateSamplePass(passKitHelper);

            //// uncomment to send push notification
            //// attention! your pass (you push update for) must already include WebService fields (auth.token and endpoint), and your web service/app must be up and running
            //// var registrationPushToken = "1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef";
            //// var ok = await passKitHelper.SendPushNotificationAsync(registrationPushToken);
        }

        public static async Task GenerateSamplePass(IPassKitHelper passKitHelper)
        {
            var pass = passKitHelper.CreateNewPass()
                .Standard
                    .SerialNumber("PassKitHelper")
                    .Description("PassKitHelper demo pass")
                ////.WebService
                ////    .AuthenticationToken("some-nonce")
                ////    .WebServiceURL("https://www.example.com/")
                .VisualAppearance
                    .Barcodes("1234567890128", BarcodeFormat.Code128)
                    .ForegroundColor("rgb(44, 62, 80)")
                    .BackgroundColor("rgb(149, 165, 166)")
                    .LabelColor("rgb(236, 240, 241)")
                .StoreCard
                    .PrimaryFields
                        .Add("version")
                            .Label("Library version")
                            .Value(typeof(IPassKitHelper).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion)
                ;

            var package = await passKitHelper.CreateNewPassPackage(pass)
                .Icon(await File.ReadAllBytesAsync("images/icon.png"))
                .Icon2X(await File.ReadAllBytesAsync("images/icon@2x.png"))
                .Icon3X(await File.ReadAllBytesAsync("images/icon@3x.png"))
                .Logo(await File.ReadAllBytesAsync("images/logo.jpg"))
                .Strip(await File.ReadAllBytesAsync("images/strip.jpg"))
                .Strip2X(await File.ReadAllBytesAsync("images/strip@2x.jpg"))
                .Strip3X(await File.ReadAllBytesAsync("images/strip@3x.jpg"))
                .SignAndBuildAsync();

            await File.WriteAllBytesAsync("Sample.pkpass", package.ToArray());
        }
    }
}
