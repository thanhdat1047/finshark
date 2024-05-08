using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        public PortfolioController(UserManager<AppUser> userManager,
        IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager; 
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }
        [HttpGet("/email")]
        [Authorize]

        public async Task<IActionResult> GetEmailUserPortfolio()
        {
            var userName = HttpContext.User?.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(userName);
        }
    }
}