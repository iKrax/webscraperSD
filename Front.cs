using System;
using System.IO;

namespace SDWebScraper
{
    class Front
    {
        static void Main(string[] args)
        {
            // Window formatting
            try
            {
                Console.WindowWidth = 150;
                Console.WindowHeight = 35;
            }
            catch (Exception)
            {
                // Usually means monitor resolution program is being run on is small. 
                // If happens just leave the console default size.
            }

            // Set this to the URLList location.
            string[] lines = File.ReadAllLines(@"C:\Users\Jorda\Documents\visual studio 2015\Projects\SDWebScraper\SDWebScraper\bin\Debug\URLList.txt");

            // Split URL List into individual URLs and parse each URL.
            foreach (string line in lines)
            {
                string source = HTMLWorker.getSource(line);
                ArgosParser.beginParse(source);
            }

            // Keep the console window open
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }

        // Used for debugging
        private static void saveSourceToFile(string source)
        {
            StreamWriter sw = new StreamWriter("argos.txt");
            sw.Write(source);
            sw.Close();
        }
    }
}
