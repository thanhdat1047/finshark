using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
            var stocks = _context.Stocks.ToList()
            .Select(s =>s.ToStockDto());
            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public IActionResult GetStockById([FromRoute]int id)
        {
            var stock  = _context.Stocks.Find(id); //find on primary col
            if(stock == null)
            {
                return NotFound();
            }
            
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public IActionResult CreateStock([FromBody]CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetStockById),new {id = stockModel.Id },stockModel.ToStockDto());
           
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);
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

            _context.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }
    }
}