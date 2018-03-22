using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SDWebScraper
{
    class HTMLWorker
    {
        public static string getSource(string url)
        {
            //Create Request
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            //Get Response
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            //Read Response
            StreamReader sr = new StreamReader(resp.GetResponseStream());

            //Output to string
            string source = sr.ReadToEnd();

            //Close all the things
            sr.Close();
            resp.Close();

            return source;
        }

    }
}
