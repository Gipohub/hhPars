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



    

    public class SaverOnlySettings
    {
        public string saved_ParsFolder { get ; set; }
        public int saved_LimitOfVacancyPars { get; set; }
        public string saved_LastReqest { get; set; }
        public string saved_SearchBoxPointer { get; set; } 
        public string saved_SearchButtonPointer { get; set; } 
        public string saved_LinkToFullVacancyPointer { get; set; } 
        public string saved_ShortVacancyPointer { get; set; } 
        public string saved_NameOfVacancyPointer { get; set; } 
        public string saved_NameOfCompanyPointer { get; set; } 
        public string saved_AdressOfVacancyPointer { get; set; }        
        public string[] saved_SpareAdressOfVacancyPointer { get; set; }
        public string saved_VacancyDescriptionPointer { get; set; }
        public string[] saved_RichVacancyDescriptionPointer { get; set; }
    }
    [Serializable]
    public partial class Settings : Window
    {
        private readonly string file = @"ParserSettings.json";
        //класс для интерпретации сохраненных настроек
        public SaverOnlySettings? ss = new();
        //основные настройки программы
        public string ParsFolder { get; set; } = @"C:\Users\Professional\Documents\Visual Studio 2022\hhPars\";
        public int LimitOfVacancyPars { get; set; } = 0;
        public string LastReqest { get; set; } = @"C#";
        public string DayToday { get; set; } = DateTime.Today.ToString()[..^8];
        // далее указатели для Selenium драйвера
        public string SearchBoxPointer { get; set; } = @"//html/body/div[4]/div/div[3]/div[1]/div[1]/div/div/div[1]/div/form/div/div[1]/fieldset/input";
        public string SearchButtonPointer { get; set; } = @"//html/body/div[4]/div/div[3]/div[1]/div[1]/div/div/div[1]/div/form/div/div[2]/button";
        public string LinkToFullVacancyPointer { get; set; } = @"body.s-friendly.xs-friendly div#HH-React-Root div div.HH-MainContent.HH-Supernova-MainContent div.main-content div.bloko-columns-wrapper div.sticky-sidebar-and-content--NmOyAQ7IxIOkgRiBRSEg div.bloko-column.bloko-column_xs-4.bloko-column_s-8.bloko-column_m-9.bloko-column_l-13 main.vacancy-serp-content div#a11y-main-content div.serp-item div.vacancy-serp-item__layout div.vacancy-serp-item-body div.vacancy-serp-item-body__main-info div h3.bloko-header-section-3 span a.serp-item__title";
        public string ShortVacancyPointer { get; set; } = @"body.s-friendly.xs-friendly div#HH-React-Root div div.HH-MainContent.HH-Supernova-MainContent div.main-content div.bloko-columns-wrapper div.sticky-sidebar-and-content--NmOyAQ7IxIOkgRiBRSEg div.bloko-column.bloko-column_xs-4.bloko-column_s-8.bloko-column_m-9.bloko-column_l-13 main.vacancy-serp-content div#a11y-main-content div.serp-item";
        public string NameOfVacancyPointer { get; set; } = @"body.s-friendly.xs-friendly div#HH-React-Root div div.HH-MainContent.HH-Supernova-MainContent div.main-content div.bloko-columns-wrapper div.sticky-sidebar-and-content--NmOyAQ7IxIOkgRiBRSEg div.bloko-column.bloko-column_xs-4.bloko-column_s-8.bloko-column_m-9.bloko-column_l-13 main.vacancy-serp-content div#a11y-main-content div.serp-item div.vacancy-serp-item__layout div.vacancy-serp-item-body div.vacancy-serp-item-body__main-info div h3.bloko-header-section-3 span a.serp-item__title";
        public string NameOfCompanyPointer { get; set; } = @"body.s-friendly.xs-friendly div#HH-React-Root div div.HH-MainContent.HH-Supernova-MainContent div.main-content div.bloko-columns-wrapper div.sticky-sidebar-and-content--NmOyAQ7IxIOkgRiBRSEg div.bloko-column.bloko-column_xs-4.bloko-column_s-8.bloko-column_m-9.bloko-column_l-13 main.vacancy-serp-content div#a11y-main-content div.serp-item div.vacancy-serp-item__layout div.vacancy-serp-item-body div.vacancy-serp-item-body__main-info div.vacancy-serp-item-company div.vacancy-serp-item__info div.bloko-v-spacing-container.bloko-v-spacing-container_base-2 div.bloko-text div.vacancy-serp-item__meta-info-company a.bloko-link.bloko-link_kind-tertiary";
        public string AdressOfVacancyPointer { get; set; } = @"//html/body/div[5]/div/div[3]/div[1]/div/div/div/div/div/div[2]/div/div[1]/div/div/div/a/span";
        public string[] SpareAdressOfVacancyPointer { get; set; } = { @"//html/body/div[5]/div/div[3]/div[1]/div/div/div/div/div[2]/div/div[1]/div[1]/div/div/div/a/span", "@//html/body/div[5]/div/div[3]/div[1]/div/div/div/div/div/div[2]/div/div[1]/div/div/div/p" };

        public string VacancyDescriptionPointer { get; set; } = @"body.s-friendly.xs-friendly div#HH-React-Root div div.HH-MainContent.HH-Supernova-MainContent div.main-content div.bloko-columns-wrapper div.row-content div.bloko-text.bloko-text_large div.bloko-columns-row div.bloko-column.bloko-column_container.bloko-column_xs-4.bloko-column_s-8.bloko-column_m-12.bloko-column_l-10 div.bloko-columns-row div.bloko-column.bloko-column_xs-4.bloko-column_s-8.bloko-column_m-12.bloko-column_l-10 div.vacancy-description div.vacancy-section div.g-user-content";
        public string[] RichVacancyDescriptionPointer { get; set; } = {@"//*[@id=""HH-React-Root""]/div/div[3]/div[1]/div/div/div/div/div/div[3]/div[1]/div/div/div/div/div/div/div[2]/div/div", "@//html/body/div[5]/div/div[3]/div[1]/div/div/div/div/div/div[3]/div/div/div[1]/div"};
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
                ss = serializer.Deserialize<SaverOnlySettings>(jtr);
                this.ParsFolder = ss.saved_ParsFolder;
                this.LimitOfVacancyPars = ss.saved_LimitOfVacancyPars;
                this.LastReqest = ss.saved_LastReqest;
                this.SearchBoxPointer = ss.saved_SearchBoxPointer;
                this.SearchButtonPointer = ss.saved_SearchButtonPointer ;
                this.LinkToFullVacancyPointer = ss.saved_LinkToFullVacancyPointer;
                this.ShortVacancyPointer = ss.saved_ShortVacancyPointer;
                this.NameOfVacancyPointer = ss.saved_NameOfVacancyPointer;
                this.NameOfCompanyPointer = ss.saved_NameOfCompanyPointer;
                this.AdressOfVacancyPointer = ss.saved_AdressOfVacancyPointer;
                this.SpareAdressOfVacancyPointer = ss.saved_SpareAdressOfVacancyPointer;
                this.VacancyDescriptionPointer = ss.saved_VacancyDescriptionPointer;
                this.RichVacancyDescriptionPointer = ss.saved_RichVacancyDescriptionPointer;

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
                PathWayParsBlock.Text = folder;
            }
        }  
        public void oks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.SearchBoxPointer = SearchBoxBox.Text;
                this.SearchButtonPointer = SearchButtonBox.Text;               
                this.LinkToFullVacancyPointer = LinkToFullVacancyBox.Text;
                this.ShortVacancyPointer = ShortVacancyBox.Text;
                this.NameOfVacancyPointer = NameOfVacancyBox.Text;
                this.NameOfCompanyPointer = NameOfCompanyBox.Text;
                this.AdressOfVacancyPointer = AdressOfVacancyBox.Text;

                this.SpareAdressOfVacancyPointer = SpareAdressOfVacancyBox.Text.Split(new char[] { ',' });


                this.VacancyDescriptionPointer = VacancyDescriptionBox.Text;
                this.RichVacancyDescriptionPointer = RichVacancyDescriptionBox.Text.Split(new char[] { ',' });
                SaverOnlySettings ss = new()
                {
                    saved_LimitOfVacancyPars = this.LimitOfVacancyPars,
                    saved_LastReqest = this.LastReqest,
                    saved_ParsFolder = this.ParsFolder,
                    saved_SearchBoxPointer = this.SearchBoxPointer,
                    saved_SearchButtonPointer = this.SearchButtonPointer,
                    saved_LinkToFullVacancyPointer = this.LinkToFullVacancyPointer,
                    saved_ShortVacancyPointer = this.ShortVacancyPointer,
                    saved_NameOfVacancyPointer = this.NameOfVacancyPointer,
                    saved_NameOfCompanyPointer = this.NameOfCompanyPointer,
                    saved_AdressOfVacancyPointer = this.AdressOfVacancyPointer,
                    saved_SpareAdressOfVacancyPointer = this.SpareAdressOfVacancyPointer,
                    saved_VacancyDescriptionPointer = this.VacancyDescriptionPointer,
                    saved_RichVacancyDescriptionPointer = this.RichVacancyDescriptionPointer
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
                MessageBox.Show("Настройки сохранены!", "Сохранение настроек");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Ошибка сохранения настроек");
            }
        }

        private void Cans_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void PathWayParsBlock_Initialized(object sender, EventArgs e)
        {
            PathWayParsBlock.Text = this.ParsFolder;
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

        private void SearchBoxBox_Initialized(object sender, EventArgs e)
        {
            SearchBoxBox.Text = SearchBoxPointer;
        }        

        private void SearchButtonBox_Initialized(object sender, EventArgs e)
        {
            SearchButtonBox.Text = SearchButtonPointer;
        }

        private void LinkToFullVacancyBox_Initialized(object sender, EventArgs e)
        {
            LinkToFullVacancyBox.Text = LinkToFullVacancyPointer;
        }

        private void ShortVacancyBox_Initialized(object sender, EventArgs e)
        {
            ShortVacancyBox.Text = ShortVacancyPointer;
        }

        private void NameOfVacancyBox_Initialized(object sender, EventArgs e)
        {
            NameOfVacancyBox.Text = NameOfVacancyPointer;
        }

        private void NameOfCompanyBox_Initialized(object sender, EventArgs e)
        {
            NameOfCompanyBox.Text = NameOfCompanyPointer;
        }

        private void AdressOfVacancyBox_Initialized(object sender, EventArgs e)
        {
            AdressOfVacancyBox.Text = AdressOfVacancyPointer;
        }
        private void SpareAdressOfVacancyBox_Initialized(object sender, EventArgs e)
        {
            //SpareAdressOfVacancyPointer
            SpareAdressOfVacancyBox.Text = String.Join(',', SpareAdressOfVacancyPointer);
        }

        private void VacancyDescriptionBox_Initialized(object sender, EventArgs e)
        {
            VacancyDescriptionBox.Text = VacancyDescriptionPointer;
        }

        private void RichVacancyDescriptionBox_Initialized(object sender, EventArgs e)
        {
            RichVacancyDescriptionBox.Text = String.Join(',', RichVacancyDescriptionPointer);
        }

        
    }   
}

