using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NRWA_Communication_Acceptance
{
    internal class Json
    {
        public static string ReadJsonFile(string jsonFileIn)
        {
            string jsonReturn = "";
            //Root myDeserializedClass = JsonConvert.DeserializeObject(myJsonResponse);

            //JToken creator = jsonFile.SelectToken("creator");
            //jsonReturn += creator.ToString();

            return jsonReturn;
        }


    }
}
