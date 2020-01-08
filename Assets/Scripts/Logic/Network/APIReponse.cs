using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Logic.Network
{
    public class APIReponse
    {
        public int StatusCode { get; set; }
        public JSONNode Response { get; set; }

        public static APIReponse textToReponse(string responseJson)
        {
            var jsonDB = JSON.Parse(responseJson);
            return new APIReponse
            {
                StatusCode = jsonDB["StatusCode"].AsInt,
                Response = jsonDB["Response"]
            };
        }
    }

}
