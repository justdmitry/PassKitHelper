# PassKit Helper

Helper library for all your Apple PassKit (Apple Wallet, Apple Passbook) needs: create passes, sign pass packages.

**Attention:** Apple Developer Account required to create passes.

[![NuGet](https://img.shields.io/nuget/v/PassKitHelper.svg?maxAge=86400&style=flat)](https://www.nuget.org/packages/PassKitHelper/) 

## Sample usage

```csharp
JObject pass = new PassInfoBuilder()
    .Standard
        .PassTypeIdentifier("your-pass-type-identifier")
        .SerialNumber("PassKitHelper")
        .TeamIdentifier("your-team-identifier")
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
var passBytes = await File.ReadAllBytesAsync("pass.pfx");

MemoryStream package = await new PassPackageBuilder(pass)
    .Icon(await File.ReadAllBytesAsync("images/icon.png"))
    .Icon2X(await File.ReadAllBytesAsync("images/icon@2x.png"))
    .Logo(await File.ReadAllBytesAsync("images/logo.jpg"))
    .Strip(await File.ReadAllBytesAsync("images/strip.jpg"))
    .Strip2X(await File.ReadAllBytesAsync("images/strip@2x.jpg"))
    .SignAndBuildAsync(appleBytes, passBytes, passPfxPassword);

await File.WriteAllBytesAsync("Sample.pkpass", package.ToArray());
```

Code above will create this beautiful pass:

![](sample_pass.jpg)

## Installation

Use NuGet package [PassKitHelper](https://www.nuget.org/packages/PassKitHelper/)

## Dependencies

* Newtonsoft.Json, v12.0.2
* System.Security.Cryptography.Pkcs, v4.5.2

## Testing

Tests can be run with `dotnet test`.

## Credits

* Thanks for inspiration to Tomas McGuinness and his [dotnet-passbook](https://github.com/tomasmcguinness/dotnet-passbook) package.
* Thanks to [maxpixel.net](https://www.maxpixel.net) for sample kitten images.