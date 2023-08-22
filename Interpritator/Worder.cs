using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1Tech.Interpritator
{
    public class Worder
    {
        //List<TechDictionary> vec = new List<TechDictionary>();
        public SortedList<string, long[]> pairs = new();
        public Worder(List<TechDictionary> searchingWords/*,string wayToDictionaryFolder*/)
        {

            DictResearch(searchingWords);
            DictBuilder(pairs);

        }
        void DictResearch(List<TechDictionary> searchingWords)
        {
            SortedList<string, long[]> wordPair = new();
            foreach (var tecDic in searchingWords)
            {
                wordPair.Add(tecDic.Word, tecDic.VacancyID);
            }
            RecursiveShot(wordPair);


        }

        void RecursiveShot(SortedList<string, long[]> pair)
        {
            if (pair.Count > 1)
            {
                for (var i = 1; i < pair.Count; i++)
                {
                    IEnumerable<long> both = pair.Values[0].Intersect(pair.Values[i]);
                    if (both.Any())
                    {
                        long[] bothInt = new long[both.Count()];
                        int counter = 0;
                        foreach (var t in both)
                        {
                            bothInt[counter++] = t;
                        }
                        SortedList<string, long[]> result = new();
                        result.Add($"{pair.Keys[0]} {pair.Keys[i]}", bothInt);///////
                        for (int j = 1; j < pair.Count; j++)
                        {
                            if (j != i)
                            {
                                result.Add(pair.Keys[j], pair.Values[j]);
                            }

                        }
                        RecursiveShot(result);
                        foreach (var ult in result)
                        {
                            try
                            {
                                pairs.Add(ult.Key, ult.Value);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }
            }
        }
        void DictBuilder(SortedList<string, long[]> pairsResult)
        {

        }
    }

    internal class Wr1wr2
    {
        public long[] VacancyIDHash { get; set; }
        private List<TechDictionary> Pairs { get; set; }
        public Wr1wr2(long[] vacancyIDHash, List<TechDictionary> pairs)
        {
            VacancyIDHash = vacancyIDHash;
            Pairs = pairs;
        }
    }

}
