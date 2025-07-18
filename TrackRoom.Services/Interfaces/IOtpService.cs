﻿namespace TrackRoom.Services.Interfaces
{
    public interface IOtpService
    {
        Task SetOtpAsync(string key, string otp, TimeSpan expiration);
        Task<string> GetOtpAsync(string key);

        Task RemoveOtpAsync(string key);
    }

}
