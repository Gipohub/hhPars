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
using WpfApp1Tech.Interpritator;

namespace WpfApp1Tech
{
    /// <summary>
    /// Логика взаимодействия для Lemm2Wind.xaml
    /// (таска для Lemm2)
    /// </summary>
    public partial class Lemm2Wind : Window
    {
        public string NewTech = "";//{ get; set;}
        
        public Lemm2Wind()
        {

            InitializeComponent();
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
                        NewVacancyTechBox.Text += line + "\n";
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

            NewVacancyTechBox.Text = r.ReadToEnd();
            r.Close();
            stream.Close();
        }

        private void new_ButonClick(object sender, RoutedEventArgs e)
        {
            VacancyRichTextBox.Document = new FlowDocument();
        }
        //public string b = "";

        private void VacancyRichTextBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TextSelection? selection = VacancyRichTextBox.Selection;
            TextRange a = new TextRange(selection.Start, selection.End);

            NewTech += $"*{selection.Text}";
            selection.ApplyPropertyValue(TextElement.BackgroundProperty, (SolidColorBrush)new BrushConverter().ConvertFromString("#FFE4FCDF"));
            //Environment.NewLine
            NewVacancyTechBox.Text = NewTech;
            
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

        private void OksButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CansButton_Click(object sender, RoutedEventArgs e)
        {
            NewTech = "";
            Lemm2.Stop = true;
            Close();
        }
    }
}
