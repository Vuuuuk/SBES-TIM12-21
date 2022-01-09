using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.ConfigurationManager
{
    public class ConfigurationManager
    {
       
        public int GetLockDuration() {

            StreamReader sw = new StreamReader("../../../Config.txt");
            string line = sw.ReadLine();
            return int.Parse(line.Split('|')[0].Split(':')[1]);
        }
        public int GetTimeOutInterval()
        {
            StreamReader sw = new StreamReader("../../../Config.txt");
            string line = sw.ReadLine();
            return int.Parse(line.Split('|')[1].Split(':')[1]);
        }
    }
}
