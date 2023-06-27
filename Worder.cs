using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1Tech
{
    public class Worder
    {
        public string Word  { get;}
        public string LemmasWord { get; set; }
        public string VoisPartOfLemmas { get; set; }

        public int LemmasWordNumOfDic { get; set; }
        public int VacancyID { get; set; }
        public int StringID { get; set; }
       
        public bool IsTech { get; set; }

    }


}
