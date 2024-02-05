using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
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
                        Microsoft.Extensions.Primitives.StringValues conn = "??";
                        Microsoft.Extensions.Primitives.StringValues containername = "??";
                        req.Form.TryGetValue("Connection", out conn);
                        req.Form.TryGetValue("ContainerName", out containername);

                        string connStr = conn[0];
                        string containernameStr = containername[0];

                        string Connection = connStr;
                        string containerName = containernameStr;
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

    public static class FileDownload {
        [FunctionName("FileDownload")]
        public static async Task < IActionResult > Run(
            //[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log) {
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, ILogger log) {
                string rtnStr = "";
                try {
                        Microsoft.Extensions.Primitives.StringValues conn = "??";
                        Microsoft.Extensions.Primitives.StringValues containername = "??";
                        Microsoft.Extensions.Primitives.StringValues filetodownload = "??";
                        Microsoft.Extensions.Primitives.StringValues downloadpath = "??";
                        req.Form.TryGetValue("Connection", out conn);
                        req.Form.TryGetValue("ContainerName", out containername);
                        req.Form.TryGetValue("FileToDownload", out filetodownload);
                        req.Form.TryGetValue("DownloadPath", out downloadpath);

                        string connStr = conn[0];
                        string containernameStr = containername[0];
                        string filetodownloadStr = filetodownload[0];
                        string downloadpathStr = downloadpath[0];

                        string filetodownloadNameStr = filetodownloadStr.Split('.')[0] + "_DL" + "." + filetodownloadStr.Split('.')[1];

                        string Connection = connStr;
                        string containerName = containernameStr;
                        
                        CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(Connection);  
                        CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();  

                        CloudBlobContainer container = blobClient.GetContainerReference(containerName);  
                        CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filetodownloadStr); 

                        Stream file = File.OpenWrite(downloadpathStr + filetodownloadNameStr);
                        await cloudBlockBlob.DownloadToStreamAsync(file);
                        await file.FlushAsync();
                        file.Close();

                        rtnStr = "file downloaded successfylly";    
                    } catch(Exception ex) {
                        rtnStr = "There was an error: " + ex.ToString();
                    }
                return new OkObjectResult(rtnStr);
        }
    }
}