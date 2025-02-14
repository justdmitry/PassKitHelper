namespace PassKitHelper
{
    using System;
    using System.Security.Cryptography.X509Certificates;

    public class PassKitOptions
    {
        /// <summary>
        /// Apple WWDR certificate.
        /// </summary>
        [Obsolete("Not needed anymore. Will be removed in future versions.")]
        public X509Certificate2? AppleCertificate { get; set; }

        /// <summary>
        /// Your pass certificate (with private key).
        /// </summary>
        /// <remarks>Obtain via https://developer.apple.com/account/resources/certificates/list (see `how_to_create_pfx.md` for step-by-step instructions).</remarks>
        public X509Certificate2? PassCertificate { get; set; }

        /// <summary>
        /// This action will be called for each new pass you create via <see cref="IPassKitHelper.CreateNewPass"/>.
        /// </summary>
        public Action<PassBuilder>? ConfigureNewPass { get; set; }

        /// <summary>
        /// This action will be called for each new package you create via <see cref="IPassKitHelper.CreateNewPassPackage(PassBuilder)"/>.
        /// </summary>
        public Action<PassPackageBuilder>? ConfigureNewPassPackage { get; set; }
    }
}
