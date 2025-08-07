using MediatR;
using Microsoft.AspNetCore.Mvc;
using Task_Management.Application.Dtos;
using Task_Management.Application.User.CreateUser;
using Task_Management.Application.User.LoginUser;
using Task_Management.Application.User.refreshToken;
using Task_Management.Application.User.RevokeToken;

namespace Task_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUserCommand command)
        {
            AuthGeneralResponse Result = await _mediator.Send(command);
            return Ok(Result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            AuthGeneralResponse Result = await _mediator.Send(command);
            return Ok(Result);
        }

        [HttpPost("RevokeRefreshToken")]
        public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenCommand command = null!)
        {
            if (command is null)
            {
                command = new RevokeRefreshTokenCommand();
                command.Token = Request.Cookies["RefreshToken"] ?? String.Empty;
            }

            bool Result = await _mediator.Send(command);
            return Result ? Ok("Refresh token revoked successfully") : BadRequest("Invalid or expired refresh token");
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command = null!)
        {
            if (command is null)
            {
                command = new RefreshTokenCommand();
                command.Token = Request.Cookies["RefreshToken"] ?? String.Empty;
            }

            AuthGeneralResponse Result = await _mediator.Send(command);
            return Ok(Result);
        }
    }
}
