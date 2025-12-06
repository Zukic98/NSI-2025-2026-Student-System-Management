namespace Identity.Application.DTO
{
    public class TwoFASetupResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string QrCode { get; set; } = string.Empty;

        public TwoFASetupResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public TwoFASetupResult(bool success, string secret, string qrCode)
        {
            Success = success;
            Secret = secret;
            QrCode = qrCode;
        }

        public TwoFASetupResult(bool success, string message, string secret, string qrCode)
        {
            Success = success;
            Message = message;
            Secret = secret;
            QrCode = qrCode;
        }
    }
}
