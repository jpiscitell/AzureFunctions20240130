using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Renci.SshNet;


namespace AZUREFUNCTIONS20240130.classes {
    public class Process3 {

        public async void testSFTP(string updown, string host, int port, string username, string password, string remotepath, string uploadFilePath, string downloadFilePath, MemoryStream uploadStream, string uploadFileName, string fileToDownload, byte[] fs) {
            if(updown == "u")
            {
                bool uploadSuccess = await SFTPUpload(host,port,username,password,remotepath,uploadFileName,fs);
            }
            if(updown == "d")
            {            
                bool downloadSuccess =  await SFTPDownload(host,port,username,password,remotepath,downloadFilePath,fileToDownload);
            }
        }

        private async Task<bool> SFTPUpload(string host, int port, string username, string password, string remotepath, string uploadFileName, byte[] fs)
        {
            bool success = false;
            using SftpClient sftp1 = new(host,port,username,password);
            string ic = "NO";

            try {
                sftp1.Connect();
                
                if(sftp1.IsConnected)
                {
                    ic = "YES";

                    //get the status, not sure what we might do with this yet
                    var stat = sftp1.GetStatus(remotepath);

                    //give the file a unique name
                    DateTime dt = DateTime.Now;
                    string ulFilename = uploadFileName.Split('.')[0] + dt.Ticks.ToString() + "." + uploadFileName.Split('.')[1];

                    //process for uploading a file from passed in bytes to SFTP site
                    sftp1.WriteAllBytes(remotepath + "/" +  ulFilename, fs);
                }
                success = true;
            } catch (Exception ex)
            {
                string exS = ex.ToString();
            } finally {
                if(sftp1.IsConnected)
                {
                    sftp1.Disconnect();
                }
            }
            return success;
        }

        private async Task<bool> SFTPDownload(string host, int port, string username, string password, string remotepath, string downloadFilePath, string fileToDownload)
        {
            bool success = false;
            using SftpClient sftp1 = new(host,port,username,password);
            string ic = "NO";

            try {
                sftp1.Connect();
                
                if(sftp1.IsConnected)
                {
                    ic = "YES";

                    //give the downloaded file a unique name based on the filename on the sftp site
                    DateTime dt = DateTime.Now;
                    string dlFilename = fileToDownload.Split('.')[0] + dt.Ticks.ToString() + "." + fileToDownload.Split('.')[1];

                    //process for downloading file from SFTP to local disk
                    using (var fs = new FileStream(downloadFilePath + dlFilename, FileMode.Create))
                    {
                        sftp1.DownloadFile(remotepath + "/" +  fileToDownload, fs);
                    }

                } else {
                    ic = "NO";
                }
                success = true;
            } catch (Exception ex)
            {
                string exS = ex.ToString();
            } finally {
                if(sftp1.IsConnected)
                {
                    sftp1.Disconnect();
                }
            }
            return success;
        }
    }
}