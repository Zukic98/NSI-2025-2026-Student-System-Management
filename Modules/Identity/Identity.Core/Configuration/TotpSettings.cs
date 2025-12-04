namespace Identity.Core.Configuration
{
    /// <summary>
    /// Configuration for generating and validating TOTP codes.
    /// Defaults align with Google Authenticator expectations.
    /// </summary>
    public class TotpSettings
    {
        /// <summary>
        /// Display name shown inside authenticator apps.
        /// </summary>
        public string Issuer { get; set; } = "StudentSystem";

        /// <summary>
        /// Number of seconds per TOTP window.
        /// </summary>
        public int StepSeconds { get; set; } = 30;

        /// <summary>
        /// Number of digits expected in the generated code.
        /// </summary>
        public int Digits { get; set; } = 6;

        /// <summary>
        /// Length of the random secret in bytes before Base32 encoding.
        /// Google Authenticator works well with 160+ bit secrets.
        /// </summary>
        public int SecretLength { get; set; } = 20;

        /// <summary>
        /// Controls QR-code resolution (pixel size) – tweak for UI needs.
        /// </summary>
        public int QrPixelsPerModule { get; set; } = 10;
    }
}
