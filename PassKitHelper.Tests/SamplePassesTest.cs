namespace PassKitHelper
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Sample pass.jon files from developer docs/samples.
    /// </summary>
    /// <remarks>
    /// Changes:
    /// 1) 'barcode' changed to 'barcodes'
    /// 2) seconds (:00) added to Event / relevantDate.
    /// </remarks>
    public class SamplePassesTest
    {
        private readonly ITestOutputHelper output;

        public SamplePassesTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Coupon()
        {
            var actual = new PassInfoBuilder()
                .Standard
                    .PassTypeIdentifier("pass.com.apple.devpubs.example")
                    .SerialNumber("E5982H-I2")
                    .TeamIdentifier("A93A5CM278")
                    .OrganizationName("Paw Planet")
                    .Description("Paw Planet Coupon")
                .WebService
                    .AuthenticationToken("vxwxd7J8AlNNFPS8k0a0FfUFtq0ewzFdc")
                    .WebServiceURL("https://example.com/passes/")
                .Relevance
                    .Locations(37.6189722, -122.3748889)
                    .Locations(37.33182, -122.03118)
                .VisualAppearance
                    .Barcodes("123456789", BarcodeFormat.Pdf417)
                    .LogoText("Paw Planet")
                    .ForegroundColor("rgb(255, 255, 255)")
                    .BackgroundColor("rgb(206, 140, 53)")
                .Coupon
                    .PrimaryFields
                        .Add("offer")
                            .Label("Any premium dog food")
                            .Value("20% off")
                    .AuxiliaryFields
                        .Add("expires")
                            .Label("EXPIRES")
                            .Value("2013-04-24T10:00-05:00")
                            .IsRelative(true)
                            .DateStyle(DateStyle.Short)
                .Build();

            output.WriteLine("Actual JSON:");
            output.WriteLine(actual.ToString(Formatting.Indented));

            var expected = LoadFromResource("Coupon.pass.json");

            var jdp = new JsonDiffPatchDotNet.JsonDiffPatch();
            var diff = jdp.Diff(expected, actual);

            output.WriteLine("Differences (expected -> actual):");
            output.WriteLine(diff == null ? "No differences!" : diff.ToString());

            Assert.True(JToken.DeepEquals(expected, actual));
        }

        [Fact]
        public void Event()
        {
            var actual = new PassInfoBuilder()
                .Standard
                    .PassTypeIdentifier("pass.com.apple.devpubs.example")
                    .SerialNumber("nmyuxofgna")
                    .TeamIdentifier("A93A5CM278")
                    .OrganizationName("Apple Inc.")
                    .Description("Apple Event Ticket")
                .WebService
                    .AuthenticationToken("vxwxd7J8AlNNFPS8k0a0FfUFtq0ewzFdc")
                    .WebServiceURL("https://example.com/passes/")
                .Relevance
                    .Locations(37.6189722, -122.3748889)
                    .Locations(37.33182, -122.03118)
                    .RelevantDate(new DateTimeOffset(2011, 12, 8, 13, 0, 0, TimeSpan.FromHours(-8)))
                .VisualAppearance
                    .Barcodes("123456789", BarcodeFormat.Pdf417)
                    .ForegroundColor("rgb(255, 255, 255)")
                    .BackgroundColor("rgb(60, 65, 76)")
                .EventTicket
                    .PrimaryFields
                        .Add("event")
                            .Label("EVENT")
                            .Value("The Beat Goes On")
                    .SecondaryFields
                        .Add("loc")
                            .Label("LOCATION")
                            .Value("Moscone West")
                .Build();

            output.WriteLine("Actual JSON:");
            output.WriteLine(actual.ToString(Formatting.Indented));

            var expected = LoadFromResource("Event.pass.json");

            var jdp = new JsonDiffPatchDotNet.JsonDiffPatch();
            var diff = jdp.Diff(expected, actual);

            output.WriteLine("Differences (expected -> actual):");
            output.WriteLine(diff == null ? "No differences!" : diff.ToString());

            Assert.True(JToken.DeepEquals(expected, actual));
        }

        [Fact]
        public void StoreCard()
        {
            var actual = new PassInfoBuilder()
                .Standard
                    .PassTypeIdentifier("pass.com.apple.devpubs.example")
                    .SerialNumber("p69f2J")
                    .TeamIdentifier("A93A5CM278")
                    .OrganizationName("Organic Produce")
                    .Description("Organic Produce Loyalty Card")
                .WebService
                    .AuthenticationToken("vxwxd7J8AlNNFPS8k0a0FfUFtq0ewzFdc")
                    .WebServiceURL("https://example.com/passes/")
                .Relevance
                    .Locations(37.6189722, -122.3748889)
                .VisualAppearance
                    .Barcodes("123456789", BarcodeFormat.Pdf417)
                    .LogoText("Organic Produce")
                    .ForegroundColor("rgb(255, 255, 255)")
                    .BackgroundColor("rgb(55, 117, 50)")
                .StoreCard
                    .PrimaryFields
                        .Add("balance")
                            .Label("remaining balance")
                            .Value(21.75)
                            .CurrencyCode("USD")
                    .AuxiliaryFields
                        .Add("deal")
                            .Label("Deal of the Day")
                            .Value("Lemons")
                .Build();

            output.WriteLine("Actual JSON:");
            output.WriteLine(actual.ToString(Formatting.Indented));

            var expected = LoadFromResource("StoreCard.pass.json");

            var jdp = new JsonDiffPatchDotNet.JsonDiffPatch();
            var diff = jdp.Diff(expected, actual);

            output.WriteLine("Differences (expected -> actual):");
            output.WriteLine(diff == null ? "No differences!" : diff.ToString());

            Assert.True(JToken.DeepEquals(expected, actual));
        }

        private JObject LoadFromResource(string name)
        {
            using var stream = this.GetType().Assembly.GetManifestResourceStream($"PassKitHelper.res.{name}");
            var json = new StreamReader(stream).ReadToEnd();
            return JsonConvert.DeserializeObject<JObject>(json, PassInfoBuilder.JsonSettings);
        }
    }
}
