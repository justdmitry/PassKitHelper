namespace PassKitHelper
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main()
        {
            const string passTypeIdentifier = "pass.com.apple.devpubs.example";
            const string teamIdentifier = "A93A5CM278";
            const string passPfxFile = "pass.pfx";
            const string passPfxPassword = null;

            var libraryVersion = typeof(PassInfoBuilder).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            var pass = new PassInfoBuilder()
                .Standard
                    .PassTypeIdentifier(passTypeIdentifier)
                    .SerialNumber("PassKitHelper")
                    .TeamIdentifier(teamIdentifier)
                    .OrganizationName("PassKit")
                    .Description("PassKitHelper demo pass")
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
            var passBytes = await File.ReadAllBytesAsync(passPfxFile);

            var package = await new PassPackageBuilder(pass)
                .Icon(await File.ReadAllBytesAsync("images/icon.png"))
                .Icon2X(await File.ReadAllBytesAsync("images/icon@2x.png"))
                .Logo(await File.ReadAllBytesAsync("images/logo.jpg"))
                .Strip(await File.ReadAllBytesAsync("images/strip.jpg"))
                .Strip2X(await File.ReadAllBytesAsync("images/strip@2x.jpg"))
                .SignAndBuildAsync(appleBytes, passBytes, passPfxPassword);

            await File.WriteAllBytesAsync("Sample.pkpass", package.ToArray());
        }
    }
}
