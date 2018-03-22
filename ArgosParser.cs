using System;

namespace SDWebScraper
{
    public static class ArgosParser
    {
        public static void beginParse(string source)
        {
            // Begin Parsing.
            searchForProductPanel(source);
        }

        private static void searchForProductPanel(string source)
        {
            // Used to determine when closing tag has been found.
            bool continueRunning = true;

            while (continueRunning == true)
            {
                // Locate start of product card.
                int indexOfCardStart = source.IndexOf("ac-product-link ac-product-card__details") - 10;

                // If indexOfCardStart is -11, no more products located on the page.
                // -11 since string.IndexOf returns -1 if search is not found, minus the 10 to locate the opening "<".
                if (indexOfCardStart != -11)
                {
                    // Extract codeblock with data inside.
                    string codeblock = findCodeBlock(indexOfCardStart, source);

                    // Remove selected codeblock from code.
                    source = source.Replace(codeblock, "");

                    // Parse codeblock for info.
                    parseCodeBlock(codeblock);
                }
                else
                {
                    // No products left on page.
                    continueRunning = false;
                }
            } 
        }

        private static string findCodeBlock(int currentIndex, string source)
        {
            // Save startIndex for later use.
            int startIndex = currentIndex;
            // Used to find the ending >. 
            // Logic for this is that for every opening tag (<), there must be one closing tag (>).
            // So every opening tag, bracketThisLoops is incremented by 1, and every closing tag
            // bracetsThisLoop is reduced by 1. (There are a few exclusions to this rule, but they
            // have been coded in already.)
            short bracketsThisLoop = 1;
            bool isSearchDone = false;

            do
            {
                // Increase index by 1
                currentIndex++;

                // Search for starting tags
                if ((source.Substring(currentIndex, 1) == "<") && (source.Substring(currentIndex + 1, 1) != "/"))
                {
                    bracketsThisLoop++;
                }

                // Search for ending tags
                else if ((source.Substring(currentIndex, 1) == "<") && (source.Substring(currentIndex + 1, 1) == "/"))
                {
                    bracketsThisLoop--;
                }

                // Clean up numbers
                if ((source.Substring(currentIndex, 1) == "<") && (source.Substring(currentIndex + 1, 1) == "!"))
                {
                    bracketsThisLoop--;
                }

                if ((source.Substring(currentIndex, 1) == "/") && (source.Substring(currentIndex + 1, 1) == ">"))
                {
                    bracketsThisLoop--;
                }

                // If bracketsThisLoop is 0, all opening tags are accounted for
                if (bracketsThisLoop == 0)
                {
                    isSearchDone = true;
                }
            } while (isSearchDone == false);

            // Select code block and copy it so variable "substring". The plus 4 in this
            // statement is due to the length of the closing tag (</div>)
            string substring = source.Substring(startIndex, (currentIndex - startIndex + 4));
            return substring;
        }

        private static void parseCodeBlock(string codeblock)
        {
            // Formatting for output
            const string format = "{0,-80} {1,-10} {2,-18} {3,-10}";
            string name;
            string prodnumber;
            string rating;
            string price;
            
            
            string code = codeblock;

            // Search for product name.
            int indexStart = code.IndexOf("aria-label=\"");
            indexStart = indexStart +12;
            int indexStop = indexStart;
            do
            {
                indexStop++;
            } while (code.Substring(indexStop, 1) != "\"");
            name = (code.Substring(indexStart, indexStop - indexStart));
            
            // Search for product number
            indexStart = code.IndexOf("/product/");
            indexStart = indexStart + 8;
            indexStop = indexStart;
            do
            {
                indexStop++;
            } while (code.Substring(indexStop, 1) != "\"");
            prodnumber = (code.Substring(indexStart + 1, indexStop - indexStart - 1));

            // Search for rating (out of 5)
            indexStart = code.IndexOf("data-star-rating=\"");
            indexStart = indexStart + 17;
            if (indexStart != 16)
            {
                indexStop = indexStart;
                do
                {
                    indexStop++;
                } while (code.Substring(indexStop, 1) != "\"");
                rating = (code.Substring(indexStart + 1, indexStop - indexStart - 1));
            }
            else
            {
                rating = ("Rating not found");
            }            

            // Search for price
            indexStart = code.IndexOf("<meta itemProp=\"price\" content=\"");
            indexStart = indexStart + 31;
            indexStop = indexStart;
            do
            {
                indexStop++;
            } while (code.Substring(indexStop, 1) != "\"");
            price = ("£" + code.Substring(indexStart + 1, indexStop - indexStart - 1));

            string output = string.Format(format, name, prodnumber, rating, price);
            Console.WriteLine(output);
        }
    }
}
