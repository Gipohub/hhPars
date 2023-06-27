using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static WpfApp1Tech.LematizatorQuest;
using DeepMorphy;
using DeepMorphy.Model;
using System.Windows.Media;
using System.Linq.Expressions;
using System.Windows.Documents;

namespace WpfApp1Tech
{
    internal class Lemmizator1
    {
        
        public string Description { get; set; }
        public static bool Tech { get; internal set; }
        public static bool Pair { get; internal set; }
        public static bool Triple { get; internal set; }

        public Lemmizator1(string description)
        {

            Description = description;
            //Lemmization();
        }
        /*public class ComboWord
        {

            public bool Tech { get; set; }
            public bool Pair { get; set; }
            public bool Triple { get; set; }
            public ComboWord(bool tech, bool pair, bool triple)
            {
                Tech = tech;
                Pair = pair;
                Triple = triple;
            }
        }*/
        public static void Lemmization(string[] waysToParsFolder)
        { }
        public static void Lemmization(string wayToParsFolder)
        {

        Settings settings = new Settings();
            Vector<TechDictionary>? vec = new(); //обьявление словаря
            JsonSerializer serializer = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
             
            try // попытка присоединиться к существующему словарю
            {
                using var sr = new StreamReader(settings.ParsFolder);//чтение потока из указанного файла
                using var jr = new JsonTextReader(sr);// валидауция например
                vec = serializer.Deserialize<Vector<TechDictionary>>(jr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
            }
            //string path = Settings.ssDef.ParsFolder;
            var morph = new MorphAnalyzer(withLemmatization: true);
            int waysToPars = 1;
            int numOfVacancy = 1;
            int comOfVacancy = 1;

            //foreach (var PathDate in waysToParsFolder)
            //{
                string[] AllCompanyOfVacacy = Directory.GetDirectories(wayToParsFolder);
                foreach (var CompanyOfVacancy in AllCompanyOfVacacy)
                {

                    string[] files = Directory.GetFiles(CompanyOfVacancy, "linkText*.json");
                    foreach (string file in files)
                    {
                       
                        serializer = new JsonSerializer();
                        serializer.NullValueHandling = NullValueHandling.Ignore;
                        try
                        {


                            using var sr = new StreamReader(file);//чтение потока из указанного файла
                            using var jr = new JsonTextReader(sr);// валидауция например
                            string fileText = serializer.Deserialize<Lemmizator1>(jr).Description;
                            string[] words = fileText.Split(new char[] { ' ', ',', '.', ':', /*'-'*/'–','—', '\r', '\n', '•', ';', '_', '?', '!', '"', '(', ')', '@', '%', '*', '`', '<', '|', '>', '~', '+', '=' }, StringSplitOptions.RemoveEmptyEntries); ;
                            var results = morph.Parse(words).ToArray();
                            ////var usingTimes = new UseWordPerDate(PathDate, 1);
                      
                            for (int w = 0; w < results.Length; w++)
                            {
                                //bool tech = false;
                                //bool pair = false;
                                //bool triple = false;
                                string[] l;
                                if (w == results.Length - 2) { l = new[] { $"{results[w].BestTag.Lemma}", $"{results[w + 1].BestTag.Lemma}", "" }; }
                                else if (w == results.Length - 1) { l = new[] { $"{results[w].BestTag.Lemma}", "", "" }; }
                                else { l = new[] { $"{results[w].BestTag.Lemma}", $"{results[w + 1].BestTag.Lemma}", $"{results[w + 2].BestTag.Lemma}" }; }

                                int check = 0;

                                for (int i = 0; i < vec.Count; i++)
                                {
                                    if (l[0] == vec.At(i).Word || $"{l[0]} {l[1]}" == vec.At(i).Word || $"{l[0]} {l[1]} {l[2]}" == vec.At(i).Word)
                                    {
                                        ////for (int t = 0; t < vec.At(i).VectorPerDate.Count; t++)
                                        {
                                        ////    if (PathDate == vec.At(i).VectorPerDate.At(t).Date)
                                            {
                                        ////        vec.At(i).VectorPerDate.At(t).UsingTimes += 1;
                                                check++;
                                                if ($"{l[0]} {l[1]}" == vec.At(i).Word) w++;
                                                if ($"{l[0]} {l[1]} {l[2]}" == vec.At(i).Word) { w++; w++; }
                                            }

                                        }

                                    }

                                }
                               
                                if (check == 0)
                                {

                                    //taskWindow.Show();
                                    LematizatorQuest taskWindow = new();
                                    taskWindow.FirstWordBlock.Text = l[0];
                                    taskWindow.SecondWordBlock.Text = $"{l[0]} {l[1]}";
                                    taskWindow.ThirdWordBlock.Text = $"{l[0]} {l[1]} {l[2]}";
                                    taskWindow.PointInText.Text = fileText[fileText.IndexOf(words[w])..];
                                  ////  taskWindow.ProgressOf.Text = $"{waysToPars}/{waysToParsFolder.Length},{numOfVacancy} of {AllCompanyOfVacacy.Length}";
                                  ////  taskWindow.VacancyProgress.Value = 100 *(100 * numOfVacancy / AllCompanyOfVacacy.Length)/(100* waysToPars / waysToParsFolder.Length);

                                    string dic = "";
                                    for (int i = 0; i < vec.Count; i++)
                                    {
                                        if (vec.At(i).IsTech)
                                        {
                                        ////    for (int t = 0; t < vec.At(i).VectorPerDate.Count; t++)
                                            {
                                         ////       if (PathDate == vec.At(i).VectorPerDate.At(t).Date)
                                                {
                                                ////    dic = string.Concat(vec.At(i).Word.ToUpper(),"-"," [",vec.At(i).VectorPerDate.At(t).UsingTimes,"]", "\n***\r",dic);
                                                    
                                                }
                                            }
                                        }
                                    }

                                    //taskWindow.DictionaryBox.Text = dic;
                                    //taskWindow.RichPointInText.SelectionTextBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFE4FCDF"); ;
                                    
                                    TextRange doc = new TextRange(taskWindow.RichPointInText.Document.ContentStart, taskWindow.RichPointInText.Document.ContentEnd);

                                    UTF8Encoding uniEncoding = new UTF8Encoding();
                                    int count = 0;
                                    byte[] byteArray;
                                    char[] charArray;
                                    byte[] b = uniEncoding.GetBytes(dic);
                                    using (MemoryStream memStream = new MemoryStream())
                                    byteArray = new byte[memStream.Length];
                                    charArray = new char[uniEncoding.GetCharCount(byteArray, 0, count)];
                                    uniEncoding.GetDecoder().GetChars(
                                        byteArray, 0, count, charArray, 0);
                                    //Console.WriteLine(charArray);
                                    taskWindow.DictionaryBox.Text = charArray.ToString();


                                    using (MemoryStream memStream = new MemoryStream(b))
                                        doc.Load(memStream, DataFormats.Text);
                                    //taskWindow.RichPointInText.Paste(doc);

                                    Tech = false;
                                    Pair = false;
                                    Triple = false;
                                    taskWindow.ShowDialog();

                                    /*MessageBoxResult result = MessageBox.Show($"{l} - Is Technology?\r(\"Cancel\", if {j} is a Technology", "TechDictionary", MessageBoxButton.YesNoCancel);
                                    switch (result)
                                    {
                                        case MessageBoxResult.Yes:                                            
                                            tech = true;
                                            break;
                                        case MessageBoxResult.No:
                                            break;
                                        case MessageBoxResult.Cancel:
                                            pair = true;
                                            break;
                                    }
                                    switch (pair)
                                    {
                                        case true:
                                            TechDictionary word = new(j, , tech);
                                            vec.PushBack(word);
                                            break;
                                        case false:
                                            word = new(l, 1, tech);
                                            vec.PushBack(word);
                                            break;
                                    } */
                                   //// UseWordPerDate date = new(PathDate, 1);
                                  ////  TechDictionary word = new("",numOfVacancy, new(), Tech);
                                  ////  word.VectorPerDate.PushBack(date);
                                  ////  if (Pair == true)
                                 ////   { word.Word = $"{l[0]} {l[1]}"; w++; }
                                 ////   else if (Triple == true)
                                ////    { word.Word = $"{l[0]} {l[1]} {l[2]}"; w++;w++; }
                                 ////   else 
                                ////    { word.Word = l[0]; }
                                    
                                ////    vec.PushBack(word);
                                    
                                }
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                       //     ss = ssDef;
                        }
                        numOfVacancy += 1;
                    }
                    comOfVacancy += 1; 
                }
                //waysToPars += 1;
            //}
            string saveName = Path.Combine(settings.ParsFolder, "TechDictionary");

            serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.NullValueHandling = NullValueHandling.Ignore;
            using (var sw = new StreamWriter(saveName))
            using (var jw = new JsonTextWriter(sw))
            {

                serializer.Serialize(jw, vec);
            }

        }

    }
    
}
