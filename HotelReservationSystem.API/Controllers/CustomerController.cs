using AutoMapper;
using HotelReservationSystem.API.Dtos;
using HotelReservationSystem.API.Interfaces;
using HotelReservationSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelReservationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly IHotelCustomerRepository _customerRepository;
        private readonly IHotelReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public CustomersController(IHotelCustomerRepository customerRepository, IHotelReservationRepository reservationRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<CustomerDto>> GetAllCustomers()
        {
            var customers = _customerRepository.GetCustomers();

            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);

            return Ok(customerDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<CustomerDto> GetCustomerById(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);

            if (customer == null)
                return NotFound();

            var customerDto = _mapper.Map<CustomerDto>(customer);

            return Ok(customerDto);
        }

        [HttpGet("SearchCustomers")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<CustomerDto>> SearchCustomers(string? searchTerm, string? sortBy, bool ascending = true, int pageNumber = 1, int pageSize = 5)
        {
            var result = _customerRepository.SearchCustomers(searchTerm, sortBy, ascending, pageNumber, pageSize);

            if (!result.Customers.Any())
                return NotFound();

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(result.PaginationMetadata));

            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(result.Customers);

            return Ok(customerDtos);
        }


        [HttpGet("{id}/reservations")]
        [Authorize(Roles = "user")]
        public ActionResult<IEnumerable<ReservationDto>> GetCustomerReservations(int id)
        {
            var reservations = _reservationRepository.GetReservationsByCustomerId(id);

            if (reservations == null)
                return NotFound();

            var reservationDtos = _mapper.Map<IEnumerable<ReservationDto>>(reservations);

            return Ok(reservationDtos);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateCustomer([FromBody] CreateCustomerDto createCustomerDto)
        {
            var customer = _mapper.Map<Customer>(createCustomerDto);

            if (customer == null)
                return BadRequest();

            _customerRepository.CreateCustomer(customer);
            _customerRepository.SaveChanges();

            return Created($"api/Customers/{customer.Id}", new { customer.Id });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult UpdateCustomer(int id, [FromBody] UpdateCustomerDto updateCustomerDto)
        {
            var customer = _mapper.Map<Customer>(updateCustomerDto);
            _customerRepository.UpdateCustomer(customer);
            _customerRepository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteCustomer(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);

            if (customer == null)
                return BadRequest();

            _customerRepository.DeleteCustomer(id);

            return NoContent();
        }
    }

}
