namespace PassKitHelper
{
    public static class PassBuilderNfcKeysExtensions
    {
        /// <summary>
        /// Optional. Information used for Value Added Service Protocol transactions.
        /// </summary>
        /// <remarks>
        /// Available in iOS 9.0.
        /// </remarks>
        public static PassBuilder.PassBuilderNfcKeys Nfc(this PassBuilder.PassBuilderNfcKeys builder, PassBuilder.Nfc value)
        {
            builder.AppendValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Information used for Value Added Service Protocol transactions.
        /// </summary>
        /// <remarks>
        /// Available in iOS 9.0.
        /// </remarks>
        public static PassBuilder.PassBuilderNfcKeys Nfc(this PassBuilder.PassBuilderNfcKeys builder, string message, string? encryptionPublicKey)
        {
            var value = new PassBuilder.Nfc(message)
            {
                EncryptionPublicKey = encryptionPublicKey,
            };
            return Nfc(builder, value);
        }
    }
}
