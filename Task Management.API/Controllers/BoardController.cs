using MediatR;
using Microsoft.AspNetCore.Mvc;
using Task_Management.Application.BoardOperations.CreateBoard;
using Task_Management.Application.BoardOperations.DeleteBoard;
using Task_Management.Application.BoardOperations.GetBoard;
using Task_Management.Application.BoardOperations.UpdateBoard;
using Task_Management.Application.Dtos;
using Task_Management.Domain.Interfaces;
using Task_Management.Responses;

namespace Task_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BoardController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateBoard")]
        public async Task<ActionResult<ApiResponse<int>>> CreateBoard([FromBody] CreateBoardCommand command)
        {
            int BoardID = await _mediator.Send(command);

            return Ok(ApiResponse<int>.Success(BoardID));
        }

        [HttpGet("GetBoard/{ID:int}")]
        public async Task<ActionResult<ApiResponse<BoardDto>>> GetBoard([FromRoute] int ID)
        {
            BoardDto boardDto = await _mediator.Send(new GetBoardQuery { BoardID = ID });
            return Ok(ApiResponse<BoardDto>.Success(boardDto));
        }

        [HttpPut("UpdateBoard/{ID:int}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateBoard([FromRoute] int ID, [FromBody] UpdateBoardCommand command)
        {
            command.BoardID = ID;

            bool Result = await _mediator.Send(command);

            return Ok(ApiResponse<bool>.Success(Result, "Board updated successfully."));
        }

        [HttpDelete("DeleteBoard/{ID:int}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteBoard([FromRoute] int ID)
        {
            bool Result = await _mediator.Send(new DeleteBoardCommand { BoardID = ID });

            return Ok(ApiResponse<bool>.Success(Result, "Board deleted successfully."));
        }
    }
}