using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DictionarySearch
{
    internal class WordDictionary
    {
        private static WordDictionary _instance;

        public static WordDictionary Instance
        {
            get { return _instance ?? (_instance = new WordDictionary()); }
            set { _instance = value; }
        }

        IEnumerable<string> Words;

        public enum SearchConditon { StartsWith, EndsWith, Contains, ExactMatch, SimilarTo, Regex, None, WordAt}
        public WordDictionary()
        {
            Words = Properties.Resources.words.Split('\n').Select(x => x.ToLower());
        }

        public void Refresh()
        {
            Words = Properties.Resources.words.Split('\n').Select(x => x.ToLower());
        }

        private bool Contains(string str, string substr, bool inverse)
        {
            bool output = true;

            foreach (var s in substr.Split(' '))
            {
                if(inverse)
                    output &= !str.Contains(s);
                else
                    output &= str.Contains(s);
            }

            if (inverse)
                return !output;

            return output;
        }

       
        public IEnumerable<string> Search(int wordLength, string pattern, SearchConditon searchConditon, bool inverse, int index = -1)
        {

            IEnumerable<string> innerDictionary;

            if (wordLength > -1)
                
                innerDictionary = Words.Where(x => x.Length == wordLength);

            else
                innerDictionary = Words.ToList();


            switch (searchConditon)
            {
                case SearchConditon.StartsWith:
                    innerDictionary =  innerDictionary.Where(x => x.StartsWith(pattern,StringComparison.CurrentCultureIgnoreCase) == !inverse);
                    break;
                case SearchConditon.EndsWith:
                    innerDictionary = innerDictionary.Where(x => x.EndsWith(pattern, StringComparison.CurrentCultureIgnoreCase) == !inverse);
                    break;
                case SearchConditon.Contains:
                    innerDictionary = innerDictionary.Where(x => Contains(x,pattern,inverse) == !inverse);
                    break;
                case SearchConditon.ExactMatch:
                    innerDictionary = innerDictionary.Where(x => x.Equals(pattern, StringComparison.CurrentCultureIgnoreCase));
                    break;
                case SearchConditon.SimilarTo:
                    innerDictionary = innerDictionary.Where(x => CalculateSimilarity(x, pattern) >= 65);
                    break;
                case SearchConditon.WordAt:
                    if(index > 0)
                        innerDictionary = innerDictionary.Where(x => x.Length >= index  && (x[index - 1] == pattern[0]) == !inverse);
                    break;
                case SearchConditon.Regex:
                    innerDictionary = innerDictionary.Where(x => Regex.IsMatch(x, pattern) == !inverse);
                    break;
            }

            Words = innerDictionary;
            return innerDictionary;
        }

        int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Initalize 2d array
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Calculate the cost between letter at index i and j
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    //Note the minumum distance beteen letter at index i and letter at index j
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }

        double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length))) * 100;
        }
    }
}
