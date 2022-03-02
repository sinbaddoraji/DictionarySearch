using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionarySearch
{
    internal class Program
    {
        static int wordLength = -1;
        static string pattern = "";
        static WordDictionary.SearchConditon searchConditon = WordDictionary.SearchConditon.None;
        static bool isInverse = false;

        static void Main(string[] args)
        {
            StartSearch();
            Console.WriteLine("Application about to exit... ");
            Console.ReadKey();
        }

        static void StartSearch()
        {
            GetWordLength();

            GetSearchType();

            DisplaySearch();

            Console.WriteLine();
            Console.Write("Do you want to Continue? Y/N");
            char response = Console.ReadKey().KeyChar;

            if (response == 'Y' || response == 'y')
            {
                StartSearch();
            }
        }

        private static void DisplaySearch()
        {
           
            int index = -1;
            if ((int)searchConditon == 8-1)
            {
                Console.WriteLine("Note index starts from 1");
                Console.WriteLine();

                Console.Write("At index :");
                int.TryParse(Console.ReadLine(), out index);

                Console.WriteLine();
                Console.Write("Search for? ");
                pattern = Console.ReadKey().KeyChar.ToString();
            }
            else
            {
                Console.WriteLine();
                Console.Write("Search for? ");
                pattern = Console.ReadLine();
            }

            Console.WriteLine("Searching...\n\n");

            var results = WordDictionary.Instance.Search(wordLength, pattern, searchConditon, isInverse, index);
            Console.WriteLine("Results: ");

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        private static void GetWordLength()
        {
            if (wordLength != -1)
                return;

            Console.WriteLine();
            Console.Write("Specify Word Length? Y/N: ");
            var response = Console.ReadKey().KeyChar;
           

            if (response == 'Y' || response == 'y')
            {
                Console.WriteLine();
                Console.Write("Word Length?");
                wordLength = int.Parse(Console.ReadLine());
            }
        }

        private static void GetSearchType()
        {
            Console.WriteLine();
            Console.WriteLine("Negate Search by making input negative");
            Console.WriteLine("For example, -1 means not start with input");
            Console.WriteLine("Select Search type");
            Console.WriteLine("(1) Starts With");
            Console.WriteLine("(2) Ends With");
            Console.WriteLine("(3) Contains");
            Console.WriteLine("(5) Exact Match");
            Console.WriteLine("(6) Similar To");
            Console.WriteLine("(7) Regex");
            Console.WriteLine("(8) Word at Index");
            Console.WriteLine("(9) Refresh word list");
            Console.Write(" : ");

            int.TryParse(Console.ReadLine(), out int sTyChoice);

            while (sTyChoice < -9 || sTyChoice > 8)
            {
                if(sTyChoice == 9)
                {
                    WordDictionary.Instance.Refresh();
                    Console.WriteLine("Dictionally successfully reset");
                }
                Console.WriteLine("Please input value between 1 and 8\n");
                Console.Write(" : ");
                int.TryParse(Console.ReadLine(), out sTyChoice);
            }


            isInverse = sTyChoice < 0;
            searchConditon = (WordDictionary.SearchConditon)Math.Abs(sTyChoice) - 1;
        }
    }
}
