using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using WpfApp1Tech.Works;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WpfApp1Tech.Parser
{
    public class SeleniumParser
    {
        Settings settings = new Settings();
        public int iterationPars = 0;

        public SeleniumParser(string request)
        {
            ChromeParser(request);
        }
        public void ChromeParser(string request)
        {
            /*var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments(new string[] { "--no-sandbox", "test-type", "--start-maximized" });
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = false;
            IWebDriver driver = new ChromeDriver(chromeDriverService, chromeOptions, TimeSpan.FromSeconds(120.0));
            */

            //var capabilities = new DesiredCapabilities();
            // capabilities.setCapability(CapabilityType.PageLoadStrategy, "eager");
            // WebDriver driver = new FirefoxDriver(capabilities);
            // WebDriverWait wait = new WebDriverWait(driver, 10);


            // var options = new ChromeOptions();
            // options.AddAdditionalOptions(CapabilityType.UnexpectedAlertBehavior, "accept");
            // драйвер для откытия и навигации по браузеру Chrome

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddExtension(@"C:\Users\Professional\source\repos\WpfApp1\1.51.0_0.crx");
            IWebDriver driver = new ChromeDriver(chromeOptions);
            HelperVoids.RndmWait(1000, 1000);

            //далее ждем ответа от сайта
            bool noAccept = true;
            bool stoped = false;
            int trysCount = 0;
            //
            do
            {
                try
                {
                    //driver.Url = @"https://google.com/";
                   // driver.Navigate().Refresh();
                    driver.Url = @"https://hh.ru/";
                    driver.Navigate().Refresh();
                    noAccept = false;
                }
                catch
                {
                    //driver.Close();
                    driver = new ChromeDriver();
                    continue;
                }
                trysCount++;
                Math.DivRem(trysCount, 5, out int intResult);
                //каждые пять попыток можно остановить запрос
                if (intResult == 0 || noAccept == true)
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show($"done {intResult} tryes, continue?\r(\"No\", if stop trying");
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            break;
                        case MessageBoxResult.No:
                            stoped = true;
                            break;

                    }
                }
                if (stoped) return;
            }
            while (noAccept);
            if (noAccept) return;

            //установка требуеиого домашнего региона
            if (driver.FindElement(By.XPath(@"//html/body/div[4]/div/div[1]/div/div/div/div/div[1]/div[1]/div[1]/button/span")).Text != "Киров (Кировская область)")
            {//индвивидуальный блок,ТРЕБУЕТСЯ УНИФИКАЦИЯ
                try
                {
                    driver.FindElement(By.XPath(@"//html/body/div[4]/div/div[1]/div/div/div/div/div[1]/div[1]/div[1]/button/span")).Click();
                    HelperVoids.RndmWait(1000, 1000);
                    driver.FindElement(By.XPath(@"/html/body/div[4]/div/div[1]/div[2]/div/div[2]/div[2]/div[2]/div/div/div[1]/div[2]/div/div/button")).Click();
                    HelperVoids.RndmWait(1000, 1000);
                    driver.FindElement(By.XPath(@"/html/body/div[4]/div/div[1]/div[2]/div/div[2]/div[2]/div[2]/div/div/div[5]/div/div[2]/div[2]/div[12]/div/div[2]/ul/li[52]/a")).Click();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Сбой установки требуемого региона") ;
                }
            }//}

            

            //находим поле для поиска и вводим в него последний сохранённый запрос
            driver.FindElement(By.Id(@$"a11y-search-input")).SendKeys(request);

            //подождем пару секунд для иммитации действий пользователя
            HelperVoids.RndmWait(500, 3000); 

            //находим кнопку для запуска поиска и нажимаем её
            driver.FindElement(By.XPath($@"{settings.SearchButtonPointer}")).Click();
            
            string nameOfVacancy;
            string nameOfCompany;
            string cityOfVacancy = "none";
            string herfOfVacancy;
            string textOfVacancy = "none";
            
            try
            {
                //массив ссылок на все полные вакансии на странице
                var linkMassive = driver.FindElements(By.CssSelector(@$"{settings.LinkToFullVacancyPointer}"));
                //поиск всех коротких анкет вакансий на странице
                var companyList = driver.FindElements(By.CssSelector(@$"{settings.ShortVacancyPointer}"));

                if (companyList.Count > 0)
                {
                    for (int i = 0; i < companyList.Count; i++)
                    {
                        iterationPars++;
                        //находим название конкретной вакансии
                        nameOfVacancy = companyList[i].FindElement(By.CssSelector(@$"{settings.NameOfVacancyPointer}")).Text;                        
                        //находим название компании
                        nameOfCompany = companyList[i].FindElement(By.CssSelector(@$"{settings.NameOfCompanyPointer}")).Text;
                        
                        //чистим названия воизбежание искажений пути к папке
                        nameOfVacancy = nameOfVacancy.Replace("/", "");
                        nameOfCompany = nameOfCompany.Replace("/", "");
                        nameOfVacancy = nameOfVacancy.Replace("\\", "");
                        nameOfCompany = nameOfCompany.Replace("\\", "");

                        HelperVoids.RndmWait(500, 2000);
                        //переходим на страничку вакансии
                        linkMassive[i].Click();
                        //переключаемся на эту вкладку
                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                        //запоминаем Url вакансии
                        herfOfVacancy = driver.Url;
                        
                        //собираем текст вакансии
                        try
                        {
                            textOfVacancy = driver.FindElement(By.CssSelector(@$"{settings.VacancyDescriptionPointer}")).Text;
                        }
                        catch //страничка вакансии на hh может быть другой структуры (с банерами)
                        {
                            try
                            {
                                foreach (var item in settings.SpareAdressOfVacancyPointer)
                                {
                                    textOfVacancy = driver.FindElement(By.XPath(@$"{item}")).Text;
                                }
                            }
                            catch
                            {
                                                             
                            }
                            
                        }
                        //запоминаем город вакансии
                        try
                        {
                            cityOfVacancy = driver.FindElement(By.XPath(@$"{settings.AdressOfVacancyPointer}")).Text;
                        }
                        catch
                        {
                            try
                            {
                                foreach (var item in settings.SpareAdressOfVacancyPointer)
                                {
                                    cityOfVacancy = driver.FindElement(By.XPath(@$"{item}")).Text;
                                }
                            }
                            catch
                            {
                                                           
                            }
                        }

                        //генерируем ID вакансии на основе ее данных
                        if (int.TryParse($"{iterationPars}0000{textOfVacancy.Length}", out int vacancyId))
                             { }
                        else { vacancyId = iterationPars * 10000; }
                        
                        //заворачиваем собранную информацию в класс
                        VacancyData vacancy = new(vacancyId, nameOfVacancy, nameOfCompany, cityOfVacancy, herfOfVacancy, textOfVacancy);
                        try
                        {
                            string pathStringCompany = Path.Combine(settings.ParsFolder, request, settings.DayToday, $"{nameOfCompany}");
                           
                            string fileString;                                                      
                            if (Directory.Exists(pathStringCompany))
                            {
                                fileString = Path.Combine(pathStringCompany, $"{Directory.GetFiles(pathStringCompany).Length + 1}{nameOfVacancy} ({iterationPars}).json");
                            } 
                            else
                            {
                                Directory.CreateDirectory(pathStringCompany);
                                fileString = Path.Combine(pathStringCompany, $"{01}{nameOfVacancy} ({iterationPars}).json");

                            }

                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Formatting = Formatting.Indented;
                            serializer.NullValueHandling = NullValueHandling.Ignore;
                            using (var sw = new StreamWriter(fileString))
                            using (var jw = new JsonTextWriter(sw))
                            {
                                serializer.Serialize(jw, vacancy);                                                                
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{nameOfVacancy} {nameOfCompany}, Ошибка:{ex.Message}", $"Oшибка при записи");
                        }

                        HelperVoids.RndmWait(500, 3000);//ожидание и закрытие вкладки
                        driver.Close();
                        driver.SwitchTo().Window(driver.WindowHandles.First());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Oшибка");
            }


        }
    }    
            
}
