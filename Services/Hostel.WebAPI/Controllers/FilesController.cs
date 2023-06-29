using Hostel.Domain.DTO.FilesDTOs;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Net.Http.Headers;
using System.Linq;

namespace Hostel.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Upload([FromBody] FileUploadRequestDTO fileInfo)
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("wwwroot", fileInfo.Directory);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    string newFileName = Guid.NewGuid().ToString() + "." + fileName.Split('.').Last();

                    var fullPath = Path.Combine(pathToSave, newFileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new FileUploadResponseDTO { IsSuccesful = true, FileName = newFileName });
                }
                else
                {
                    return BadRequest(new FileUploadResponseDTO { IsSuccesful = false, Errors = "Ошибка объема файла" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new FileUploadResponseDTO { Errors = ex.ToString(), IsSuccesful=false });
            }
        }


        [HttpDelete]
        public IActionResult Delete([FromBody] FileDeleteRequestDTO fileInfo)
        {
            try
            {
                var folderName = Path.Combine("wwwroot", fileInfo.Directory);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var pathToFile = Path.Combine(pathToSave, fileInfo.Filename);

                if (System.IO.File.Exists(pathToFile))
                    System.IO.File.Delete(pathToFile);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка на сервере: {ex}");
            }
        }
    }
}
