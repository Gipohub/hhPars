using DeepMorphy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1Tech
{
    /// <summary>
    /// Логика взаимодействия для Lemmizator.xaml
    /// </summary>
    public partial class Lemmizator : System.Windows.Window
    {
        public string Description { get; set; }
        public static bool Tech { get; internal set; }
        public static bool Pair { get; internal set; }
        public static bool Triple { get; internal set; }
        

        public Lemmizator()
        {
           // Lemmization(settings.ParserSettings.ParsFolder);
            InitializeComponent();
            
        }

        public static void Lemmization(string waysToParsFolder)
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
                string[] AllCompanyOfVacacy = Directory.GetDirectories(waysToParsFolder);
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
                            string[] words = fileText.Split(new char[] { ' ', ',', '.', ':', /*'-'*/'–', '—', '\r', '\n', '•', ';', '_', '?', '!', '"', '(', ')', '@', '%', '*', '`', '<', '|','/', '>', '~', '+', '=' }, StringSplitOptions.RemoveEmptyEntries); ;
                            var results = morph.Parse(words).ToArray();
                            var usingTimes = new UseWordPerDate(waysToParsFolder, 1);

                            Lemmizator taskWindow = new();

                            for (int w = 0; w < words.Length; w++)
                            {
                                //bool tech = false;
                                //bool pair = false;
                                //bool triple = false;
                                string[] l;
                                //if (w == results.Length - 2) { l = new[] { $"{results[w].BestTag.Lemma}", $"{results[w + 1].BestTag.Lemma}", "" }; }
                                //else if (w == results.Length - 1) { l = new[] { $"{results[w].BestTag.Lemma}", "", "" }; }
                                /*else { */l = new[] { $"{results[w].BestTag.Lemma}", $"{results[w + 1].BestTag.Lemma}", $"{results[w + 2].BestTag.Lemma}" }; 

                                int check = 0;

                                for (int i = 0; i < vec.Count; i++)
                                {
                                    if (l[0] == vec.At(i).Word || $"{l[0]} {l[1]}" == vec.At(i).Word || $"{l[0]} {l[1]} {l[2]}" == vec.At(i).Word)
                                    {
                                   //     for (int t = 0; t < vec.At(i).VectorPerDate.Count; t++)
                                        {
                                  //          if (waysToParsFolder == vec.At(i).VectorPerDate.At(t).Date)
                                            {
                                  //              vec.At(i).VectorPerDate.At(t).UsingTimes += 1;
                                                check++;
                                                if ($"{l[0]} {l[1]}" == vec.At(i).Word) w++;
                                                if ($"{l[0]} {l[1]} {l[2]}" == vec.At(i).Word) { w++; w++; }
                                            }

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
                                    taskWindow.ProgressOf.Text = $"{waysToPars}/{waysToParsFolder.Length},{numOfVacancy} of {AllCompanyOfVacacy.Length}";
                                    taskWindow.VacancyProgress.Value = 100 * (100 * numOfVacancy / AllCompanyOfVacacy.Length) / (100 * waysToPars / waysToParsFolder.Length);

                                    string dic = "";
                                    for (int i = 0; i < vec.Count; i++)
                                    {
                                        if (vec.At(i).IsTech)
                                        {
                                       //     for (int t = 0; t < vec.At(i).VectorPerDate.Count; t++)
                                            {
                                      //          if (waysToParsFolder == vec.At(i).VectorPerDate.At(t).Date)
                                                {
                                       //             dic = string.Concat(vec.At(i).Word.ToUpper(), "-", " [", vec.At(i).VectorPerDate.At(t).UsingTimes, "]", "\n***\r", dic);

                                                }
                                            }
                                        }
                                    }
                                    
                                    //taskWindow.DictionaryBox.Text = dic;
                                    //taskWindow.RichPointInText.SelectionTextBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFE4FCDF"); ;

                                    /*
                                    TextRange doc = new TextRange(taskWindow.VacancyRichTextBox.Document.ContentStart, taskWindow.VacancyRichTextBox.Document.ContentEnd);

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
                                    UseWordPerDate date = new(waysToParsFolder, 1);
                                  //  TechDictionary word = new("", numOfVacancy, new(), Tech);
                                  //  word.VectorPerDate.PushBack(date);
                                 //   if (Pair == true)
                                 //   { word.Word = $"{l[0]} {l[1]}"; w++; }
                                 //   else if (Triple == true)
                                 //   { word.Word = $"{l[0]} {l[1]} {l[2]}"; w++; w++; }
                                 //   else
                                //    { word.Word = l[0]; }

                                //    vec.PushBack(word);

                                //}

                            }

                            //StackPanel myStackPanel = new StackPanel();

                            // Create a FlowDocument to contain content for the RichTextBox.
                            FlowDocument myFlowDoc = new FlowDocument();
                            Paragraph myParagraph = new Paragraph();

                            int[] dicWordInFileTextIndicator = new int[fileText.Length];
                           
                            for (int wr = 0; wr < vec.Count; wr++)
                            {
                                int indexes = fileText.IndexOf(vec.At(wr).Word);
                                while (indexes > -1)
                                {
                                    dicWordInFileTextIndicator[fileText.IndexOf(vec.At(wr).Word, indexes)] = fileText.LastIndexOf(vec.At(wr).Word, indexes);
                                    indexes = fileText.LastIndexOf(vec.At(wr).Word, indexes);                                    
                                }
                            }

                            int pointer = 0;
                            for (int wr = 0; wr < fileText.Length; wr++)
                            {
                                if (dicWordInFileTextIndicator[wr] != 0)
                                {
                                    Run myRun = new Run(fileText.Substring(pointer, wr));
                                    Bold myBold = new Bold(new Run(fileText.Substring(wr, dicWordInFileTextIndicator[wr] - wr)));

                                    // Create a paragraph and add the Run and Bold to it.
                                    
                                    myParagraph.Inlines.Add(myRun);
                                    myParagraph.Inlines.Add(myBold);
                                    // Add the paragraph to the FlowDocument.
                                    pointer = wr + (dicWordInFileTextIndicator[wr] - wr);
                                }
                            }
                            myFlowDoc.Blocks.Add(myParagraph);
                            //TextRange tr = new TextRange(
                    //taskWindow.VacancyRichTextBox.Document.ContentStart, taskWindow.VacancyRichTextBox.Document.ContentEnd);
                            taskWindow.VacancyRichTextBox.Document = myFlowDoc;
                            taskWindow.ShowDialog();
                            // 
                            //string[] stringDic = Array.Empty<string>();
                            //for (int w = 0; w < vec.Count; w++)
                            //{
                            //    stringDic[w] = vec.At(w).Word;
                            //}
                            // Add paragraphs to the FlowDocument.


                            //fileText.IndexOf(r);
                            //    myFlowDoc.Blocks.Add(new Paragraph(new Run("Paragraph 2")));
                            //myFlowDoc.Blocks.Add(new Paragraph(new Run("Paragraph 3")));

                            //RichTextBox VacancyRichTextBox = new RichTextBox();

                            // Add initial content to the RichTextBox.
                            

                            //myStackPanel.Children.Add(myRichTextBox);
                            //this.Content = myStackPanel;

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
                waysToPars += 1;
            //}
            string saveName = System.IO.Path.Combine(settings.ParsFolder, "TechDictionary");

            serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.NullValueHandling = NullValueHandling.Ignore;
            using (var sw = new StreamWriter(saveName))
            using (var jw = new JsonTextWriter(sw))
            {

                serializer.Serialize(jw, vec);

            }
        }
        
        private void save_ButonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog save = new Microsoft.Win32.SaveFileDialog();
            save.Filter =
                "Файл XAML (*.xaml)|*.xaml|RTF-файл (*.rtf)|*.rtf";

            if (save.ShowDialog() == true)
            {
                // Создание контейнера TextRange для всего документа
                TextRange documentTextRange = new TextRange(
                    VacancyRichTextBox.Document.ContentStart, VacancyRichTextBox.Document.ContentEnd);

                // Если такой файл существует, он перезаписывается, 
                using (FileStream fs = File.Create(save.FileName))
                {
                    if (System.IO.Path.GetExtension(save.FileName).ToLower() == ".rtf")
                    {
                        documentTextRange.Save(fs, DataFormats.Rtf);
                    }
                    else if (System.IO.Path.GetExtension(save.FileName).ToLower() == ".txt")
                        documentTextRange.Save(fs, DataFormats.Text);
                    else
                    {
                        documentTextRange.Save(fs, DataFormats.Xaml);
                    }
                }
            }
        }

        private void open_ButonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFile =
                new Microsoft.Win32.OpenFileDialog();

            openFile.Filter = "RichText files (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (openFile.ShowDialog() == true)
            {
                TextRange tr = new TextRange(
                    VacancyRichTextBox.Document.ContentStart, VacancyRichTextBox.Document.ContentEnd);

                using (FileStream fs = File.Open(openFile.FileName, FileMode.Open))

                // using var fs = new StreamReader(openFile);//чтение потока из указанного файла
                // using var jr = new JsonTextReader(sr);// валидауция например
                // string fileText = serializer.Deserialize<Lemmizator1>(jr).Description;
                {
                    if (System.IO.Path.GetExtension(openFile.FileName).ToLower() == ".rtf")
                        tr.Load(fs, DataFormats.Rtf);
                    else if (System.IO.Path.GetExtension(openFile.FileName).ToLower() == ".txt")
                        tr.Load(fs, DataFormats.Text);
                    else
                        tr.Load(fs, DataFormats.Text);
                }

            }

            // Копирование содержимого документа в MemoryStream. 
            using (MemoryStream stream = new MemoryStream())
            {
                TextRange range = new TextRange(VacancyRichTextBox.Document.ContentStart,
                    VacancyRichTextBox.Document.ContentEnd);
                range.Save(stream, DataFormats.Text);
                stream.Position = 0;

                // Чтение содержимого из потока и вывод его в текстовом поле. 
                using (StreamReader r = new StreamReader(stream))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                        txb_xaml.Text += line + "\n";
                }
            }
        }

        private void updatexaml_Click(object sender, RoutedEventArgs e)
        {
            TextRange range;

            range = new TextRange(VacancyRichTextBox.Document.ContentStart, VacancyRichTextBox.Document.ContentEnd);

            MemoryStream stream = new MemoryStream();
            range.Save(stream, DataFormats.Xaml);
            stream.Position = 0;

            StreamReader r = new StreamReader(stream);

            txb_xaml.Text = r.ReadToEnd();
            r.Close();
            stream.Close();
        }

        private void new_ButonClick(object sender, RoutedEventArgs e)
        {
            VacancyRichTextBox.Document = new FlowDocument();
        }
        public string b = "";
        private void richTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void VacancyRichTextBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TextSelection? selection = VacancyRichTextBox.Selection;
            //TextRange a = new TextRange(selection.Start, selection.End);

            b += selection.Text;
            selection.ApplyPropertyValue(TextElement.BackgroundProperty, (SolidColorBrush)new BrushConverter().ConvertFromString("#FFEEF5FD"));
            //Environment.NewLine
            txb_xaml.Text = b;
        }
        // This method will search for a specified word (string) starting at a specified position.
        TextPointer FindWordFromPosition(TextPointer position, string word)
        {
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);

                    // Find the starting index of any substring that matches "word".
                    int indexInRun = textRun.IndexOf(word);
                    if (indexInRun >= 0)
                    {
                        position = position.GetPositionAtOffset(indexInRun);
                        break;
                    }
                }
                else
                {
                    position = position.GetNextContextPosition(LogicalDirection.Forward);
                }
            }

            // position will be null if "word" is not found.
            return position;
        }
    }
}
