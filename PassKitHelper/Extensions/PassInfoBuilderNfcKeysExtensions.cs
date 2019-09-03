namespace PassKitHelper
{
    public static class PassInfoBuilderNfcKeysExtensions
    {
        /// <summary>
        /// Optional. Information used for Value Added Service Protocol transactions.
        /// </summary>
        /// <remarks>
        /// Available in iOS 9.0.
        /// </remarks>
        public static PassInfoBuilder.PassInfoBuilderNfcKeys Nfc(this PassInfoBuilder.PassInfoBuilderNfcKeys builder, PassInfoBuilder.Nfc value)
        {
            builder.AppendValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Information used for Value Added Service Protocol transactions.
        /// </summary>
        /// <remarks>
        /// Available in iOS 9.0.
        /// </remarks>
        public static PassInfoBuilder.PassInfoBuilderNfcKeys Nfc(this PassInfoBuilder.PassInfoBuilderNfcKeys builder, string message, string? encryptionPublicKey)
        {
            var value = new PassInfoBuilder.Nfc(message)
            {
                EncryptionPublicKey = encryptionPublicKey,
            };
            return Nfc(builder, value);
        }
    }
}
