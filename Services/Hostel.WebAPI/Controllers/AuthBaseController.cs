using Hostel.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Hostel.Domain.DTO.UsersDTOs;
using Hostel.Domain.Security;

namespace Hostel.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthBaseController : ControllerBase
    {
        #region Поля
        private readonly IConfiguration configuration;
        private readonly IConfigurationSection jwtSettings;
        private readonly UserManager<UserEntity> repository;
        #endregion

        #region Базовые компоненты
        public AuthBaseController(UserManager<UserEntity> userManager, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.jwtSettings = this.configuration.GetSection("JWTSettings");
            this.repository = userManager;
        }
        #endregion

        #region Публичные методы
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginRequestDTO userEntity)
        {
            var user = await repository.FindByEmailAsync(userEntity.Username);

            if (user is null) return Unauthorized(new UserLoginResponseDTO { IsAuthSuccessful = false, ErrorMessage = "Пользователь не найден" });
            if (!await repository.CheckPasswordAsync(user, userEntity.Password)) return Unauthorized(new UserLoginResponseDTO { IsAuthSuccessful = false, ErrorMessage = "Неверный пароль" });
            if (userEntity.IsServer && !user.IsAdmin) return Unauthorized(new UserLoginResponseDTO { IsAuthSuccessful = false, ErrorMessage = "Вы не являетесь администратором" });

            var signingCredentials = GetSigningCredentials();

            var claims = GetClaims(user);

            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new UserLoginResponseDTO { IsAuthSuccessful = true, Token = token, Username = user.UserName });
        }

        #endregion

        #region Закрытые методы
        /// <summary>
        /// Поулчение ключа шифрования
        /// </summary>
        /// <returns>Ключ шифрования</returns>
        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// Получение прав
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Права</returns>
        private List<Claim> GetClaims(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new Claim("userID", user.Id),
                new Claim("UserName", user.Fullname),
                new Claim("CreatedAt",user.CreatedAt.ToString())
            };

            if (!string.IsNullOrEmpty(user.ProfileImg))
                claims.Add(new Claim("UserImage", user.ProfileImg));
            else
                claims.Add(new Claim("UserImage", ""));

            return claims;
        }

        /// <summary>
        /// Генерирование токена
        /// </summary>
        /// <param name="signingCredentials">Ключ шифрования</param>
        /// <param name="claims">Права</param>
        /// <returns>JWT токен</returns>
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("validIssuer").Value,
                audience: jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expiryInMinutes").Value)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }
        #endregion
    }
}
