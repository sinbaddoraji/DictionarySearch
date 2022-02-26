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
        readonly string[] Words;

        public enum SearchConditon { StartsWith, EndsWith, Contains, ExactMatch, SimilarTo, Regex, None}
        public WordDictionary()
        {
            Words = Properties.Resources.words.Split('\n');
        }

        public IEnumerable<string> Search(int wordLength = -1, string pattern = "", SearchConditon searchConditon = SearchConditon.ExactMatch)
        {

            IEnumerable<string> innerDictionary;

            if (wordLength > -1)
                innerDictionary = Words.Where(x => x.Length == wordLength);

            else
                innerDictionary = Words.ToList();

            switch (searchConditon)
            {
                case SearchConditon.StartsWith:
                    return innerDictionary.Where(x => x.StartsWith(pattern));
                case SearchConditon.EndsWith:
                    return innerDictionary.Where(x => x.EndsWith(pattern));
                case SearchConditon.Contains:
                    return innerDictionary.Where(x => x.Contains(pattern));
                case SearchConditon.ExactMatch:
                    return innerDictionary.Where(x => x == pattern);
                case SearchConditon.SimilarTo:
                    return innerDictionary.Where(x => CalculateSimilarity(x, pattern) > 70);
                case SearchConditon.Regex:
                    return innerDictionary.Where(x => Regex.IsMatch(x, pattern));
                default: return innerDictionary;
            }
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
