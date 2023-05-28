using AutoMapper;
using HotelReservationSystem.API.Dtos;
using HotelReservationSystem.API.Interfaces;
using HotelReservationSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;

namespace HotelReservationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IHotelReservationRepository _reservationRepository;
        private readonly IHotelRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public ReservationsController(IHotelReservationRepository reservationRepository, IHotelRoomRepository roomRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ReservationDto>> GetAllReservations()
        {
            var reservations = _reservationRepository.GetAllReservations();
            var reservationDtos = _mapper.Map<IEnumerable<ReservationDto>>(reservations);

            return Ok(reservationDtos);
        }

        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "user,Administrator")]
        public ActionResult<IEnumerable<ReservationDto>> GetReservationsByCustomerId(int customerId)
        {
            var reservations = _reservationRepository.GetReservationsByCustomerId(customerId);

            var reservationDtos = _mapper.Map<IEnumerable<ReservationDto>>(reservations);

            return Ok(reservationDtos);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "user")]
        public ActionResult<ReservationDto> GetReservationById(int id)
        {
            var reservation = _reservationRepository.GetReservationById(id);

            if (reservation == null)
                return NotFound();

            var reservationDto = _mapper.Map<ReservationDto>(reservation);

            return Ok(reservationDto);
        }

        [HttpGet("SearchReservations")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ReservationDto>> SearchReservations(DateTime? minDate, DateTime? maxDate, string? sortBy, bool ascending = true, int pageNumber = 1, int pageSize = 5)
        {
            var result = _reservationRepository.SearchReservations(minDate, maxDate, sortBy, ascending, pageNumber, pageSize);

            if (!result.Reservations.Any())
                return NotFound();

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(result.PaginationMetadata));

            var reservationDtos = _mapper.Map<IEnumerable<ReservationDto>>(result.Reservations);

            return Ok(reservationDtos);
        }

        [HttpPost]
        [Authorize(Roles = "user,Administrator")]
        public ActionResult CreateReservation([FromBody] CreateReservationDto createReservationDto)
        {
            var reservation = _mapper.Map<Reservation>(createReservationDto);

            if (reservation == null)
                return BadRequest();

            var room = _roomRepository.GetRoomById(reservation.RoomId);
            if (room == null)
            {
                return BadRequest("Room is not available.");
            }

            var existingReservations = _reservationRepository.GetReservationsByRoomId(room.Id);
            if (existingReservations.Any(r => (r.StartDate <= reservation.EndDate) && (r.EndDate >= reservation.StartDate)))
                return BadRequest("Room is not available for the requested date range.");

            _reservationRepository.CreateReservation(reservation);
            _reservationRepository.SaveChanges();

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, _mapper.Map<ReservationDto>(reservation));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "user,Administrator")]
        public ActionResult UpdateReservation(int id, [FromBody] UpdateReservationDto updateReservationDto)
        {  
            var reservation = _mapper.Map<Reservation>(updateReservationDto);
            _reservationRepository.UpdateReservation(reservation);
            _reservationRepository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "user,Administrator")]
        public ActionResult DeleteReservation(int id)
        {
            var reservation = _reservationRepository.GetReservationById(id);

            if (reservation == null)
                return BadRequest();

            _reservationRepository.DeleteReservation(id);

            return NoContent();
        }
    }
}
