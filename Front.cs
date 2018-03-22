using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWebScraper
{
    class Front
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Jorda\Documents\visual studio 2015\Projects\SDWebScraper\SDWebScraper\bin\Debug\URLList.txt");

            System.Console.WriteLine("Contents of URLList.txt =:");
            foreach (string line in lines)
            {
                string source = HTMLWorker.getSource(line);
                ArgosParser.beginParse(source);
            }

            // Keep the console window open
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }

        private static void saveSourceToFile(string source)
        {
            StreamWriter sw = new StreamWriter("argos.txt");
            sw.Write(source);
            sw.Close();
        }
    }
}
