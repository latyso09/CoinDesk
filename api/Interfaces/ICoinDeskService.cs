using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;

namespace api.Interfaces
{
    public interface ICoinDeskService
    {
        public Task<string> GetCoinDesk();
        public Task<List<CurrencyMappingDto>> GetFormatedCoinDesk();
    }
}