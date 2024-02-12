using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client_var7
{
    internal class Program
    {
        private const string ServerIpAddress = "26.129.50.40";
        private const int ServerPort = 8000;

        static void Main(string[] args)
        {
            while (true)
            {
                using (UdpClient client = new UdpClient())
                {
                    IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ServerIpAddress), ServerPort);
                    byte[] requestBytes = Encoding.ASCII.GetBytes("object1");

                    // Send request to server
                    client.Send(requestBytes, requestBytes.Length, serverEndPoint);

                    // Receive file extension from server
                    byte[] extensionBytes = client.Receive(ref serverEndPoint);
                    string extension = Encoding.ASCII.GetString(extensionBytes);

                    // Receive file data from server
                    byte[] fileData = client.Receive(ref serverEndPoint);

                    // Write file to disk
                    var clock_now = DateTime.Now.Date;
                    string filePath = $"file.{extension}";
                    File.WriteAllBytes(filePath, fileData);

                    // Open file with default program
                    switch (extension)
                    {
                        case "txt":
                            System.Diagnostics.Process.Start(filePath);
                            break;
                        case "html":
                            System.Diagnostics.Process.Start(filePath);
                            break;
                        case "jpg":
                            System.Diagnostics.Process.Start(filePath);
                            break;
                    }

                    Console.WriteLine($"Received and saved file {filePath}");
                }
            }
        }
    }
}