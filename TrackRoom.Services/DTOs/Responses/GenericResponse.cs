﻿namespace TrackRoom.Services.DTOs.Responses
{
    public class GenericResponse<T> : Response
    {
        public T Data { get; set; }
    }
}
