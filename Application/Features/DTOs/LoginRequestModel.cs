using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DTOs
{
    public class LoginRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
