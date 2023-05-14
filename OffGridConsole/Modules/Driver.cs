using OffGridConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace OffGridConsole.Modules
{
    struct Command {
        public string status;
        public Dictionary<string, string> consumers;   
    }

    internal class Driver : IDriver
    {
        public int SendCommand(string token, Dictionary<string, string> consumers, string status) {
            // http://193.6.19.58:2023/offgrid/control/wqeasd

            Command c1 = new Command();
            c1.status = status;
            c1.consumers = consumers;

            string jCommand = JsonConvert.SerializeObject(c1);

            string url = "http://193.6.19.58:2023/offgrid/control/" + token;

            var request = WebRequest.Create(url);
            request.Method = "POST";

            byte[] byteArray = Encoding.UTF8.GetBytes(jCommand);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            var reqStream = request.GetRequestStream();
            reqStream.Write(byteArray, 0, byteArray.Length);

            var response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            var respStream = response.GetResponseStream();

            var reader = new StreamReader(respStream);
            string data = reader.ReadToEnd();
            Console.WriteLine(data);




            return 1;
        }
    }
}
