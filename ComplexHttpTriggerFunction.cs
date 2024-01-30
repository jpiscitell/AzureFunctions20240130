using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            // dynamic data = JsonConvert.DeserializeObject(requestBody);
            // name = name ?? data?.name;

            // var rsp = JsonConvert.DeserializeObject<somevals>(requestBody);

            // string responseMessage = string.IsNullOrEmpty(name)
            //     ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //     : $"Hello, {name}, {name}. This HTTP triggered function executed successfully." + data + " - " +
            //     rsp.id + " - " +
            //     rsp.dateCreated.ToString() + " - " +
            //     rsp.minutes.ToString() + " - " +
            //     rsp.description;

            // return new OkObjectResult(responseMessage);
        }


    }

    public class ProcessData {
        public string BreakArrayVals(JArray array)
        {
            string rtnval = "";
            foreach(JObject obj in array.Children<JObject>())
            {
                somevals sv = obj.ToObject<somevals>();
                sv.description = alterDESC(sv.description); 
                if(rtnval == "")
                {
                    rtnval = sv.id.ToString() + "|" + sv.dateCreated.ToString() + "|" + sv.minutes.ToString() + "|" + sv.description.ToString();
                } else {
                    rtnval = rtnval  + "?" + sv.id.ToString() + "|" + sv.dateCreated.ToString() + "|" + sv.minutes.ToString() + "|" + sv.description.ToString();
                }
            }
            return rtnval;
        }

        public string alterDESC(string description)
        {
            DateTime dt = new DateTime();
            return description + dt.ToString();
        }
    }

    public class somevals {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public int id { get; set; }
            [JsonProperty("dateCreated", NullValueHandling = NullValueHandling.Ignore)]
            public string dateCreated { get; set; }
            [JsonProperty("minutes", NullValueHandling = NullValueHandling.Ignore)]
            public int minutes { get; set; }
            [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
            public string description { get; set; }
    }
}
