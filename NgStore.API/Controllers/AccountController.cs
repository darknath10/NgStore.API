using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NgStore.API.Entities;
using NgStore.API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NgStore.API.Controllers
{
    [Route("api/[controller]"), EnableCors("AllowNgStore.Client")]
    public class AccountController : Controller
    {
        private UserManager<User> _usrMgr;
        private IPasswordHasher<User> _pwdHasher;
        private IConfigurationRoot _config;

        public AccountController(UserManager<User> usrMgr, IPasswordHasher<User> pwdHasher, IConfigurationRoot config)
        {
            _usrMgr = usrMgr;
            _pwdHasher = pwdHasher;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                User user;

                if (model.UserNameEmail.Contains('@'))
                {
                    user = await _usrMgr.FindByEmailAsync(model.UserNameEmail);
                    if (user == null)
                    {
                        user = await _usrMgr.FindByNameAsync(model.UserNameEmail);
                    }
                }
                else
                {
                    user = await _usrMgr.FindByNameAsync(model.UserNameEmail);
                }

                if (user != null)
                {
                    if (_pwdHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                    {
                        var userClaims = await _usrMgr.GetClaimsAsync(user);

                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email)
                        }.Union(userClaims);

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            issuer: _config["Tokens:Issuer"],
                            audience: _config["Tokens:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(15),
                            signingCredentials: credentials);

                        return Ok(new { user_token = new JwtSecurityTokenHandler().WriteToken(token) });
                    }
                }

                return BadRequest("Invalid Credentials");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
