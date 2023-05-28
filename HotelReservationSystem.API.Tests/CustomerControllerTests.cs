using AutoMapper;
using HotelReservationSystem.API.Controllers;
using HotelReservationSystem.API.Dtos;
using HotelReservationSystem.API.Interfaces;
using HotelReservationSystem.API.Models;
using HotelReservationSystem.API.Profiles;
using HotelReservationSystem.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelReservationSystem.API.Tests
{
    public class CustomersControllerTests
    {
        private Mock<IHotelCustomerRepository> _customerRepo;
        private Mock<IHotelReservationRepository> _reservationRepo;
        private IMapper _mapper;

        public CustomersControllerTests()
        {
            // Arrange 
            _customerRepo = new Mock<IHotelCustomerRepository>();
            _reservationRepo = new Mock<IHotelReservationRepository>();

            var customerProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customerProfile));
            _mapper = new Mapper(configuration);
        }

        [Fact]
        public void GetAllCustomers_ReturnZeroItems_WhenNoItemsInDb()
        {
            _customerRepo.Setup(repo => repo.GetCustomers()).Returns(new List<Customer>());
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.GetAllCustomers();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCustomers_ReturnsCorrectNumberOfCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer() { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Customer() { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
            };
            _customerRepo.Setup(repo => repo.GetCustomers()).Returns(customers);
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.GetAllCustomers();
            var okResult = result.Result as OkObjectResult;
            var customerDtos = okResult.Value as IEnumerable<CustomerDto>;

            // Assert
            Assert.Equal(2, customerDtos.Count());
        }

        [Fact]
        public void GetCustomerById_ReturnsCorrectCustomer()
        {
            // Arrange
            var customer = new Customer() { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };
            _customerRepo.Setup(repo => repo.GetCustomerById(1)).Returns(customer);
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.GetCustomerById(1);
            var okResult = result.Result as OkObjectResult;
            var customerDto = okResult.Value as CustomerDto;

            // Assert
            Assert.Equal(customer.Name, customerDto.Name);
            Assert.Equal(customer.Email, customerDto.Email);
        }

        [Fact]
        public void GetCustomerById_ReturnsNotFound_WhenNonExistingIdIsAdded()
        {
            _customerRepo.Setup(repo => repo.GetCustomerById(1)).Returns(new Customer() { Id = 1, Name = "John Doe", Email = "john.doe@example.com" });
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.GetCustomerById(2);

            // Assert 
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCustomerById_ReturnsCorrectDtoType_WhenExistingIdIsAdded()
        {
            _customerRepo.Setup(repo => repo.GetCustomerById(1)).Returns(new Customer() { Id = 1, Name = "John Doe", Email = "john.doe@example.com" });
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.GetCustomerById(1);

            var okResult = result.Result as OkObjectResult;
            var customerDto = okResult.Value as CustomerDto;

            // Assert 
            Assert.IsType<CustomerDto>(customerDto);
        }

        [Fact]
        public void GetCustomerById_ReturnsCorrectName_WhenExistingIdIsAsked()
        {
            var customer = new Customer() { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };

            _customerRepo.Setup(repo => repo.GetCustomerById(1)).Returns(customer);
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.GetCustomerById(1);

            var okResult = result.Result as OkObjectResult;
            var customerDto = okResult.Value as CustomerDto;

            // Assert 
            Assert.Equal(customer.Name, customerDto.Name);
        }

        [Fact]
        public void CreateCustomer_ReturnsCreatedResult_WhenNewObjectIsAdded()
        {
            var createCustomerDto = new CreateCustomerDto() { Name = "John Doe", Email = "john.doe@example.com" };
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.CreateCustomer(createCustomerDto);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public void CreateCustomer_ReturnsBadRequestResult_WhenNullObjectIsAdded()
        {
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.CreateCustomer(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void UpdateCustomer_ReturnsNoContent_WhenValidObjectIsAdded()
        {
            var updateCustomerDto = new UpdateCustomerDto() { Id = 1, Name = "Updated Name", Email = "updated.email@example.com" };
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.UpdateCustomer(1, updateCustomerDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCustomer_ReturnsNoContent_WhenExistingIdIsPassed()
        {
            _customerRepo.Setup(repo => repo.GetCustomerById(1)).Returns(new Customer() { Id = 1, Name = "John Doe", Email = "john.doe@example.com" });
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCustomer_ReturnsBadRequest_WhenNonExistingIdIsPassed()
        {
            _customerRepo.Setup(repo => repo.GetCustomerById(1)).Returns(new Customer() { Id = 1, Name = "John Doe", Email = "john.doe@example.com" });
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.DeleteCustomer(2);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetCustomerReservations_ReturnsOkResultWithEmptyList_WhenNoReservationsExistForGivenCustomerId()
        {
            // Arrange
            _reservationRepo.Setup(repo => repo.GetReservationsByCustomerId(2)).Returns(new List<Reservation>());
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.GetCustomerReservations(2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ReservationDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }


        [Fact]
        public void GetCustomerReservations_ReturnsOkResult_WhenExistingIdIsPassed()
        {
            _reservationRepo.Setup(repo => repo.GetReservationsByCustomerId(1)).Returns(new List<Reservation>());
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.GetCustomerReservations(1);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void SearchCustomers_ReturnsNotFound_WhenNoMatchFound()
        {
            var paginationMetadata = new PaginationMetadata(1, 5, 0); // No items
            _customerRepo.Setup(repo => repo.SearchCustomers("John", null, true, 1, 5)).Returns((new List<Customer>(), paginationMetadata));
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Act
            var result = controller.SearchCustomers("Jane", null, true, 1, 5);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void SearchCustomers_ReturnsOkResult_WhenMatchFound()
        {
            // Arrange
            var paginationMetadata = new PaginationMetadata(1, 5, 1); // One item
            _customerRepo.Setup(repo => repo.SearchCustomers("John", null, true, 1, 5)).Returns((new List<Customer> { new Customer() { Id = 1, Name = "John Doe", Email = "john.doe@example.com" } }, paginationMetadata));
            var controller = new CustomersController(_customerRepo.Object, _reservationRepo.Object, _mapper);

            // Set up the Response property on the controller
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var actionResult = controller.SearchCustomers("John", null, true, 1, 5);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<CustomerDto>>(okResult.Value);
            Assert.Single(returnValue); // Assert that only one customer is returned
            Assert.Equal("John Doe", returnValue.First().Name); // Assert the customer name
        }


    }
}