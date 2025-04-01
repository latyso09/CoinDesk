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
        public CoinDeskService(ICurrencyRepository currencyRepo)
        {
            _currencyRepo = currencyRepo;
        }

        public async Task<string> GetCoinDesk()
        {
            using HttpClient client = new HttpClient();
            string url = "https://api.coindesk.com/v1/bpi/currentprice.json";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
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

                // mapping data
                if (data?.Bpi?.USD != null)
                {
                    var item = new CurrencyMappingDto
                    {
                        Code = data.Bpi.USD.Code,
                        Name = currencyData.Where(x => x.Code == data.Bpi.USD.Code).FirstOrDefault()?.Name,
                        Rate = data.Bpi.USD.Rate,
                        UpdateTime = formattedTime
                    };
                    responseList.Add(item);
                }
                if (data?.Bpi?.GBP != null)
                {
                    var item = new CurrencyMappingDto
                    {
                        Code = data.Bpi.GBP.Code,
                        Name = currencyData.Where(x => x.Code == data.Bpi.GBP.Code).FirstOrDefault()?.Name,
                        Rate = data.Bpi.GBP.Rate,
                        UpdateTime = formattedTime
                    };
                    responseList.Add(item);
                }
                if (data?.Bpi?.EUR != null)
                {
                    var item = new CurrencyMappingDto
                    {
                        Code = data.Bpi.EUR.Code,
                        Name = currencyData.Where(x => x.Code == data.Bpi.EUR.Code).FirstOrDefault()?.Name,
                        Rate = data.Bpi.EUR.Rate,
                        UpdateTime = formattedTime
                    };
                    responseList.Add(item);
                }

                var response = responseList.OrderBy(x => x.Code).ToList();

                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}