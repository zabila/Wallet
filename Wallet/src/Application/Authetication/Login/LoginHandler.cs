using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Messaging;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;
using SharedKernel.DTO.Login;

namespace Application.Authetication.Login;

internal sealed class LoginHandler(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager) : ICommandHandler<LoginCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure<TokenResponse>(UserErrors.NotFoundByEmail);
        }
        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Result.Failure<TokenResponse>(UserErrors.Unauthorized());
        }

        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaimsAsync(request.Email);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var refreshToken = GenerateRefreshToken();
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        var token = new TokenResponse { AccessToken = accessToken, RefreshToken = refreshToken };

        return token;
    }

    private static SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!);
        var secretKey = new SymmetricSecurityKey(key);
        return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaimsAsync(string Email)
    {
        var user = await userManager.FindByNameAsync(Email) ?? throw new UnauthorizedAccessException("Invalid Authentication");
        var claims = new List<Claim> {
            new Claim("user_name", user.UserName!)
        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim("Role", role)));
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpiryInMinutes"])),
            signingCredentials: signingCredentials);

        return tokenOptions;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
