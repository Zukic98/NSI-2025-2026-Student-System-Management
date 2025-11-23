using Identity.Core.DomainServices;

namespace Identity.Core.DomainServices
{
    // Domain logika za generisanje TOTP secreta / QR koda i validaciju koda.
    public class TwoFactorDomainService
    {
        private readonly ITotpProvider _totpProvider;

        public TwoFactorDomainService(ITotpProvider totpProvider)
        {
            _totpProvider = totpProvider;
        }

        // Generiše novi secret i "QR code" payload za zadani email.
        public (string Secret, string QrCodeBase64) GenerateSetupFor(string email)
        {
            var secret = _totpProvider.GenerateSecret();
            var qrCode = _totpProvider.GenerateQrCode(email, secret);

            return (secret, qrCode);
        }
        
        public bool VerifyCode(string secret, string code)
        {
            return _totpProvider.ValidateCode(secret, code);
        }
    }
}
