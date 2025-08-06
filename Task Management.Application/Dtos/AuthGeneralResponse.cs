using System;

namespace Task_Management.Application.Dtos
{
    public class AuthGeneralResponse
    {
        public string FullName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationOn { get; set; }
        public dynamic? Data { get; set; }
    }
}
