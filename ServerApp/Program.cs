using ServerApp;
using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MulticastApp
{
    class Program
    {
        static string ipAddress;
        static int port;

        static void Main(string[] args)
        {
            try
            {
                ipAddress = ConfigurationManager.AppSettings.Get("address");
                port = int.Parse(ConfigurationManager.AppSettings.Get("port"));

                UdpClient sender = new UdpClient();
                sender.Ttl = 1;
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                sender.Connect(endPoint);


                /* for (int i = 0; i < 20; i++)
                 {
                     var number = GeneratingNumbers.Generate();

                     byte[] data = Encoding.UTF8.GetBytes(number.ToString());

                     Console.WriteLine($"байт: {data.Length}");
                     Console.WriteLine($"число: {number}"); //для наглядности

                     sender.Send(data, data.Length);
                 }*/

                while (true)
                {
                    var number = GeneratingNumbers.Generate();
                    Console.WriteLine(number); //для наглядности

                    byte[] data = Encoding.UTF8.GetBytes(number.ToString());

                    sender.Send(data, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}