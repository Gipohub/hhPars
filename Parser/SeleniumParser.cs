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
            int processedVacancy = 0;
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
            switch (settings.RegionSettings)
            {
                case 0: // не менять
                    {
                        break;
                    }
                case 1: // установить общий
                    {
                        try
                        {
                            driver.FindElement(By.XPath(settings.WaysToCityOfSearchPointers[0])).Click();
                            HelperVoids.RndmWait(1000, 1000);
                            driver.FindElement(By.XPath(settings.WaysToCityOfSearchPointers[3])).Click();
                            HelperVoids.RndmWait(1000, 1000);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Сбой сброса региона");
                        }
                        break;
                    }
                case 2: // устанавливать нужный домашний !!! не работает
                    {
                        if (driver.FindElement(By.XPath(settings.WaysToCityOfSearchPointers[0])).Text != settings.HomeRegion)
                        {//индвивидуальный блок,ТРЕБУЕТСЯ УНИФИКАЦИЯ
                            try
                            {
                                driver.FindElement(By.XPath(settings.WaysToCityOfSearchPointers[0])).Click();
                                HelperVoids.RndmWait(1000, 2000);
                                driver.FindElement(By.XPath(settings.WaysToCityOfSearchPointers[1])).Click();
                                HelperVoids.RndmWait(1000, 2000);
                                driver.FindElement(By.XPath(settings.WaysToCityOfSearchPointers[2])).Click();
                                HelperVoids.RndmWait(1000, 2000);
                                var chars = driver.FindElements(By.XPath(settings.WaysToCityOfSearchPointers[3]));
                                foreach (var ch in chars)
                                {
                                    if (ch.Text.ToCharArray()[0] == settings.HomeRegion[0])
                                    {
                                        ch.Click();
                                        break;
                                    }
                                }
                                HelperVoids.RndmWait(1000, 2000);
                                var regionList = driver.FindElements(By.XPath(settings.WaysToCityOfSearchPointers[4]));
                                foreach (var region in regionList)
                                {
                                    if (region.Text == settings.HomeRegion)
                                    {
                                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                                        js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                                        region.FindElement(By.XPath(@"//a[@class='bloko-link bloko-link_disable-visited']")).Click();
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Сбой установки требуемого региона");
                            }
                        }//}
                        break;
                    }

            }
            HelperVoids.RndmWait(1000, 2000);
            //находим поле для поиска и вводим в него последний сохранённый запрос
            driver.FindElement(By.Id(settings.SearchBoxPointer)).SendKeys(request);

            //подождем пару секунд для иммитации действий пользователя
            HelperVoids.RndmWait(500, 3000); 

            //находим кнопку для запуска поиска и нажимаем её
            driver.FindElement(By.XPath($@"{settings.SearchButtonPointer}")).Click();

            //находим общее число найденых вакансий по запросу
            string vacancyCountstr = driver.FindElement(By.XPath($@"{settings.VacancyCountPointer}")).Text;
            int vacancyCount = 0;
            for (int i = 0; i < vacancyCountstr.Length;i++)
            {
                if (int.TryParse($"{vacancyCountstr[i]}", out int num))
                {
                    vacancyCount = (vacancyCount * 10) + num;
                }
            }
            string nameOfVacancy;
            string nameOfCompany;
            string cityOfVacancy = "none";
            string herfOfVacancy;
            string textOfVacancy = "none";

            try
            {
                do
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
                            //страничка вакансии на hh может быть другой структуры (с банерами)
                            int checker = 1;
                            foreach (var item in settings.VacancyDescriptionPointers)
                            {
                                try
                                {
                                    textOfVacancy = driver.FindElement(By.XPath(@$"{item}")).Text;
                                }
                                catch (Exception ex)
                                {
                                    /*try
                                    {
                                        var linksToTextOfVacancy = driver.FindElements(By.XPath(@$"{item}"));
                                        foreach(var link in linksToTextOfVacancy)
                                        {
                                            try
                                            {
                                                textOfVacancy = $"{textOfVacancy} {driver.FindElement(By.XPath(@$"{link}")).Text}";
                                            }
                                            catch {  }
                                        }
                                    }
                                    catch {  }*/
                                    if (checker == settings.VacancyDescriptionPointers.Length)
                                    {
                                        MessageBox.Show(ex.Message, $"Не найден локатор описания вакансии {nameOfVacancy}");
                                    }
                                }
                                checker++;
                                if (textOfVacancy != "none") break;
                            }

                            //запоминаем город вакансии
                            checker = 1;
                            foreach (var item in settings.AdressesOfVacancyPointers)
                            {
                                try
                                {
                                    cityOfVacancy = driver.FindElement(By.XPath(@$"{item}")).Text;
                                }
                                catch (Exception ex)
                                {
                                    if (checker == settings.AdressesOfVacancyPointers.Length)
                                    {
                                        MessageBox.Show(ex.Message, $"Не найден локатор города вакансии {nameOfVacancy}");
                                    }
                                }
                                checker++;
                                if (cityOfVacancy != "none") break;

                            }

                            //генерируем ID вакансии на основе ее данных
                            if (int.TryParse($"{iterationPars}00{vacancyCount}00{textOfVacancy.Length}", out int vacancyId))
                            { }
                            else 
                            {
                                int counter = 1;
                                for(i = 0; i < vacancyCount.Length(); i++)
                                {
                                    counter *= 10;
                                }
                                vacancyId = (iterationPars * 100 * counter) + vacancyCount * 100; }

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
                            processedVacancy++;
                            if (processedVacancy > settings.LimitOfVacancyPars)
                            {
                                stoped = true;
                            }
                        }
                    }
                    else
                    {
                        stoped = true;
                    }
                    try
                    {
                        driver.FindElement(By.XPath(@"//a[@data-qa='pager-next']")).Click();
                    }
                    catch
                    {
                        stoped = true;
                    }
                } while (stoped == false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Oшибка");
            }


        }
    }    
            
}
