using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1Tech
{
    public class VacancyData1
    {
        public string NameOfCompany { get; set; }
        public int Count { get; set; }
        public string Title { get; set; }
        public string Herf { get; set; }
        public string CityOfCompany { get; set; }
        public string Description { get; set; }
        public VacancyData1(int companyId, string nameOfCompany, string titleOfVacancy, string vacancyHerf, string cityOfCompany, string textOfVacancy)
        {
            NameOfCompany = nameOfCompany;
            Count = companyId;
            Title = titleOfVacancy;
            Herf = vacancyHerf;
            CityOfCompany = cityOfCompany;
            Description = textOfVacancy;

        }

    }
}
