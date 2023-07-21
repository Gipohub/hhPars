using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var shortDateToday = DateTime.Today.ToString()[..^8];
        }
    }
}
