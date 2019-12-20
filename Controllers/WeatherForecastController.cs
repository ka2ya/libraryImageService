using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Storage.v1;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace libraryCoverImageMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
            private readonly ILogger<WeatherForecastController> _logger;
        StorageClient storageClient;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            var credential = GoogleCredential.FromFile("./API_Project-3dcd77bea01f.json");

            storageClient = StorageClient.Create(credential);
            _logger = logger;
        }
/*
        [HttpGet("{id}")]
        public Task<IActionResult> Get(String id)
        {
            Object imageObject=storageClient.GetObject("library_media_cover",id);
            imageObject.
            return File(,"image/jpg");
        }
*/
        [HttpPost]
        public String Post([FromForm]IFormFile image)
        {
            if(image==null){
                return "error";
            }
            String uri = "https://storage.googleapis.com/{0}/{1}";
            Guid newImageGuid = Guid.NewGuid();
            Console.WriteLine("saving image");

            using(var imageMemoryStream=new MemoryStream()){
                image.CopyTo(imageMemoryStream);
                Console.WriteLine(newImageGuid);
                storageClient.UploadObject("library_media_cover", newImageGuid.ToString(), "image/jpeg", new MemoryStream(imageMemoryStream.ToArray()));
            }
            String completeUri=String.Format(uri, "library_media_cover", newImageGuid.ToString());
            Console.WriteLine(completeUri);
            return completeUri;
        }

        [HttpDelete]
        public Boolean delete(Int16 id)
        {

            return false;
        }
    }
}
