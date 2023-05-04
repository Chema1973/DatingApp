using APIDatingApp.Data;
using APIDatingApp.DTOs;
using APIDatingApp.Entities;
using APIDatingApp.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIDatingApp.Controllers
{
    // [ApiController]
    // [Route("api/[controller]")]
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        // private readonly DataContext _context;

        public UsersController(// DataContext context
            IUserRepository userRepository,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            //_context = context;
        }

        // [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            // var users = await _context.Users.ToListAsync();
            // return users;
            // return Ok(await _userRepository.GetUsersAsync());


            // var users = await _userRepository.GetUsersAsync();
            // var usersToReturn  = _mapper.Map<IEnumerable<MemberDTO>>(users);

            // return Ok(usersToReturn);

            return Ok(await _userRepository.GetMembersAsync());
        }

        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDTO>> GetUser(int id)
        {
            // return await _context.Users.FindAsync(id);
            // return await _userRepository.GetUserByIdAsync(id);
            var user = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<MemberDTO>(user);
        }

/*
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUserByUserName(string username)
        {
            // return await _context.Users.FindAsync(id);
            // return await _userRepository.GetUserByUserNameAsync(username);
            var user = await _userRepository.GetUserByUserNameAsync(username);

            return _mapper.Map<MemberDTO>(user);

        }*/

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUserByUserName(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }

    }
}