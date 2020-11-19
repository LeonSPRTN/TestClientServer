using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public static class Calculation
    {
        static public double AverageResult { get; set; }
        static public double StdDevResult { get; set; }
        static public double MedianResult { get; set; }
        static public double ModeResult { get; set; }

        public static void Average(List<double> dataNumber)
        {
            if (dataNumber.Count() > 1)
            {
                var copyArr = (double[])dataNumber.ToArray().Clone();
                AverageResult = copyArr.Average();
            }
        }

        public static void CalculateStdDev(List<double> dataNumber)
        {
            if (dataNumber.Count() > 1)
            {
                var copyArr = (double[])dataNumber.ToArray().Clone();
                double average = copyArr.Average();
                StdDevResult = Math.Sqrt((copyArr.Select(x => (x - average) * (x - average)).Sum()) / dataNumber.Count - 1);
            }
        }

        public static void Median(List<double> dataNumber)
        {
            if (dataNumber.Count() > 1)
            {
                double[] copyArr = (double[])dataNumber.ToArray().Clone();
                Array.Sort(copyArr);
                var median = copyArr[copyArr.Length / 2];
                MedianResult = median;
            }
        }

        public static void Mode(List<double> dataNumber)
        {
            if (dataNumber.Count() > 1)
            {
                double[] copyArr = (double[])dataNumber.ToArray().Clone();
                Dictionary<double, int> dict = new Dictionary<double, int>();
                foreach (double elem in copyArr)
                {
                    if (dict.ContainsKey(elem))
                        dict[elem]++;
                    else
                        dict[elem] = 1;
                }

                int maxCount = 0;
                double mode = Double.NaN;
                foreach (double elem in dict.Keys)
                {
                    if (dict[elem] > maxCount)
                    {
                        maxCount = dict[elem];
                        mode = elem;
                    }
                }
                ModeResult = mode;
            }
        }
    }
}
