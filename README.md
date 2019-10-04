# PassKit Helper

Helper library for all your Apple PassKit (Apple Wallet, Apple Passbook) needs: create passes, sign pass packages, receive [de]install notifications and send pass updates.

**Attention:** Apple Developer Account required!

[![NuGet](https://img.shields.io/nuget/v/PassKitHelper.svg?maxAge=86400&style=flat)](https://www.nuget.org/packages/PassKitHelper/) 

## Features

1. Create pass packages (`*.pkpass` files):
    * With Fluent-styled `PassInfoBuilder` and `PassPackageBuilder`
    * Using `byte[]` and/or `Stream` as content images
    * Using `byte[]` or `Stream` or `X509Certificate2` as certificates
    * Receive `MemoryStream` as result (save it to file or write to HttpResponse)
2. Receive notifications from Apple about pass [de]installations and send updates:
    * Add `UsePassKitMiddleware` into your `Startup.Configure()`
    * Implement `IPassKitService` for real processing.

## Samples

### 1. Creating pass package file

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
var passPfxPassword = "password-to-your-pfx-file";

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

### 2. Implementing WebService for interaction

#### 2.1. Implement IPassKitService

```csharp
public class PassKitService : IPassKitService
{
    public Task<int> RegisterDeviceAsync(…) {…}

    public Task<int> UnregisterDeviceAsync(…) {…}

    public Task<(int status, string[]? passes, string? tag)> GetAssociatedPassesAsync(…) {…}

    public Task<(int statusCode, MemoryStream? passData)> GetPassAsync(…) {…}

    public Task ProcessLogsAsync(…) {…}
}
```

#### 2.2. Register in `Startup`

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddSingleton<IPassService, PassService>();
}

public void Configure(IApplicationBuilder app)
{
    ...
    app.UsePassKitMiddleware("/callbacks/passkit");
    ...
}
```


Done! Now you can add `WebService` section when building your pass:
```csharp
var pass = new PassInfoBuilder()
    ...
    .WebService
        .AuthenticationToken(someAuthenticationToken)
        .WebServiceURL("https://example.com/callbacks/passkit")
```

## Installation

Use NuGet package [PassKitHelper](https://www.nuget.org/packages/PassKitHelper/)

## Dependencies

* Microsoft.AspNetCore.Http.Abstractions, v2.1.1
* Microsoft.Extensions.DependencyInjection.Abstractions, v2.1.1
* Microsoft.Extensions.Logging.Abstractions, v2.1.1
* Newtonsoft.Json, v12.0.2
* System.Security.Cryptography.Pkcs, v4.6.0

## Dvelopment & Testing

You need `netcore3.0` to run build and tests;

Tests can be run with `dotnet test`.

## Credits

* Thanks for inspiration to Tomas McGuinness and his [dotnet-passbook](https://github.com/tomasmcguinness/dotnet-passbook) package.
* Thanks to [maxpixel.net](https://www.maxpixel.net) for sample kitten images.