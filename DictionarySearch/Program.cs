using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionarySearch
{
    internal class Program
    {
        static WordDictionary wordDictionary;
        static int wordLength = -1;
        static string pattern = "";
        static WordDictionary.SearchConditon searchConditon = WordDictionary.SearchConditon.None;


        static void Main(string[] args)
        {
            Console.WriteLine("Loading Dictionary...");
            WordDictionary wordDictionary = new WordDictionary();
            Console.Clear();


            Console.WriteLine();
            Console.Write("Specify Word Length? Y/N");
            var response = Console.ReadKey().KeyChar;

            if(response == 'Y' || response == 'y')
            {
                Console.Write("Word Length?");
                wordLength = int.Parse(Console.ReadLine());
            }

            Console.WriteLine();
            Console.WriteLine("Select Search type");

            Console.WriteLine("(1) Starts With");
            Console.WriteLine("(2) Ends With");
            Console.WriteLine("(3) Contains");
            Console.WriteLine("(4) Exact Match");
            Console.WriteLine("(5) Similar To");
            Console.WriteLine("(6) Regex");
            Console.WriteLine("(7) None");
            Console.Write(" : ");
            searchConditon = (WordDictionary.SearchConditon)int.Parse(Console.ReadLine()) - 1
                ;

            Console.WriteLine();
            Console.Write("Search for? ");
            pattern = Console.ReadLine();

            Console.WriteLine("Searching...\n\n");



            var results = wordDictionary.Search(wordLength, pattern, searchConditon);
            Console.WriteLine("Results: ");

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }

            Console.ReadKey();
        }
    }
}
