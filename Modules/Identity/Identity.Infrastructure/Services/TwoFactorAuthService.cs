using System;
using System.Linq;
using System.Threading.Tasks;
using Identity.Application.DTO;
using Identity.Application.Interfaces;
using Identity.Core.DomainServices;
using Identity.Core.Repositories;
using Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Services
{
    public class TwoFactorAuthService : ITwoFactorAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly TwoFactorDomainService _twoFactorDomain;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecretEncryptionService _encryption;

        public TwoFactorAuthService(
            IUserRepository userRepository,
            TwoFactorDomainService twoFactorDomain,
            UserManager<ApplicationUser> userManager,
            ISecretEncryptionService encryption
        )
        {
            _userRepository = userRepository;
            _twoFactorDomain = twoFactorDomain;
            _userManager = userManager;
            _encryption = encryption;
        }

        public async Task<TwoFASetupResult> EnableTwoFactorAsync(string userId)
        {
            var guid = Guid.Parse(userId);

            var domainUser = await _userRepository.GetByIdAsync(guid);
            if (domainUser == null)
                throw new InvalidOperationException("Domain user not found.");

            var identityUser = await EnsureIdentityUserExistsAsync(userId, domainUser);

            var (secret, qrCode) = _twoFactorDomain.GenerateSetupFor(domainUser.Username);

            var encrypted = _encryption.Encrypt(secret);

            identityUser.TwoFactorSecretEncrypted = encrypted;
            identityUser.TwoFactorEnabled = false; // not confirmed yet
            await _userManager.UpdateAsync(identityUser);

            // Return raw secret + QR to frontend
            return new TwoFASetupResult(secret, qrCode);
        }
        public async Task<TwoFAVerificationResult> VerifySetupAsync(string userId, string code)
        {
            var user = await GetOrCreateIdentityUserAsync(userId);
            if (user == null)
                return new(false, "User does not exist.");

            if (string.IsNullOrEmpty(user.TwoFactorSecretEncrypted))
                return new(false, "Two-factor authentication has not been initialized for this user.");

            var secret = _encryption.Decrypt(user.TwoFactorSecretEncrypted);

            bool ok = _twoFactorDomain.VerifyCode(secret, code);

            if (!ok)
                return new(false, "The verification code is invalid or has expired. Please try again.");

            user.TwoFactorEnabled = true;
            await _userManager.UpdateAsync(user);

            return new(true, "Two-factor authentication has been successfully activated.");
        }

        public async Task<TwoFAVerificationResult> VerifyLoginAsync(string userId, string code)
        {
            var user = await GetOrCreateIdentityUserAsync(userId);
            if (user == null)
                return new(false, "User does not exist.");

            if (!user.TwoFactorEnabled)
                return new(false, "Two-factor authentication is not enabled for this user.");

            var secret = _encryption.Decrypt(user.TwoFactorSecretEncrypted);

            bool ok = _twoFactorDomain.VerifyCode(secret, code);

            if (!ok)
                return new(false, "Invalid code. Please try again.");

            return new(true, "Login successful.");
        }

        // TODO: remove this and create ApplicationUser on POST /api/users 
        private async Task<ApplicationUser> EnsureIdentityUserExistsAsync(string userId, Identity.Core.Entities.User domainUser)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser != null)
                return identityUser;

            identityUser = new ApplicationUser
            {
                Id = userId,
                UserName = domainUser.Username,
                PasswordHash = domainUser.PasswordHash,
                TwoFactorEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var createResult = await _userManager.CreateAsync(identityUser);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to provision identity user: {errors}");
            }

            return identityUser;
        }

        private async Task<ApplicationUser?> GetOrCreateIdentityUserAsync(string userId)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser != null)
                return identityUser;

            var guid = Guid.Parse(userId);
            var domainUser = await _userRepository.GetByIdAsync(guid);
            if (domainUser == null)
                return null;

            return await EnsureIdentityUserExistsAsync(userId, domainUser);
        }
    }
}
