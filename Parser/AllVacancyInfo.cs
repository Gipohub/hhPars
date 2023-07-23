﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1Tech.Parser
{
    public class AllVacancyInfo
    {
        public string NameOfVacancy { get; set; }
        public string NameOfCompany { get; set; }
        public string CityOfVacancy { get; set; }
        public string HerfOfVacancy { get; set; }
        public string TextOfVacancy { get; set; }
        public AllVacancyInfo(string nameOfVacancy, string nameOfCompany, string cityOfVacancy, string herfOfVacancy, string textOfVacancy)
        {
            this.NameOfVacancy = nameOfVacancy;
            this.NameOfCompany = nameOfCompany;
            this.CityOfVacancy = cityOfVacancy;
            this.HerfOfVacancy = herfOfVacancy;
            this.TextOfVacancy = textOfVacancy;
        }
    }
}
