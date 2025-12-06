using System.Linq;
using System.Text;
using Identity.Core.Configuration;
using Identity.Core.DomainServices;
using Microsoft.Extensions.Options;
using OtpNet;
using QRCoder;

namespace Identity.Infrastructure.TOTP
{
    /// <summary>
    /// Real TOTP provider backed by Otp.NET.
    /// Handles secret generation, otpauth URI formatting, QR encoding and verification.
    /// </summary>
    public class TotpProvider : ITotpProvider
    {
        private readonly TotpSettings _settings;
        private const string DataUriPrefix = "data:image/png;base64,";

        public TotpProvider(IOptions<TotpSettings> settings)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public string GenerateSecret()
        {
            var secretLength = _settings.SecretLength <= 0 ? 20 : _settings.SecretLength;
            // Generates a cryptographically-secure random key and encodes it in Base32.
            var keyBytes = KeyGeneration.GenerateRandomKey(secretLength);
            return Base32Encoding.ToString(keyBytes);
        }

        public TotpSetupArtifacts GenerateSetupArtifacts(string username, string secret)
        {
            var otpauthUri = BuildOtpAuthUri(username, secret);

            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(otpauthUri, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);

            var pixelsPerModule = _settings.QrPixelsPerModule <= 0 ? 10 : _settings.QrPixelsPerModule;
            var qrBytes = qrCode.GetGraphic(pixelsPerModule);

            var qrBase64 = $"{DataUriPrefix}{Convert.ToBase64String(qrBytes)}";
            return new TotpSetupArtifacts(otpauthUri, qrBase64);
        }

        public bool ValidateCode(string secret, string code)
        {
            if (string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(code))
                return false;

            var sanitizedCode = new string(code.Trim().Where(char.IsDigit).ToArray());
            if (sanitizedCode.Length != _settings.Digits)
                return false;

            try
            {
                var secretBytes = Base32Encoding.ToBytes(secret);
                var totp = new Totp(
                    secretBytes,
                    step: _settings.StepSeconds <= 0 ? 30 : _settings.StepSeconds,
                    totpSize: _settings.Digits);

                return totp.VerifyTotp(
                    sanitizedCode,
                    out _,
                    new VerificationWindow(previous: 1, future: 1));
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private string BuildOtpAuthUri(string username, string secret)
        {
            var issuerValue = string.IsNullOrWhiteSpace(_settings.Issuer)
                ? "StudentSystem"
                : _settings.Issuer;
            var issuer = Uri.EscapeDataString(issuerValue);

            var labelValue = string.IsNullOrWhiteSpace(username)
                ? issuerValue
                : $"{issuerValue}:{username}";
            var label = Uri.EscapeDataString(labelValue);

            var builder = new StringBuilder();
            builder.Append("otpauth://totp/")
                .Append(label)
                .Append("?secret=").Append(secret)
                .Append("&issuer=").Append(issuer)
                .Append("&digits=").Append(_settings.Digits)
                .Append("&period=").Append(_settings.StepSeconds <= 0 ? 30 : _settings.StepSeconds);

            return builder.ToString();
        }
    }
}
