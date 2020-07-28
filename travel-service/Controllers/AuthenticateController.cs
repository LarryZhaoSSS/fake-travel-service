using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using travel_service.Dtos;

namespace travel_service.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult login([FromBody] LoginDto loginDto)
        {
            // 1 验证用户名密码

            // 2 创建jwt
            // header
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            // payload
            var claims = new[]
            {
                // sub
                new Claim(JwtRegisteredClaimNames.Sub, "fake_user_id")
            };
            // signiture
            var secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            var signingKey = new SymmetricSecurityKey(secretByte);
            var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            // 3 return 200 ok + jwt
            return Ok(tokenStr);
        }
    }
}
