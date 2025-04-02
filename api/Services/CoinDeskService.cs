using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using api.Dtos;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Services
{
    public class CoinDeskService : ICoinDeskService
    {
        private readonly ICurrencyRepository _currencyRepo;
        private static readonly HttpClient _client = new HttpClient();
        public CoinDeskService(ICurrencyRepository currencyRepo)
        {
            _currencyRepo = currencyRepo;
        }

        public async Task<string> GetCoinDesk()
        {
            string url = "https://api.coindesk.com/v1/bpi/currentprice.json";

            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Throw if not a success code (2xx)

                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                // mock data
                var mockData = "{\"time\":{\"updated\":\"Aug3, 2022 20:25:00 UTC\",\"updatedISO\":\"2022-08-03T20:25:00+00:00\",\"updateduk\":\"Aug3, 2022 at 21:25 BST\"},\"disclaimer\":\"ThisdatawasproducedfromtheCoinDeskBitcoinPriceIndex(USD).Non-USDcurrencydataconvertedusinghourlyconversionratefromopenexchangerates.org\",\"chartName\":\"Bitcoin\",\"bpi\":{\"USD\":{\"code\":\"USD\",\"symbol\":\"$\",\"rate\":\"23,342.0112\",\"description\":\"USDollar\",\"rate_float\":23342.0112},\"GBP\":{\"code\":\"GBP\",\"symbol\":\"£\",\"rate\":\"19,504.3978\",\"description\":\"BritishPoundSterling\",\"rate_float\":19504.3978},\"EUR\":{\"code\":\"EUR\",\"symbol\":\"€\",\"rate\":\"22,738.5269\",\"description\":\"Euro\",\"rate_float\":22738.5269}}}";
                return mockData;
            }
        }

        public async Task<List<CurrencyMappingDto>> GetFormatedCoinDesk()
        {
            try
            {
                var stringData = await GetCoinDesk();
                var data = JsonConvert.DeserializeObject<CoinDeskMockDataDto>(stringData);

                var codeList = new List<string>();
                foreach (PropertyInfo property in typeof(Bpi).GetProperties())
                {
                    codeList.Add(property.Name);
                }

                var currencyData = await _currencyRepo.GetByCodeListAsync(codeList);

                var responseList = new List<CurrencyMappingDto>();

                var formattedTime = DateTime.Parse(data.Time.UpdatedISO).ToString("yyyy/MM/dd hh:MM:ss");

                var currencyDict = currencyData.ToDictionary(x => x.Code, x => x.Name);

                // mapping data
                void AddCurrency(CurrencyInfoDto currencyInfo)
                {
                    if (currencyInfo == null) return;

                    responseList.Add(new CurrencyMappingDto
                    {
                        Code = currencyInfo.Code,
                        Name = currencyDict.ContainsKey(currencyInfo.Code) ? currencyDict[currencyInfo.Code] : null,
                        Rate = currencyInfo.Rate,
                        UpdateTime = formattedTime
                    });
                }

                if (data?.Bpi?.USD != null)
                    AddCurrency(data?.Bpi?.USD);
                if (data?.Bpi?.GBP != null)
                    AddCurrency(data?.Bpi?.GBP);
                if (data?.Bpi?.EUR != null)
                    AddCurrency(data?.Bpi?.EUR);

                return responseList.OrderBy(x => x.Code).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in GetFormattedCoinDesk: {e.Message}");
                return null;
            }
        }
    }
}