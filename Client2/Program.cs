using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string serverIpAddress = "127.0.0.1";
            int serverPort = 5007;
            UdpClient client = new UdpClient();

            Console.WriteLine("Connecting to server {0}:{1}", serverIpAddress, serverPort);

            // Send request
            Console.WriteLine("Введите txt, html, jpg: ");
            byte[] requestData = System.Text.Encoding.ASCII.GetBytes(Console.ReadLine());
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIpAddress), serverPort);
            client.Send(requestData, requestData.Length, serverEndPoint);

            Console.WriteLine("Request sent to server");

            // Receive file extension
            byte[] extensionData = client.Receive(ref serverEndPoint);
            string extension = System.Text.Encoding.ASCII.GetString(extensionData);

            // Receive file data
            byte[] fileData = client.Receive(ref serverEndPoint);

            // Save file to disk
            string fileName = "file." + extension;
            File.WriteAllBytes(fileName, fileData);

            // Open file with default program
            switch (extension)
            {
                case "txt":
                    System.Diagnostics.Process.Start(fileName);
                    break;
                case "html":
                    System.Diagnostics.Process.Start(fileName);
                    break;
                case "jpg":
                    System.Diagnostics.Process.Start(fileName);
                    break;
            }

            client.Close();
            Console.WriteLine("Disconnected from server");
        }
    }
}