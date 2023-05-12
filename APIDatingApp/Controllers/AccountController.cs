using System.Security.Cryptography;
using System.Text;
using APIDatingApp.Data;
using APIDatingApp.DTOs;
using APIDatingApp.Entities;
using APIDatingApp.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIDatingApp.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        // public AccountController(DataContext context, ITokenService tokenService, IUserRepository userRepository, IMapper mapper){
        // --> Antes de Identity
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper){
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTO registerDTO)
        {

            if (await UserExists(registerDTO.UserName)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.UserName.ToLower();

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return new UserDto{
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> UserExists(string userName){
            return await _userManager.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDTO loginDto){

            var user  = await _userManager.Users //.GetUserByUserNameAsync(loginDto.UserName);
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null) return Unauthorized("Invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) return Unauthorized("Invalid password");

            return new UserDto{
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }


        #region "Antes de Identity"
/*
        [HttpPost("registerAntes")]
        public async Task<ActionResult<UserDto>> RegisterAntes(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.UserName)) return BadRequest("Username is taken");
            var user = _mapper.Map<AppUser>(registerDTO);
            // using var hmac = new HMACSHA512();
            // --> Por meter el Identitity
            user.UserName = registerDTO.UserName.ToLower();
            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            // --> Por meter el Identitity
            // user.PasswordSalt = hmac.Key;
            // --> Por meter el Identitity

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto{
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("loginAntes")]
        public async Task<ActionResult<UserDto>> LoginAntes(LoginDTO loginDto){
            // var user = await _context.Users.SingleOrDefaultAsync(a => a.UserName == loginDto.UserName);
            var user  = await _userRepository.GetUserByUserNameAsync(loginDto.UserName);
            if (user == null) return Unauthorized("Invalid username");
            // using var hmac = new HMACSHA512(user.PasswordSalt);
            // --> Por meter el Identitity
            // var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            // --> Por meter el Identitity
            // for (int i=0;i < computedHash.Length; i++){
            //    if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            //}
            // --> Por meter el Identitity

            return new UserDto{
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
            // return user;
        }
*/
        #endregion "Antes de Identity"

    }
}