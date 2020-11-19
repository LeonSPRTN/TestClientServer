using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;

namespace ServerApp
{
    public class GeneratingNumbers
    {
        static int initialRange;
        static int initialFinite;

        public static int Generate()
        {
            initialRange = int.Parse(ConfigurationManager.AppSettings.Get("initialRange"));
            initialFinite = int.Parse(ConfigurationManager.AppSettings.Get("initialFinite"));

            Random rndm = new Random();
            return rndm.Next(initialRange, initialFinite);
        }
    }
}
