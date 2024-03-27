using Microsoft.AspNetCore.Identity;
using Wallet.Domain.Entities.Model;

namespace Wallet.API.Identity.Services;

public class EmailSender : IEmailSender<WalletIdentityUser> {
    public Task SendConfirmationLinkAsync(WalletIdentityUser user, string email, string confirmationLink) {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetLinkAsync(WalletIdentityUser user, string email, string resetLink) {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetCodeAsync(WalletIdentityUser user, string email, string resetCode) {
        throw new NotImplementedException();
    }
}