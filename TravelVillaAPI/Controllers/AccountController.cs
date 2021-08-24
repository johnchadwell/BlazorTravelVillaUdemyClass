using Common;
using DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TravelVillaAPI.Controllers.Helper;

namespace TravelVillaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signinmgr;
        private readonly UserManager<ApplicationUser> _usermgr;
        private readonly RoleManager<IdentityRole> _rolemgr;
        private readonly APISettings _APISettings;

        public AccountController(SignInManager<ApplicationUser> signinmgr, 
            UserManager<ApplicationUser> usermgr, 
            RoleManager<IdentityRole> rolemgr,
            IOptions<APISettings> options)
        {
            _signinmgr = signinmgr;
            _usermgr = usermgr;
            _rolemgr = rolemgr;
            _APISettings = options.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserRequestDTO userDTO)
        {
            if (userDTO==null || !ModelState.IsValid)
            {
                return BadRequest();

            }
            var user = new ApplicationUser
            {
                UserName = userDTO.Email,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                PhoneNumber = userDTO.PhoneNo,
                EmailConfirmed = true
            };
            var result = await _usermgr.CreateAsync(user, userDTO.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDTO()
                {
                    Errors = error,
                    IsRegSuccess = false
                });
            }
            var roleRes = await _usermgr.AddToRoleAsync(user, SD.Role_Customer);
            if (!roleRes.Succeeded)
            {
                var error = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDTO()
                {
                    Errors = error,
                    IsRegSuccess = false
                });
            }
            return StatusCode(201);

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] AuthDTO authDTO)
        {
            var result = await _signinmgr.PasswordSignInAsync(authDTO.UserName, authDTO.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _usermgr.FindByNameAsync(authDTO.UserName);
                if (user==null)
                {
                    return Unauthorized(new AuthResponseDTO
                    {
                        IsAuthSuccess = false,
                        ErrorMsg = "Invalid Auth"
                    });
                }
                //everything is good so login user
                var creds = GetSigningCredentials();
                var claims = await GetClaims(user);
                var tokenOptions = new JwtSecurityToken(
                    issuer: _APISettings.ValidIssuer,
                    audience: _APISettings.ValidAudience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: creds
                    );

                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return Ok(new AuthResponseDTO
                {
                    IsAuthSuccess = true,
                    Token = token,
                    userDTO = new UserDTO()
                    {
                        Name = user.LastName,
                        Id = user.Id,
                        Email = user.Email,
                        PhoneNo = user.PhoneNumber
                    }
                });

            } else
            {
                return Unauthorized(new AuthResponseDTO
                {
                    IsAuthSuccess = false,
                    ErrorMsg = "Invalid Authentication"
                });
            }
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_APISettings.SecretKey));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id)
            };
            var roles = await _usermgr.GetRolesAsync(await _usermgr.FindByEmailAsync(user.Email));
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
