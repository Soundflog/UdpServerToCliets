using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IPAddress serverIpAddress = IPAddress.Parse("127.0.0.1");
            int serverPort = 8000;

            TcpClient client = new TcpClient();
            client.Connect(serverIpAddress, serverPort);

            Console.WriteLine("Connected to server");

            // Receive file extension
            byte[] extensionData = new byte[10];
            using (NetworkStream stream = client.GetStream())
            {
                int bytesRead = stream.Read(extensionData, 0, extensionData.Length);
                string extension = System.Text.Encoding.UTF8.GetString(extensionData, 0, bytesRead);

                // Receive file data
                byte[] fileData = new byte[client.ReceiveBufferSize];
                bytesRead = stream.Read(fileData, 0, fileData.Length);

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
            }

            client.Close();
            Console.WriteLine("Disconnected from server");
        }
    }
}