using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace api.Mappers
{
    public static class CurrencyMapper
    {
        public static CurrencyDto ToCurrencyDto(this Currency currencyModel)
        {
            return new CurrencyDto
            {
                Id = currencyModel.Id,
                Code = currencyModel.Code,
                Name = currencyModel.Name
            };
        }

        public static Currency ToCurrencyFromCreateDto(this CreateCurrencyRequestDto currecyDto)
        {
            return new Currency
            {
                Code = currecyDto.Code,
                Name = currecyDto.Name
            };
        }
    }
}