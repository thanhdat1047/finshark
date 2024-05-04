using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : Controller
    {   
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stoIStockRepository;
        public StockController(ApplicationDBContext context, 
        IStockRepository stockRepository)
        {
            _context = context;
            _stoIStockRepository = stockRepository;
        }
        [HttpGet]
        public async Task<IActionResult>GetAll()
        {
            var stocks = await _stoIStockRepository.GetAllAsync();
            var stockDto = stocks.Select(s =>s.ToStockDto());
            return Ok(stockDto);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStockById([FromRoute]int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stock  = await _stoIStockRepository.GetByIdAsync(id); //find on primary col
            if(stock == null)
            {
                return NotFound();
            }
            
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult>  CreateStock([FromBody]CreateStockRequestDto stockDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = stockDto.ToStockFromCreateDto();
            await _stoIStockRepository.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetStockById),new {id = stockModel.Id },stockModel.ToStockDto());
           
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = await _stoIStockRepository.UpdateAsync(id,stockDto);
            if(stockModel == null)
            {
                return NotFound();
            }
        
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel =  await _stoIStockRepository.DeleteAsync(id);
            if(stockModel == null)
            {
                return NotFound();
            }
            return NoContent();

        }
    }
}