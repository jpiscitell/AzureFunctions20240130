using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AZUREFUNCTIONS20240130.classes {
    public class Process1 {
        public string processval1(AZUREFUNCTIONS20240130.classes.somevals sv)
        {
            string rtnval = "";

            if(rtnval == "")
            {
                rtnval = sv.id.ToString() + "|" + sv.dateCreated.ToString() + "|" + sv.minutes.ToString() + "|" + sv.description.ToString();
                if(sv.subvals.Count() > 0)
                {
                    for (int t=0; t<sv.subvals.Count(); t++)
                    {
                        rtnval = rtnval + "(<" + sv.subvals[t].subval1.ToString() + "><" + sv.subvals[t].subval2.ToString() + ">)";
                    }
                }
            } else {
                rtnval = rtnval  + "?" + sv.id.ToString() + "|" + sv.dateCreated.ToString() + "|" + sv.minutes.ToString() + "|" + sv.description.ToString();
                if(sv.subvals.Count() > 0)
                {
                    for (int t=0; t<sv.subvals.Count(); t++)
                    {
                        rtnval = rtnval + "(<" + sv.subvals[t].subval1.ToString() + "><" + sv.subvals[t].subval2.ToString() + ">)";
                    }
                }
            }
            return rtnval;
        }
    }
}