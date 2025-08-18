using MediatR;
using Microsoft.AspNetCore.Mvc;
using Task_Management.Application.Dtos;
using Task_Management.Application.HomeOperations.GetAllBoards;
using Task_Management.Application.HomeOperations.GetAllProjects;
using Task_Management.Responses;

namespace Task_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllIndependentBoards")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BoardDto>>>> GetAllIndependentBoards()
        {
            IEnumerable<BoardDto> boardDtos = await _mediator.Send(new GetAllBoardsQuery());

            return Ok(ApiResponse<IEnumerable<BoardDto>>.Success(boardDtos));
        }

        [HttpGet("GetAllProjects")]
        public async Task<ActionResult<ApiResponse<ProjectDto>>> GetAllProjects()
        {
            IEnumerable<ProjectDto> projectDtos = await _mediator.Send(new GetAllProjectsQuery());

            return Ok(ApiResponse<IEnumerable<ProjectDto>>.Success(projectDtos));
        }
    }
}