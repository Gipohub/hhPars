using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WindowsAPICodePack.Dialogs;
using System.IO;
using System.Security.Cryptography.Xml;

//using System.Windows.Forms;

namespace WpfApp1Tech
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>



    

    public class SaverSettings
    {
        public string save_ParsFolder { get ; set; }
        public int save_LimitOfVacancyPars { get; set; }
        public string save_LastReqest { get; set; }

        /*public SaverSettings(string wayToParsFolder,int limit, string lastReqest)
        {

            ParsFolder = wayToParsFolder;
            LimitOfVacancyPars = limit;
            LastReqest = lastReqest;

        }*/

    }
    [Serializable]
    public partial class Settings : Window
    {
        private readonly string file = @"ParserSettings.json";
        //public static SaverSettings ssDef = new(@"C:\Users\Professional\Documents\Visual Studio 2022\hhPars\C#", 0, @"C#");
        public SaverSettings? ss = new();
        //ISaverSettings parserSettings;
        public string ParsFolder { get; set; } = @"C:\Users\Professional\Documents\Visual Studio 2022\hhPars\";
        public int LimitOfVacancyPars { get; set; } = 0;
        public string LastReqest { get; set; } = @"C#";
        public string DayToday { get; set; } = DateTime.Today.ToString()[..^8];
        // далее указатели для Selenium драйвера
        public string SearchBoxPointer { get; set; }
        public string SearchButtonPointer { get; set; }
        public string LinkToFullVacancyPointer { get; set; }
        public string ShortVacancyPointer { get; set; }
        public string NameOfVacancyPointer { get; set; }
        public string NameOfCompanyPointer { get; set; }
        public string CityOfVacancyPointer { get; set; }
        public string TextOfVacancyPointer { get; set; }

        public Settings()
        {
            StartSet();
            InitializeComponent();
            
            // чтение данных


        }
        //public ISaverSettings ParserSettings
        //{
        //    get { return parserSettings; } 
        //    set
        //    {
        //        parserSettings = value;
                //новые настройки парсера
                //loader = new HtmlLoader(value);
                // сюда помещаются настройки для загрузчика страницы
        //    }
        //}
         
        public void StartSet()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            try
            {


                using var sr = new StreamReader(file);//чтение потока из указанного файла
                using var jtr = new JsonTextReader(sr);// валидауция например
                ss = serializer.Deserialize<SaverSettings>(jtr);
                this.ParsFolder = ss.save_ParsFolder;
                this.LimitOfVacancyPars = ss.save_LimitOfVacancyPars;
                this.LastReqest = ss.save_LastReqest;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Установлены настройки по умолчанию");                
            }
        }
        public void BtnOpenFolder_Click(object sender, RoutedEventArgs e)
        {

            using var dialog = new CommonOpenFileDialog
            {   
                IsFolderPicker = true,
                Multiselect = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {            
                var folder = string.Join("\n", dialog.FileNames);         
                this.ParsFolder = folder;
                TextBlock1.Text = folder;
            }
        }  
        public void oks_Click(object sender, RoutedEventArgs e)
        {
            SaverSettings ss = new()
            {
                save_LimitOfVacancyPars = this.LimitOfVacancyPars,
                save_LastReqest = this.LastReqest,
                save_ParsFolder = this.ParsFolder
            };
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.NullValueHandling = NullValueHandling.Ignore;
            using (var sw = new StreamWriter(file))
            using (var jw = new JsonTextWriter(sw))
            {
                serializer.Serialize(jw, ss);
            }
            Close();
        }

        private void Cans_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void TextBlock1_Initialized(object sender, EventArgs e)
        {
            TextBlock1.Text = this.ParsFolder;
        }



        private void numberBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out int result))
            {
                e.Handled = true;
            }
            else if (numberBox.Text.Length > 3)
            {
                e.Handled = true;
            }
        }
        private void numberBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(numberBox.Text, out int result))
            {
                this.LimitOfVacancyPars = result;
            }
            
        }

       

        private void numberBox_Initialized(object sender, EventArgs e)
        {
            if (this.LimitOfVacancyPars.ToString() != "0")
            {
                numberBox.IsEnabled = true;
                var i = this.LimitOfVacancyPars.ToString();
                numberBox.Text = i;
            }
        }

       
        private void CheckLim_Initialized(object sender, EventArgs e)
        {
            if (this.LimitOfVacancyPars > 0)
            {
            //    CheckLim.Checked = CheckBox. 
            }
        }

        void CheckLim_Checked(object sender, RoutedEventArgs e)
        {
            Max.Visibility = Visibility.Visible;
            numberBox.IsEnabled = true;
            //numberBox.Text = "";
        }

        private void CheckLim_Unchecked(object sender, RoutedEventArgs e)
        {
            Max.Visibility = Visibility.Hidden;
            numberBox.IsEnabled = false;
            //numberBox.Text = "none";
        }
    }   
}

