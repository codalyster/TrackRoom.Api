﻿using BrainHope.Services.DTO.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackRoom.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
