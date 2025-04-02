using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using api.Interfaces;
using api.Services;
using Moq.Protected;
using api.Dtos;
using api.Models;

namespace api.tests
{ 
    [TestClass]
    public class CoinDeskServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private Mock<ICurrencyRepository> _currencyRepoMock;
        private ICoinDeskService _coinDeskService;

        [TestInitialize]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _currencyRepoMock = new Mock<ICurrencyRepository>();
            _coinDeskService = new CoinDeskService(_currencyRepoMock.Object);
        }

        [TestMethod]
        public async Task GetCoinDesk_ShouldReturnMockData_OnHttpRequestException()
        {
            // Arrange
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", 
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException());

            // Act
            var result = await _coinDeskService.GetCoinDesk();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("updatedISO"));
        }

        [TestMethod]
        public async Task GetCoinDesk_ShouldReturnActualData_OnSuccess()
        {
            // Arrange
            var expectedJson = "{\"time\":{\"updated\":\"Aug3, 2022 20:25:00 UTC\",\"updatedISO\":\"2022-08-03T20:25:00+00:00\",\"updateduk\":\"Aug3, 2022 at 21:25 BST\"},\"disclaimer\":\"ThisdatawasproducedfromtheCoinDeskBitcoinPriceIndex(USD).Non-USDcurrencydataconvertedusinghourlyconversionratefromopenexchangerates.org\",\"chartName\":\"Bitcoin\",\"bpi\":{\"USD\":{\"code\":\"USD\",\"symbol\":\"$\",\"rate\":\"23,342.0112\",\"description\":\"USDollar\",\"rate_float\":23342.0112},\"GBP\":{\"code\":\"GBP\",\"symbol\":\"£\",\"rate\":\"19,504.3978\",\"description\":\"BritishPoundSterling\",\"rate_float\":19504.3978},\"EUR\":{\"code\":\"EUR\",\"symbol\":\"€\",\"rate\":\"22,738.5269\",\"description\":\"Euro\",\"rate_float\":22738.5269}}}";

            _httpMessageHandlerMock
                .Protected() // Allows mocking protected members
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", // Mock the correct method
                    ItExpr.IsAny<HttpRequestMessage>(), 
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedJson)
                });

            // Act
            var result = await _coinDeskService.GetCoinDesk();

            // Assert
            Assert.AreEqual(expectedJson, result);
        }

        [TestMethod]
        public async Task GetFormattedCoinDesk_ShouldReturnFormattedData()
        {
            // Arrange
            var mockJson = "{\"time\":{\"updated\":\"Aug3, 2022 20:25:00 UTC\",\"updatedISO\":\"2022-08-03T20:25:00+00:00\",\"updateduk\":\"Aug3, 2022 at 21:25 BST\"},\"disclaimer\":\"ThisdatawasproducedfromtheCoinDeskBitcoinPriceIndex(USD).Non-USDcurrencydataconvertedusinghourlyconversionratefromopenexchangerates.org\",\"chartName\":\"Bitcoin\",\"bpi\":{\"USD\":{\"code\":\"USD\",\"symbol\":\"$\",\"rate\":\"23,342.0112\",\"description\":\"USDollar\",\"rate_float\":23342.0112},\"GBP\":{\"code\":\"GBP\",\"symbol\":\"£\",\"rate\":\"19,504.3978\",\"description\":\"BritishPoundSterling\",\"rate_float\":19504.3978},\"EUR\":{\"code\":\"EUR\",\"symbol\":\"€\",\"rate\":\"22,738.5269\",\"description\":\"Euro\",\"rate_float\":22738.5269}}}";
            _httpMessageHandlerMock
                .Protected() // Allows mocking protected members
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", // Mock the correct method
                    ItExpr.IsAny<HttpRequestMessage>(), 
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(mockJson)
                });

            var mockCurrencyData = new List<Currency> { new Currency { Code = "USD", Name = "US Dollar" } };
            _currencyRepoMock.Setup(repo => repo.GetByCodeListAsync(It.IsAny<List<string>>()))
                .Returns(Task.FromResult(mockCurrencyData));

            // Act
            var result = await _coinDeskService.GetFormatedCoinDesk();

            // Assert
            Assert.IsNotNull(result);
            // Assert.AreEqual(1, result.Count);
            Assert.AreEqual("USD", result[2].Code);
            Assert.AreEqual("US Dollar", result[2].Name);
            Assert.AreEqual("23,342.0112", result[2].Rate);
        }
    }
}