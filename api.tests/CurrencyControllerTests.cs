using api.Controllers;
using api.Dtos;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Moq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace api.tests
{
    [TestClass]
    public class CurrencyControllerTests
    {
        private Mock<ICurrencyRepository>? _mockCurrencyRepo;
        private CurrencyController? _controller;
        [TestInitialize]
        public void Setup()
        {
            _mockCurrencyRepo = new Mock<ICurrencyRepository>();
            _controller = new CurrencyController(null, _mockCurrencyRepo.Object);
        }

        [TestMethod]
        public async void GetAll_Should_Return_True()
        {
            // Arrange
            var data = new List<Currency>
            {
                new() 
                {
                    Id = 1,
                    Code = "USD",
                    Name = "美金"
                }
            };
            _mockCurrencyRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(data);
            
            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            var returnedResult = okResult?.Value as List<CurrencyDto>;
            Assert.IsNotNull(returnedResult);
            Assert.AreEqual(1, returnedResult.Count);
        }

        [TestMethod]
        public async Task GetById_ExistingId_ReturnsOk()
        {
            // Arrange
            var currency = new Currency { Id = 1, Code = "USD", Name = "US Dollar" };
            _mockCurrencyRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(currency);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = result as OkObjectResult;
            var returnedResult = okResult.Value as CurrencyDto;
            Assert.IsNotNull(returnedResult);
            Assert.AreEqual("USD", returnedResult.Code);
        }

        [TestMethod]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockCurrencyRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Currency)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Create_ValidRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            var currencyDto = new CreateCurrencyRequestDto { Code = "USD", Name = "US Dollar" };
            var currencyModel = new Currency { Id = 1, Code = "USD", Name = "US Dollar" };

            // Mock the conversion method
            _mockCurrencyRepo.Setup(repo => repo.CreateAsync(It.IsAny<Currency>()))
                             .Returns(Task.FromResult(currencyModel))
                             .Callback<Currency>(c => c.Id = 1); // Simulate ID assignment

            // Act
            var result = await _controller.Create(currencyDto);

            // Assert
            var okResult = result as ObjectResult;
            var returnedCurrency = okResult?.Value as CurrencyDto;
            Assert.IsNotNull(returnedCurrency);
            Assert.AreEqual("USD", returnedCurrency.Code);
        }

        [TestMethod]
        public async Task Update_ExistingCurrency_ReturnsOk()
        {
            // Arrange
            var currencyDto = new UpdateCurrencyRequestDto { Code = "USD", Name = "Updated Dollar" };
            var updatedCurrency = new Currency { Id = 1, Code = "USD", Name = "Updated Dollar" };

            _mockCurrencyRepo.Setup(repo => repo.UpdateAsync(1, currencyDto)).ReturnsAsync(updatedCurrency);

            // Act
            var result = await _controller.Update(1, currencyDto);

            // Assert
            var okResult = result as ObjectResult;
            Assert.IsNotNull(okResult);
            var returnedCurrency = okResult?.Value as CurrencyDto;
            Assert.IsNotNull(returnedCurrency);
            Assert.AreEqual("Updated Dollar", returnedCurrency.Name);
        }

        [TestMethod]
        public async Task Update_NonExistingCurrency_ReturnsNotFound()
        {
            // Arrange
            var currencyDto = new UpdateCurrencyRequestDto { Code = "Non-Existing", Name = "Non-Existing" };
            _mockCurrencyRepo.Setup(repo => repo.UpdateAsync(99, currencyDto)).ReturnsAsync((Currency)null);

            // Act
            var result = await _controller.Update(99, currencyDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ExistingCurrency_ReturnsNoContent()
        {
            // Arrange
            var existingCurrency = new Currency { Id = 1, Code = "USD", Name = "US Dollar" };
            _mockCurrencyRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(existingCurrency);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Delete_NonExistingCurrency_ReturnsNotFound()
        {
            // Arrange
            _mockCurrencyRepo.Setup(repo => repo.DeleteAsync(99)).ReturnsAsync((Currency)null);

            // Act
            var result = await _controller.Delete(99);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
