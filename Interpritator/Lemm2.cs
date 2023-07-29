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
using WpfApp1Tech.Parser;

namespace WpfApp1Tech.Interpritator
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
        {
            foreach (string wayToParsFolder in waysToParsFolder)
            {
                Lemmization(wayToParsFolder);
            }
        }
        public static void Lemmization(string wayToParsFolder)
        {
            Stop = false;
            string shortParsDate = wayToParsFolder[^10..];

            Settings settings = new();
            TechDictionary test = new(new int[] { 1 }, "софтстэк", 1, false);
            List<TechDictionary>? vec = new(); //обьявление словаря
            vec.Add(test);

            JsonSerializer serializer = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            try // попытка присоединиться к существующему словарю
            {
                string haveAdict = Path.Combine(wayToParsFolder, $"TechDictionary {shortParsDate}.json");
                if (File.Exists(haveAdict))
                {
                    MessageBox.Show("Словарь доступен");
                }
                using var sr = new StreamReader(haveAdict);//чтение потока из указанного файла
                using var jr = new JsonTextReader(sr);// валидауция например
                vec = serializer.Deserialize<List<TechDictionary>>(jr);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Словарь не найден");
            }
            //string path = Settings.ssDef.ParsFolder;
            var morph = new MorphAnalyzer(withLemmatization: true);
            //int waysToPars = 1;
            int numOfVacancy = 1;
            int comOfVacancy = 1;


            //далее проходимся по всем найденым парсингом компаниям
            string[] AllCompanyOfVacacy = Directory.GetDirectories(wayToParsFolder);
            int directoryCount = AllCompanyOfVacacy.Length;

            foreach (var CompanyOfVacancy in AllCompanyOfVacacy)
            {//в каждой конкретной компании

                string[] files = Directory.GetFiles(CompanyOfVacancy, "*.json");
                foreach (string file in files)//находим все вакансии от этой компании
                {
                    
                    serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    try
                    {
                        using var sr = new StreamReader(file);//чтение потока из указанного файла
                        using var jr = new JsonTextReader(sr);// валидауция например
                        var vacancydata = serializer.Deserialize<VacancyData>(jr);
                        string fileText = vacancydata.TextOfVacancy;

                        //out int id текущей вакансии для последующей навигации
                        int currentIdOfVacancy = vacancydata.VacancyId;
                        var taskWindow = new Lemm2Wind(); //далее сбор таски

                        //dic выводится в richTextBox на таске для отображения текущего собранного словаря 
                        string dic = "";

                        //массив-индикатор слов-технологий для индикации их в тексте 
                        int[] dicWordInFileTextIndicator = new int[fileText.Length];


                        for (int i = 0; i < vec.Count; i++)
                        {
                            var needWord = vec[i];
                            if (needWord.IsTech)
                            {


                                int index = fileText.IndexOf(needWord.Word);
                                if (index != -1)
                                {
                                    //на этом этапе сразу прибавляем найденное знакомое слово-технологию 
                                    vec[i].UsingTimes += 1;
                                    //далее добавление ID текущей вакансии в массив айди всех найденых с этим словом вакансий
                                    int counter = 0;
                                    int[] newVacancyID = new int[vec[i].VacancyID.Length + 1];
                                    foreach (var id in vec[i].VacancyID)
                                    {
                                        newVacancyID[counter++] = id;
                                    }
                                    newVacancyID[^1] = currentIdOfVacancy;
                                    vec[i].VacancyID = newVacancyID;
                                    
                                    do // Поиск всех повторений слова-технологии в тексте для подсветки
                                    {
                                        dicWordInFileTextIndicator[index] = index + needWord.Word.Length;// Диапозон нахождения слова в тексте
                                        index = fileText.IndexOf(needWord.Word, dicWordInFileTextIndicator[index]);

                                    } while (index != -1);
                                }
                                dic = string.Concat(needWord.Word.ToUpper(), "-", " [", needWord.UsingTimes, "]", "\n***\r", dic);
                            }
                        }
                        //выводим готовый dic
                        taskWindow.DictionaryBox.Document = new FlowDocument(new Paragraph(new Run(dic)));

                        var VacancyFlowDoc = new FlowDocument();
                        var VacancyParagraph = new Paragraph();

                        int pointer = 0;
                        for (int wr = 0; wr < fileText.Length; wr++)
                        {

                            if (dicWordInFileTextIndicator[wr] != 0)
                            {//собираем обычный текст до слова-технологии
                                var vacancyRun = new Run(fileText.Substring(pointer, wr - pointer));
                                var techBold = new Bold(new Run(fileText[wr..dicWordInFileTextIndicator[wr]]));
                                techBold.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFEEF5FD");
                                //слово-технологию раскрашиваем цветным фоном and Bold to it.

                                VacancyParagraph.Inlines.Add(vacancyRun);
                                VacancyParagraph.Inlines.Add(techBold);
                                // дополняем flowDocument для richTextBox и перетаскиваем pointer начала поиска технологий
                                // для следующей иттерации цикла.
                                pointer = dicWordInFileTextIndicator[wr];
                                wr = pointer;
                            }
                        }
                        //если слов-технологий в текущем тексте нет, выводим его в box полностью без изменений
                        if (pointer == 0) VacancyParagraph.Inlines.Add(new Run(fileText));
                        else VacancyParagraph.Inlines.Add(new Run(fileText.Substring(pointer, fileText.Length - pointer)));
                        //если слова-технологии были (pointer не нулевой) - остаток текста после последнего нахождения,
                        //также выводим без изменений

                        VacancyFlowDoc.Blocks.Add(VacancyParagraph);
                        taskWindow.VacancyRichTextBox.Document = VacancyFlowDoc;

                        taskWindow.ProgressOf.Text = $"{shortParsDate},{numOfVacancy} of {AllCompanyOfVacacy.Length}";
                        taskWindow.VacancyProgress.Value = 100 * (100 * numOfVacancy / AllCompanyOfVacacy.Length);// (100 * waysToPars / waysToParsFolder.Length);
                        //рассчитываем примерный прогресс сбора словаря

                        taskWindow.ShowDialog();//отображается собранная таска с данными из файла и словаря

                        //НЕ ГОТОВО!!!!!!!!!!!!!!!! 
                        //ТРЕБУЕТСЯ БУЛЬ НА КАЖДОЙ ИТЕРАЦИИ КАЖДОГО ФОРЫЧА ИНАЧЕ КАСКАДНЫЙ БРИК
                        if (Stop)
                        {
                            string saveStopDicName = Path.Combine(wayToParsFolder, $"TechDict {shortParsDate}.{numOfVacancy}.json");

                            serializer = new JsonSerializer();
                            serializer.Formatting = Formatting.Indented;
                            serializer.NullValueHandling = NullValueHandling.Ignore;
                            using (var sw = new StreamWriter(saveStopDicName))
                            using (var jw = new JsonTextWriter(sw))
                            {

                                serializer.Serialize(jw, vec);

                            }
                            break;
                        }



                        //далее нахождение выделенных пользователем слов-технологий из текста в словаре
                        var UseShortPathDate = new UseWordPerDate(shortParsDate, 1);//???
                        string[] NewTech = taskWindow.NewTech.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < NewTech.Length; i++)
                        {
                            NewTech[i] = NewTech[i].Trim();
                            //проходим по словарю и находим технологии
                            for (int j = 0; j < vec.Count; j++)
                                if (vec[j].IsTech)
                                {
                                    //если отмеченое слово уже есть, +1 использование.
                                    if (NewTech[i].ToLower() == vec[j].Word.ToLower())
                                    {
                                        NewTech[i] = "";//удаление из массива новонайденных
                                        vec[j].UsingTimes += 1;
                                        //далее добавление ID текущей вакансии
                                        //в массив айди всех найденых с этим словом вакансий
                                        /*int[] newVacancyID = new int[vec[i].VacancyID.Length + 1];
                                        for (int id = 0; id < vec[i].VacancyID.Length; id++)
                                        {
                                            newVacancyID[id] = vec[i].VacancyID[j];
                                        }
                                        newVacancyID[newVacancyID.Length - 1] = currentIdOfVacancy;
                                        vec[i].VacancyID = newVacancyID;
                                        */
                                        //далее добавление ID текущей вакансии в массив айди всех найденых с этим словом вакансий
                                        int counter = 0;
                                        int[] newVacancyID = new int[vec[i].VacancyID.Length + 1];
                                        foreach (var id in vec[i].VacancyID)
                                        {
                                            newVacancyID[counter++] = id;
                                        }
                                        newVacancyID[^1] = currentIdOfVacancy;
                                        vec[i].VacancyID = newVacancyID;

                                    }
                                }
                            //если слова нет в словаре добавляем
                            if (NewTech[i] != "")


                                vec.Add(new(new int[] { currentIdOfVacancy }, NewTech[i], 1, true));
                        }

                        //блок записи всех слов НЕ технологий и подсчет
                        string textWithoultTech = fileText.ToLower();
                        for (int i = 0; i < vec.Count; i++)
                        {
                            var dicWord = vec[i];
                            if (dicWord.IsTech)
                            {
                                //если слово технология - удаляем, они уже подсчитаны в текущем тексте
                                textWithoultTech = textWithoultTech.Replace($"{dicWord.Word.ToLower()}", "");
                            }
                        }

                        //оставшиеся слова приводим к именительному падежу и далее сохраняем или прибавляем
                        //string[] allWords = fileText.Split(new char[] { ' ', ',', '.', ':', /*'-'*/'–', '—', '\r', '\n', '•', ';', '_', '?', '!', '"', '(', ')', '@', '%', '*', '`', '<', '|', '/', '>', '~', '+', '=' }, StringSplitOptions.RemoveEmptyEntries);

                        string[] lowerWords = textWithoultTech.Split(new char[] { ' ', ',', '.', ':', '-', '–', '—', '\r', '\n', '•', ';', '_', '?', '!', '"', '(', ')', '@', '%', '*', '`', '<', '|', '/', '>', '~', '+', '=' }, StringSplitOptions.RemoveEmptyEntries);

                        var results = morph.Parse(lowerWords).ToArray();


                        //прогон слов текста на наличие в словаре 
                        for (int w = 0; w < results.Length; w++)
                        {
                            bool checkItsNew = true;// есть ли слово в словаре
                            string lem = results[w].BestTag.Lemma;

                            if (lem != null)
                            {
                                for (int i = 0; i < vec.Count; i++)
                                {
                                    var dicWord = vec[i];

                                    if (lem.ToLower() == dicWord.Word.ToLower())
                                    {
                                        vec[i].UsingTimes += 1;
                                        //далее добавление ID текущей вакансии
                                        //в массив айди всех найденых с этим словом вакансий
                                        /*int[] newVacancyID = new int[vec[i].VacancyID.Length];//было +1
                                        for (int j = 0; j < vec[i].VacancyID.Length; j++)
                                        {
                                            newVacancyID[j] = vec[i].VacancyID[j];
                                        }
                                        newVacancyID[newVacancyID.Length - 1] = vacancydata.VacancyId;
                                        vec[i].VacancyID = newVacancyID;
                                        checkItsNew = false;
                                        */
                                        //далее добавление ID текущей вакансии в массив айди всех найденых с этим словом вакансий
                                        int counter = 0;
                                        int[] newVacancyID = new int[vec[i].VacancyID.Length + 1];
                                        foreach (var id in vec[i].VacancyID)
                                        {
                                            newVacancyID[counter++] = id;
                                        }
                                        newVacancyID[^1] = currentIdOfVacancy;
                                        vec[i].VacancyID = newVacancyID;
                                        checkItsNew = false;

                                    }

                                }
                                /////////////////////////////////////////////////////////...////////
                                if (checkItsNew)//слова в словаре не найдено
                                {
                                    //поднимаем первую букву,остальные опускаем и записываем в словарь.
                                    string toUpperfChar = $"{char.ToUpper(lem[0])}{lem[1..].ToLower()}";
                                    TechDictionary word = new(new int[] { vacancydata.VacancyId }, toUpperfChar, 1, false);
                                    vec.Add(word);
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "текстовый файл не был обработан");
                    }
                    numOfVacancy += 1;//счетчик подсчета уникальных вакансий
                }
                comOfVacancy += 1;// счетчик подсчета уникальных компаний
            }

            if (directoryCount > 0)
            {
                string saveName = Path.Combine(wayToParsFolder, $"TechDictionary {shortParsDate}.json");

                serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.NullValueHandling = NullValueHandling.Ignore;
                using (var sw = new StreamWriter(saveName))
                using (var jw = new JsonTextWriter(sw))
                {

                    serializer.Serialize(jw, vec);

                }
                MessageBox.Show($"Словарь успешно создан в директории:/n/r{wayToParsFolder}");
            }
            else MessageBox.Show("количество доступных для обработки компаний равно нулю, или другая проблема директорий");

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