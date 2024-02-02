using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
namespace My.Functions
{
    public static class FileUpload {
        [FunctionName("FileUpload")]
        public static async Task < IActionResult > Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log) {
                string rtnStr = "";
                try {
                        string Connection = "DefaultEndpointsProtocol=https;AccountName=rrmstestpoc;AccountKey=JrvBvUPeVaxs6TsezGagYhifChwTZA222qX+VWozzMdXBEkDw4uf9BpOC9lh5B8cyU1WDa3lv01K+ASt4TDkig==;EndpointSuffix=core.windows.net";//Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                        string containerName = "jmp";//Environment.GetEnvironmentVariable("ContainerName");
                        Stream myBlob = new MemoryStream();
                        var file = req.Form.Files["File"];
                        myBlob = file.OpenReadStream();
                        var blobClient = new BlobContainerClient(Connection, containerName);
                        var blob = blobClient.GetBlobClient(file.FileName);
                        await blob.UploadAsync(myBlob);
                    rtnStr = "file uploaded successfylly";    
                } catch(Exception ex) {
                    rtnStr = "There was an error: " + ex.ToString();
                }
            return new OkObjectResult(rtnStr);
        }
    }
}