using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1Tech.Works
{
    public static class HelperVoids
    {
        public static void RndmWait(int min, int max)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var randomInt = random.Next(min, max);
            Thread.Sleep(randomInt);
        }
        //public static void Separator(string data, char separator, out string[] result)
        //{

        //}
    }
}
