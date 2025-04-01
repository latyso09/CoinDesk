using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ApplicationDBContext _context;
        public CurrencyRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Currency> CreateAsync(Currency currencyModel)
        {
            currencyModel.CreateDate = DateTime.Now;
            currencyModel.CreatedBy = "User";
            currencyModel.ModifyDate = null;
            currencyModel.ModifyDate = null;
            await _context.Currency.AddAsync(currencyModel);
            await _context.SaveChangesAsync();
            return currencyModel;
        }

        public async Task<Currency?> DeleteAsync(int id)
        {
            var currencyModel = await _context.Currency.FirstOrDefaultAsync(x => x.Id == id);
            if (currencyModel == null)
            {
                return null;
            }

            _context.Currency.Remove(currencyModel);
            await _context.SaveChangesAsync();
            return currencyModel;
        }

        public async Task<List<Currency>> GetAllAsync()
        {
            return await _context.Currency.ToListAsync();
        }

        public async Task<List<Currency>> GetByCodeListAsync(List<string> codeList)
        {
            return await _context.Currency.Where(x => codeList.Contains(x.Code)).ToListAsync();
        }

        public async Task<Currency?> GetByIdAsync(int id)
        {
            return await _context.Currency.FindAsync(id);
        }

        public async Task<Currency?> UpdateAsync(int id, UpdateCurrencyRequestDto currencyDto)
        {
            var existingCurrency = await _context.Currency.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCurrency == null)
            {
                return null;
            }

            existingCurrency.Code = currencyDto.Code;
            existingCurrency.Name = currencyDto.Name;
            existingCurrency.ModifiedBy = "User";
            existingCurrency.ModifyDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingCurrency;
        }
    }
}