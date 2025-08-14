using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;

namespace Task_Management.Application.UserOperations.RevokeToken
{
    public class RevokeRefreshTokenHandler : IRequestHandler<RevokeRefreshTokenCommand, bool>
    {
        private readonly IJwtTokenService _jwtTokenService;

        public RevokeRefreshTokenHandler(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public async Task<bool> Handle(RevokeRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            bool IsRevoked = await _jwtTokenService.RevokeTokenAsync(command.Token);
            return IsRevoked;
        }
    }
}
