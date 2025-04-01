using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<List<Currency>> GetAllAsync();
        Task<Currency?> GetByIdAsync(int id);
        Task<List<Currency>> GetByCodeListAsync(List<string> codeList);
        Task<Currency> CreateAsync(Currency currencyModel);
        Task<Currency?> UpdateAsync(int id, UpdateCurrencyRequestDto currencyDto);
        Task<Currency?> DeleteAsync(int id);
    }
}