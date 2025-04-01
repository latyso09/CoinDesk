using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using api.Dtos;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;
using api.Interfaces;
using System.Reflection;

namespace api.Controllers
{
    [Route("api/coindesk")]
    [ApiController]
    public class CoinDeskController : ControllerBase
    {
        private readonly ICurrencyRepository _currencyRepo;
        private readonly ICoinDeskService _coinDeskService;
        public CoinDeskController(ICurrencyRepository currencyRepo, ICoinDeskService coinDeskServuce)
        {
            _currencyRepo = currencyRepo;
            _coinDeskService = coinDeskServuce;
        }

        [HttpGet]
        public async Task<IActionResult> GetCoinDesk()
        {
            var response = await _coinDeskService.GetCoinDesk();
            return Ok(response);
        }

        [HttpGet("formated")]
        public async Task<IActionResult> GetFormatedCoinDesk()
        {
            var response = await _coinDeskService.GetFormatedCoinDesk();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}