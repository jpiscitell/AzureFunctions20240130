using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace AZUREFUNCTIONS20240130.classes {
    public class Process2 {
        public async void sendMessage(string smstext, string[] phones) {
            string rtnval = "";
            try
            {
                HttpClient httpClient1 = new HttpClient();
                httpClient1.BaseAddress = new Uri("https://east.rrmsalarm.com/webhook/api/sendsms/stagessinch");
                string sup = "smswebhook:2mqRyE2@2Oe3";
                var byteArray = Encoding.ASCII.GetBytes(sup);
                httpClient1.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                for(int i = 0; i < phones.Length; i++)
                {
                    string tPhone = phones[i].ToString();
                    string msg = "This is a personalized message from AZURE Function, from rapid tech number personalized for, " + tPhone + "-->" + smstext;
                    var content = new StringContent("{\"fromphone\": \"8336970289\",\"tophone\": \"" + tPhone + "\",\"message\": \"" + msg + "\",\"smsservice\": \"<sinch>Rapid_Tech\",\"callingapp\": \"sendsms\"}", Encoding.UTF8, "application/json");
                    var result = await httpClient1.PostAsync("https://east.rrmsalarm.com/webhook/api/sendsms/stagessinch", content);
                }


                rtnval = "success";
            } catch (Exception ex)
            {
                string exS = ex.ToString();
                rtnval = exS;
            }
            //return rtnval;
        }
    }
}