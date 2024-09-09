using Domain.Enum;
using System;


namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ResetToken { get; set; } = default!;
    }
}
