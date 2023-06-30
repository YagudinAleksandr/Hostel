using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http.Headers;
using Hostel.Infrastructure.Repositories;
using Hostel.Domain.DTO.FilesDTOs;

namespace Hostel.BlazorUI.Components
{
    public partial class FileUploadComponent
    {
        [Parameter]
        public FileUploadResponseDTO FileUrl { get; set; }

        [Parameter]
        public string FileFilter { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; }

        [Parameter]
        public EventCallback<FileUploadResponseDTO> OnChange { get; set; }

        [Inject]
        public IWebFilesRepository<FileUploadResponseDTO,FileDeleteResponseDTO> FilesRepository { get; set; }

        private async Task HandleSelected(InputFileChangeEventArgs e)
        {
            var files = e.GetMultipleFiles();

            foreach (var file in files)
            {
                if (file is not null)
                {
                    using (var ms = file.OpenReadStream(file.Size))
                    {
                        var content = new MultipartFormDataContent();

                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                        content.Add(new StreamContent(ms, Convert.ToInt32(file.Size)), "file", file.Name);

                        FileUrl = await FilesRepository.Upload(content);

                        await OnChange.InvokeAsync(FileUrl);
                    }
                }
            }
        }

    }
}
