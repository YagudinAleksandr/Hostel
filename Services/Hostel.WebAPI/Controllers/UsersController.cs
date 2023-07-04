using Hostel.DAL.Entities;
using Hostel.Infrastructure.Pagination.Entities;
using Hostel.Infrastructure.Pagination.Repositories;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Hostel.Domain.DTO.UsersDTOs;
using System;
using System.Collections.Generic;

namespace Hostel.WebAPI.Controllers
{
    /// <summary>
    /// Контроллер обработки пользователей
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Поля
        private UserManager<UserEntity> repository;
        #endregion

        #region Базовые компоненты
        public UsersController(UserManager<UserEntity> repository) =>
            this.repository = repository;
        #endregion

        #region Публичные методы
        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        /// <param name="pageParametrs">Данные для отображения страницы</param>
        /// <returns>Сущности в определенном объеме</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PageParametrs pageParametrs)
        {
            var users = await repository.Users.ToListAsync();

            List<UserResponseDTO> usersResponse = new List<UserResponseDTO>();

            foreach(var user in users)
            {
                usersResponse.Add(new UserResponseDTO
                {
                    Id = user.Id,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    Fullname = user.Fullname,
                    IsActive = user.IsActive,
                    IsAdmin = user.IsAdmin,
                    NormalizedUserName = user.NormalizedUserName,
                    NormilizedEmail = user.NormalizedEmail,
                    ProfileImg = user.ProfileImg,
                    UserName = user.UserName,
                    IsSucceed = true,
                    Post = user.Post,
                    Phone = user.PhoneNumber
                });
            }

            var page = PagedList<UserResponseDTO>.ToPagedList(usersResponse, pageParametrs.PageNumber, pageParametrs.PageSize);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(page.MetaData));

            return Ok(page);
        }

        /// <summary>
        /// Получение сущности пользователя
        /// </summary>
        /// <param name="id">ID сущности</param>
        /// <returns>Сущность</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await repository.FindByIdAsync(id);

            if (user is null) return NotFound(new UserResponseDTO { IsSucceed = false, Errors = "Пользователь не найден" });

            UserResponseDTO responseDTO = new UserResponseDTO
            {
                Id = user.Id,
                ProfileImg = user.ProfileImg,
                IsActive = user.IsActive,
                IsAdmin = user.IsAdmin,
                Fullname = user.Fullname,
                NormalizedUserName = user.NormalizedUserName,
                NormilizedEmail = user.NormalizedEmail,
                UserName = user.UserName,
                IsSucceed = true,
                Post = user.Post,
                Phone = user.PhoneNumber
            };

            return Ok(responseDTO);
        }

        /// <summary>
        /// Добавление сущности пользователя
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserCreateRequestDTO entity)
        {
            try
            {
                UserEntity user = new UserEntity
                {
                    Email = entity.Email,
                    NormalizedEmail = entity.Email,
                    UserName = entity.UserName,
                    NormalizedUserName = entity.Fullname,
                    Fullname = entity.Fullname,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ProfileImg = entity.ProfileImg,
                    IsAdmin = entity.IsAdmin,
                    IsActive = entity.IsActive,
                    Post = entity.Post,
                    PhoneNumber = entity.Phone
                };

                var result = await repository.CreateAsync(user, entity.Password);

                if (result.Succeeded)
                {
                    var response = await repository.FindByEmailAsync(user.Email);

                    return Ok(new UserResponseDTO
                    {
                        CreatedAt = response.CreatedAt,
                        UpdatedAt = response.UpdatedAt,
                        Fullname = response.Fullname,
                        Id = response.Id,
                        IsActive = response.IsActive,
                        IsAdmin = response.IsAdmin,
                        NormalizedUserName = response.NormalizedUserName,
                        NormilizedEmail = response.NormalizedEmail,
                        ProfileImg = response.ProfileImg,
                        UserName = response.UserName,
                        IsSucceed = true,
                        Post = response.Post,
                        Phone = response.PhoneNumber
                    });
                }
                else
                    return BadRequest(new UserResponseDTO { IsSucceed = false, Errors = $"{result.Errors}" });
            }
            catch(Exception ex)
            {
                return BadRequest(new UserResponseDTO { IsSucceed = false, Errors = $"{ex.Message}" });
            }
        }

        /// <summary>
        /// Удаление сущности пользователя
        /// </summary>
        /// <param name="id">ID сущности</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await repository.FindByIdAsync(id);
            if (user == null)
                return NotFound(new UserResponseDTO { IsSucceed = false, Errors = "Пользователь не найден" });

            var result = await repository.DeleteAsync(user);

            if (result.Succeeded) return Ok(new UserResponseDTO { IsSucceed = true });
            else return BadRequest(new UserResponseDTO { IsSucceed = false, Errors = $"{result.Errors}" });
        }

        /// <summary>
        /// Обновление сущности пользователя
        /// </summary>
        /// <param name="userUpdate">Сущность</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserUpdateDTO userUpdate)
        {
            var user = await repository.FindByIdAsync(userUpdate.Id);

            if (user == null)
                return NotFound("Пользователь не найден");

            user.UserName = userUpdate.UserName;
            user.Email=userUpdate.Email;
            user.NormalizedEmail = userUpdate.Email;
            user.IsActive = userUpdate.IsActive;
            user.IsAdmin = userUpdate.IsAdmin;
            user.ProfileImg = userUpdate.ProfileImg;
            user.Fullname = userUpdate.Fullname;
            user.UpdatedAt = DateTime.UtcNow;
            user.PhoneNumber = userUpdate.Phone;
            user.Post = userUpdate.Post;

            if (string.IsNullOrEmpty(userUpdate.Password))
            {
                var result = await repository.UpdateAsync(user);

                if (result.Succeeded) return Ok(new UserResponseDTO
                {
                    IsSucceed = true,
                    Id = userUpdate.Id,
                    UserName = user.UserName,
                    Fullname = user.Fullname
                });
                else return BadRequest(new UserResponseDTO { IsSucceed = false, Errors = $"{result.Errors}" });
            }
            else
            {
                var passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<UserEntity>)) as IPasswordValidator<UserEntity>;
                var passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<UserEntity>)) as IPasswordHasher<UserEntity>;

                IdentityResult identityResult = await passwordValidator.ValidateAsync(repository, user, userUpdate.Password);
                if(identityResult.Succeeded)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, userUpdate.Password);

                    await repository.UpdateAsync(user);

                    return Ok(new UserResponseDTO
                    {
                        IsSucceed = true,
                        Id = userUpdate.Id,
                        UserName = user.UserName,
                        Fullname = user.Fullname
                    });
                }
                else
                {
                    return BadRequest(new UserResponseDTO { IsSucceed = false, Errors = $"{identityResult.Errors}" });
                }
            }
            
        }
        #endregion
    }
}
