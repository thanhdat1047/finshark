using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : Controller
    {   
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult>GetAll()
        {
            var stocks = await _context.Stocks.ToListAsync();
            var stockDto = stocks.Select(s =>s.ToStockDto());
            return Ok(stockDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockById([FromRoute]int id)
        {
            var stock  = await _context.Stocks.FindAsync(id); //find on primary col
            if(stock == null)
            {
                return NotFound();
            }
            
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult>  CreateStock([FromBody]CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStockById),new {id = stockModel.Id },stockModel.ToStockDto());
           
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(stockModel == null)
            {
                return NotFound();
            }
            stockModel.Symbol = stockDto.Symbol;
            stockModel.CompanyName = stockDto.CompanyName;
            stockModel.Purchase = stockDto.Purchase; 
            stockModel.LastDiv = stockDto.LastDiv; 
            stockModel.Industry = stockDto.Industry; 
            stockModel.MarketCap = stockDto.MarketCap;

            await _context.SaveChangesAsync();
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var stockModel =  await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if(stockModel == null)
            {
                return NotFound();
            }
            _context.Remove(stockModel);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}