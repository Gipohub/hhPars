using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfApp1Tech.Parser
{
    public class SeleniumParser
    {
        Settings settings = new Settings();

        public SeleniumParser()
        {
            ChromeParser();
        }
        void ChromeParser()
        {
            var shortDateToday = settings.DayToday;

            // драйвер для откытия и навигации по браузеру Chrome
            IWebDriver driver = new ChromeDriver();
            driver.Url = @"https://hh.ru/";

            //находим поле для поиска и вводим в него последний сохранённый запрос
            driver.FindElement(By.XPath(settings.SearchBoxPointer)).SendKeys(settings.LastReqest);

            //RndmWait(1000, 2000); //click on search:
            //находим кнопку для запуска поиска и нажимаем её
            driver.FindElement(By.XPath(settings.SearchButtonPointer)).Click();



            int iterationPars = 0;
            string nameOfVacancy;
            string nameOfCompany;
            string cityOfVacancy;
            string herfOfVacancy;
            string textOfVacancy;
            ReadOnlyCollection<IWebElement> ss = (ReadOnlyCollection<IWebElement>)driver;
            try
            {
                //массив ссылок на все полные вакансии на странице
                var linkMassive = driver.FindElements(By.CssSelector(settings.LinkToFullVacancyPointer));
                //поиск всех коротких анкет вакансий на странице
                var companyList = driver.FindElements(By.CssSelector(settings.ShortVacancyPointer));

                if (companyList.Count > 0)
                {
                    for (int i = 0; i < companyList.Count; i++)
                    {
                        iterationPars++;
                        //находим название конкретной вакансии
                        nameOfVacancy = companyList[i].FindElement(By.CssSelector(settings.NameOfVacancyPointer)).Text;
                        //находим название компании
                        nameOfCompany = companyList[i].FindElement(By.CssSelector(settings.NameOfCompanyPointer)).Text;

                        //переходим на страничку вакансии
                        linkMassive[i].Click();
                        //переключаемся на эту вкладку
                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                        //запоминаем Url вакансии
                        herfOfVacancy = driver.Url;
                        //запоминаем город вакансии
                        cityOfVacancy = driver.FindElement(By.CssSelector(settings.CityOfVacancyPointer)).Text;
                        //собираем текст вакансии
                        textOfVacancy = driver.FindElement(By.CssSelector(settings.TextOfVacancyPointer)).Text;

                        //заворачиваем собранную инфориацию в класс
                        AllVacancyInfo vacancy = new(nameOfVacancy, nameOfCompany, cityOfVacancy, herfOfVacancy, textOfVacancy);
                        try
                        {
                            string fileString = System.IO.Path.Combine(settings.ParsFolder,settings.DayToday,$"{iterationPars} {nameOfCompany}",$"{nameOfVacancy}.json");

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
                            MessageBox.Show(ex.Message, "Oшибка при записи");
                        }
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
