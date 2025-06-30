using BrainHope.Services.DTO.Authentication.SingUp;
using TrackRoom.DataAccess.Models;
using TrackRoom.Services.DTOs.Responses;

namespace TrackRoom.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<TokenType>> RegisterUserAsync(RegisterUser registerUser);

        Task<ApiResponse<LoginResponse>> GetJwtTokenAsync(ApplicationUser user);
    }
}
