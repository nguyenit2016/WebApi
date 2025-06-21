using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderServices.Controllers;
using OrderServices.Helpers;
using OrderServices.Models;
using OrderServices.Services;
using OrderServices.UnitOfWork;
using OrderServices.ViewModels;
using System.Threading.Tasks;

namespace TestOrderServices.Controllers
{
    public class CustomerControllerTest
    {
        private readonly Mock<ICustomerService> _customerService;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CustomerController _controller;

        public CustomerControllerTest()
        {
            _customerService = new Mock<ICustomerService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _controller = new CustomerController(_customerService.Object);
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturnOkResult()
        {
            var request = new CustomerRequest
            {
                FullName = "Nguyễn Văn A",
                Email = "a@example.com",
                PhoneNumber = "0123456789"
            };
            var response = new CustomerResponse
            {
                CustomerId = 1,
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            _customerService.Setup(s => s.AddAsync(It.IsAny<CustomerRequest>()))
                .ReturnsAsync(ApiResponse<CustomerResponse>.SuccessResponse(response));

            var result = await _controller.CreateCustomer(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsAssignableFrom<ApiResponse<CustomerResponse>>(okResult.Value);

            Assert.True(apiResponse.Success);
            Assert.Equal("Nguyễn Văn A", apiResponse.Data.FullName);
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturnBadRequest_WhenValidationFails()
        {
            var request = new CustomerRequest
            {
                FullName = "",
                Email = "email",
                PhoneNumber = ""
            };

            var errors = new Dictionary<string, List<string>>
            {
                { "FullName", new List<string> { "FullName is required" } },
                { "PhoneNumber", new List<string> { "Phone number is required" } }
            };

            _customerService.Setup(s => s.AddAsync(It.IsAny<CustomerRequest>()))
                .ReturnsAsync(ApiResponse<CustomerResponse>.FailureResponse("Validation failed", errors));

            var result = await _controller.CreateCustomer(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsAssignableFrom<ApiResponse<CustomerResponse>>(badRequest.Value);
            Assert.False(response.Success);
            Assert.Equal("Validation failed", response.Message);
            Assert.NotNull(response.Errors);
            Assert.Contains("FullName", response.Errors.Keys);
        }
    }
}
