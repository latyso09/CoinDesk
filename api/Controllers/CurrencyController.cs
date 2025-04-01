using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace api.Controllers
{
    [Route("api/currency")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ICurrencyRepository _currencyRepo;
        public CurrencyController(ApplicationDBContext context, ICurrencyRepository currencyRepo)
        {
            _context = context;
            _currencyRepo = currencyRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currencies = await _currencyRepo.GetAllAsync();
            var currenciesDto = currencies.OrderBy(x => x.Code).Select(x => x.ToCurrencyDto());

            return Ok(currenciesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var currency = await _currencyRepo.GetByIdAsync(id);
            if (currency == null)
            {
                return NotFound();
            }
            return Ok(currency.ToCurrencyDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCurrencyRequestDto currencyDto)
        {
            var currencyModel = currencyDto.ToCurrencyFromCreateDto();
            await _currencyRepo.CreateAsync(currencyModel);
            return CreatedAtAction(nameof(GetById), new { id = currencyModel.Id }, currencyModel.ToCurrencyDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCurrencyRequestDto currencyDto)
        {
            var currencyModel = await _currencyRepo.UpdateAsync(id, currencyDto);
            if (currencyModel == null)
            {
                return NotFound();
            }

            return Ok(currencyModel.ToCurrencyDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var currencyModel = await _currencyRepo.DeleteAsync(id);
            if (currencyModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
        
    }
}