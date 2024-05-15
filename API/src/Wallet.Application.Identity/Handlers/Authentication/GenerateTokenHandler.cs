using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Wallet.Application.Identity.Commands.Authentication;
using Wallet.Domain.Entities.Model;
using Wallet.Shared.DataTransferObjects;
using SigningCredentials = Microsoft.IdentityModel.Tokens.SigningCredentials;

namespace Wallet.Application.Identity.Handlers.Authentication;

internal sealed class GenerateTokenHandler(IConfiguration configuration, UserManager<WalletIdentityUser> userManager) : IRequestHandler<GenerateTokenCommand, TokenDto> {
    public async Task<TokenDto> Handle(GenerateTokenCommand request, CancellationToken cancellationToken) {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(request.UserForAuthenticationDto);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var refreshToken = GenerateRefreshToken();
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        var tokenDto = new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken };

        return tokenDto;
    }

    private static SigningCredentials GetSigningCredentials() {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!);
        var secretKey = new SymmetricSecurityKey(key);
        return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims(UserForAuthenticationDto userForAuthenticationDto)
    {
        var user = await userManager.FindByNameAsync(userForAuthenticationDto.UserName!) ?? throw new UnauthorizedAccessException("Invalid Authentication");
        var claims = new List<Claim> {
            new Claim("user_name", user.UserName!)
        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim("Role", role)));
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims) {
        var tokenOptions = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpiryInMinutes"])),
            signingCredentials: signingCredentials);

        return tokenOptions;
    }

    private static string GenerateRefreshToken() {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}