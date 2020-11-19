using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
using System.Xml;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        //static IPAddress ipAddress;

        static List<double> dataNumber = new List<double>();
        static bool writeCalculation = false;
        static UdpClient receiver;
        static uint lostPacket;
        static int delayRes;

        static void Main(string[] args)
        {
            try
            {
                #region XmlLoad
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("Setting.xml");

                var xDocElems = xDoc.DocumentElement;

                var ChildNodes = xDocElems.ChildNodes;

                var ipRes = ChildNodes[0].ChildNodes[0].Value;
                delayRes = int.Parse(ChildNodes[1].ChildNodes[0].Value);
                #endregion

                IPAddress ipAddress = IPAddress.Parse(ipRes);

                receiver = new UdpClient(8001);
                receiver.JoinMulticastGroup(ipAddress, 1);

                //Thread receiveThread = new Thread(new ThreadStart(ReceiveNumber));
                Task receiveThread = new Task(() => ReceiveNumber());
                receiveThread.Start();

                Console.WriteLine("Нажмите enter для получения данных");
                while (true)
                {
                    Console.ReadLine();

                    writeCalculation = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ReceiveNumber()
        {
            try
            {
                IPEndPoint remoteIp = null;
                byte[] data = null;
                int number;
                while (true)
                {
                    var receive = new Task(() =>
                    {
                        byte[] data = null;
                        Thread.Sleep(delayRes);                      
                        data = receiver.Receive(ref remoteIp);
                        string numberString = Encoding.UTF8.GetString(data);
                        number = int.Parse(numberString);
                        dataNumber.Add(number);
                    });
                    receive.Start();

                    Task task = new Task(() =>
                    {
                        if (data == null)
                        {
                            lostPacket += 1;
                        }
                    });
                    task.Start();

                    Thread CalculationsThread = new Thread(new ThreadStart(Calculations));
                    CalculationsThread.Name = "WriteCalculation";
                    CalculationsThread.Start();
                    CalculationsThread.Join();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }

        private static void Calculations()
        {
            Calculation.Average(dataNumber);
            Calculation.CalculateStdDev(dataNumber);
            Calculation.Mode(dataNumber);
            Calculation.Median(dataNumber);

            if (writeCalculation)
            {
                Thread CalculationsThread = new Thread(new ThreadStart(WriteCalculation));
                CalculationsThread.Name = "Calculations";
                CalculationsThread.Start();
                CalculationsThread.Join();
            }
        }

        private static void WriteCalculation()
        {
            Console.WriteLine($"Среднее: {Calculation.AverageResult}");

            var standardDeviation = Calculation.StdDevResult;
            Console.WriteLine($"Стандартное отклонение: {standardDeviation}");

            var mode = Calculation.MedianResult;
            Console.WriteLine($"Мода: {mode}");

            var median = Calculation.MedianResult;
            Console.WriteLine($"Медиана: {median}");

            Console.WriteLine($"Потерянных пакетов: {lostPacket}");

            writeCalculation = false;
            Console.WriteLine("Нажмите enter для получения данных");
        }
    }
}