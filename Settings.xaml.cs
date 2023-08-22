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
        public string saved_HomeRegion { get ; set; }
        public int saved_LimitOfVacancyPars { get; set; }
        public int saved_RegionSettings { get; set; }
        public string saved_LastReqest { get; set; }
        public string saved_SearchBoxPointer { get; set; } 
        public string saved_SearchButtonPointer { get; set; }
        public string saved_VacancyCountPointer { get; set; }
        public string saved_LinkToFullVacancyPointer { get; set; } 
        public string saved_ShortVacancyPointer { get; set; } 
        public string saved_NameOfVacancyPointer { get; set; } 
        public string saved_NameOfCompanyPointer { get; set; } 
        //public string saved_AdressOfVacancyPointer { get; set; }        
        public string[] saved_AdressesOfVacancyPointers { get; set; }
        //public string saved_VacancyDescriptionPointer { get; set; }
        public string[] saved_VacancyDescriptionPointers { get; set; }
        //public string saved_CityOfSearchPointer { get; set; }
        public string[] saved_WaysToCityOfSearchPointers { get; set; }
    }
    [Serializable]
    public partial class Settings : Window
    {
        private readonly string file = @"ParserSettings.json";
        //класс для интерпретации сохраненных настроек
        public SaverOnlySettings? ss = new();
        //основные настройки программы
        public string ParsFolder { get; set; } = @"C:\Users\Professional\Documents\Visual Studio 2022\hhPars\";
        public string HomeRegion { get; set; } = @"Санкт-Петербург";
        public int LimitOfVacancyPars { get; set; } = 0;
        public int RegionSettings { get; set; } = 0;
        public string LastReqest { get; set; } = @"C#";
        public string DayToday { get; set; } = DateTime.Today.ToString()[..^8];
        // далее указатели для Selenium драйвера
        public string SearchBoxPointer { get; set; } = @$"a11y-search-input";
        public string SearchButtonPointer { get; set; } = @"//button[@data-qa='search-button']";
        public string VacancyCountPointer { get; set; } = @"//span[text()='вакансий' or text()='вакансии']/..";
        public string LinkToFullVacancyPointer { get; set; } = @"body.s-friendly.xs-friendly div#HH-React-Root div div.HH-MainContent.HH-Supernova-MainContent div.main-content div.bloko-columns-wrapper div.sticky-sidebar-and-content--NmOyAQ7IxIOkgRiBRSEg div.bloko-column.bloko-column_xs-4.bloko-column_s-8.bloko-column_m-9.bloko-column_l-13 main.vacancy-serp-content div#a11y-main-content div.serp-item div.vacancy-serp-item__layout div.vacancy-serp-item-body div.vacancy-serp-item-body__main-info div h3.bloko-header-section-3 span a.serp-item__title";
        public string ShortVacancyPointer { get; set; } = @"body.s-friendly.xs-friendly div#HH-React-Root div div.HH-MainContent.HH-Supernova-MainContent div.main-content div.bloko-columns-wrapper div.sticky-sidebar-and-content--NmOyAQ7IxIOkgRiBRSEg div.bloko-column.bloko-column_xs-4.bloko-column_s-8.bloko-column_m-9.bloko-column_l-13 main.vacancy-serp-content div#a11y-main-content div.serp-item";
        public string NameOfVacancyPointer { get; set; } = @"body.s-friendly.xs-friendly div#HH-React-Root div div.HH-MainContent.HH-Supernova-MainContent div.main-content div.bloko-columns-wrapper div.sticky-sidebar-and-content--NmOyAQ7IxIOkgRiBRSEg div.bloko-column.bloko-column_xs-4.bloko-column_s-8.bloko-column_m-9.bloko-column_l-13 main.vacancy-serp-content div#a11y-main-content div.serp-item div.vacancy-serp-item__layout div.vacancy-serp-item-body div.vacancy-serp-item-body__main-info div h3.bloko-header-section-3 span a.serp-item__title";
        public string NameOfCompanyPointer { get; set; } = @"body.s-friendly.xs-friendly div#HH-React-Root div div.HH-MainContent.HH-Supernova-MainContent div.main-content div.bloko-columns-wrapper div.sticky-sidebar-and-content--NmOyAQ7IxIOkgRiBRSEg div.bloko-column.bloko-column_xs-4.bloko-column_s-8.bloko-column_m-9.bloko-column_l-13 main.vacancy-serp-content div#a11y-main-content div.serp-item div.vacancy-serp-item__layout div.vacancy-serp-item-body div.vacancy-serp-item-body__main-info div.vacancy-serp-item-company div.vacancy-serp-item__info div.bloko-v-spacing-container.bloko-v-spacing-container_base-2 div.bloko-text div.vacancy-serp-item__meta-info-company a.bloko-link.bloko-link_kind-tertiary";
        public string[] AdressesOfVacancyPointers { get; set; } = { @"//p[@data-qa='vacancy-view-location']", @"//span[@data-qa='vacancy-view-raw-address']" };
        public string[] VacancyDescriptionPointers { get; set; } = {@"//div[@data-qa='vacancy-description']", @"//*[@class='g-user-content']", @"//div[class='vacancy-description']"};
        public string[] WaysToCityOfSearchPointers { get; set; } = { @"//span[@data-page-analytics-event='vacancy_search_region']", @"//button[@class='bloko-link bloko-link_pseudo bloko-link_kind-secondary HH-ViewSwitcher-Switcher']", @"//button[@class='bloko-icon-link Bloko-Tabs-More-Icon']", @"//div[@class='bloko-tabs__dropdown Bloko-Tabs-Dropdown-Data']/button", @"//div[class='bloko-gap bloko-gap_top']//div[class='Bloko-Tabs-Body']//div[class='bloko-column bloko-column_xs-4 bloko-column_s-4 bloko-column_m-4 bloko-column_l-4']//a[class='bloko-link bloko-link_disable-visited']" };
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
                this.HomeRegion = ss.saved_HomeRegion;
                this.LimitOfVacancyPars = ss.saved_LimitOfVacancyPars;
                this.RegionSettings = ss.saved_RegionSettings;
                this.LastReqest = ss.saved_LastReqest;
                this.SearchBoxPointer = ss.saved_SearchBoxPointer;
                this.SearchButtonPointer = ss.saved_SearchButtonPointer ;
                this.VacancyCountPointer = ss.saved_VacancyCountPointer;
                this.LinkToFullVacancyPointer = ss.saved_LinkToFullVacancyPointer;
                this.ShortVacancyPointer = ss.saved_ShortVacancyPointer;
                this.NameOfVacancyPointer = ss.saved_NameOfVacancyPointer;
                this.NameOfCompanyPointer = ss.saved_NameOfCompanyPointer;
                this.AdressesOfVacancyPointers = ss.saved_AdressesOfVacancyPointers;
                this.VacancyDescriptionPointers = ss.saved_VacancyDescriptionPointers;
                this.WaysToCityOfSearchPointers = ss.saved_WaysToCityOfSearchPointers;
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
                if (CheckLim.IsChecked == false)
                {
                    this.LimitOfVacancyPars = 0;
                }

                if ((bool)NoChangeRadio.IsChecked)
                {
                    this.RegionSettings = 0;
                }
                else if ((bool)ResetRadio.IsChecked)
                {
                    this.RegionSettings = 1;
                }
                else if ((bool)InstallRadio.IsChecked)
                {
                    this.RegionSettings = 2;
                }

                this.SearchBoxPointer = SearchBoxBox.Text;
                this.HomeRegion = HomeRegionTextBox.Text;
                this.SearchButtonPointer = SearchButtonBox.Text;
                this.VacancyCountPointer = VacancyCountBox.Text;
                this.LinkToFullVacancyPointer = LinkToFullVacancyBox.Text;
                this.ShortVacancyPointer = ShortVacancyBox.Text;
                this.NameOfVacancyPointer = NameOfVacancyBox.Text;
                this.NameOfCompanyPointer = NameOfCompanyBox.Text;
                //this.AdressOfVacancyPointer = AdressOfVacancyBox.Text;
                this.AdressesOfVacancyPointers = AdressesOfVacancyBox.Text.Split(new char[] { ',' });
                //this.VacancyDescriptionPointer = VacancyDescriptionBox.Text;
                this.VacancyDescriptionPointers = VacancyDescriptionBox.Text.Split(new char[] { ',' });
                //this.CityOfSearchPointer = CityOfSearchBox.Text;
                this.WaysToCityOfSearchPointers = WaysToCityOfSearchBox.Text.Split(new char[] { ',' });
                SaverOnlySettings ss = new()
                {
                    saved_LimitOfVacancyPars = this.LimitOfVacancyPars,
                    saved_LastReqest = this.LastReqest,
                    saved_RegionSettings = this.RegionSettings,
                    saved_ParsFolder = this.ParsFolder,
                    saved_HomeRegion = this.HomeRegion,
                    saved_SearchBoxPointer = this.SearchBoxPointer,
                    saved_SearchButtonPointer = this.SearchButtonPointer,
                    saved_VacancyCountPointer = this.VacancyCountPointer,
                    saved_LinkToFullVacancyPointer = this.LinkToFullVacancyPointer,
                    saved_ShortVacancyPointer = this.ShortVacancyPointer,
                    saved_NameOfVacancyPointer = this.NameOfVacancyPointer,
                    saved_NameOfCompanyPointer = this.NameOfCompanyPointer,
                    //saved_AdressOfVacancyPointer = this.AdressOfVacancyPointer,
                    saved_AdressesOfVacancyPointers = this.AdressesOfVacancyPointers,
                   // saved_VacancyDescriptionPointer = this.VacancyDescriptionPointer,
                    saved_VacancyDescriptionPointers = this.VacancyDescriptionPointers,
                    //saved_CityOfSearchPointer = this.CityOfSearchPointer,
                    saved_WaysToCityOfSearchPointers = this.WaysToCityOfSearchPointers
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

        private void ParsLimitBox_Initialized(object sender, EventArgs e)
        {
            if (this.LimitOfVacancyPars.ToString() != "0")
            {
                //ParsLimitBox.IsEnabled = true;
                ParsLimitBox.Text = this.LimitOfVacancyPars.ToString();
                if(CheckLim.IsInitialized == true)
                {
                    CheckLim.IsChecked = true;
                }
            }
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

       // private void AdressesOfVacancyBox_Initialized(object sender, EventArgs e)
       // {
        //    AdressesOfVacancyBox.Text = AdressesOfVacancyPointers;
       // }
        private void AdressesOfVacancyBox_Initialized(object sender, EventArgs e)
        {
            //SpareAdressOfVacancyPointer
            AdressesOfVacancyBox.Text = String.Join(',', AdressesOfVacancyPointers);
        }

        private void VacancyDescriptionBox_Initialized(object sender, EventArgs e)
        {
            VacancyDescriptionBox.Text = String.Join(',', VacancyDescriptionPointers);
        }
        private void WaysToCityOfSearchBox_Initialized(object sender, EventArgs e)
        {
            WaysToCityOfSearchBox.Text = String.Join(',', WaysToCityOfSearchPointers);
        }
        private void HomeRegionTextBox_Initialized(object sender, EventArgs e)
        {
            HomeRegionTextBox.Text = HomeRegion;
            if(this.RegionSettings == 2)
            {
                HomeRegionTextBox.IsEnabled = true;
            }

        }
        private void VacancyCountBox_Initialized(object sender, EventArgs e)
        {
            VacancyCountBox.Text = VacancyCountPointer;
        }
        private void NoChangeRadio_Initialized(object sender, EventArgs e)
        {
            if (this.RegionSettings == 0)
            {
                NoChangeRadio.IsChecked = true;
            }
        }
        private void ResetRadio_Initialized(object sender, EventArgs e)
        {
            if (this.RegionSettings == 1)
            {
                ResetRadio.IsChecked = true;
            }
        }
        private void InstallRadio_Initialized(object sender, EventArgs e)
        {
            if (this.RegionSettings == 2)
            {
                InstallRadio.IsChecked = true;
            }
        }

        /// <summary>
        /// only int input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParsLimitBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out int result)) //допускаем символ к записи если это цифра
            {
                e.Handled = true;
            }
            else if (ParsLimitBox.Text.Length > 3)// также цифр не более 4
            {
                e.Handled = true;
            }
            
        }
        private void ParsLimitBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(ParsLimitBox.Text, out int result))
            {
                this.LimitOfVacancyPars = result;
            }

        }
        void CheckLim_Checked(object sender, RoutedEventArgs e)
        {
            //Max.Visibility = Visibility.Visible;
            ParsLimitBox.IsEnabled = true;
            //numberBox.Text = "";
        }

        private void CheckLim_Unchecked(object sender, RoutedEventArgs e)
        {
            //Max.Visibility = Visibility.Hidden;
            ParsLimitBox.IsEnabled = false;
            //numberBox.Text = "none";
        }

        /*private void NoChangeRadio_Checked(object sender, RoutedEventArgs e)
        {
            this.RegionSettings = 0;
        }

        private void ResetRadio_Checked(object sender, RoutedEventArgs e)
        {
            this.RegionSettings = 1;
        }*/

        private void InstallRadio_Checked(object sender, RoutedEventArgs e)
        {
            this.RegionSettings = 2;
            try
            {
                HomeRegionTextBox.IsEnabled = true;
            }
            catch {  }
        }

        private void InstallRadio_Unchecked(object sender, RoutedEventArgs e)
        {
            HomeRegionTextBox.IsEnabled = false;
        }

        
    }   
}

