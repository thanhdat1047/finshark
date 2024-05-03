using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        public CommentController(ICommentRepository commentRepository,
        IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentDto = comments.Select(s =>s.ToCommentDto());
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment  = await _commentRepository.GetByIdAsync(id);
            if(comment == null)
                return NotFound();
            var commentDto = comment.ToCommentDto();
            return Ok(commentDto);
        }
        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create ([FromRoute]int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if(!await _stockRepository.StockExists(stockId))
            {
                return BadRequest("Stock does not exist");
            }

            var commentModel = commentDto.ToCommentFromCreate(stockId);
            await _commentRepository.CreatedAsync(commentModel);
            return CreatedAtAction(nameof(GetById),new {id = commentModel.Id},commentModel.ToCommentDto());

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
        {
            var comment = await _commentRepository.UpdateAsync(id,commentDto.ToCommentFromUpdate());
            if(comment == null)
                return NotFound("Comment not found");
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment  = await _commentRepository.DeleteAsync(id);
            if(comment == null)
                return NotFound("Comment not found");
            return Ok(comment);
        }
    }
}