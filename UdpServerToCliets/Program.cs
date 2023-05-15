using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpServerToCliets
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8000;

            TcpListener listener = new TcpListener(ipAddress, port);
            listener.Start();
            Console.WriteLine("Server started");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");

                // Generate random object type
                Random random = new Random();
                int objectType = random.Next(1, 4);
                // int objectType = 3;
                // Generate file
                string extension = "";
                byte[] fileData = null;
                switch (objectType)
                {
                    case 1:
                        extension = "txt";
                        fileData = GenerateTextFile();
                        break;
                    case 2:
                        extension = "html";
                        fileData = GenerateHtmlFile();
                        break;
                    case 3:
                        extension = "jpg";
                        fileData = GenerateImageFile();
                        break;
                }

                // Send file to client
                using (NetworkStream stream = client.GetStream())
                {
                    // Send file extension
                    byte[] extensionData = Encoding.UTF8.GetBytes(extension);
                    stream.Write(extensionData, 0, extensionData.Length);
                    Console.WriteLine($"file.{extension} sent");
                    // Send file data
                    stream.Write(fileData, 0, fileData.Length);
                    // Console.WriteLine($"File: {Encoding.UTF8.GetString(fileData)} sent");
                }

                client.Close();
                Console.WriteLine("Client disconnected");
            }
        }

        static byte[] GenerateTextFile()
        {
            string text = "Hello, world!";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
            return data;
        }

        static byte[] GenerateHtmlFile()
        {
            string html = "<html><body><h1>Hello, world!</h1></body></html>";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(html);
            return data;
        }

        static byte[] GenerateImageFile()
        {
            
            byte[] data = File.ReadAllBytes("image.jpg");
            return data;
        }
    }
}
