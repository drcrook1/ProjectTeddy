using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTeddy.Analytics;
using System.Net;
using System.IO;

namespace ProjectTeddy.Tester
{
    public class Program
    {
        static void Main(string[] args)
        {
            string tweet = "@T_Time30";
            //string s = ResponseEngine.GetBestResponse(tweet);
            WebClient client = new WebClient();
            client.Headers.Add("");
            Stream data = client.OpenRead("URL");
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            Console.WriteLine(s);
            Console.ReadKey();
        }
    }
}
