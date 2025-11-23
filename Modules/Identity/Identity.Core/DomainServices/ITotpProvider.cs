namespace Identity.Core.DomainServices
{
    /// <summary>
    /// Abstraction over TOTP implementation so we can swap libraries later.
    /// </summary>
    public interface ITotpProvider
    {
        string GenerateSecret();
        string GenerateQrCode(string email, string secret);
        bool ValidateCode(string secret, string code);
    }
}
