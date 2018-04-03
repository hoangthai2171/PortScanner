using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Timers;

namespace CheckPortStatus
{
    class Program
    {


        static void Main(string[] args)
        {
            

            Console.Write("Nhap vào port muon check: ");
            int portno = int.Parse(Console.ReadLine());
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("126.0.0.1"), portno);

            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(ipe);
                Console.WriteLine("Port tcp open");
                tcpClient.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Port tcp closed");
            }

            UdpClient udpClient = new UdpClient();
            udpClient.Connect(ipe);
            byte[] data = Encoding.ASCII.GetBytes("Nubakachi");
            udpClient.Send(data, data.Length);

            var timer = TimeSpan.FromSeconds(10);
            var asyncResult = udpClient.BeginReceive(null, null);
            asyncResult.AsyncWaitHandle.WaitOne(timer);
            if (asyncResult.IsCompleted)
            {
                try
                {
                    udpClient.Receive(ref ipe);
                    Console.WriteLine("Port udp open");
                    udpClient.Close();
                }
                catch (Exception)
                {
                    Console.WriteLine("Port udp closed");
                }
            }
            else
            {
                Console.WriteLine("TIME OUT !");
            }

        }

    }
}