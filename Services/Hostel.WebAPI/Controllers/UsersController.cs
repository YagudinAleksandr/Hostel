using Hostel.DAL.Entities;
using Hostel.Infrastructure.Pagination.Entities;
using Hostel.Infrastructure.Pagination.Repositories;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hostel.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Поля
        private UserManager<UserEntity> repository;
        #endregion

        #region Базовые компоненты
        public UsersController(UserManager<UserEntity> repository)
        {
            this.repository = repository;
        }
        #endregion

        #region Публичные методы
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PageParametrs pageParametrs)
        {
            var users = await repository.Users.ToListAsync();

            var page = PagedList<UserEntity>.ToPagedList(users, pageParametrs.PageNumber, pageParametrs.PageSize);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(page.MetaData));

            return Ok(page);
        }
        #endregion
    }
}
