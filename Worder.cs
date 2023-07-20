using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1Tech
{
    public class Worder
    {
        List<TechDictionary> vec = new List<TechDictionary>();
        public Worder(string[] searchingWords, string wayToDictionaryFolder)
        {
            
            DictResearch(wayToDictionaryFolder);
           // WordShot(searchingWords);
        }
        void DictResearch(string wayToDictionaryFolder)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            //List<TechDictionary>? vec = new(); //обьявление словаря

            try
            {


                using var sr = new StreamReader(wayToDictionaryFolder);//чтение потока из указанного файла
                using var jr = new JsonTextReader(sr);// валидауция например
                vec = serializer.Deserialize<List<TechDictionary>>(jr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No acception Dictionary on this path way\n\r", ex.Message);
                //vec = ssDef;
            }
        }
        void WordShot(string[] searchingWords,out SortedList<string[], int[]> pair)
        {
            
            int counter = 0;
            var result = searchingWords.Where(x => !string.IsNullOrWhiteSpace(x));
            SortedList<string[], int[]> pairs = new();
            pairs = null;
            if (result.Any())// Any усовершенствованная версия "Array.Count() > 0"
            {
                foreach (var word in result)
                {
                    for(int i = 0; i < vec.Count; i++)
                    {
                        if (word == vec[i].Word)
                        {
                            foreach (var word2 in result)
                            {
                                if (word2 != word)
                                {
                                    for (int j = 0; j < vec.Count; j++)
                                    {
                                        if (word2 == vec[j].Word)
                                        {


                                            //int[] id1 = { 44, 26, 92, 30, 71, 38 };
                                            //int[] id2 = { 39, 59, 83, 47, 26, 4, 30 };

                                            IEnumerable<int> both = vec[j].VacancyID.Intersect(vec[i].VacancyID);
                                            pairs.Add(new string[] { word, word2 }, (int[])both);
                                            
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //var kek = new TechDictionary(1, word, 1, true);
                    //string[] tempSearchWords = searchingWords;
                    /*
                    do
                    {
                        for (int i = 0; i < vec.Count; i++)
                        {
                            if (vec[i].Word == word)
                            {

                            }
                        }
                    } while (tempSearchWords.Length > 0);
                    */
                    //var pek = vec.IndexOf(kek);
                    //MessageBox.Show(pek.ToString());
                }
            }
            //else return;
            pair = pairs;
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
                        SortedList<string, int[]> result = new();
                        result.Add($"{pair.Keys[0]} {pair.Keys[i]}", (int[])both);
                        for (int j = 1; j < pair.Count; j++)
                        {
                            if (j != i)
                            {
                                result.Add(pair.Keys[j], pair.Values[j]);
                            }

                        }
                        RecursiveShot(result);
                    }                    
                }
            }            
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
