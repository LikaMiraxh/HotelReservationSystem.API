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
    public class RoomsController : ControllerBase
    {
        private readonly IHotelRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomsController(IHotelRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<RoomDto>> GetAllRooms()
        {
            var rooms = _roomRepository.GetRooms();

            var roomDtos = _mapper.Map<IEnumerable<RoomDto>>(rooms);

            return Ok(roomDtos);
        }

        [HttpGet("SearchRooms")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<RoomDto>> SearchRooms(int? minPrice, int? maxPrice, string? Type, string? sortBy, bool ascending = true, int pageNumber = 1, int pageSize = 5)
        {
            var result = _roomRepository.SearchRooms(minPrice, maxPrice, Type, sortBy, ascending, pageNumber, pageSize);

            if (!result.Rooms.Any())
                return NotFound();

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(result.PaginationMetadata));

            var roomDtos = _mapper.Map<IEnumerable<RoomDto>>(result.Rooms);

            return Ok(roomDtos);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "user")]
        public ActionResult<RoomDto> GetRoomById(int id)
        {
            var room = _roomRepository.GetRoomById(id);

            if (room == null)
                return NotFound();

            var roomDto = _mapper.Map<RoomDto>(room);

            return Ok(roomDto);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateRoom([FromBody] CreateRoomDto createRoomDto)
        {
            var room = _mapper.Map<Room>(createRoomDto);

            if (room == null)
                return BadRequest();

            _roomRepository.CreateRoom(room);
            _roomRepository.SaveChanges();

            return Created("RoomUri", new { room.Type });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult UpdateRoom(int id, [FromBody] UpdateRoomDto updateRoomDto)
        {
            var room = _mapper.Map<Room>(updateRoomDto);
            _roomRepository.UpdateRoom(room);
            _roomRepository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteRoom(int id)
        {
            var room = _roomRepository.GetRoomById(id);

            if (room == null)
                return BadRequest();

            _roomRepository.DeleteRoom(id);

            return NoContent();
        }
    }
}
