﻿using Application.Features.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Interfaces.IService
{
    public interface IEmailService
    {
        Task SendEmail(EmailRequestModel mailRequest);
        Task SendWelcomeMessage(string email, string name, string code);
    }
}
