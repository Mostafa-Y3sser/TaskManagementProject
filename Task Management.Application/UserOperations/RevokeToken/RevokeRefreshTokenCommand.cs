using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Task_Management.Application.UserOperations.RevokeToken
{
    public class RevokeRefreshTokenCommand : IRequest<bool>
    {
        public string Token { get; set; } = string.Empty;
    }
}
