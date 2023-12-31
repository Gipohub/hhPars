﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1Tech.Interpritator
{
    public class TechDictionary
    {
        public long[] VacancyID { get; set; }
        public string Word { get; set; }
        public int UsingTimes { get; set; }
        public bool IsTech { get; set; }
        //public Vector <UseWordPerDate> VectorPerDate { get; set; }
        public TechDictionary(long[] vacancyId, string word, int usingTimes/*Vector <UseWordPerDate> vectorPerDate*/, bool isTech)
        {
            VacancyID = vacancyId;
            Word = word;
            UsingTimes = usingTimes;
            IsTech = isTech;
            //VectorPerDate = vectorPerDate;
        }

    }
    public class UseWordPerDate
    {
        public string Date { get; set; }
        public int UsingTimes { get; set; }
        public UseWordPerDate(string date, int usingTimes)
        {
            Date = date;
            UsingTimes = usingTimes;
        }
    }
}
