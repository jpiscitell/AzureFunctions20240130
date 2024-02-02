using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using AZUREFUNCTIONS20240130.classes;
using System.Collections.Generic;

namespace My.Functions
{
    public static class ComplexHttpTriggerFunction
    {
        [FunctionName("ComplexHttpTriggerFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            ProcessData pd = new ProcessData();
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            

            JArray array = JArray.Parse(requestBody);

            string xxx = pd.BreakArrayVals(array);
            
            return new OkObjectResult(xxx);
        }
    }

    public class ProcessData {
        public string BreakArrayVals(JArray array)
        {
            string rtnval = "";
            foreach(JObject obj in array.Children<JObject>())
            {
                //somevals sv = obj.ToObject<somevals>();
                AZUREFUNCTIONS20240130.classes.somevals sv = obj.ToObject<AZUREFUNCTIONS20240130.classes.somevals>();
             
                if(rtnval == "")
                {
                    rtnval = ProcessTheVal(sv);
                } else {
                    rtnval = rtnval + "~~~" + ProcessTheVal(sv);
                }
                
            }
            return rtnval;
        }

        public string ProcessTheVal(AZUREFUNCTIONS20240130.classes.somevals sv)
        {
            string rtnval = "";
            sv.description = alterVAL(sv.description); 
            sv.subvals[0].subval2 = alterVAL(sv.subvals[0].subval2);
            List<string> phones = new List<string>();
            phones.Add("3154475230");
            phones.Add("3157663842");

            switch (sv.process)
            {
                case "process1":
                    Process1 p1 = new Process1();
                    rtnval = p1.processval1(sv);                    
                break;
                case "process2":
                    Process2 p2 = new Process2();
                    p2.sendMessage(sv.description, phones.ToArray()); 
                break;
                case "process3":
                    Process3 p3 = new Process3();
                    string host = sv.sftpcreds[0].host;
                    int port = sv.sftpcreds[0].port;
                    string username = sv.sftpcreds[0].username;
                    string password = sv.sftpcreds[0].password;
                    string remotepath = "/upload/processed";
                    string updown = "u";
                    string uploadFilePath = @"C:\temp\";
                    string uploadFileName = @"TestFile.csv"; 
                    string downloadFilePath = @"C:\temp\";
                    string fileToDownload = "TestFile2.txt";
                    datasetdata[] ds = sv.dataset;

                    string sfcsv = ",Site Number,Site Name,Contact Number,To Phone,Last Name,First Name,From Phone,Call Time,Call Length in Minutes";
                    for(int dsl = 0; dsl < ds.Count(); dsl++)
                    {
                        if(sfcsv == "")
                        {
                            sfcsv = ds[dsl].id + "," + ds[dsl].sitenum.ToString() + "," + ds[dsl].sitename.Replace(',', ' ') + "," + ds[dsl].contactnum.ToString()
                             + "," + ds[dsl].tophone + "," + ds[dsl].lastname + "," + ds[dsl].firstname + "," + ds[dsl].fromphone
                             + "," + ds[dsl].calltime.ToString() + "," + ds[dsl].calllengthminutes.ToString();
                        } else {
                            sfcsv = sfcsv + "\r" +  ds[dsl].id + "," + ds[dsl].sitenum.ToString() + "," + ds[dsl].sitename.Replace(',', ' ') + "," + ds[dsl].contactnum.ToString()
                             + "," + ds[dsl].tophone + "," + ds[dsl].lastname + "," + ds[dsl].firstname + "," + ds[dsl].fromphone
                             + "," + ds[dsl].calltime.ToString() + "," + ds[dsl].calllengthminutes.ToString();
                        }
                    }


                    UnicodeEncoding uniEncoding = new UnicodeEncoding();
                    if(sfcsv == "")
                    {
                        sfcsv = "val1,val2,val3,val4,val5,val6\rval7,val8,val9,val10,val11,val12\rval13,val14,val15,val16,val17,val18";
                    }

                    byte[] fs = uniEncoding.GetBytes(sfcsv);
                    MemoryStream uploadStream = new MemoryStream();
                    uploadStream.Write(fs, 0, fs.Length);

                    if(File.Exists(uploadFilePath + uploadFileName)){
                        DateTime dt = DateTime.Now;
                        uploadFileName = uploadFileName.Split('.')[0] + dt.Ticks.ToString() + "." + uploadFileName.Split('.')[1];
                    }

                    p3.testSFTP(updown,host,port,username,password, remotepath, uploadFilePath,downloadFilePath,uploadStream,uploadFileName,fileToDownload,fs);

                break;
                case "process4":
                break;
                case "process5":
                break;
                default:
                break;
            }

            return rtnval;


            // if(rtnval == "")
            // {
            //     rtnval = sv.id.ToString() + "|" + sv.dateCreated.ToString() + "|" + sv.minutes.ToString() + "|" + sv.description.ToString();
            //     if(sv.subvals.Count() > 0)
            //     {
            //         for (int t=0; t<sv.subvals.Count(); t++)
            //         {
            //             rtnval = rtnval + "(<" + sv.subvals[t].subval1.ToString() + "><" + sv.subvals[t].subval2.ToString() + ">)";
            //         }
            //     }
            // } else {
            //     rtnval = rtnval  + "?" + sv.id.ToString() + "|" + sv.dateCreated.ToString() + "|" + sv.minutes.ToString() + "|" + sv.description.ToString();
            //     if(sv.subvals.Count() > 0)
            //     {
            //         for (int t=0; t<sv.subvals.Count(); t++)
            //         {
            //             rtnval = rtnval + "(<" + sv.subvals[t].subval1.ToString() + "><" + sv.subvals[t].subval2.ToString() + ">)";
            //         }
            //     }
            // }
            // return rtnval;
        }
        
        public string alterVAL(string val)
        {
            DateTime dt = new DateTime();
            return val + dt.ToString();
        }
    }

    // public class somevals {
    //     [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    //     public int id { get; set; }
    //     [JsonProperty("process", NullValueHandling = NullValueHandling.Ignore)]
    //     public string process { get; set; }
    //     [JsonProperty("dateCreated", NullValueHandling = NullValueHandling.Ignore)]
    //     public string dateCreated { get; set; }
    //     [JsonProperty("minutes", NullValueHandling = NullValueHandling.Ignore)]
    //     public int minutes { get; set; }
    //     [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    //     public string description { get; set; }
    //     [JsonProperty("subvals", NullValueHandling = NullValueHandling.Ignore)]
    //     public somesubvals[] subvals {get; set;}
    // }

    // public class somesubvals {
    //     [JsonProperty("subval1", NullValueHandling = NullValueHandling.Ignore)]
    //     public int subval1 { get; set; }
    //     [JsonProperty("subval2", NullValueHandling = NullValueHandling.Ignore)]
    //     public string subval2 { get; set; }
    // }
}
