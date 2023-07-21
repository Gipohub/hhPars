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
        public SortedList<string, int[]> pairs = new();
        public Worder(List<TechDictionary> searchingWords/*,string wayToDictionaryFolder*/)
        {

            DictResearch(searchingWords);
            DictBuilder(pairs);

        }
        void DictResearch(List<TechDictionary> searchingWords)
        {
            SortedList<string, int[]> wordPair = new();
            foreach (var tecDic in searchingWords)
            {
                wordPair.Add(tecDic.Word, tecDic.VacancyID);
            }
            RecursiveShot(wordPair);


        }

        void RecursiveShot(SortedList<string, int[]> pair)
        {
            if (pair.Count > 1)
            {
                for (var i = 1; i < pair.Count; i++)
                {
                    IEnumerable<int> both = pair.Values[0].Intersect(pair.Values[i]);
                    if (both.Any())
                    {
                        int[] bothInt = new int[both.Count()];
                        int counter = 0;
                        foreach (var t in both)
                        {
                            bothInt[counter++] = t;
                        }
                        SortedList<string, int[]> result = new();
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
        void DictBuilder(SortedList<string, int[]> pairsResult)
        {

        }
    }

    internal class Wr1wr2
    {
        public int[] VacancyIDHash { get; set; }
        private List<TechDictionary> Pairs { get; set; }
        public Wr1wr2(int[] vacancyIDHash, List<TechDictionary> pairs)
        {
            VacancyIDHash = vacancyIDHash;
            Pairs = pairs;
        }
    }

}
