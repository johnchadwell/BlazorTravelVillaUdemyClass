using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TravelVillaServer.Service.IService;

using Microsoft.JSInterop;

namespace TravelVillaServer.Service
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IJSRuntime _JSRun;

        //public FileUpload(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        public FileUpload(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IJSRuntime JSRun)
        {
            _webHostEnvironment = webHostEnvironment;
            //_httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _JSRun = JSRun;
        }
        
        public bool DeleteFile(string fileName)
        {
            bool status = false;
            try
            {

                var path = $"{_webHostEnvironment.WebRootPath}\\RoomImages\\{fileName}";
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> UploadFile(IBrowserFile file)
        {
            try
            {
                FileInfo fileinfo = new FileInfo(file.Name);
                var fileName = Guid.NewGuid().ToString() + fileinfo.Extension;

                await _JSRun.InvokeVoidAsync("showsalert", "_webHostEnvironment.WebRootPath", $"{_webHostEnvironment.WebRootPath}");
                var folderDir = $"{_webHostEnvironment.WebRootPath}\\RoomImages";

                await _JSRun.InvokeVoidAsync("showsalert", "folderDir", folderDir);
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "RoomImages", fileName);

                await _JSRun.InvokeVoidAsync("showsalert", "path", path);
                var memoryStream = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(memoryStream);

                if (Directory.Exists(folderDir))
                {
                    Directory.CreateDirectory(folderDir);
                }

                await using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fs);
                }
                //var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/";
                var url = $"{_configuration.GetValue<string>("ServerUrl")}";
                var fullPath = $"{url}RoomImages/{fileName}";
                return fullPath;

            } catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
