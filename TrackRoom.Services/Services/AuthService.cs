﻿using BrainHope.Services.DTO.Authentication.SingUp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TrackRoom.DataAccess.Contexts;
using TrackRoom.DataAccess.Models;
using TrackRoom.Services.DTOs.Responses;
using TrackRoom.Services.Interfaces;
using TrackRoom.Utilities;
using Utilites;

namespace TrackRoom.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
            _context = context;
        }

        public async Task<ApiResponse<TokenType>> RegisterUserAsync(RegisterUser registerUser)
        {
            var existingByEmail = await _userManager.FindByEmailAsync(registerUser.Email);
            if (existingByEmail != null)
                return new ApiResponse<TokenType>
                {
                    IsSuccess = false,
                    Message = "Email already registered.",
                    StatusCode = 400
                };

            var existingByUsername = await _userManager.FindByNameAsync(registerUser.UserName);
            if (existingByUsername != null)
                return new ApiResponse<TokenType>
                {
                    IsSuccess = false,
                    Message = "Username is already taken.",
                    StatusCode = 400
                };

            var user = new ApplicationUser
            {
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                SecurityStamp = Guid.NewGuid().ToString(), // to ensure the user is treated as new
                TwoFactorEnabled = true,
                ProfilePictureUrl = registerUser.ProfilePicture != null
                    ? await ImageHelper
                    .SaveImageAsync(registerUser.ProfilePicture)
                    : null,
                Bio = registerUser.Bio != null ? registerUser.Bio : string.Empty,
            };

            var createResult = await _userManager.CreateAsync(user, registerUser.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return new ApiResponse<TokenType>
                {
                    IsSuccess = false,
                    Message = $"User creation failed: {errors}",
                    StatusCode = 400
                };
            }

            if (!await _roleManager.RoleExistsAsync(SD.Role_User))
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_User));

            await _userManager.AddToRoleAsync(user, SD.Role_User);

            // Generate email confirmation token
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // هنا رجّعنا التوكين
            return new ApiResponse<TokenType>
            {
                IsSuccess = true,
                Message = "User created successfully. Please confirm your email.",
                StatusCode = 200,
                Response = new TokenType
                {
                    Token = emailToken,
                    ExpiryTokenDate = DateTime.Now.AddHours(1) // أو أي وقت مناسب
                }
            };
        }


        public async Task<ApiResponse<LoginResponse>> GetJwtTokenAsync(ApplicationUser user)
        {
            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtToken = GetToken(authClaims);
            var refreshToken = GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidity"], out int refreshTokenValidity);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenValidity);

            await _userManager.UpdateAsync(user);

            return new ApiResponse<LoginResponse>
            {
                Response = new LoginResponse
                {
                    AccessToken = new TokenType
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        ExpiryTokenDate = jwtToken.ValidTo
                    },
                    RefreshToken = new TokenType
                    {
                        Token = user.RefreshToken,
                        ExpiryTokenDate = (DateTime)user.RefreshTokenExpiry
                    }
                },
                IsSuccess = true,
                StatusCode = 200,
                Message = "Token created"
            };
        }

        #region Private Methods

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            var expirationTimeUtc = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);
            var localTimeZone = TimeZoneInfo.Local;
            var expirationTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(expirationTimeUtc, localTimeZone);

            return new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: expirationTimeInLocalTimeZone,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        #endregion
    }

}
