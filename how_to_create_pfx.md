# How to get your own PKCS12 certificate file

## Using OpenSSL

1. Join Apple Developer Program (https://developer.apple.com/account/)
2. Create certificate signing request:
    * Obtain your copy of OpenSSL (see https://www.openssl.org/ and/or https://wiki.openssl.org/index.php/Binaries)
    * Run: `openssl req -new -newkey rsa:2048 -nodes -keyout pass.key -out pass.csr` and answer questions it asks.
3. Obtain certificate:
    * Go to https://developer.apple.com/account/resources/certificates/list and create new certificate, using `pass.csr` file from previous step.
    * Download certificate (`pass.cer`)
4. Convert key and certificate into X509 file:
    * Run `openssl x509 -in pass.cer -inform der -outform pem -out pass.cer.pem` to convert certificate from DER to PEM format
    * Run `openssl pkcs12 -export -out pass.pfx -inkey pass.key -in pass.cer.pem` to combine certificate and key files into one `pfx` file you will need to create passes. Protect it with password to prevent unauthorized usage.
5. Final steps:
    * Delete/remove `pass.cer` and `pass.cer.pem` - they are not needed anymore.
    * Save `pass.csr` and `pass.key` into safe place to use them to re-create certificate when current one will expire (1 year from now by default). Start from step 3 (not from step 1) when this happens.
    * Download fresh `WWDR Certificate` from [Apple website](https://www.apple.com/certificateauthority/) and compare `Authority Key Identifier` in your certificate with `Subject Key Identifier` in WWDR Certificate, they must match.
