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

        public async void testSFTP(string host, int port, string username, string password) {
            bool uploadSuccess = await SFTPUpload(host,port,username,password);
            bool downloadSuccess =  await SFTPDownload(host,port,username,password);
        }

        private async Task<bool> SFTPUpload(string host, int port, string username, string password)
        {
            bool success = false;
            using SftpClient sftp1 = new(host,port,username,password);
            string ic = "NO";

            try {
                sftp1.Connect();
                
                if(sftp1.IsConnected)
                {
                    ic = "YES";

                    var stat = sftp1.GetStatus("/upload/processed");
                    var stat2 = "?";
                    stat2 = "##";

                    string filename = "TestFile.txt";
                    string ulFilename = "TestFile4.txt";

                    //process for uploading a file from local disk to SFTP site
                    using (FileStream fStream = File.Open(@"C:\temp\" + filename, FileMode.Open))
                    {
                        sftp1.UploadFile(fStream, "/upload/processed/" +  ulFilename, null);
                    }
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

        private async Task<bool> SFTPDownload(string host, int port, string username, string password)
        {
            bool success = false;
            using SftpClient sftp1 = new(host,port,username,password);
            string ic = "NO";

            try {
                sftp1.Connect();
                
                if(sftp1.IsConnected)
                {
                    ic = "YES";

                    var stat = sftp1.GetStatus("/upload/processed");
                    var stat2 = "?";
                    stat2 = "##";

                    string filename = "TestFile4.txt";
                    string ulFilename = "TestFile4.txt";

                    //process for downloading file from SFTP to local disk
                    using (var fs = new FileStream(@"C:\temp\" + ulFilename, FileMode.Create))
                    {
                        sftp1.DownloadFile("/upload/processed/" +  ulFilename, fs);
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