using DeepMorphy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;

namespace WpfApp1Tech
{
    internal class Lemm2
    {
        public string Description { get; internal set; }
        public static bool Tech { get; internal set; }
        public static bool Stop { get; internal set; }
        public static bool Pair { get; internal set; }
        public static bool Triple { get; internal set; }

        public Lemm2(string description)
        {

            Description = description;
            //Lemmization();
        }
        public static void Lemmization(string[] waysToParsFolder)
        { foreach(string wayTPF in waysToParsFolder)
            {
                Lemmization(wayTPF);
            }
        }
        public static void Lemmization(string wayToParsFolder)
        {
            Stop = false;
            string shortParsDate = wayToParsFolder[^10..];

            Settings settings = new();
            TechDictionary test = new( 1, "софтстэк", 1, false);
            Vector<TechDictionary>? vec = new(); //обьявление словаря
            vec.PushBack(test);

            JsonSerializer serializer = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            try // попытка присоединиться к существующему словарю
            {
                if(File.Exists(System.IO.Path.Combine(wayToParsFolder, $"TechDictionary{shortParsDate}")))
                {
                    MessageBox.Show("ppp");
                }
                using var sr = new StreamReader(wayToParsFolder);//чтение потока из указанного файла
                using var jr = new JsonTextReader(sr);// валидауция например
                vec = serializer.Deserialize<Vector<TechDictionary>>(jr);
                
               // Settings va = new();
               // va.StartSet(va.ss);
               // va.ss.ParsFolder = "";
               // va.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cущ Слов");
            }
            //string path = Settings.ssDef.ParsFolder;
            var morph = new MorphAnalyzer(withLemmatization: true);
            //int waysToPars = 1;
            int numOfVacancy = 1;
            int comOfVacancy = 1;

            //foreach (var PathDate in waysToParsFolder)
            //{
                string[] AllCompanyOfVacacy = Directory.GetDirectories(wayToParsFolder);
                int directoryCount = AllCompanyOfVacacy.Length;

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
                        string fileText = serializer.Deserialize<Lemm2>(jr).Description;

                        var taskWindow = new Lemm2Wind(); //далее сбор таски

                        string dic = "";
                        int[] dicWordInFileTextIndicator = new int[fileText.Length];

                        for (int i = 0; i < vec.Count; i++)
                        {
                            var needWord = vec.At(i);
                            if (needWord.IsTech)
                            {

                                //for (int t = 0; t < needWord.VectorPerDate.Count; t++)
                                //{
                                //    if (shortParsDate == needWord.VectorPerDate.At(t).Date)
                                //    {
                                dic = string.Concat(needWord.Word.ToUpper(), "-", " [", needWord.UsingTimes, "]", "\n***\r", dic);

                                //    }
                                //}                                    
                                int index = fileText.IndexOf(needWord.Word);
                                if (index != -1)
                                {
                                    do // Поиск всех повторений слова в тексте для подсветки
                                    {
                                        dicWordInFileTextIndicator[index] = index + needWord.Word.Length;// Диапозон нахождения слова в тексте
                                        index = fileText.IndexOf(fileText, dicWordInFileTextIndicator[index]);

                                    } while (index != -1);
                                }
                            }
                        }
                        taskWindow.DictionaryBox.Document = new FlowDocument(new Paragraph(new Run(dic)));

                        var VacancyFlowDoc = new FlowDocument();
                        var VacancyParagraph = new Paragraph();

                        int pointer = 0;
                        for (int wr = 0; wr < fileText.Length; wr++)
                        {

                            if (dicWordInFileTextIndicator[wr] != 0)
                            {
                                var vacancyRun = new Run(fileText.Substring(pointer, wr));
                                var techBold = new Bold(new Run(fileText[wr..dicWordInFileTextIndicator[wr]]));
                                techBold.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFEEF5FD");
                                // Create a paragraph and add the Run and Bold to it.

                                VacancyParagraph.Inlines.Add(vacancyRun);
                                VacancyParagraph.Inlines.Add(techBold);
                                // Add the paragraph to the FlowDocument.
                                pointer = dicWordInFileTextIndicator[wr];
                            }
                        }
                        if (pointer == 0) VacancyParagraph.Inlines.Add(new Run(fileText));

                        VacancyFlowDoc.Blocks.Add(VacancyParagraph);
                        taskWindow.VacancyRichTextBox.Document = VacancyFlowDoc;

                        taskWindow.ProgressOf.Text = $"{shortParsDate},{numOfVacancy} of {AllCompanyOfVacacy.Length}";
                        taskWindow.VacancyProgress.Value = 100 * (100 * numOfVacancy / AllCompanyOfVacacy.Length);// (100 * waysToPars / waysToParsFolder.Length);

                        taskWindow.ShowDialog();//таска с данными из файла и словаря
                                                // 
                        if (Stop)
                        {
                            string saveStopDicName = System.IO.Path.Combine(settings.ParsFolder, $"TechDict {shortParsDate}({numOfVacancy}VacCount)");

                            serializer = new JsonSerializer();
                            serializer.Formatting = Formatting.Indented;
                            serializer.NullValueHandling = NullValueHandling.Ignore;
                            using (var sw = new StreamWriter(saveStopDicName))
                            using (var jw = new JsonTextWriter(sw))
                            {

                                serializer.Serialize(jw, vec);

                            }
                        }




                        var UseShortPathDate = new UseWordPerDate(shortParsDate, 1);
                        string[] NewTech = taskWindow.NewTech.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < NewTech.Length; i++)
                        {

                            for (int j = 0; j < vec.Count; j++)
                                if (vec.At(j).IsTech)
                                {
                                    if (NewTech[i] == vec.At(j).Word)
                                    {
                                        NewTech[i] = "";
                                        //for(int k = 0; k < vec.At(j).VectorPerDate.Count; k++)
                                        //if (vec.At(j).VectorPerDate.At(k).Date == shortParsDate)
                                        {
                                            vec.At(j).UsingTimes += 1;
                                        }
                                    }
                                }
                            if (NewTech[i] != "")
                            {
                                //UseWordPerDate date = new(PathDate, 1);
                                //TechDictionary word = new(numOfVacancy, NewTech[i],  1, true);
                                //word.VectorPerDate.PushBack(UseShortPathDate);
                                vec.PushBack(new(numOfVacancy, NewTech[i], 1, true));
                            }

                        }

                        string textWithoultTech = fileText;
                        for (int i = 0; i < vec.Count; i++)
                        {
                            var dicWord = vec.At(i);
                            if (dicWord.IsTech)
                            {
                                textWithoultTech.Replace($"{dicWord.Word}", "");
                            }
                        }
                        string[] words = textWithoultTech.Split(new char[] { ' ', ',', '.', ':', /*'-'*/'–', '—', '\r', '\n', '•', ';', '_', '?', '!', '"', '(', ')', '@', '%', '*', '`', '<', '|', '/', '>', '~', '+', '=' }, StringSplitOptions.RemoveEmptyEntries);
                        var results = morph.Parse(words).ToArray();


                        //прогон слов текмта на наличие в словаре 
                        for (int w = 0; w < results.Length; w++)
                        {
                            //bool tech = false;
                            //bool pair = false;
                            //bool triple = false;
                            //string[] l;
                            //if (w == results.Length - 2) { l = new[] { $"{results[w].BestTag.Lemma}", $"{results[w + 1].BestTag.Lemma}", "" }; }
                            //else if (w == results.Length - 1) { l = new[] { $"{results[w].BestTag.Lemma}", "", "" }; }
                            //else { l = new[] { $"{results[w].BestTag.Lemma}", $"{results[w + 1].BestTag.Lemma}", $"{results[w + 2].BestTag.Lemma}" }; }

                            int check = 0;
                            string lem = results[w].BestTag.Lemma;

                            for (int i = 0; i < vec.Count; i++)
                            {
                                var dicWord = vec.At(i);

                                //if (l[0] == dicWord.Word || $"{l[0]} {l[1]}" == dicWord.Word || $"{l[0]} {l[1]} {l[2]}" == dicWord.Word)
                                if (lem == dicWord.Word)
                                {
                                    //for (int t = 0; t < dicWord.VectorPerDate.Count; t++)
                                    //{                                            
                                    //if (shortParsDate == dicWord.VectorPerDate.At(t).Date)
                                    //{
                                    vec.At(i).UsingTimes += 1;
                                    check++;
                                    //   if ($"{l[0]} {l[1]}" == dicWord.Word) w++;
                                    //   if ($"{l[0]} {l[1]} {l[2]}" == dicWord.Word) { w++; w++; }
                                    //}

                                    //}

                                }

                            }
                            /////////////////////////////////////////////////////////...////////
                            if (check == 0)
                            {
                                TechDictionary word = new(numOfVacancy, "", new(), Tech);
                                vec.PushBack(word);
                            }

                            //word.VectorPerDate.PushBack(UseShortPathDate);
                            //if (Pair == true)
                            //{ word.Word = $"{l[0]} {l[1]}"; w++; }
                            //else if (Triple == true)
                            //{ word.Word = $"{l[0]} {l[1]} {l[2]}"; w++; w++; }
                            //else
                            //{ word.Word = l[0]; }



                            //}

                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "qeqe");
                        //     ss = ssDef;
                    }
                    numOfVacancy += 1;
                }
                comOfVacancy += 1;
            }
            //waysToPars += 1;

            if (directoryCount > 0)
            {
                string saveName = System.IO.Path.Combine(settings.ParsFolder, $"TechDictionary {shortParsDate}");

                serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.NullValueHandling = NullValueHandling.Ignore;
                using (var sw = new StreamWriter(saveName))
                using (var jw = new JsonTextWriter(sw))
                {

                    serializer.Serialize(jw, vec);

                }
            }
                
            //}
            
        }
        void SaveDicVecToJSONFile(dynamic dictionary)
        {

        }
    }
    
}
//if (check == 0)
                                //{

//taskWindow.Show();

//taskWindow.FirstWordBlock.Text = l[0];
//taskWindow.SecondWordBlock.Text = $"{l[0]} {l[1]}";
//taskWindow.ThirdWordBlock.Text = $"{l[0]} {l[1]} {l[2]}";
//taskWindow.PointInText.Text = fileText[fileText.IndexOf(words[w])..];



//TextRange doc = new TextRange(taskWindow.DictionaryBox.Document.ContentStart, taskWindow.DictionaryBox.Document.ContentEnd);
//FlowDocument fd = 
//taskWindow.RichPointInText.SelectionTextBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFE4FCDF"); ;

/*

//TextRange doc = new TextRange(taskWindow.DictionaryBox.Document.ContentStart, taskWindow.DictionaryBox.Document.ContentEnd);

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
taskWindow.ShowDialog();*/

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