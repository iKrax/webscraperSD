using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDWebScraper
{
    public static class ArgosParser
    {
        public static void beginParse(string source)
        {
            searchForProductPanel(source);
            //Console.ReadLine();
        }

        private static void searchForProductPanel(string source)
        {
            bool continueRunning = true;

            while (continueRunning == true)
            {
                //Calculate start of product card
                int indexOfCardStart = source.IndexOf("ac-product-link ac-product-card__details") - 10;

                if (indexOfCardStart != -11)
                {
                    //Extract codeblock with data inside
                    string codeblock = findCodeBlock(indexOfCardStart, source);

                    //Remove selected codeblock from code
                    source = source.Replace(codeblock, "");

                    //Parse codeblock for info
                    parseCodeBlock(codeblock);
                }
                else
                {
                    continueRunning = false;
                }
            } 
        }

        private static string findCodeBlock(int currentIndex, string source)
        {
            int startIndex = currentIndex;
            short bracketsThisLoop = 1;
            bool isSearchDone = false;

            do
            {
                //Increase index
                currentIndex++;

                //Search for starting tags
                if ((source.Substring(currentIndex, 1) == "<") && (source.Substring(currentIndex + 1, 1) != "/"))
                {
                    bracketsThisLoop++;
                }

                //Search for ending tags
                else if ((source.Substring(currentIndex, 1) == "<") && (source.Substring(currentIndex + 1, 1) == "/"))
                {
                    bracketsThisLoop--;
                }

                //Clean up numbers
                if ((source.Substring(currentIndex, 1) == "<") && (source.Substring(currentIndex + 1, 1) == "!"))
                {
                    bracketsThisLoop--;
                }

                if ((source.Substring(currentIndex, 1) == "/") && (source.Substring(currentIndex + 1, 1) == ">"))
                {
                    bracketsThisLoop--;
                }

                //If bracketsThisLoop is 0, all opening tags are accounted for
                if (bracketsThisLoop == 0)
                {
                    isSearchDone = true;
                }

            } while (isSearchDone == false);

            string substring = source.Substring(startIndex, (currentIndex - startIndex + 4));
            return substring;
        }

        private static void parseCodeBlock(string codeblock)
        {
            string code = codeblock;
            int indexStart = code.IndexOf("aria-label=\"");
            indexStart = indexStart +12;
            int indexStop = indexStart;
            do
            {
                indexStop++;
            } while (code.Substring(indexStop, 1) != "\"");
            Console.Write(code.Substring(indexStart, indexStop - indexStart) + " | ");

            indexStart = code.IndexOf("/product/");
            indexStart = indexStart + 8;
            indexStop = indexStart;
            do
            {
                indexStop++;
            } while (code.Substring(indexStop, 1) != "\"");
            Console.Write(code.Substring(indexStart + 1, indexStop - indexStart - 1) + " | ");

            indexStart = code.IndexOf("data-star-rating=\"");
            indexStart = indexStart + 17;
            if (indexStart != 16)
            {
                indexStop = indexStart;
                do
                {
                    indexStop++;
                } while (code.Substring(indexStop, 1) != "\"");
                Console.Write(code.Substring(indexStart + 1, indexStop - indexStart - 1) + " of 5 | ");
            }
            else
            {
                Console.Write("Rating not found | ");
            }            

            indexStart = code.IndexOf("<meta itemProp=\"price\" content=\"");
            indexStart = indexStart + 31;
            indexStop = indexStart;
            do
            {
                indexStop++;
            } while (code.Substring(indexStop, 1) != "\"");
            Console.WriteLine("£" + code.Substring(indexStart + 1, indexStop - indexStart - 1) + " | ");
        }
    }
}
