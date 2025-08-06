using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Task_Management.Application.Dtos;

namespace Task_Management.Application.User.LoginUser
{
    public class LoginUserCommand : IRequest<AuthGeneralResponse>
    {
        public string UserNameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
