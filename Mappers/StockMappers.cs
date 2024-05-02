using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockMode)
        {
            return new StockDto {
                Id = stockMode.Id,
                Symbol =stockMode.Symbol,
                CompanyName = stockMode.CompanyName,
                Purchase = stockMode.Purchase,
                LastDiv = stockMode.LastDiv,
                Industry = stockMode.Industry, 
                MarketCap = stockMode.MarketCap
            };
        }

        public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockDto)
        {
            return new Stock {
                Symbol = stockDto.Symbol, 
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv, 
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }
        
    }
}